using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AppCommon;
using AppCommon.DataLayer.DataMain.Models;

namespace AppCommon.DataLayer.DataMain.Repository.Dto
{
    public partial class DtoUye : Uye
    {
        protected readonly MainDataContext dataContext;

        public string CcUyeDurumIdAd { get; set; } = "";
        public string CcUyeGrupIdAd { get; set; } = "";
        public string CcIsConfirmed
        {
            get { return (this.IsConfirmed ? this.dataContext.TranslateTo("xLng.Aktif") : this.dataContext.TranslateTo("xLng.Pasif")); }
        }

        //Constructor
        public DtoUye(MainDataContext dataContext)
        {
            this.dataContext = dataContext;
        }

    }
}


