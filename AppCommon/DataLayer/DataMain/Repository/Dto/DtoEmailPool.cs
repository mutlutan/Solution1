using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AppCommon;
using AppCommon.DataLayer.DataMain.Models;

namespace AppCommon.DataLayer.DataMain.Repository.Dto
{
    public partial class DtoEmailPool : EmailPool
    {
        protected readonly MainDataContext dataContext;

        public string CcEmailTemplateIdName { get; set; } = "";
        public string CcEmailPoolStatusIdName { get; set; } = "";

        //Constructor
        public DtoEmailPool(MainDataContext dataContext)
        {
            this.dataContext = dataContext;
        }

    }
}


