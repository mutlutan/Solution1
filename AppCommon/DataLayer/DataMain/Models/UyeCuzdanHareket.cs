using System;
using System.Collections.Generic;

namespace AppCommon.DataLayer.DataMain.Models
{
    public partial class UyeCuzdanHareket
    {
        public int Id { get; set; }
        public Guid UniqueId { get; set; }
        public int UyeId { get; set; }
        public int CuzdanHareketTurId { get; set; }
        public decimal Borc { get; set; }
        public decimal Alacak { get; set; }
        public DateTime Tarih { get; set; }
        public string? Aciklama { get; set; }

        public virtual CuzdanHareketTur CuzdanHareketTur { get; set; } = null!;
        public virtual Uye Uye { get; set; } = null!;
    }
}
