using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AppCommon;
using AppCommon.DataLayer.DataMain.Models;

namespace AppCommon.DataLayer.DataMain.Repository.Dto
{
    public partial class DtoJob : Job
    {
        protected readonly MainDataContext dataContext;

        public string CcIsActive
        {
            get { return (this.IsActive ? this.dataContext.TranslateTo("xLng.Aktif") : this.dataContext.TranslateTo("xLng.Pasif")); }
        }
        public string CcIsPeriodic
        {
            get { return (this.IsPeriodic ? this.dataContext.TranslateTo("xLng.Evet") : this.dataContext.TranslateTo("xLng.Hayir")); }
        }
        public string CcDaysOfTheWeek
        {
           get
           {
               string rV = string.Empty;
               try
               {
                   rV = this.DaysOfTheWeek.MyToTrim();
               }
               catch { }
               return rV;
           }
        }
        public string CcMonths
        {
           get
           {
               string rV = string.Empty;
               try
               {
                   rV = this.Months.MyToTrim();
               }
               catch { }
               return rV;
           }
        }
        public string CcDaysOfTheMonth
        {
           get
           {
               string rV = string.Empty;
               try
               {
                   rV = this.DaysOfTheMonth.MyToTrim();
               }
               catch { }
               return rV;
           }
        }

        //Constructor
        public DtoJob(MainDataContext dataContext)
        {
            this.dataContext = dataContext;
        }

    }
}


