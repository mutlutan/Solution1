using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebApp.Panel.Codes;
using AppCommon;

namespace WebApp.Panel.Controllers
{
    [Route("Panel/[controller]")]
    [ApiController]
    public class LogsController : BaseController
    {
        public LogsController(IServiceProvider _serviceProvider) : base(_serviceProvider) { }

        [HttpPost("ReadAccessLog")]
        [AuthenticateRequired(AuthorityKeys = "AccessLog.D_R.")]
        public ActionResult ReadAccessLog([FromBody] ApiRequest request)
        {
            var response = this.business.ReadUserLog(this.business.UserToken, request);

            return Ok(response);
        }

    }
}


