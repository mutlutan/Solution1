using System;
using System.Collections.Generic;

namespace AppData.Main.Models
{
    public partial class KullaniciGrup
    {
        public KullaniciGrup()
        {
            Kullanici = new HashSet<Kullanici>();
        }

        public int Id { get; set; }
        public Guid UniqueId { get; set; }
        public bool Durum { get; set; }
        public string? Ad { get; set; }

        public virtual ICollection<Kullanici> Kullanici { get; set; }
    }
}
