using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using WebApp.Panel.Controllers;
using WebApp.Panel.Codes;
using AppData.Main.Models;
using Telerik.DataSource;
using Telerik.DataSource.Extensions;
using Newtonsoft.Json.Linq;
using AppCommon;

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
			var response = new MoResponse<List<Dashboard>>();
			response = this.business.ReadDashboardList();

			return Ok(response);
		}

		[HttpGet("ReadDashboardData")]
		[AuthenticateRequired]
		public ActionResult ReadDashboardData(int id)
		{
			var response = new MoResponse<object>();
			response = this.business.ReadDashboardData(id);

			return Ok(response);
		}

	}
}


