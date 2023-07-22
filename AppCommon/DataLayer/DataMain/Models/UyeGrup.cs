using System;
using System.Collections.Generic;

namespace AppCommon.DataLayer.DataMain.Models
{
    public partial class UyeGrup
    {
        public UyeGrup()
        {
            Uye = new HashSet<Uye>();
        }

        public int Id { get; set; }
        public Guid UniqueId { get; set; }
        public bool Durum { get; set; }
        public string? Ad { get; set; }

        public virtual ICollection<Uye> Uye { get; set; }
    }
}
