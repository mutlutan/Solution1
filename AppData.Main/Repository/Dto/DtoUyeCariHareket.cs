using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AppCommon;
using AppData.Main.Models;

namespace AppData.Main.Repository.Dto
{
    public partial class DtoUyeCariHareket : UyeCariHareket
    {
        protected readonly DataContext dataContext;

        public string CcUyeIdAd { get; set; } = "";
        public string CcCariHareketTurIdAd { get; set; } = "";

        //Constructor
        public DtoUyeCariHareket(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

    }
}


