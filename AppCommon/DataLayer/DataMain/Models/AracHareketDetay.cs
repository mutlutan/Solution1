using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace AppCommon.DataLayer.DataMain.Models
{
    public partial class AracHareketDetay
    {
        public int Id { get; set; }
        public int AracHareketId { get; set; }
        public Geometry? Konum { get; set; }
        public DateTime Tarih { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? CreatedUserId { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? UpdatedUserId { get; set; }
        public int ReportId { get; set; }

        public virtual AracHareket AracHareket { get; set; } = null!;
    }
}
