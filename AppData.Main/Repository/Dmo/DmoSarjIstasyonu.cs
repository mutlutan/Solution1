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
    public class DmoSarjIstasyonu : BaseDmo
    {
        public DmoSarjIstasyonu(DataContext dataContext) : base(dataContext) { }

        public IQueryable<DtoSarjIstasyonu> Get()
        {
            return this.dataContext.SarjIstasyonu.AsNoTracking()
                .Select(s => new DtoSarjIstasyonu(this.dataContext)
                {
                    Id = s.Id,
                    UniqueId = s.UniqueId,
                    Sira = s.Sira,
                    Durum = s.Durum,
                    Ad = s.Ad,
                    Aciklama = s.Aciklama,
                    MusaitlikDurum = s.MusaitlikDurum,
                    Konum = s.Konum,
                    YazilimVersiyon = s.YazilimVersiyon,
                    YazilimVersiyonNo = s.YazilimVersiyonNo,
                    ModelNo = s.ModelNo,
                    SeriNo = s.SeriNo,
                    CreateDate = s.CreateDate,
                    CreatedUserId = s.CreatedUserId,
                    UpdateDate = s.UpdateDate,
                    UpdatedUserId = s.UpdatedUserId
                });
     }

        public DtoSarjIstasyonu GetByNew()
        {
            //Default değerler ile bir row döner, Burada field default değerleri veriliyor...
            DtoSarjIstasyonu row = new(this.dataContext) {
                Id = 0,
                UniqueId = Guid.NewGuid(),
                Sira = 0,
                Durum = true,
                MusaitlikDurum = true,
                CreateDate = DateTime.Now,
                CreatedUserId = this.dataContext.UserId,
                UpdatedUserId = 0
            };

            return row;
        }

        public IEnumerable<DtoSarjIstasyonu> GetById(Int32 _id)
        {
            return this.Get().Where(c => c.Id == _id);
        }

        public Int32 CreateOrUpdate(SarjIstasyonu _model, Boolean isNew)
        {
            SarjIstasyonu? row;
            
            if (isNew)
            {
                //sadece insertte eklenip, update de değişmeyecek alanlar buraya
                row = new SarjIstasyonu() {
                    Id = (int)this.dataContext.GetNextSequenceValue("sqSarjIstasyonu")
                };
            }
            else
            {
                row = this.dataContext.SarjIstasyonu.Where(c => c.Id == _model.Id && c.UniqueId == _model.UniqueId).FirstOrDefault();
                if (row == null)
                {
                    throw new Exception(this.dataContext.TranslateTo("xLng.IslemYapilacakKayitBulunamadi"));
                }
            }

         row.UniqueId = _model.UniqueId;
         row.Sira = _model.Sira;
         row.Durum = _model.Durum;
         row.Ad = _model.Ad;
         row.Aciklama = _model.Aciklama;
         row.MusaitlikDurum = _model.MusaitlikDurum;
         row.Konum = _model.Konum;
         row.YazilimVersiyon = _model.YazilimVersiyon;
         row.YazilimVersiyonNo = _model.YazilimVersiyonNo;
         row.ModelNo = _model.ModelNo;
         row.SeriNo = _model.SeriNo;
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
             this.dataContext.SarjIstasyonu.Add(row);
         }
         
         return row.Id;
     }

     public bool Delete(Int32 _id)
     {
         Boolean rV = false;

         var row = this.dataContext.SarjIstasyonu.Where(c => c.Id == _id).FirstOrDefault();
         if (row != null)
         {
             this.dataContext.SarjIstasyonu.Remove(row);
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


