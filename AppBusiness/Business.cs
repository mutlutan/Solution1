using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using Telerik.DataSource.Extensions;
using AppCommon;
using AppCommon.DataLayer.DataMain.Repository;
using AppCommon.DataLayer.DataMain.Models;
using AppCommon.DataLayer.DataLog.Models;

#nullable disable

namespace AppBusiness
{
	#region token models
	public class MoCaptchaToken
    {
        public string Code { get; set; } = "";
    }

    public class MoUserToken
    {
        public string SessionGuid { get; set; } = "";
        public string Culture { get; set; } = "tr-TR";
        public EnmYetkiGrup YetkiGrup { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = "";
        public bool IsUserLogin { get; set; } = false;
        public bool IsGoogleSecretKey { get; set; } = false;
        public bool IsGoogleValidate { get; set; } = false;

        public string RoleIds { get; set; }
        public string NameSurname { get; set; } = "";

        public bool IsPasswordDateValid { get; set; } = false;
    }
    #endregion

    public class Business : IDisposable
    {
        public readonly AppConfig appConfig;
        public readonly MainDataContext dataContext;
        public Repository repository;
        public MailHelper mailHelper;
        public CacheHelper cacheHelper;
        public MemoryCache memoryCache;

        public readonly LogDataContext logDataContext;

        public MoUserToken UserToken { get; set; } = new();

        public MoMemberToken MemberToken { get; set; } = new();

        public string AppName { get; set; } = "SmartBike Panel";
        public string ContentRootPath { get; set; }
        public string UserIp { get; set; }
        public string UserBrowser { get; set; }

		public string LogDirectory { get; set; } = "logs";

		public Business(MemoryCache memoryCache, AppConfig appConfig)
        {
            this.memoryCache = memoryCache;
            this.appConfig = appConfig;
            //default dataContext
            this.dataContext = new();
            this.dataContext.SetConnectionString(this.appConfig.MainConnection);
            this.repository = new Repository(dataContext);
            this.mailHelper = new MailHelper(dataContext);
            this.cacheHelper = new CacheHelper(this.dataContext, memoryCache);


            //log dataContext
            this.logDataContext = new();
            this.logDataContext.SetConnectionString(this.appConfig.LogConnection);
        }

        #region logs

        /// <summary>
        /// Kullanıcı giriş bilgisinin log kaydını tutuyoruz
        /// </summary>
        /// <param name="response"></param>
        /// <param name="input"></param>
        public void UserLogAdd(MoUserToken userToken)
        {
            try
            {
                this.logDataContext.UserLogAdd(new()
                {
                    Id = Guid.NewGuid(),
                    UserId = userToken.UserId,
                    UserName = userToken.UserName,
                    UserIp = this.UserIp,
                    UserSessionGuid = userToken.SessionGuid,
                    UserBrowser = this.UserBrowser.MyToMaxLength(250),
                    LoginDate = DateTime.Now,
                    LogoutDate = DateTime.Now,
                    EkAlan1 = userToken.Culture,
                    EkAlan2 = userToken.NameSurname,
                    EkAlan3 = null
                });
            }
            catch (Exception ex)
            {
                WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }
        }

        /// <summary>
        /// Kullanıcının son işlem zamanı çıkış bilgisi olarak güncelliyoruz(son çıkış zamanı gerçeğe en yakın çıkış zamanıdır)
        /// </summary>
        /// <param name="userToken"></param>
        public void UserLogSetLogoutDate()
        {
            if (this.UserToken.IsUserLogin && this.UserToken.IsGoogleValidate)
            {
                try
                {
                    this.logDataContext.UserLogSetLogoutDate(this.UserToken.SessionGuid);
                }
                catch (Exception ex)
                {
                    WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
                }
            }
        }

        private void LogSaveForDb(string processName, DateTime processStart, DateTime processEnd, EnmLogTur logTur, string icerik)
        {
            try
            {
                this.logDataContext.SystemLogAdd(new()
                {
                    Id = Guid.NewGuid(),
                    UserId = this.UserToken.UserId,
                    UserType = this.UserToken.YetkiGrup.ToString(),
                    UserName = this.UserToken.UserName,
                    UserIp = this.UserIp,
                    UserBrowser = this.UserBrowser,
                    UserSessionGuid = this.UserToken.SessionGuid,
                    ProcessTypeId = (int)logTur,
                    ProcessDate = processStart,
                    ProcessName = processName,
                    ProcessContent = icerik,
                    ProcessingTime = processEnd - processStart
                });
            }
            catch (Exception ex)
            {
                string text = $"{MethodBase.GetCurrentMethod()?.Name} = " + ex.ToString() + " StackTrace: " + ex.StackTrace;
                this.LogSaveForFile(EnmLogTur.Hata, text);
            }
        }

        static readonly object LogKilit = new();
        public void LogSaveForFile(EnmLogTur logTur, string icerik)
        {
            string strAy = DateTime.Today.ToString("yyyy.MM");
            string strGun = DateTime.Today.ToString("yyyy.MM.dd");

            try
            {
                if (!Directory.Exists(this.LogDirectory))
                {
                    Directory.CreateDirectory(this.LogDirectory);
                }

                lock (LogKilit)
                {
                    StreamWriter dosya = System.IO.File.AppendText(this.LogDirectory + "/log_" + strAy + ".txt");
                    dosya.WriteLine(logTur.ToString() + " : " + icerik);
                    dosya.Close();
                }
            }
            catch (Exception ex)
            {
                try
                {
                    lock (LogKilit)
                    {
                        StreamWriter dosya = System.IO.File.AppendText(this.LogDirectory + "/log_err_" + strGun + ".txt");
                        dosya.WriteLine("hata(" + DateTime.Now.ToString() + "):" + ex.Message);
                        dosya.Close();
                    }
                }
                catch { }
            }
        }

        public void WriteLog(string processName, DateTime processStart, DateTime processEnd, EnmLogTur logTur, string icerik)
        {
            this.LogSaveForDb(processName, processStart, processEnd, logTur, icerik); //dbye yazacağın zaman açarsın
                                                                                      // this.LogSaveForFile(logTur, icerik);
        }

        public void WriteLogForMethod(MethodBase method, EnmLogTur logTur, string text)
        {
            var sbParams = new System.Text.StringBuilder();
            foreach (var pi in method.GetParameters())
            {
                sbParams.Append(pi.Name + "=" + "??");
            }
            string icerik = $"{method.DeclaringType?.FullName}.{method.Name}({sbParams}) => log : " + text;
            this.WriteLog(method.Name, DateTime.Now, DateTime.Now, logTur, icerik);
        }

        public void WriteLogForMethodExceptionMessage(MethodBase method, Exception ex)
        {
            var sbParams = new System.Text.StringBuilder();
            foreach (var pi in method.GetParameters())
            {
                sbParams.Append(pi.Name + "=" + "??");
            }
            string icerik = $"{method.DeclaringType?.FullName}.{method.Name}({sbParams}) => hata : " + ex.ToString() + " StackTrace: " + ex.StackTrace;
            this.WriteLogForMethod(method, EnmLogTur.Hata, icerik);
        }

        public void WriteLogForRequestResponse(SystemLog systemLog)
        {
            try
            {
                this.logDataContext.SystemLogAdd(systemLog);
            }
            catch (Exception ex)
            {
                string text = $"{MethodBase.GetCurrentMethod()?.Name} = " + ex.ToString() + " StackTrace: " + ex.StackTrace;
                this.LogSaveForFile(EnmLogTur.Hata, text);
            }
        }

        #endregion

        #region token işlemleri
        public string JWTKey
        {
            get
            {
                return this.AppName.Replace(" ","")+ "-JWT-KEY-00000000000000001" + "-" + "v.01";
			}
        }

        #region JWT Token(Captcha Token; captcha verisi taşımada kullanılır, şifreli olmalıdır  )

        public bool ValidateCaptchaToken(string captchaCode, string captchaToken)
        {
            bool rV = false;
            try
            {
                var moCaptchaToken = new MoCaptchaToken();
                string authToken = captchaToken.MyToTrim().Trim().Replace("Bearer ", "");
                if (!string.IsNullOrEmpty(authToken))
                {
                    var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                    var validationParameters = new TokenValidationParameters()
                    {
                        ValidateAudience = false, // oluşturulan jetonda izleyici yok
                        ValidateIssuer = false,   // oluşturulan jetonda bir yayıncı yok
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(this.JWTKey)) // The same key as the one that generate the token
                    };

                    System.Security.Principal.IPrincipal principal = tokenHandler.ValidateToken(authToken, validationParameters, out SecurityToken validatedToken);

                    var data = ((System.Security.Claims.ClaimsIdentity)principal.Identity).Claims.Select(s => new { s.Type, s.Value });
                    string jsonText = data.Where(c => c.Type == "Data").FirstOrDefault()?.Value ?? "";
                    moCaptchaToken = System.Text.Json.JsonSerializer.Deserialize<MoCaptchaToken>(jsonText.MyToDecrypt(this.JWTKey));

                    rV = moCaptchaToken?.Code == captchaCode;
                }
            }
            catch (Exception ex)
            {
                WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return rV;
        }
        #endregion

        #region JWT Token(User Token; api için login olunduğunda oluşur )
        public string GenerateUserToken(EnmClaimType claimType, string jsonText)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.JWTKey));

            var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
                claims: new System.Security.Claims.Claim[]{
                    new System.Security.Claims.Claim(claimType.ToString(), jsonText.MyToBase64Str())
                },
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512),
                expires: DateTime.Now.AddDays(1)
            );

            return new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().WriteToken(token);
        }

        public T ValidateToken<T>(EnmClaimType claimType, string token) where T : class
        {
            var moToken = default(T);

            var moUserToken = new MoUserToken();
            var moMemberToken = new MoMemberToken();

            try
            {
                string authToken = token.MyToTrim().Replace("Bearer ", "");
                if (!string.IsNullOrEmpty(authToken))
                {
                    var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                    var validationParameters = new TokenValidationParameters()
                    {
                        ValidateAudience = false, // oluşturulan jetonda izleyici yok
                        ValidateIssuer = false,   // oluşturulan jetonda bir yayıncı yok
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.JWTKey)) // The same key as the one that generate the token
                    };

                    System.Security.Principal.IPrincipal principal = tokenHandler.ValidateToken(authToken, validationParameters, out SecurityToken validatedToken);

                    var data = ((System.Security.Claims.ClaimsIdentity)principal.Identity).Claims.Select(s => new { s.Type, s.Value });

                    var dataClaim = data.Where(c => c.Type == claimType.ToString()).FirstOrDefault();

                    if (claimType == EnmClaimType.User && dataClaim != null)
                    {
                        string jsonText = dataClaim.Value.MyFromBase64Str();
                        moUserToken = System.Text.Json.JsonSerializer.Deserialize<MoUserToken>(jsonText);

                        var user = this.dataContext.User
                            .Where(c => c.Id == moUserToken.UserId && c.IsActive == true)
                            .OrderByDescending(o => o.Id).FirstOrDefault();

                        if (user == null)
                        {
                            moUserToken.IsUserLogin = false;
                        }
                        else if (moUserToken.SessionGuid != user.SessionGuid)
                        {
                            moUserToken.IsUserLogin = false;
                        }
                    }

                    if (claimType == EnmClaimType.Member && dataClaim != null)
                    {
                        string jsonText = dataClaim.Value.MyFromBase64Str();
                        moMemberToken = System.Text.Json.JsonSerializer.Deserialize<MoMemberToken>(jsonText);

                        var member = this.dataContext.Uye
                            .Where(c => c.Id == moMemberToken.MemberId && c.UyeDurumId != (int)EnmUyeDurum.Pasif)
                            .OrderByDescending(o => o.Id).FirstOrDefault();

                        if (member == null)
                        {
                            moMemberToken.IsMemberLogin = false;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                //çok log yazarsa kapatırsın
                moUserToken = new MoUserToken();
                moMemberToken = new MoMemberToken();
                this.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            if (claimType == EnmClaimType.User)
            {
                moToken = moUserToken as T;
            }

            if (claimType == EnmClaimType.Member)
            {
                moToken = moMemberToken as T;
            }

            return moToken;
        }

        public void AllValidateUserToken(string token)
        {
            this.UserToken = this.ValidateToken<MoUserToken>(EnmClaimType.User, token);
            this.MemberToken = this.ValidateToken<MoMemberToken>(EnmClaimType.Member, token);
        }

        #endregion

        #endregion

        #region translate
        //public string TranslateToForPrms(string key, string lang, string[] prms)
        //{
        //    string rValue = key;
        //    var dictionary = this.cacheHelper.GetDictionary(this.ContentRootPath)
        //        .Where(c => c.Key == key).FirstOrDefault();
        //    if (dictionary != null)
        //    {
        //        rValue = dictionary.Value.Tr; //default tr value
        //        if (lang == "en")
        //        {
        //            rValue = dictionary.Value.En;
        //        }
        //    }

        //    try
        //    {
        //        if (prms != null && prms.Length > 0)
        //        {
        //            rValue = string.Format(rValue, prms);
        //        }
        //    }
        //    catch { }

        //    return rValue;
        //}

        //public string TranslateTo(string key, string lang)
        //{
        //    return this.TranslateToForPrms(key, lang, null);
        //}

        //public string TranslateTo(string key)
        //{
        //    return this.TranslateTo(key, this.UserToken?.Culture[..2] ?? "tr");
        //}
        #endregion

        #region parameterler
        public Parameter GetParameter()
        {
            Parameter rV = new();
            try
            {
                var data = dataContext.Parameter.AsNoTracking()
                    .Where(c => c.Id == 1).FirstOrDefault();

                if (data != null)
                {
                    rV = data;
                }
            }
            catch (Exception ex)
            {
                WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return rV;
        }

        #endregion

        #region Kullanıcı

        public User GetUser(int userId)
        {
            User rV = new();
            try
            {
                rV = this.repository.dataContext.User
                    .Where(c => c.Id == userId)
                    .FirstOrDefault() ?? new User();


            }
            catch (Exception ex)
            {
                WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return rV;
        }

        public List<string> GetUserAuthorities(int userId)
        {
            IEnumerable<string> rV = new List<string>();
            try
            {
                var kullanici = this.repository.dataContext.User
                    .Where(c => c.Id == userId).FirstOrDefault();

                if (kullanici != null)
                {
                    foreach (string rolId in kullanici.RoleIds.MyToStr().Split(','))
                    {
                        if (rolId.MyToTrim().Length > 0)
                        {
                            var rolRow = this.repository.dataContext.Role.Where(c => c.Id == Convert.ToInt32(rolId)).FirstOrDefault();
                            if (rolRow != null)
                            {
                                if (rolRow.Authority != null)
                                {
                                    rV = rV.Union(rolRow.Authority.Split(','));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return rV.ToList();
        }

        public bool UserIsAuthorized(MoUserToken userToken, string _Key)
        {
            bool rV = false;
            try
            {
                var userYetki = this.GetUserAuthorities(userToken.UserId);

                //admin vey a yönetici değil ise normal user ...
                if (userToken.YetkiGrup == EnmYetkiGrup.Admin)
                {
                    rV = true;
                }
                else
                {
                    if (userYetki != null && userYetki.Any())
                    {
                        var sonuc = userYetki.Where(c => c.Contains(_Key)).FirstOrDefault();
                        if (!string.IsNullOrEmpty(sonuc))
                        {
                            rV = sonuc.Length > 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return rV;
        }

        public MoResponse<MoTokenResponse> UserLoginForApi(MoTokenRequest input)
        {
            MoResponse<MoTokenResponse> response = new() { Data = new MoTokenResponse() };

            try
            {
                var userModel = this.dataContext.User
                    .Where(c => c.Id > 0)
                    .Where(c => c.UserName == input.UserName && c.UserPassword == input.Password.MyToEncryptPassword())
                    .FirstOrDefault();

                if (userModel != null)
                {
                    if (userModel.IsActive == true)
                    {
                        MoUserToken moUserToken = new()
                        {
                            SessionGuid = Guid.NewGuid().ToString(),
                            Culture = input.Culture,
                            UserId = userModel.Id,
                            UserName = userModel.UserName,
                            NameSurname = userModel.UserName,
                            RoleIds = userModel.RoleIds,
                            YetkiGrup = EnmYetkiGrup.Personel,
                            IsUserLogin = true,
                            IsPasswordDateValid = true
                        };

                        if (userModel.RoleIds.MyToStr().Split(',').Where(c => c.StartsWith("10")).FirstOrDefault() != null)
                        {
                            moUserToken.YetkiGrup = EnmYetkiGrup.Admin;
                        }

                        if (userModel.ValidityDate != null)
                        {
                            moUserToken.IsPasswordDateValid = DateTime.Now.Date <= userModel.ValidityDate;
                        }

                        userModel.SessionGuid = moUserToken.SessionGuid;

                        this.dataContext.SaveChanges();

                        #region Google Auth

                        if (!string.IsNullOrEmpty(userModel.GaSecretKey.MyToTrim()))
                        {
                            moUserToken.IsGoogleSecretKey = true;
                            var googleValidate = this.GoogleValidateTwoFactorPIN(userModel.GaSecretKey.MyToTrim(), input.GaCode);
                            if (googleValidate.Success)
                            {
                                moUserToken.IsGoogleValidate = true;
                            }
                            else
                            {
                                response.Message.Add(dataContext.TranslateTo("xLng.GaKoduGecersiz"));
                            }
                        }

                        response.Data.UserToken = this.GenerateUserToken(EnmClaimType.User, System.Text.Json.JsonSerializer.Serialize(moUserToken));
                        response.Data.IsUserLogin = moUserToken.IsUserLogin;
                        response.Data.IsGoogleSecretKey = moUserToken.IsGoogleSecretKey;
                        response.Data.IsGoogleValidate = moUserToken.IsGoogleValidate;
                        response.Data.IsPasswordDateValid = moUserToken.IsPasswordDateValid;

                        //response.Data.IsGoogleSecretKey = true; //GA'yı pas geçer
                        //response.Data.IsGoogleValidate = true; //GA'yı pas geçer

                        #endregion

                        #region user log
                        this.UserLogAdd(moUserToken);
                        #endregion

                        response.Success = true;
                    }
                    else
                    {
                        response.Message.Add(dataContext.TranslateTo("xLng.HesabinizAskiyaAlinmistir"));
                    }
                }
                else
                {
                    response.Message.Add(dataContext.TranslateTo("xLng.KullaniciveyaSifreGecersiz"));
                }

            }
            catch (Exception ex)
            {
                response.Message.Add(dataContext.TranslateTo("xLng.IstekBasarisizOldu"));
                WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return response;
        }

        public void UserLogout()
        {
            if (this.UserToken.IsUserLogin && this.UserToken.IsGoogleValidate)
            {
                try
                {
                    //user çıkış bilgisi güncelleniyor (son işlem zamanı)(bu asenkron olsun bekleme yapmasın)
                    // son yazılabilen çıkış zamanı gerçeğe en yakın çıkış zamanıdır, session end event gelince bunu düzeltirsin
                    var userLog = this.dataContext.User
                        .Where(c => c.SessionGuid == this.UserToken.SessionGuid)
                        .FirstOrDefault();

                    if (userLog != null)
                    {
                        userLog.SessionGuid = "";
                        this.dataContext.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
                }
            }
        }

        public MoResponse<object> ChangePassword(MoUserToken userToken, MoChangePasswordRequest request)
        {
            MoResponse<object> response = new();

            try
            {
                var user = dataContext.User
                     .Where(c => c.Id > 0 && c.Id == userToken.UserId)
                     .Where(c => c.UserPassword == request.OldPassword.MyToEncryptPassword())
                     .FirstOrDefault();

                if (user != null)
                {
                    if (request.NewPassword.Length >= 6)
                    {
                        user.UserPassword = request.NewPassword.MyToEncryptPassword();
                        user.ValidityDate = DateTime.Now.Date.AddYears(1);
                        dataContext.SaveChanges();
                        response.Success = true;
                        response.Message.Add(dataContext.TranslateTo("xLng.YeniSifrenizKayitEdildi"));
                    }
                    else
                    {
                        response.Message.Add(dataContext.TranslateTo("xLng.EnAzAltiKarekterBirHarfBirSayi"));
                    }
                }
                else
                {
                    response.Message.Add(dataContext.TranslateTo("xLng.EskiSifreGecersiz"));
                }

            }
            catch (Exception ex)
            {
                response.Message.Add(ex.MyLastInner().Message);
                WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return response;
        }

        public MoResponse<object> ResetPassword(string email)
        {
            MoResponse<object> response = new();

            try
            {
                var user = dataContext.User
                     .Where(c => c.Id > 0 && c.UserName == email)
                     .FirstOrDefault();

                if (user != null)
                {
                    var yeniSifre = Guid.NewGuid().ToString().MyToUpper().Replace("-", "")[..8];

                    var sonuc = mailHelper.SendMail_SifreBildirim(email, yeniSifre);
                    if (sonuc)
                    {
                        //var dtoKisi = repository.RepKisi.GetById(user.Id).FirstOrDefault();
                        //dtoKisi.SifreGecerlilikTarihi = DateTime.Now.Date.AddDays(-1);
                        //dtoKisi.Sifre = yeniSifre;
                        //repository.RepKisi.CreateOrUpdate(dtoKisi);
                        repository.dataContext.SaveChanges();

                        response.Success = true;
                        response.Message.Add(dataContext.TranslateTo("xLng.YeniSifreKayitliMailAdresineGonderildi"));
                    }
                    else
                    {
                        response.Message.Add(dataContext.TranslateTo("xLng.IsleminizYapilamadiDahaSonraTekrarDeneyebilirsiniz"));
                    }
                }
                else
                {
                    response.Message.Add(dataContext.TranslateTo("xLng.MailAdresiTaninlanmamis"));
                }

            }
            catch (Exception ex)
            {
                response.Message.Add(dataContext.TranslateTo("xLng.IsleminizYapilamadiDahaSonraTekrarDeneyebilirsiniz"));
                WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return response;
        }

        #region Yetkili olmayan fieldların çıkartılması
        public List<Newtonsoft.Json.Linq.JObject> GetAuthorityColumnsAndData(MoUserToken userToken, string tableName, IEnumerable<dynamic> data)
        {
            List<Newtonsoft.Json.Linq.JObject> jsonList = new();

            foreach (var item in data)
            {
                var jobj = Newtonsoft.Json.Linq.JObject.FromObject(item);

                foreach (var p in item.GetType().GetProperties())
                {
                    if (p.PropertyType.Name == p.Name || p.PropertyType.Name.StartsWith("ICollection"))
                    {
                        //forenkeyden gelen alan tanımının silinmesi
                        jobj.Property(p.Name)?.Remove();
                    }
                    else if (!this.UserIsAuthorized(userToken, $".{tableName}.D_R.{p.Name}."))
                    {
                        //yetkisiz alan kaydının silinmesi
                        jobj.Property(p.Name)?.Remove();
                    }
                }

                jsonList.Add(jobj);
            }

            return jsonList;
        }
        #endregion

        #endregion

        #region GA
        public MoResponse<Google.Authenticator.SetupCode> GaSetupCode()
        {
            var moResponse = new MoResponse<Google.Authenticator.SetupCode>();
            try
            {
                Google.Authenticator.TwoFactorAuthenticator tfa = new();
                moResponse.Data = tfa.GenerateSetupCode(this.AppName, this.AppName, this.UserToken.SessionGuid, false);
                moResponse.Success = true;

                var personeUser = this.dataContext.User.Where(c => c.Id == this.UserToken.UserId).FirstOrDefault();
                personeUser.GaSecretKey = this.UserToken.SessionGuid;
                this.dataContext.SaveChanges();
            }
            catch (Exception ex)
            {
                WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return moResponse;
        }

        public MoResponse<bool> GoogleValidateTwoFactorPIN(string accountSecretKey, string twoFactorCodeFromClient)
        {
            var moResponse = new MoResponse<bool>();
            try
            {
                Google.Authenticator.TwoFactorAuthenticator tfa = new();
                moResponse.Success = tfa.ValidateTwoFactorPIN(accountSecretKey, twoFactorCodeFromClient); //the width and height of the Qr Code in pixels
            }
            catch (Exception ex)
            {
                WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return moResponse;
        }
        #endregion

        #region lookup
        public IEnumerable<dynamic> GetLookupRead(string culture, LookupRequest lookupRequest)
        {
            //hangi tablo dan ise ondan çekilecek, (tablo içinde VeriDili alanı var ise içindeki ni dönecek hale getir)
            lookupRequest.TableName = lookupRequest.TableName.MyToStrAntiInjection();
            lookupRequest.ValueField = lookupRequest.ValueField.MyToStrAntiInjection();
            lookupRequest.OtherFields = lookupRequest.OtherFields.MyToStrAntiInjection();

            if(lookupRequest.TableName.ToLower() == "user")
            {
                if (lookupRequest.ValueField.ToLower() == "Password")
                {
                    lookupRequest.ValueField = "not used [password] field";
                }
            }

            string textFieldLine = string.Empty;
            if (!string.IsNullOrEmpty(lookupRequest.TextField))
            {
                foreach (string textField in lookupRequest.TextField.Split(","))
                {
                    if (!string.IsNullOrEmpty(textFieldLine))
                    {
                        textFieldLine += " + ' ' + ";
                    }
                    textFieldLine += textField.MyToStrAntiInjection();
                }
            }
            string sqlSelect = "Select " + lookupRequest.ValueField + " as value, " + textFieldLine + " as text";

            if (!string.IsNullOrEmpty(lookupRequest.OtherFields.MyToTrim()))
            {
                sqlSelect += ", " + lookupRequest.OtherFields;
            }

            string sqlFrom = " From " + lookupRequest.TableName;
            string sqlWhere = "";
            string sqlWhereLine = "";
            string sqlOrder = "";
            string sqlOrderLine = "";

            //filters
            if (lookupRequest.Filters != null)
            {
                foreach (var filterLine in lookupRequest.Filters)
                {
                    if (sqlWhereLine.Length > 0)
                    {
                        sqlWhereLine += " and ";
                    }

                    var valueType = dataContext.ColumnDataType(lookupRequest.TableName, filterLine.Field);

                    if (valueType == "int" | valueType == "decimal" | valueType == "float" | valueType == "bit")
                    {
                        sqlWhereLine += filterLine.Field + filterLine.Operator + filterLine.Value;
                    }

                    if (valueType == "char" | valueType == "varchar" | valueType == "nvarchar" | valueType == "date" | valueType == "time" | valueType == "datetime")
                    {
                        sqlWhereLine += filterLine.Field + filterLine.Operator + "'" + filterLine.Value + "'";
                    }
                }

                if (sqlWhereLine.Length > 0)
                {
                    sqlWhere += " Where " + sqlWhereLine;
                }
            }

            //sort
            if (lookupRequest.Sorts != null)
            {
                foreach (var sortLine in lookupRequest.Sorts)
                {
                    if (sqlOrderLine.Length > 0)
                    {
                        sqlOrderLine += ", ";
                    }

                    if (sortLine.Field.MyToTrim().Length > 0)
                    {
                        sqlOrderLine += sortLine.Field.MyToTrim() + " " + sortLine.Dir.MyToTrim();
                    }
                }

                if (sqlOrderLine.MyToTrim().Length > 0)
                {
                    sqlOrder = " Order By " + sqlOrderLine;
                }
            }

            #region veri dili işlemleri
            string[] tablesVeriDili = dataContext.GetVeriDiliTablolari();
            try
            {
                //veri dili verisi
                if (tablesVeriDili.Contains(lookupRequest.TableName))
                {
                    sqlSelect += ", VeriDili";
                }
            }
            catch { }

            #endregion

            var sqlCommand = sqlSelect + " " + sqlFrom + " " + sqlWhere + " " + sqlOrder;

            var queryResult = dataContext.RawQuery(sqlCommand);


            #region veri dili işlemleri

            try
            {
                //veri dili döngüsü
                if (tablesVeriDili.Contains(lookupRequest.TableName))
                {
                    foreach (var item in queryResult)
                    {
                        if (item.VeriDili != null)
                        {
                            string text = "";
                            foreach (string textField in lookupRequest.TextField.Split(","))
                            {
                                if (!string.IsNullOrEmpty(text))
                                {
                                    text += " + ' ' + ";
                                }
                                string itemVeriDili = item.VeriDili.ToString();
                                string itemText = item.text.ToString();
                                text += itemVeriDili.MyVeriDiliToStr(culture[..2], textField, itemText);

                            }
                            item.text = text;
                        }
                        item.VeriDili = "";
                    }
                }
            }
            catch { }

            #endregion

            return queryResult;

        }
		#endregion

		#region kendo filter
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
		public Telerik.DataSource.FilterOperator ToKendoFilterOperator(string filterOperator)
        {
            var rV = filterOperator.ToLower() switch
            {
                "eq" => Telerik.DataSource.FilterOperator.IsEqualTo,
                "neq" => Telerik.DataSource.FilterOperator.IsNotEqualTo,
                "isnull" => Telerik.DataSource.FilterOperator.IsNull,
                "isnotnull" => Telerik.DataSource.FilterOperator.IsNotNull,
                "lt" => Telerik.DataSource.FilterOperator.IsLessThan,
                "lte" => Telerik.DataSource.FilterOperator.IsLessThanOrEqualTo,
                "gt" => Telerik.DataSource.FilterOperator.IsGreaterThan,
                "gte" => Telerik.DataSource.FilterOperator.IsGreaterThanOrEqualTo,
                "startswith" => Telerik.DataSource.FilterOperator.StartsWith,
                "endswith" => Telerik.DataSource.FilterOperator.EndsWith,
                "contains" => Telerik.DataSource.FilterOperator.Contains,
                "doesnotcontain" => Telerik.DataSource.FilterOperator.DoesNotContain,
                "isempty" => Telerik.DataSource.FilterOperator.IsNullOrEmpty,
                "isnotempty" => Telerik.DataSource.FilterOperator.IsNotNullOrEmpty,
                _ => (Telerik.DataSource.FilterOperator)Enum.Parse(typeof(Telerik.DataSource.FilterOperator), filterOperator, true),
            };

            //kullanılabilecek filter operatorleri
            //"eq"(equal to) eşit
            //"neq"(not equal to) eşit değil
            //"isnull"(is equal to null) 
            //"isnotnull"(is not equal to null)
            //"lt"(less than) küçük
            //"lte"(less than or equal to) küçük veya eşit
            //"gt"(greater than) büyük
            //"gte"(greater than or equal to) büyük veya eşit
            //"startswith" başlayan
            //"endswith" biten
            //"contains" içinde geçen
            //"doesnotcontain" içinde geçmeyen
            //"isempty" boş
            //"isnotempty" boş değil

            return rV;
        }

        public Telerik.DataSource.DataSourceRequest ApiRequestToDataSourceRequest(ApiRequest request)
        {
            Telerik.DataSource.DataSourceRequest req = new()
            {
                Page = request.Page,
                PageSize = request.PageSize
            };

            if (request.Filter != null)
            {
                var compositeFilterDescriptor = new Telerik.DataSource.CompositeFilterDescriptor()
                {
                    LogicalOperator = Telerik.DataSource.FilterCompositionLogicalOperator.And
                };

                foreach (var filter in request.Filter.Filters)
                {
                    var filterOperator = this.ToKendoFilterOperator(filter.Operator);
                    var filterDescriptor = new Telerik.DataSource.FilterDescriptor(filter.Field, filterOperator, filter.Value);
                    compositeFilterDescriptor.FilterDescriptors.Add(filterDescriptor);
                }

                req.Filters = new List<Telerik.DataSource.IFilterDescriptor>() { };
                req.Filters.Add(compositeFilterDescriptor);
            }

            if (request.Sort != null)
            {
                req.Sorts = new List<Telerik.DataSource.SortDescriptor>();
                if (request.Sort != null)
                {
                    foreach (var sort in request.Sort)
                    {
                        Telerik.DataSource.ListSortDirection sortDirection = Telerik.DataSource.ListSortDirection.Ascending;
                        if (sort.Dir == "desc")
                        {
                            sortDirection = Telerik.DataSource.ListSortDirection.Descending;
                        }

                        req.Sorts.Add(new Telerik.DataSource.SortDescriptor()
                        {
                            Member = sort.Field,
                            SortDirection = sortDirection
                        });
                    }
                }
            }
            return req;
        }
        #endregion

        #region Logs reads
        public MoResponse<object> ReadUserLog(MoUserToken userToken, ApiRequest request)
        {
            MoResponse<object> response = new();

            try
            {
                var query = this.logDataContext.UserLog;

                var dsr = query.ToDataSourceResult(this.ApiRequestToDataSourceRequest(request));

                response.Total = dsr.Total;
                response.Data = this.GetAuthorityColumnsAndData(userToken, "UserLog", dsr.Data.Cast<dynamic>());
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message.Add(ex.MyLastInner().Message);
            }

            return response;
        }

        #endregion

        #region User functions
        public MoResponse<object> UserRead(MoUserToken userToken, ApiRequest request)
        {
            MoResponse<object> response = new();

            try
            {
                var query = this.dataContext.User
                    .Where(c => c.Id > 0)
                    .Select(s => new
                    {
                        s.Id,
                        s.UniqueId,
                        s.IsActive,
                        s.RoleIds,
                        s.UserName,
                        UserPassword = "",
                        s.IdentificationNumber,
                        s.ResidenceAddress,
                        s.NameSurname,
                        s.IsEmailConfirmed
                    });

                var dsr = query.ToDataSourceResult(this.ApiRequestToDataSourceRequest(request));

                response.Total = dsr.Total;
                response.Data = this.GetAuthorityColumnsAndData(userToken, "User", dsr.Data.Cast<dynamic>());
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message.Add(ex.MyLastInner().Message);
            }

            return response;
        }

        public MoResponse<object> UserSave(object obj)
        {
            MoResponse<object> response = new();
            Boolean isNew = false;

            try
            {
                var reqModel = JsonConvert.DeserializeObject<UserSaveRequest>(obj.MyToStr(), new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                var reqObj = JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(obj.ToString());

                User model = new();

                if (reqModel.Id == 0)
                {
                    //insertise
                    isNew = true;
                    model = this.repository.RepUser.GetByNew();
                }
                else
                {
                    //update ise
                    model = this.dataContext.User.FirstOrDefault(f => f.Id == reqModel.Id);
                    model.UserPassword = model.UserPassword.MyToDecryptPassword();
                }

                foreach (var p in model.GetType().GetProperties())
                {
                    if (reqObj[p.Name] != null)
                    {
                        var val = reqModel.GetType().GetProperty(p.Name).GetValue(reqModel);
                        p.SetValue(model, val);
                    }
                }

                reqModel.Id = this.repository.RepUser.CreateOrUpdate(model, isNew);

                #region diğer tablo insert ve update gereksinimleri, değerler null ise o alanlar hiç kullanılmadı anlamında
                /*
                //PersonUserPayMethodList
                if (reqModel.PersonUserPayMethodList != null)
                {
                    //listede yeni gelenleri eklenmesi
                    foreach (var payMethodId in reqModel.PersonUserPayMethodList)
                    {
                        var personUserPayMethod = this.dataContext.PersonUserPayMethod
                            .Where(c => c.PersonUserId == reqModel.Id && c.PayMethodId == payMethodId)
                            .FirstOrDefault();

                        if (personUserPayMethod == null)
                        {
                            personUserPayMethod = new PersonUserPayMethod
                            {
                                IsActive = true,
                                CreateDate = DateTime.Now,
                                PersonUserId = reqModel.Id,
                                PayMethodId = payMethodId
                            };
                        }
                        else
                        {
                            personUserPayMethod.IsActive = true;
                        }
                        int personUserPayMethodId = this.repository.RepPersonUserPayMethod.CreateOrUpdate(personUserPayMethod);
                    }

                    //listede olmayıp tabloda olanların silinmesi, pasif edilmesi
                    if (reqObj["PersonUserPayMethodList"] != null)
                    {
                        var silinecekPersonUserPayMethodList = this.dataContext.PersonUserPayMethod
                            .Where(c => c.PersonUserId == reqModel.Id && !reqModel.PersonUserPayMethodList.Contains(c.PayMethodId))
                            .ToList();

                        foreach (var personUserPayMethod in silinecekPersonUserPayMethodList)
                        {
                            //silinmesi
                            this.repository.RepPersonUserPayMethod.Delete(personUserPayMethod.Id);
                            //pasif edilmesi
                            //personUserPayMethod.IsActive = false;
                        }
                    }
                }

                //PersonUserTraderList 
                if (reqModel.PersonUserTraderList != null)
                {
                    //listede yeni gelenleri eklenmesi
                    foreach (var traderId in reqModel.PersonUserTraderList)
                    {
                        var personUserTrader = this.dataContext.PersonUserTrader
                            .Where(c => c.PersonUserId == reqModel.Id && c.TraderId == traderId)
                            .FirstOrDefault();

                        if (personUserTrader == null)
                        {
                            personUserTrader = new PersonUserTrader
                            {
                                IsActive = true,
                                CreateDate = DateTime.Now,
                                PersonUserId = reqModel.Id,
                                TraderId = traderId
                            };
                        }
                        else
                        {
                            personUserTrader.IsActive = true;
                        }
                        int personUserTraderId = this.repository.RepPersonUserTrader.CreateOrUpdate(personUserTrader);
                    }

                    //listede olmayı tabloda olanların silinmesi
                    if (reqObj["PersonUserTraderList"] != null)
                    {
                        var silinecekPersonUserTraderList = this.dataContext.PersonUserTrader
                            .Where(c => c.PersonUserId == reqModel.Id && !reqModel.PersonUserTraderList.Contains(c.TraderId))
                            .ToList();

                        foreach (var personUserTrader in silinecekPersonUserTraderList)
                        {
                            this.repository.RepPersonUserTrader.Delete(personUserTrader.Id);
                        }
                    }
                }
                */
                #endregion

                this.repository.SaveChanges();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message.Add(ex.MyLastInner().Message);
            }

            return response;
        }

        public MoResponse<object> UserDelete(object obj)
        {
            MoResponse<object> response = new();
            try
            {
                var model = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(obj.MyToStr());
                if (model != null)
                {
                    var row = this.dataContext.User
                        .Where(c => c.Id == model.Id && c.UniqueId == model.UniqueId).FirstOrDefault();
                    if (row != null)
                    {
                        this.repository.RepUser.Delete(model.Id);
                        this.repository.SaveChanges();
                        response.Success = true;
                    }
                    else
                    {
                        response.Message.Add(this.repository.dataContext.TranslateTo("xLng.IslemYapilacakKayitBulunamadi"));
                    }
                }
                else
                {
                    response.Message.Add(this.repository.dataContext.TranslateTo("xLng.VeriModeliDonusturulemedi"));
                }

            }
            catch (Exception ex)
            {
                if (ex.Source == "Microsoft.EntityFrameworkCore.Relational")
                {
                    response.Message.Add(this.repository.dataContext.TranslateTo("xLng.BaglantiliKayitVarSilinemez"));
                }
                else
                {
                    response.Message.Add(ex.MyLastInner().Message);
                }
            }
            return response;
        }
        #endregion

        #region Role functions
        public MoResponse<object> RoleRead(MoUserToken userToken, ApiRequest request)
        {
            MoResponse<object> response = new();

            try
            {
                var query = this.dataContext.Role
                    .Where(c => c.Id > 0)
                    .Select(s => new
                    {
                        s.Id,
                        s.IsActive,
                        s.LineNumber,
                        s.Name,
                        s.Authority,
                        s.CreateDate
                    });

                var dsr = query.ToDataSourceResult(this.ApiRequestToDataSourceRequest(request));

                response.Total = dsr.Total;
                response.Data = this.GetAuthorityColumnsAndData(userToken, "Role", dsr.Data.Cast<dynamic>());
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message.Add(ex.MyLastInner().Message);
            }

            return response;
        }

        public MoResponse<object> RoleSave(object obj)
        {
            MoResponse<object> response = new();
            Boolean isNew = false;
            try
            {
                var reqModel = JsonConvert.DeserializeObject<RoleSaveRequest>(obj.MyToStr(), new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                var reqObj = JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(obj.ToString());

                Role model = new();

                if (reqModel.Id == 0)
                {
                    //insertise
                    isNew = true;
                    model = this.repository.RepRole.GetByNew();
                }
                else
                {
                    //update ise
                    model = this.dataContext.Role.FirstOrDefault(f => f.Id == reqModel.Id);
                }

                foreach (var p in model.GetType().GetProperties())
                {
                    if (reqObj[p.Name] != null)
                    {
                        var val = reqModel.GetType().GetProperty(p.Name).GetValue(reqModel);
                        p.SetValue(model, val);
                    }
                }

                int id = this.repository.RepRole.CreateOrUpdate(model, isNew);

                #region diğer tablo insert ve update gereksinimleri, değerler null ise o alanlar hiç kullanılmadı anlamında
                #endregion

                this.repository.SaveChanges();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message.Add(ex.MyLastInner().Message);
            }

            return response;
        }

        public MoResponse<object> RoleDelete(object obj)
        {
            MoResponse<object> response = new();
            try
            {
                var model = Newtonsoft.Json.JsonConvert.DeserializeObject<Role>(obj.MyToStr());
                if (model != null)
                {
                    var row = this.dataContext.Role
                        .Where(c => c.Id == model.Id).FirstOrDefault();
                    if (row != null)
                    {
                        this.repository.RepRole.Delete(model.Id);
                        this.repository.SaveChanges();
                        response.Success = true;
                    }
                    else
                    {
                        response.Message.Add(this.repository.dataContext.TranslateTo("xLng.IslemYapilacakKayitBulunamadi"));
                    }
                }
                else
                {
                    response.Message.Add(this.repository.dataContext.TranslateTo("xLng.VeriModeliDonusturulemedi"));
                }

            }
            catch (Exception ex)
            {
                if (ex.Source == "Microsoft.EntityFrameworkCore.Relational")
                {
                    response.Message.Add(this.repository.dataContext.TranslateTo("xLng.BaglantiliKayitVarSilinemez"));
                }
                else
                {
                    response.Message.Add(ex.MyLastInner().Message);
                }
            }
            return response;
        }
        #endregion

        #region API İşlemleri

        #region Base API Request-Response Class. Get, Post, Put
        /// <summary>
        /// Genel Servis Methodu. Post-Get Request-Response
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        //Base Service Method
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
        public async Task<HttpClientResponseModel> GetOrPostRestService(HttpClientModel request)
        {
            HttpClientResponseModel result = new();
            if (request != null)
            {
                HttpClient httpClient = new()
                {
                    BaseAddress = new Uri(request.ServiceBaseUrl)
                };
                httpClient.DefaultRequestHeaders.Accept.Clear();

                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(request.ContentType));

                //Token varsa Headera eklenir
                if (!string.IsNullOrEmpty(request.Token))
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", request.Token);
                }

                // Header parametreleri ekleniyor
                if (request.HeaderParams != null)
                {
                    foreach (var prm in request.HeaderParams)
                    {
                        httpClient.DefaultRequestHeaders.Add(prm.Key, prm.Value);
                    }
                }

                // Body için Data ekleniyor                
                HttpContent content = null;
                if (request.Data != null)
                {
                    if (request.ContentType == "application/json")
                    {
                        content = new System.Net.Http.StringContent(System.Text.Json.JsonSerializer.Serialize(request.Data), Encoding.UTF8, request.ContentType);
                    }
                    else if (request.ContentType == "application/x-www-form-urlencoded")
                    {
                        content = new FormUrlEncodedContent(
                            new List<KeyValuePair<string, string>> {
                                new KeyValuePair<string, string>("data", request.Data.ToString())
                        });
                    }
                }

                // Get, Post işlemi ile servise bağlanılıyor
                if (request.MethodType == HttpMethod.Post)
                {

                    var response = await httpClient.PostAsync(request.ServiceMethodUrl, request.Data != null ? content : null);
                    result.IsSuccess = response.IsSuccessStatusCode;
                    result.StatusCode = response.StatusCode;
                    result.Result = response.Content.ReadAsStringAsync().Result;
                }
                else if (request.MethodType == HttpMethod.Get)
                {
                    var response = await httpClient.GetAsync(request.ServiceMethodUrl);
                    result.IsSuccess = response.IsSuccessStatusCode;
                    result.StatusCode = response.StatusCode;
                    result.Result = response.Content.ReadAsStringAsync().Result;
                }
                else if (request.MethodType == HttpMethod.Put)
                {
                    var response = await httpClient.PutAsync(request.ServiceMethodUrl, request.Data != null ? content : null);
                    result.IsSuccess = response.IsSuccessStatusCode;
                    result.StatusCode = response.StatusCode;
                    result.Result = response.Content.ReadAsStringAsync().Result;
                }
            }
            return result;
        }

        /// <summary>
        /// Soap Servisler için genel yapı.  HttpClient kullanır.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
        public async Task<string> GetOrPostSoapService(HttpClientModel request)
        {
            string result = string.Empty;
            if (request != null)
            {
                HttpClient httpClient = new();

                HttpContent httpContent = new StringContent(request.Data.ToString(), Encoding.UTF8, "text/xml");
                HttpResponseMessage response;

                if (!string.IsNullOrEmpty(request.Token))
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", request.Token);
                }

                // Header parametreleri ekleniyor
                if (request.HeaderParams != null)
                {
                    foreach (var prm in request.HeaderParams)
                    {
                        httpClient.DefaultRequestHeaders.Add(prm.Key, prm.Value);
                    }
                }

                HttpRequestMessage req = new(request.MethodType, request.ServiceBaseUrl);
                req.Headers.Add("SOAPAction", request.ServiceSoapaActionUrl);

                req.Method = request.MethodType;
                req.Content = httpContent;
                req.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(request.ContentType);
                response = await httpClient.SendAsync(req);
                result = await response.Content.ReadAsStringAsync();

            }
            return result;
        }
        #endregion

        #region Araç API-Servisleri

        /// <summary>
        /// yourassetsonline Apisi
        /// </summary>
        /// <param name="MaptexApiKey"></param>
        /// <param name="MapTexBaseServiceUrl"></param>
        /// <param name="AracImeiNo"></param>
        /// <returns></returns>

        //Aracın Statu Bilgisi getirir
        public MoResponse<AracModel> GetAracStatu(AracApiRequestModel request)
        {
            MoResponse<AracModel> result = new();
            try
            {
				Dictionary<string, string> header = new()
				{
					{ "MAPTEX-API-KEY", request.MaptexApiKey }
				};

				var response = GetOrPostRestService(new HttpClientModel
                {
                    ContentType = "application/json",
                    HeaderParams = header,
                    MethodType = HttpMethod.Get,
                    ServiceBaseUrl = request.MapTexBaseServiceUrl,
                    ServiceMethodUrl = $"/devices/{request.ImeiNo}/actions/current-status?cached=0",
                    Data = null,
                    Token = null
                }).Result;

                if (response.IsSuccess && response.StatusCode == HttpStatusCode.OK)
                {
                    result.Data = System.Text.Json.JsonSerializer.Deserialize<AracModel>(response.Result);
                    result.Success = true;
                }
                else if (response.StatusCode == HttpStatusCode.GatewayTimeout || response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    result.Data = null;
                    result.Success = false;
                    result.Message.Add($"StatusCode {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                result.Message.Add(ex.MyLastInner().Message);
            }

            return result;
        }

        //Aracı Çalıştırır
        public MoResponse<AracModel> AracStart(AracApiRequestModel request)
        {
            MoResponse<AracModel> result = new();
            try
            {
				Dictionary<string, string> header = new()
				{
					{ "MAPTEX-API-KEY", request.MaptexApiKey }
				};

				var response = GetOrPostRestService(new HttpClientModel
                {
                    ContentType = "application/json",
                    HeaderParams = header,
                    MethodType = HttpMethod.Post,
                    ServiceBaseUrl = request.MapTexBaseServiceUrl,
                    ServiceMethodUrl = $"/devices/{request.ImeiNo}/actions/start-vehicle",
                    Data = null,
                    Token = null
                }).Result;

                if (response.IsSuccess && response.StatusCode == HttpStatusCode.OK)
                {
                    result.Data = System.Text.Json.JsonSerializer.Deserialize<AracModel>(response.Result);
                    result.Success = true;
                }
                else if (response.StatusCode == HttpStatusCode.GatewayTimeout || response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    result.Data = null;
                    result.Success = false;
                }

            }
            catch (Exception ex)
            {
                result.Message.Add(ex.MyLastInner().Message);
            }

            return result;
        }

        //Araçları Durdurur
        public MoResponse<AracModel> AracStop(AracApiRequestModel request)
        {
            MoResponse<AracModel> result = new();
            try
            {
				Dictionary<string, string> header = new()
				{
					{ "MAPTEX-API-KEY", request.MaptexApiKey }
				};

				var response = GetOrPostRestService(new HttpClientModel
                {
                    ContentType = "application/json",
                    HeaderParams = header,
                    MethodType = HttpMethod.Post,
                    ServiceBaseUrl = request.MapTexBaseServiceUrl,
                    ServiceMethodUrl = $"/devices/{request.ImeiNo}/actions/stop-vehicle?cached=0&wait=30",
                    Data = null,
                    Token = null
                }).Result;

                if (response.IsSuccess && response.StatusCode == HttpStatusCode.OK)
                {
                    result.Data = System.Text.Json.JsonSerializer.Deserialize<AracModel>(response.Result);
                    result.Success = true;
                }
                else if (response.StatusCode == HttpStatusCode.GatewayTimeout || response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    result.Data = null;
                    result.Success = false;
                }

            }
            catch (Exception ex)
            {
                result.Message.Add(ex.MyLastInner().Message);
            }

            return result;
        }

        //Aracı Bulur (GPRS). !!!!!!! Response verileri kontrol edilecek
        public MoResponse<AracModel> AracBul(AracApiRequestModel request)
        {
            MoResponse<AracModel> result = new();
            try
            {
                Dictionary<string, string> header = new();
                header.Add("MAPTEX-API-KEY", request.MaptexApiKey);

                var response = GetOrPostRestService(new HttpClientModel
                {
                    ContentType = "application/json",
                    HeaderParams = header,
                    MethodType = HttpMethod.Post,
                    ServiceBaseUrl = request.MapTexBaseServiceUrl,
                    ServiceMethodUrl = $"/devices/{request.ImeiNo}/actions/locate?wait=30",
                    Data = null,
                    Token = null
                }).Result;

                if (response.IsSuccess && response.StatusCode == HttpStatusCode.OK)
                {
                    result.Data = System.Text.Json.JsonSerializer.Deserialize<AracModel>(response.Result);
                    result.Success = true;
                }
                else if (response.StatusCode == HttpStatusCode.GatewayTimeout || response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    result.Data = null;
                    result.Success = false;
                }

            }
            catch (Exception ex)
            {
                result.Message.Add(ex.MyLastInner().Message);
            }

            return result;
        }

        //Araçların dijital verilerini getirir. 
        public MoResponse<List<AracDataResponse>> AracDijitalVeriGetir(AracApiRequestModel request)
        {
            MoResponse<List<AracDataResponse>> result = new();
            try
            {
                Dictionary<string, string> header = new();
                header.Add("MAPTEX-API-KEY", request.MaptexApiKey);

                var response = GetOrPostRestService(new HttpClientModel
                {
                    ContentType = "application/json",
                    HeaderParams = header,
                    MethodType = HttpMethod.Get,
                    ServiceBaseUrl = request.MapTexBaseServiceUrl,
                    ServiceMethodUrl = $"/devices/data/current/digital?type={request.FilterType}&restrict-time={request.FilterRestrictTime}&timestamp={request.FilterTimeStamp.ToString("yyyy-MM-dd HH:ss")}",
                    Data = null,
                    Token = null
                }).Result;

                if (response.IsSuccess && response.StatusCode == HttpStatusCode.OK)
                {
                    result.Data = System.Text.Json.JsonSerializer.Deserialize<List<AracDataResponse>>(response.Result);
                    result.Success = true;
                }
                else if (response.StatusCode == HttpStatusCode.GatewayTimeout || response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    result.Data = null;
                    result.Success = false;
                }

            }
            catch (Exception ex)
            {
                result.Message.Add(ex.MyLastInner().Message);
            }

            return result;
        }

        //Araçların analog verilerini getirir. 
        public MoResponse<List<AracDataResponse>> AracAnalogVeriGetir(AracApiRequestModel request)
        {
            MoResponse<List<AracDataResponse>> result = new();
            try
            {
                Dictionary<string, string> header = new();
                header.Add("MAPTEX-API-KEY", request.MaptexApiKey);

                var response = GetOrPostRestService(new HttpClientModel
                {
                    ContentType = "application/json",
                    HeaderParams = header,
                    MethodType = HttpMethod.Get,
                    ServiceBaseUrl = request.MapTexBaseServiceUrl,
                    ServiceMethodUrl = $"/devices/data/current/analog?type={request.FilterType}&restrict-time={request.FilterRestrictTime}&timestamp={request.FilterTimeStamp.ToString("yyyy-MM-dd HH:ss")}",
                    Data = null,
                    Token = null
                }).Result;

                if (response.IsSuccess && response.StatusCode == HttpStatusCode.OK)
                {
                    result.Data = System.Text.Json.JsonSerializer.Deserialize<List<AracDataResponse>>(response.Result);
                    result.Success = true;
                }
                else if (response.StatusCode == HttpStatusCode.GatewayTimeout || response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    result.Data = null;
                    result.Success = false;
                }

            }
            catch (Exception ex)
            {
                result.Message.Add(ex.MyLastInner().Message);
            }

            return result;
        }

        //Araçların text verilerini getirir. 
        public MoResponse<List<AracDataResponse>> AracTextVeriGetir(AracApiRequestModel request)
        {
            MoResponse<List<AracDataResponse>> result = new();
            try
            {
                Dictionary<string, string> header = new();
                header.Add("MAPTEX-API-KEY", request.MaptexApiKey);

                var response = GetOrPostRestService(new HttpClientModel
                {
                    ContentType = "application/json",
                    HeaderParams = header,
                    MethodType = HttpMethod.Get,
                    ServiceBaseUrl = request.MapTexBaseServiceUrl,
                    ServiceMethodUrl = $"/devices/data/current/text?type={request.FilterType}&restrict-time={request.FilterRestrictTime}&timestamp={request.FilterTimeStamp.ToString("yyyy-MM-dd HH:ss")}",
                    Data = null,
                    Token = null
                }).Result;

                if (response.IsSuccess && response.StatusCode == HttpStatusCode.OK)
                {
                    result.Data = System.Text.Json.JsonSerializer.Deserialize<List<AracDataResponse>>(response.Result);
                    result.Success = true;
                }
                else if (response.StatusCode == HttpStatusCode.GatewayTimeout || response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    result.Data = null;
                    result.Success = false;
                }

            }
            catch (Exception ex)
            {
                result.Message.Add(ex.MyLastInner().Message);
            }

            return result;
        }

        //Araçların bilgilerini döner
        public MoResponse<AracInfo> AracInfo(AracApiRequestModel request)
        {
            MoResponse<AracInfo> result = new();
            try
            {

                Dictionary<string, string> header = new();
                header.Add("MAPTEX-API-KEY", request.MaptexApiKey);

                var response = GetOrPostRestService(new HttpClientModel
                {
                    ContentType = "application/json",
                    HeaderParams = header,
                    MethodType = HttpMethod.Get,
                    ServiceBaseUrl = request.MapTexBaseServiceUrl,
                    ServiceMethodUrl = $"/devices/{request.ImeiNo}/info",
                    Data = null,
                    Token = null
                }).Result;

                if (response.IsSuccess && response.StatusCode == HttpStatusCode.OK)
                {
                    result.Data = System.Text.Json.JsonSerializer.Deserialize<AracInfo>(response.Result);
                    result.Success = true;
                }
                else if (response.StatusCode == HttpStatusCode.GatewayTimeout || response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    result.Data = null;
                    result.Success = false;
                }

            }
            catch (Exception ex)
            {
                result.Message.Add(ex.MyLastInner().Message);
            }

            return result;
        }

        //Araç klonlama
        public MoResponse<int> AracClone(AracApiRequestModel request, AracCloneRequest requestClone)
        {
            MoResponse<int> result = new();
            try
            {

                Dictionary<string, string> header = new();
                header.Add("MAPTEX-API-KEY", request.MaptexApiKey);

                var response = GetOrPostRestService(new HttpClientModel
                {
                    ContentType = "application/json",
                    HeaderParams = header,
                    MethodType = HttpMethod.Post,
                    ServiceBaseUrl = request.MapTexBaseServiceUrl,
                    ServiceMethodUrl = $"/devices/{request.ImeiNo}/clone",
                    Data = requestClone,
                    Token = null
                }).Result;

                if (response.IsSuccess && response.StatusCode == HttpStatusCode.OK)
                {
                    result.Data = System.Text.Json.JsonSerializer.Deserialize<int>(response.Result);
                    result.Success = true;
                }
                else if (response.StatusCode == HttpStatusCode.GatewayTimeout || response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    result.Data = 0;
                    result.Success = false;
                }

            }
            catch (Exception ex)
            {
                result.Message.Add(ex.MyLastInner().Message);
            }

            return result;
        }

        //Araç hız limiti güncelleme
        public MoResponse<int> AracHizLimitiGüncelle(AracApiRequestModel request, AracHizLimitRequest requestHizLimit)
        {
            MoResponse<int> result = new();
            try
            {

                Dictionary<string, string> header = new();
                header.Add("MAPTEX-API-KEY", request.MaptexApiKey);

                var response = GetOrPostRestService(new HttpClientModel
                {
                    ContentType = "application/json",
                    HeaderParams = header,
                    MethodType = HttpMethod.Post,
                    ServiceBaseUrl = request.MapTexBaseServiceUrl,
                    ServiceMethodUrl = $"/devices/{request.ImeiNo}/actions/set-speed-limit?wait=30",
                    Data = requestHizLimit,
                    Token = null
                }).Result;

                if (response.IsSuccess && response.StatusCode == HttpStatusCode.OK)
                {
                    result.Data = System.Text.Json.JsonSerializer.Deserialize<int>(response.Result);
                    result.Success = true;
                }
                else if (response.StatusCode == HttpStatusCode.GatewayTimeout || response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    result.Data = 0;
                    result.Success = false;
                }

            }
            catch (Exception ex)
            {
                result.Message.Add(ex.MyLastInner().Message);
            }

            return result;
        }

        //Araç temel donanım yazılımı
        public MoResponse<int> AracTemelDonanimYazilimi(AracApiRequestModel request, AracTemelDonanimYazilimRequest requestDonanim)
        {
            MoResponse<int> result = new();
            try
            {

                Dictionary<string, string> header = new();
                header.Add("MAPTEX-API-KEY", request.MaptexApiKey);

                var response = GetOrPostRestService(new HttpClientModel
                {
                    ContentType = "application/json",
                    HeaderParams = header,
                    MethodType = HttpMethod.Post,
                    ServiceBaseUrl = request.MapTexBaseServiceUrl,
                    ServiceMethodUrl = $"/devices/{request.ImeiNo}/actions/load-firmware-basic?wait=30",
                    Data = requestDonanim,
                    Token = null
                }).Result;

                if (response.IsSuccess && response.StatusCode == HttpStatusCode.OK)
                {
                    result.Data = System.Text.Json.JsonSerializer.Deserialize<int>(response.Result);
                    result.Success = true;
                }
                else if (response.StatusCode == HttpStatusCode.GatewayTimeout || response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    result.Data = 0;
                    result.Success = false;
                }

            }
            catch (Exception ex)
            {
                result.Message.Add(ex.MyLastInner().Message);
            }

            return result;
        }

        //Araç rapor yapılandırması
        public MoResponse<int> AracRaporYapilandirma(AracApiRequestModel request)
        {
            MoResponse<int> result = new();
            try
            {

                Dictionary<string, string> header = new();
                header.Add("MAPTEX-API-KEY", request.MaptexApiKey);

                var response = GetOrPostRestService(new HttpClientModel
                {
                    ContentType = "application/json",
                    HeaderParams = header,
                    MethodType = HttpMethod.Post,
                    ServiceBaseUrl = request.MapTexBaseServiceUrl,
                    ServiceMethodUrl = $"/devices/{request.ImeiNo}/actions/report-configuration?wait=30",
                    Data = null,
                    Token = null
                }).Result;

                if (response.IsSuccess && response.StatusCode == HttpStatusCode.OK)
                {
                    result.Data = System.Text.Json.JsonSerializer.Deserialize<int>(response.Result);
                    result.Success = true;
                }
                else if (response.StatusCode == HttpStatusCode.GatewayTimeout || response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    result.Data = 0;
                    result.Success = false;
                }

            }
            catch (Exception ex)
            {
                result.Message.Add(ex.MyLastInner().Message);
            }

            return result;
        }

        //Aracın batarya kilidini açma
        public MoResponse<int> AracBataryaKilidiniAcma(AracApiRequestModel request)
        {
            MoResponse<int> result = new();
            try
            {

                Dictionary<string, string> header = new();
                header.Add("MAPTEX-API-KEY", request.MaptexApiKey);

                var response = GetOrPostRestService(new HttpClientModel
                {
                    ContentType = "application/json",
                    HeaderParams = header,
                    MethodType = HttpMethod.Post,
                    ServiceBaseUrl = request.MapTexBaseServiceUrl,
                    ServiceMethodUrl = $"/devices/{request.ImeiNo}/actions/unlock-battery?wait=30",
                    Data = null,
                    Token = null
                }).Result;

                if (response.IsSuccess && response.StatusCode == HttpStatusCode.OK)
                {
                    result.Data = System.Text.Json.JsonSerializer.Deserialize<int>(response.Result);
                    result.Success = true;
                }
                else if (response.StatusCode == HttpStatusCode.GatewayTimeout || response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    result.Data = 0;
                    result.Success = false;
                }

            }
            catch (Exception ex)
            {
                result.Message.Add(ex.MyLastInner().Message);
            }

            return result;
        }

        //Aracın batarya kilitleme
        public MoResponse<int> AracBataryaKilitleme(AracApiRequestModel request)
        {
            MoResponse<int> result = new();
            try
            {

                Dictionary<string, string> header = new();
                header.Add("MAPTEX-API-KEY", request.MaptexApiKey);

                var response = GetOrPostRestService(new HttpClientModel
                {
                    ContentType = "application/json",
                    HeaderParams = header,
                    MethodType = HttpMethod.Post,
                    ServiceBaseUrl = request.MapTexBaseServiceUrl,
                    ServiceMethodUrl = $"/devices/{request.ImeiNo}/actions/lock-battery?wait=30",
                    Data = null,
                    Token = null
                }).Result;

                if (response.IsSuccess && response.StatusCode == HttpStatusCode.OK)
                {
                    result.Data = System.Text.Json.JsonSerializer.Deserialize<int>(response.Result);
                    result.Success = true;
                }
                else if (response.StatusCode == HttpStatusCode.GatewayTimeout || response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    result.Data = 0;
                    result.Success = false;
                }

            }
            catch (Exception ex)
            {
                result.Message.Add(ex.MyLastInner().Message);
            }

            return result;
        }

        //Aracın kablo kilidini açar. Start işleminden önce kullanılır.
        public MoResponse<object> AracKabloKilidiniAcma(AracApiRequestModel request)
        {
            MoResponse<object> result = new();
            try
            {
                Dictionary<string, string> header = new();
                header.Add("MAPTEX-API-KEY", request.MaptexApiKey);

                var response = GetOrPostRestService(new HttpClientModel
                {
                    ContentType = "application/json",
                    HeaderParams = header,
                    MethodType = HttpMethod.Post,
                    ServiceBaseUrl = request.MapTexBaseServiceUrl,
                    ServiceMethodUrl = $"/devices/{request.ImeiNo}/actions/unlock-cable",
                    Data = null,
                    Token = null
                }).Result;

                if (response.IsSuccess && response.StatusCode == HttpStatusCode.OK)
                {
                    result.Data = response.Result;
                    result.Success = true;
                }
                else if (response.StatusCode == HttpStatusCode.GatewayTimeout || response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    result.Data = null;
                    result.Success = false;
                }

            }
            catch (Exception ex)
            {
                result.Message.Add(ex.MyLastInner().Message);
            }

            return result;
        }


        /// <summary>
        /// DUCKT API V1.5
        /// </summary>

        //  Authenticate with username and password to get JWT token. Bearer Token
        public MoResponse<TokenResponseModel> GetBearerToken(DucktApiRequestModel request)
        {
            MoResponse<TokenResponseModel> result = new();
            try
            {

                var response = GetOrPostRestService(new HttpClientModel
                {
                    ContentType = "application/json",
                    HeaderParams = null,
                    MethodType = HttpMethod.Post,
                    ServiceBaseUrl = request.DucktBaseServiceUrl,
                    ServiceMethodUrl = $"/api/authenticate",
                    Data = new { username = request.UserName, password = request.Password },
                    Token = null
                }).Result;

                if (response.IsSuccess && response.StatusCode == HttpStatusCode.OK)
                {
                    result.Data = System.Text.Json.JsonSerializer.Deserialize<TokenResponseModel>(response.Result);
                    result.Success = true;
                }
                else if (response.StatusCode == HttpStatusCode.GatewayTimeout || response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    result.Data = null;
                    result.Success = false;
                }

            }
            catch (Exception ex)
            {
                result.Message.Add(ex.MyLastInner().Message);
            }

            return result;
        }

        // Araç Kaydı yapılır
        public MoResponse<RegisterVehicleResponseModel> AracKaydet(DucktApiRequestModel request, RegisterVehicleRequestModel requestData)
        {
            MoResponse<RegisterVehicleResponseModel> result = new();
            try
            {

                var response = GetOrPostRestService(new HttpClientModel
                {
                    ContentType = "application/json",
                    HeaderParams = null,
                    MethodType = HttpMethod.Post,
                    ServiceBaseUrl = request.DucktBaseServiceUrl,
                    ServiceMethodUrl = $"/api/vehicles",
                    Data = requestData,
                    Token = request.Token
                }).Result;

                if (response.IsSuccess && response.StatusCode == HttpStatusCode.OK)
                {
                    result.Data = System.Text.Json.JsonSerializer.Deserialize<RegisterVehicleResponseModel>(response.Result);
                    result.Success = true;
                }
                else if (response.StatusCode == HttpStatusCode.GatewayTimeout || response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    result.Data = null;
                    result.Success = false;
                }

            }
            catch (Exception ex)
            {
                result.Message.Add(ex.MyLastInner().Message);
            }

            return result;
        }

        // İstasyon Kaydı yapılır
        public MoResponse<StationResponseModel> IstasyonKaydet(DucktApiRequestModel request, StationRequestModel requestData)
        {
            MoResponse<StationResponseModel> result = new();
            try
            {

                var response = GetOrPostRestService(new HttpClientModel
                {
                    ContentType = "application/json",
                    HeaderParams = null,
                    MethodType = HttpMethod.Post,
                    ServiceBaseUrl = request.DucktBaseServiceUrl,
                    ServiceMethodUrl = $"/api/stations/register?station={request.StationId}",
                    Data = requestData,
                    Token = request.Token
                }).Result;

                if (response.IsSuccess && response.StatusCode == HttpStatusCode.OK)
                {
                    result.Data = System.Text.Json.JsonSerializer.Deserialize<StationResponseModel>(response.Result);
                    result.Success = true;
                }
                else if (response.StatusCode == HttpStatusCode.GatewayTimeout || response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    result.Data = null;
                    result.Success = false;
                }

            }
            catch (Exception ex)
            {
                result.Message.Add(ex.MyLastInner().Message);
            }

            return result;
        }

        // Araç Kilidini Açma
        public MoResponse<bool> AracKilidiniAcma(DucktApiRequestModel request)
        {
            MoResponse<bool> result = new();
            try
            {

                var response = GetOrPostRestService(new HttpClientModel
                {
                    ContentType = "application/json",
                    HeaderParams = null,
                    MethodType = HttpMethod.Post,
                    ServiceBaseUrl = request.DucktBaseServiceUrl,
                    ServiceMethodUrl = $"/api/unlock?vehicle={request.VehicleId}",
                    Data = null,
                    Token = request.Token
                }).Result;

                if (response.IsSuccess && response.StatusCode == HttpStatusCode.OK)
                {
                    result.Data = true;
                    result.Success = true;
                }
                else if (response.StatusCode == HttpStatusCode.GatewayTimeout || response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    result.Data = false;
                    result.Success = false;
                }

            }
            catch (Exception ex)
            {
                result.Message.Add(ex.MyLastInner().Message);
            }

            return result;
        }

        // Status
        public MoResponse<List<StatusResponseModel>> GetStatus(DucktApiRequestModel request)
        {
            // param=> station=stationId, port=portId, vehicle=vehicleId, user
            MoResponse<List<StatusResponseModel>> result = new();
            try
            {

                var response = GetOrPostRestService(new HttpClientModel
                {
                    ContentType = "application/json",
                    HeaderParams = null,
                    MethodType = HttpMethod.Get,
                    ServiceBaseUrl = request.DucktBaseServiceUrl,
                    ServiceMethodUrl = $"/api/status?{request.Param}",
                    Data = null,
                    Token = request.Token
                }).Result;

                if (response.IsSuccess && response.StatusCode == HttpStatusCode.OK)
                {
                    result.Data = System.Text.Json.JsonSerializer.Deserialize<List<StatusResponseModel>>(response.Result); ;
                    result.Success = true;
                }
                else if (response.StatusCode == HttpStatusCode.GatewayTimeout || response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    result.Data = null;
                    result.Success = false;
                }

            }
            catch (Exception ex)
            {
                result.Message.Add(ex.MyLastInner().Message);
            }

            return result;
        }

        // Health - en son durumu verir
        public MoResponse<HealthResponseModel> GetHealth(DucktApiRequestModel request)
        {
            // param=> station=stationId, port=portId, vehicle=vehicleId, user
            MoResponse<HealthResponseModel> result = new();
            try
            {

                var response = GetOrPostRestService(new HttpClientModel
                {
                    ContentType = "application/json",
                    HeaderParams = null,
                    MethodType = HttpMethod.Get,
                    ServiceBaseUrl = request.DucktBaseServiceUrl,
                    ServiceMethodUrl = $"/api/status?{request.Param}",
                    Data = null,
                    Token = request.Token
                }).Result;

                if (response.IsSuccess && response.StatusCode == HttpStatusCode.OK)
                {
                    result.Data = System.Text.Json.JsonSerializer.Deserialize<HealthResponseModel>(response.Result); ;
                    result.Success = true;
                }
                else if (response.StatusCode == HttpStatusCode.GatewayTimeout || response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    result.Data = null;
                    result.Success = false;
                }

            }
            catch (Exception ex)
            {
                result.Message.Add(ex.MyLastInner().Message);
            }

            return result;
        }

        // Araç güncelle
        public MoResponse<VehicleUpdateResponseModel> AracGuncelle(DucktApiRequestModel request, VehicleUpdateRequestModel requestData = null)
        {
            // param=> station=stationId, port=portId, vehicle=vehicleId, user
            MoResponse<VehicleUpdateResponseModel> result = new();
            try
            {

                var response = GetOrPostRestService(new HttpClientModel
                {
                    ContentType = "application/json",
                    HeaderParams = null,
                    MethodType = HttpMethod.Put,
                    ServiceBaseUrl = request.DucktBaseServiceUrl,
                    ServiceMethodUrl = $"/api/vehicles/update?vehicle={request.VehicleId}",
                    Data = requestData,
                    Token = request.Token
                }).Result;

                if (response.IsSuccess && response.StatusCode == HttpStatusCode.OK)
                {
                    result.Data = System.Text.Json.JsonSerializer.Deserialize<VehicleUpdateResponseModel>(response.Result); ;
                    result.Success = true;
                }
                else if (response.StatusCode == HttpStatusCode.GatewayTimeout || response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    result.Data = null;
                    result.Success = false;
                }

            }
            catch (Exception ex)
            {
                result.Message.Add(ex.MyLastInner().Message);
            }

            return result;
        }

        // İstasyon güncelle
        public MoResponse<StationResponseModel> IstasyonGuncelle(DucktApiRequestModel request, StationRequestModel requestData = null)
        {
            // param=> station=stationId, port=portId, vehicle=vehicleId, user
            MoResponse<StationResponseModel> result = new();
            try
            {

                var response = GetOrPostRestService(new HttpClientModel
                {
                    ContentType = "application/json",
                    HeaderParams = null,
                    MethodType = HttpMethod.Put,
                    ServiceBaseUrl = request.DucktBaseServiceUrl,
                    ServiceMethodUrl = $"/api/stations/update?station={request.StationId}",
                    Data = requestData,
                    Token = request.Token
                }).Result;

                if (response.IsSuccess && response.StatusCode == HttpStatusCode.OK)
                {
                    result.Data = System.Text.Json.JsonSerializer.Deserialize<StationResponseModel>(response.Result);
                    result.Success = true;
                }
                else if (response.StatusCode == HttpStatusCode.GatewayTimeout || response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    result.Data = null;
                    result.Success = false;
                }

            }
            catch (Exception ex)
            {
                result.Message.Add(ex.MyLastInner().Message);
            }

            return result;
        }

        //Callback Method
        public MoResponse<bool> CallbackPost(string callbackUrl = "", string callbackMethod = "", CallbackRequestModel request = null)
        {
            MoResponse<bool> result = new();
            try
            {

                var response = GetOrPostRestService(new HttpClientModel
                {
                    ContentType = "application/json",
                    HeaderParams = null,
                    MethodType = HttpMethod.Post,
                    ServiceBaseUrl = callbackUrl,
                    ServiceMethodUrl = callbackMethod,
                    Data = request,
                    Token = null
                }).Result;

                if (response.IsSuccess && response.StatusCode == HttpStatusCode.OK)
                {
                    result.Data = true;
                    result.Success = true;
                }
                else if (response.StatusCode == HttpStatusCode.GatewayTimeout || response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    result.Data = false;
                    result.Success = false;
                }

            }
            catch (Exception ex)
            {
                result.Message.Add(ex.MyLastInner().Message);
            }

            return result;
        }

        #endregion

        #region Sms-Email-Notificaton Gönderim Function
        public MoResponse<object> SendSms(MesajPaneliData data)
        {
            MoResponse<object> response = new();
            try
            {
                var parametre = this.dataContext.Parameter.FirstOrDefault();

                var user = new SmsUser
                {
                    name = parametre.SmsServiceUserName,
                    pass = parametre.SmsServicePassword
                };

                data.user = data.user ?? user;
                data.msgBaslik = data.msgBaslik ?? parametre.SmsServiceBaslik;
                data.start = data.start == 0 ? (int)DateTime.UtcNow.Subtract(DateTime.Now.AddSeconds(40)).TotalSeconds : data.start;
                data.tr = true;

                var responseSmsApi = GetOrPostRestService(new HttpClientModel
                {
                    ContentType = "application/x-www-form-urlencoded",
                    HeaderParams = null,
                    MethodType = HttpMethod.Post,
                    ServiceBaseUrl = parametre.SmsServiceBaseUrl, //"http://api.mesajpaneli.com",
                    ServiceMethodUrl = parametre.SmsServiceUrl.StartsWith('/') ? parametre.SmsServiceUrl : $"/{parametre.SmsServiceUrl}",  //$"/json_api/",
                    Data = data.MyObjToJsonText().MyToBase64Str(),
                    Token = null
                }).Result;

                var resObje = JObject.Parse(responseSmsApi.Result.MyFromBase64Str());
                if ((bool)resObje["status"])
                {
                    //Servisten dönen bilgiler kayıt altına alınması için kullanılabilir. 
                    MesajPaneliResponse msgPanelResponse = new()
                    {
                        amount = Convert.ToInt32(resObje["amount"]), //Hesabınızdan düşülen kredi
                        credits = Convert.ToInt32(resObje["credits"]), //Gönderim sonrası hesabınızda kalan kredi
                        priv = (bool)resObje["priv"], //true-false
                        referance = Convert.ToInt32(resObje["ref"]), //Rapor sorgulaması için referans numarası
                        status = (bool)resObje["status"], //Gönderim Durumu
                        type = resObje["type"].ToString() //Gönderim Tipi Numeric => Numerik gönderim Alphanumeric => Başlıklı gönderim

                    };

                    response.Success = true;
                    response.Data = msgPanelResponse;
                }
                else
                {
                    response.Data = null;
                    response.Message.Add(resObje["error"].ToString());
                }
            }
            catch (Exception ex)
            {
                response.Message.Add(ex.MyLastInner().Message);
                WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }
            return response;

        }


        #endregion

        #region Araç-İstasyon-Kiralama Kontrol ve İşlemleri

        /// <summary>
        /// Bu method mevcuttaki araç bilgilerini çekmek için kullnılır. Asıl araç listesi methodu DEĞİLDİR.
        /// </summary>
        public void SetVehicleInfo()
        {
            var parameter = this.dataContext.Parameter.FirstOrDefault();
            if (parameter != null)
            {
                var apiVehicle = this.AracDijitalVeriGetir(new AracApiRequestModel
                {
                    MaptexApiKey = parameter.MaptexApiKey,
                    MapTexBaseServiceUrl = parameter.MapTexBaseServiceUrl,
                    FilterType = EnmDijitalDataType.Ignition.MyGetDescription(),
                    FilterRestrictTime = "updated-since",
                    FilterTimeStamp = new DateTime(2020, 1, 1)
                });
                if (apiVehicle.Success)
                {
                    var listImei = apiVehicle.Data.Select(s => s.IMEI).Distinct();
                    var listAracOzellik = this.dataContext.AracOzellik.Where(w => w.Durum).ToList();


                    foreach (var imeiNo in listImei)
                    {
                        var apiVehicleInfo = this.AracInfo(new AracApiRequestModel
                        {
                            MaptexApiKey = parameter.MaptexApiKey,
                            MapTexBaseServiceUrl = parameter.MapTexBaseServiceUrl,
                            ImeiNo = imeiNo
                        });
                        if (apiVehicleInfo.Success)
                        {
                            var rowVehicle = this.dataContext.Arac.Where(w => w.ImeiNo == imeiNo).FirstOrDefault();
                            if (rowVehicle != null)
                            {
                                rowVehicle.Marka = apiVehicleInfo.Data.DeviceManufacturer;
                                rowVehicle.Model = apiVehicleInfo.Data.DeviceType;
                                rowVehicle.Ad = apiVehicleInfo.Data.Name;

                                foreach (var property in apiVehicleInfo.Data.ExtendedProperties)
                                {
                                    var vehicleProperties = this.dataContext.AracOzellikDetay.Include(i => i.AracOzellik)
                                        .Where(w => w.AracId == rowVehicle.Id && w.AracOzellik.Ad == property.Name)
                                        .FirstOrDefault();
                                    if (vehicleProperties != null)
                                    {
                                        vehicleProperties.Deger = vehicleProperties.Deger;
                                        vehicleProperties.UpdateDate = DateTime.Now;
                                    }
                                    else
                                    {
                                        this.dataContext.AracOzellikDetay.Add(new AracOzellikDetay
                                        {
                                            Id = (int)this.dataContext.GetNextSequenceValue("sqAracOzellikDetay"),
                                            AracId = rowVehicle.Id,
                                            AracOzellikId = listAracOzellik.Where(w => w.Ad == property.Name).Select(s => s.Id).FirstOrDefault(),
                                            Deger = property.Value,
                                            CreateDate = DateTime.Now

                                        });
                                    }
                                }

                                this.dataContext.SaveChanges();
                            }
                            else
                            {
                                var newVehicle = new Arac
                                {
                                    Id = (int)this.dataContext.GetNextSequenceValue("sqArac"),
                                    UniqueId = Guid.NewGuid(),
                                    Sira = 10,
                                    Durum = true,
                                    Marka = apiVehicleInfo.Data.DeviceManufacturer,
                                    Model = apiVehicleInfo.Data.DeviceType,
                                    Ad = apiVehicleInfo.Data.Name,
                                    ImeiNo = imeiNo,
                                    Aciklama = "",
                                    BlokeDurum = false,
                                    AcilUyariIstemi = false,
                                    ArizaDurumu = false,
                                    KilitDurumu = true,
                                    KilometreSayaci = 0,
                                    SarjOluyorMu = false,
                                    SarjOrani = 0,
                                    Resim = "",
                                    QrKod = apiVehicleInfo.Data.Name.Replace("Ankara Project - ", "")
                                };

                                this.dataContext.Add(newVehicle);
                                if (this.dataContext.SaveChanges() > 0)
                                {
                                    foreach (var property in apiVehicleInfo.Data.ExtendedProperties)
                                    {

                                        this.dataContext.AracOzellikDetay.Add(new AracOzellikDetay
                                        {
                                            Id = (int)this.dataContext.GetNextSequenceValue("sqAracOzellikDetay"),
                                            AracId = newVehicle.Id,
                                            AracOzellikId = listAracOzellik.Where(w => w.Ad == property.Name).Select(s => s.Id).FirstOrDefault(),
                                            Deger = property.Value,
                                            CreateDate = DateTime.Now

                                        });

                                    }
                                }

                            }
                        }

                    }
                }

            }
        }

        public void SetVehicleQrCode()
        {
            var listVehicle = this.dataContext.Arac.Where(w => w.Durum && w.Ad.Contains("Ankara Project")).ToList();
            foreach (var item in listVehicle)
            {
                item.QrKod = item.Ad.Replace("Ankara Project - ", "");
            }
            this.dataContext.SaveChanges();
        }

        public decimal TarifeFiyatGetir(int memberGroupId)
        {


            var tarife = this.dataContext.Fiyat.Include(i => i.Tarife).Where(w => w.Tarife.Durum == true && w.UyeGrupId == memberGroupId).FirstOrDefault();
            decimal birimFiyat = (decimal)tarife?.HaftaIciFiyat;

            if ((DateTime.Now.DayOfWeek == DayOfWeek.Saturday) || (DateTime.Now.DayOfWeek == DayOfWeek.Sunday))
            {
                birimFiyat = (decimal)tarife?.HaftaSonuFiyat;
            }
            return birimFiyat;
        }

        #endregion

        #region Ödeme Entegrasyon Api İşlemleri
        public MoResponse<object> CreateAndGetMasterPassToken(MasterPassRequest request, string servisBaseUrl)
        {
            MoResponse<object> response = new();

            var result = this.GetOrPostRestService(new HttpClientModel
            {
                MethodType = HttpMethod.Post,
                ContentType = "application/json",
                ServiceBaseUrl = servisBaseUrl,
                ServiceMethodUrl = "/merchant/api/token/generate",
                Data = request
            }).Result;

            if (result.IsSuccess && result.StatusCode == HttpStatusCode.OK)
            {
                response.Data = System.Text.Json.JsonSerializer.Deserialize<MasterPassToken>(result.Result);
                response.Success = true;

            }
            else if (result.StatusCode == HttpStatusCode.NotFound)
            {
                response.Data = System.Text.Json.JsonSerializer.Deserialize<MasterPassError404>(result.Result);
            }
            else
            {
                response.Message.Add(dataContext.TranslateTo("xLng.SistemHatasi"));
            }

            return response;
        }
        #endregion

        #region Üye (Öğrenci APIsi)
        public bool OgrenciMi(string token = "2e2e14a1208kdhy!q4llsmhwfa9011", string tckNo = "")
        {
            bool result = false;
            var responseMebOgrenci = this.GetOrPostSoapService(new HttpClientModel
            {
                ContentType = "text/xml; charset=utf-8",
                HeaderParams = null,
                MethodType = HttpMethod.Post,
                ServiceBaseUrl = "https://ulstekwebser.ego.gov.tr/Projeler/PasoServices.asmx",
                ServiceSoapaActionUrl = "http://tempuri.org/MebOgrencilikSorgula",
                Data = HtmlSoapGetMebOgrenciRequest(token, tckNo),
                Token = null
            }).Result;
            var mebOgrenciXml = XDocument.Parse(responseMebOgrenci);
            var mebResult = ((System.Xml.Linq.XElement)((System.Xml.Linq.XContainer)((System.Xml.Linq.XContainer)((System.Xml.Linq.XContainer)mebOgrenciXml.Root.FirstNode).FirstNode).FirstNode).FirstNode.NextNode).Value;


            var responseYokOgrenci = this.GetOrPostSoapService(new HttpClientModel
            {
                ContentType = "text/xml; charset=utf-8",
                HeaderParams = null,
                MethodType = HttpMethod.Post,
                ServiceBaseUrl = "https://ulstekwebser.ego.gov.tr/Projeler/PasoServices.asmx",
                ServiceSoapaActionUrl = "http://tempuri.org/YokOgrencilikSorgula",
                Data = HtmlSoapGetYokOgrenciRequest(token, tckNo),
                Token = null
            }).Result;
            var yokOgrenciXml = XDocument.Parse(responseYokOgrenci);
            var yokResult = ((System.Xml.Linq.XElement)((System.Xml.Linq.XContainer)((System.Xml.Linq.XContainer)((System.Xml.Linq.XContainer)yokOgrenciXml.Root.FirstNode).FirstNode).FirstNode).FirstNode.NextNode).Value;

            if (mebResult == "E" || yokResult == "E")
                result = true;

            return result;
        }
        private string HtmlSoapGetMebOgrenciRequest(string token = "", string tckNo = "")
        {

            return $@"<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                        <soap:Body>
                          <MebOgrencilikSorgula xmlns=""http://tempuri.org/"">
                            <token>{token}</token>
                            <tckn>{tckNo}</tckn>
                          </MebOgrencilikSorgula>
                        </soap:Body>
                      </soap:Envelope>
                     ";
        }
        private string HtmlSoapGetYokOgrenciRequest(string token = "", string tckNo = "")
        {

            return $@"<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                          <soap:Body>
                            <YokOgrencilikSorgula xmlns=""http://tempuri.org/"">
                              <token>{token}</token>
                              <tckn>{tckNo}</tckn>
                            </YokOgrencilikSorgula>
                          </soap:Body>
                        </soap:Envelope>
                     ";
        }


        #endregion

        #endregion

        #region JOB İşlemleri

        #region local web request
        public void LocalWebRequest()
        {
            try
            {
                //Bu işlem ile application stop olmasını engellemek için doğal bir yöntem olarak kullanılabilir.
                using var client = new HttpClient() { BaseAddress = new Uri(this.appConfig.SelfHost) };
                var response = client.GetAsync("").Result;
            }
            catch (Exception ex)
            {
                WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }
        }

        #endregion

        #region Log Db Jobları
        /// <summary>
        /// Main Dbden AuditLog'ları alıp Log Db deki AuditLog tablosuna yazar. Main Dbdeki aktarılan logları siler.
        /// </summary>      
        public void SetAuditLogToDbLogFromDbMain()
        {
            try
            {
                var mainAuditLogList = this.dataContext.AuditLog.OrderBy(x => x.OperationDate).Take(200).ToList();

                if (mainAuditLogList.Count > 0)
                {
                    foreach (var item in mainAuditLogList)
                    {
                        string sqlText = $@"
                           BEGIN TRY
                                begin tran                                          
                        			  Insert Into smart_bike_log.dbo.AuditLog Select * From smart_bike_main.dbo.AuditLog Where Id='{item.Id}';
                        			  Delete From smart_bike_main.dbo.AuditLog Where Id='{item.Id}';
                                commit tran
                            END TRY
                            BEGIN CATCH
                                IF @@TRANCOUNT > 0
                                    ROLLBACK TRAN  
                        			
                        		DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
                                DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
                                DECLARE @ErrorState INT = ERROR_STATE();
                        
                        		RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
                        
                            END CATCH 

                    ";
                        this.dataContext.Database.ExecuteSqlRaw(sqlText);
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }
        }
        #endregion

        #region Araç-İstasyon-Hareket Jobları

        /// <summary>
        /// Araç-İstasyon Job
        /// </summary>

        // 1- Araç Statu 
        public void SetStatusAllVehicle()
        {
            string ImeiNo = "";
            try
            {
                var param = this.dataContext.Parameter.FirstOrDefault();

                var listVehicle = this.dataContext.Arac.Where(x => x.Durum && x.ImeiNo != null).ToList();
                foreach (var vehicle in listVehicle)
                {
                    AracStatuLog aracLog = new();
                    ImeiNo = vehicle.ImeiNo;

                    var response = new MoResponse<AracModel>();
                    if (param != null)
                    {
                        response = this.GetAracStatu(new AracApiRequestModel
                        {
                            MaptexApiKey = param.MaptexApiKey,
                            MapTexBaseServiceUrl = param.MapTexBaseServiceUrl,
                            ImeiNo = vehicle.ImeiNo
                        });

                        if (response.Success)
                        {
                            var ntsService = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
                            var location = ntsService.CreatePoint(new Coordinate(response.Data.Lat, response.Data.Lon));

                            vehicle.SarjOrani = Convert.ToDecimal(response.Data.ExtendedData.ScooterBattery);
                            vehicle.SonKonum = location;
                            vehicle.KilometreSayaci = Convert.ToDecimal(response.Data.ExtendedData.Odometer);
                            vehicle.SarjOluyorMu = response.Data.ExtendedData.ScooterCharging;
                            vehicle.UpdateDate = DateTime.Now;

                            var vehicleTrip = this.dataContext.AracHareket.Where(w => w.AracId == vehicle.Id && w.BitisTarih == null && w.BitisKonum == null).FirstOrDefault();
                            if (vehicleTrip != null)
                            {
                                vehicleTrip.Mesafe += vehicleTrip.BitisKonum == null ? Convert.ToDecimal(vehicleTrip.BaslangicKonum.Distance(location)) : Convert.ToDecimal(vehicleTrip.BitisKonum.Distance(location));
                                vehicleTrip.BitisKonum = location;

                                var vehicleTripDetail = new AracHareketDetay();
                                vehicleTripDetail.Id = (int)this.dataContext.GetNextSequenceValue("sqAracHareketDetay");
                                vehicleTripDetail.ReportId = response.Data.ReportId;
                                vehicleTripDetail.Konum = location;
                                vehicleTripDetail.AracHareketId = vehicleTrip.Id;
                                vehicleTripDetail.Tarih = response.Data.GpsTimestamp;
                                vehicleTripDetail.CreateDate = DateTime.Now;

                                this.dataContext.AracHareketDetay.Add(vehicleTripDetail);
                            }

                            aracLog.ResponseData = response.Data.MyObjToJsonText();
                            aracLog.ImeiNo = vehicle.ImeiNo;
                            aracLog.ReportId = response.Data.ReportId;
                        }
                        else
                        {
                            aracLog.ResponseData = response.Message.MyObjToJsonText();
                            aracLog.ImeiNo = vehicle.ImeiNo;
                            aracLog.ReportId = 0;
                        }
                        aracLog.Id = Guid.NewGuid();
                        aracLog.Durum = response.Success;
                        aracLog.Tarih = DateTime.Now;
                        aracLog.RequestData = (new { vehicle.Ad, vehicle.ImeiNo, vehicle.UpdateDate }).MyObjToJsonText();
                    }
                    this.dataContext.SaveChanges();

                    if (aracLog.ReportId > 0)
                    {
                        this.logDataContext.AracStatuLog.Add(aracLog);
                        this.logDataContext.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                this.logDataContext.AracStatuLog.Add(new AracStatuLog
                {
                    Id = Guid.NewGuid(),
                    Durum = false,
                    Tarih = DateTime.Now,
                    ImeiNo = ImeiNo,
                    ReportId = 0,
                    RequestData = "Exception",
                    ResponseData = ex.Message.MyObjToJsonText()
                });
                this.logDataContext.SaveChanges();
            }
        }

        public void SetInfoAllVehicle()
        {
            var param = this.dataContext.Parameter.FirstOrDefault();

            var listVehicleProperty = this.dataContext.AracOzellik.Where(w => w.Durum == true).ToList();

            var listVehicle = this.dataContext.Arac.Where(x => x.Durum).ToList();
            foreach (var vehicle in listVehicle)
            {
                var response = new MoResponse<AracInfo>();
                if (param != null)
                {
                    response = this.AracInfo(new AracApiRequestModel
                    {
                        MaptexApiKey = param.MaptexApiKey,
                        MapTexBaseServiceUrl = param.MapTexBaseServiceUrl,
                        ImeiNo = vehicle.ImeiNo
                    });

                    if (response.Success)
                    {
                        vehicle.Ad = response.Data.Name;
                        vehicle.Marka = response.Data.DeviceManufacturer;
                        vehicle.Model = response.Data.DeviceType;

                        foreach (var extendedData in response.Data.ExtendedProperties)
                        {
                            var itemVehiclePropertyDetail = this.dataContext.AracOzellikDetay.Include(i => i.AracOzellik).Where(w => w.AracId == vehicle.Id && w.AracOzellik.Ad == extendedData.Name).FirstOrDefault();

                            if (itemVehiclePropertyDetail != null)
                            {
                                itemVehiclePropertyDetail.Deger = extendedData.Value;
                                itemVehiclePropertyDetail.UpdateDate = DateTime.Now;
                            }
                            else
                            {
                                this.dataContext.AracOzellikDetay.Add(new AracOzellikDetay
                                {
                                    Id = (int)this.dataContext.GetNextSequenceValue("sqAracOzellikDetay"),
                                    AracId = vehicle.Id,
                                    AracOzellikId = listVehicleProperty.Where(w => w.Ad == extendedData.Name).Select(s => s.Id).FirstOrDefault(),
                                    CreateDate = DateTime.Now
                                });
                            }
                        }
                        this.dataContext.SaveChanges();
                    }
                }
            }
        }

        #endregion

        #region Bildirim Jobları
        //30 snde bir çalışabilir
        public bool MailJobMailHareklerdenBekliyorOlanlariGoder()
        {
            bool rV = false;
            try
            {
                //bekliyor olan kayıtların ilk 10 adetini çeker
                //bu kayıtları SendMailForMailHareket e gönderir

                var mailHareketList = dataContext.EmailPool
                    .Where(c => c.TryQuantity <= 3 && c.EmailPoolStatusId == (int)EnmEmailPoolStatus.Waiting)
                    .Take(50);

                foreach (var mailHareket in mailHareketList)
                {
                    mailHelper.SendMailForMailHareket(mailHareket.Id);
                }

            }
            catch (Exception ex)
            {
                WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return rV;
        }

        //2dk da bir çalışabilir
        public bool MailJobMailHarekleriTekrarDene()
        {
            bool rV = false;
            try
            {
                //hata olan kayıtların ilk 50 adetini çeker
                //bu kayıtları SendMailForMailHareket e gönderir

                var mailHareketList = dataContext.EmailPool
                    .Where(c => c.TryQuantity <= 3 && c.EmailPoolStatusId == (int)EnmEmailPoolStatus.Error)
                    .Take(50);

                foreach (var mailHareket in mailHareketList)
                {
                    mailHelper.SendMailForMailHareket(mailHareket.Id);
                }

            }
            catch (Exception ex)
            {
                WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return rV;

        }

        #endregion

        #endregion

        #region Image Proccess
        public MoResponse<object> CreateFileAndGetUrl(int aracHareketId, byte[] imegeByteArray)
        {
            MoResponse<object> response = new();

            //TO DO Resim kaydetme yapılıp url dönecek

            response.Success = true;
            response.Data = "/Vehicle/User/Date/vehicle.jpg";

            return response;
        }
        #endregion

        #region Mobile Api işlemleri 

        /// <summary>
        /// Sms Gönderimi yapılacak ve Sms ile gönderilen kod 5 tane rakamdan oluşacak 
        /// </summary>
        /// <param name="smsTur">1-Login, 2-Register, 3-NewPassword</param>
        /// <param name="adSoyad">Sms içeriğinde hitaben Ad soyad barındırır</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0017:Simplify object initialization", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
        public MoResponse<object> SendSmsVerificationCode(EnmUyeSmsTur smsTur, string adSoyad, string gsm)
        {
            MoResponse<object> response = new();

            response.Data = new Random().Next(10000, 99999);


            List<string> gsmList = new List<string>();
            gsmList.Add(gsm);

            List<MsgDatum> listMsgDatum = new();
            if (smsTur == EnmUyeSmsTur.Login)
            {
                listMsgDatum.Add(new MsgDatum { msg = $"Sn.{adSoyad} sisteme giriş için doğrulama kodunuz: {response.Data}", tel = gsmList });
            }
            else if (smsTur == EnmUyeSmsTur.Register)
            {
                listMsgDatum.Add(new MsgDatum { msg = $"Sn.{adSoyad} üyelik doğrulama kodunuz: {response.Data}", tel = gsmList });
            }
            else if (smsTur == EnmUyeSmsTur.NewPassword)
            {
                listMsgDatum.Add(new MsgDatum { msg = $"Sn.{adSoyad} şifre yenileme kodunuz: {response.Data}", tel = gsmList });
            }
            else
            {
                listMsgDatum.Add(new MsgDatum { msg = $"Sn.{adSoyad} tek kullanımlık sms kodunuz: {response.Data}", tel = gsmList });
            }

            MesajPaneliData mesagePaneliData = new()
            {
                start = (int)DateTime.UtcNow.Subtract(DateTime.Now.AddSeconds(40)).TotalSeconds,
                msgData = listMsgDatum,
                tr = true
            };

            var smsResponse = this.SendSms(mesagePaneliData);
            response.Success = smsResponse.Success;


            return response;
        }

        /// <summary>
        /// Mail Gönderimi yapılacak ve mail ile gönderilen kod 5 tane rakamdan oluşacak 
        /// </summary>
        /// <param name="adSoyad">Email içeriğinde hitaben Ad soyad barındırır</param>
        /// <returns></returns>
        public MoResponse<object> SendMailVerificationCode(string adSoyad)
        {
            MoResponse<object> response = new();

            // mail gönderilecek.....

            response.Data = 12345; //new Random().Next(10000, 99999);
            response.Success = true;

            return response;
        }


        /// <summary>
        /// Uye Sms doğrulama kodu gönderilir. 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MoResponse<object> MemberVerification()
        {
            MoResponse<object> response = new();

            try
            {
                var memberModel = this.dataContext.Uye
                    .Where(c => c.Id == this.MemberToken.MemberId)
                    .FirstOrDefault();

                if (memberModel != null)
                {
                    var responseSms = this.SendSmsVerificationCode(EnmUyeSmsTur.Register, $"{memberModel.Ad} {memberModel.Soyad}", memberModel.Gsm);

                    if (responseSms.Success)
                    {

                        this.MemberToken.IsSmsValidate = false;
                        this.MemberToken.IsMemberLogin = true;
                        this.MemberToken.SmsVerificationCode = responseSms.Data.ToString();

                        response.Data = new
                        {
                            MemberToken = this.GenerateUserToken(EnmClaimType.Member, System.Text.Json.JsonSerializer.Serialize(this.MemberToken))
                        };
                    }
                    response.Success = responseSms.Success;
                    response.Message = responseSms.Message;
                }
                else
                {
                    response.Message.Add(dataContext.TranslateTo("xLng.UyeBilgisiBulunamadi"));
                }

            }
            catch (Exception ex)
            {
                response.Message.Add(ex.MyLastInner().Message);
                WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }
            return response;
        }

        /// <summary>
        /// Uyenin Sms doğrulama kodu ile doğrulanması yapılır. 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MoResponse<object> MemberSmsVerification(MoMemberTokenRequest input)
        {

            MoResponse<object> response = new();

            try
            {
                var memberModel = this.dataContext.Uye
                    .Where(c => c.Id == this.MemberToken.MemberId)
                    .FirstOrDefault();

                if (memberModel != null)
                {

                    if (this.MemberToken.SmsVerificationCode == input.SmsVerificationCode)
                    {

                        this.MemberToken.IsSmsValidate = true;
                        this.MemberToken.IsMemberLogin = true;

                        memberModel.UyelikDogrulama = true;
                        dataContext.SaveChanges();

                        response.Data = new
                        {
                            MemberToken = this.GenerateUserToken(EnmClaimType.Member, System.Text.Json.JsonSerializer.Serialize(this.MemberToken))
                        };

                        response.Success = true;
                    }
                    else
                    {
                        response.Message.Add(dataContext.TranslateTo("xLng.GecersizSmsKodu"));
                    }
                }
                else
                {
                    response.Message.Add(dataContext.TranslateTo("xLng.UyeBilgisiBulunamadi"));
                }

            }
            catch (Exception ex)
            {
                response.Message.Add(ex.MyLastInner().Message);
                WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }
            return response;
        }

        /// <summary>
        /// Login işlemi yapılır token bilgisi döner 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MoResponse<object> MemberLoginForToken(MoLoginRequest input)
        {
            MoResponse<object> response = new();

            try
            {

                var memberModel = this.dataContext.Uye
                    .Where(c => c.Id > 0)
                    .Where(c => c.Gsm == input.Gsm.Replace(" ", "") && c.Sifre == input.Password.MyToEncryptPassword())
                    .FirstOrDefault();

                if (memberModel != null)
                {
                    if (memberModel.UyeDurumId == (int)EnmUyeDurum.Aktif)
                    {
                        this.MemberToken.IsSmsValidate = memberModel.UyelikDogrulama;
                        this.MemberToken.IsMemberLogin = true;

                        MoMemberToken tokenData = new MoMemberToken()
                        {
                            Culture = input.Culture,
                            MemberId = memberModel.Id,
                            UniqueId = memberModel.UniqueId.ToString(),
                            Gsm = memberModel.Gsm,
                            Email = memberModel.Email,
                            NameSurname = $"{memberModel.Ad} {memberModel.Soyad}",
                            MemberGroupId = memberModel.UyeGrupId,
                            MemberStatuId = memberModel.UyeDurumId,

                            IsMemberLogin = true,
                            IsSmsValidate = memberModel.UyelikDogrulama,
                            IsMsisdnValidated = memberModel.MsisdnDogrulama
                        };

                        response.Data = new
                        {
                            MemberToken = this.GenerateUserToken(EnmClaimType.Member, System.Text.Json.JsonSerializer.Serialize(tokenData))
                        };
                        response.Success = true;
                    }
                    else
                    {
                        string errorMsg = memberModel.UyeDurumId == (int)EnmUyeDurum.Bloke ? dataContext.TranslateTo("xLng.UyeliginizBlokeDurumdadir") : dataContext.TranslateTo("xLng.UyeliginizPasifDurumdadir");
                        response.Message.Add(errorMsg);
                    }
                }
                else
                {
                    //response.Success = false; zaten false
                    response.Message.Add(dataContext.TranslateTo("xLng.UyeBilgisiBulunamadi"));
                }
            }
            catch (Exception ex)
            {
                response.Message.Add(ex.MyLastInner().Message);
                WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }
            return response;
        }

        /// <summary>
        ///  Yeni üye kaydı yapılır ve Sms doğrulama kodu ile üyelik doğrulanır. 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MoResponse<object> MemberRegister(MoMemberRegisterRequest input)
        {
            MoResponse<object> response = new();

            try
            {
                var memberModel = this.dataContext.Uye
                    .Where(c => (c.KimlikNumarasi == input.IdentificationNo || c.Gsm == input.Gsm.Replace(" ", "") || c.Email == input.Email) && c.UyeDurumId != (int)EnmUyeDurum.Deleted)
                    .FirstOrDefault();

                bool gsmFormat = input.Gsm.Replace(" ", "").Length == 10 && input.Gsm.Replace(" ", "").Substring(0, 1) == "5" ? true : false;

                if (memberModel == null && gsmFormat)
                {
                    var sendSms = SendSmsVerificationCode(EnmUyeSmsTur.Register, $"{input.Name} {input.Surname}", input.Gsm);
                    if (sendSms.Success)
                    {
                        var member = new Uye()
                        {
                            Id = (int)this.dataContext.GetNextSequenceValue("sqUye"),
                            UniqueId = Guid.NewGuid(),
                            Ad = input.Name,
                            Soyad = input.Surname,
                            KimlikNumarasi = input.IdentificationNo,
                            Gsm = input.Gsm,
                            Email = input.Email,
                            DogumTarihi = input.BirthDate,
                            CinsiyetId = input.Gender,
                            Sifre = input.Password.MyToEncryptPassword(),
                            UyeDurumId = (int)EnmUyeDurum.Aktif,
                            UyeGrupId = this.OgrenciMi(input.IdentificationNo) ? 3 : 1,
                            UyelikTarihi = DateTime.Now,
                            UyelikDogrulama = false,
                            UyelikSozlesmeOnayi = input.MembershipAgreementConfirmation,
                            KvkkOnayi = input.ApprovalAgreementConfirmation,
                            AydinlatmaMetniOnayi = false,
                            SifreSifirlamaKod = "",
                            MsisdnDogrulama = false,
                            CuzdanBakiye = 0,
                            FcmRegistrationToken = "",
                        };

                        this.dataContext.Uye.Add(member);
                        if (this.dataContext.SaveChanges() > 0)
                        {


                            MoMemberToken tokenData = new MoMemberToken()
                            {
                                Culture = input.Culture,
                                MemberId = member.Id,
                                UniqueId = member.UniqueId.ToString(),
                                Gsm = member.Gsm,
                                Email = member.Email,
                                NameSurname = $"{member.Ad} {member.Soyad}",
                                MemberGroupId = member.UyeGrupId,
                                MemberStatuId = member.UyeDurumId,

                                IsMemberLogin = false,
                                IsSmsValidate = false,
                                SmsVerificationCode = sendSms.Data.ToString().MyToEncryptPassword()
                            };

                            response.Data = new
                            {
                                MemberToken = this.GenerateUserToken(EnmClaimType.Member, System.Text.Json.JsonSerializer.Serialize(tokenData)),
                                tokenData.UniqueId
                            };

                            response.Message.AddRange(sendSms.Message);
                            response.Success = sendSms.Success;
                        }
                        else
                        {
                            response.Message.Add(dataContext.TranslateTo("xLng.KayitIslemiYapilamadi"));
                        }
                    }
                    else
                    {
                        response.Message.Add(dataContext.TranslateTo("xLng.KayitIslemiYapilamadi"));
                    }
                }
                else
                {
                    response.Message.Add(dataContext.TranslateTo("xLng.SistemeKayitliUyeBilgisi"));
                }

            }
            catch (Exception ex)
            {
                response.Message.Add(ex.MyLastInner().Message);
                WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }
            return response;
        }

        /// <summary>
        /// Üyelik doğrulama işleminin son adımı olarak gelen Sms kodu karşılaştırılır ve token bilgisi döner 
        /// Veritabanında Uye tablosundaki UyelikDogrulama alanı güncellenir  
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MoResponse<object> MemberRegisterForToken(MoMemberTokenRequest input)
        {
            MoResponse<object> response = new();

            try
            {
                if (this.MemberToken.SmsVerificationCode == input.SmsVerificationCode.MyToEncryptPassword())
                {
                    var memberModel = this.dataContext.Uye
                        .Where(c => c.Id == this.MemberToken.MemberId)
                        .FirstOrDefault();

                    if (memberModel != null)
                    {
                        memberModel.UyelikDogrulama = true;
                        this.dataContext.SaveChanges();

                        this.MemberToken.IsSmsValidate = true;
                        this.MemberToken.IsMemberLogin = true;

                        response.Data = new
                        {
                            MemberToken = this.GenerateUserToken(EnmClaimType.Member, System.Text.Json.JsonSerializer.Serialize(this.MemberToken))
                        };
                        response.Success = true;
                    }
                    else
                    {
                        response.Message.Add(dataContext.TranslateTo("xLng.UyeBilgisiBulunamadi"));
                    }
                }
                else
                {
                    response.Message.Add(dataContext.TranslateTo("xLng.GecersizSmsKodu"));
                }
            }
            catch (Exception ex)
            {
                response.Message.Add(ex.MyLastInner().Message);
                WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }
            return response;
        }

        /// <summary>
        ///  Şifre yenileme/sıfırmalama methodudur.Opsiyonel olarak Sms veya emaile Kod gönderilir ve doğrulama yapılır. 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MoResponse<object> MemberForgotPassword(MoMemberForgotPasswordRequest input)
        {
            MoResponse<object> response = new();

            try
            {
                var memberModel = this.dataContext.Uye
                    .Where(c => c.Gsm == input.Gsm.Replace(" ", "") || c.Email == input.Email)
                    .FirstOrDefault();

                if (memberModel != null)
                {
                    var responseCode = new MoResponse<object>();

                    if (!string.IsNullOrEmpty(input.Gsm.Replace(" ", "")))
                    {
                        responseCode = SendSmsVerificationCode(EnmUyeSmsTur.NewPassword, $"{memberModel.Ad} {memberModel.Soyad}", memberModel.Gsm);
                    }

                    if (!string.IsNullOrEmpty(input.Email))
                    {
                        responseCode = SendMailVerificationCode($"{memberModel.Ad} {memberModel.Soyad}");
                    }

                    memberModel.SifreSifirlamaKod = responseCode.Data.ToString();
                    this.dataContext.SaveChanges();

                    MoMemberToken tokenData = new MoMemberToken()
                    {
                        Culture = input.Culture,
                        MemberId = memberModel.Id,
                        UniqueId = memberModel.UniqueId.ToString(),
                        Gsm = memberModel.Gsm,
                        Email = memberModel.Email,
                        NameSurname = $"{memberModel.Ad} {memberModel.Soyad}",
                        MemberGroupId = memberModel.UyeGrupId,
                        MemberStatuId = memberModel.UyeDurumId,

                        IsMemberLogin = false,
                        IsSmsValidate = false,
                        SmsVerificationCode = responseCode.Data.ToString().MyToEncryptPassword()
                    };

                    response.Data = new
                    {
                        MemberToken = this.GenerateUserToken(EnmClaimType.Member, System.Text.Json.JsonSerializer.Serialize(tokenData)),
                        tokenData.UniqueId
                    };

                    response.Message.AddRange(responseCode.Message);
                    response.Success = responseCode.Success;

                }
                else
                {
                    response.Message.Add(dataContext.TranslateTo("xLng.UyeBilgisiBulunamadi"));
                }

            }
            catch (Exception ex)
            {
                response.Message.Add(ex.MyLastInner().Message);
                WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }
            return response;
        }

        /// <summary>
        /// Şifre yenilemek işlemi için gelen Sms kodu karşılaştırılır ve token bilgisi döner. 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MoResponse<object> MemberForgotPasswordForToken(MoMemberTokenRequest input)
        {
            MoResponse<object> response = new();

            try
            {
                if (this.MemberToken.SmsVerificationCode == input.SmsVerificationCode.MyToEncryptPassword())
                {
                    var memberModel = this.dataContext.Uye
                        .Where(c => c.Id == this.MemberToken.MemberId)
                        .FirstOrDefault();

                    if (memberModel != null)
                    {
                        this.MemberToken.IsSmsValidate = true;
                        this.MemberToken.IsMemberLogin = false;

                        response.Data = new
                        {
                            MemberToken = this.GenerateUserToken(EnmClaimType.Member, System.Text.Json.JsonSerializer.Serialize(this.MemberToken))
                        };
                        response.Success = true;
                    }
                    else
                    {
                        response.Message.Add(dataContext.TranslateTo("xLng.UyeBilgisiBulunamadi"));
                    }
                }
                else
                {
                    response.Message.Add(dataContext.TranslateTo("xLng.GecersizSmsKodu"));
                }
            }
            catch (Exception ex)
            {
                response.Message.Add(ex.MyLastInner().Message);
                WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }
            return response;
        }

        /// <summary>
        /// Sms veya mail kod doğrulaması yapıldıktan sonra kullanıcı tarafından oluşturulan parola veritabanına kaydedilir ve token bilgisi döner. 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MoResponse<object> MemberSetNewPassword(MoMemberNewPasswordRequest input)
        {
            MoResponse<object> response = new();

            try
            {
                var memberModel = this.dataContext.Uye
                    .Where(c => c.Id == this.MemberToken.MemberId)
                    .FirstOrDefault();

                if (memberModel != null)
                {
                    memberModel.Sifre = input.NewPassword.MyToEncryptPassword();
                    this.dataContext.SaveChanges();

                    this.MemberToken.IsSmsValidate = true;
                    this.MemberToken.IsMemberLogin = false;

                    response.Data = new
                    {
                        MemberToken = this.GenerateUserToken(EnmClaimType.Member, System.Text.Json.JsonSerializer.Serialize(this.MemberToken))
                    };
                    response.Success = true;
                }
                else
                {
                    response.Message.Add(dataContext.TranslateTo("xLng.UyeBilgisiBulunamadi"));
                }

            }
            catch (Exception ex)
            {
                response.Message.Add(ex.MyLastInner().Message);
                WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }
            return response;
        }

        /// <summary>
        /// Üyenin şifresini değiştirmek için eski şifre kontrolü yapılıp yeni şifre set edilir. 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MoResponse<object> MemberChangePassword(MoMemberChangePasswordRequest input)
        {
            MoResponse<object> response = new();

            try
            {
                var memberModel = this.dataContext.Uye
                    .Where(c => c.Id == this.MemberToken.MemberId)
                    .FirstOrDefault();

                if (memberModel != null)
                {
                    if (memberModel.Sifre == input.OldPassword.MyToEncryptPassword())
                    {
                        memberModel.Sifre = input.NewPassword.MyToEncryptPassword();
                        this.dataContext.SaveChanges();

                        response.Data = new { SuccessMessage = dataContext.TranslateTo("xLng.SifreDegistirmeIslemiBasarili") };
                        response.Success = true;
                    }
                    else
                    {
                        response.Message.Add(dataContext.TranslateTo("xLng.MevcutSifreFarkliligi"));
                    }
                }
                else
                {
                    response.Message.Add(dataContext.TranslateTo("xLng.UyeBilgisiBulunamadi"));
                }

            }
            catch (Exception ex)
            {
                response.Message.Add(ex.MyLastInner().Message);
                WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }
            return response;
        }

        /// <summary>
        /// Üye hesabını iptal eder
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MoResponse<object> MemberRemove()
        {
            MoResponse<object> response = new();

            try
            {
                var memberModel = this.dataContext.Uye
                    .Where(c => c.Id == this.MemberToken.MemberId)
                    .FirstOrDefault();

                if (memberModel != null)
                {
                    memberModel.UyeDurumId = (int)EnmUyeDurum.Deleted;
                    this.dataContext.SaveChanges();

                    response.Success = true;
                    response.Data = new { SuccessMessage = dataContext.TranslateTo("xLng.HesabinizKapatilmistir") };
                }
                else
                {
                    response.Message.Add(dataContext.TranslateTo("xLng.UyeBilgisiBulunamadi"));
                }

            }
            catch (Exception ex)
            {
                response.Message.Add(ex.MyLastInner().Message);
                WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }
            return response;
        }

        /// <summary>
        /// Token geçerliliğine göre Üye bilgisi döner 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MoResponse<object> MemberGetInfo()
        {
            MoResponse<object> response = new();

            try
            {
                var member = this.dataContext.Uye
                    .Include(c => c.Cinsiyet)
                    .Include(c => c.UyeDurum)
                    .Include(c => c.UyeGrup)
                    .Where(c => c.Id == this.MemberToken.MemberId)
                    .FirstOrDefault();

                if (member != null)
                {
                    var moMember = new MoMemberInfo()
                    {
                        UniqueId = member.UniqueId.ToString(),
                        IdentificationNumber = member.KimlikNumarasi,
                        NameSurname = $"{member.Ad} {member.Soyad}",
                        Gsm = member.Gsm,
                        Email = member.Email,
                        GenderId = member.CinsiyetId,
                        GenderName = member.Cinsiyet.Ad,
                        Avatar = member.Avatar,
                        BirthDate = Convert.ToDateTime(member.DogumTarihi),
                        StatuId = member.UyeDurumId,
                        StatuName = member.UyeDurum.Ad,
                        GroupId = member.UyeGrupId,
                        GroupName = member.UyeGrup.Ad,
                        FcmTopicName = member.UyeGrup.Ad.MyToSeoUrl(),
                        MemberVerification = member.UyelikDogrulama,
                        ApprovalPrivacy = member.KvkkOnayi,
                        ApprovalMemberContract = member.UyelikSozlesmeOnayi,
                        IsMsisdnValidated = member.MsisdnDogrulama

                    };

                    response.Data = moMember;
                    response.Success = true;
                }
                else
                {
                    response.Message.Add(dataContext.TranslateTo("xLng.UyeBilgisiBulunamadi"));
                }

            }
            catch (Exception ex)
            {
                response.Message.Add(ex.MyLastInner().Message);
                WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }
            return response;
        }

        /// <summary>
        /// Token geçerliliğine göre Üyenin Bakiyesini ve bakiye hareketlerini döner 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MoResponse<object> MemberWalletInfo()
        {
            MoResponse<object> response = new();

            try
            {
                var member = this.dataContext.Uye
                    .Where(c => c.Id == this.MemberToken.MemberId)
                    .FirstOrDefault();

                if (member != null)
                {
                    var moMemberWallet = new MoMemberWalletInfo()
                    {
                        Balance = member.CuzdanBakiye == null ? 0 : (decimal)member.CuzdanBakiye,
                        WalletEvents = new List<MemberWalletEvent>()
                    };
                    moMemberWallet.WalletEvents = this.dataContext.UyeCuzdanHareket.Where(w => w.UyeId == this.MemberToken.MemberId)
                        .OrderByDescending(od => od.Tarih)
                        .Select(s => new MemberWalletEvent
                        {
                            Title = s.Aciklama,
                            EventDate = s.Tarih,
                            Ammount = s.CuzdanHareketTurId == 2 ? s.Borc : s.Alacak,
                            EventType = s.CuzdanHareketTurId
                        }).ToList();

                    response.Data = moMemberWallet;
                    response.Success = true;
                }
                else
                {
                    response.Message.Add(dataContext.TranslateTo("xLng.UyeBilgisiBulunamadi"));
                }

            }
            catch (Exception ex)
            {
                response.Message.Add(ex.MyLastInner().Message);
                WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }
            return response;
        }

        /// <summary>
        /// Token geçerliliğine göre Üyenin Bakiyesini ve bakiye hareketlerini döner 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MoResponse<object> MemberBillingInfo()
        {
            MoResponse<object> response = new();

            try
            {
                var member = this.dataContext.Uye
                    .Where(c => c.Id == this.MemberToken.MemberId)
                    .FirstOrDefault();

                if (member != null)
                {
                    var listMemberBilling = this.dataContext.UyeFaturaBilgisi.Where(w => w.UyeId == this.MemberToken.MemberId)
                        .OrderByDescending(od => od.Tarih)
                        .Select(s => new MemberBillingInfo
                        {
                            Title = s.Aciklama,
                            BillingDate = s.Tarih,
                            Ammount = s.Tutar,
                            Url = s.DokumanUrl
                        }).ToList();

                    response.Data = new { MemberBillings = listMemberBilling };
                    response.Success = true;
                }
                else
                {
                    response.Message.Add(dataContext.TranslateTo("xLng.UyeBilgisiBulunamadi"));
                }

            }
            catch (Exception ex)
            {
                response.Message.Add(ex.MyLastInner().Message);
                WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }
            return response;
        }

        /// <summary>
        /// Üye Profile Resmini Base64 olarak kaydeder. 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MoResponse<object> MemberSetProfileImage(MoMemberAvatarRequest input)
        {
            MoResponse<object> response = new();

            try
            {
                var member = this.dataContext.Uye
                    .Where(c => c.Id == this.MemberToken.MemberId)
                    .FirstOrDefault();

                if (member != null)
                {
                    member.Avatar = input.ProfileImageBase64;
                    this.dataContext.SaveChanges();

                    response.Data = new { member.Avatar };
                    response.Success = true;
                }
                else
                {
                    response.Message.Add(dataContext.TranslateTo("xLng.UyeBilgisiBulunamadi"));
                }

            }
            catch (Exception ex)
            {
                response.Message.Add(ex.MyLastInner().Message);
                WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }
            return response;
        }

        /// <summary>
        /// Üyenin sürüş geçmisinin özet bilgisinin listesini döner. 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MoResponse<object> MemberDrivingHistory(MoMemberDrivingHistoryRequest input)
        {
            MoResponse<object> response = new();

            try
            {
                var memberDrivingHistory = this.dataContext.AracHareket
                    .Where(c => c.UyeId == this.MemberToken.MemberId);

                if (input.DateRange == (int)EnmUyeSurusGecmisi.BirHafta)
                {
                    memberDrivingHistory = memberDrivingHistory.Where(c => c.BaslangicTarih.AddDays(-7) <= DateTime.Now);
                }

                if (input.DateRange == (int)EnmUyeSurusGecmisi.BirAy)
                {
                    memberDrivingHistory = memberDrivingHistory.Where(c => c.BaslangicTarih.AddMonths(-1) <= DateTime.Now);
                }

                if (input.DateRange == (int)EnmUyeSurusGecmisi.UcAy)
                {
                    memberDrivingHistory = memberDrivingHistory.Where(c => c.BaslangicTarih.AddMonths(-3) <= DateTime.Now);
                }

                if (input.DateRange == (int)EnmUyeSurusGecmisi.AltiAy)
                {
                    memberDrivingHistory = memberDrivingHistory.Where(c => c.BaslangicTarih.AddMonths(-6) <= DateTime.Now);
                }

                if (input.DateRange == (int)EnmUyeSurusGecmisi.BirYil)
                {
                    memberDrivingHistory = memberDrivingHistory.Where(c => c.BaslangicTarih.AddYears(-1) <= DateTime.Now);
                }

                response.Total = memberDrivingHistory.Count();

                memberDrivingHistory = memberDrivingHistory.OrderByDescending(c => c.BaslangicTarih);

                if (input.Page > 0 && input.PageSize > 0)
                {
                    memberDrivingHistory = memberDrivingHistory.Skip((input.Page - 1) * input.PageSize).Take(input.PageSize);
                }

                var drivingHistoryList = memberDrivingHistory.Include(c => c.Arac).Where(w => w.BitisTarih != null)
                                .Select(c => new MoMemberDrivingHistory
                                {
                                    Amount = c.Tutar,
                                    DrivingTime = Convert.ToInt32(((TimeSpan)(c.BitisTarih - c.BaslangicTarih)).TotalMinutes),
                                    DrivingDate = c.BaslangicTarih.ToString("yyyy-MM-dd"),
                                    Vehicle = $"{c.Arac.Marka} {c.Arac.Model}"
                                }).ToList();

                response.Data = new { DrivingHistory = drivingHistoryList };
                response.Success = true;

                //response.Message.Add(dataContext.TranslateTo("xLng.UyeBilgisiBulunamadi"));


            }
            catch (Exception ex)
            {
                response.Message.Add(ex.MyLastInner().Message);
                WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }
            return response;
        }

        /// <summary>
        /// Üyenin konum bilgisine göre Arac ve istasyon bilgisi döner
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MoResponse<object> ListVehicleAndStation(MoLocationRequest input)
        {

            MoResponse<object> response = new();
            try
            {
                input.Distance = input.Distance == -1 ? 1000000000 : input.Distance;

                var ntsService = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
                var location = ntsService.CreatePoint(new Coordinate(input.Latitude, input.Longitude));

                var listStation = this.dataContext.SarjIstasyonu.Where(x => x.Durum && x.Konum.Distance(location) <= input.Distance)
                                .Select(x => new MoStation
                                {
                                    UniqId = x.UniqueId.ToString(),
                                    Name = x.Ad,
                                    IsAvailable = x.MusaitlikDurum,
                                    Type = EnmVehicleStation.Station.MyGetDescription(),
                                    Location = NtsGeometryServices.Instance.CreateGeometryFactory().CreatePoint(x.Konum.Coordinate),
                                    Latitude = x.Konum.Coordinate.X,
                                    Longitude = x.Konum.Coordinate.Y,
                                    Distance = (int)x.Konum.Distance(location),
                                    VehicleCount = this.dataContext.SarjIstasyonuHareket.Where(w => w.SarjIstasyonuId == x.Id && w.BitisTarih == null).Count()

                                }).ToList();


                var listVehicle = this.dataContext.Arac.Where(x => x.Durum && !x.ArizaDurumu && !x.BlokeDurum && x.SonKonum.Distance(location) <= input.Distance)
                                .Select(x => new MoVehicle
                                {
                                    UniqId = x.UniqueId.ToString(),
                                    Name = x.Ad,
                                    IsAvailable = x.Durum,
                                    Type = EnmVehicleStation.Vehicle.MyGetDescription(),
                                    Location = NtsGeometryServices.Instance.CreateGeometryFactory().CreatePoint(x.SonKonum.Coordinate),
                                    Latitude = x.SonKonum.Coordinate.X,
                                    Longitude = x.SonKonum.Coordinate.Y,
                                    Distance = (int)x.SonKonum.Distance(location),
                                    ChargeRate = Convert.ToDouble(x.SarjOrani)

                                }).ToList();


                response.Data = new
                {
                    Stations = listStation,
                    Vehicles = listVehicle,
                };

                response.Success = true;

            }
            catch (Exception ex)
            {
                response.Message.Add(ex.MyLastInner().Message);
                WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }
            return response;
        }

        /// <summary>
        /// Araç UniqId göre araç detay bilgileri döner
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MoResponse<object> VehicleDetailInfo(MoVehicleDetailRequest input)
        {

            MoResponse<object> response = new();

            try
            {
                var vehicleGuid = new Guid(input.UniqId);
                var rowVehicle = this.dataContext.Arac.Where(w => w.UniqueId == vehicleGuid).FirstOrDefault();

                if (rowVehicle != null)
                {

                    response.Data = rowVehicle;

                    response.Success = true;
                }
                else
                {
                    response.Message.Add(dataContext.TranslateTo("xLng.AracBilgisineUlasilamadi"));
                }

            }
            catch (Exception ex)
            {
                response.Message.Add(ex.MyLastInner().Message);
                WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }
            return response;
        }

        /// <summary>
        /// Üyenin kiraladığı aracın UniqIdne göre Şarj durumunu yüzde olarak döner
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MoResponse<object> VehicleChargeRate(MoVehicleDetailRequest input)
        {

            MoResponse<object> response = new();

            try
            {
                var vehicleGuid = new Guid(input.UniqId);
                var rowVehicle = this.dataContext.Arac.Where(w => w.UniqueId == vehicleGuid).FirstOrDefault();

                if (rowVehicle != null)
                {

                    response.Data = new { ChargeRate = rowVehicle.SarjOrani };

                    response.Success = true;
                }
                else
                {
                    response.Message.Add(dataContext.TranslateTo("xLng.AracBilgisineUlasilamadi"));
                }

            }
            catch (Exception ex)
            {
                response.Message.Add(ex.MyLastInner().Message);
                WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }
            return response;
        }

        /// <summary>
        /// Üye QR kodunu okutarak kiralayacağı aracın müsaitlik durumu kontrol edilir.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MoResponse<object> CheckVehicle(MoVehicleQrCodeRequest input)
        {

            MoResponse<object> response = new();

            try
            {
                var rowVehicle = this.dataContext.Arac.Where(w => w.QrKod == input.QrCode).FirstOrDefault();

                if (rowVehicle != null && rowVehicle.Durum == true && rowVehicle.ArizaDurumu == false && rowVehicle.KilitDurumu == true && rowVehicle.BlokeDurum == false)
                {
                    bool isAracHareket = this.dataContext.AracHareket.Any(a => a.AracId == rowVehicle.Id && a.BitisTarih == null);
                    if (!isAracHareket)
                    {
                        bool isReservation = this.dataContext.AracRezervasyon.Any(a => a.AracId == rowVehicle.Id && a.UyeId != this.MemberToken.MemberId && a.AracRezervasyonDurumId == (int)EnmAracRezervasyonDurum.Rezerve && a.BaslangicTarih < DateTime.Now && a.BitisTarih > DateTime.Now);
                        if (!isReservation)
                        {
                            response.Data = new
                            {
                                VehicleUniqId = rowVehicle.UniqueId,
                                MemberBalance = this.dataContext.Uye.Where(w => w.Id == this.MemberToken.MemberId).Select(s => s.CuzdanBakiye).FirstOrDefault()
                            };
                            response.Success = true;
                        }
                        else
                        {
                            response.Message.Add(dataContext.TranslateTo("xLng.AracRezerveEdilmis"));
                        }
                    }
                    else
                    {
                        response.Message.Add(dataContext.TranslateTo("xLng.AracKiralamaIcinUygunDegil"));
                    }
                }
                else
                {
                    response.Message.Add(dataContext.TranslateTo("xLng.AracKiralamaIcinUygunDegil"));
                }

            }
            catch (Exception ex)
            {
                response.Message.Add(ex.MyLastInner().Message);
                WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }
            return response;
        }

        /// <summary>
        /// Üye QR kodunu okutarak kiralayacağı aracın kilidini açması
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MoResponse<object> RentAndControlVehicle(MoVehicleDetailRequest input)
        {
            MoResponse<object> response = new();

            try
            {
                var vehicleGuid = new Guid(input.UniqId);
                var rowVehicle = this.dataContext.Arac.Where(w => w.UniqueId == vehicleGuid).FirstOrDefault();
                var rowParameter = this.dataContext.Parameter.FirstOrDefault();

                var apiRequestData = new AracApiRequestModel
                {
                    MaptexApiKey = rowParameter?.MaptexApiKey,
                    MapTexBaseServiceUrl = rowParameter?.MapTexBaseServiceUrl,
                    ImeiNo = rowVehicle?.ImeiNo
                };

                //var apiUnlockCable = this.AracKabloKilidiniAcma(apiRequestData);
                //if (apiUnlockCable.Success)
                //{               

                var apiStartVehicle = this.AracStart(apiRequestData);

                if (apiStartVehicle.Success)
                {
                    rowVehicle.KilitDurumu = false;

                    var ntsService = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
                    var location = ntsService.CreatePoint(new Coordinate(apiStartVehicle.Data.Lon, apiStartVehicle.Data.Lat));

                    var rowRezervayon = this.dataContext.AracRezervasyon.Where(w => w.AracId == rowVehicle.Id && w.UyeId == this.MemberToken.MemberId
                         && w.AracRezervasyonDurumId == (int)EnmAracRezervasyonDurum.Rezerve && w.BaslangicTarih < DateTime.Now && w.BitisTarih > DateTime.Now)
                        .FirstOrDefault();

                    this.dataContext.AracHareket.Add(new AracHareket
                    {
                        Id = (int)this.dataContext.GetNextSequenceValue("sqAracHareket"),
                        UniqueId = Guid.NewGuid(),
                        UyeId = this.MemberToken.MemberId,
                        AracId = rowVehicle.Id,
                        BirimFiyat = this.TarifeFiyatGetir(this.MemberToken.MemberGroupId),
                        Mesafe = 0,
                        Tutar = 0,
                        BaslangicTarih = DateTime.Now,
                        BaslangicKonum = location,
                        CreateDate = DateTime.Now,
                        AracRezervasyonId = rowRezervayon != null ? rowRezervayon.Id : 0
                    });
                    if (rowRezervayon != null)
                    {
                        rowRezervayon.AracRezervasyonDurumId = (int)EnmAracRezervasyonDurum.Kullanildi;
                    }
                    this.dataContext.SaveChanges();

                    response.Data = new { ChargeRate = rowVehicle.SarjOrani };
                    response.Success = true;

                }
                else
                {
                    response.Message.Add(dataContext.TranslateTo("xLng.CihazlaBaglantiKurulamadi"));
                }
                //}
                //else
                //{
                //	response.Message.Add(dataContext.TranslateTo("xLng.CihazlaBaglantiKurulamadi"));
                //}
            }
            catch (Exception ex)
            {
                response.Message.Add(ex.MyLastInner().Message);
                WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }
            return response;
        }

        /// <summary>
        /// Üye QR kodunu okutarak kiralayacağı aracın kilidini açması
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MoResponse<object> SetStopVehicle(MoVehicleDetailRequest input)
        {
            MoResponse<object> response = new();

            try
            {
                var vehicleGuid = new Guid(input.UniqId);
                var rowVehicle = this.dataContext.Arac.Where(w => w.UniqueId == vehicleGuid).FirstOrDefault();
                var rowAracHareket = this.dataContext.AracHareket.Where(w => w.AracId == rowVehicle.Id && w.UyeId == this.MemberToken.MemberId && w.BitisTarih == null).FirstOrDefault();
                if (rowAracHareket != null)
                {
                    var rowParameter = this.dataContext.Parameter.FirstOrDefault();
                    var apiRequestData = new AracApiRequestModel
                    {
                        MaptexApiKey = rowParameter?.MaptexApiKey,
                        MapTexBaseServiceUrl = rowParameter?.MapTexBaseServiceUrl,
                        ImeiNo = rowVehicle?.ImeiNo
                    };

                    var apiStartVehicle = this.AracStop(apiRequestData);

                    if (apiStartVehicle.Success)
                    {
                        rowAracHareket.BitisTarih = DateTime.Now;
                        dataContext.SaveChanges();

                        response.Data = new
                        {
                            SuccessMessage = "Tebrikler! Sürüşünüzü başarılı bir şekilde bitirdiniz..."
                        };
                        response.Success = true;
                    }
                    else
                    {
                        response.Message.Add(dataContext.TranslateTo("xLng.CihazlaBaglantiKurulamadi"));
                    }
                }
                else
                {
                    response.Message.Add(dataContext.TranslateTo("xLng.SurusBilgisineUlasilamadi"));
                }
            }
            catch (Exception ex)
            {
                response.Message.Add(ex.MyLastInner().Message);
                WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }
            return response;
        }

        /// <summary>
        /// Üye QR kodunu okutarak kiralayacağı aracın kilidini açması
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MoResponse<object> ReservationVehicle(MoReservationRequest input)
        {

            MoResponse<object> response = new();

            try
            {
                var vehicleGuid = new Guid(input.UniqId);
                var rowVehicle = this.dataContext.Arac.Where(w => w.UniqueId == vehicleGuid).FirstOrDefault();
                if (rowVehicle != null)
                {
                    int rowReservationControl = this.dataContext.AracRezervasyon.Where(w => w.AracId == rowVehicle.Id && w.AracRezervasyonDurumId == (int)EnmAracRezervasyonDurum.Rezerve).Count();

                    if (rowVehicle.Durum == true && rowVehicle.ArizaDurumu == false && rowVehicle.KilitDurumu == true && rowVehicle.BlokeDurum == false && rowReservationControl == 0)
                    {
                        var guid = Guid.NewGuid();
                        var datetimeNow = DateTime.Now;
                        this.dataContext.AracRezervasyon.Add(new AracRezervasyon()
                        {
                            Id = (int)this.dataContext.GetNextSequenceValue("sqAracRezervasyon"),
                            UniqueId = guid,
                            AracId = rowVehicle.Id,
                            UyeId = this.MemberToken.MemberId,
                            BaslangicTarih = datetimeNow,
                            BitisTarih = datetimeNow.AddMinutes(input.ReservationDuration),
                            Sure = input.ReservationDuration,
                            Konum = rowVehicle.SonKonum,
                            AracRezervasyonDurumId = (int)EnmAracRezervasyonDurum.Rezerve
                        });


                        this.dataContext.SaveChanges();

                        response.Data = new
                        {
                            SuccessMessage = dataContext.TranslateTo("xLng.RezervasyonunuzOlusturulmustur"),
                            ReservationInfo = new RezervasyonInfo
                            {
                                UniqId = guid.ToString(),
                                Statu = "Rezerve",
                                Vehicle = rowVehicle.Ad,
                                ReservationDate = datetimeNow.AddMinutes(input.ReservationDuration),
                                CreatedDate = datetimeNow
                            }
                        };
                        response.Success = true;
                    }
                    else
                    {
                        response.Message.Add(dataContext.TranslateTo("xLng.AracRezervasyonIcinUygunDegil"));
                    }
                }
                else
                {
                    response.Message.Add(dataContext.TranslateTo("xLng.AracKiralamaIcinUygunDegil"));
                }

            }
            catch (Exception ex)
            {
                response.Message.Add(ex.MyLastInner().Message);
                WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }
            return response;
        }

        /// <summary>
        /// Üye QR kodunu okutarak kiralayacağı aracın kilidini açması
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MoResponse<object> GetMobileParameter()
        {
            MoResponse<object> response = new();
            try
            {
                var rowParameter = this.dataContext.Parameter.FirstOrDefault();
                if (rowParameter != null)
                {
                    response.Data = new MoParameter
                    {
                        ReservationDuration = rowParameter.AracRezervasyonSure,
                        MasterpassMerchantId = rowParameter.MasterpassMerchantId
                    };
                    response.Success = true;
                }

            }
            catch (Exception ex)
            {
                response.Message.Add(ex.MyLastInner().Message);
                WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }
            return response;
        }

        /// <summary>
        /// Masterpass ödeme-kart kayıt vb. işlem başlatmak için token bilgisi döner
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MoResponse<object> GetAndCreateMasterpassToken(MasterPassRequest input)
        {
            MoResponse<object> response = new();
            try
            {
                var rowParameter = this.dataContext.Parameter.FirstOrDefault();
                input.merchantId = rowParameter.MasterpassMerchantId;
                input.userId = this.MemberToken.UniqueId;

                response = this.CreateAndGetMasterPassToken(input, rowParameter?.MasterpassServiceUrl);

            }
            catch (Exception ex)
            {
                response.Message.Add(ex.MyLastInner().Message);
                WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }
            return response;
        }

        /// <summary>
        /// Firebase Notification için Device/User FcmRegistrationToken set eder
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MoResponse<object> SetFcmRegistrationToken(MoFcmRegistrationTokenRequest input)
        {
            MoResponse<object> response = new();
            try
            {
                var rowMember = this.dataContext.Uye.Where(w => w.Id == this.MemberToken.MemberId).FirstOrDefault();
                if (rowMember.FcmRegistrationToken != input.FcmRegistrationToken)
                {
                    rowMember.FcmRegistrationToken = input.FcmRegistrationToken;

                    this.dataContext.SaveChanges();
                }
                response.Data = input;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message.Add(ex.MyLastInner().Message);
                WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }
            return response;
        }

        /// <summary>
        /// Üye seyahat sonlandırmada araç resmi set eder
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MoResponse<object> SetTripVehicleFile(MoVehicleFileRequest input)
        {
            MoResponse<object> response = new();

            try
            {
                var vehicleGuid = new Guid(input.UniqId);
                var rowVehicle = this.dataContext.Arac.Where(w => w.UniqueId == vehicleGuid).FirstOrDefault();
                if (rowVehicle != null)
                {
                    var rowAracHareket = this.dataContext.AracHareket.Where(w => w.AracId == rowVehicle.Id && w.UyeId == this.MemberToken.MemberId && w.BitisTarih == null).FirstOrDefault();
                    if (rowAracHareket != null)
                    {
                        var responseFile = CreateFileAndGetUrl(rowAracHareket.Id, Convert.FromBase64String(input.VehicleImageBase64));
                        if (responseFile.Success)
                        {
                            this.dataContext.AracHareketResim.Add(new AracHareketResim
                            {
                                Id = (int)this.dataContext.GetNextSequenceValue("sqAracHareketResim"),
                                AracHareketId = rowAracHareket.Id,
                                ResimUrl = responseFile.Data.ToString()
                            });
                            this.dataContext.SaveChanges();

                            response.Success = true;
                            response.Data = new { SuccessMessage = "Araç resmi başarılı bir şekilde kaydedildi." };
                        }
                    }
                    else
                    {
                        response.Message.Add(dataContext.TranslateTo("xLng.SurusBilgisineUlasilamadi"));
                    }
                }
            }
            catch (Exception ex)
            {
                response.Message.Add(ex.MyLastInner().Message);
                WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }
            return response;
        }

        /// <summary>
        /// Sürüş Tutarı bakiyeden düşülür ve bakiye yeterli değilse yükleme için bilgi döner
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MoResponse<object> PaymentForTrip(MoVehicleDetailRequest input)
        {
            MoResponse<object> response = new();

            try
            {
                var vehicleGuid = new Guid(input.UniqId);
                var rowVehicle = this.dataContext.Arac.Where(w => w.UniqueId == vehicleGuid).FirstOrDefault();
                if (rowVehicle != null)
                {
                    var rowAracHareket = this.dataContext.AracHareket.Where(w => w.AracId == rowVehicle.Id && w.UyeId == this.MemberToken.MemberId && w.BitisTarih == null).FirstOrDefault();
                    if (rowAracHareket != null)
                    {
                        TimeSpan timeSpan = DateTime.Now - rowAracHareket.BaslangicTarih;
                        decimal toplamTutar = Convert.ToDecimal(timeSpan.TotalMinutes) * rowAracHareket.BirimFiyat;

                        var rowUye = this.dataContext.Uye.Where(w => w.Id == this.MemberToken.MemberId).FirstOrDefault();

                        if (rowUye.CuzdanBakiye >= toplamTutar)
                        {
                            decimal bakiye = (decimal)rowUye.CuzdanBakiye;
                            rowUye.CuzdanBakiye -= toplamTutar;

                            rowAracHareket.Tutar = Math.Round(toplamTutar, 2);

                            this.dataContext.UyeCuzdanHareket.Add(new UyeCuzdanHareket
                            {
                                Id = (int)this.dataContext.GetNextSequenceValue("sqUyeCuzdanHareket"),
                                UniqueId = Guid.NewGuid(),
                                UyeId = this.MemberToken.MemberId,
                                CuzdanHareketTurId = (int)EnmUyeCuzdanHareketTur.Odeme,
                                Tarih = DateTime.Now,
                                Aciklama = $"{timeSpan.TotalMinutes} dk. Sürüş",
                                Alacak = 0,
                                Borc = toplamTutar
                            });

                            this.dataContext.UyeCariHareket.Add(new UyeCariHareket
                            {
                                Id = (int)this.dataContext.GetNextSequenceValue("sqUyeCariHareket"),
                                UniqueId = Guid.NewGuid(),
                                UyeId = this.MemberToken.MemberId,
                                CariHareketTurId = (int)EnmUyeCariHareketTur.CuzdandanOdeme,
                                Tarih = DateTime.Now,
                                Aciklama = $"{timeSpan.TotalMinutes} dk. Sürüş",
                                Alacak = toplamTutar,
                                Borc = 0
                            });

                            this.dataContext.SaveChanges();

                            response.Success = true;
                            response.Data = new
                            {
                                SuccessMessage = dataContext.TranslateTo("xLng.OdemeIslemiBasarili"),
                                Balance = Math.Round((decimal)rowUye.CuzdanBakiye, 2),
                                TotalAmount = Math.Round(toplamTutar, 2),
                                DepositAmount = 0
                            };
                        }
                        else
                        {
                            response.Success = true;
                            response.Data = new
                            {
                                SuccessMessage = dataContext.TranslateTo("xLng.CuzdanBakiyenizYetersizdir"),
                                Balance = Math.Round((decimal)rowUye.CuzdanBakiye, 2),
                                TotalAmount = Math.Round(toplamTutar, 2),
                                DepositAmount = Math.Round((decimal)(toplamTutar - rowUye.CuzdanBakiye), 2)
                            };
                        }
                    }
                    else
                    {
                        response.Message.Add(dataContext.TranslateTo("xLng.SurusBilgisineUlasilamadi"));
                    }
                }
            }
            catch (Exception ex)
            {
                response.Message.Add(ex.MyLastInner().Message);
                WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }
            return response;
        }

        /// <summary>
        /// Araç kablosu kilitlenir ve stop edilip sürüş bitirilir
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MoResponse<object> SetVehicleLockCableAndStop(MoVehicleDetailRequest input)
        {
            MoResponse<object> response = new();

            try
            {
                var member = this.dataContext.Uye.Where(w => w.Id == this.MemberToken.MemberId).FirstOrDefault();
                var listMobileAppState = member?.MobileAppState != null ? System.Text.Json.JsonSerializer.Deserialize<List<MobileAppState>>(member.MobileAppState) : new List<MobileAppState>();
                var removeItem = listMobileAppState.Where(w => w.VehicleUniqId == input.UniqId).FirstOrDefault();
                listMobileAppState.Remove(removeItem);

                var vehicleGuid = new Guid(input.UniqId);
                var rowVehicle = this.dataContext.Arac.Where(w => w.UniqueId == vehicleGuid).FirstOrDefault();
                if (rowVehicle != null)
                {
                    var rowAracHareket = this.dataContext.AracHareket.Where(w => w.AracId == rowVehicle.Id && w.UyeId == this.MemberToken.MemberId && w.BitisTarih == null).FirstOrDefault();
                    if (rowAracHareket != null)
                    {                        
                        var rowParameter = this.dataContext.Parameter.FirstOrDefault();
                        var apiRequestData = new AracApiRequestModel
                        {
                            MaptexApiKey = rowParameter?.MaptexApiKey,
                            MapTexBaseServiceUrl = rowParameter?.MapTexBaseServiceUrl,
                            ImeiNo = rowVehicle?.ImeiNo
                        };
                        //TODO Araç Kablo kilitleme apisi gelince entegre edilecek
                        bool vehicleLockCable = true; //LockCable apisinden dönen successs kullanılacak
                        if (vehicleLockCable)
                        {
                           var apiStartVehicle = this.AracStop(apiRequestData);

                            if (apiStartVehicle.Success)
                            {
                                rowVehicle.KilitDurumu = true;
                                rowAracHareket.BitisTarih = DateTime.Now;

                                member.MobileAppState = System.Text.Json.JsonSerializer.Serialize(listMobileAppState);
                                dataContext.SaveChanges();

                                response.Data = new
                                {
                                    SuccessMessage = "Tebrikler! Sürüşünüzü başarılı bir şekilde bitirdiniz..."
                                };
                                response.Success = true;
                            }
                            else
                            {
                                response.Message.Add(dataContext.TranslateTo("xLng.CihazlaBaglantiKurulamadi"));
                            }
                        }
                        else
                        {
                            response.Message.Add(dataContext.TranslateTo("xLng.SurusBilgisineUlasilamadi"));
                        }                        
                    }
                    else
                    {
                        response.Message.Add(dataContext.TranslateTo("xLng.SurusBilgisineUlasilamadi"));
                    }
                }
            }
            catch (Exception ex)
            {
                response.Message.Add(ex.MyLastInner().Message);
                WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }
            return response;
        }

        /// <summary>
        /// Sürüş adımlarını getirir
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MoResponse<object> GetMemberTripState()
        {
            MoResponse<object> response = new();

            try
            {
                var member = this.dataContext.Uye.Where(w => w.Id == this.MemberToken.MemberId).FirstOrDefault();

                if (member?.MobileAppState == null || string.IsNullOrEmpty(member?.MobileAppState))
                {
                    response.Data = new
                    {
                        ListDrivingInfo = new List<MobileAppStateResponse>()
                    };
                    response.Success = true;
                }
                else //if (this.MemberToken.TripState == "Driving" || this.MemberToken.TripState == "LockCable")
                {
                    List<MobileAppStateResponse> listState = new();
                    var listMobileAppState = member?.MobileAppState != null ? System.Text.Json.JsonSerializer.Deserialize<List<MobileAppState>>(member.MobileAppState) : new List<MobileAppState>();

                    foreach (var itemState in listMobileAppState)
                    {
                        var guidVehicle = new Guid(itemState.VehicleUniqId);
                        var memberTrip = this.dataContext.AracHareket.Include(i => i.Arac).Where(w => w.UyeId == this.MemberToken.MemberId && w.Arac.UniqueId == guidVehicle && w.BitisTarih == null).FirstOrDefault();
                        if (memberTrip != null) { 
                        TimeSpan timeSpan = DateTime.Now - memberTrip.BaslangicTarih;
                        listState.Add(new MobileAppStateResponse
                        {
                            StartDate = memberTrip.BaslangicTarih,
                            State = itemState.State,
                            UnitPrice = this.TarifeFiyatGetir(this.MemberToken.MemberGroupId),
                            UniqueId = itemState.VehicleUniqId,
                            QrCode = memberTrip.Arac.QrKod,
                            TripTime = timeSpan.TotalMinutes,
                            ChargeRate = memberTrip.Arac.SarjOrani
                        });
                        }
                    }
                    response.Data = new
                    {
                        ListDrivingInfo = listState.Count>0 ? listState : new List<MobileAppStateResponse>(),
                    };
                    response.Success = true;
                    response.Total = listState.Count;
                    
                }
            }
            catch (Exception ex)
            {
                response.Message.Add(ex.MyLastInner().Message);
                WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }
            return response;
        }

        /// <summary>
        /// Sürüş adımlarını tokene ekleyip geri döner
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MoResponse<object> SetMemberTripState(MoMemberTripStateRequest input)
        {
            MoResponse<object> response = new();

            try
            {
                if (input.State == "Driving" || input.State == "LockCable")
                {
                    var member = this.dataContext.Uye.Where(w => w.Id == this.MemberToken.MemberId).FirstOrDefault();
                    if (string.IsNullOrEmpty(member.MobileAppState))
                    {
                        List<MobileAppState> listState = new();
                        listState.Add(new MobileAppState { VehicleUniqId = input.UniqId, State = input.State });
                        member.MobileAppState = System.Text.Json.JsonSerializer.Serialize(listState);
                    }
                    else
                    {
                        var listMobileAppState = member?.MobileAppState != null ? System.Text.Json.JsonSerializer.Deserialize<List<MobileAppState>>(member.MobileAppState) : new List<MobileAppState>();
                        var rowMobileAppState = listMobileAppState.Where(w => w.VehicleUniqId == input.UniqId).FirstOrDefault();
                        if (rowMobileAppState != null)
                        {
                            listMobileAppState.FirstOrDefault(u => u.VehicleUniqId == input.UniqId).State = input.State;
                        }
                        else
                        {
                            listMobileAppState.Add(new MobileAppState { VehicleUniqId = input.UniqId, State = input.State });
                        }
                        member.MobileAppState = System.Text.Json.JsonSerializer.Serialize(listMobileAppState);
                    }
                    this.dataContext.SaveChanges();

                    response.Data = null;
                    response.Success = true;
                }
            }
            catch (Exception ex)
            {
                response.Message.Add(ex.MyLastInner().Message);
                WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }
            return response;
        }

        #endregion

        #region Maps metodları
        /// <summary>
        /// haritada gösterilecek bölge 
        /// </summary>
        /// <returns></returns>
        public MoResponse<Bolge> ReadBolge()
        {
            MoResponse<Bolge> response = new();
            try
            {
                response.Data = this.dataContext.Bolge.Where(c => c.Id == 1).FirstOrDefault();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message.Add(ex.MyLastInner().Message);
                WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }
            return response;
        }

        /// <summary>
        /// haritada gösterilecek bölge detayları (yasaklı ve sakıncalı)
        /// </summary>
        /// <returns></returns>
        public MoResponse<List<BolgeDetay>> ReadBolgeDetayList()
        {
            MoResponse<List<BolgeDetay>> response = new();
            try
            {
                response.Data = this.dataContext.BolgeDetay.Where(c => c.Durum == true).ToList();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message.Add(ex.MyLastInner().Message);
                WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }
            return response;
        }

        /// <summary>
        /// haritada listelenecek Şarj İstasyonu listesi
        /// </summary>
        /// <returns></returns>
        public MoResponse<List<SarjIstasyonu>> ReadSarjIstasyonuList()
        {
            MoResponse<List<SarjIstasyonu>> response = new();
            try
            {
                response.Data = this.dataContext.SarjIstasyonu.Where(c => c.Durum == true).ToList();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message.Add(ex.MyLastInner().Message);
                WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }
            return response;
        }

        /// <summary>
        /// haritada listelenecek Arac listesi
        /// </summary>
        /// <returns></returns>
        public MoResponse<List<Arac>> ReadAracList()
        {
            MoResponse<List<Arac>> response = new();
            try
            {
                response.Data = this.dataContext.Arac.Where(c => c.Durum == true).ToList();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message.Add(ex.MyLastInner().Message);
                WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }
            return response;
        }

        #endregion

        #region Dashboard işlemleri
        public MoResponse<List<Dashboard>> ReadDashboardList()
        {
            MoResponse<List<Dashboard>> response = new();

            try
            {
                List<Dashboard> dashboardList = new();

                dashboardList.Add(new Dashboard()
                {
                    Id = 101,
                    LineNumber = 1,
                    TemplateName = "template1",
                    Title = "Kullanıcı", //this.dataContext.TranslateTo("xLng.User.ShortTitle"),
                    IconClass = "fa fa-fw fa-4x fa-users",
                    IconStyle = "color:red;",
                    DetailUrl = "#/User",
                    Query = "Select Count(*) From [User]"
                });

                dashboardList.Add(new Dashboard()
                {
                    Id = 102,
                    LineNumber = 2,
                    TemplateName = "template1",
                    Title = "Arac",//this.dataContext.TranslateTo("xLng.User.ShortTitle"),
                    IconClass = "fa fa-fw fa-4x fa-bicycle",
                    IconStyle = "color:blue;",
                    DetailUrl = "#/Arac",
                    Query = "Select Count(*) From Arac"
                });

                response.Data = dashboardList;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message.Add(ex.MyLastInner().Message);
                WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return response;
        }

        public MoResponse<object> ReadDashboardData(int id)
        {
            MoResponse<object> response = new();

            try
            {
                var dashboardList = this.ReadDashboardList();
                if (dashboardList.Success && dashboardList.Data != null && dashboardList.Data.Any())
                {
                    var dashboard = dashboardList.Data.Where(c => c.Id == id).FirstOrDefault();
                    if (dashboard != null)
                    {
                        string val = "";
                        try
                        {
                            ////burada reflection kullanılarak bir metod adı da çağrılabilir, veta sql yazssın
                            val = this.dataContext.RawQueryExecuteScalar<string>(dashboard.Query);
                        }
                        catch { }

                        response.Data = new
                        {
                            dashboard.Id,
                            dashboard.IconClass,
                            dashboard.IconStyle,
                            dashboard.DetailUrl,
                            Text = dashboard.Title,
                            Value = val,
                            Saat = DateTime.Now.ToString("HH:mm:ss")
                        };

                        response.Success = true;
                    }
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message.Add(ex.MyLastInner().Message);
                WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return response;
        }

        #endregion

        #region Sözleşmeler
        /// <summary>
        /// Sözleşme linklerini döner
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MoResponse<object> GetContractLinks()
        {
            MoResponse<object> response = new();

            try
            {
                response.Success = true;
                response.Data = new
                {
                    UyelikSozlesmesi = "http://195.46.154.92/sozlesme.html",
                    Kvkk = "http://195.46.154.92/sozlesme.html"
                };
            }
            catch (Exception ex)
            {
                response.Message.Add(ex.MyLastInner().Message);
                WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }
            return response;
        }
        #endregion

        #region Kampanyalar
        /// <summary>
        /// Kampanya bilgisi döner
        /// </summary>
        /// <returns></returns>
        public MoResponse<object> ListCampaign()
        {
            MoResponse<object> response = new();

            try
            {
                //string host = this.dataContext.Parameter.FirstOrDefault().SiteAddress;
                //var list = this.dataContext.Kampanya.Where(w => w.Durum && w.BitisTarihi > DateTime.Now)
                //    .Select(s => new MoCampaingHead
                //    {
                //        Id = s.Id,
                //        Ad = s.Ad,
                //        GorselUrl = s.GorselUrl.MyImageUrlAddStartWithHost(host),
                //        BaslangicTarih = s.BaslangicTarihi,
                //        BitisTarih = s.BitisTarihi
                //    }).OrderByDescending(o => o.BaslangicTarih).ToList();

                //response.Data = new { Campaigns = list };
                response.Success = true;
                //response.Total=list.Count;
            }
            catch (Exception ex)
            {
                response.Message.Add(ex.MyLastInner().Message);
                WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }
            return response;
        }

        /// <summary>
        /// Kampanya detay bilgisi döner
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MoResponse<object> GetCampaign(MoCampaignRequest input)
        {
            MoResponse<object> response = new();

            try
            {
                //string host = this.dataContext.Parameter.FirstOrDefault().SiteAddress;
                //var rowCampaign = this.dataContext.Kampanya.Include(i => i.KampanyaIndirimTipi).Include(i => i.KampanyaIndirimTipi)
                //    .Where(w => w.Id==input.CampaignId).Select(s => new MoCampaing
                //    {
                //        Id = s.Id,
                //        Ad = s.Ad,
                //        TurId = s.KampanyaTurId,
                //        TurAd = s.KampanyaTur.Ad,
                //        IndirimTurId = s.KampanyaIndirimTipiId,
                //        IndirimTurAd = s.KampanyaIndirimTipi.Ad,
                //        Icerik = s.SayfaIcerik,
                //        GorselUrl = s.GorselUrl.MyImageUrlAddStartWithHost(host),
                //        IndirimDegeri = s.IndirimDegeri,
                //        BaslangicTarih = s.BaslangicTarihi,
                //        BitisTarih = s.BitisTarihi
                //    }).FirstOrDefault();

                //response.Data = new { Campaign = rowCampaign };
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message.Add(ex.MyLastInner().Message);
                WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }
            return response;
        }
        #endregion

        #region Stop Trip Manuel Vehicle
        public string EndVehicleTrip(string qrKod)
        {
            var rowVehicle = this.dataContext.Arac.Where(w => w.QrKod == qrKod).FirstOrDefault();
            rowVehicle.KilitDurumu = true;

            var rowVehicleTrip = this.dataContext.AracHareket.Where(w => w.AracId == rowVehicle.Id && w.BitisTarih == null).FirstOrDefault();
            rowVehicleTrip.BitisTarih = DateTime.Now.AddMinutes(-1);

            this.dataContext.SaveChanges();
            return "Sürüş bitirildi";
        }
        public string EndVehicleTripAll()
        {
            var listVehicle = this.dataContext.Arac.ToList();
            foreach (var vehicle in listVehicle)
            {
                vehicle.KilitDurumu = true;
                var rowVehicleTrip = this.dataContext.AracHareket.Where(w => w.AracId == vehicle.Id && w.BitisTarih == null).FirstOrDefault();
                rowVehicleTrip.BitisTarih = DateTime.Now.AddMinutes(-1);

                this.dataContext.SaveChanges();
            }      
            return "Sürüş bitirildi";
        }
        #endregion
        public void Dispose()
        {
            this.mailHelper.Dispose();
            this.repository.Dispose();
            this.dataContext.Dispose();
            this.logDataContext.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}
