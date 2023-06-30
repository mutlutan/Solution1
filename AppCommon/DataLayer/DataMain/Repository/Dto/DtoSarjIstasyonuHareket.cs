using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AppCommon;
using AppCommon.DataLayer.DataMain.Models;

namespace AppCommon.DataLayer.DataMain.Repository.Dto
{
    public partial class DtoSarjIstasyonuHareket : SarjIstasyonuHareket
    {
        protected readonly MainDataContext dataContext;

        public string CcSarjIstasyonuIdAd { get; set; } = "";
        public string CcAracIdAd { get; set; } = "";

        //Constructor
        public DtoSarjIstasyonuHareket(MainDataContext dataContext)
        {
            this.dataContext = dataContext;
        }

    }
}


