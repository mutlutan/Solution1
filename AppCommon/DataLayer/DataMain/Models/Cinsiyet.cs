using System;
using System.Collections.Generic;

namespace AppCommon.DataLayer.DataMain.Models
{
    public partial class Cinsiyet
    {
        public Cinsiyet()
        {
            Uye = new HashSet<Uye>();
        }

        public int Id { get; set; }
        public bool Durum { get; set; }
        public int Sira { get; set; }
        public string Ad { get; set; } = null!;
        public string AdEng { get; set; } = null!;

        public virtual ICollection<Uye> Uye { get; set; }
    }
}
