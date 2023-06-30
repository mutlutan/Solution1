using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AppCommon;
using AppData.Main.Models;

namespace AppData.Main.Repository.Dto
{
    public partial class DtoEmailPool : EmailPool
    {
        protected readonly DataContext dataContext;

        public string CcEmailTemplateIdName { get; set; } = "";
        public string CcEmailPoolStatusIdName { get; set; } = "";

        //Constructor
        public DtoEmailPool(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

    }
}


