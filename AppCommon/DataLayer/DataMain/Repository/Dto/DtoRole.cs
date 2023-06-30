using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AppCommon;
using AppData.Main.Models;

namespace AppData.Main.Repository.Dto
{
    public partial class DtoRole : Role
    {
        protected readonly DataContext dataContext;

        public string CcIsActive
        {
            get { return (this.IsActive ? this.dataContext.TranslateTo("xLng.Aktif") : this.dataContext.TranslateTo("xLng.Pasif")); }
        }

        //Constructor
        public DtoRole(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

    }
}


