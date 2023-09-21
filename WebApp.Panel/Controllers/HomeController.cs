using System;
using Microsoft.AspNetCore.Mvc;
using AppCommon;

namespace WebApp.Panel.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IServiceProvider _serviceProvider) : base(_serviceProvider) { }

        public IActionResult Test()
        {
            MoResponse<object> response = new();

            #region mail test
            //this.business.SetVehicleQrCode();
            // this.business.SendMobileNotifyJob();
            //var sonuc = this.cmsBusiness.mailHelper.SendMail_Sifre_Bildirim("qq@qq.com","1234");
            //this.business.SendSmsVerificationCode(EnmUyeSmsTur.Login, "Mutlu Mutlutan", "5321571180");
            //this.business.GetAracStatu(new AppBusiness.AracApiRequestModel
            //{
            //    ImeiNo = "868159055278985",
            //    FilterTimeStamp = DateTime.Now.AddDays(-30),
            //    FilterRestrictTime = ""
            //});

            //var resultToken = this.business.GetBearerToken(new DucktApiRequestModel
            //{
            //    DucktBaseServiceUrl = "https://api.duckt.app:8444",
            //    Password = "nMXp2axwm5",
            //    UserName = "sacepni"
            //});

            //this.business.AracKabloKilidiniAcma(new AracApiRequestModel {
            //    MaptexApiKey = "06AF2D0E-4475-4262-AF39-48061DEC8D2A",
            //    MapTexBaseServiceUrl = "https://ffsapi.yourassetsonline.com:8446",
            //    ImeiNo = "868159055278985"
            //});
            //21347585128
            //this.business.OgrenciMi("2e2e14a1208kdhy!q4llsmhwfa9011", "21347585128");


            #endregion

            return Json(response);
        }

        public IActionResult Index()
        {
            if (!this.business.UserToken.IsLogin)
            {
                return RedirectToAction("Index", "Account");
            }

            return View();
        }

        //https://localhost:44309/Home/Create?token=TOKEN-001-MSM
        //https://localhost:44309/Home/Create?token=TOKEN-001-MSM&db=log
        public string Create(string token, string db = "main")
        {
            if (token == "TOKEN-001-MSM")
            {
                var sonuc = "";

                if (db == "main")
                {
                    sonuc += this.business.dataContext.NewDatabase();
                }
                else if (db == "log")
                {
                    sonuc += this.business.logDataContext.NewDatabase();
                }

                return sonuc.ToString();
            }
            else
            {
                return "Token geçersiz";
            }
        }

        //https://localhost:44309/Home/Update?token=TOKEN-001-MSM
        public string Update(string token, string db = "main")
        {
            if (token == "TOKEN-001-MSM")
            {
                var sonuc = "";

                if (db == "main")
                {
                    sonuc += this.business.dataContext.MigrateDatabase();
                }
                else if (db == "log")
                {
                    sonuc += this.business.logDataContext.MigrateDatabase();
                }

                return sonuc.ToString();
            }
            else
            {
                return "Token geçersiz";
            }
        }


    }
}