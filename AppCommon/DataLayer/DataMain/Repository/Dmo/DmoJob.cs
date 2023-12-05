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
    public class DmoJob : BaseDmo
    {
        public DmoJob(MainDataContext dataContext) : base(dataContext) { }

        public IQueryable<DtoJob> Get()
        {
            return this.dataContext.Job.AsNoTracking()
                .Select(s => new DtoJob(this.dataContext)
                {
                    Id = s.Id,
                    IsActive = s.IsActive,
                    MethodName = s.MethodName,
                    MethodParams = s.MethodParams,
                    MethodComment = s.MethodComment,
                    CronExpression = s.CronExpression,
                    IsPeriodic = s.IsPeriodic,
                    StartTime = s.StartTime,
                    DaysOfTheWeek = s.DaysOfTheWeek,
                    Months = s.Months,
                    DaysOfTheMonth = s.DaysOfTheMonth
                });
     }

        public DtoJob GetByNew()
        {
            //Default değerler ile bir row döner, Burada field default değerleri veriliyor...
            DtoJob row = new(this.dataContext) {
                IsActive = true,
                IsPeriodic = true
            };

            return row;
        }

        public IEnumerable<DtoJob> GetById(Guid _id)
        {
            return this.Get().Where(c => c.Id == _id);
        }

        public Guid CreateOrUpdate(Job _model, Boolean isNew)
        {
            Job? row;
            
            if (isNew)
            {
                //sadece insertte eklenip, update de değişmeyecek alanlar buraya
                row = new Job() {
                    Id = Guid.NewGuid()
                };
            }
            else
            {
                row = this.dataContext.Job.Where(c => c.Id == _model.Id).FirstOrDefault();
                if (row == null)
                {
                    throw new Exception(this.dataContext.TranslateTo("xLng.IslemYapilacakKayitBulunamadi"));
                }
            }

         row.IsActive = _model.IsActive;
         row.MethodName = _model.MethodName;
         row.MethodParams = _model.MethodParams;
         row.MethodComment = _model.MethodComment;
         row.CronExpression = _model.CronExpression;

         if (!isNew)
         {
             //sadece update eklenip, insertte de değişmeyecek alanlar buraya
         }
         
         if (isNew)
         {
             this.dataContext.Job.Add(row);
         }
         
         return row.Id;
     }

     public bool Delete(Guid _id)
     {
         Boolean rV = false;

         var row = this.dataContext.Job.Where(c => c.Id == _id).FirstOrDefault();
         if (row != null)
         {
             this.dataContext.Job.Remove(row);
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


