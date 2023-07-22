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

#nullable disable

namespace AppCommon.Business
{

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

    public class AracApiRequestModel
    {
        public string MaptexApiKey { get; set; } = "06AF2D0E-4475-4262-AF39-48061DEC8D2A";
        public string MapTexBaseServiceUrl { get; set; } = "https://ffsapi.yourassetsonline.com:8446";
        public string ImeiNo { get; set; } = "";
        public string FilterType { get; set; } = ""; // EnmDijitalDataType, EnmAnalogDataType ve EnmTextDataType descriptionlarından gelir
        public string FilterRestrictTime { get; set; } = "update-since";
        public DateTime FilterTimeStamp { get; set; } = DateTime.Now.AddDays(-1);
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
 
    #endregion

    #region Mobile Service Modelleri
 
    public class MoMemberToken
    {
        public string Culture { get; set; } = "tr-TR";
        public int MemberId { get; set; }
        public string UniqueId { get; set; } = "";
        public string Email { get; set; } = "";
        public string NameSurname { get; set; } = "";

		public int MemberStatusId { get; set; }
        public int MemberGroupId { get; set; }
        public bool IsMemberLogin { get; set; } = false;

    }


    #endregion


}
