using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using AppCommon;
using WebApp.Panel.Controllers;
using WebApp.Panel.Codes;
using AppData.Main.Models;
using Telerik.DataSource;
using Telerik.DataSource.Extensions;
using Newtonsoft.Json.Linq;

namespace WebApp.Panel.Controllers
{
	[Route("Panel/[controller]")]
	[ApiController]
	public class MapsController : BaseController
	{
		public MapsController(IServiceProvider _serviceProvider) : base(_serviceProvider) { }


		[HttpGet("ReadBolge")]
		[AuthenticateRequired]
		public ActionResult ReadBolge()
		{
			var response = this.business.ReadBolge();

			return Ok(response);
		}

		[HttpGet("ReadBolgeDetayList")]
		[AuthenticateRequired]
		public ActionResult ReadBolgeDetayList()
		{
			var response = this.business.ReadBolgeDetayList();

			return Ok(response);
		}

		[HttpGet("ReadSarjIstasyonuList")]
		[AuthenticateRequired]
		public ActionResult ReadSarjIstasyonuList()
		{
			var response = this.business.ReadSarjIstasyonuList();

			return Ok(response);
		}

		[HttpGet("ReadAracList")]
		[AuthenticateRequired]
		public ActionResult ReadAracList()
		{
			var response = this.business.ReadAracList();

			return Ok(response);
		}



	}
}


