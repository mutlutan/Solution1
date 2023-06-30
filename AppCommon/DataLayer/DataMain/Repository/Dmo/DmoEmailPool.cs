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
    public class DmoEmailPool : BaseDmo
    {
        public DmoEmailPool(MainDataContext dataContext) : base(dataContext) { }

        public IQueryable<DtoEmailPool> Get()
        {
            return this.dataContext.EmailPool.AsNoTracking()
                .Select(s => new DtoEmailPool(this.dataContext)
                {
                    Id = s.Id,
                    EmailTemplateId = s.EmailTemplateId,
                    EmailPoolStatusId = s.EmailPoolStatusId,
                    TryQuantity = s.TryQuantity,
                    LastTryDate = s.LastTryDate,
                    Description = s.Description,
                    EmailTo = s.EmailTo,
                    EmailCc = s.EmailCc,
                    EmailBcc = s.EmailBcc,
                    EmailSubject = s.EmailSubject,
                    EmailContent = s.EmailContent,
                    UniqueId = s.UniqueId,
                    CreateDate = s.CreateDate,
                    CreatedUserId = s.CreatedUserId,
                    UpdateDate = s.UpdateDate,
                    UpdatedUserId = s.UpdatedUserId,
                    CcEmailTemplateIdName = s.EmailTemplate.Name.MyToTrim(),
                    CcEmailPoolStatusIdName = s.EmailPoolStatus.Name.MyToTrim()
                });
     }

        public DtoEmailPool GetByNew()
        {
            //Default değerler ile bir row döner, Burada field default değerleri veriliyor...
            DtoEmailPool row = new(this.dataContext) {
                Id = 0,
                EmailTemplateId = 0,
                EmailPoolStatusId = 0,
                TryQuantity = 0,
                UniqueId = Guid.NewGuid(),
                CreateDate = DateTime.Now,
                CreatedUserId = this.dataContext.UserId,
                UpdatedUserId = 0
            };

            return row;
        }

        public IEnumerable<DtoEmailPool> GetById(Int32 _id)
        {
            return this.Get().Where(c => c.Id == _id);
        }

        public Int32 CreateOrUpdate(EmailPool _model, Boolean isNew)
        {
            EmailPool? row;
            
            if (isNew)
            {
                //sadece insertte eklenip, update de değişmeyecek alanlar buraya
                row = new EmailPool() {
                    Id = (int)this.dataContext.GetNextSequenceValue("sqEmailPool")
                };
            }
            else
            {
                row = this.dataContext.EmailPool.Where(c => c.Id == _model.Id && c.UniqueId == _model.UniqueId).FirstOrDefault();
                if (row == null)
                {
                    throw new Exception(this.dataContext.TranslateTo("xLng.IslemYapilacakKayitBulunamadi"));
                }
            }

         row.EmailTemplateId = _model.EmailTemplateId;
         row.EmailPoolStatusId = _model.EmailPoolStatusId;
         row.TryQuantity = _model.TryQuantity;
         row.LastTryDate = _model.LastTryDate;
         row.Description = _model.Description;
         row.EmailTo = _model.EmailTo;
         row.EmailCc = _model.EmailCc;
         row.EmailBcc = _model.EmailBcc;
         row.EmailSubject = _model.EmailSubject;
         row.EmailContent = _model.EmailContent;
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
             this.dataContext.EmailPool.Add(row);
         }
         
         return row.Id;
     }

     public bool Delete(Int32 _id)
     {
         Boolean rV = false;

         var row = this.dataContext.EmailPool.Where(c => c.Id == _id).FirstOrDefault();
         if (row != null)
         {
             this.dataContext.EmailPool.Remove(row);
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


