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
    public class DmoVwSmsLog : BaseDmo
    {
        public DmoVwSmsLog(MainDataContext dataContext) : base(dataContext) { }

        public IQueryable<DtoVwSmsLog> Get()
        {
            return this.dataContext.VwSmsLog.AsNoTracking()
                .Select(s => new DtoVwSmsLog(this.dataContext)
                {
                    Id = s.Id,
                    SmsBildirimId = s.SmsBildirimId,
                    Durum = s.Durum,
                    Tarih = s.Tarih,
                    MesajData = s.MesajData,
                    ResponseData = s.ResponseData
                });
     }

        public DtoVwSmsLog GetByNew()
        {
            //Default değerler ile bir row döner, Burada field default değerleri veriliyor...
            DtoVwSmsLog row = new(this.dataContext) {
                SmsBildirimId = 0,
                Durum = true
            };

            return row;
        }

        public IEnumerable<DtoVwSmsLog> GetById(Guid _id)
        {
            return this.Get().Where(c => c.Id == _id);
        }

        public Guid CreateOrUpdate(VwSmsLog _model, Boolean isNew)
        {
            VwSmsLog? row;
            
            if (isNew)
            {
                //sadece insertte eklenip, update de değişmeyecek alanlar buraya
                row = new VwSmsLog() {
                    Id = Guid.NewGuid()
                };
            }
            else
            {
                row = this.dataContext.VwSmsLog.Where(c => c.Id == _model.Id).FirstOrDefault();
                if (row == null)
                {
                    throw new Exception(this.dataContext.TranslateTo("xLng.IslemYapilacakKayitBulunamadi"));
                }
            }

         row.Id = _model.Id;
         row.SmsBildirimId = _model.SmsBildirimId;
         row.Durum = _model.Durum;
         row.Tarih = _model.Tarih;
         row.MesajData = _model.MesajData;
         row.ResponseData = _model.ResponseData;

         if (!isNew)
         {
             //sadece update eklenip, insertte de değişmeyecek alanlar buraya
         }
         
         if (isNew)
         {
             this.dataContext.VwSmsLog.Add(row);
         }
         
         return row.Id;
     }

     public bool Delete(Guid _id)
     {
         Boolean rV = false;

         var row = this.dataContext.VwSmsLog.Where(c => c.Id == _id).FirstOrDefault();
         if (row != null)
         {
             this.dataContext.VwSmsLog.Remove(row);
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


