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
    public class DmoUyeKaraListe : BaseDmo
    {
        public DmoUyeKaraListe(MainDataContext dataContext) : base(dataContext) { }

        public IQueryable<DtoUyeKaraListe> Get()
        {
            return this.dataContext.UyeKaraListe.AsNoTracking()
                .Select(s => new DtoUyeKaraListe(this.dataContext)
                {
                    Id = s.Id,
                    UniqueId = s.UniqueId,
                    UyeId = s.UyeId,
                    BaslangicTarih = s.BaslangicTarih,
                    BitisTarih = s.BitisTarih,
                    Aciklama = s.Aciklama,
                    CreateDate = s.CreateDate,
                    CreatedUserId = s.CreatedUserId,
                    UpdateDate = s.UpdateDate,
                    UpdatedUserId = s.UpdatedUserId,
                    CcUyeIdAd = s.Uye.Ad.MyToTrim()
                });
     }

        public DtoUyeKaraListe GetByNew()
        {
            //Default değerler ile bir row döner, Burada field default değerleri veriliyor...
            DtoUyeKaraListe row = new(this.dataContext) {
                Id = 0,
                UniqueId = Guid.NewGuid(),
                UyeId = 0,
                CreateDate = DateTime.Now,
                CreatedUserId = this.dataContext.UserId,
                UpdatedUserId = 0
            };

            return row;
        }

        public IEnumerable<DtoUyeKaraListe> GetById(Int32 _id)
        {
            return this.Get().Where(c => c.Id == _id);
        }

        public Int32 CreateOrUpdate(UyeKaraListe _model, Boolean isNew)
        {
            UyeKaraListe? row;
            
            if (isNew)
            {
                //sadece insertte eklenip, update de değişmeyecek alanlar buraya
                row = new UyeKaraListe() {
                    Id = (int)this.dataContext.GetNextSequenceValue("sqUyeKaraListe")
                };
            }
            else
            {
                row = this.dataContext.UyeKaraListe.Where(c => c.Id == _model.Id && c.UniqueId == _model.UniqueId).FirstOrDefault();
                if (row == null)
                {
                    throw new Exception(this.dataContext.TranslateTo("xLng.IslemYapilacakKayitBulunamadi"));
                }
            }

         row.UniqueId = _model.UniqueId;
         row.UyeId = _model.UyeId;
         row.BaslangicTarih = _model.BaslangicTarih;
         row.BitisTarih = _model.BitisTarih;
         row.Aciklama = _model.Aciklama;
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
             this.dataContext.UyeKaraListe.Add(row);
         }
         
         return row.Id;
     }

     public bool Delete(Int32 _id)
     {
         Boolean rV = false;

         var row = this.dataContext.UyeKaraListe.Where(c => c.Id == _id).FirstOrDefault();
         if (row != null)
         {
             this.dataContext.UyeKaraListe.Remove(row);
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


