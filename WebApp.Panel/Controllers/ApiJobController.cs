using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using AppCommon;
using WebApp.Panel.Codes;
using AppCommon.DataLayer.DataMain.Models;
using Telerik.DataSource;
using Telerik.DataSource.Extensions;

namespace WebApp.Panel.Controllers
{
    [Route("Panel/[controller]")]
    [ApiController]
    public class ApiJobController : BaseController
    {
        public ApiJobController(IServiceProvider _serviceProvider) : base(_serviceProvider) { }

        [HttpPost("Read")]
        [ResponseCache(Duration = 0)]
        [AuthenticateRequired(AuthorityKeys = "Job.D_R.")]
        public ActionResult Read([FromBody]ApiRequest request)
        {
            DataSourceResult dsr = new();
            try
            {
                var query = this.business.repository.RepJob.Get();
                dsr = query.ToDataSourceResult(this.business.ApiRequestToDataSourceRequest(request));
                //Yetkide görebileceği sütunlar döner sadece
                //dsr.Data = this.business.GetAuthorityColumnsAndData(this.userToken, "Job", dsr.Data.ToDynamicList());
            }
            catch (Exception ex)
            {
                dsr.Errors = ex.MyLastInner().Message;
            }
            return Json(dsr);
        }

        [HttpGet("GetByNew")]
        [ResponseCache(Duration = 0)]
        [AuthenticateRequired(AuthorityKeys = "Job.D_C.")]
        public ActionResult GetByNew()
        {
             return Json(this.business.repository.RepJob.GetByNew());
        }

        [HttpPost("Create")]
        [ResponseCache(Duration = 0)]
        [AuthenticateRequired(AuthorityKeys = "Job.D_C.")]
        public ActionResult Create([FromBody] object obj)
        {
            DataSourceResult dsr = new();
            try
            {
                var model = Newtonsoft.Json.JsonConvert.DeserializeObject<Job>(obj.MyToStr());
                if (model != null)
                {
                    Guid id = this.business.repository.RepJob.CreateOrUpdate(model, true);
                    this.business.repository.SaveChanges();
                    dsr.Data = this.business.repository.RepJob.GetById(id);
                }
                else
                {
                    dsr.Errors = this.business.repository.dataContext.TranslateTo("xLng.VeriModeliDonusturulemedi");
                }

            }
            catch (Exception ex)
            {
                dsr.Errors = ex.MyLastInner().Message;
            }
            
            return Json(dsr);
        }

        [HttpPost("Update")]
        [ResponseCache(Duration = 0)]
        [AuthenticateRequired(AuthorityKeys = "Job.D_U.")]
        public ActionResult Update([FromBody] object obj)
        {
            DataSourceResult dsr = new();
            try
            {
                var model = Newtonsoft.Json.JsonConvert.DeserializeObject<Job>(obj.MyToStr());
                if (model != null)
                {
                    Guid id = this.business.repository.RepJob.CreateOrUpdate(model, false);
                    this.business.repository.SaveChanges();
                    dsr.Data = this.business.repository.RepJob.GetById(id);
                }
                else
                {
                    dsr.Errors = this.business.repository.dataContext.TranslateTo("xLng.VeriModeliDonusturulemedi");
                }

            }
            catch (Exception ex)
            {
                dsr.Errors = ex.MyLastInner().Message;
            }
            
            return Json(dsr);
        }

        [HttpPost("Delete")]
        [ResponseCache(Duration = 0)]
        [AuthenticateRequired(AuthorityKeys = "Job.D_D.")]
        public ActionResult Delete([FromBody] object obj)
        {
            DataSourceResult dsr = new();
            try
            {
                var model = Newtonsoft.Json.JsonConvert.DeserializeObject<Job>(obj.MyToStr());
                if (model != null)
                {
                    this.business.repository.RepJob.Delete(model.Id);
                    this.business.repository.SaveChanges();
                }
                else
                {
                    dsr.Errors = this.business.repository.dataContext.TranslateTo("xLng.VeriModeliDonusturulemedi");
                }

            }
            catch (Exception ex)
            {
                if (ex.Source == "Microsoft.EntityFrameworkCore.Relational")
                {
                    dsr.Errors = this.business.repository.dataContext.TranslateTo("xLng.BaglantiliKayitVarSilinemez");
                }
                else
                {
                    dsr.Errors = ex.MyLastInner().Message;
                }
            }
            return Json(dsr);
        }


    }
}


