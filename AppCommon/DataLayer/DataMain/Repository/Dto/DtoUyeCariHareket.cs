using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AppCommon;
using AppCommon.DataLayer.DataMain.Models;

namespace AppCommon.DataLayer.DataMain.Repository.Dto
{
    public partial class DtoUyeCariHareket : UyeCariHareket
    {
        protected readonly MainDataContext dataContext;

        public string CcUyeIdAd { get; set; } = "";
        public string CcCariHareketTurIdAd { get; set; } = "";

        //Constructor
        public DtoUyeCariHareket(MainDataContext dataContext)
        {
            this.dataContext = dataContext;
        }

    }
}


