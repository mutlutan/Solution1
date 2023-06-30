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
    public class DmoBolge : BaseDmo
    {
        public DmoBolge(MainDataContext dataContext) : base(dataContext) { }

        public IQueryable<DtoBolge> Get()
        {
            return this.dataContext.Bolge.AsNoTracking()
                .Select(s => new DtoBolge(this.dataContext)
                {
                    Id = s.Id,
                    UniqueId = s.UniqueId,
                    Durum = s.Durum,
                    Polygon = s.Polygon
                });
     }

        public DtoBolge GetByNew()
        {
            //Default değerler ile bir row döner, Burada field default değerleri veriliyor...
            DtoBolge row = new(this.dataContext) {
                Id = 0,
                UniqueId = Guid.NewGuid(),
                Durum = true
            };

            return row;
        }

        public IEnumerable<DtoBolge> GetById(Int32 _id)
        {
            return this.Get().Where(c => c.Id == _id);
        }

        public Int32 CreateOrUpdate(Bolge _model, Boolean isNew)
        {
            Bolge? row;
            
            if (isNew)
            {
                //sadece insertte eklenip, update de değişmeyecek alanlar buraya
                row = new Bolge() {
                    Id = (int)this.dataContext.GetNextSequenceValue("sqBolge")
                };
            }
            else
            {
                row = this.dataContext.Bolge.Where(c => c.Id == _model.Id && c.UniqueId == _model.UniqueId).FirstOrDefault();
                if (row == null)
                {
                    throw new Exception(this.dataContext.TranslateTo("xLng.IslemYapilacakKayitBulunamadi"));
                }
            }

         row.UniqueId = _model.UniqueId;
         row.Durum = _model.Durum;
         row.Polygon = _model.Polygon;

         if (!isNew)
         {
             //sadece update eklenip, insertte de değişmeyecek alanlar buraya
         }
         
         if (isNew)
         {
             this.dataContext.Bolge.Add(row);
         }
         
         return row.Id;
     }

     public bool Delete(Int32 _id)
     {
         Boolean rV = false;

         var row = this.dataContext.Bolge.Where(c => c.Id == _id).FirstOrDefault();
         if (row != null)
         {
             this.dataContext.Bolge.Remove(row);
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


