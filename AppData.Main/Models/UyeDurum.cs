using System;
using System.Collections.Generic;

namespace AppData.Main.Models
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
