using System;
using System.Collections.Generic;

namespace AppData.Main.Models
{
    public partial class UyeKaraListe
    {
        public int Id { get; set; }
        public Guid UniqueId { get; set; }
        public int UyeId { get; set; }
        public DateTime BaslangicTarih { get; set; }
        public DateTime BitisTarih { get; set; }
        public string? Aciklama { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? CreatedUserId { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? UpdatedUserId { get; set; }

        public virtual Uye Uye { get; set; } = null!;
    }
}
