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
    public class DmoUser : BaseDmo
    {
        public DmoUser(MainDataContext dataContext) : base(dataContext) { }

        public IQueryable<DtoUser> Get()
        {
            return this.dataContext.User.AsNoTracking()
                .Select(s => new DtoUser(this.dataContext)
                {
                    Id = s.Id,
                    UserStatusId = s.UserStatusId,
                    UserTypeId = s.UserTypeId,
                    IsEmailConfirmed = s.IsEmailConfirmed,
                    NameSurname = s.NameSurname,
                    ResidenceAddress = s.ResidenceAddress,
                    Avatar = s.Avatar.MyToStr(),
                    GeoLocation = s.GeoLocation,
                    Email = s.Email,
                    Password = s.Password,
                    RoleIds = s.RoleIds,
                    GaSecretKey = s.GaSecretKey,
                    SessionGuid = s.SessionGuid,
                    ValidityDate = s.ValidityDate,
                    UniqueId = s.UniqueId,
                    CreateDate = s.CreateDate,
                    CreatedUserId = s.CreatedUserId,
                    UpdateDate = s.UpdateDate,
                    UpdatedUserId = s.UpdatedUserId,
                    CcUserStatusIdName = s.UserStatus.Name.MyToTrim(),
                    CcUserTypeIdName = s.UserType.Name.MyToTrim()
                });
     }

        public DtoUser GetByNew()
        {
            //Default değerler ile bir row döner, Burada field default değerleri veriliyor...
            DtoUser row = new(this.dataContext) {
                Id = 0,
                UserStatusId = 0,
                UserTypeId = 0,
                IsEmailConfirmed = false,
                UniqueId = Guid.NewGuid(),
                CreateDate = DateTime.Now,
                CreatedUserId = this.dataContext.UserId,
                UpdatedUserId = 0
            };

            return row;
        }

        public IEnumerable<DtoUser> GetById(Int32 _id)
        {
            return this.Get().Where(c => c.Id == _id);
        }

        public Int32 CreateOrUpdate(User _model, Boolean isNew)
        {
            User? row;
            
            if (isNew)
            {
                //sadece insertte eklenip, update de değişmeyecek alanlar buraya
                row = new User() {
                    Id = (int)this.dataContext.GetNextSequenceValue("sqUser")
                };
            }
            else
            {
                row = this.dataContext.User.Where(c => c.Id == _model.Id && c.UniqueId == _model.UniqueId).FirstOrDefault();
                if (row == null)
                {
                    throw new Exception(this.dataContext.TranslateTo("xLng.IslemYapilacakKayitBulunamadi"));
                }
            }

         row.UserStatusId = _model.UserStatusId;
         row.UserTypeId = _model.UserTypeId;
         row.IsEmailConfirmed = _model.IsEmailConfirmed;
         row.NameSurname = _model.NameSurname;
         row.ResidenceAddress = _model.ResidenceAddress;
         row.Avatar = _model.Avatar;
         row.GeoLocation = _model.GeoLocation;
         row.Email = _model.Email;
         row.Password = _model.Password;
         row.RoleIds = _model.RoleIds;
         row.GaSecretKey = _model.GaSecretKey;
         row.SessionGuid = _model.SessionGuid;
         row.ValidityDate = _model.ValidityDate;
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
             this.dataContext.User.Add(row);
         }
         
         return row.Id;
     }

     public bool Delete(Int32 _id)
     {
         Boolean rV = false;

         var row = this.dataContext.User.Where(c => c.Id == _id).FirstOrDefault();
         if (row != null)
         {
             this.dataContext.User.Remove(row);
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


