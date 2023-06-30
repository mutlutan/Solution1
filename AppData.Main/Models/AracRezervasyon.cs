using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace AppData.Main.Models
{
    public partial class AracRezervasyon
    {
        public int Id { get; set; }
        public Guid UniqueId { get; set; }
        public int AracId { get; set; }
        public int UyeId { get; set; }
        public int AracRezervasyonDurumId { get; set; }
        public DateTime BaslangicTarih { get; set; }
        public DateTime BitisTarih { get; set; }
        public int Sure { get; set; }
        public Geometry? Konum { get; set; }

        public virtual Arac Arac { get; set; } = null!;
        public virtual AracRezervasyonDurum AracRezervasyonDurum { get; set; } = null!;
        public virtual Uye Uye { get; set; } = null!;
    }
}
