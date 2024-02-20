using AppCommon;
using AppCommon.Business;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace WebApp.Job.Controllers
{
    [ApiController]
	[Route("[controller]")]
	public class JobController : ControllerBase
	{
		public IServiceProvider serviceProvider;
		public IHttpContextAccessor accessor;
		public AppConfig appConfig;
		public Business business;

		public JobController(IServiceProvider serviceProvider)
		{
			this.serviceProvider = serviceProvider;
			this.accessor = this.serviceProvider.GetService<IHttpContextAccessor>();
			this.appConfig = this.serviceProvider.GetService<IOptions<AppConfig>>().Value ?? new();
			this.business = this.serviceProvider.GetService<Business>();
		}

		public decimal GetMemory()
		{
			return Convert.ToInt32(GC.GetTotalMemory(false) / (1024 * 1024));
		}

		#region arayüz oluþturucu metodlar
		public MoResponse<object> InfoHtml()
		{
			string link = "";
			link += "<meta charset=\"utf-8\">";
			link += "<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">";
			link += "<link href=\"https://cdn.jsdelivr.net/npm/bootstrap@5.2.3/dist/css/bootstrap.min.css\" rel=\"stylesheet\">";
			link += "<script src=\"https://cdn.jsdelivr.net/npm/bootstrap@5.2.3/dist/js/bootstrap.bundle.min.js\"></script>";
			link += "";

			string style = "";
			style += "html {color:black}";

			string script = "";
			script += "setInterval(function () {window.location.reload();}, 5000);";

			string body = "";
			body += "<h5>Job Api Info</h5>";
			body += "<p>Sunucu Aktif!</p>";
			body += "<p>Memory: " + this.GetMemory().ToString() + " MB </p>";

			string content = $"<html> <title>Process Api</title> <head>{link} <style>{style}</style> <script>{script}</script> </head> <body>{body}</body></html>";

			MoResponse<object> response = new()
			{
				Data = content,
				Success = true
			};

			return response;
		}

		#endregion

		[HttpGet("Info")]
		public IActionResult Info()
		{
			MoResponse<object> moResponse = new();
			try
			{
				moResponse = this.InfoHtml();
			}
			catch (Exception ex)
			{
				business.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
			}

			return new ContentResult()
			{
				Content = moResponse.Data?.ToString(),
				ContentType = "text/html"
			};
		}


		#region job statr stop
		[HttpGet("SetJobState")]
		public IActionResult SetJobState(string token, Boolean state)
		{
			MoResponse<Boolean> moResponse = new();

			if (token == "TOKEN-001-STATE")
			{
				MyJob.OnActive = state;
				moResponse.Success = true;
				moResponse.Messages.Add("Job State : " + state);
			}
			else
			{
				moResponse.Messages.Add("Token invalid!");
			}

			return Ok(moResponse);
		}

		#endregion

	}
}