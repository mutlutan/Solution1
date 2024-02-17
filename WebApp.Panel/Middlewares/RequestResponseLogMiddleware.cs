using System;
using System.IO;
using System.Text;
using WebApi.Codes;
using Microsoft.Extensions.Options;
using Shyjus.BrowserDetection;
using Microsoft.Extensions.Caching.Memory;
using System.Reflection;
using AppCommon;
using AppCommon.Business;

namespace WebApp.Panel.Middlewares
{
    public class HttpLogMiddleware : IDisposable
    {
        private readonly RequestDelegate _next;
        private readonly IBrowserDetector _browserDetector;
        private readonly Business _business;

        public HttpLogMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
        {
            _next = next;
            using var scope = serviceProvider.CreateScope();
            _browserDetector = scope.ServiceProvider.GetService<IBrowserDetector>();
            _business = scope.ServiceProvider.GetRequiredService<Business>();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            DateTime requestDateTime = DateTime.Now;

            DateTime responseDateTime;
            string responseLogMessage = null;

            //First, get the incoming request
            string requestLogMessage = await RequestLogMessage(context.Request);

            bool isLogHttp = context.Request.Path.HasValue && context.Request.Path.Value == "/LogHttp/GetList";

            if (isLogHttp)
            {
                await _next(context);

                responseLogMessage = "Response was not saved on purpose to avoid huge amount data to be saved. To see its response, re-run the same http request which is in RequestRaw field.";
                responseDateTime = DateTime.Now;
            }
            else
            {
                //Copy a pointer to the original response body stream
                var originalBodyStream = context.Response.Body;

                //Create a new memory stream...
                using var responseBody = new MemoryStream();
                //...and use that for the temporary response body
                context.Response.Body = responseBody;

                //Continue down the Middleware pipeline, eventually returning to this class
                await _next(context);

                responseDateTime = DateTime.Now;
                //Format the response from the server
                responseLogMessage = await ResponseLogMessage(context.Response);

                //_logger.LogInformation(responseLogMessage);

                //Copy the contents of the new memory stream (which contains the response) to the original stream, which is then returned to the client.
                await responseBody.CopyToAsync(originalBodyStream);
            }

            _business.AllValidateToken(context.MyToToken());

            if (_business.MemberToken.AccountId > 0)
            {
                try
                {
                    _business.logDataContext.SystemLogAdd(new()
                    {
                        Id = Guid.NewGuid(),
                        UserId = _business.MemberToken.AccountId,
                        UserType = EnmYetkiGrup.Member.GetHashCode().ToString(),
                        UserName = _business.MemberToken.AccountName,
                        UserIp = context.MyToRemoteIpAddress(),
                        UserBrowser = _browserDetector.MyToUserBrowser(),
                        //UserSessionGuid = this._business.MemberToken.se,
                        ProcessTypeId = (int)EnmLogTur.Middleware,
                        ProcessDate = requestDateTime,
                        ProcessName = context.Request.Path.Value,
                        ProcessContent = requestLogMessage + Environment.NewLine + responseLogMessage,
                        ProcessingTime = responseDateTime - requestDateTime
                    });
                }
                catch (Exception ex)
                {
                    string text = $"{MethodBase.GetCurrentMethod()?.Name} = " + ex.ToString() + " StackTrace: " + ex.StackTrace;
                    _business.LogSaveForFile(EnmLogTur.Hata, text);
                }
            }
            else
            {
                try
                {
                    _business.logDataContext.SystemLogAdd(new()
                    {
                        Id = Guid.NewGuid(),
                        UserId = _business.UserToken.AccountId,
                        UserType = _business.UserToken.YetkiGrup.ToString(),
                        UserName = _business.UserToken.AccountName,
                        UserIp = context.MyToRemoteIpAddress(),
                        UserBrowser = _browserDetector.MyToUserBrowser(),
                        UserSessionGuid = _business.UserToken.SessionGuid,
                        ProcessTypeId = (int)EnmLogTur.Middleware,
                        ProcessDate = requestDateTime,
                        ProcessName = context.Request.Path.Value,
                        ProcessContent = requestLogMessage + Environment.NewLine + responseLogMessage,
                        ProcessingTime = responseDateTime - requestDateTime
                    });
                }
                catch (Exception ex)
                {
                    string text = $"{MethodBase.GetCurrentMethod()?.Name} = " + ex.ToString() + " StackTrace: " + ex.StackTrace;
                    _business.LogSaveForFile(EnmLogTur.Hata, text);
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
        private async Task<string> RequestLogMessage(HttpRequest request)
        {
            //Usually, the request body can be read only once. Here we are making use of the new EnableBuffering extension method to read the request body multiple times. 
            request.EnableBuffering();

            //This line allows us to set the reader for the request back at the beginning of its stream.
            //request.EnableRewind();

            //We now need to read the request stream.  First, we create a new byte[] with the same length as the request stream...
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];

            //...Then we copy the entire request stream into the new buffer.
            await request.Body.ReadAsync(buffer);

            //We convert the byte[] into a string using UTF8 encoding...
            var bodyAsText = Encoding.UTF8.GetString(buffer);

            //Reset the request body stream position so the next middleware can read it
            request.Body.Position = 0;

            var builder = new StringBuilder();
            builder.AppendLine($"{request.Method} {request.Scheme}://{request.Host.ToUriComponent()}{request.PathBase.ToUriComponent()}{request.Path.ToUriComponent()}{request.QueryString.ToUriComponent()}");
            builder.AppendLine("Request Details:");
            if (!string.IsNullOrEmpty(bodyAsText))
            {
                builder.AppendLine($"Body: {bodyAsText}");
            }
            builder.AppendLine("Request Header BEGINS");
            foreach (var header in request.Headers)
            {
                builder.Append(header.Key).Append(':').AppendLine(header.Value);
            }
            builder.AppendLine("Request Header ENDS");

            return builder.ToString();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
        private async Task<string> ResponseLogMessage(HttpResponse response)
        {
            //We need to read the response stream from the beginning...
            response.Body.Seek(0, SeekOrigin.Begin);

            //...and copy it into a string
            string responseBody = await new StreamReader(response.Body).ReadToEndAsync();

            //We need to reset the reader for the response so that the client can read it.
            response.Body.Seek(0, SeekOrigin.Begin);

            var builder = new StringBuilder();
            builder.AppendLine("Response Details:");
            builder.AppendLine($"Http Status: {response.StatusCode}");
            if (!string.IsNullOrEmpty(responseBody))
            {
                builder.AppendLine($"Body: {responseBody}");
            }
            if (response.Headers.Count > 0)
            {
                builder.AppendLine("Response Header BEGINS");
                foreach (var header in response.Headers)
                {
                    builder.Append(header.Key).Append(':').AppendLine(header.Value);
                }
                builder.AppendLine("Response Header ENDS");
            }
            return builder.ToString();
        }

        public void Dispose()
        {
            _business.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
