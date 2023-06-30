using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AppCommon;
using AppCommon.DataLayer.DataMain.Repository;
using AppCommon.DataLayer.DataMain.Models;
using AppCommon.DataLayer.DataMain.Repository.Dto;

namespace AppCommon.DataLayer.DataMain.Repository.Dmo
{
    public class DmoVwAracStatuLog : BaseDmo
    {
        public DmoVwAracStatuLog(MainDataContext dataContext) : base(dataContext) { }

        public IQueryable<DtoVwAracStatuLog> Get()
        {
            return this.dataContext.VwAracStatuLog.AsNoTracking()
                .Select(s => new DtoVwAracStatuLog(this.dataContext)
                {
                    Id = s.Id,
                    ImeiNo = s.ImeiNo,
                    ReportId = s.ReportId,
                    Durum = s.Durum,
                    Tarih = s.Tarih,
                    RequestData = s.RequestData,
                    ResponseData = s.ResponseData
                });
     }

        public DtoVwAracStatuLog GetByNew()
        {
            //Default değerler ile bir row döner, Burada field default değerleri veriliyor...
            DtoVwAracStatuLog row = new(this.dataContext) {
                ReportId = 0,
                Durum = false
            };

            return row;
        }

        public IEnumerable<DtoVwAracStatuLog> GetById(Guid _id)
        {
            return this.Get().Where(c => c.Id == _id);
        }

        public Guid CreateOrUpdate(VwAracStatuLog _model, Boolean isNew)
        {
            VwAracStatuLog? row;
            
            if (isNew)
            {
                //sadece insertte eklenip, update de değişmeyecek alanlar buraya
                row = new VwAracStatuLog() {
                    Id = Guid.NewGuid()
                };
            }
            else
            {
                row = this.dataContext.VwAracStatuLog.Where(c => c.Id == _model.Id).FirstOrDefault();
                if (row == null)
                {
                    throw new Exception(this.dataContext.TranslateTo("xLng.IslemYapilacakKayitBulunamadi"));
                }
            }

         row.Id = _model.Id;
         row.ImeiNo = _model.ImeiNo;
         row.ReportId = _model.ReportId;
         row.Durum = _model.Durum;
         row.Tarih = _model.Tarih;
         row.RequestData = _model.RequestData;
         row.ResponseData = _model.ResponseData;

         if (!isNew)
         {
             //sadece update eklenip, insertte de değişmeyecek alanlar buraya
         }
         
         if (isNew)
         {
             this.dataContext.VwAracStatuLog.Add(row);
         }
         
         return row.Id;
     }

     public bool Delete(Guid _id)
     {
         Boolean rV = false;

         var row = this.dataContext.VwAracStatuLog.Where(c => c.Id == _id).FirstOrDefault();
         if (row != null)
         {
             this.dataContext.VwAracStatuLog.Remove(row);
             rV = true;
         }
         else
         {
             throw new Exception(this.dataContext.TranslateTo("xLng.IslemYapilacakKayitBulunamadi"));
         }

         return rV;
     }

     #region Ek fonksiyonlar

     #endregion

 }

}


