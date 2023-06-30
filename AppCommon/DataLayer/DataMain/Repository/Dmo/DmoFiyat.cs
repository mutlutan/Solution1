using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AppData.Main.Repository;
using AppData.Main.Models;
using AppData.Main.Repository.Dto;
using AppCommon;

namespace AppData.Main.Repository.Dmo
{
    public class DmoFiyat : BaseDmo
    {
        public DmoFiyat(DataContext dataContext) : base(dataContext) { }

        public IQueryable<DtoFiyat> Get()
        {
            return this.dataContext.Fiyat.AsNoTracking()
                .Select(s => new DtoFiyat(this.dataContext)
                {
                    Id = s.Id,
                    UniqueId = s.UniqueId,
                    TarifeId = s.TarifeId,
                    UyeGrupId = s.UyeGrupId,
                    HaftaIciFiyat = s.HaftaIciFiyat,
                    HaftaSonuFiyat = s.HaftaSonuFiyat,
                    BayramFiyat = s.BayramFiyat,
                    CcTarifeIdAd = s.Tarife.Ad.MyToTrim(),
                    CcUyeGrupIdAd = s.UyeGrup.Ad.MyToTrim()
                });
     }

        public DtoFiyat GetByNew()
        {
            //Default değerler ile bir row döner, Burada field default değerleri veriliyor...
            DtoFiyat row = new(this.dataContext) {
                Id = 0,
                UniqueId = Guid.NewGuid(),
                TarifeId = 0,
                UyeGrupId = 0,
                HaftaIciFiyat = 0,
                HaftaSonuFiyat = 0,
                BayramFiyat = 0
            };

            return row;
        }

        public IEnumerable<DtoFiyat> GetById(Int32 _id)
        {
            return this.Get().Where(c => c.Id == _id);
        }

        public Int32 CreateOrUpdate(Fiyat _model, Boolean isNew)
        {
            Fiyat? row;
            
            if (isNew)
            {
                //sadece insertte eklenip, update de değişmeyecek alanlar buraya
                row = new Fiyat() {
                    Id = (int)this.dataContext.GetNextSequenceValue("sqFiyat")
                };
            }
            else
            {
                row = this.dataContext.Fiyat.Where(c => c.Id == _model.Id && c.UniqueId == _model.UniqueId).FirstOrDefault();
                if (row == null)
                {
                    throw new Exception(this.dataContext.TranslateTo("xLng.IslemYapilacakKayitBulunamadi"));
                }
            }

         row.UniqueId = _model.UniqueId;
         row.TarifeId = _model.TarifeId;
         row.UyeGrupId = _model.UyeGrupId;
         row.HaftaIciFiyat = _model.HaftaIciFiyat;
         row.HaftaSonuFiyat = _model.HaftaSonuFiyat;
         row.BayramFiyat = _model.BayramFiyat;

         if (!isNew)
         {
             //sadece update eklenip, insertte de değişmeyecek alanlar buraya
         }
         
         if (isNew)
         {
             this.dataContext.Fiyat.Add(row);
         }
         
         return row.Id;
     }

     public bool Delete(Int32 _id)
     {
         Boolean rV = false;

         var row = this.dataContext.Fiyat.Where(c => c.Id == _id).FirstOrDefault();
         if (row != null)
         {
             this.dataContext.Fiyat.Remove(row);
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


