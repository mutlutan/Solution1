using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AppCommon;
using AppCommon.DataLayer.DataMain.Models;

namespace AppCommon.DataLayer.DataMain.Repository.Dto
{
    public partial class DtoArac : Arac
    {
        protected readonly MainDataContext dataContext;

        public string CcDurum
        {
            get { return (this.Durum ? this.dataContext.TranslateTo("xLng.Aktif") : this.dataContext.TranslateTo("xLng.Pasif")); }
        }
        public string CcSarjOluyorMu
        {
            get { return (this.SarjOluyorMu ? this.dataContext.TranslateTo("xLng.Aktif") : this.dataContext.TranslateTo("xLng.Pasif")); }
        }
        public string CcArizaDurumu
        {
            get { return (this.ArizaDurumu ? this.dataContext.TranslateTo("xLng.Aktif") : this.dataContext.TranslateTo("xLng.Pasif")); }
        }
        public string CcKilitDurumu
        {
            get { return (this.KilitDurumu ? this.dataContext.TranslateTo("xLng.Aktif") : this.dataContext.TranslateTo("xLng.Pasif")); }
        }
        public string CcBlokeDurum
        {
            get { return (this.BlokeDurum ? this.dataContext.TranslateTo("xLng.Aktif") : this.dataContext.TranslateTo("xLng.Pasif")); }
        }
        public string CcAcilUyariIstemi
        {
            get { return (this.AcilUyariIstemi ? this.dataContext.TranslateTo("xLng.Aktif") : this.dataContext.TranslateTo("xLng.Pasif")); }
        }

        //Constructor
        public DtoArac(MainDataContext dataContext)
        {
            this.dataContext = dataContext;
        }

    }
}


