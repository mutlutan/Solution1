using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AppCommon;
using AppCommon.DataLayer.DataMain.Models;

namespace AppCommon.DataLayer.DataMain.Repository.Dto
{
    public partial class DtoAracHareket : AracHareket
    {
        protected readonly MainDataContext dataContext;

        public string CcAracIdAd { get; set; } = "";
        public string CcUyeIdAd { get; set; } = "";

        //Constructor
        public DtoAracHareket(MainDataContext dataContext)
        {
            this.dataContext = dataContext;
        }

    }
}


