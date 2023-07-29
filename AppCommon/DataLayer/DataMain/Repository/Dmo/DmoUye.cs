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
    public class DmoUye : BaseDmo
    {
        public DmoUye(MainDataContext dataContext) : base(dataContext) { }

        public IQueryable<DtoUye> Get()
        {
            return this.dataContext.Uye.AsNoTracking()
                .Select(s => new DtoUye(this.dataContext)
                {
                    Id = s.Id,
                    UyeDurumId = s.UyeDurumId,
                    UyeGrupId = s.UyeGrupId,
                    IsConfirmed = s.IsConfirmed,
                    NameSurname = s.NameSurname,
                    CountryCode = s.CountryCode,
                    Avatar = s.Avatar,
                    GeoLocation = s.GeoLocation,
                    UserName = s.UserName,
                    UserPassword = string.Empty,
                    SessionGuid = s.SessionGuid,
                    ValidityDate = s.ValidityDate,
                    UniqueId = s.UniqueId,
                    CreateDate = s.CreateDate,
                    CreatedUserId = s.CreatedUserId,
                    UpdateDate = s.UpdateDate,
                    UpdatedUserId = s.UpdatedUserId,
                    CcUyeDurumIdAd = s.UyeDurum.Ad.MyToTrim(),
                    CcUyeGrupIdAd = s.UyeGrup.Ad.MyToTrim()
                });
     }

        public DtoUye GetByNew()
        {
            //Default değerler ile bir row döner, Burada field default değerleri veriliyor...
            DtoUye row = new(this.dataContext) {
                Id = 0,
                UyeDurumId = 0,
                UyeGrupId = 0,
                IsConfirmed = true,
                UniqueId = Guid.NewGuid(),
                CreateDate = DateTime.Now,
                CreatedUserId = this.dataContext.UserId,
                UpdatedUserId = 0
            };

            return row;
        }

        public IEnumerable<DtoUye> GetById(Int32 _id)
        {
            return this.Get().Where(c => c.Id == _id);
        }

        public Int32 CreateOrUpdate(Uye _model, Boolean isNew)
        {
            Uye? row;
            
            if (isNew)
            {
                //sadece insertte eklenip, update de değişmeyecek alanlar buraya
                row = new Uye() {
                    Id = (int)this.dataContext.GetNextSequenceValue("sqUye")
                };
            }
            else
            {
                row = this.dataContext.Uye.Where(c => c.Id == _model.Id && c.UniqueId == _model.UniqueId).FirstOrDefault();
                if (row == null)
                {
                    throw new Exception(this.dataContext.TranslateTo("xLng.IslemYapilacakKayitBulunamadi"));
                }
            }

         row.UyeDurumId = _model.UyeDurumId;
         row.UyeGrupId = _model.UyeGrupId;
         row.IsConfirmed = _model.IsConfirmed;
         row.NameSurname = _model.NameSurname;
         row.CountryCode = _model.CountryCode;
         row.Avatar = _model.Avatar;
         row.GeoLocation = _model.GeoLocation;
         row.UserName = _model.UserName;

         if (!string.IsNullOrEmpty(_model.UserPassword))
         {
             row.UserPassword = _model.UserPassword.MyToEncryptPassword();
         }

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
             this.dataContext.Uye.Add(row);
         }
         
         return row.Id;
     }

     public bool Delete(Int32 _id)
     {
         Boolean rV = false;

         var row = this.dataContext.Uye.Where(c => c.Id == _id).FirstOrDefault();
         if (row != null)
         {
             this.dataContext.Uye.Remove(row);
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


