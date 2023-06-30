using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AppCommon;
using AppCommon.DataLayer.DataMain.Models;

namespace AppCommon.DataLayer.DataMain.Repository.Dto
{
    public partial class DtoEmailTemplate : EmailTemplate
    {
        protected readonly MainDataContext dataContext;

        public string CcEmailLetterheadIdDescription { get; set; } = "";

        //Constructor
        public DtoEmailTemplate(MainDataContext dataContext)
        {
            this.dataContext = dataContext;
        }

    }
}


