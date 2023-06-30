using System;
using System.Collections.Generic;

namespace AppCommon.DataLayer.DataMain.Models
{
    public partial class SmsBildirimUye
    {
        public int Id { get; set; }
        public int SmsBildirimId { get; set; }
        public int UyeId { get; set; }

        public virtual SmsBildirim SmsBildirim { get; set; } = null!;
        public virtual Uye Uye { get; set; } = null!;
    }
}
