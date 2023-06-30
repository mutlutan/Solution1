using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AppCommon;
using AppData.Main.Models;

namespace AppData.Main.Repository.Dto
{
    public partial class DtoAracHareket : AracHareket
    {
        protected readonly DataContext dataContext;

        public string CcAracIdAd { get; set; } = "";
        public string CcUyeIdAd { get; set; } = "";

        //Constructor
        public DtoAracHareket(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

    }
}


