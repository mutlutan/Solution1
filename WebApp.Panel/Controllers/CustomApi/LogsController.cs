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
using AppCommon;

namespace WebApp.Panel.Controllers
{
    [Route("Panel/[controller]")]
    [ApiController]
    public class LogsController : BaseController
    {
        public LogsController(IServiceProvider _serviceProvider) : base(_serviceProvider) { }

        [HttpPost("ReadUserLog")]
        [AuthenticateRequired(AuthorityKeys = "UserLog.D_R.")]
        public ActionResult ReadUserLog([FromBody] ApiRequest request)
        {
            var response = this.business.ReadUserLog(this.business.UserToken, request);

            return Ok(response);
        }

    }
}


