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
    public class DmoBolgeDetay : BaseDmo
    {
        public DmoBolgeDetay(DataContext dataContext) : base(dataContext) { }

        public IQueryable<DtoBolgeDetay> Get()
        {
            return this.dataContext.BolgeDetay.AsNoTracking()
                .Select(s => new DtoBolgeDetay(this.dataContext)
                {
                    Id = s.Id,
                    UniqueId = s.UniqueId,
                    Durum = s.Durum,
                    ParkEdilebilirMi = s.ParkEdilebilirMi,
                    BolgeId = s.BolgeId,
                    BolgeDetayTurId = s.BolgeDetayTurId,
                    Ad = s.Ad,
                    Polygon = s.Polygon,
                    CreateDate = s.CreateDate,
                    CreatedUserId = s.CreatedUserId,
                    UpdateDate = s.UpdateDate,
                    UpdatedUserId = s.UpdatedUserId,
                    CcBolgeDetayTurIdAd = s.BolgeDetayTur.Ad.MyToTrim()
                });
     }

        public DtoBolgeDetay GetByNew()
        {
            //Default değerler ile bir row döner, Burada field default değerleri veriliyor...
            DtoBolgeDetay row = new(this.dataContext) {
                Id = 0,
                UniqueId = Guid.NewGuid(),
                Durum = true,
                ParkEdilebilirMi = true,
                BolgeId = 0,
                BolgeDetayTurId = 0,
                CreateDate = DateTime.Now,
                CreatedUserId = this.dataContext.UserId,
                UpdatedUserId = 0
            };

            return row;
        }

        public IEnumerable<DtoBolgeDetay> GetById(Int32 _id)
        {
            return this.Get().Where(c => c.Id == _id);
        }

        public Int32 CreateOrUpdate(BolgeDetay _model, Boolean isNew)
        {
            BolgeDetay? row;
            
            if (isNew)
            {
                //sadece insertte eklenip, update de değişmeyecek alanlar buraya
                row = new BolgeDetay() {
                    Id = (int)this.dataContext.GetNextSequenceValue("sqBolgeDetay")
                };
            }
            else
            {
                row = this.dataContext.BolgeDetay.Where(c => c.Id == _model.Id && c.UniqueId == _model.UniqueId).FirstOrDefault();
                if (row == null)
                {
                    throw new Exception(this.dataContext.TranslateTo("xLng.IslemYapilacakKayitBulunamadi"));
                }
            }

         row.UniqueId = _model.UniqueId;
         row.Durum = _model.Durum;
         row.ParkEdilebilirMi = _model.ParkEdilebilirMi;
         row.BolgeId = _model.BolgeId;
         row.BolgeDetayTurId = _model.BolgeDetayTurId;
         row.Ad = _model.Ad;
         row.Polygon = _model.Polygon;
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
             this.dataContext.BolgeDetay.Add(row);
         }
         
         return row.Id;
     }

     public bool Delete(Int32 _id)
     {
         Boolean rV = false;

         var row = this.dataContext.BolgeDetay.Where(c => c.Id == _id).FirstOrDefault();
         if (row != null)
         {
             this.dataContext.BolgeDetay.Remove(row);
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


