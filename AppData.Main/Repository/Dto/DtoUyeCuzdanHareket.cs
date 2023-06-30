using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AppCommon;
using AppData.Main.Models;

namespace AppData.Main.Repository.Dto
{
    public partial class DtoUyeCuzdanHareket : UyeCuzdanHareket
    {
        protected readonly DataContext dataContext;

        public string CcUyeIdAd { get; set; } = "";
        public string CcCuzdanHareketTurIdAd { get; set; } = "";

        //Constructor
        public DtoUyeCuzdanHareket(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

    }
}


