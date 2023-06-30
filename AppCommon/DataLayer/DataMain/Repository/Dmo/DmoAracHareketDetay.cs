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
    public class DmoAracHareketDetay : BaseDmo
    {
        public DmoAracHareketDetay(MainDataContext dataContext) : base(dataContext) { }

        public IQueryable<DtoAracHareketDetay> Get()
        {
            return this.dataContext.AracHareketDetay.AsNoTracking()
                .Select(s => new DtoAracHareketDetay(this.dataContext)
                {
                    Id = s.Id,
                    AracHareketId = s.AracHareketId,
                    Konum = s.Konum,
                    Tarih = s.Tarih,
                    CreateDate = s.CreateDate,
                    CreatedUserId = s.CreatedUserId,
                    UpdateDate = s.UpdateDate,
                    UpdatedUserId = s.UpdatedUserId,
                    ReportId = s.ReportId
                });
     }

        public DtoAracHareketDetay GetByNew()
        {
            //Default değerler ile bir row döner, Burada field default değerleri veriliyor...
            DtoAracHareketDetay row = new(this.dataContext) {
                Id = 0,
                AracHareketId = 0,
                CreateDate = DateTime.Now,
                CreatedUserId = this.dataContext.UserId,
                UpdatedUserId = 0,
                ReportId = 0
            };

            return row;
        }

        public IEnumerable<DtoAracHareketDetay> GetById(Int32 _id)
        {
            return this.Get().Where(c => c.Id == _id);
        }

        public Int32 CreateOrUpdate(AracHareketDetay _model, Boolean isNew)
        {
            AracHareketDetay? row;
            
            if (isNew)
            {
                //sadece insertte eklenip, update de değişmeyecek alanlar buraya
                row = new AracHareketDetay() {
                    Id = (int)this.dataContext.GetNextSequenceValue("sqAracHareketDetay")
                };
            }
            else
            {
                row = this.dataContext.AracHareketDetay.Where(c => c.Id == _model.Id).FirstOrDefault();
                if (row == null)
                {
                    throw new Exception(this.dataContext.TranslateTo("xLng.IslemYapilacakKayitBulunamadi"));
                }
            }

         row.AracHareketId = _model.AracHareketId;
         row.Konum = _model.Konum;
         row.Tarih = _model.Tarih;
         row.ReportId = _model.ReportId;
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
             this.dataContext.AracHareketDetay.Add(row);
         }
         
         return row.Id;
     }

     public bool Delete(Int32 _id)
     {
         Boolean rV = false;

         var row = this.dataContext.AracHareketDetay.Where(c => c.Id == _id).FirstOrDefault();
         if (row != null)
         {
             this.dataContext.AracHareketDetay.Remove(row);
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


