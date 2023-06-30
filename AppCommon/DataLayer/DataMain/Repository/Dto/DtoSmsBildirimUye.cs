using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AppCommon;
using AppCommon.DataLayer.DataMain.Models;

namespace AppCommon.DataLayer.DataMain.Repository.Dto
{
    public partial class DtoSmsBildirimUye : SmsBildirimUye
    {
        protected readonly MainDataContext dataContext;

        public string CcUyeIdAd { get; set; } = "";

        //Constructor
        public DtoSmsBildirimUye(MainDataContext dataContext)
        {
            this.dataContext = dataContext;
        }

    }
}


