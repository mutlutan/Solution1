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
    public class DmoMobilBildirimUye : BaseDmo
    {
        public DmoMobilBildirimUye(MainDataContext dataContext) : base(dataContext) { }

        public IQueryable<DtoMobilBildirimUye> Get()
        {
            return this.dataContext.MobilBildirimUye.AsNoTracking()
                .Select(s => new DtoMobilBildirimUye(this.dataContext)
                {
                    Id = s.Id,
                    MobilBildirimId = s.MobilBildirimId,
                    UyeId = s.UyeId,
                    CcUyeIdAd = s.Uye.Ad.MyToTrim()
                });
     }

        public DtoMobilBildirimUye GetByNew()
        {
            //Default değerler ile bir row döner, Burada field default değerleri veriliyor...
            DtoMobilBildirimUye row = new(this.dataContext) {
                Id = 0,
                MobilBildirimId = 0,
                UyeId = 0
            };

            return row;
        }

        public IEnumerable<DtoMobilBildirimUye> GetById(Int32 _id)
        {
            return this.Get().Where(c => c.Id == _id);
        }

        public Int32 CreateOrUpdate(MobilBildirimUye _model, Boolean isNew)
        {
            MobilBildirimUye? row;
            
            if (isNew)
            {
                //sadece insertte eklenip, update de değişmeyecek alanlar buraya
                row = new MobilBildirimUye() {
                    Id = (int)this.dataContext.GetNextSequenceValue("sqMobilBildirimUye")
                };
            }
            else
            {
                row = this.dataContext.MobilBildirimUye.Where(c => c.Id == _model.Id).FirstOrDefault();
                if (row == null)
                {
                    throw new Exception(this.dataContext.TranslateTo("xLng.IslemYapilacakKayitBulunamadi"));
                }
            }

         row.MobilBildirimId = _model.MobilBildirimId;
         row.UyeId = _model.UyeId;

         if (!isNew)
         {
             //sadece update eklenip, insertte de değişmeyecek alanlar buraya
         }
         
         if (isNew)
         {
             this.dataContext.MobilBildirimUye.Add(row);
         }
         
         return row.Id;
     }

     public bool Delete(Int32 _id)
     {
         Boolean rV = false;

         var row = this.dataContext.MobilBildirimUye.Where(c => c.Id == _id).FirstOrDefault();
         if (row != null)
         {
             this.dataContext.MobilBildirimUye.Remove(row);
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


