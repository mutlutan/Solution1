using System;
using System.Collections.Generic;

namespace AppData.Main.Models
{
    public partial class UyeFaturaBilgisi
    {
        public int Id { get; set; }
        public int UyeId { get; set; }
        public decimal Tutar { get; set; }
        public DateTime Tarih { get; set; }
        public string? Aciklama { get; set; }
        public string? DokumanUrl { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? CreatedUserId { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? UpdatedUserId { get; set; }

        public virtual Uye Uye { get; set; } = null!;
    }
}
