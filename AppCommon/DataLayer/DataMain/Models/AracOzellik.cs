using System;
using System.Collections.Generic;

namespace AppCommon.DataLayer.DataMain.Models
{
    public partial class AracOzellik
    {
        public AracOzellik()
        {
            AracOzellikDetay = new HashSet<AracOzellikDetay>();
        }

        public int Id { get; set; }
        public Guid UniqueId { get; set; }
        public bool Durum { get; set; }
        public string? Ad { get; set; }
        public string? Aciklama { get; set; }

        public virtual ICollection<AracOzellikDetay> AracOzellikDetay { get; set; }
    }
}
