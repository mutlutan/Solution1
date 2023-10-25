using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AppCommon;
using AppCommon.DataLayer.DataMain.Models;

namespace AppCommon.DataLayer.DataMain.Repository.Dto
{
    public partial class DtoParameter : Parameter
    {
        protected readonly MainDataContext dataContext;

        public string CcUseAuthenticator
        {
            get { return (this.UseAuthenticator ? this.dataContext.TranslateTo("xLng.Aktif") : this.dataContext.TranslateTo("xLng.Pasif")); }
        }
        public string CcAuditLog
        {
            get { return (this.AuditLog ? this.dataContext.TranslateTo("xLng.Aktif") : this.dataContext.TranslateTo("xLng.Pasif")); }
        }
        public string CcAuditLogTables
        {
           get
           {
               string rV = string.Empty;
               try
               {
                   rV = this.AuditLogTables.MyToTrim();
               }
               catch { }
               return rV;
           }
        }
        public string CcEmailEnableSsl
        {
            get { return (this.EmailEnableSsl ? this.dataContext.TranslateTo("xLng.Aktif") : this.dataContext.TranslateTo("xLng.Pasif")); }
        }

        //Constructor
        public DtoParameter(MainDataContext dataContext)
        {
            this.dataContext = dataContext;
        }

    }
}


