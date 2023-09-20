
using Microsoft.AspNetCore.Mvc;
using Shyjus.BrowserDetection;
using Microsoft.Extensions.Options;
using AppCommon;
using AppCommon.Business;

#nullable disable

namespace WebApp.Portal.Controllers
{
    public class BaseController : Controller
    {
        public IServiceProvider serviceProvider;
        public IHttpContextAccessor accessor;
        public AppConfig appConfig;
		public Business business;

        public BaseController(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.accessor = this.serviceProvider.GetService<IHttpContextAccessor>();
            this.appConfig = this.serviceProvider.GetService<IOptions<AppConfig>>().Value ?? new();
            this.business = this.serviceProvider.GetService<Business>();

            //this.business.AllValidateUserToken(this.accessor.MyToToken());
            //this.business.UserIp = this.accessor.MyToRemoteIpAddress();
            //this.business.UserBrowser = this.serviceProvider.GetService<IBrowserDetector>()?.MyToUserBrowser();

            //this.business.ContentRootPath = Codes.MyApp.Env.ContentRootPath;
            //this.business.repository.dataContext.AppDictionary = business.cacheHelper.GetDictionary(Codes.MyApp.Env?.WebRootPath);
            //this.business.repository.dataContext.UserId = this.business.UserToken.UserId;
            //this.business.repository.dataContext.UserName = this.business.UserToken.UserName;
            //this.business.repository.dataContext.Culture = new System.Globalization.CultureInfo(this.business.UserToken.Culture);
            //this.business.repository.dataContext.RefreshConnectionString();

        }

        public override void OnActionExecuting(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext context)
        {
            //ortak viewbag ler
            //ViewBag.ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            //ViewBag.ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
            ViewBag.Culture = "tr-TR";
            ViewBag.Language = System.Globalization.CultureInfo.GetCultureInfo(ViewBag.Culture).Parent.IetfLanguageTag;
            //ViewBag.LoginImageUrl = "/img/account/login.png?v." + Codes.MyApp.Version;
            //ViewBag.LogoImageUrl = "/img/logo/logo-yatay.png?v." + Codes.MyApp.Version;
            //ViewBag.Title = Codes.MyApp.AppName;
            //ViewBag.Version = Codes.MyApp.Version;

            var parameter = this.business.GetParameter(); //bu cache den gelmeli
            ViewBag.GoogleMapApiKey = parameter.GoogleMapApiKey;

            base.OnActionExecuting(context);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.business.Dispose();
            }

            base.Dispose(disposing);
        }

    }
}