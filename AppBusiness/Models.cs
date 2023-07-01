using AppCommon;
using NetTopologySuite.Geometries;
using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AppBusiness
{
#nullable disable

    #region Genel Api Request-Response Model
    //Base Servis Response-RequestModel
    public class HttpClientModel
    {
        public string ServiceBaseUrl { get; set; }
        public string ServiceMethodUrl { get; set; }
        public string ServiceSoapaActionUrl { get; set; }
        public HttpMethod MethodType { get; set; }
        public string ContentType { get; set; }
        public object Data { get; set; }
        public string Token { get; set; }
        public Dictionary<string, string> HeaderParams { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class HttpClientResponseModel
    {
        public string Result { get; set; }
        public bool IsSuccess { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
    #endregion

    #region Araç-İstasyon Api Modelleri Request-Response-Result
    public class ErrorResponse
    {
        [JsonPropertyName("errorCode")]
        public int ErrorCode { get; set; }

        [JsonPropertyName("errorDetails")]
        public string ErrorDetails { get; set; }
    }
    public class AracApiRequestModel
    {
        public string MaptexApiKey { get; set; } = "06AF2D0E-4475-4262-AF39-48061DEC8D2A";
        public string MapTexBaseServiceUrl { get; set; } = "https://ffsapi.yourassetsonline.com:8446";
        public string ImeiNo { get; set; } = "";
        public string FilterType { get; set; } = ""; // EnmDijitalDataType, EnmAnalogDataType ve EnmTextDataType descriptionlarından gelir
        public string FilterRestrictTime { get; set; } = "update-since";
        public DateTime FilterTimeStamp { get; set; } = DateTime.Now.AddDays(-1);
    }
    public class DucktApiRequestModel
    {
        public string DucktBaseServiceUrl { get; set; } = "https://api.duckt.app:8444";
        public string Token { get; set; } = "";
        public string VehicleId { get; set; } = "";
        public string StationId { get; set; } = "";
        public string UserName { get; set; } = "";
        public string Password { get; set; } = "";
        public string Param { get; set; } = "station=0"; //param=> station=stationId, port=portId, vehicle=vehicleId, user
    }

    // Araçların Model
    public class AracModel
    {
        [JsonPropertyName("IMEI")]
        public string IMEI { get; set; }

        [JsonPropertyName("rtcTimestamp")]
        public DateTime RtcTimestamp { get; set; }

        [JsonPropertyName("reportId")]
        public int ReportId { get; set; }

        [JsonPropertyName("gpsTimestamp")]
        public DateTime GpsTimestamp { get; set; }

        [JsonPropertyName("lat")]
        public double Lat { get; set; }

        [JsonPropertyName("lon")]
        public double Lon { get; set; }

        [JsonPropertyName("valid")]
        public bool Valid { get; set; }

        [JsonPropertyName("altitude")]
        public int Altitude { get; set; }

        [JsonPropertyName("heading")]
        public int Heading { get; set; }

        [JsonPropertyName("speedGps")]
        public double SpeedGps { get; set; }

        [JsonPropertyName("iotBattery")]
        public double IotBattery { get; set; }

        [JsonPropertyName("ignitionOn")]
        public bool IgnitionOn { get; set; }

        [JsonPropertyName("extendedDataType")]
        public string ExtendedDataType { get; set; }

        [JsonPropertyName("extendedData")]
        public AracExtendedData ExtendedData { get; set; }
    }
    public class AracExtendedData
    {
        [JsonPropertyName("scooterBattery")]
        public int ScooterBattery { get; set; }

        [JsonPropertyName("scooterCharging")]
        public bool ScooterCharging { get; set; }

        [JsonPropertyName("tamper")]
        public bool Tamper { get; set; }

        [JsonPropertyName("onSide")]
        public bool OnSide { get; set; }

        [JsonPropertyName("unauthorisedDriver")]
        public bool UnauthorisedDriver { get; set; }

        [JsonPropertyName("odometer")]
        public int Odometer { get; set; }

        [JsonPropertyName("tripDistance")]
        public int TripDistance { get; set; }

        [JsonPropertyName("maximumSpeed")]
        public int MaximumSpeed { get; set; }
    }
    public class AracDataResponse
    {
        [JsonPropertyName("IMEI")]
        public string IMEI { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonPropertyName("value")]
        public bool Value { get; set; }
    }
    public class AracInfo
    {
        [JsonPropertyName("lastContact")]
        public DateTime LastContact { get; set; }

        [JsonPropertyName("deviceManufacturer")]
        public string DeviceManufacturer { get; set; }

        [JsonPropertyName("deviceType")]
        public string DeviceType { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("smsNumber")]
        public string SmsNumber { get; set; }

        [JsonPropertyName("extendedProperties")]
        public List<AracExtendedProperty> ExtendedProperties { get; set; }
    }
    public class AracExtendedProperty
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("value")]
        public string Value { get; set; }
    }
    public class AracCloneRequest
    {
        [JsonPropertyName("IMEI")]
        public string IMEI { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("SmsNumber")]
        public string SmsNumber { get; set; }
    }
    public class AracHizLimitRequest
    {
        [JsonPropertyName("SpeedLimit")]
        public int SpeedLimit { get; set; }
    }
    public class AracTemelDonanimYazilimRequest
    {
        [JsonPropertyName("LoadCommand")]
        public string LoadCommand { get; set; }
    }
    #endregion

    #region Duckt Api Modelleri
    public class TokenResponseModel
    {
        [JsonPropertyName("jwt")]
        public string Jwt { get; set; }
    }
    public class RegisterVehicleRequestModel
    {
        [JsonPropertyName("adapter")]
        public string Adapter { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("vehicleType")]
        public string VehicleType { get; set; } //"SCOOTER" veya "BICYCLE"

        [JsonPropertyName("model")]
        public string Model { get; set; }

        [JsonPropertyName("callbackUrl")]
        public string CallbackUrl { get; set; }
    }
    public class RegisterVehicleResponseModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("model")]
        public string Model { get; set; }

        [JsonPropertyName("adapter")]
        public string Adapter { get; set; }

        [JsonPropertyName("callbackUrl")]
        public string CallbackUrl { get; set; }
    }
    public class StationRequestModel
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("lat")]
        public double Lat { get; set; }

        [JsonPropertyName("lng")]
        public double Lng { get; set; }
    }
    public class StationResponseModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("ducktName")]
        public string DucktName { get; set; }

        [JsonPropertyName("lat")]
        public int Lat { get; set; }

        [JsonPropertyName("lng")]
        public int Lng { get; set; }

        [JsonPropertyName("ports")]
        public List<Port> Ports { get; set; }
    }
    public class Port
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
    public class CallbackRequestModel
    {

        [JsonPropertyName("status")]
        public string Status { get; set; } //LOCKED - UNLOCKED

        [JsonPropertyName("station")]
        public string Station { get; set; }

        [JsonPropertyName("port")]
        public string Port { get; set; }

        [JsonPropertyName("lat")]
        public double Lat { get; set; }

        [JsonPropertyName("lng")]
        public double Lng { get; set; }

        [JsonPropertyName("vehicle")]
        public string Vehicle { get; set; }

        [JsonPropertyName("vehicleType")]
        public string VehicleType { get; set; } // SCOOTER - BICYCLE

        [JsonPropertyName("adapter")]
        public string Adapter { get; set; }

        [JsonPropertyName("time")]
        public DateTime Time { get; set; }

        [JsonPropertyName("cardId")]
        public string CardId { get; set; }
    }
    public class StatusResponseModel
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("station")]
        public string Station { get; set; }

        [JsonPropertyName("port")]
        public string Port { get; set; }

        [JsonPropertyName("lat")]
        public int Lat { get; set; }

        [JsonPropertyName("lng")]
        public int Lng { get; set; }

        [JsonPropertyName("vehicle")]
        public string Vehicle { get; set; }

        [JsonPropertyName("vehicleType")]
        public string VehicleType { get; set; }

        [JsonPropertyName("adapter")]
        public string Adapter { get; set; }

        [JsonPropertyName("time")]
        public DateTime Time { get; set; }

        [JsonPropertyName("cardId")]
        public string CardId { get; set; }
    }
    public class HealthResponseModel
    {
        [JsonPropertyName("station")]
        public string Station { get; set; }

        [JsonPropertyName("isOnline")]
        public bool IsOnline { get; set; }

        [JsonPropertyName("time")]
        public DateTime Time { get; set; }
    }
    public class VehicleUpdateRequestModel
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("vehicleType")]
        public string VehicleType { get; set; }

        [JsonPropertyName("model")]
        public string Model { get; set; }

        [JsonPropertyName("callbackUrl")]
        public string CallbackUrl { get; set; }
    }
    public class VehicleUpdateResponseModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("adapter")]
        public string Adapter { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("vehicleType")]
        public string VehicleType { get; set; }

        [JsonPropertyName("model")]
        public string Model { get; set; }

        [JsonPropertyName("callbackUrl")]
        public string CallbackUrl { get; set; }
    }

    #endregion

    #region Mobile Service Modelleri
    public class MoLoginRequest
    {
        [JsonPropertyName("culture")]
        public string Culture { get; set; } = "tr-TR";

        [JsonPropertyName("gsm")]
        public string Gsm { get; set; } = "";

        [JsonPropertyName("password")]
        public string Password { get; set; } = "";
    }

    public class MoMemberTokenRequest
    {
        [JsonPropertyName("smsVerificationCode")]
        public string SmsVerificationCode { get; set; } = "";
    }

    public class MoMemberToken
    {
        public string Culture { get; set; } = "tr-TR";
        public int MemberId { get; set; }
        public string UniqueId { get; set; } = "";
        public string Gsm { get; set; } = "";
        public string Email { get; set; } = "";
        public string NameSurname { get; set; } = "";
        public int MemberStatuId { get; set; }
        public int MemberGroupId { get; set; }
        public bool IsMemberLogin { get; set; } = false;

        public bool IsSmsValidate { get; set; } = false;
        public string SmsVerificationCode { get; set; } = "";
        public bool IsMsisdnValidated { get; set; }
    }

    public class MoMemberRegisterRequest
    {
        [JsonPropertyName("culture")]
        public string Culture { get; set; } = "tr-TR";

        [JsonPropertyName("gsm")]
        public string Gsm { get; set; } = "";

        [JsonPropertyName("email")]
        public string Email { get; set; } = "";

        [JsonPropertyName("identificationNo")]
        public string IdentificationNo { get; set; } = "";

        [JsonPropertyName("name")]
        public string Name { get; set; } = "";

        [JsonPropertyName("surname")]
        public string Surname { get; set; } = "";

        [JsonPropertyName("birthDate")]
        public DateTime BirthDate { get; set; }

        [JsonPropertyName("gender")]
        public int Gender { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; } = "";

        [JsonPropertyName("membershipAgreementConfirmation")]
        public bool MembershipAgreementConfirmation { get; set; } = false;

        [JsonPropertyName("approvalAgreementConfirmation")]
        public bool ApprovalAgreementConfirmation { get; set; } = false;

    }

    public class MoMemberForgotPasswordRequest
    {
        [JsonPropertyName("culture")]
        public string Culture { get; set; } = "tr-TR";

        [JsonPropertyName("email")]
        public string Email { get; set; } = "";
    }

    public class MoMemberNewPasswordRequest
    {
        [JsonPropertyName("newPassword")]
        public string NewPassword { get; set; } = "";
    }

    public class MoMemberChangePasswordRequest
    {
        [JsonPropertyName("oldPassword")]
        public string OldPassword { get; set; } = "";

        [JsonPropertyName("newPassword")]
        public string NewPassword { get; set; } = "";
    }

    public class MoMemberInfo
    {
        public string UniqueId { get; set; } = "";
        public int StatuId { get; set; }
        public string StatuName { get; set; } = "";
        public int GroupId { get; set; }
        public string GroupName { get; set; } = "";
        public string FcmTopicName { get; set; } = "";
        public string Gsm { get; set; } = "";
        public string Email { get; set; } = "";
        public string IdentificationNumber { get; set; } = "";
        public string NameSurname { get; set; } = "";
        public DateTime BirthDate { get; set; }
        public int GenderId { get; set; }
        public string Avatar { get; set; } = "";
        public string GenderName { get; set; } = "";
        public bool MemberVerification { get; set; }
        public bool ApprovalPrivacy { get; set; }
        public bool ApprovalMemberContract { get; set; }
        public bool IsMsisdnValidated { get; set; }

    }
    public class MoMemberAvatarRequest
    {
        [JsonPropertyName("profileImageBase64")]
        public string ProfileImageBase64 { get; set; } = "";
    }

    public class MoMemberWalletInfo
    {
        public decimal Balance { get; set; }
        public List<MemberWalletEvent> WalletEvents { get; set; }
    }
    public class MemberWalletEvent
    {
        public string Title { get; set; } = "";
        public int EventType { get; set; } // 1=giris, 2=çıkış, 3=iade
        public DateTime EventDate { get; set; }
        public decimal Ammount { get; set; }
    }

    public class MemberBillingInfo
    {
        public string Title { get; set; } = "";
        public string Url { get; set; }
        public DateTime BillingDate { get; set; }
        public decimal Ammount { get; set; }
    }

    public class MoMemberDrivingHistoryRequest
    {
        [JsonPropertyName("page")]
        public int Page { get; set; }

        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; }

        [JsonPropertyName("dateRange")]
        public int DateRange { get; set; }

    }
    public class MoMemberDrivingHistory
    {
        public string DrivingDate { get; set; } = "";
        public decimal Amount { get; set; }
        public int DrivingTime { get; set; }
        public string Vehicle { get; set; } = "";
    }

    public class MoLocationRequest
    {
        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }

        [JsonPropertyName("distance")]
        public int Distance { get; set; } //metre

    }

    public class MoRentVehicleRequest
    {
        [JsonPropertyName("uniqCode")]
        public string UniqCode { get; set; }
    }
    public class MoVehicleDetailRequest
    {
        [JsonPropertyName("uniqId")]
        public string UniqId { get; set; }
    }
    public class MoVehicleQrCodeRequest
    {
        [JsonPropertyName("qrCode")]
        public string QrCode { get; set; }
    }
    public class MoVehicle
    {
        public string Type { get; set; } = "";
        public string Name { get; set; } = "";
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string UniqId { get; set; } = "";
        public bool IsAvailable { get; set; }
        public int Distance { get; set; }//metre
        public Point Location { get; set; }
        public double ChargeRate { get; set; }
    }
    public class MoStation
    {
        public string Type { get; set; } = "";
        public string Name { get; set; } = "";
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string UniqId { get; set; } = "";
        public bool IsAvailable { get; set; }
        public int Distance { get; set; }//metre
        public Point Location { get; set; }
        public int VehicleCount { get; set; }
    }

    public class MoParameter
    {
        public int ReservationDuration { get; set; }
        public Int64 MasterpassMerchantId { get; set; }
    }

    public class MoReservationRequest
    {
        [JsonPropertyName("uniqId")]
        public string UniqId { get; set; }

        [JsonPropertyName("reservationDuration")]
        public int ReservationDuration { get; set; }
    }
    public class RezervasyonInfo
    {
        public string UniqId { get; set; }
        public string Vehicle { get; set; }
        public string Statu { get; set; }
        public DateTime ReservationDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class MoVehicleFileRequest
    {
        [JsonPropertyName("vehicleImageBase64")]
        public string VehicleImageBase64 { get; set; }

        [JsonPropertyName("uniqId")]
        public string UniqId { get; set; }
    }

    public class MoMemberTripStateRequest
    {
        [JsonPropertyName("uniqId")]
        public string UniqId { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }
    }

    #endregion

    #region Sms-MesajPaneli Models

    public class MesajPaneliData
    {
        public SmsUser user { get; set; }
        public string msgBaslik { get; set; }
        public bool tr { get; set; }
        public int start { get; set; }
        public List<MsgDatum> msgData { get; set; }
    }

    public class SmsUser
    {
        public string name { get; set; }
        public string pass { get; set; }
    }

    public class MsgDatum
    {
        public string msg { get; set; }
        public List<string> tel { get; set; }
    }

    public class MesajPaneliResponse
    {
        public bool status { get; set; }
        public int amount { get; set; }
        public string type { get; set; }
        public int credits { get; set; }
        public int referance { get; set; }
        public bool priv { get; set; }

    }
    #endregion

    #region Ödeme-Masterpass Modelleri
    public class Basket
    {
        [JsonPropertyName("productId")]
        public string productId { get; set; }

        [JsonPropertyName("basketType")]
        public string basketType { get; set; }

        [JsonPropertyName("productAmount")]
        public string productAmount { get; set; }
    }

    public class CustomTerminalInfo
    {
        [JsonPropertyName("icaNumber")]
        public string icaNumber { get; set; }

        [JsonPropertyName("currency")]
        public string currency { get; set; }

        [JsonPropertyName("posnetId")]
        public string posnetId { get; set; }

        [JsonPropertyName("acquirerMerchantId")]
        public string acquirerMerchantId { get; set; }

        [JsonPropertyName("acquirerTerminalId")]
        public string acquirerTerminalId { get; set; }

        [JsonPropertyName("acquirerMerchantEmail")]
        public string acquirerMerchantEmail { get; set; }

        [JsonPropertyName("terminalUserId")]
        public string terminalUserId { get; set; }

        [JsonPropertyName("terminalPassword")]
        public string terminalPassword { get; set; }

        [JsonPropertyName("provisionUserId")]
        public string provisionUserId { get; set; }

        [JsonPropertyName("storeKey")]
        public string storeKey { get; set; }

        [JsonPropertyName("refundPassword")]
        public string refundPassword { get; set; }

        [JsonPropertyName("integrationVersion")]
        public string integrationVersion { get; set; }
    }

    public class MasterPassRequest
    {
        [JsonPropertyName("merchantId")]
        public Int64 merchantId { get; set; }

        [JsonPropertyName("accountKey")]
        public string accountKey { get; set; }

        [JsonPropertyName("isMsisdnValidated")]
        public bool isMsisdnValidated { get; set; }

        [JsonPropertyName("userId")]
        public string userId { get; set; }

        [JsonPropertyName("authenticationMethod")]
        public string authenticationMethod { get; set; }

        [JsonPropertyName("secure3dType")]
        public string secure3dType { get; set; }

        [JsonPropertyName("bankIca")]
        public string bankIca { get; set; }

        [JsonPropertyName("isOtpValidated")]
        public bool isOtpValidated { get; set; }

        [JsonPropertyName("customTerminalInfo")]
        public CustomTerminalInfo customTerminalInfo { get; set; }

        [JsonPropertyName("issuerIca")]
        public int issuerIca { get; set; }

        [JsonPropertyName("isCustom")]
        public bool isCustom { get; set; }

        [JsonPropertyName("orderNo")]
        public string orderNo { get; set; }

        [JsonPropertyName("amount")]
        public string amount { get; set; }

        [JsonPropertyName("creditType")]
        public string creditType { get; set; }

        [JsonPropertyName("basket")]
        public List<Basket> basket { get; set; }

        [JsonPropertyName("campaingCode")]
        public string campaingCode { get; set; }

        [JsonPropertyName("currencyCode")]
        public string currencyCode { get; set; }

        [JsonPropertyName("transactionToken")]
        public string transactionToken { get; set; }
    }

    public class MasterPassToken
    {
        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; }
    }

    public class MasterPassError404
    {
        [JsonPropertyName("type")]
        public string type { get; set; }

        [JsonPropertyName("title")]
        public string title { get; set; }

        [JsonPropertyName("status")]
        public int status { get; set; }

        [JsonPropertyName("detail")]
        public string detail { get; set; }

        [JsonPropertyName("instance")]
        public string instance { get; set; }

        [JsonPropertyName("additionalProp1")]
        public string additionalProp1 { get; set; }

        [JsonPropertyName("additionalProp2")]
        public string additionalProp2 { get; set; }

        [JsonPropertyName("additionalProp3")]
        public string additionalProp3 { get; set; }
    }

    #endregion
    #region Firebase Token
    public class MoFcmRegistrationTokenRequest
    {
        [JsonPropertyName("fcmRegistrationToken")]
        public string FcmRegistrationToken { get; set; }
    }
    #endregion
    public class MobileAppState
    {
        public string VehicleUniqId { get; set; }
        public string State { get; set; }
    }
    public class MobileAppStateResponse
    {
        public string State { get; set; }
        public DateTime StartDate { get; set; }
        public decimal UnitPrice { get; set; }
        public double TripTime { get; set; }
        public string UniqueId { get; set; }
        public string QrCode { get; set; }
        public decimal ChargeRate { get; set; }
    }

    public class MoCampaignRequest
    {
        [JsonPropertyName("campaignId")]
        public int CampaignId { get; set; }
    }
    public class MoCampaingHead
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public string GorselUrl { get; set; }
        public DateTime? BaslangicTarih { get; set; }
        public DateTime? BitisTarih { get; set; }

    }
    public class MoCampaing 
    { 
        public int Id { get; set; }
        public int IndirimTurId { get; set; }
        public int TurId { get; set; }
        public string IndirimTurAd { get; set; }
        public string TurAd { get; set; }
        public string Ad { get; set; }
        public string GorselUrl { get; set; }
        public string Icerik { get; set; }
        public DateTime? BaslangicTarih { get; set; }
        public DateTime? BitisTarih { get; set; }
        public decimal IndirimDegeri { get; set; }

    }

}
