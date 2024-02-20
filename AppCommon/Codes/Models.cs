using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Telerik.DataSource;

namespace AppCommon
{
    #region Temel modleller
    public class AppConfig :IDisposable
    {
        public string SelfHost { get; set; } = "";

        public string MainConnection { get; set; } = "";
        public string LogConnection { get; set; } = "";

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
	}

    public class MoResponse<T> where T : /*class,*/ new()
    {
        public bool Success { get; set; } = false;
        public List<string> Messages { get; set; } = new List<string>();
        public T? Data { get; set; }
        public int Total { get; set; } = 0;
    }

    public class MoLogin
    {
        public string Culture { get; set; } = "tr-TR";
        public string UserName { get; set; } = "";
        public string Password { get; set; } = "";
        public string GaCode { get; set; } = "";
    }

    public class MoTokenRequest
    {
        public string Culture { get; set; } = "";
        public string UserName { get; set; } = "";
        public string Password { get; set; } = "";
        //public string CaptchaCode { get; set; } = "";
        //public string CaptchaToken { get; set; } = "";
        public string GaCode { get; set; } = "";
    }

    public class MoTokenResponse
    {
        public string UserToken { get; set; } = "";
        public bool IsUserLogin { get; set; } = false;
        public bool IsGoogleSecretKey { get; set; } = false;
        public bool IsGoogleValidate { get; set; } = false;
        public bool IsPasswordDateValid { get; set; } = false;
    }

    public class MoCreateCaptchaResponse
    {
        public string CaptchaImage { get; set; } = "";
        public string CaptchaToken { get; set; } = "";
    }

    public class MoChangePasswordRequest
    {
        public string OldPassword { get; set; } = "";
        public string NewPassword { get; set; } = "";
        public string CaptchaCode { get; set; } = "";
        public string CaptchaToken { get; set; } = "";
    }

    public class MoResetPasswordRequest
    {
        public string Email { get; set; } = "";
        public string CaptchaCode { get; set; } = "";
        public string CaptchaToken { get; set; } = "";
    }

	public class MoCaptchaToken
	{
		public string Code { get; set; } = "";
	}


    //MoAccessToken
    public class MoAccessToken
	{
        public EnmClaimType ClaimType { get; set; }
        public string SessionGuid { get; set; } = "";
		public string Culture { get; set; } = "tr-TR";
		public EnmYetkiGrup YetkiGrup { get; set; }
		public int AccountId { get; set; }
		public string AccountName { get; set; } = "";
		public bool IsLogin { get; set; } = false;
		public bool IsGoogleSecretKey { get; set; } = false;
		public bool IsGoogleValidate { get; set; } = false;

		public string RoleIds { get; set; } = "";
		public string NameSurname { get; set; } = "";

		public bool IsPasswordDateValid { get; set; } = false;
	}


    #endregion

    #region Google Authenticator
    public class MoGoogleAuthenticatorSetupResponse
    {
        public string GaCode { get; set; } = "";
        public string GaSecretKey { get; set; } = "";
        public string GaImageUrl { get; set; } = "";
    }
    #endregion

    #region Authority models
    public class MoAuthority
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = "";

        [JsonPropertyName("text")]
        public string Text { get; set; } = "";

        [JsonPropertyName("hint")]
        public string Hint { get; set; } = "";

        [JsonPropertyName("rout")]
        public string Rout { get; set; } = "";

        [JsonPropertyName("params")]
        public string Params { get; set; } = "";

        [JsonPropertyName("showType")]
        public string ShowType { get; set; } = "";

        [JsonPropertyName("header")]
        public bool Header { get; set; }

        [JsonPropertyName("viewFolder")]
        public string ViewFolder { get; set; } = "";

        [JsonPropertyName("viewName")]
        public string ViewName { get; set; } = "";

        [JsonPropertyName("cssClass")]
        public string CssClass { get; set; } = "";

        [JsonPropertyName("expanded")]
        public bool Expanded { get; set; }

        //[JsonPropertyName("prefix")]
        //public bool Prefix { get; set; }

        [JsonPropertyName("menu")]
        public bool Menu { get; set; }

        //[JsonPropertyName("yetkiGrups")]
        //public string YetkiGrups { get; set; } = "";

        [JsonPropertyName("items")]
        public List<MoAuthority> Items { get; set; } = new();
    }


    #endregion

    #region Words for Translate
    public class MoWord
    {
        public string Key { get; set; } = "";
        public MoWordValue Value { get; set; } = new();
    }

    public class MoWordValue
    {
        public string Tr { get; set; } = "";
        public string En { get; set; } = "";
    }

    #endregion


    #region Lookup Object

    public class LookupFilters
    {
        public string Field { get; set; } = "";
        public string Operator { get; set; } = "";
        public string Value { get; set; } = "";
        //public string ValueType { get; set; } = "";
    }

    public class LookupSort
    {
        public string Field { get; set; } = "";
        public string Dir { get; set; } = "";
    }

    public class LookupRequest
    {
        public string TableName { get; set; } = "";
        public string ValueField { get; set; } = "";
        public string TextField { get; set; } = "";
        public string OtherFields { get; set; } = "";
        public List<LookupFilters> Filters { get; set; } = new List<LookupFilters>();
        public List<LookupSort> Sorts { get; set; } = new List<LookupSort>();
    }

    #endregion

    #region Gorseller model

    public class MyFile
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string Extension { get; set; } = "";
        public string FileUrl { get; set; } = "";
        public string FileViewUrl { get; set; } = "";
        public string FileVersion { get; set; } = "";
        public long Size { get; set; }
        public string SizeText { get; set; } = "";
        public DateTime ModifiedDate { get; set; }
    }

    #endregion

    #region ApiRequest models
    public class ApiFilter
    {
        public string Logic { get; set; } = "";
        public List<ApiFilterItem> Filters { get; set; } = new();
    }

    public class ApiFilterItem
    {
        public string Field { get; set; } = "";
        public string Operator { get; set; } = "";
        //public string Value { get; set; } = "";
        public object Value { get; set; } = "";
    }

    public class ApiSort
    {
        public string Field { get; set; } = "";
        public string Dir { get; set; } = "";
    }

    public class ApiRequest
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public List<ApiSort> Sort { get; set; } = new();
        public ApiFilter? Filter { get; set; }
    }

	#endregion

	#region sqlcommand model
	public class MoSql
    {
        public string SelectText { get; set; } = "";
        public string FromText { get; set; } = "";
        public string WhereText { get; set; } = "";
        public string OrderByText { get; set; } = "";
        public string PagingText { get; set; } = "";

        public string ToCommandText()
        {
            if (string.IsNullOrEmpty(OrderByText.MyToStr()))
            {
                OrderByText = "Order By 1";
            }

            return SelectText + " " + FromText + " " + WhereText + " " + OrderByText + " " + PagingText;
        }
        public string ToCommandTextNoPaging()
        {
            return SelectText + " " + FromText + " " + WhereText + " " + OrderByText;
        }

        //dsr.Data = this.defaultBusiness.repository.dataContext.RawQuery<User>(query.ToCommandText());
    }

    #endregion

    #region SaveRequest
    public class UserSaveRequest
    {
        public int Id { get; set; }
        public Guid UniqueId { get; set; }
        public bool IsActive { get; set; }
        public string UserName { get; set; } = "";
        public string UserPassword { get; set; } = "";
        public string NameSurname { get; set; } = "";
        public string IdentificationNumber { get; set; } = "";
        public string ResidenceAddress { get; set; } = "";
        public int RoleId { get; set; } = 0;
        public string Authorities { get; set; } = "";
        public bool IsEmailConfirmed { get; set; }

    }

    public class RoleSaveRequest
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public int LineNumber { get; set; }
        public string Name { get; set; } = "";
        public string Authority { get; set; } = "";
    }

    #endregion

}
