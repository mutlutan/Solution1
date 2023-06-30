using System;
using System.Collections.Generic;

namespace AppData.Main.Models
{
    public partial class KullaniciDurum
    {
        public KullaniciDurum()
        {
            Kullanici = new HashSet<Kullanici>();
        }

        public int Id { get; set; }
        public string? Ad { get; set; }

        public virtual ICollection<Kullanici> Kullanici { get; set; }
    }
}
