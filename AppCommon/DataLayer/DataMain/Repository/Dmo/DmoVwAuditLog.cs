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
    public class DmoVwAuditLog : BaseDmo
    {
        public DmoVwAuditLog(MainDataContext dataContext) : base(dataContext) { }

        public IQueryable<DtoVwAuditLog> Get()
        {
            return this.dataContext.VwAuditLog.AsNoTracking()
                .Select(s => new DtoVwAuditLog(this.dataContext)
                {
                    Id = s.Id,
                    UserId = s.UserId,
                    UserType = s.UserType,
                    UserName = s.UserName,
                    UserIp = s.UserIp,
                    UserBrowser = s.UserBrowser,
                    UserSessionGuid = s.UserSessionGuid,
                    OperationType = s.OperationType,
                    OperationDate = s.OperationDate,
                    TableName = s.TableName,
                    PrimaryKeyField = s.PrimaryKeyField,
                    PrimaryKeyValue = s.PrimaryKeyValue,
                    CurrentValues = s.CurrentValues,
                    OriginalValues = s.OriginalValues
                });
     }

        public DtoVwAuditLog GetByNew()
        {
            //Default değerler ile bir row döner, Burada field default değerleri veriliyor...
            DtoVwAuditLog row = new(this.dataContext) {
                Id = Guid.NewGuid(),
                UserId = 0
            };

            return row;
        }

        public IEnumerable<DtoVwAuditLog> GetById(Guid _id)
        {
            return this.Get().Where(c => c.Id == _id);
        }

        public Guid CreateOrUpdate(VwAuditLog _model, Boolean isNew)
        {
            VwAuditLog? row;
            
            if (isNew)
            {
                //sadece insertte eklenip, update de değişmeyecek alanlar buraya
                row = new VwAuditLog() {
                    Id = Guid.NewGuid()
                };
            }
            else
            {
                row = this.dataContext.VwAuditLog.Where(c => c.Id == _model.Id).FirstOrDefault();
                if (row == null)
                {
                    throw new Exception(this.dataContext.TranslateTo("xLng.IslemYapilacakKayitBulunamadi"));
                }
            }

         row.Id = _model.Id;
         row.UserId = _model.UserId;
         row.UserType = _model.UserType;
         row.UserName = _model.UserName;
         row.UserIp = _model.UserIp;
         row.UserBrowser = _model.UserBrowser;
         row.UserSessionGuid = _model.UserSessionGuid;
         row.OperationType = _model.OperationType;
         row.OperationDate = _model.OperationDate;
         row.TableName = _model.TableName;
         row.PrimaryKeyField = _model.PrimaryKeyField;
         row.PrimaryKeyValue = _model.PrimaryKeyValue;
         row.CurrentValues = _model.CurrentValues;
         row.OriginalValues = _model.OriginalValues;

         if (!isNew)
         {
             //sadece update eklenip, insertte de değişmeyecek alanlar buraya
         }
         
         if (isNew)
         {
             this.dataContext.VwAuditLog.Add(row);
         }
         
         return row.Id;
     }

     public bool Delete(Guid _id)
     {
         Boolean rV = false;

         var row = this.dataContext.VwAuditLog.Where(c => c.Id == _id).FirstOrDefault();
         if (row != null)
         {
             this.dataContext.VwAuditLog.Remove(row);
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


