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
    public class DmoParameter : BaseDmo
    {
        public DmoParameter(MainDataContext dataContext) : base(dataContext) { }

        public IQueryable<DtoParameter> Get()
        {
            return this.dataContext.Parameter.AsNoTracking()
                .Select(s => new DtoParameter(this.dataContext)
                {
                    Id = s.Id,
                    SiteAddress = s.SiteAddress,
                    InstitutionEmail = s.InstitutionEmail,
                    AuditLog = s.AuditLog,
                    AuditLogTables = s.AuditLogTables,
                    EmailHost = s.EmailHost,
                    EmailPort = s.EmailPort,
                    EmailEnableSsl = s.EmailEnableSsl,
                    EmailUserName = s.EmailUserName,
                    EmailPassword = string.Empty,
                    GoogleMapApiKey = s.GoogleMapApiKey
                });
     }

        public DtoParameter GetByNew()
        {
            //Default değerler ile bir row döner, Burada field default değerleri veriliyor...
            DtoParameter row = new(this.dataContext) {
                Id = 0,
                AuditLog = true,
                EmailPort = 0,
                EmailEnableSsl = true
            };

            return row;
        }

        public IEnumerable<DtoParameter> GetById(Int32 _id)
        {
            return this.Get().Where(c => c.Id == _id);
        }

        public Int32 CreateOrUpdate(Parameter _model, Boolean isNew)
        {
            Parameter? row;
            
            if (isNew)
            {
                //sadece insertte eklenip, update de değişmeyecek alanlar buraya
                row = new Parameter() {
                };
            }
            else
            {
                row = this.dataContext.Parameter.Where(c => c.Id == _model.Id).FirstOrDefault();
                if (row == null)
                {
                    throw new Exception(this.dataContext.TranslateTo("xLng.IslemYapilacakKayitBulunamadi"));
                }
            }

         row.SiteAddress = _model.SiteAddress;
         row.InstitutionEmail = _model.InstitutionEmail;
         row.AuditLog = _model.AuditLog;
         row.AuditLogTables = _model.AuditLogTables;
         row.EmailHost = _model.EmailHost;
         row.EmailPort = _model.EmailPort;
         row.EmailEnableSsl = _model.EmailEnableSsl;
         row.EmailUserName = _model.EmailUserName;

         if (!string.IsNullOrEmpty(_model.EmailPassword))
         {
             row.EmailPassword = _model.EmailPassword.MyToEncryptPassword();
         }

         row.GoogleMapApiKey = _model.GoogleMapApiKey;

         if (!isNew)
         {
             //sadece update eklenip, insertte de değişmeyecek alanlar buraya
         }
         
         if (isNew)
         {
             this.dataContext.Parameter.Add(row);
         }
         
         return row.Id;
     }

     public bool Delete(Int32 _id)
     {
         Boolean rV = false;

         var row = this.dataContext.Parameter.Where(c => c.Id == _id).FirstOrDefault();
         if (row != null)
         {
             this.dataContext.Parameter.Remove(row);
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


