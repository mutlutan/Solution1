using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AppCommon;
using AppData.Main.Models;

namespace AppData.Main.Repository.Dto
{
    public partial class DtoParameter : Parameter
    {
        protected readonly DataContext dataContext;

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
        public DtoParameter(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

    }
}


