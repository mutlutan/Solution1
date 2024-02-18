using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AppCommon;
using AppCommon.DataLayer.DataMain.Models;

namespace AppCommon.DataLayer.DataMain.Repository.Dto
{
    public partial class DtoCustomer : Customer
    {
        protected readonly MainDataContext dataContext;

        public string CcUserStatusIdName { get; set; } = "";
        public string CcCustomerTypeIdName { get; set; } = "";
        public string CcIsEmailConfirmed
        {
            get { return (this.IsEmailConfirmed ? this.dataContext.TranslateTo("xLng.Aktif") : this.dataContext.TranslateTo("xLng.Pasif")); }
        }

        //Constructor
        public DtoCustomer(MainDataContext dataContext)
        {
            this.dataContext = dataContext;
        }

    }
}


