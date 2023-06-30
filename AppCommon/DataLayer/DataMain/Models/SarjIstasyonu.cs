using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace AppCommon.DataLayer.DataMain.Models
{
    public partial class SarjIstasyonu
    {
        public SarjIstasyonu()
        {
            SarjIstasyonuHareket = new HashSet<SarjIstasyonuHareket>();
        }

        public int Id { get; set; }
        public Guid UniqueId { get; set; }
        public int Sira { get; set; }
        public bool Durum { get; set; }
        public string? Ad { get; set; }
        public string? Aciklama { get; set; }
        public bool MusaitlikDurum { get; set; }
        public Geometry? Konum { get; set; }
        public string? YazilimVersiyon { get; set; }
        public string? YazilimVersiyonNo { get; set; }
        public string? ModelNo { get; set; }
        public string? SeriNo { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? CreatedUserId { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? UpdatedUserId { get; set; }

        public virtual ICollection<SarjIstasyonuHareket> SarjIstasyonuHareket { get; set; }
    }
}
