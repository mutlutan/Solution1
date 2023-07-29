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
    public class DmoCurrency : BaseDmo
    {
        public DmoCurrency(MainDataContext dataContext) : base(dataContext) { }

        public IQueryable<DtoCurrency> Get()
        {
            return this.dataContext.Currency.AsNoTracking()
                .Select(s => new DtoCurrency(this.dataContext)
                {
                    Id = s.Id,
                    IsActive = s.IsActive,
                    LineNumber = s.LineNumber,
                    Icon = s.Icon,
                    Code = s.Code,
                    Name = s.Name,
                    SubName = s.SubName
                });
     }

        public DtoCurrency GetByNew()
        {
            //Default değerler ile bir row döner, Burada field default değerleri veriliyor...
            DtoCurrency row = new(this.dataContext) {
                Id = 0,
                IsActive = true,
                LineNumber = 0
            };

            return row;
        }

        public IEnumerable<DtoCurrency> GetById(Int32 _id)
        {
            return this.Get().Where(c => c.Id == _id);
        }

        public Int32 CreateOrUpdate(Currency _model, Boolean isNew)
        {
            Currency? row;
            
            if (isNew)
            {
                //sadece insertte eklenip, update de değişmeyecek alanlar buraya
                row = new Currency() {
                    Id = (int)this.dataContext.GetNextSequenceValue("sqCurrency")
                };
            }
            else
            {
                row = this.dataContext.Currency.Where(c => c.Id == _model.Id).FirstOrDefault();
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
            row.LineNumber = this.dataContext.Currency
                .DefaultIfEmpty().Max(m => m == null ? 0 : m.LineNumber) + 1;
         }

         row.Icon = _model.Icon;
         row.Code = _model.Code;
         row.Name = _model.Name;
         row.SubName = _model.SubName;

         if (!isNew)
         {
             //sadece update eklenip, insertte de değişmeyecek alanlar buraya
         }
         
         if (isNew)
         {
             this.dataContext.Currency.Add(row);
         }
         
         return row.Id;
     }

     public bool Delete(Int32 _id)
     {
         Boolean rV = false;

         var row = this.dataContext.Currency.Where(c => c.Id == _id).FirstOrDefault();
         if (row != null)
         {
             this.dataContext.Currency.Remove(row);
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


