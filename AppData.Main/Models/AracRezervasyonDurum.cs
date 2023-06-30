using System;
using System.Collections.Generic;

namespace AppData.Main.Models
{
    public partial class AracRezervasyonDurum
    {
        public AracRezervasyonDurum()
        {
            AracRezervasyon = new HashSet<AracRezervasyon>();
        }

        public int Id { get; set; }
        public bool Durum { get; set; }
        public string? Ad { get; set; }

        public virtual ICollection<AracRezervasyon> AracRezervasyon { get; set; }
    }
}
