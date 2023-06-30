using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace AppData.Main.Models
{
    public partial class BolgeDetay
    {
        public int Id { get; set; }
        public Guid UniqueId { get; set; }
        public bool Durum { get; set; }
        public bool ParkEdilebilirMi { get; set; }
        public int BolgeId { get; set; }
        public int BolgeDetayTurId { get; set; }
        public string? Ad { get; set; }
        public Geometry? Polygon { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? CreatedUserId { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? UpdatedUserId { get; set; }

        public virtual Bolge Bolge { get; set; } = null!;
        public virtual BolgeDetayTur BolgeDetayTur { get; set; } = null!;
    }
}
