using AppBusiness;
using AppCommon;
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


        /// <summary>
        /// Üyelik kayýt iþlemi yapýlýr.
        /// </summary>
        /// <returns> Token bilgileri döner </returns>
        [HttpPost("Register")]
        [ResponseCache(Duration = 0)]
        public IActionResult Register([FromBody] MoMemberRegisterRequest request)
        {
            MoResponse<object> response = new();
            try
            {
                response = business.MemberRegister(request);
            }
            catch (Exception ex)
            {
                response.Message.Add("Hata: " + ex.MyLastInner().Message);
                business.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return Json(response);
        }




        /// <summary>
        /// Üyelik kayýt iþlemi yapýlýr
        /// </summary>
        /// <returns> Token bilgileri döner </returns>
        [HttpPost("ForgotPassword")]
        [ResponseCache(Duration = 0)]
        public IActionResult ForgotPassword([FromBody] MoMemberForgotPasswordRequest request)
        {
            MoResponse<object> response = new();
            try
            {
                response = business.MemberForgotPassword(request);
            }
            catch (Exception ex)
            {
                response.Message.Add("Hata: " + ex.MyLastInner().Message);
                business.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return Json(response);
        }

        /// <summary>
        /// Þifremi unuttum ve yeni þifre oluþturmak için Sms doðrulama yapmak için kullanýlan methodur.
        /// </summary>
        /// <returns> Token bilgileri döner </returns>
        [HttpPost("ForgotPasswordCodeVerification")]
        [ResponseCache(Duration = 0)]
        public IActionResult ForgotPasswordCodeVerification([FromBody] MoMemberTokenRequest request)
        {
            MoResponse<object> response = new();
            try
            {
                response = business.MemberForgotPasswordForToken(request);
            }
            catch (Exception ex)
            {
                response.Message.Add("Hata: " + ex.MyLastInner().Message);
                business.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return Json(response);
        }

        /// <summary>
        /// Sms veya mail kod doðrulamasý yapýldýktan sonra kullanýcý tarafýndan oluþturulan parolayý set eder.
        /// </summary>
        /// <returns> Token bilgileri döner </returns>
        [HttpPost("SetNewPassword")]
        [ResponseCache(Duration = 0)]
        public IActionResult SetNewPassword([FromBody] MoMemberNewPasswordRequest request)
        {
            MoResponse<object> response = new();
            try
            {
                response = business.MemberSetNewPassword(request);
            }
            catch (Exception ex)
            {
                response.Message.Add("Hata: " + ex.MyLastInner().Message);
                business.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return Json(response);
        }

        /// <summary>
        /// Þifre deðiþtirme methodudur
        /// </summary>
        /// <returns> Token bilgileri döner </returns>
        [HttpPost("ChangePassword")]
        [ResponseCache(Duration = 0)]
        [AuthenticateRequired]
        public IActionResult ChangePassword([FromBody] MoMemberChangePasswordRequest request)
        {
            MoResponse<object> response = new();
            try
            {
                response = business.MemberChangePassword(request);
            }
            catch (Exception ex)
            {
                response.Message.Add("Hata: " + ex.MyLastInner().Message);
                business.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return Json(response);
        }

        /// <summary>
        /// Üye bilgilerini döner
        /// </summary>
        /// <returns> Token bilgileri döner </returns>
        [HttpPost("GetMember")]
        [ResponseCache(Duration = 0)]
        [AuthenticateRequired]
        public IActionResult GetMember()
        {
            MoResponse<object> response = new();
            try
            {
                response = business.MemberGetInfo();
            }
            catch (Exception ex)
            {
                response.Message.Add("Hata: " + ex.MyLastInner().Message);
                business.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return Json(response);
        }

        /// <summary>
        /// Üye cüzdan bilgilerini döner
        /// </summary>
        /// <returns> Token bilgileri döner </returns>
        [HttpPost("GetWalletInfo")]
        [ResponseCache(Duration = 0)]
        [AuthenticateRequired]
        public IActionResult GetWalletInfo()
        {
            MoResponse<object> response = new();
            try
            {
                response = business.MemberWalletInfo();
            }
            catch (Exception ex)
            {
                response.Message.Add("Hata: " + ex.MyLastInner().Message);
                business.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return Json(response);
        }


        /// <summary>
        /// Üye hesabýný iptal eder.
        /// </summary>
        /// <returns> Token bilgileri döner </returns>
        [HttpPost("DeleteAccount")]
        [ResponseCache(Duration = 0)]
        [AuthenticateRequired]
        public IActionResult DeleteAccount()
        {
            MoResponse<object> response = new();
            try
            {
                response = business.MemberRemove();
            }
            catch (Exception ex)
            {
                response.Message.Add("Hata: " + ex.MyLastInner().Message);
                business.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return Json(response);
        }

        /// <summary>
        /// Üye sms doðrulama kodu gönderilir.
        /// </summary>
        /// <returns> Token bilgileri döner </returns>
        [HttpPost("MemberVerification")]
        [ResponseCache(Duration = 0)]
        [AuthenticateRequired]
        public IActionResult MemberVerification()
        {
            MoResponse<object> response = new();
            try
            {
                response = business.MemberVerification();
            }
            catch (Exception ex)
            {
                response.Message.Add("Hata: " + ex.MyLastInner().Message);
                business.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return Json(response);
        }


        /// <summary>
        /// Üye profile resmini kaydeden method. Base64
        /// </summary>
        /// <returns> Token bilgileri döner </returns>
        [HttpPost("SetProfileImage")]
        [ResponseCache(Duration = 0)]
        [AuthenticateRequired]
        public IActionResult SetProfileImage([FromBody] MoMemberAvatarRequest request)
        {
            MoResponse<object> response = new();
            try
            {
                response = business.MemberSetProfileImage(request);
            }
            catch (Exception ex)
            {
                response.Message.Add("Hata: " + ex.MyLastInner().Message);
                business.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return Json(response);
        }

        /// <summary>
        /// Parametre döner
        /// </summary>
        /// <returns> </returns>
        [HttpPost("GetParameter")]
        [ResponseCache(Duration = 0)]
        [AuthenticateRequired]
        public IActionResult GetParameter()
        {
            MoResponse<object> response = new();
            try
            {
                response = business.GetMobileParameter();
            }
            catch (Exception ex)
            {
                response.Message.Add("Hata: " + ex.MyLastInner().Message);
                business.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return Json(response);
        }

    }
}


