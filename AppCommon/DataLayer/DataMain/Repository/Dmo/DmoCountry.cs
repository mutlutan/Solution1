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
    public class DmoCountry : BaseDmo
    {
        public DmoCountry(MainDataContext dataContext) : base(dataContext) { }

        public IQueryable<DtoCountry> Get()
        {
            return this.dataContext.Country.AsNoTracking()
                .Select(s => new DtoCountry(this.dataContext)
                {
                    Id = s.Id,
                    IsActive = s.IsActive,
                    LineNumber = s.LineNumber,
                    Code = s.Code,
                    Name = s.Name
                });
     }

        public DtoCountry GetByNew()
        {
            //Default değerler ile bir row döner, Burada field default değerleri veriliyor...
            DtoCountry row = new(this.dataContext) {
                Id = 0,
                IsActive = true,
                LineNumber = 0
            };

            return row;
        }

        public IEnumerable<DtoCountry> GetById(Int32 _id)
        {
            return this.Get().Where(c => c.Id == _id);
        }

        public Int32 CreateOrUpdate(Country _model, Boolean isNew)
        {
            Country? row;
            
            if (isNew)
            {
                //sadece insertte eklenip, update de değişmeyecek alanlar buraya
                row = new Country() {
                    Id = (int)this.dataContext.GetNextSequenceValue("sqCountry")
                };
            }
            else
            {
                row = this.dataContext.Country.Where(c => c.Id == _model.Id).FirstOrDefault();
                if (row == null)
                {
                    throw new Exception(this.dataContext.TranslateTo("xLng.IslemYapilacakKayitBulunamadi"));
                }
            }

         row.IsActive = _model.IsActive;

         if (_model.LineNumber != 0 )
         {
             row.LineNumber = _model.LineNumber;
         } 
         else {
            row.LineNumber = this.dataContext.Country
                .DefaultIfEmpty().Max(m => m == null ? 0 : m.LineNumber) + 1;
         }

         row.Code = _model.Code;
         row.Name = _model.Name;

         if (!isNew)
         {
             //sadece update eklenip, insertte de değişmeyecek alanlar buraya
         }
         
         if (isNew)
         {
             this.dataContext.Country.Add(row);
         }
         
         return row.Id;
     }

     public bool Delete(Int32 _id)
     {
         Boolean rV = false;

         var row = this.dataContext.Country.Where(c => c.Id == _id).FirstOrDefault();
         if (row != null)
         {
             this.dataContext.Country.Remove(row);
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


