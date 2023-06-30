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
    public class DmoUye : BaseDmo
    {
        public DmoUye(DataContext dataContext) : base(dataContext) { }

        public IQueryable<DtoUye> Get()
        {
            return this.dataContext.Uye.AsNoTracking()
                .Select(s => new DtoUye(this.dataContext)
                {
                    Id = s.Id,
                    UniqueId = s.UniqueId,
                    UyeDurumId = s.UyeDurumId,
                    UyeGrupId = s.UyeGrupId,
                    UyelikTarihi = s.UyelikTarihi,
                    Email = s.Email,
                    Sifre = string.Empty,
                    KimlikNumarasi = s.KimlikNumarasi,
                    Ad = s.Ad,
                    Soyad = s.Soyad,
                    Gsm = s.Gsm,
                    DogumTarihi = s.DogumTarihi,
                    CinsiyetId = s.CinsiyetId,
                    Avatar = s.Avatar,
                    UyelikDogrulama = s.UyelikDogrulama,
                    SifreSifirlamaKod = s.SifreSifirlamaKod,
                    KvkkOnayi = s.KvkkOnayi,
                    UyelikSozlesmeOnayi = s.UyelikSozlesmeOnayi,
                    AydinlatmaMetniOnayi = s.AydinlatmaMetniOnayi,
                    CuzdanBakiye = s.CuzdanBakiye,
                    MsisdnDogrulama = s.MsisdnDogrulama,
                    FcmRegistrationToken = s.FcmRegistrationToken,
                    MobileAppState = s.MobileAppState,
                    CcUyeDurumIdAd = s.UyeDurum.Ad.MyToTrim(),
                    CcUyeGrupIdAd = s.UyeGrup.Ad.MyToTrim(),
                    CcCinsiyetIdAd = s.Cinsiyet.Ad.MyToTrim()
                });
     }

        public DtoUye GetByNew()
        {
            //Default değerler ile bir row döner, Burada field default değerleri veriliyor...
            DtoUye row = new(this.dataContext) {
                Id = 0,
                UniqueId = Guid.NewGuid(),
                UyeDurumId = 0,
                UyeGrupId = 0,
                CinsiyetId = 0,
                UyelikDogrulama = false,
                KvkkOnayi = false,
                UyelikSozlesmeOnayi = false,
                AydinlatmaMetniOnayi = false,
                CuzdanBakiye = 0,
                MsisdnDogrulama = true
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

         row.UniqueId = _model.UniqueId;
         row.UyeDurumId = _model.UyeDurumId;
         row.UyeGrupId = _model.UyeGrupId;
         row.UyelikTarihi = _model.UyelikTarihi;
         row.Email = _model.Email;

         if (!string.IsNullOrEmpty(_model.Sifre))
         {
             row.Sifre = _model.Sifre.MyToEncryptPassword();
         }

         row.KimlikNumarasi = _model.KimlikNumarasi;
         row.Ad = _model.Ad;
         row.Soyad = _model.Soyad;
         row.Gsm = _model.Gsm;
         row.DogumTarihi = _model.DogumTarihi;
         row.CinsiyetId = _model.CinsiyetId;
         row.Avatar = _model.Avatar;
         row.UyelikDogrulama = _model.UyelikDogrulama;
         row.SifreSifirlamaKod = _model.SifreSifirlamaKod;
         row.KvkkOnayi = _model.KvkkOnayi;
         row.UyelikSozlesmeOnayi = _model.UyelikSozlesmeOnayi;
         row.AydinlatmaMetniOnayi = _model.AydinlatmaMetniOnayi;
         row.CuzdanBakiye = _model.CuzdanBakiye;
         row.MsisdnDogrulama = _model.MsisdnDogrulama;
         row.FcmRegistrationToken = _model.FcmRegistrationToken;
         row.MobileAppState = _model.MobileAppState;

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


