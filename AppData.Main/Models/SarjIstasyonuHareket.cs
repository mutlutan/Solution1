using System;
using System.Collections.Generic;

namespace AppData.Main.Models
{
    public partial class SarjIstasyonuHareket
    {
        public int Id { get; set; }
        public Guid UniqueId { get; set; }
        public int SarjIstasyonuId { get; set; }
        public int AracId { get; set; }
        public DateTime BaslangicTarih { get; set; }
        public DateTime? BitisTarih { get; set; }
        public DateTime? CreateDate { get; set; }

        public virtual Arac Arac { get; set; } = null!;
        public virtual SarjIstasyonu SarjIstasyonu { get; set; } = null!;
    }
}
