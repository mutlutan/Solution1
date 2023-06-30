using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AppCommon;
using AppData.Main.Models;

namespace AppData.Main.Repository.Dto
{
    public partial class DtoEmailTemplate : EmailTemplate
    {
        protected readonly DataContext dataContext;

        public string CcEmailLetterheadIdDescription { get; set; } = "";

        //Constructor
        public DtoEmailTemplate(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

    }
}


