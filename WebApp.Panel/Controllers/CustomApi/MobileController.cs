using AppCommon;
using AppCommon.Business;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using WebApp.Panel.Codes;

namespace WebApp.Panel.Controllers
{
	[Route("Panel/[controller]")]
    [ApiController]
    public class MobileController : BaseController
    {
        public MobileController(IServiceProvider _serviceProvider) : base(_serviceProvider) { }

        //mobile uygulama apileri buraya yazýlýr

        [HttpGet("ApiInfo")]
        public IActionResult ApiInfo()
        {
            return Json("Client App V.1.0");
        }


    }
}


