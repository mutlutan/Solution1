using System;
using Microsoft.AspNetCore.Mvc;
using WebApp.Panel.Codes;


namespace WebApp.Panel.Controllers
{
    [Route("Panel/[controller]")]
	[ApiController]
	public class DashboardController : BaseController
	{
		public DashboardController(IServiceProvider _serviceProvider) : base(_serviceProvider) { }


		[HttpGet("ReadDashboardList")]
		[AuthenticateRequired]
		public ActionResult ReadDashboardList()
		{
			var response = this.business.ReadDashboardList();

			return Ok(response);
		}

		[HttpGet("ReadDashboardData")]
		[AuthenticateRequired]
		public ActionResult ReadDashboardData(int id)
		{
			var response = this.business.ReadDashboardData(id);

			return Ok(response);
		}

	}
}


