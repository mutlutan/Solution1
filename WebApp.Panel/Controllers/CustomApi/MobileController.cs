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
        /// Sms doðrulama yapmak için kullanýlan methodur.
        /// </summary>
        /// <returns> Token bilgileri döner </returns>
        [HttpPost("Login")]
        [ResponseCache(Duration = 0)]
        public IActionResult LoginSmsVerification([FromBody] MoLoginRequest request)
        {
            MoResponse<object> response = new();
            try
            {
                response = business.MemberLoginForToken(request);
            }
            catch (Exception ex)
            {
                response.Message.Add("Hata: " + ex.MyLastInner().Message);
                business.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return Json(response);
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
        /// Üyelik Sms doðrulama yapmak için kullanýlan methodur.
        /// </summary>
        /// <returns> Token bilgileri döner </returns>
        [HttpPost("RegisterSmsVerification")]
        [ResponseCache(Duration = 0)]
        public IActionResult RegisterSmsVerification([FromBody] MoMemberTokenRequest request)
        {
            MoResponse<object> response = new();
            try
            {
                response = business.MemberRegisterForToken(request);
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
        /// Üye fatura bilgilerini döner
        /// </summary>
        /// <returns> Token bilgileri döner </returns>
        [HttpPost("GetBillingInfo")]
        [ResponseCache(Duration = 0)]
        [AuthenticateRequired]
        public IActionResult GetBillingInfo()
        {
            MoResponse<object> response = new();
            try
            {
                response = business.MemberBillingInfo();
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
        /// Üyeye gönderilen doðrulama kodunun kontrolü yapýlýr.
        /// </summary>
        /// <returns> Token bilgileri döner </returns>
        [HttpPost("MemberSmsVerification")]
        [ResponseCache(Duration = 0)]
        [AuthenticateRequired]
        public IActionResult MemberSmsVerification([FromBody] MoMemberTokenRequest request)
        {
            MoResponse<object> response = new();
            try
            {
                response = business.MemberSmsVerification(request);
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
        /// Üyenin sürüþ hareketlerini getirir
        /// </summary>
        /// <returns> Sürüþ zamaný, tutarý ve tarihi döner </returns>
        [HttpPost("DrivingHistory")]
        [ResponseCache(Duration = 0)]
        [AuthenticateRequired]
        public IActionResult DriverHistory([FromBody] MoMemberDrivingHistoryRequest request)
        {
            MoResponse<object> response = new();
            try
            {
                response = business.MemberDrivingHistory(request);
            }
            catch (Exception ex)
            {
                response.Message.Add("Hata: " + ex.MyLastInner().Message);
                business.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return Json(response);
        }


        /// <summary>
        /// Müsait Araç ve Ýstasyon Bilgileri Döndürür
        /// </summary>
        /// <returns> </returns>
        [HttpPost("ListVehicleAndStation")]
        [ResponseCache(Duration = 0)]
        [AuthenticateRequired]
        public IActionResult ListVehicleAndStation([FromBody] MoLocationRequest request)
        {
            MoResponse<object> response = new();
            try
            {
                response = business.ListVehicleAndStation(request);
            }
            catch (Exception ex)
            {
                response.Message.Add("Hata: " + ex.MyLastInner().Message);
                business.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return Json(response);
        }

        /// <summary>
        /// Araç UniqId ye göre Araç detay bilgisi döner
        /// </summary>
        /// <returns> </returns>
        [HttpPost("GetVehicleDetail")]
        [ResponseCache(Duration = 0)]
        [AuthenticateRequired]
        public IActionResult GetVehicleDetail([FromBody] MoVehicleDetailRequest request)
        {
            MoResponse<object> response = new();
            try
            {
                response = business.VehicleDetailInfo(request);
            }
            catch (Exception ex)
            {
                response.Message.Add("Hata: " + ex.MyLastInner().Message);
                business.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return Json(response);
        }

        /// <summary>
        /// Üyenin kiraladýðý aracýn UniqIdne göre Þarj durumunu yüzde olarak döner
        /// </summary>
        /// <returns> </returns>
        [HttpPost("GetVehicleChargeRate")]
        [ResponseCache(Duration = 0)]
        [AuthenticateRequired]
        public IActionResult GetVehicleChargeRate([FromBody] MoVehicleDetailRequest request)
        {
            MoResponse<object> response = new();
            try
            {
                response = business.VehicleChargeRate(request);
            }
            catch (Exception ex)
            {
                response.Message.Add("Hata: " + ex.MyLastInner().Message);
                business.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return Json(response);
        }

        /// <summary>
        /// Üye QR kodunu okutarak kiralanacak aracýn müsaitlik durumu kontrol edilir
        /// </summary>
        /// <returns> </returns>
        [HttpPost("CheckVehicle")]
        [ResponseCache(Duration = 0)]
        [AuthenticateRequired]
        public IActionResult CheckVehicle([FromBody] MoVehicleQrCodeRequest request)
        {
            MoResponse<object> response = new();
            try
            {
                response = business.CheckVehicle(request);
            }
            catch (Exception ex)
            {
                response.Message.Add("Hata: " + ex.MyLastInner().Message);
                business.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return Json(response);
        }

        /// <summary>
        /// Üye QR kodunu okutarak kiralayacaðý aracýn kilidini açmasý
        /// </summary>
        /// <returns> </returns>
        [HttpPost("RentVehicle")]
        [ResponseCache(Duration = 0)]
        [AuthenticateRequired]
        public IActionResult RentVehicle([FromBody] MoVehicleDetailRequest request )
        {
            MoResponse<object> response = new();
            try
            {
                response = business.RentAndControlVehicle(request);
            }
            catch (Exception ex)
            {
                response.Message.Add("Hata: " + ex.MyLastInner().Message);
                business.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return Json(response);
        }

        /// <summary>
        /// Üye sürüþ iþlemini sonlandýrýr.
        /// </summary>
        /// <returns> </returns>
        [HttpPost("SetTripVehicleFile")]
        [ResponseCache(Duration = 0)]
        [AuthenticateRequired]
        public IActionResult SetTripVehicleFile([FromBody] MoVehicleFileRequest request)
        {
            MoResponse<object> response = new();
            try
            {
                response = business.SetTripVehicleFile(request);
            }
            catch (Exception ex)
            {
                response.Message.Add("Hata: " + ex.MyLastInner().Message);
                business.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return Json(response);
        }

        /// <summary>
        /// Sürüþ Tutarý bakiyeden düþülür ve bakiye yeterli deðilse yükleme için bilgi döner
        /// </summary>
        /// <returns> </returns>
        [HttpPost("PaymentForTrip")]
        [ResponseCache(Duration = 0)]
        [AuthenticateRequired]
        public IActionResult PaymentForTrip([FromBody] MoVehicleDetailRequest request)
        {
            MoResponse<object> response = new();
            try
            {
                response = business.PaymentForTrip(request);
            }
            catch (Exception ex)
            {
                response.Message.Add("Hata: " + ex.MyLastInner().Message);
                business.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return Json(response);
        }

        /// <summary>
        /// Aracýn kablo kilidini kapatýr
        /// </summary>
        /// <returns> </returns>
        [HttpPost("SetVehicleLockCableAndStop")]
        [ResponseCache(Duration = 0)]
        [AuthenticateRequired]
        public IActionResult SetVehicleLockCableAndStop([FromBody] MoVehicleDetailRequest request)
        {
            MoResponse<object> response = new();
            try
            {
                response = business.SetVehicleLockCableAndStop(request);
            }
            catch (Exception ex)
            {
                response.Message.Add("Hata: " + ex.MyLastInner().Message);
                business.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return Json(response);
        }

        /// <summary>
        /// Aracýn stop edilir ve sürüþ sonlandýrýlýr
        /// </summary>
        /// <returns> </returns>
        [HttpPost("SetStopVehicle")]
        [ResponseCache(Duration = 0)]
        [AuthenticateRequired]
        public IActionResult SetStopVehicle([FromBody] MoVehicleDetailRequest request)
        {
            MoResponse<object> response = new();
            try
            {
                response = business.SetStopVehicle(request);
            }
            catch (Exception ex)
            {
                response.Message.Add("Hata: " + ex.MyLastInner().Message);
                business.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return Json(response);
        }

        /// <summary>
        /// Araç Rezervasyonu Yapýlýr
        /// </summary>
        /// <returns> </returns>
        [HttpPost("CreateVehicleReservation")]
        [ResponseCache(Duration = 0)]
        [AuthenticateRequired]
        public IActionResult CreateVehicleReservation([FromBody] MoReservationRequest request)
        {
            MoResponse<object> response = new();
            try
            {
                response = business.ReservationVehicle(request);
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

        /// <summary>
        /// Masterpass ödeme-kart kayýt vb. iþlem baþlatmak için token bilgisi döner
        /// </summary>
        /// <returns> </returns>
        [HttpPost("GetMasterpassToken")]
        [ResponseCache(Duration = 0)]
        [AuthenticateRequired]
        public IActionResult GetMasterpassToken([FromBody] MasterPassRequest request)
        {
            MoResponse<object> response = new();
            try
            {
                response = business.GetAndCreateMasterpassToken(request);
            }
            catch (Exception ex)
            {
                response.Message.Add("Hata: " + ex.MyLastInner().Message);
                business.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return Json(response);
        }

        /// <summary>
        /// Firebase Notification için Device/User FcmRegistrationToken set eder
        /// </summary>
        /// <returns> </returns>
        [HttpPost("SetFcmRegistrationToken")]
        [ResponseCache(Duration = 0)]
        [AuthenticateRequired]
        public IActionResult SetFcmRegistrationToken([FromBody] MoFcmRegistrationTokenRequest request)
        {
            MoResponse<object> response = new();
            try
            {
                response = business.SetFcmRegistrationToken(request);
            }
            catch (Exception ex)
            {
                response.Message.Add("Hata: " + ex.MyLastInner().Message);
                business.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return Json(response);
        }

        /// <summary>
        /// Sürüþ state bilgisi döner
        /// </summary>
        /// <returns> Sürüþ zamaný, tutarý ve tarihi döner </returns>
        [HttpPost("GetMemberTripState")]
        [ResponseCache(Duration = 0)]
        [AuthenticateRequired]
        public IActionResult GetMemberTripState()
        {
            MoResponse<object> response = new();
            try
            {
                response = business.GetMemberTripState();
            }
            catch (Exception ex)
            {
                response.Message.Add("Hata: " + ex.MyLastInner().Message);
                business.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return Json(response);
        }

        /// <summary>
        /// Sürüþ State bilgisi Set Eder
        /// </summary>
        /// <returns> Sürüþ zamaný, tutarý ve tarihi döner </returns>
        [HttpPost("SetMemberTripState")]
        [ResponseCache(Duration = 0)]
        [AuthenticateRequired]
        public IActionResult SetMemberTripState([FromBody] MoMemberTripStateRequest request)
        {
            MoResponse<object> response = new();
            try
            {
                response = business.SetMemberTripState(request);
            }
            catch (Exception ex)
            {
                response.Message.Add("Hata: " + ex.MyLastInner().Message);
                business.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return Json(response);
        }

        /// <summary>
        ///Üyelik Sözleþme ve Kvkk Url link
        /// </summary>
        /// <returns> Sürüþ zamaný, tutarý ve tarihi döner </returns>
        [HttpPost("GetContractLinks")]
        [ResponseCache(Duration = 0)]
        public IActionResult GetContractLinks()
        {
            MoResponse<object> response = new();
            try
            {
                response = business.GetContractLinks();
            }
            catch (Exception ex)
            {
                response.Message.Add("Hata: " + ex.MyLastInner().Message);
                business.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return Json(response);
        }

        /// <summary>
        ///Kampanya listesini döner
        /// </summary>
        /// <returns> Sürüþ zamaný, tutarý ve tarihi döner </returns>
        [HttpPost("ListCampaign")]
        [ResponseCache(Duration = 0)]
        public IActionResult ListCampaign()
        {
            MoResponse<object> response = new();
            try
            {
                response = business.ListCampaign();
            }
            catch (Exception ex)
            {
                response.Message.Add("Hata: " + ex.MyLastInner().Message);
                business.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return Json(response);
        }

        /// <summary>
        ///Kampanya listesini döner
        /// </summary>
        /// <returns> Sürüþ zamaný, tutarý ve tarihi döner </returns>
        [HttpPost("GetCampaignDetail")]
        [ResponseCache(Duration = 0)]
        public IActionResult GetCampaignDetail([FromBody] MoCampaignRequest request)
        {
            MoResponse<object> response = new();
            try
            {
                response = business.GetCampaign(request);
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


