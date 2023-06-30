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

        //mobile uygulama apileri buraya yaz�l�r

        [HttpGet("ApiInfo")]
        public IActionResult ApiInfo()
        {
            return Json("Client App V.1.0");
        }

        /// <summary>
        /// Sms do�rulama yapmak i�in kullan�lan methodur.
        /// </summary>
        /// <returns> Token bilgileri d�ner </returns>
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
        /// �yelik kay�t i�lemi yap�l�r.
        /// </summary>
        /// <returns> Token bilgileri d�ner </returns>
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
        /// �yelik Sms do�rulama yapmak i�in kullan�lan methodur.
        /// </summary>
        /// <returns> Token bilgileri d�ner </returns>
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
        /// �yelik kay�t i�lemi yap�l�r
        /// </summary>
        /// <returns> Token bilgileri d�ner </returns>
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
        /// �ifremi unuttum ve yeni �ifre olu�turmak i�in Sms do�rulama yapmak i�in kullan�lan methodur.
        /// </summary>
        /// <returns> Token bilgileri d�ner </returns>
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
        /// Sms veya mail kod do�rulamas� yap�ld�ktan sonra kullan�c� taraf�ndan olu�turulan parolay� set eder.
        /// </summary>
        /// <returns> Token bilgileri d�ner </returns>
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
        /// �ifre de�i�tirme methodudur
        /// </summary>
        /// <returns> Token bilgileri d�ner </returns>
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
        /// �ye bilgilerini d�ner
        /// </summary>
        /// <returns> Token bilgileri d�ner </returns>
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
        /// �ye c�zdan bilgilerini d�ner
        /// </summary>
        /// <returns> Token bilgileri d�ner </returns>
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
        /// �ye fatura bilgilerini d�ner
        /// </summary>
        /// <returns> Token bilgileri d�ner </returns>
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
        /// �ye hesab�n� iptal eder.
        /// </summary>
        /// <returns> Token bilgileri d�ner </returns>
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
        /// �ye sms do�rulama kodu g�nderilir.
        /// </summary>
        /// <returns> Token bilgileri d�ner </returns>
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
        /// �yeye g�nderilen do�rulama kodunun kontrol� yap�l�r.
        /// </summary>
        /// <returns> Token bilgileri d�ner </returns>
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
        /// �ye profile resmini kaydeden method. Base64
        /// </summary>
        /// <returns> Token bilgileri d�ner </returns>
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
        /// �yenin s�r�� hareketlerini getirir
        /// </summary>
        /// <returns> S�r�� zaman�, tutar� ve tarihi d�ner </returns>
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
        /// M�sait Ara� ve �stasyon Bilgileri D�nd�r�r
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
        /// Ara� UniqId ye g�re Ara� detay bilgisi d�ner
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
        /// �yenin kiralad��� arac�n UniqIdne g�re �arj durumunu y�zde olarak d�ner
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
        /// �ye QR kodunu okutarak kiralanacak arac�n m�saitlik durumu kontrol edilir
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
        /// �ye QR kodunu okutarak kiralayaca�� arac�n kilidini a�mas�
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
        /// �ye s�r�� i�lemini sonland�r�r.
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
        /// S�r�� Tutar� bakiyeden d���l�r ve bakiye yeterli de�ilse y�kleme i�in bilgi d�ner
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
        /// Arac�n kablo kilidini kapat�r
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
        /// Arac�n stop edilir ve s�r�� sonland�r�l�r
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
        /// Ara� Rezervasyonu Yap�l�r
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
        /// Parametre d�ner
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
        /// Masterpass �deme-kart kay�t vb. i�lem ba�latmak i�in token bilgisi d�ner
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
        /// Firebase Notification i�in Device/User FcmRegistrationToken set eder
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
        /// S�r�� state bilgisi d�ner
        /// </summary>
        /// <returns> S�r�� zaman�, tutar� ve tarihi d�ner </returns>
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
        /// S�r�� State bilgisi Set Eder
        /// </summary>
        /// <returns> S�r�� zaman�, tutar� ve tarihi d�ner </returns>
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
        ///�yelik S�zle�me ve Kvkk Url link
        /// </summary>
        /// <returns> S�r�� zaman�, tutar� ve tarihi d�ner </returns>
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
        ///Kampanya listesini d�ner
        /// </summary>
        /// <returns> S�r�� zaman�, tutar� ve tarihi d�ner </returns>
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
        ///Kampanya listesini d�ner
        /// </summary>
        /// <returns> S�r�� zaman�, tutar� ve tarihi d�ner </returns>
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


