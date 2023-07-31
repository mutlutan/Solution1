using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Globalization;
using WebApp1.Codes;
using WebApp1.Models;
using WebAppCodeGen.Models;

namespace WebApp1.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IServiceProvider serviceProvider;
        public IHttpContextAccessor? accessor;
        public IConfiguration? configuration;
        public string? connectionString;

        public BaseController(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.accessor = this.serviceProvider.GetService(typeof(IHttpContextAccessor)) as IHttpContextAccessor;
            this.configuration = this.serviceProvider.GetService(typeof(IConfiguration)) as IConfiguration;

            this.connectionString = this.configuration?["ConnectionStrings:MainConnection"];

        }

        public override void OnActionExecuting(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext context)
        {
            //ortak viewbag ler
            //ViewBag.ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            //ViewBag.ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
            ViewBag.Culture = "tr-TR";
            ViewBag.Language = CultureInfo.GetCultureInfo(ViewBag.Culture).Parent.IetfLanguageTag;
            ViewBag.LogoImageUrl = "/img/logo/logoYatay.png?v" + MyApp.Version;
            ViewBag.Title = "CodeGen";

            base.OnActionExecuting(context);
        }

    }
}