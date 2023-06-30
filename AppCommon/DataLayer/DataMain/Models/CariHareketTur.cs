using System;
using System.Collections.Generic;

namespace AppCommon.DataLayer.DataMain.Models
{
    public partial class CariHareketTur
    {
        public CariHareketTur()
        {
            UyeCariHareket = new HashSet<UyeCariHareket>();
        }

        public int Id { get; set; }
        public string? Ad { get; set; }

        public virtual ICollection<UyeCariHareket> UyeCariHareket { get; set; }
    }
}
