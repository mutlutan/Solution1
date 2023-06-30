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
    public class DmoTarife : BaseDmo
    {
        public DmoTarife(MainDataContext dataContext) : base(dataContext) { }

        public IQueryable<DtoTarife> Get()
        {
            return this.dataContext.Tarife.AsNoTracking()
                .Select(s => new DtoTarife(this.dataContext)
                {
                    Id = s.Id,
                    UniqueId = s.UniqueId,
                    Durum = s.Durum,
                    BaslangicTarihi = s.BaslangicTarihi,
                    Ad = s.Ad
                });
     }

        public DtoTarife GetByNew()
        {
            //Default değerler ile bir row döner, Burada field default değerleri veriliyor...
            DtoTarife row = new(this.dataContext) {
                Id = 0,
                UniqueId = Guid.NewGuid(),
                Durum = true
            };

            return row;
        }

        public IEnumerable<DtoTarife> GetById(Int32 _id)
        {
            return this.Get().Where(c => c.Id == _id);
        }

        public Int32 CreateOrUpdate(Tarife _model, Boolean isNew)
        {
            Tarife? row;
            
            if (isNew)
            {
                //sadece insertte eklenip, update de değişmeyecek alanlar buraya
                row = new Tarife() {
                    Id = (int)this.dataContext.GetNextSequenceValue("sqTarife")
                };
            }
            else
            {
                row = this.dataContext.Tarife.Where(c => c.Id == _model.Id && c.UniqueId == _model.UniqueId).FirstOrDefault();
                if (row == null)
                {
                    throw new Exception(this.dataContext.TranslateTo("xLng.IslemYapilacakKayitBulunamadi"));
                }
            }

         row.UniqueId = _model.UniqueId;
         row.Durum = _model.Durum;
         row.BaslangicTarihi = _model.BaslangicTarihi;
         row.Ad = _model.Ad;

         if (!isNew)
         {
             //sadece update eklenip, insertte de değişmeyecek alanlar buraya
         }
         
         if (isNew)
         {
             this.dataContext.Tarife.Add(row);
         }
         
         return row.Id;
     }

     public bool Delete(Int32 _id)
     {
         Boolean rV = false;

         var row = this.dataContext.Tarife.Where(c => c.Id == _id).FirstOrDefault();
         if (row != null)
         {
             this.dataContext.Tarife.Remove(row);
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


