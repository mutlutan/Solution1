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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;


#nullable disable

namespace AppCommon.Business
{
	public class Business : IDisposable
	{
		public readonly MainDataContext dataContext;
		public readonly LogDataContext logDataContext;
		public Repository repository;
		public MailHelper mailHelper;

		public MoAccessToken UserToken { get; set; } = new();
		public MoAccessToken MemberToken { get; set; } = new();

		public string AppName { get; set; } = "SmartBike Panel";
		public string ContentRootPath { get; set; }
		public string UserIp { get; set; }
		public string UserBrowser { get; set; }
		public string LogDirectory { get; set; } = "logs";

		public Business(IServiceProvider serviceProvider)
		{
			var appConfig = serviceProvider.GetService<IOptions<AppConfig>>()?.Value ?? new();
			this.mailHelper = serviceProvider.GetService<MailHelper>();

			//default dataContext
			this.dataContext = new();
			this.dataContext.SetConnectionString(appConfig.MainConnection);
			this.repository = new Repository(dataContext);

			//log dataContext
			this.logDataContext = new();
			this.logDataContext.SetConnectionString(appConfig.LogConnection);						
		}

		#region logs -- bunu logHelper olarak ayır aslında ILoger a geç XXXXXXXXXXX  

		/// <summary>
		/// Kullanıcı giriş bilgisinin log kaydını tutuyoruz
		/// </summary>
		/// <param name="response"></param>
		/// <param name="input"></param>
		public void UserLogAdd(MoAccessToken accessToken)
		{
			try
			{
				this.logDataContext.AccessLogAdd(new()
				{
					Id = Guid.NewGuid(),
					TableName = accessToken.ClaimType.ToString(),
					UserId = accessToken.AccountId,
					UserName = accessToken.AccountName,
					UserIp = this.UserIp,
					SessionGuid = accessToken.SessionGuid,
					UserBrowser = this.UserBrowser.MyToMaxLength(250),
					LoginDate = DateTime.Now,
					LogoutDate = DateTime.Now,
					ExtraSpace1 = accessToken.NameSurname,
					ExtraSpace2 = accessToken.Culture,
					ExtraSpace3 = accessToken.YetkiGrup.ToString()
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
			if (this.UserToken.IsLogin && this.UserToken.IsGoogleValidate)
			{
				try
				{
					this.logDataContext.AccessLogSetLogoutDate(this.UserToken.SessionGuid);
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
					UserId = this.UserToken.AccountId,
					UserType = this.UserToken.YetkiGrup.ToString(),
					UserName = this.UserToken.AccountName,
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
		public string GenerateToken(EnmClaimType claimType, string jsonText)
		{
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.JWTKey));

			var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
				claims: new System.Security.Claims.Claim[]{
					new (claimType.ToString(), jsonText.MyToBase64Str())
				},
				signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512),
				expires: DateTime.Now.AddDays(1)
			);

			return new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().WriteToken(token);
		}

		public T ValidateToken<T>(EnmClaimType claimType, string token) where T : class
		{
			var moToken = default(T);

			var moUserToken = new MoAccessToken();
			var moMemberToken = new MoAccessToken();

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
						moUserToken = System.Text.Json.JsonSerializer.Deserialize<MoAccessToken>(jsonText);

						var user = this.dataContext.User
							.Where(c => c.Id == moUserToken.AccountId && c.UserStatusId == EnmUserStatus.Active.GetHashCode())
							.OrderByDescending(o => o.Id).FirstOrDefault();

						if (user == null)
						{
							moUserToken.IsLogin = false;
						}
						else if (moUserToken.SessionGuid != user.SessionGuid)
						{
							moUserToken.IsLogin = false;
						}
					}

					if (claimType == EnmClaimType.Member && dataClaim != null)
					{
						string jsonText = dataClaim.Value.MyFromBase64Str();
						moMemberToken = System.Text.Json.JsonSerializer.Deserialize<MoAccessToken>(jsonText);

						var member = this.dataContext.Customer
							.Where(c => c.Id == moMemberToken.AccountId && c.UserStatusId == (int)EnmUserStatus.Active && c.IsEmailConfirmed == true)
							.OrderByDescending(o => o.Id).FirstOrDefault();

						if (member == null)
						{
							moMemberToken.IsLogin = false;
						}

					}
				}
			}
			catch (Exception ex)
			{
				//çok log yazarsa kapatırsın
				moUserToken = new MoAccessToken();
				moMemberToken = new MoAccessToken();
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

		public void AllValidateToken(string token)
		{
			this.UserToken = this.ValidateToken<MoAccessToken>(EnmClaimType.User, token);
			this.MemberToken = this.ValidateToken<MoAccessToken>(EnmClaimType.Member, token);
		}
		#endregion

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

		public List<string> GetUserAuthorities(int userId)
		{
			IEnumerable<string> rV = new List<string>();
			try
			{
				var kullanici = this.dataContext.User
					.Where(c => c.Id == userId).FirstOrDefault();

				if (kullanici != null)
				{
					foreach (string rolId in kullanici.RoleIds.MyToStr().Split(','))
					{
						if (rolId.MyToTrim().Length > 0)
						{
							var rolRow = this.dataContext.Role.Where(c => c.Id == Convert.ToInt32(rolId)).FirstOrDefault();
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

		public bool UserIsAuthorized(MoAccessToken accessToken, string _Key)
		{
			bool rV = false;
			try
			{
				var userYetki = this.GetUserAuthorities(accessToken.AccountId);

				//admin vey a yönetici değil ise normal user ...
				if (accessToken.YetkiGrup == EnmYetkiGrup.Admin)
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
					.Where(c => c.Email == input.UserName && c.Password == input.Password.MyToEncryptPassword())
					.FirstOrDefault();

				if (userModel != null)
				{
					if (userModel.UserStatusId == EnmUserStatus.Active.GetHashCode())
					{
						MoAccessToken accessToken = new()
						{
							SessionGuid = Guid.NewGuid().ToString(),
							Culture = input.Culture,
							AccountId = userModel.Id,
							AccountName = userModel.Email,
							NameSurname = userModel.NameSurname,
							RoleIds = userModel.RoleIds,
							YetkiGrup = EnmYetkiGrup.Personnel,
							IsLogin = true,
							IsPasswordDateValid = true
						};

						if (userModel.RoleIds.MyToStr().Split(',').Where(c => c.StartsWith("10")).FirstOrDefault() != null)
						{
							accessToken.YetkiGrup = EnmYetkiGrup.Admin;
						}

						if (userModel.ValidityDate != null)
						{
							accessToken.IsPasswordDateValid = DateTime.Now.Date <= userModel.ValidityDate;
						}

						userModel.SessionGuid = accessToken.SessionGuid;

						this.dataContext.SaveChanges();

						#region Google Auth
						if (this.GetParameter().UseAuthenticator)
						{
							if (!string.IsNullOrEmpty(userModel.GaSecretKey.MyToTrim()))
							{
								accessToken.IsGoogleSecretKey = true;
								var googleValidate = this.GoogleValidateTwoFactorPIN(userModel.GaSecretKey.MyToTrim(), input.GaCode);
								if (googleValidate.Success)
								{
									accessToken.IsGoogleValidate = true;
								}
								else
								{
									response.Messages.Add(dataContext.TranslateTo("xLng.GaKoduGecersiz"));
								}
							}
						}
						else
						{
							accessToken.IsGoogleSecretKey = true; //GA'yı pas geçer
							accessToken.IsGoogleValidate = true; //GA'yı pas geçer
						}

						response.Data.UserToken = this.GenerateToken(EnmClaimType.User, System.Text.Json.JsonSerializer.Serialize(accessToken));
						response.Data.IsUserLogin = accessToken.IsLogin;
						response.Data.IsGoogleSecretKey = accessToken.IsGoogleSecretKey;
						response.Data.IsGoogleValidate = accessToken.IsGoogleValidate;
						response.Data.IsPasswordDateValid = accessToken.IsPasswordDateValid;


						#endregion

						#region user log
						this.UserLogAdd(accessToken);
						#endregion

						response.Success = true;
					}
					else
					{
						response.Messages.Add(dataContext.TranslateTo("xLng.HesabinizAskiyaAlinmistir"));
					}
				}
				else
				{
					response.Messages.Add(dataContext.TranslateTo("xLng.KullaniciveyaSifreGecersiz"));
				}

			}
			catch (Exception ex)
			{
				response.Messages.Add(dataContext.TranslateTo("xLng.IstekBasarisizOldu"));
				WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
			}

			return response;
		}

		public void UserLogout()
		{
			if (this.UserToken.IsLogin)
			{
				try
				{
					//user çıkış bilgisi güncelleniyor (son işlem zamanı)(bu asenkron olsun bekleme yapmasın)
					// son yazılabilen çıkış zamanı gerçeğe en yakın çıkış zamanıdır
					var user = this.dataContext.User
						.Where(c => c.SessionGuid == this.UserToken.SessionGuid)
						.FirstOrDefault();

					if (user != null)
					{
						user.SessionGuid = "";
						this.dataContext.SaveChanges();
					}
				}
				catch (Exception ex)
				{
					WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
				}
			}
		}

		public MoResponse<object> ChangePassword(MoAccessToken accessToken, MoChangePasswordRequest request)
		{
			MoResponse<object> response = new();

			try
			{
				var user = dataContext.User
					 .Where(c => c.Id > 0 && c.Id == accessToken.AccountId)
					 .Where(c => c.Password == request.OldPassword.MyToEncryptPassword())
					 .FirstOrDefault();

				if (user != null)
				{
					if (request.NewPassword.Length >= 6)
					{
						user.Password = request.NewPassword.MyToEncryptPassword();
						user.ValidityDate = DateTime.Now.Date.AddYears(1);
						dataContext.SaveChanges();
						response.Success = true;
						response.Messages.Add(dataContext.TranslateTo("xLng.YeniSifrenizKayitEdildi"));
					}
					else
					{
						response.Messages.Add(dataContext.TranslateTo("xLng.EnAzAltiKarekterBirHarfBirSayi"));
					}
				}
				else
				{
					response.Messages.Add(dataContext.TranslateTo("xLng.EskiSifreGecersiz"));
				}

			}
			catch (Exception ex)
			{
				response.Messages.Add(ex.MyLastInner().Message);
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
					 .Where(c => c.Id > 0 && c.Email == email)
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
						repository.SaveChanges();

						response.Success = true;
						response.Messages.Add(dataContext.TranslateTo("xLng.YeniSifreKayitliMailAdresineGonderildi"));
					}
					else
					{
						response.Messages.Add(dataContext.TranslateTo("xLng.IsleminizYapilamadiDahaSonraTekrarDeneyebilirsiniz"));
					}
				}
				else
				{
					response.Messages.Add(dataContext.TranslateTo("xLng.MailAdresiTaninlanmamis"));
				}

			}
			catch (Exception ex)
			{
				response.Messages.Add(dataContext.TranslateTo("xLng.IsleminizYapilamadiDahaSonraTekrarDeneyebilirsiniz"));
				WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
			}

			return response;
		}

		#region Yetkili olmayan fieldların çıkartılması
		public List<Newtonsoft.Json.Linq.JObject> GetAuthorityColumnsAndData(MoAccessToken accessToken, string tableName, IEnumerable<dynamic> data)
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
					else if (!this.UserIsAuthorized(accessToken, $".{tableName}.D_R.{p.Name}."))
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

		#region Member
		public MoResponse<MoTokenResponse> MemberLoginForApi(MoTokenRequest input)
		{
			MoResponse<MoTokenResponse> response = new() { Data = new MoTokenResponse() };

			try
			{
				var memberModel = this.dataContext.Customer
					.Where(c => c.Id > 0)
					.Where(c => c.Email == input.UserName && c.Password == input.Password.MyToEncryptPassword())
					.FirstOrDefault();

				if (memberModel != null)
				{
					if (memberModel.UserStatusId == EnmUserStatus.Active.GetHashCode())
					{
						MoAccessToken accessToken = new()
						{
							AccountId = memberModel.Id,
							SessionGuid = Guid.NewGuid().ToString(),
							Culture = input.Culture,
							AccountName = memberModel.Email,
							NameSurname = memberModel.NameSurname,
							IsLogin = true,
							IsPasswordDateValid = true
						};

						if (memberModel.ValidityDate != null)
						{
							accessToken.IsPasswordDateValid = DateTime.Now.Date <= memberModel.ValidityDate;
						}

						memberModel.SessionGuid = accessToken.SessionGuid;

						this.dataContext.SaveChanges();

						#region Google Auth
						if (this.GetParameter().UseAuthenticator)
						{
							if (!string.IsNullOrEmpty(memberModel.GaSecretKey.MyToTrim()))
							{
								accessToken.IsGoogleSecretKey = true;
								var googleValidate = this.GoogleValidateTwoFactorPIN(memberModel.GaSecretKey.MyToTrim(), input.GaCode);
								if (googleValidate.Success)
								{
									accessToken.IsGoogleValidate = true;
								}
								else
								{
									response.Messages.Add(dataContext.TranslateTo("xLng.GaKoduGecersiz"));
								}
							}
						}
						else
						{
							accessToken.IsGoogleSecretKey = true; //GA'yı pas geçer
							accessToken.IsGoogleValidate = true; //GA'yı pas geçer
						}

						response.Data.UserToken = this.GenerateToken(EnmClaimType.User, System.Text.Json.JsonSerializer.Serialize(accessToken));
						response.Data.IsUserLogin = accessToken.IsLogin;
						response.Data.IsGoogleSecretKey = accessToken.IsGoogleSecretKey;
						response.Data.IsGoogleValidate = accessToken.IsGoogleValidate;
						response.Data.IsPasswordDateValid = accessToken.IsPasswordDateValid;


						#endregion

						#region user log
						this.UserLogAdd(accessToken);
						#endregion

						response.Success = true;
					}
					else
					{
						response.Messages.Add(dataContext.TranslateTo("xLng.HesabinizAskiyaAlinmistir"));
					}
				}
				else
				{
					response.Messages.Add(dataContext.TranslateTo("xLng.KullaniciveyaSifreGecersiz"));
				}

			}
			catch (Exception ex)
			{
				response.Messages.Add(dataContext.TranslateTo("xLng.IstekBasarisizOldu"));
				WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
			}

			return response;
		}

		public void MemberLogout()
		{
			if (this.MemberToken.IsLogin)
			{
				try
				{
					//member çıkış bilgisi güncelleniyor (son işlem zamanı)(bu asenkron olsun bekleme yapmasın)
					// son yazılabilen çıkış zamanı gerçeğe en yakın çıkış zamanıdır
					var member = this.dataContext.Customer
						.Where(c => c.SessionGuid == this.UserToken.SessionGuid)
						.FirstOrDefault();

					if (member != null)
					{
						member.SessionGuid = "";
						this.dataContext.SaveChanges();
					}
				}
				catch (Exception ex)
				{
					WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
				}
			}
		}
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

				var personeUser = this.dataContext.User.Where(c => c.Id == this.UserToken.AccountId).FirstOrDefault();
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

		#region Logs reads
		public MoResponse<object> ReadUserLog(MoAccessToken accessToken, ApiRequest request)
		{
			MoResponse<object> response = new();

			try
			{
				var query = this.logDataContext.UserLog;

				var dsr = query.ToDataSourceResult(request.MyToDataSourceRequest());

				response.Total = dsr.Total;
				response.Data = this.GetAuthorityColumnsAndData(accessToken, "AccessLog", dsr.Data.Cast<dynamic>());
				response.Success = true;
			}
			catch (Exception ex)
			{
				response.Messages.Add(ex.MyLastInner().Message);
			}

			return response;
		}

		#endregion

		#region User functions
		public MoResponse<object> UserRead(MoAccessToken accessToken, ApiRequest request)
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
						s.UserStatusId,
						s.UserTypeId,
						s.RoleIds,
						s.Email,
						UserPassword = "",
						s.ResidenceAddress,
						s.NameSurname,
						s.IsEmailConfirmed
					});

				var dsr = query.ToDataSourceResult(request.MyToDataSourceRequest());

				response.Total = dsr.Total;
				response.Data = this.GetAuthorityColumnsAndData(accessToken, "User", dsr.Data.Cast<dynamic>());
				response.Success = true;
			}
			catch (Exception ex)
			{
				response.Messages.Add(ex.MyLastInner().Message);
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
					model.Password = model.Password.MyToDecryptPassword();
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
				response.Messages.Add(ex.MyLastInner().Message);
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
						response.Messages.Add(this.dataContext.TranslateTo("xLng.IslemYapilacakKayitBulunamadi"));
					}
				}
				else
				{
					response.Messages.Add(this.dataContext.TranslateTo("xLng.VeriModeliDonusturulemedi"));
				}

			}
			catch (Exception ex)
			{
				if (ex.Source == "Microsoft.EntityFrameworkCore.Relational")
				{
					response.Messages.Add(this.dataContext.TranslateTo("xLng.BaglantiliKayitVarSilinemez"));
				}
				else
				{
					response.Messages.Add(ex.MyLastInner().Message);
				}
			}
			return response;
		}
		#endregion

		#region Role functions
		public MoResponse<object> RoleRead(MoAccessToken accessToken, ApiRequest request)
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

				var dsr = query.ToDataSourceResult(request.MyToDataSourceRequest());

				response.Total = dsr.Total;
				response.Data = this.GetAuthorityColumnsAndData(accessToken, "Role", dsr.Data.Cast<dynamic>());
				response.Success = true;
			}
			catch (Exception ex)
			{
				response.Messages.Add(ex.MyLastInner().Message);
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
				response.Messages.Add(ex.MyLastInner().Message);
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
						response.Messages.Add(this.dataContext.TranslateTo("xLng.IslemYapilacakKayitBulunamadi"));
					}
				}
				else
				{
					response.Messages.Add(this.dataContext.TranslateTo("xLng.VeriModeliDonusturulemedi"));
				}

			}
			catch (Exception ex)
			{
				if (ex.Source == "Microsoft.EntityFrameworkCore.Relational")
				{
					response.Messages.Add(this.dataContext.TranslateTo("xLng.BaglantiliKayitVarSilinemez"));
				}
				else
				{
					response.Messages.Add(ex.MyLastInner().Message);
				}
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
				var query = this.dataContext.Dashboard
					.Where(c => c.Id > 0);

				response.Data = query.ToList();
				response.Success = true;
			}
			catch (Exception ex)
			{
				response.Messages.Add(ex.MyLastInner().Message);
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
				response.Messages.Add(ex.MyLastInner().Message);
				WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
			}

			return response;
		}

		#endregion

		#region maps işlemleri

		/// <summary>
		/// haritada listelenecek Customers listesi
		/// </summary>
		/// <returns></returns>
		public MoResponse<List<Customer>> ReadCustomers()
		{
			MoResponse<List<Customer>> response = new();

			try
			{
				var query = this.dataContext.Customer
					.Where(c => c.Id > 0).ToList();

				response.Data = query;
				response.Total=query.Count;
				response.Success = true;
			}
			catch (Exception ex)
			{
				response.Messages.Add(ex.MyLastInner().Message);
				WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
			}
			return response;
		}
		#endregion

		public void Dispose()
		{
			this.repository.Dispose();
			this.dataContext.Dispose();
			this.logDataContext.Dispose();

			GC.SuppressFinalize(this);
		}
	}
}
