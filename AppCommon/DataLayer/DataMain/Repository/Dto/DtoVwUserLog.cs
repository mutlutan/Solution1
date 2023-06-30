using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AppCommon;
using AppCommon.DataLayer.DataMain.Models;

namespace AppCommon.DataLayer.DataMain.Repository.Dto
{
    public partial class DtoVwUserLog : VwUserLog
    {
        protected readonly MainDataContext dataContext;


        //Constructor
        public DtoVwUserLog(MainDataContext dataContext)
        {
            this.dataContext = dataContext;
        }

    }
}


