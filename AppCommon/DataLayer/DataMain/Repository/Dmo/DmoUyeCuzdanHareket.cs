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
    public class DmoUyeCuzdanHareket : BaseDmo
    {
        public DmoUyeCuzdanHareket(MainDataContext dataContext) : base(dataContext) { }

        public IQueryable<DtoUyeCuzdanHareket> Get()
        {
            return this.dataContext.UyeCuzdanHareket.AsNoTracking()
                .Select(s => new DtoUyeCuzdanHareket(this.dataContext)
                {
                    Id = s.Id,
                    UniqueId = s.UniqueId,
                    UyeId = s.UyeId,
                    CuzdanHareketTurId = s.CuzdanHareketTurId,
                    Borc = s.Borc,
                    Alacak = s.Alacak,
                    Tarih = s.Tarih,
                    Aciklama = s.Aciklama,
                    CcUyeIdAd = s.Uye.Ad.MyToTrim(),
                    CcCuzdanHareketTurIdAd = s.CuzdanHareketTur.Ad.MyToTrim()
                });
     }

        public DtoUyeCuzdanHareket GetByNew()
        {
            //Default değerler ile bir row döner, Burada field default değerleri veriliyor...
            DtoUyeCuzdanHareket row = new(this.dataContext) {
                Id = 0,
                UyeId = 0,
                CuzdanHareketTurId = 0,
                Borc = 0,
                Alacak = 0
            };

            return row;
        }

        public IEnumerable<DtoUyeCuzdanHareket> GetById(Int32 _id)
        {
            return this.Get().Where(c => c.Id == _id);
        }

        public Int32 CreateOrUpdate(UyeCuzdanHareket _model, Boolean isNew)
        {
            UyeCuzdanHareket? row;
            
            if (isNew)
            {
                //sadece insertte eklenip, update de değişmeyecek alanlar buraya
                row = new UyeCuzdanHareket() {
                    Id = (int)this.dataContext.GetNextSequenceValue("sqUyeCuzdanHareket")
                };
            }
            else
            {
                row = this.dataContext.UyeCuzdanHareket.Where(c => c.Id == _model.Id && c.UniqueId == _model.UniqueId).FirstOrDefault();
                if (row == null)
                {
                    throw new Exception(this.dataContext.TranslateTo("xLng.IslemYapilacakKayitBulunamadi"));
                }
            }

         row.UniqueId = _model.UniqueId;
         row.UyeId = _model.UyeId;
         row.CuzdanHareketTurId = _model.CuzdanHareketTurId;
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
             this.dataContext.UyeCuzdanHareket.Add(row);
         }
         
         return row.Id;
     }

     public bool Delete(Int32 _id)
     {
         Boolean rV = false;

         var row = this.dataContext.UyeCuzdanHareket.Where(c => c.Id == _id).FirstOrDefault();
         if (row != null)
         {
             this.dataContext.UyeCuzdanHareket.Remove(row);
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


