using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace AppCommon.DataLayer.DataMain.Models
{
    public partial class Bolge
    {
        public Bolge()
        {
            BolgeDetay = new HashSet<BolgeDetay>();
        }

        public int Id { get; set; }
        public Guid UniqueId { get; set; }
        public bool Durum { get; set; }
        public Geometry? Polygon { get; set; }

        public virtual ICollection<BolgeDetay> BolgeDetay { get; set; }
    }
}
