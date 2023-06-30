using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AppCommon;
using AppData.Main.Models;

namespace AppData.Main.Repository.Dto
{
    public partial class DtoSarjIstasyonu : SarjIstasyonu
    {
        protected readonly DataContext dataContext;

        public string CcDurum
        {
            get { return (this.Durum ? this.dataContext.TranslateTo("xLng.Aktif") : this.dataContext.TranslateTo("xLng.Pasif")); }
        }
        public string CcMusaitlikDurum
        {
            get { return (this.MusaitlikDurum ? this.dataContext.TranslateTo("xLng.Aktif") : this.dataContext.TranslateTo("xLng.Pasif")); }
        }

        //Constructor
        public DtoSarjIstasyonu(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

    }
}


