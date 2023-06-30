using System;
using System.Collections.Generic;

namespace AppData.Main.Models
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
