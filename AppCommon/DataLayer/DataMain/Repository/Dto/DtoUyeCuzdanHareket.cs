using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AppCommon;
using AppCommon.DataLayer.DataMain.Models;

namespace AppCommon.DataLayer.DataMain.Repository.Dto
{
    public partial class DtoUyeCuzdanHareket : UyeCuzdanHareket
    {
        protected readonly MainDataContext dataContext;

        public string CcUyeIdAd { get; set; } = "";
        public string CcCuzdanHareketTurIdAd { get; set; } = "";

        //Constructor
        public DtoUyeCuzdanHareket(MainDataContext dataContext)
        {
            this.dataContext = dataContext;
        }

    }
}


