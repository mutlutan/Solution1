using WebApp.Panel.Codes;
using Microsoft.AspNetCore.Mvc;


namespace WebApp.Panel.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController(IServiceProvider _serviceProvider) : base(_serviceProvider) { }

        public IActionResult Index()
        {
            if (this.webHostEnvironment.EnvironmentName == "Development")
            {
                ViewBag.UserName = "admin";
                ViewBag.Password = "1";
            }

            ViewBag.UseAuthenticator =  this.business.GetParameter().UseAuthenticator;

            return View();
        }

    }
}
