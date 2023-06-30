using System;
using System.Collections.Generic;

namespace AppCommon.DataLayer.DataMain.Models
{
    public partial class KampanyaUye
    {
        public int Id { get; set; }
        public int KampanyaId { get; set; }
        public int UyeId { get; set; }

        public virtual Kampanya Kampanya { get; set; } = null!;
        public virtual Uye Uye { get; set; } = null!;
    }
}
