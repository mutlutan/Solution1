using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;


namespace WebApp.Portal.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IServiceProvider _serviceProvider) : base(_serviceProvider) { }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }


    }
}