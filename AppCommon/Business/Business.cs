﻿using Microsoft.EntityFrameworkCore;
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

namespace AppCommon.Business
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
				return this.AppName.Replace(" ", "") + "-JWT-KEY-00000000000000001" + "-" + "v.01";
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
						if (this.appConfig.UseAuthenticator)
						{
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
						}
						else
						{
							moUserToken.IsGoogleSecretKey = true; //GA'yı pas geçer
							moUserToken.IsGoogleValidate = true; //GA'yı pas geçer
						}

						response.Data.UserToken = this.GenerateUserToken(EnmClaimType.User, System.Text.Json.JsonSerializer.Serialize(moUserToken));
						response.Data.IsUserLogin = moUserToken.IsUserLogin;
						response.Data.IsGoogleSecretKey = moUserToken.IsGoogleSecretKey;
						response.Data.IsGoogleValidate = moUserToken.IsGoogleValidate;
						response.Data.IsPasswordDateValid = moUserToken.IsPasswordDateValid;


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

			if (lookupRequest.TableName.ToLower() == "user")
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

		#region Mobile Api işlemleri 

		/// <summary>
		/// Mail Gönderimi yapılacak ve mail ile gönderilen kod 5 tane rakamdan oluşacak 
		/// </summary>
		/// <param name="adSoyad">Email içeriğinde hitaben Ad soyad barındırır</param>
		/// <returns></returns>
		public MoResponse<object> SendMailVerificationCode(string email, string adSoyad)
		{
			MoResponse<object> response = new();

			// mail gönderilecek.....

			response.Data = 12345; //new Random().Next(10000, 99999);
			response.Success = true;

			return response;
		}


		/// <summary>
		/// Uye doğrulama kodu gönderilir. 
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
					var responseSendVerificationCode = SendMailVerificationCode(memberModel.Email, $"{memberModel.Ad} {memberModel.Soyad}");

					if (responseSendVerificationCode.Success)
					{
						this.MemberToken.IsSmsValidate = false;
						this.MemberToken.IsMemberLogin = true;
						this.MemberToken.SmsVerificationCode = responseSendVerificationCode.Data.ToString();

						response.Data = new
						{
							MemberToken = this.GenerateUserToken(EnmClaimType.Member, System.Text.Json.JsonSerializer.Serialize(this.MemberToken))
						};
					}
					response.Success = responseSendVerificationCode.Success;
					response.Message = responseSendVerificationCode.Message;
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
					var responseSendVerificationCode = SendMailVerificationCode(memberModel.Email, $"{memberModel.Ad} {memberModel.Soyad}");

					if (responseSendVerificationCode.Success)
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
								SmsVerificationCode = responseSendVerificationCode.Data.ToString().MyToEncryptPassword()
							};

							response.Data = new
							{
								MemberToken = this.GenerateUserToken(EnmClaimType.Member, System.Text.Json.JsonSerializer.Serialize(tokenData)),
								tokenData.UniqueId
							};

							response.Message.AddRange(responseSendVerificationCode.Message);
							response.Success = responseSendVerificationCode.Success;
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
					.Where(c => c.Email == input.Email)
					.FirstOrDefault();

				if (memberModel != null)
				{
					var responseCode = new MoResponse<object>();

					if (!string.IsNullOrEmpty(input.Email))
					{
						responseCode = this.SendMailVerificationCode(memberModel.Email, $"{memberModel.Ad} {memberModel.Soyad}");
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