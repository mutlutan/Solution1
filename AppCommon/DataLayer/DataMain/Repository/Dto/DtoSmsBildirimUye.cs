using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AppCommon;
using AppData.Main.Models;

namespace AppData.Main.Repository.Dto
{
    public partial class DtoSmsBildirimUye : SmsBildirimUye
    {
        protected readonly DataContext dataContext;

        public string CcUyeIdAd { get; set; } = "";

        //Constructor
        public DtoSmsBildirimUye(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

    }
}


