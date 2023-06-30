using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AppCommon;
using AppData.Main.Models;

namespace AppData.Main.Repository.Dto
{
    public partial class DtoUye : Uye
    {
        protected readonly DataContext dataContext;

        public string CcUyeDurumIdAd { get; set; } = "";
        public string CcUyeGrupIdAd { get; set; } = "";
        public string CcCinsiyetIdAd { get; set; } = "";
        public string CcUyelikDogrulama
        {
            get { return (this.UyelikDogrulama ? this.dataContext.TranslateTo("xLng.Aktif") : this.dataContext.TranslateTo("xLng.Pasif")); }
        }
        public string CcKvkkOnayi
        {
            get { return (this.KvkkOnayi ? this.dataContext.TranslateTo("xLng.Aktif") : this.dataContext.TranslateTo("xLng.Pasif")); }
        }
        public string CcUyelikSozlesmeOnayi
        {
            get { return (this.UyelikSozlesmeOnayi ? this.dataContext.TranslateTo("xLng.Aktif") : this.dataContext.TranslateTo("xLng.Pasif")); }
        }
        public string CcAydinlatmaMetniOnayi
        {
            get { return (this.AydinlatmaMetniOnayi ? this.dataContext.TranslateTo("xLng.Aktif") : this.dataContext.TranslateTo("xLng.Pasif")); }
        }
        public string CcMsisdnDogrulama
        {
            get { return (this.MsisdnDogrulama ? this.dataContext.TranslateTo("xLng.Aktif") : this.dataContext.TranslateTo("xLng.Pasif")); }
        }

        //Constructor
        public DtoUye(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

    }
}


