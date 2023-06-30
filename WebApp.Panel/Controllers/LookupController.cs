using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using System.Reflection;
using WebApp.Panel.Codes;
using Telerik.DataSource;
using Telerik.DataSource.Extensions;
using AppCommon;

namespace WebApp.Panel.Controllers
{
    public class LookupController : BaseController
    {
        public LookupController(IServiceProvider _serviceProvider) : base(_serviceProvider) { }

        [HttpPost]
        [ResponseCache(Duration = 0)]
        [AuthenticateRequired]
        public ActionResult Read(LookupRequest request)
        {
            DataSourceResult dsr = new();
            try
            {
                var lookupResult = this.business.GetLookupRead(this.business.UserToken.Culture, request);

                dsr.Data = lookupResult;
                dsr.Total = lookupResult.Count();
            }
            catch (Exception ex)
            {
                dsr.Errors = ex.MyLastInner().Message;
                this.business.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return Json(dsr);
        }

        #region Ulke Sehir Ilce lookup

        //[ResponseCache(Duration = 0)]
        //[AuthenticateRequired]
        //public ActionResult ReadSehirIlce()
        //{
        //    DataSourceResult dsr = new();
        //    try
        //    {
        //        var queryResult = this.cmsBusiness.repository.dataContext.Ilce
        //            .OrderBy(o => o.Ad)
        //            .Select(s => new { value = s.Id, text = s.Id > 0 ? s.Ad.MyToTrim() + " / " + s.Sehir.Ad.MyToTrim() : "" });

        //        dsr.Data = queryResult;
        //        dsr.Total = queryResult.Count();
        //    }
        //    catch (Exception ex)
        //    {
        //        dsr.Errors = ex.MyLastInner().Message;
        //    }
        //    return Json(dsr);
        //}

        #endregion


        #region enum listeleri 
        [ResponseCache(Duration = 0)]
        [AuthenticateRequired]
        public ActionResult ReadEnmYetkiGrup()
        {
            DataSourceResult dsr = new();
            try
            {
                var enumArray = (EnmYetkiGrup[])Enum.GetValues(typeof(EnmYetkiGrup));
                var data = enumArray
                    .Select(s => new { value = (int)s, text = s.ToString() });

                dsr.Data = data;
                dsr.Total = data.Count();
            }
            catch (Exception ex)
            {
                dsr.Errors = ex.MyLastInner().Message;
            }
            return Json(dsr);
        }

        #endregion

    }
}


