using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AppData.Main.Models;
using AppCommon;

namespace AppData.Main.Repository.Dto
{
    public partial class DtoUser : User
    {
        protected readonly DataContext dataContext;

        public string CcIsActive
        {
            get { return (this.IsActive ? this.dataContext.TranslateTo("xLng.Aktif") : this.dataContext.TranslateTo("xLng.Pasif")); }
        }
        public string CcIsEmailConfirmed
        {
            get { return (this.IsEmailConfirmed ? this.dataContext.TranslateTo("xLng.Aktif") : this.dataContext.TranslateTo("xLng.Pasif")); }
        }
        public string CcRoleIdsName{
            get {
                string rV = string.Empty;
                try
                {
                    if (!string.IsNullOrEmpty(this.RoleIds) && this.RoleIds.MyToTrim().Length > 0)
                    {
                        foreach (string s in this.RoleIds.Split(','))
                        {
                            int id = Convert.ToInt32(s.MyToInt());
                            if (rV != string.Empty) { rV += " | "; }
                            var refDataName = this.dataContext.Role.Where(c => c.Id == id).FirstOrDefault();
                            if (refDataName != null)
                            {
                                rV += refDataName.Name.MyToTrim();
                            }
                        }
                    }
                }
                catch { }
                return rV;
            }
        }

        //Constructor
        public DtoUser(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

    }
}


