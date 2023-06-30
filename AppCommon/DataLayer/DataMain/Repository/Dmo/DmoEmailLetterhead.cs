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
    public class DmoEmailLetterhead : BaseDmo
    {
        public DmoEmailLetterhead(DataContext dataContext) : base(dataContext) { }

        public IQueryable<DtoEmailLetterhead> Get()
        {
            return this.dataContext.EmailLetterhead.AsNoTracking()
                .Select(s => new DtoEmailLetterhead(this.dataContext)
                {
                    Id = s.Id,
                    Description = s.Description,
                    Content = s.Content,
                    UniqueId = s.UniqueId,
                    CreateDate = s.CreateDate,
                    CreatedUserId = s.CreatedUserId,
                    UpdateDate = s.UpdateDate,
                    UpdatedUserId = s.UpdatedUserId
                });
     }

        public DtoEmailLetterhead GetByNew()
        {
            //Default değerler ile bir row döner, Burada field default değerleri veriliyor...
            DtoEmailLetterhead row = new(this.dataContext) {
                Id = 0,
                UniqueId = Guid.NewGuid(),
                CreateDate = DateTime.Now,
                CreatedUserId = this.dataContext.UserId,
                UpdatedUserId = 0
            };

            return row;
        }

        public IEnumerable<DtoEmailLetterhead> GetById(Int32 _id)
        {
            return this.Get().Where(c => c.Id == _id);
        }

        public Int32 CreateOrUpdate(EmailLetterhead _model, Boolean isNew)
        {
            EmailLetterhead? row;
            
            if (isNew)
            {
                //sadece insertte eklenip, update de değişmeyecek alanlar buraya
                row = new EmailLetterhead() {
                    Id = (int)this.dataContext.GetNextSequenceValue("sqEmailLetterhead")
                };
            }
            else
            {
                row = this.dataContext.EmailLetterhead.Where(c => c.Id == _model.Id && c.UniqueId == _model.UniqueId).FirstOrDefault();
                if (row == null)
                {
                    throw new Exception(this.dataContext.TranslateTo("xLng.IslemYapilacakKayitBulunamadi"));
                }
            }

         row.Description = _model.Description;
         row.Content = _model.Content;
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
             this.dataContext.EmailLetterhead.Add(row);
         }
         
         return row.Id;
     }

     public bool Delete(Int32 _id)
     {
         Boolean rV = false;

         var row = this.dataContext.EmailLetterhead.Where(c => c.Id == _id).FirstOrDefault();
         if (row != null)
         {
             this.dataContext.EmailLetterhead.Remove(row);
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


