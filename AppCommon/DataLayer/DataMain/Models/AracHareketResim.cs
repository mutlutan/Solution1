using System;
using System.Collections.Generic;

namespace AppCommon.DataLayer.DataMain.Models
{
    public partial class AracHareketResim
    {
        public int Id { get; set; }
        public int AracHareketId { get; set; }
        public string? ResimUrl { get; set; }

        public virtual AracHareket AracHareket { get; set; } = null!;
    }
}
