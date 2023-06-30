using System;
using System.Collections.Generic;

namespace AppCommon.DataLayer.DataMain.Models
{
    public partial class UyeCariHareket
    {
        public int Id { get; set; }
        public Guid UniqueId { get; set; }
        public int UyeId { get; set; }
        public int CariHareketTurId { get; set; }
        public decimal Borc { get; set; }
        public decimal Alacak { get; set; }
        public DateTime Tarih { get; set; }
        public string? Aciklama { get; set; }

        public virtual CariHareketTur CariHareketTur { get; set; } = null!;
        public virtual Uye Uye { get; set; } = null!;
    }
}
