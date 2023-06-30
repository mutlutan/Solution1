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
    public class DmoVwSystemLog : BaseDmo
    {
        public DmoVwSystemLog(MainDataContext dataContext) : base(dataContext) { }

        public IQueryable<DtoVwSystemLog> Get()
        {
            return this.dataContext.VwSystemLog.AsNoTracking()
                .Select(s => new DtoVwSystemLog(this.dataContext)
                {
                    Id = s.Id,
                    UserId = s.UserId,
                    UserType = s.UserType,
                    UserName = s.UserName,
                    UserIp = s.UserIp,
                    UserBrowser = s.UserBrowser,
                    UserSessionGuid = s.UserSessionGuid,
                    ProcessTypeId = s.ProcessTypeId,
                    ProcessDate = s.ProcessDate,
                    ProcessName = s.ProcessName,
                    ProcessContent = s.ProcessContent,
                    ProcessingTime = s.ProcessingTime
                });
     }

        public DtoVwSystemLog GetByNew()
        {
            //Default değerler ile bir row döner, Burada field default değerleri veriliyor...
            DtoVwSystemLog row = new(this.dataContext) {
                UserId = 0,
                ProcessTypeId = 0
            };

            return row;
        }

        public IEnumerable<DtoVwSystemLog> GetById(Guid _id)
        {
            return this.Get().Where(c => c.Id == _id);
        }

        public Guid CreateOrUpdate(VwSystemLog _model, Boolean isNew)
        {
            VwSystemLog? row;
            
            if (isNew)
            {
                //sadece insertte eklenip, update de değişmeyecek alanlar buraya
                row = new VwSystemLog() {
                    Id = Guid.NewGuid()
                };
            }
            else
            {
                row = this.dataContext.VwSystemLog.Where(c => c.Id == _model.Id).FirstOrDefault();
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
         row.ProcessTypeId = _model.ProcessTypeId;
         row.ProcessDate = _model.ProcessDate;
         row.ProcessName = _model.ProcessName;
         row.ProcessContent = _model.ProcessContent;
         row.ProcessingTime = _model.ProcessingTime;

         if (!isNew)
         {
             //sadece update eklenip, insertte de değişmeyecek alanlar buraya
         }
         
         if (isNew)
         {
             this.dataContext.VwSystemLog.Add(row);
         }
         
         return row.Id;
     }

     public bool Delete(Guid _id)
     {
         Boolean rV = false;

         var row = this.dataContext.VwSystemLog.Where(c => c.Id == _id).FirstOrDefault();
         if (row != null)
         {
             this.dataContext.VwSystemLog.Remove(row);
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


