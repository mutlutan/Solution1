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
    public class ApiParameterController : BaseController
    {
        public ApiParameterController(IServiceProvider _serviceProvider) : base(_serviceProvider) { }

        [HttpPost("Read")]
        [ResponseCache(Duration = 0)]
        [AuthenticateRequired(AuthorityKeys = "Parameter.D_R.")]
        public ActionResult Read([FromBody]ApiRequest request)
        {
            DataSourceResult dsr = new();
            try
            {
                var query = this.business.repository.RepParameter.Get();
                query = query.Where(c => c.Id > 0);
                dsr = query.ToDataSourceResult(request.MyToDataSourceRequest());
            }
            catch (Exception ex)
            {
                dsr.Errors = ex.MyLastInner().Message;
            }
            return Json(dsr);
        }

        [HttpGet("GetByNew")]
        [ResponseCache(Duration = 0)]
        [AuthenticateRequired(AuthorityKeys = "Parameter.D_C.")]
        public ActionResult GetByNew()
        {
             return Json(this.business.repository.RepParameter.GetByNew());
        }

        [HttpPost("Create")]
        [ResponseCache(Duration = 0)]
        [AuthenticateRequired(AuthorityKeys = "Parameter.D_C.")]
        public ActionResult Create([FromBody] object obj)
        {
            DataSourceResult dsr = new();
            try
            {
                var model = new NetTopologySuite.IO.GeoJsonReader().Read<Parameter>(obj.MyToStr());
                if (model != null)
                {
                    Int32 id = this.business.repository.RepParameter.CreateOrUpdate(model, true);
                    this.business.repository.SaveChanges();
                    dsr.Data = this.business.repository.RepParameter.GetById(id);
                }
                else
                {
                    dsr.Errors = this.business.dataContext.TranslateTo("xLng.VeriModeliDonusturulemedi");
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
        [AuthenticateRequired(AuthorityKeys = "Parameter.D_U.")]
        public ActionResult Update([FromBody] object obj)
        {
            DataSourceResult dsr = new();
            try
            {
                var model = new NetTopologySuite.IO.GeoJsonReader().Read<Parameter>(obj.MyToStr());
                if (model != null)
                {
                    Int32 id = this.business.repository.RepParameter.CreateOrUpdate(model, false);
                    this.business.repository.SaveChanges();
                    dsr.Data = this.business.repository.RepParameter.GetById(id);
                }
                else
                {
                    dsr.Errors = this.business.dataContext.TranslateTo("xLng.VeriModeliDonusturulemedi");
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
        [AuthenticateRequired(AuthorityKeys = "Parameter.D_D.")]
        public ActionResult Delete([FromBody] object obj)
        {
            DataSourceResult dsr = new();
            try
            {
                var model = Newtonsoft.Json.JsonConvert.DeserializeObject<Parameter>(obj.MyToStr());
                if (model != null)
                {
                    this.business.repository.RepParameter.Delete(model.Id);
                    this.business.repository.SaveChanges();
                }
                else
                {
                    dsr.Errors = this.business.dataContext.TranslateTo("xLng.VeriModeliDonusturulemedi");
                }

            }
            catch (Exception ex)
            {
                if (ex.Source == "Microsoft.EntityFrameworkCore.Relational")
                {
                    dsr.Errors = this.business.dataContext.TranslateTo("xLng.BaglantiliKayitVarSilinemez");
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


