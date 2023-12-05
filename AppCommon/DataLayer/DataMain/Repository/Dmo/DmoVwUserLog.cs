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
    public class DmoVwUserLog : BaseDmo
    {
        public DmoVwUserLog(MainDataContext dataContext) : base(dataContext) { }

        public IQueryable<DtoVwUserLog> Get()
        {
            return this.dataContext.VwUserLog.AsNoTracking()
                .Select(s => new DtoVwUserLog(this.dataContext)
                {
                    Id = s.Id,
                    TableName = s.TableName,
                    UserId = s.UserId,
                    UserName = s.UserName,
                    UserIp = s.UserIp,
                    UserBrowser = s.UserBrowser,
                    SessionGuid = s.SessionGuid,
                    LoginDate = s.LoginDate,
                    LogoutDate = s.LogoutDate,
                    ExtraSpace1 = s.ExtraSpace1,
                    ExtraSpace2 = s.ExtraSpace2,
                    ExtraSpace3 = s.ExtraSpace3
                });
     }

        public DtoVwUserLog GetByNew()
        {
            //Default değerler ile bir row döner, Burada field default değerleri veriliyor...
            DtoVwUserLog row = new(this.dataContext) {
                Id = Guid.NewGuid(),
                UserId = 0
            };

            return row;
        }

        public IEnumerable<DtoVwUserLog> GetById(Guid _id)
        {
            return this.Get().Where(c => c.Id == _id);
        }

        public Guid CreateOrUpdate(VwUserLog _model, Boolean isNew)
        {
            VwUserLog? row;
            
            if (isNew)
            {
                //sadece insertte eklenip, update de değişmeyecek alanlar buraya
                row = new VwUserLog() {
                    Id = Guid.NewGuid()
                };
            }
            else
            {
                row = this.dataContext.VwUserLog.Where(c => c.Id == _model.Id).FirstOrDefault();
                if (row == null)
                {
                    throw new Exception(this.dataContext.TranslateTo("xLng.IslemYapilacakKayitBulunamadi"));
                }
            }

         row.Id = _model.Id;
         row.TableName = _model.TableName;
         row.UserId = _model.UserId;
         row.UserName = _model.UserName;
         row.UserIp = _model.UserIp;
         row.UserBrowser = _model.UserBrowser;
         row.SessionGuid = _model.SessionGuid;
         row.LoginDate = _model.LoginDate;
         row.LogoutDate = _model.LogoutDate;
         row.ExtraSpace1 = _model.ExtraSpace1;
         row.ExtraSpace2 = _model.ExtraSpace2;
         row.ExtraSpace3 = _model.ExtraSpace3;

         if (!isNew)
         {
             //sadece update eklenip, insertte de değişmeyecek alanlar buraya
         }
         
         if (isNew)
         {
             this.dataContext.VwUserLog.Add(row);
         }
         
         return row.Id;
     }

     public bool Delete(Guid _id)
     {
         Boolean rV = false;

         var row = this.dataContext.VwUserLog.Where(c => c.Id == _id).FirstOrDefault();
         if (row != null)
         {
             this.dataContext.VwUserLog.Remove(row);
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


