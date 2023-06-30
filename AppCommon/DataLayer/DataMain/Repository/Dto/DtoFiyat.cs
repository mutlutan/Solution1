using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AppCommon;
using AppData.Main.Models;

namespace AppData.Main.Repository.Dto
{
    public partial class DtoFiyat : Fiyat
    {
        protected readonly DataContext dataContext;

        public string CcTarifeIdAd { get; set; } = "";
        public string CcUyeGrupIdAd { get; set; } = "";

        //Constructor
        public DtoFiyat(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

    }
}


