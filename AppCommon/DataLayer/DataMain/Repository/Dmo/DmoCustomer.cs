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
    public class DmoCustomer : BaseDmo
    {
        public DmoCustomer(MainDataContext dataContext) : base(dataContext) { }

        public IQueryable<DtoCustomer> Get()
        {
            return this.dataContext.Customer.AsNoTracking()
                .Select(s => new DtoCustomer(this.dataContext)
                {
                    Id = s.Id,
                    UserStatusId = s.UserStatusId,
                    CustomerTypeId = s.CustomerTypeId,
                    IsEmailConfirmed = s.IsEmailConfirmed,
                    NameSurname = s.NameSurname,
                    ResidenceAddress = s.ResidenceAddress,
                    Avatar = s.Avatar,
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
                    CcCustomerTypeIdName = s.CustomerType.Name.MyToTrim()
                });
     }

        public DtoCustomer GetByNew()
        {
            //Default değerler ile bir row döner, Burada field default değerleri veriliyor...
            DtoCustomer row = new(this.dataContext) {
                Id = 0,
                UserStatusId = 0,
                CustomerTypeId = 0,
                IsEmailConfirmed = true,
                CreateDate = DateTime.Now,
                CreatedUserId = this.dataContext.UserId,
                UpdatedUserId = 0
            };

            return row;
        }

        public IEnumerable<DtoCustomer> GetById(Int32 _id)
        {
            return this.Get().Where(c => c.Id == _id);
        }

        public Int32 CreateOrUpdate(Customer _model, Boolean isNew)
        {
            Customer? row;
            
            if (isNew)
            {
                //sadece insertte eklenip, update de değişmeyecek alanlar buraya
                row = new Customer() {
                    Id = (int)this.dataContext.GetNextSequenceValue("sqCustomer")
                };
            }
            else
            {
                row = this.dataContext.Customer.Where(c => c.Id == _model.Id && c.UniqueId == _model.UniqueId).FirstOrDefault();
                if (row == null)
                {
                    throw new Exception(this.dataContext.TranslateTo("xLng.IslemYapilacakKayitBulunamadi"));
                }
            }

         row.UserStatusId = _model.UserStatusId;
         row.CustomerTypeId = _model.CustomerTypeId;
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
             this.dataContext.Customer.Add(row);
         }
         
         return row.Id;
     }

     public bool Delete(Int32 _id)
     {
         Boolean rV = false;

         var row = this.dataContext.Customer.Where(c => c.Id == _id).FirstOrDefault();
         if (row != null)
         {
             this.dataContext.Customer.Remove(row);
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


