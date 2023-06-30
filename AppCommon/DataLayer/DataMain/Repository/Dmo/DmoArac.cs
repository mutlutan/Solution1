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
    public class DmoArac : BaseDmo
    {
        public DmoArac(MainDataContext dataContext) : base(dataContext) { }

        public IQueryable<DtoArac> Get()
        {
            return this.dataContext.Arac.AsNoTracking()
                .Select(s => new DtoArac(this.dataContext)
                {
                    Id = s.Id,
                    UniqueId = s.UniqueId,
                    Sira = s.Sira,
                    Durum = s.Durum,
                    Ad = s.Ad,
                    Marka = s.Marka,
                    Model = s.Model,
                    ImeiNo = s.ImeiNo,
                    QrKod = s.QrKod,
                    Aciklama = s.Aciklama,
                    Resim = s.Resim,
                    SonKonum = s.SonKonum,
                    KilometreSayaci = s.KilometreSayaci,
                    SarjOrani = s.SarjOrani,
                    SarjOluyorMu = s.SarjOluyorMu,
                    ArizaDurumu = s.ArizaDurumu,
                    KilitDurumu = s.KilitDurumu,
                    BlokeDurum = s.BlokeDurum,
                    AcilUyariIstemi = s.AcilUyariIstemi,
                    CreateDate = s.CreateDate,
                    CreatedUserId = s.CreatedUserId,
                    UpdateDate = s.UpdateDate,
                    UpdatedUserId = s.UpdatedUserId
                });
     }

        public DtoArac GetByNew()
        {
            //Default değerler ile bir row döner, Burada field default değerleri veriliyor...
            DtoArac row = new(this.dataContext) {
                Id = 0,
                UniqueId = Guid.NewGuid(),
                Sira = 0,
                Durum = true,
                KilometreSayaci = 0,
                SarjOrani = 0,
                SarjOluyorMu = true,
                ArizaDurumu = false,
                KilitDurumu = false,
                BlokeDurum = false,
                AcilUyariIstemi = false,
                CreateDate = DateTime.Now,
                CreatedUserId = this.dataContext.UserId,
                UpdatedUserId = 0
            };

            return row;
        }

        public IEnumerable<DtoArac> GetById(Int32 _id)
        {
            return this.Get().Where(c => c.Id == _id);
        }

        public Int32 CreateOrUpdate(Arac _model, Boolean isNew)
        {
            Arac? row;
            
            if (isNew)
            {
                //sadece insertte eklenip, update de değişmeyecek alanlar buraya
                row = new Arac() {
                    Id = (int)this.dataContext.GetNextSequenceValue("sqArac")
                };
            }
            else
            {
                row = this.dataContext.Arac.Where(c => c.Id == _model.Id && c.UniqueId == _model.UniqueId).FirstOrDefault();
                if (row == null)
                {
                    throw new Exception(this.dataContext.TranslateTo("xLng.IslemYapilacakKayitBulunamadi"));
                }
            }

         row.UniqueId = _model.UniqueId;
         row.Sira = _model.Sira;
         row.Durum = _model.Durum;
         row.Ad = _model.Ad;
         row.Marka = _model.Marka;
         row.Model = _model.Model;
         row.ImeiNo = _model.ImeiNo;
         row.QrKod = _model.QrKod;
         row.Aciklama = _model.Aciklama;
         row.Resim = _model.Resim;
         row.SonKonum = _model.SonKonum;
         row.KilometreSayaci = _model.KilometreSayaci;
         row.SarjOrani = _model.SarjOrani;
         row.SarjOluyorMu = _model.SarjOluyorMu;
         row.ArizaDurumu = _model.ArizaDurumu;
         row.KilitDurumu = _model.KilitDurumu;
         row.BlokeDurum = _model.BlokeDurum;
         row.AcilUyariIstemi = _model.AcilUyariIstemi;
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
             this.dataContext.Arac.Add(row);
         }
         
         return row.Id;
     }

     public bool Delete(Int32 _id)
     {
         Boolean rV = false;

         var row = this.dataContext.Arac.Where(c => c.Id == _id).FirstOrDefault();
         if (row != null)
         {
             this.dataContext.Arac.Remove(row);
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


