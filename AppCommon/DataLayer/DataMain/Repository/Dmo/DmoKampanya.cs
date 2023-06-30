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
    public class DmoKampanya : BaseDmo
    {
        public DmoKampanya(MainDataContext dataContext) : base(dataContext) { }

        public IQueryable<DtoKampanya> Get()
        {
            return this.dataContext.Kampanya.AsNoTracking()
                .Select(s => new DtoKampanya(this.dataContext)
                {
                    Id = s.Id,
                    Durum = s.Durum,
                    KampanyaIndirimTipiId = s.KampanyaIndirimTipiId,
                    KampanyaTurId = s.KampanyaTurId,
                    UyeGrupIds = s.UyeGrupIds,
                    Ad = s.Ad,
                    AdEng = s.AdEng,
                    GorselUrl = s.GorselUrl,
                    SayfaIcerik = s.SayfaIcerik,
                    SayfaIcerikEng = s.SayfaIcerikEng,
                    BaslangicTarihi = s.BaslangicTarihi,
                    BitisTarihi = s.BitisTarihi,
                    IndirimDegeri = s.IndirimDegeri,
                    CokluKullanim = s.CokluKullanim,
                    CcKampanyaIndirimTipiIdAd = s.KampanyaIndirimTipi.Ad.MyToTrim(),
                    CcKampanyaTurIdAd = s.KampanyaTur.Ad.MyToTrim()
                });
     }

        public DtoKampanya GetByNew()
        {
            //Default değerler ile bir row döner, Burada field default değerleri veriliyor...
            DtoKampanya row = new(this.dataContext) {
                Id = 0,
                Durum = true,
                KampanyaIndirimTipiId = 0,
                KampanyaTurId = 0,
                IndirimDegeri = 0,
                CokluKullanim = true
            };

            return row;
        }

        public IEnumerable<DtoKampanya> GetById(Int32 _id)
        {
            return this.Get().Where(c => c.Id == _id);
        }

        public Int32 CreateOrUpdate(Kampanya _model, Boolean isNew)
        {
            Kampanya? row;
            
            if (isNew)
            {
                //sadece insertte eklenip, update de değişmeyecek alanlar buraya
                row = new Kampanya() {
                    Id = (int)this.dataContext.GetNextSequenceValue("sqKampanya")
                };
            }
            else
            {
                row = this.dataContext.Kampanya.Where(c => c.Id == _model.Id).FirstOrDefault();
                if (row == null)
                {
                    throw new Exception(this.dataContext.TranslateTo("xLng.IslemYapilacakKayitBulunamadi"));
                }
            }

         row.Durum = _model.Durum;
         row.KampanyaIndirimTipiId = _model.KampanyaIndirimTipiId;
         row.KampanyaTurId = _model.KampanyaTurId;
         row.UyeGrupIds = _model.UyeGrupIds;
         row.Ad = _model.Ad;
         row.AdEng = _model.AdEng;
         row.GorselUrl = _model.GorselUrl;
         row.SayfaIcerik = _model.SayfaIcerik;
         row.SayfaIcerikEng = _model.SayfaIcerikEng;
         row.BaslangicTarihi = _model.BaslangicTarihi;
         row.BitisTarihi = _model.BitisTarihi;
         row.IndirimDegeri = _model.IndirimDegeri;
         row.CokluKullanim = _model.CokluKullanim;

         if (!isNew)
         {
             //sadece update eklenip, insertte de değişmeyecek alanlar buraya
         }
         
         if (isNew)
         {
             this.dataContext.Kampanya.Add(row);
         }
         
         return row.Id;
     }

     public bool Delete(Int32 _id)
     {
         Boolean rV = false;

         var row = this.dataContext.Kampanya.Where(c => c.Id == _id).FirstOrDefault();
         if (row != null)
         {
             this.dataContext.Kampanya.Remove(row);
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


