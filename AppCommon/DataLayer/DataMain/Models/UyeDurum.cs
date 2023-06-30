using System;
using System.Collections.Generic;

namespace AppCommon.DataLayer.DataMain.Models
{
    public partial class UyeDurum
    {
        public UyeDurum()
        {
            Uye = new HashSet<Uye>();
        }

        public int Id { get; set; }
        public string? Ad { get; set; }

        public virtual ICollection<Uye> Uye { get; set; }
    }
}
