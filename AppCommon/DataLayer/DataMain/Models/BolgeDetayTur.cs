using System;
using System.Collections.Generic;

namespace AppCommon.DataLayer.DataMain.Models
{
    public partial class BolgeDetayTur
    {
        public BolgeDetayTur()
        {
            BolgeDetay = new HashSet<BolgeDetay>();
        }

        public int Id { get; set; }
        public string? Ad { get; set; }

        public virtual ICollection<BolgeDetay> BolgeDetay { get; set; }
    }
}
