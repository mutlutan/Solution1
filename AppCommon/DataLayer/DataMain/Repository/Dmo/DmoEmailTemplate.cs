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
    public class DmoEmailTemplate : BaseDmo
    {
        public DmoEmailTemplate(MainDataContext dataContext) : base(dataContext) { }

        public IQueryable<DtoEmailTemplate> Get()
        {
            return this.dataContext.EmailTemplate.AsNoTracking()
                .Select(s => new DtoEmailTemplate(this.dataContext)
                {
                    Id = s.Id,
                    EmailLetterheadId = s.EmailLetterheadId,
                    Name = s.Name,
                    EmailCc = s.EmailCc,
                    EmailBcc = s.EmailBcc,
                    EmailSubject = s.EmailSubject,
                    EmailContent = s.EmailContent,
                    UniqueId = s.UniqueId,
                    CreateDate = s.CreateDate,
                    CreatedUserId = s.CreatedUserId,
                    UpdateDate = s.UpdateDate,
                    UpdatedUserId = s.UpdatedUserId,
                    CcEmailLetterheadIdDescription = s.EmailLetterhead.Description.MyToTrim()
                });
     }

        public DtoEmailTemplate GetByNew()
        {
            //Default değerler ile bir row döner, Burada field default değerleri veriliyor...
            DtoEmailTemplate row = new(this.dataContext) {
                Id = 0,
                EmailLetterheadId = 0,
                UniqueId = Guid.NewGuid(),
                CreateDate = DateTime.Now,
                CreatedUserId = this.dataContext.UserId,
                UpdatedUserId = 0
            };

            return row;
        }

        public IEnumerable<DtoEmailTemplate> GetById(Int32 _id)
        {
            return this.Get().Where(c => c.Id == _id);
        }

        public Int32 CreateOrUpdate(EmailTemplate _model, Boolean isNew)
        {
            EmailTemplate? row;
            
            if (isNew)
            {
                //sadece insertte eklenip, update de değişmeyecek alanlar buraya
                row = new EmailTemplate() {
                };
            }
            else
            {
                row = this.dataContext.EmailTemplate.Where(c => c.Id == _model.Id && c.UniqueId == _model.UniqueId).FirstOrDefault();
                if (row == null)
                {
                    throw new Exception(this.dataContext.TranslateTo("xLng.IslemYapilacakKayitBulunamadi"));
                }
            }

         row.EmailLetterheadId = _model.EmailLetterheadId;
         row.Name = _model.Name;
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
             this.dataContext.EmailTemplate.Add(row);
         }
         
         return row.Id;
     }

     public bool Delete(Int32 _id)
     {
         Boolean rV = false;

         var row = this.dataContext.EmailTemplate.Where(c => c.Id == _id).FirstOrDefault();
         if (row != null)
         {
             this.dataContext.EmailTemplate.Remove(row);
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


