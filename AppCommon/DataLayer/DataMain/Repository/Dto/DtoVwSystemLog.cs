using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AppCommon;
using AppCommon.DataLayer.DataMain.Models;

namespace AppCommon.DataLayer.DataMain.Repository.Dto
{
    public partial class DtoVwSystemLog : VwSystemLog
    {
        protected readonly MainDataContext dataContext;


        //Constructor
        public DtoVwSystemLog(MainDataContext dataContext)
        {
            this.dataContext = dataContext;
        }

    }
}


