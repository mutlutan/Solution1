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
    public class DmoVwUserLog : BaseDmo
    {
        public DmoVwUserLog(DataContext dataContext) : base(dataContext) { }

        public IQueryable<DtoVwUserLog> Get()
        {
            return this.dataContext.VwUserLog.AsNoTracking()
                .Select(s => new DtoVwUserLog(this.dataContext)
                {
                    Id = s.Id,
                    UserId = s.UserId,
                    UserType = s.UserType,
                    UserName = s.UserName,
                    UserIp = s.UserIp,
                    UserBrowser = s.UserBrowser,
                    UserSessionGuid = s.UserSessionGuid,
                    LoginDate = s.LoginDate,
                    LogoutDate = s.LogoutDate,
                    EkAlan1 = s.EkAlan1,
                    EkAlan2 = s.EkAlan2,
                    EkAlan3 = s.EkAlan3
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
         row.UserId = _model.UserId;
         row.UserType = _model.UserType;
         row.UserName = _model.UserName;
         row.UserIp = _model.UserIp;
         row.UserBrowser = _model.UserBrowser;
         row.UserSessionGuid = _model.UserSessionGuid;
         row.LoginDate = _model.LoginDate;
         row.LogoutDate = _model.LogoutDate;
         row.EkAlan1 = _model.EkAlan1;
         row.EkAlan2 = _model.EkAlan2;
         row.EkAlan3 = _model.EkAlan3;

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


