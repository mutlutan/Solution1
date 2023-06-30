using System;
using System.Collections.Generic;

namespace AppData.Main.Models
{
    public partial class Fiyat
    {
        public int Id { get; set; }
        public Guid UniqueId { get; set; }
        public int TarifeId { get; set; }
        public int UyeGrupId { get; set; }
        public decimal? HaftaIciFiyat { get; set; }
        public decimal? HaftaSonuFiyat { get; set; }
        public decimal? BayramFiyat { get; set; }

        public virtual Tarife Tarife { get; set; } = null!;
        public virtual UyeGrup UyeGrup { get; set; } = null!;
    }
}
