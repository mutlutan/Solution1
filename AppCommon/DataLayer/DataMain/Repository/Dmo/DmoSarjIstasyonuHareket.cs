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
    public class DmoSarjIstasyonuHareket : BaseDmo
    {
        public DmoSarjIstasyonuHareket(MainDataContext dataContext) : base(dataContext) { }

        public IQueryable<DtoSarjIstasyonuHareket> Get()
        {
            return this.dataContext.SarjIstasyonuHareket.AsNoTracking()
                .Select(s => new DtoSarjIstasyonuHareket(this.dataContext)
                {
                    Id = s.Id,
                    UniqueId = s.UniqueId,
                    SarjIstasyonuId = s.SarjIstasyonuId,
                    AracId = s.AracId,
                    BaslangicTarih = s.BaslangicTarih,
                    BitisTarih = s.BitisTarih,
                    CreateDate = s.CreateDate,
                    CcSarjIstasyonuIdAd = s.SarjIstasyonu.Ad.MyToTrim(),
                    CcAracIdAd = s.Arac.Ad.MyToTrim()
                });
     }

        public DtoSarjIstasyonuHareket GetByNew()
        {
            //Default değerler ile bir row döner, Burada field default değerleri veriliyor...
            DtoSarjIstasyonuHareket row = new(this.dataContext) {
                Id = 0,
                SarjIstasyonuId = 0,
                AracId = 0,
                CreateDate = DateTime.Now
            };

            return row;
        }

        public IEnumerable<DtoSarjIstasyonuHareket> GetById(Int32 _id)
        {
            return this.Get().Where(c => c.Id == _id);
        }

        public Int32 CreateOrUpdate(SarjIstasyonuHareket _model, Boolean isNew)
        {
            SarjIstasyonuHareket? row;
            
            if (isNew)
            {
                //sadece insertte eklenip, update de değişmeyecek alanlar buraya
                row = new SarjIstasyonuHareket() {
                    Id = (int)this.dataContext.GetNextSequenceValue("sqSarjIstasyonuHareket")
                };
            }
            else
            {
                row = this.dataContext.SarjIstasyonuHareket.Where(c => c.Id == _model.Id && c.UniqueId == _model.UniqueId).FirstOrDefault();
                if (row == null)
                {
                    throw new Exception(this.dataContext.TranslateTo("xLng.IslemYapilacakKayitBulunamadi"));
                }
            }

         row.UniqueId = _model.UniqueId;
         row.SarjIstasyonuId = _model.SarjIstasyonuId;
         row.AracId = _model.AracId;
         row.BaslangicTarih = _model.BaslangicTarih;
         row.BitisTarih = _model.BitisTarih;
         row.CreateDate = _model.CreateDate;

         if (!isNew)
         {
             //sadece update eklenip, insertte de değişmeyecek alanlar buraya
         }
         
         if (isNew)
         {
             this.dataContext.SarjIstasyonuHareket.Add(row);
         }
         
         return row.Id;
     }

     public bool Delete(Int32 _id)
     {
         Boolean rV = false;

         var row = this.dataContext.SarjIstasyonuHareket.Where(c => c.Id == _id).FirstOrDefault();
         if (row != null)
         {
             this.dataContext.SarjIstasyonuHareket.Remove(row);
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


