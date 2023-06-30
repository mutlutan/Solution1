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
    public class DmoMobilBildirim : BaseDmo
    {
        public DmoMobilBildirim(DataContext dataContext) : base(dataContext) { }

        public IQueryable<DtoMobilBildirim> Get()
        {
            return this.dataContext.MobilBildirim.AsNoTracking()
                .Select(s => new DtoMobilBildirim(this.dataContext)
                {
                    Id = s.Id,
                    UniqueId = s.UniqueId,
                    GonderildiMi = s.GonderildiMi,
                    Tarih = s.Tarih,
                    GonderimTarihi = s.GonderimTarihi,
                    UyeGrupIds = s.UyeGrupIds,
                    Baslik = s.Baslik,
                    Mesaj = s.Mesaj,
                    ResimUrl = s.ResimUrl,
                    Link = s.Link
                });
     }

        public DtoMobilBildirim GetByNew()
        {
            //Default değerler ile bir row döner, Burada field default değerleri veriliyor...
            DtoMobilBildirim row = new(this.dataContext) {
                Id = 0,
                UniqueId = Guid.NewGuid(),
                GonderildiMi = false
            };

            return row;
        }

        public IEnumerable<DtoMobilBildirim> GetById(Int32 _id)
        {
            return this.Get().Where(c => c.Id == _id);
        }

        public Int32 CreateOrUpdate(MobilBildirim _model, Boolean isNew)
        {
            MobilBildirim? row;
            
            if (isNew)
            {
                //sadece insertte eklenip, update de değişmeyecek alanlar buraya
                row = new MobilBildirim() {
                    Id = (int)this.dataContext.GetNextSequenceValue("sqMobilBildirim")
                };
            }
            else
            {
                row = this.dataContext.MobilBildirim.Where(c => c.Id == _model.Id && c.UniqueId == _model.UniqueId).FirstOrDefault();
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
         row.ResimUrl = _model.ResimUrl;
         row.Link = _model.Link;

         if (!isNew)
         {
             //sadece update eklenip, insertte de değişmeyecek alanlar buraya
         }
         
         if (isNew)
         {
             this.dataContext.MobilBildirim.Add(row);
         }
         
         return row.Id;
     }

     public bool Delete(Int32 _id)
     {
         Boolean rV = false;

         var row = this.dataContext.MobilBildirim.Where(c => c.Id == _id).FirstOrDefault();
         if (row != null)
         {
             this.dataContext.MobilBildirim.Remove(row);
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


