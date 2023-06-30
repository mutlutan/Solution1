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
    public class DmoAracHareket : BaseDmo
    {
        public DmoAracHareket(MainDataContext dataContext) : base(dataContext) { }

        public IQueryable<DtoAracHareket> Get()
        {
            return this.dataContext.AracHareket.AsNoTracking()
                .Select(s => new DtoAracHareket(this.dataContext)
                {
                    Id = s.Id,
                    UniqueId = s.UniqueId,
                    AracId = s.AracId,
                    UyeId = s.UyeId,
                    AracRezervasyonId = s.AracRezervasyonId,
                    BirimFiyat = s.BirimFiyat,
                    Mesafe = s.Mesafe,
                    Tutar = s.Tutar,
                    BaslangicTarih = s.BaslangicTarih,
                    BitisTarih = s.BitisTarih,
                    BaslangicKonum = s.BaslangicKonum,
                    BitisKonum = s.BitisKonum,
                    CreateDate = s.CreateDate,
                    CreatedUserId = s.CreatedUserId,
                    UpdateDate = s.UpdateDate,
                    UpdatedUserId = s.UpdatedUserId,
                    CcAracIdAd = s.Arac.Ad.MyToTrim(),
                    CcUyeIdAd = s.Uye.Ad.MyToTrim()
                });
     }

        public DtoAracHareket GetByNew()
        {
            //Default değerler ile bir row döner, Burada field default değerleri veriliyor...
            DtoAracHareket row = new(this.dataContext) {
                Id = 0,
                UniqueId = Guid.NewGuid(),
                AracId = 0,
                UyeId = 0,
                AracRezervasyonId = 0,
                BirimFiyat = 0,
                Mesafe = 0,
                Tutar = 0,
                CreateDate = DateTime.Now,
                CreatedUserId = this.dataContext.UserId,
                UpdatedUserId = 0
            };

            return row;
        }

        public IEnumerable<DtoAracHareket> GetById(Int32 _id)
        {
            return this.Get().Where(c => c.Id == _id);
        }

        public Int32 CreateOrUpdate(AracHareket _model, Boolean isNew)
        {
            AracHareket? row;
            
            if (isNew)
            {
                //sadece insertte eklenip, update de değişmeyecek alanlar buraya
                row = new AracHareket() {
                    Id = (int)this.dataContext.GetNextSequenceValue("sqAracHareket")
                };
            }
            else
            {
                row = this.dataContext.AracHareket.Where(c => c.Id == _model.Id && c.UniqueId == _model.UniqueId).FirstOrDefault();
                if (row == null)
                {
                    throw new Exception(this.dataContext.TranslateTo("xLng.IslemYapilacakKayitBulunamadi"));
                }
            }

         row.UniqueId = _model.UniqueId;
         row.AracId = _model.AracId;
         row.UyeId = _model.UyeId;
         row.AracRezervasyonId = _model.AracRezervasyonId;
         row.BirimFiyat = _model.BirimFiyat;
         row.Mesafe = _model.Mesafe;
         row.Tutar = _model.Tutar;
         row.BaslangicTarih = _model.BaslangicTarih;
         row.BitisTarih = _model.BitisTarih;
         row.BaslangicKonum = _model.BaslangicKonum;
         row.BitisKonum = _model.BitisKonum;
         row.CreateDate = _model.CreateDate;
         row.CreatedUserId = _model.CreatedUserId;
         row.UpdateDate = DateTime.Now;
         row.UpdatedUserId = this.dataContext.UserId;

         if (!isNew)
         {
             //sadece update eklenip, insertte de değişmeyecek alanlar buraya
         }
         
         if (isNew)
         {
             this.dataContext.AracHareket.Add(row);
         }
         
         return row.Id;
     }

     public bool Delete(Int32 _id)
     {
         Boolean rV = false;

         var row = this.dataContext.AracHareket.Where(c => c.Id == _id).FirstOrDefault();
         if (row != null)
         {
             this.dataContext.AracHareket.Remove(row);
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


