using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AppCommon;
using AppCommon.DataLayer.DataMain.Models;

namespace AppCommon.DataLayer.DataMain.Repository.Dto
{
    public partial class DtoFiyat : Fiyat
    {
        protected readonly MainDataContext dataContext;

        public string CcTarifeIdAd { get; set; } = "";
        public string CcUyeGrupIdAd { get; set; } = "";

        //Constructor
        public DtoFiyat(MainDataContext dataContext)
        {
            this.dataContext = dataContext;
        }

    }
}


