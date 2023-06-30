using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AppCommon;
using AppData.Main.Models;

namespace AppData.Main.Repository.Dto
{
    public partial class DtoVwUserLog : VwUserLog
    {
        protected readonly DataContext dataContext;


        //Constructor
        public DtoVwUserLog(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

    }
}


