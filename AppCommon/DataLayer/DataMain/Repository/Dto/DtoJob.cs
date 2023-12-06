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

        //Constructor
        public DtoJob(MainDataContext dataContext)
        {
            this.dataContext = dataContext;
        }

    }
}


