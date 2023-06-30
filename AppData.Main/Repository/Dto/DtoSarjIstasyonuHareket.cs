using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AppCommon;
using AppData.Main.Models;

namespace AppData.Main.Repository.Dto
{
    public partial class DtoSarjIstasyonuHareket : SarjIstasyonuHareket
    {
        protected readonly DataContext dataContext;

        public string CcSarjIstasyonuIdAd { get; set; } = "";
        public string CcAracIdAd { get; set; } = "";

        //Constructor
        public DtoSarjIstasyonuHareket(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

    }
}


