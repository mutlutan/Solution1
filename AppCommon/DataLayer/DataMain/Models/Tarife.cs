using System;
using System.Collections.Generic;

namespace AppCommon.DataLayer.DataMain.Models
{
    public partial class Tarife
    {
        public Tarife()
        {
            Fiyat = new HashSet<Fiyat>();
        }

        public int Id { get; set; }
        public Guid UniqueId { get; set; }
        public bool Durum { get; set; }
        public DateTime BaslangicTarihi { get; set; }
        public string? Ad { get; set; }

        public virtual ICollection<Fiyat> Fiyat { get; set; }
    }
}
