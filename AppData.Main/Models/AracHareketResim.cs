using System;
using System.Collections.Generic;

namespace AppData.Main.Models
{
    public partial class AracHareketResim
    {
        public int Id { get; set; }
        public int AracHareketId { get; set; }
        public string? ResimUrl { get; set; }

        public virtual AracHareket AracHareket { get; set; } = null!;
    }
}
