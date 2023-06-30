using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AppCommon;
using AppCommon.DataLayer.DataMain.Models;

namespace AppCommon.DataLayer.DataMain.Repository.Dto
{
    public partial class DtoKampanya : Kampanya
    {
        protected readonly MainDataContext dataContext;

        public string CcDurum
        {
            get { return (this.Durum ? this.dataContext.TranslateTo("xLng.Aktif") : this.dataContext.TranslateTo("xLng.Pasif")); }
        }
        public string CcKampanyaIndirimTipiIdAd { get; set; } = "";
        public string CcKampanyaTurIdAd { get; set; } = "";
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
        public string CcCokluKullanim
        {
            get { return (this.CokluKullanim ? this.dataContext.TranslateTo("xLng.Aktif") : this.dataContext.TranslateTo("xLng.Pasif")); }
        }

        //Constructor
        public DtoKampanya(MainDataContext dataContext)
        {
            this.dataContext = dataContext;
        }

    }
}


