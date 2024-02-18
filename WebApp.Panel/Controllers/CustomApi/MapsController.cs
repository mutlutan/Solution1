using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using AppCommon;
using WebApp.Panel.Codes;
using AppCommon.Business;

namespace WebApp.Panel.Controllers
{
	[Route("Panel/[controller]")]
	[ApiController]
	public class MapsController : BaseController
	{
		public MapsController(IServiceProvider _serviceProvider) : base(_serviceProvider) { }

		[HttpGet("ReadCustomers")]
		[AuthenticateRequired]
		public ActionResult ReadCustomers()
		{
			var response = this.business.ReadCustomers();

			return Ok(response);
		}



	}
}


