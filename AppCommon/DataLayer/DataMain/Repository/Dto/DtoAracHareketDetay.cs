using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AppCommon;
using AppData.Main.Models;

namespace AppData.Main.Repository.Dto
{
    public partial class DtoAracHareketDetay : AracHareketDetay
    {
        protected readonly DataContext dataContext;


        //Constructor
        public DtoAracHareketDetay(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

    }
}


