using AppCommon;
using Microsoft.Extensions.Primitives;
using Shyjus.BrowserDetection;

#nullable disable

namespace WebApp.Portal.Codes
{
    public static class Extensions
    {
        #region BaseController Extensions
        public static string MyToRemoteIpAddress(this HttpContext httpContext)
        {
            string rV = "";

            try
            {
                rV = httpContext?.Connection.RemoteIpAddress.MyToStr() ?? "";
            }
            catch { }

            return rV;
        }

        public static string MyToRemoteIpAddress(this IHttpContextAccessor _Accessor)
        {
            string rV = "";

            try
            {
                rV = _Accessor.HttpContext.MyToRemoteIpAddress();
            }
            catch { }

            return rV;
        }

        public static string MyToToken(this HttpContext httpContext)
        {
            string rV = "";

            try
            {
                string tokenCooki = null;
                StringValues tokenHeader = new();
                var request = httpContext?.Request;
                request?.Headers.TryGetValue("Authorization", out tokenHeader);
                request?.Cookies.TryGetValue("Authorization", out tokenCooki);

                rV = tokenCooki ?? tokenHeader.MyToStr();
            }
            catch { }

            return rV;
        }

        public static string MyToToken(this IHttpContextAccessor _Accessor)
        {
            string rV = "";

            try
            {
                rV = _Accessor.HttpContext.MyToToken();
            }
            catch { }

            return rV;
        }

        public static string MyToUserBrowser(this IBrowserDetector _detector)
        {
            string rV = "";

            try
            {
                if (_detector != null && _detector.Browser != null)
                {
                    var browser = _detector.Browser;
                    rV = browser.Name + " " + browser.Version + " " + browser.OS + " " + browser.DeviceType;
                }
            }
            catch { }

            return rV;
        }
        #endregion

    }

}
