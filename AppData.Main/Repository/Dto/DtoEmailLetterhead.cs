using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AppCommon;
using AppData.Main.Models;

namespace AppData.Main.Repository.Dto
{
    public partial class DtoEmailLetterhead : EmailLetterhead
    {
        protected readonly DataContext dataContext;


        //Constructor
        public DtoEmailLetterhead(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

    }
}


