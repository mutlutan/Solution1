using System;
using System.Collections.Generic;

namespace AppCommon.DataLayer.DataMain.Models
{
    public partial class KampanyaTur
    {
        public KampanyaTur()
        {
            Kampanya = new HashSet<Kampanya>();
        }

        public int Id { get; set; }
        public bool Durum { get; set; }
        public int Sira { get; set; }
        public string? Ad { get; set; }
        public string? AdEng { get; set; }

        public virtual ICollection<Kampanya> Kampanya { get; set; }
    }
}
