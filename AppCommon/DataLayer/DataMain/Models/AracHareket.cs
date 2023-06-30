using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace AppCommon.DataLayer.DataMain.Models
{
    public partial class AracHareket
    {
        public AracHareket()
        {
            AracHareketDetay = new HashSet<AracHareketDetay>();
            AracHareketResim = new HashSet<AracHareketResim>();
        }

        public int Id { get; set; }
        public Guid UniqueId { get; set; }
        public int AracId { get; set; }
        public int UyeId { get; set; }
        public decimal BirimFiyat { get; set; }
        public decimal Mesafe { get; set; }
        public decimal Tutar { get; set; }
        public DateTime BaslangicTarih { get; set; }
        public DateTime? BitisTarih { get; set; }
        public Geometry? BaslangicKonum { get; set; }
        public Geometry? BitisKonum { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? CreatedUserId { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? UpdatedUserId { get; set; }
        public int AracRezervasyonId { get; set; }

        public virtual Arac Arac { get; set; } = null!;
        public virtual Uye Uye { get; set; } = null!;
        public virtual ICollection<AracHareketDetay> AracHareketDetay { get; set; }
        public virtual ICollection<AracHareketResim> AracHareketResim { get; set; }
    }
}
