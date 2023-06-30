using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AppData.Main.Models;
using AppCommon;

namespace AppData.Main.Repository.Dto
{
    public partial class DtoMobilBildirim : MobilBildirim
    {
        protected readonly DataContext dataContext;

        public string CcGonderildiMi
        {
            get { return (this.GonderildiMi ? this.dataContext.TranslateTo("xLng.Aktif") : this.dataContext.TranslateTo("xLng.Pasif")); }
        }
        public string CcUyeGrupIdsAd{
            get {
                string rV = string.Empty;
                try
                {
                    if (!string.IsNullOrEmpty(this.UyeGrupIds) && this.UyeGrupIds.MyToTrim().Length > 0)
                    {
                        foreach (string s in this.UyeGrupIds.Split(','))
                        {
                            int id = Convert.ToInt32(s.MyToInt());
                            if (rV != string.Empty) { rV += " | "; }
                            var refDataAd = this.dataContext.UyeGrup.Where(c => c.Id == id).FirstOrDefault();
                            if (refDataAd != null)
                            {
                                rV += refDataAd.Ad.MyToTrim();
                            }
                        }
                    }
                }
                catch { }
                return rV;
            }
        }

        //Constructor
        public DtoMobilBildirim(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

    }
}


