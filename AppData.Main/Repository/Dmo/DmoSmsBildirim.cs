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
    public class DmoSmsBildirim : BaseDmo
    {
        public DmoSmsBildirim(DataContext dataContext) : base(dataContext) { }

        public IQueryable<DtoSmsBildirim> Get()
        {
            return this.dataContext.SmsBildirim.AsNoTracking()
                .Select(s => new DtoSmsBildirim(this.dataContext)
                {
                    Id = s.Id,
                    UniqueId = s.UniqueId,
                    GonderildiMi = s.GonderildiMi,
                    Tarih = s.Tarih,
                    GonderimTarihi = s.GonderimTarihi,
                    UyeGrupIds = s.UyeGrupIds,
                    Baslik = s.Baslik,
                    Mesaj = s.Mesaj
                });
     }

        public DtoSmsBildirim GetByNew()
        {
            //Default değerler ile bir row döner, Burada field default değerleri veriliyor...
            DtoSmsBildirim row = new(this.dataContext) {
                Id = 0,
                GonderildiMi = true
            };

            return row;
        }

        public IEnumerable<DtoSmsBildirim> GetById(Int32 _id)
        {
            return this.Get().Where(c => c.Id == _id);
        }

        public Int32 CreateOrUpdate(SmsBildirim _model, Boolean isNew)
        {
            SmsBildirim? row;
            
            if (isNew)
            {
                //sadece insertte eklenip, update de değişmeyecek alanlar buraya
                row = new SmsBildirim() {
                    Id = (int)this.dataContext.GetNextSequenceValue("sqSmsBildirim")
                };
            }
            else
            {
                row = this.dataContext.SmsBildirim.Where(c => c.Id == _model.Id && c.UniqueId == _model.UniqueId).FirstOrDefault();
                if (row == null)
                {
                    throw new Exception(this.dataContext.TranslateTo("xLng.IslemYapilacakKayitBulunamadi"));
                }
            }

         row.UniqueId = _model.UniqueId;
         row.GonderildiMi = _model.GonderildiMi;
         row.Tarih = _model.Tarih;
         row.GonderimTarihi = _model.GonderimTarihi;
         row.UyeGrupIds = _model.UyeGrupIds;
         row.Baslik = _model.Baslik;
         row.Mesaj = _model.Mesaj;

         if (!isNew)
         {
             //sadece update eklenip, insertte de değişmeyecek alanlar buraya
         }
         
         if (isNew)
         {
             this.dataContext.SmsBildirim.Add(row);
         }
         
         return row.Id;
     }

     public bool Delete(Int32 _id)
     {
         Boolean rV = false;

         var row = this.dataContext.SmsBildirim.Where(c => c.Id == _id).FirstOrDefault();
         if (row != null)
         {
             this.dataContext.SmsBildirim.Remove(row);
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


