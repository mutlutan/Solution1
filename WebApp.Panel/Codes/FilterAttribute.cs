using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using AppCommon;
using AppCommon.Business;

namespace WebApp.Panel.Codes
{
    public class AuthenticateRequiredAttribute : ActionFilterAttribute
	{
		public string AuthorityKeys { get; set; } = "";
		public string AuthorityGrups { get; set; } = "";
		public override void OnActionExecuting(ActionExecutingContext actionContext)
		{
			var svc = actionContext.HttpContext.RequestServices;
			var business = svc.GetService(typeof(Business)) as Business;
			var dataContext = business.dataContext;

			if (business.MemberToken.IsMemberLogin)
			{
				//müsait işine bakar
			}
			else if (business.UserToken.IsUserLogin && business.UserToken.IsGoogleValidate)
			{
				//AuthorityKeys veya AuthorityGrups dan biri doluysa
				if (AuthorityKeys.MyToTrim().Length > 0 || AuthorityGrups.MyToTrim().Length > 0)
				{
					//AuthorityKeys boş ise yetki key kontrol edilmeyecek anlamındadır
					Boolean keyGecerli = false;
					if (AuthorityKeys.MyToTrim().Length > 0)
					{
						//Key kontrolü yapılıyor

						foreach (string key in AuthorityKeys.MyToTrim().Split(","))
						{
							if (key.MyToTrim().Length > 0 && business.UserIsAuthorized(business.UserToken, key.MyToTrim()))
							{
								keyGecerli = true;
							}
						}
					}

					//AuthorityGrups boş ise yetki grup kontrol edilmeyecek anlamındadır
					Boolean grupGecerli = false;
					if (AuthorityGrups.MyToTrim().Length > 0)
					{
						//Grup kontrolü yapılıyor
						foreach (string grup in AuthorityGrups.MyToTrim().Split(","))
						{
							if (grup.MyToTrim().Length > 0)
							{
								if (grup.MyToTrim() == business.UserToken.YetkiGrup.MyToStr())
								{
									grupGecerli = true;
								}
							}
						}
					}

					// grup veya key den biri geçerli değilse mesaj
					if (!(grupGecerli || keyGecerli))
					{
						actionContext.Result = new BadRequestObjectResult(dataContext.TranslateTo("xLng.IslemIcinYetkiGerekli"));
					}
				}
			}
			else
			{
				var result = new ObjectResult("Token geçersiz!") { StatusCode = StatusCodes.Status401Unauthorized };
				actionContext.Result = result;
			}
		}

	}

	//public class MemberAuthenticateRequiredAttribute : ActionFilterAttribute
	//{
	//	public override void OnActionExecuting(ActionExecutingContext actionContext)
	//	{
	//		var svc = actionContext.HttpContext.RequestServices;
	//		var business = svc.GetService(typeof(Business)) as Business;
	//		var dataContext = business.dataContext;

	//		if (business.MemberToken.IsMemberLogin && business.MemberToken.IsSmsValidate)
	//		{
	//		    //müsait
	//           }
	//		else
	//		{
	//			var result = new ObjectResult("Token geçersiz!") { StatusCode = StatusCodes.Status401Unauthorized };
	//			actionContext.Result = result;
	//		}
	//	}

	//}

}
