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
    public class DmoUyeGrup : BaseDmo
    {
        public DmoUyeGrup(DataContext dataContext) : base(dataContext) { }

        public IQueryable<DtoUyeGrup> Get()
        {
            return this.dataContext.UyeGrup.AsNoTracking()
                .Select(s => new DtoUyeGrup(this.dataContext)
                {
                    Id = s.Id,
                    UniqueId = s.UniqueId,
                    Durum = s.Durum,
                    Ad = s.Ad
                });
     }

        public DtoUyeGrup GetByNew()
        {
            //Default değerler ile bir row döner, Burada field default değerleri veriliyor...
            DtoUyeGrup row = new(this.dataContext) {
                Id = 0,
                UniqueId = Guid.NewGuid(),
                Durum = true
            };

            return row;
        }

        public IEnumerable<DtoUyeGrup> GetById(Int32 _id)
        {
            return this.Get().Where(c => c.Id == _id);
        }

        public Int32 CreateOrUpdate(UyeGrup _model, Boolean isNew)
        {
            UyeGrup? row;
            
            if (isNew)
            {
                //sadece insertte eklenip, update de değişmeyecek alanlar buraya
                row = new UyeGrup() {
                    Id = (int)this.dataContext.GetNextSequenceValue("sqUyeGrup")
                };
            }
            else
            {
                row = this.dataContext.UyeGrup.Where(c => c.Id == _model.Id && c.UniqueId == _model.UniqueId).FirstOrDefault();
                if (row == null)
                {
                    throw new Exception(this.dataContext.TranslateTo("xLng.IslemYapilacakKayitBulunamadi"));
                }
            }

         row.UniqueId = _model.UniqueId;
         row.Durum = _model.Durum;
         row.Ad = _model.Ad;

         if (!isNew)
         {
             //sadece update eklenip, insertte de değişmeyecek alanlar buraya
         }
         
         if (isNew)
         {
             this.dataContext.UyeGrup.Add(row);
         }
         
         return row.Id;
     }

     public bool Delete(Int32 _id)
     {
         Boolean rV = false;

         var row = this.dataContext.UyeGrup.Where(c => c.Id == _id).FirstOrDefault();
         if (row != null)
         {
             this.dataContext.UyeGrup.Remove(row);
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


