using System;
using System.Collections.Generic;

namespace AppCommon.DataLayer.DataMain.Models
{
    public partial class CuzdanHareketTur
    {
        public CuzdanHareketTur()
        {
            UyeCuzdanHareket = new HashSet<UyeCuzdanHareket>();
        }

        public int Id { get; set; }
        public string? Ad { get; set; }

        public virtual ICollection<UyeCuzdanHareket> UyeCuzdanHareket { get; set; }
    }
}
