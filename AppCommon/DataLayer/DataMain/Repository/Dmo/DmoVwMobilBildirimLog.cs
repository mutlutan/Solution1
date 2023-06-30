using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AppCommon;
using AppData.Main.Repository;
using AppData.Main.Models;
using AppData.Main.Repository.Dto;

namespace AppData.Main.Repository.Dmo
{
    public class DmoVwMobilBildirimLog : BaseDmo
    {
        public DmoVwMobilBildirimLog(DataContext dataContext) : base(dataContext) { }

        public IQueryable<DtoVwMobilBildirimLog> Get()
        {
            return this.dataContext.VwMobilBildirimLog.AsNoTracking()
                .Select(s => new DtoVwMobilBildirimLog(this.dataContext)
                {
                    Id = s.Id,
                    MobilBildirimId = s.MobilBildirimId,
                    Durum = s.Durum,
                    Tarih = s.Tarih,
                    MesajData = s.MesajData,
                    ResponseData = s.ResponseData
                });
     }

        public DtoVwMobilBildirimLog GetByNew()
        {
            //Default değerler ile bir row döner, Burada field default değerleri veriliyor...
            DtoVwMobilBildirimLog row = new(this.dataContext) {
                MobilBildirimId = 0,
                Durum = true
            };

            return row;
        }

        public IEnumerable<DtoVwMobilBildirimLog> GetById(Guid _id)
        {
            return this.Get().Where(c => c.Id == _id);
        }

        public Guid CreateOrUpdate(VwMobilBildirimLog _model, Boolean isNew)
        {
            VwMobilBildirimLog? row;
            
            if (isNew)
            {
                //sadece insertte eklenip, update de değişmeyecek alanlar buraya
                row = new VwMobilBildirimLog() {
                    Id = Guid.NewGuid()
                };
            }
            else
            {
                row = this.dataContext.VwMobilBildirimLog.Where(c => c.Id == _model.Id).FirstOrDefault();
                if (row == null)
                {
                    throw new Exception(this.dataContext.TranslateTo("xLng.IslemYapilacakKayitBulunamadi"));
                }
            }

         row.Id = _model.Id;
         row.MobilBildirimId = _model.MobilBildirimId;
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
             this.dataContext.VwMobilBildirimLog.Add(row);
         }
         
         return row.Id;
     }

     public bool Delete(Guid _id)
     {
         Boolean rV = false;

         var row = this.dataContext.VwMobilBildirimLog.Where(c => c.Id == _id).FirstOrDefault();
         if (row != null)
         {
             this.dataContext.VwMobilBildirimLog.Remove(row);
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


