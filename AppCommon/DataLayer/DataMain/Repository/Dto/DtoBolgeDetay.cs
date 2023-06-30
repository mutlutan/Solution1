using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AppCommon;
using AppData.Main.Models;

namespace AppData.Main.Repository.Dto
{
    public partial class DtoBolgeDetay : BolgeDetay
    {
        protected readonly DataContext dataContext;

        public string CcDurum
        {
            get { return (this.Durum ? this.dataContext.TranslateTo("xLng.Aktif") : this.dataContext.TranslateTo("xLng.Pasif")); }
        }
        public string CcParkEdilebilirMi
        {
            get { return (this.ParkEdilebilirMi ? this.dataContext.TranslateTo("xLng.Aktif") : this.dataContext.TranslateTo("xLng.Pasif")); }
        }
        public string CcBolgeDetayTurIdAd { get; set; } = "";

        //Constructor
        public DtoBolgeDetay(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

    }
}


