using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace AppCommon.DataLayer.DataMain.Models
{
    public partial class Arac
    {
        public Arac()
        {
            AracHareket = new HashSet<AracHareket>();
            AracOzellikDetay = new HashSet<AracOzellikDetay>();
            AracRezervasyon = new HashSet<AracRezervasyon>();
            SarjIstasyonuHareket = new HashSet<SarjIstasyonuHareket>();
        }

        public int Id { get; set; }
        public Guid UniqueId { get; set; }
        public int Sira { get; set; }
        public bool Durum { get; set; }
        public string? Ad { get; set; }
        public string? Marka { get; set; }
        public string? Model { get; set; }
        public string? ImeiNo { get; set; }
        public string? Aciklama { get; set; }
        public string? Resim { get; set; }
        public Geometry? SonKonum { get; set; }
        public decimal SarjOrani { get; set; }
        public bool ArizaDurumu { get; set; }
        public bool KilitDurumu { get; set; }
        public bool BlokeDurum { get; set; }
        public bool AcilUyariIstemi { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? CreatedUserId { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? UpdatedUserId { get; set; }
        public decimal KilometreSayaci { get; set; }
        public bool SarjOluyorMu { get; set; }
        public string? QrKod { get; set; }

        public virtual ICollection<AracHareket> AracHareket { get; set; }
        public virtual ICollection<AracOzellikDetay> AracOzellikDetay { get; set; }
        public virtual ICollection<AracRezervasyon> AracRezervasyon { get; set; }
        public virtual ICollection<SarjIstasyonuHareket> SarjIstasyonuHareket { get; set; }
    }
}
