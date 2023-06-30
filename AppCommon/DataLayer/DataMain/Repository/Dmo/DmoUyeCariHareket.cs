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
    public class DmoUyeCariHareket : BaseDmo
    {
        public DmoUyeCariHareket(MainDataContext dataContext) : base(dataContext) { }

        public IQueryable<DtoUyeCariHareket> Get()
        {
            return this.dataContext.UyeCariHareket.AsNoTracking()
                .Select(s => new DtoUyeCariHareket(this.dataContext)
                {
                    Id = s.Id,
                    UniqueId = s.UniqueId,
                    UyeId = s.UyeId,
                    CariHareketTurId = s.CariHareketTurId,
                    Borc = s.Borc,
                    Alacak = s.Alacak,
                    Tarih = s.Tarih,
                    Aciklama = s.Aciklama,
                    CcUyeIdAd = s.Uye.Ad.MyToTrim(),
                    CcCariHareketTurIdAd = s.CariHareketTur.Ad.MyToTrim()
                });
     }

        public DtoUyeCariHareket GetByNew()
        {
            //Default değerler ile bir row döner, Burada field default değerleri veriliyor...
            DtoUyeCariHareket row = new(this.dataContext) {
                Id = 0,
                UyeId = 0,
                CariHareketTurId = 0,
                Borc = 0,
                Alacak = 0
            };

            return row;
        }

        public IEnumerable<DtoUyeCariHareket> GetById(Int32 _id)
        {
            return this.Get().Where(c => c.Id == _id);
        }

        public Int32 CreateOrUpdate(UyeCariHareket _model, Boolean isNew)
        {
            UyeCariHareket? row;
            
            if (isNew)
            {
                //sadece insertte eklenip, update de değişmeyecek alanlar buraya
                row = new UyeCariHareket() {
                    Id = (int)this.dataContext.GetNextSequenceValue("sqUyeCariHareket")
                };
            }
            else
            {
                row = this.dataContext.UyeCariHareket.Where(c => c.Id == _model.Id && c.UniqueId == _model.UniqueId).FirstOrDefault();
                if (row == null)
                {
                    throw new Exception(this.dataContext.TranslateTo("xLng.IslemYapilacakKayitBulunamadi"));
                }
            }

         row.UniqueId = _model.UniqueId;
         row.UyeId = _model.UyeId;
         row.CariHareketTurId = _model.CariHareketTurId;
         row.Borc = _model.Borc;
         row.Alacak = _model.Alacak;
         row.Tarih = _model.Tarih;
         row.Aciklama = _model.Aciklama;

         if (!isNew)
         {
             //sadece update eklenip, insertte de değişmeyecek alanlar buraya
         }
         
         if (isNew)
         {
             this.dataContext.UyeCariHareket.Add(row);
         }
         
         return row.Id;
     }

     public bool Delete(Int32 _id)
     {
         Boolean rV = false;

         var row = this.dataContext.UyeCariHareket.Where(c => c.Id == _id).FirstOrDefault();
         if (row != null)
         {
             this.dataContext.UyeCariHareket.Remove(row);
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


