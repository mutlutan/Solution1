﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using WebApp.Panel.Codes;
using AppBusiness;
using AppCommon;

namespace WebApp.Panel.Controllers
{
    //[Produces("application/json", "application/xml")]
    //[Produces("application/json")]
    [Route("Panel/[controller]")]
    [ApiController]
    public class ApiController : BaseController
    {
        public ApiController(IServiceProvider _serviceProvider) : base(_serviceProvider) { }

        // GET api/Tem/Test 
        /// <summary>
        /// Sunucu tarih saatini öğrenmek için kullanılır.
        /// </summary>
        /// <returns>Sunucu Tarih Saat</returns>
        [HttpPost("Test")]
        [ResponseCache(Duration = 0)]
        [AuthenticateRequired]
        public IActionResult Test([FromBody] object request)
        {
            var resultObject = new { DateTime = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"), RequestValue = request };
            return Json(resultObject);
        }

        //login
        [HttpPost("Login")]
        [ResponseCache(Duration = 0)]
        public IActionResult Login([FromBody] MoTokenRequest request)
        {
            MoResponse<MoTokenResponse> response = new();
            try
            {
                response = business.UserLoginForApi(request);
            }
            catch (Exception ex)
            {
                response.Message.Add("Hata: " + ex.MyLastInner().Message);
                business.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return Json(response);
        }

        #region lookup
        //lookup controllerdan taşınır ise diye yazıldı, bi deli taşırsa hazır kod burda
        //[HttpPost("GetLookup")]
        //[ResponseCache(Duration = 0)]
        //[AuthenticateRequired]
        //public IActionResult GetLookup([FromBody] LookupRequest request)
        //{
        //    Kendo.Mvc.UI.DataSourceResult dsr = new();
        //    try
        //    {
        //        var lookupResult = business.GetLookupRead(userToken.Culture, request);
        //        dsr.Data = lookupResult;
        //        dsr.Total = lookupResult.Count();
        //    }
        //    catch (Exception ex)
        //    {
        //        dsr.Errors = ex.MyLastInner().Message;
        //        business.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
        //    }

        //    return Json(dsr);
        //}
        #endregion

        [HttpPost("ChangePassword")]
        [ResponseCache(Duration = 0)]
        [AuthenticateRequired]
        public IActionResult ChangePassword([FromBody] MoChangePasswordRequest request)
        {
            MoResponse<object> response = new();

            try
            {
                var captchaValid = this.business.ValidateCaptchaToken(request.CaptchaCode, request.CaptchaToken);

                if (captchaValid)
                {
                    response = this.business.ChangePassword(this.business.UserToken, request);
                }
                else
                {
                    response.Message.Add(business.repository.dataContext.TranslateTo("xLng.GuvenlikKoduGecersiz"));
                }
            }
            catch (Exception ex)
            {
                response.Message.Add("Hata: " + ex.MyLastInner().Message);
                business.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return Json(response);
        }

        [HttpPost("ResetPassword")]
        [ResponseCache(Duration = 0)]
        public IActionResult ResetPassword([FromBody] MoResetPasswordRequest request)
        {
            MoResponse<object> response = new();

            try
            {
                var captchaValid = business.ValidateCaptchaToken(request.CaptchaCode, request.CaptchaToken);

                if (captchaValid)
                {
                    response = business.ResetPassword(request.Email);
                }
                else
                {
                    response.Message.Add(business.repository.dataContext.TranslateTo("xLng.GuvenlikKoduGecersiz"));
                }
            }
            catch (Exception ex)
            {
                response.Message.Add("Hata: " + ex.MyLastInner().Message);
                business.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return Json(response);
        }

        [HttpPost("GaSetupCreate")]
        [ResponseCache(Duration = 0)]
        public IActionResult GaSetupCreate()
        {
            MoResponse<Google.Authenticator.SetupCode> response = new();

            try
            {
                //this.userToken.UserId
                response = business.GaSetupCode();
            }
            catch (Exception ex)
            {
                response.Message.Add("Hata: " + ex.MyLastInner().Message);
                business.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return Json(response);
        }

        [HttpPost("GetAuthorityTemplate")]
        [ResponseCache(Duration = 0)]
        [AuthenticateRequired]
        public IActionResult GetAuthorityTemplate()
        {
            MoResponse<MoAuthority> response = new();

            try
            {
                response = MyApp.GetAuthorityTemplate(this.business.dataContext);
            }
            catch (Exception ex)
            {
                response.Message.Add("Hata: " + ex.MyLastInner().Message);
                business.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return Json(response);
        }

        [HttpGet("GetAppInfo")]
        [ResponseCache(Duration = 0)]
        public ActionResult GetAppInfo()
        {
            var parameter = this.business.GetParameter();

            return Json(new
            {
                appName = Codes.MyApp.AppName,
                version = Codes.MyApp.Version,
                hostAddress = parameter.SiteAddress
            });
        }


        [HttpGet("GetUserInfo")]
        //[AuthenticateRequired]
        [ResponseCache(Duration = 0)]
        public ActionResult GetUserInfo()
        {
            var userAuthorities = business.GetUserAuthorities(this.business.UserToken.UserId);

            return Json(new
            {
                this.business.UserToken.UserId,
                IsAuthenticated = (this.business.UserToken.IsUserLogin == true && this.business.UserToken.IsGoogleValidate),
                IsAdmin = this.business.UserToken.YetkiGrup == EnmYetkiGrup.Admin,
                this.business.UserToken.IsUserLogin,
                this.business.UserToken.IsGoogleValidate,
                this.business.UserToken.Culture,
                this.business.UserToken.UserName,
                this.business.UserToken.NameSurname,
                UserAuthorities = string.Join(',', userAuthorities),
                nYetkiGrup = this.business.UserToken.YetkiGrup
                //UserPhoto= getUserImage()
            });
        }

        [HttpGet("GetDictionary")]
        [ResponseCache(Duration = 0)]
        public ActionResult GetDictionary()
        {
            return Json(new
            {
                dictionary = this.business.repository.dataContext.AppDictionary,
                cultures = Codes.MyApp.SupportedCultures
            });
        }

        [HttpGet("Logout")]
        [AuthenticateRequired]
        public ActionResult Logout()
        {
            this.business.UserLogout();

            return Json(new { sErrors = "" });
        }


    }
}