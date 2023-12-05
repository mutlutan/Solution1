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
    public class DmoRole : BaseDmo
    {
        public DmoRole(MainDataContext dataContext) : base(dataContext) { }

        public IQueryable<DtoRole> Get()
        {
            return this.dataContext.Role.AsNoTracking()
                .Select(s => new DtoRole(this.dataContext)
                {
                    Id = s.Id,
                    UniqueId = s.UniqueId,
                    IsActive = s.IsActive,
                    LineNumber = s.LineNumber,
                    Name = s.Name,
                    Authority = s.Authority,
                    CreateDate = s.CreateDate,
                    CreatedUserId = s.CreatedUserId,
                    UpdateDate = s.UpdateDate,
                    UpdatedUserId = s.UpdatedUserId
                });
     }

        public DtoRole GetByNew()
        {
            //Default değerler ile bir row döner, Burada field default değerleri veriliyor...
            DtoRole row = new(this.dataContext) {
                Id = 0,
                UniqueId = Guid.NewGuid(),
                IsActive = true,
                LineNumber = this.dataContext.Role.DefaultIfEmpty().Max(m => m == null ? 0 : m.LineNumber) + 1,
                CreateDate = DateTime.Now,
                CreatedUserId = this.dataContext.UserId,
                UpdatedUserId = 0
            };

            return row;
        }

        public IEnumerable<DtoRole> GetById(Int32 _id)
        {
            return this.Get().Where(c => c.Id == _id);
        }

        public Int32 CreateOrUpdate(Role _model, Boolean isNew)
        {
            Role? row;
            
            if (isNew)
            {
                //sadece insertte eklenip, update de değişmeyecek alanlar buraya
                row = new Role() {
                    Id = (int)this.dataContext.GetNextSequenceValue("sqRole")
                };
            }
            else
            {
                row = this.dataContext.Role.Where(c => c.Id == _model.Id && c.UniqueId == _model.UniqueId).FirstOrDefault();
                if (row == null)
                {
                    throw new Exception(this.dataContext.TranslateTo("xLng.IslemYapilacakKayitBulunamadi"));
                }
            }

         row.IsActive = _model.IsActive;
         row.LineNumber = _model.LineNumber;
         row.Name = _model.Name;
         row.Authority = _model.Authority;
         row.UniqueId = _model.UniqueId;
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
             this.dataContext.Role.Add(row);
         }
         
         return row.Id;
     }

     public bool Delete(Int32 _id)
     {
         Boolean rV = false;

         var row = this.dataContext.Role.Where(c => c.Id == _id).FirstOrDefault();
         if (row != null)
         {
             this.dataContext.Role.Remove(row);
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


