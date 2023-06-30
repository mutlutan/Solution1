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
    public class DmoSmsBildirimUye : BaseDmo
    {
        public DmoSmsBildirimUye(MainDataContext dataContext) : base(dataContext) { }

        public IQueryable<DtoSmsBildirimUye> Get()
        {
            return this.dataContext.SmsBildirimUye.AsNoTracking()
                .Select(s => new DtoSmsBildirimUye(this.dataContext)
                {
                    Id = s.Id,
                    SmsBildirimId = s.SmsBildirimId,
                    UyeId = s.UyeId,
                    CcUyeIdAd = s.Uye.Ad.MyToTrim()
                });
     }

        public DtoSmsBildirimUye GetByNew()
        {
            //Default değerler ile bir row döner, Burada field default değerleri veriliyor...
            DtoSmsBildirimUye row = new(this.dataContext) {
                Id = 0,
                SmsBildirimId = 0,
                UyeId = 0
            };

            return row;
        }

        public IEnumerable<DtoSmsBildirimUye> GetById(Int32 _id)
        {
            return this.Get().Where(c => c.Id == _id);
        }

        public Int32 CreateOrUpdate(SmsBildirimUye _model, Boolean isNew)
        {
            SmsBildirimUye? row;
            
            if (isNew)
            {
                //sadece insertte eklenip, update de değişmeyecek alanlar buraya
                row = new SmsBildirimUye() {
                    Id = (int)this.dataContext.GetNextSequenceValue("sqSmsBildirimUye")
                };
            }
            else
            {
                row = this.dataContext.SmsBildirimUye.Where(c => c.Id == _model.Id).FirstOrDefault();
                if (row == null)
                {
                    throw new Exception(this.dataContext.TranslateTo("xLng.IslemYapilacakKayitBulunamadi"));
                }
            }

         row.SmsBildirimId = _model.SmsBildirimId;
         row.UyeId = _model.UyeId;

         if (!isNew)
         {
             //sadece update eklenip, insertte de değişmeyecek alanlar buraya
         }
         
         if (isNew)
         {
             this.dataContext.SmsBildirimUye.Add(row);
         }
         
         return row.Id;
     }

     public bool Delete(Int32 _id)
     {
         Boolean rV = false;

         var row = this.dataContext.SmsBildirimUye.Where(c => c.Id == _id).FirstOrDefault();
         if (row != null)
         {
             this.dataContext.SmsBildirimUye.Remove(row);
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


