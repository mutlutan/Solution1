using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AppData.Main.Repository;
using AppData.Main.Models;
using AppData.Main.Repository.Dto;
using AppCommon;

namespace AppData.Main.Repository.Dmo
{
    public class DmoUser : BaseDmo
    {
        public DmoUser(DataContext dataContext) : base(dataContext) { }

        public IQueryable<DtoUser> Get()
        {
            return this.dataContext.User.AsNoTracking()
                .Select(s => new DtoUser(this.dataContext)
                {
                    Id = s.Id,
                    IsActive = s.IsActive,
                    IsEmailConfirmed = s.IsEmailConfirmed,
                    IdentificationNumber = s.IdentificationNumber,
                    NameSurname = s.NameSurname,
                    ResidenceAddress = s.ResidenceAddress,
                    Avatar = s.Avatar.MyToStr(),
                    UserName = s.UserName,
                    UserPassword = string.Empty,
                    RoleIds = s.RoleIds,
                    GaSecretKey = s.GaSecretKey,
                    SessionGuid = s.SessionGuid,
                    ValidityDate = s.ValidityDate,
                    UniqueId = s.UniqueId,
                    CreateDate = s.CreateDate,
                    CreatedUserId = s.CreatedUserId,
                    UpdateDate = s.UpdateDate,
                    UpdatedUserId = s.UpdatedUserId
                });
     }

        public DtoUser GetByNew()
        {
            //Default değerler ile bir row döner, Burada field default değerleri veriliyor...
            DtoUser row = new(this.dataContext) {
                Id = 0,
                IsActive = false,
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

         row.IsActive = _model.IsActive;
         row.IsEmailConfirmed = _model.IsEmailConfirmed;
         row.IdentificationNumber = _model.IdentificationNumber;
         row.NameSurname = _model.NameSurname;
         row.ResidenceAddress = _model.ResidenceAddress;
         row.Avatar = _model.Avatar;
         row.UserName = _model.UserName;

         if (!string.IsNullOrEmpty(_model.UserPassword))
         {
             row.UserPassword = _model.UserPassword.MyToEncryptPassword();
         }

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


