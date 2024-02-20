
using Microsoft.AspNetCore.Mvc;
using Shyjus.BrowserDetection;
using WebApi.Codes;
using Microsoft.Extensions.Options;
using AppCommon;
using AppCommon.Business;
using Microsoft.Extensions.Caching.Memory;
#nullable disable

namespace WebApp.Panel.Controllers
{
    public class BaseController : Controller
    {
        public IServiceProvider serviceProvider;
        public IWebHostEnvironment webHostEnvironment;
		public IHttpContextAccessor accessor;

        public AppConfig appConfig;
		public Business business;
        public CacheHelper cacheHelper;

        public BaseController(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.webHostEnvironment = this.serviceProvider.GetService<IWebHostEnvironment>();
            this.accessor = this.serviceProvider.GetService<IHttpContextAccessor>();
            this.appConfig = this.serviceProvider.GetService<IOptions<AppConfig>>().Value ?? new();

            this.business = this.serviceProvider.GetService<Business>();
			this.cacheHelper = new CacheHelper(this.business.dataContext, this.serviceProvider.GetService<IMemoryCache>());

			this.business.AllValidateToken(this.accessor.MyToToken());
            this.business.UserIp = this.accessor.MyToRemoteIpAddress();
            this.business.UserBrowser = this.serviceProvider.GetService<IBrowserDetector>()?.MyToUserBrowser();
            this.business.ContentRootPath = this.webHostEnvironment.ContentRootPath; 


			this.business.dataContext.AppDictionary = this.cacheHelper.GetDictionary(this.webHostEnvironment.WebRootPath);
            this.business.dataContext.UserId = this.business.UserToken.AccountId;
            this.business.dataContext.UserName = this.business.UserToken.AccountName;
            this.business.dataContext.Culture = new System.Globalization.CultureInfo(this.business.UserToken.Culture);
            this.business.dataContext.RefreshConnectionString();
		}

        public override void OnActionExecuting(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext context)
        {
            //ortak viewbag ler
            ViewBag.ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            ViewBag.ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
            ViewBag.Culture = "tr-TR";
            ViewBag.Language = System.Globalization.CultureInfo.GetCultureInfo(ViewBag.Culture).Parent.IetfLanguageTag;
            ViewBag.LoginImageUrl = "/img/account/login.png?v." + Codes.MyApp.Version;
            ViewBag.LogoImageUrl = "/img/logo/logo-yatay.png?v." + Codes.MyApp.Version;
            ViewBag.Title = Codes.MyApp.AppName;
            ViewBag.Version = Codes.MyApp.Version;

            var parameter = this.business.GetParameter(); //bu cache den gelmeli
            ViewBag.GoogleMapApiKey = parameter.GoogleMapApiKey;

            base.OnActionExecuting(context);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.appConfig.Dispose();
				this.cacheHelper.Dispose();
				this.business.Dispose();
            }

            base.Dispose(disposing);
        }

    }
}