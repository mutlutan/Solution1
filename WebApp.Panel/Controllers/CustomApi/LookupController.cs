using AppCommon;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using WebApp.Panel.Codes;
using Telerik.DataSource;


namespace WebApp.Panel.Controllers
{
	[Route("Panel/[controller]")]
    [ApiController]
    public class LookupController : BaseController
    {
        public LookupController(IServiceProvider _serviceProvider) : base(_serviceProvider) { }

        //lookup,dropdown,combo apileri buraya yazýlýr

        #region lookup

        [HttpPost("Read")]
        [ResponseCache(Duration = 0)]
        [AuthenticateRequired]
        public IActionResult Read([FromBody] LookupRequest request)
        {
            DataSourceResult dsr = new();

            try
            {
                var lookupResult = business.GetLookupRead(this.business.UserToken.Culture, request);
                dsr.Data = lookupResult;
                dsr.Total = lookupResult.Count();
            }
            catch (Exception ex)
            {
                dsr.Errors = ex.MyLastInner().Message;
                business.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return Json(dsr);
        }
        #endregion

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
        //[ResponseCache(Duration = 0)]
        //[AuthenticateRequired]
        //public ActionResult ReadEnmYetkiGrup()
        //{
        //    DataSourceResult dsr = new();
        //    try
        //    {
        //        var enumArray = (EnmYetkiGrup[])Enum.GetValues(typeof(EnmYetkiGrup));
        //        var data = enumArray
        //            .Select(s => new { value = (int)s, text = s.ToString() });

        //        dsr.Data = data;
        //        dsr.Total = data.Count();
        //    }
        //    catch (Exception ex)
        //    {
        //        dsr.Errors = ex.MyLastInner().Message;
        //    }
        //    return Json(dsr);
        //}

        #endregion


    }
}


