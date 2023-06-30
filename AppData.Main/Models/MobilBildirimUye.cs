using System;
using System.Collections.Generic;

namespace AppData.Main.Models
{
    public partial class MobilBildirimUye
    {
        public int Id { get; set; }
        public int MobilBildirimId { get; set; }
        public int UyeId { get; set; }

        public virtual MobilBildirim MobilBildirim { get; set; } = null!;
        public virtual Uye Uye { get; set; } = null!;
    }
}
