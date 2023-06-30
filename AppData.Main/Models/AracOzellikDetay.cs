using System;
using System.Collections.Generic;

namespace AppData.Main.Models
{
    public partial class AracOzellikDetay
    {
        public int Id { get; set; }
        public int AracOzellikId { get; set; }
        public int AracId { get; set; }
        public string? Deger { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? CreatedUserId { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? UpdatedUserId { get; set; }

        public virtual Arac Arac { get; set; } = null!;
        public virtual AracOzellik AracOzellik { get; set; } = null!;
    }
}
