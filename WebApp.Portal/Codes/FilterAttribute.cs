using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using AppCommon;
using AppCommon.Business;

#nullable disable

namespace WebApp.Portal.Codes
{
    public class AuthenticateRequiredAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            var svc = actionContext.HttpContext.RequestServices;
            var business = svc.GetService<Business>();

            if (business.MemberToken.IsLogin)
            {
                //müsait
            }
            else
            {
                var result = new ObjectResult("Token geçersiz!") { StatusCode = StatusCodes.Status401Unauthorized };
                actionContext.Result = result;
            }
        }

    }

}
