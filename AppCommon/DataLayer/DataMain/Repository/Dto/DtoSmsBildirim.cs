using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AppCommon;
using AppCommon.DataLayer.DataMain.Models;

namespace AppCommon.DataLayer.DataMain.Repository.Dto
{
    public partial class DtoSmsBildirim : SmsBildirim
    {
        protected readonly MainDataContext dataContext;

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
        public DtoSmsBildirim(MainDataContext dataContext)
        {
            this.dataContext = dataContext;
        }

    }
}


