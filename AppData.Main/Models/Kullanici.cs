using System;
using System.Collections.Generic;

namespace AppData.Main.Models
{
    public partial class Kullanici
    {
        public Kullanici()
        {
            AracHareket = new HashSet<AracHareket>();
            KullaniciKaraListe = new HashSet<KullaniciKaraListe>();
        }

        public int Id { get; set; }
        public Guid UniqueId { get; set; }
        public bool Durum { get; set; }
        public int KullaniciDurumId { get; set; }
        public int KullaniciGrupId { get; set; }
        public DateTime? UyelikTarihi { get; set; }
        public string? Email { get; set; }
        public string? Sifre { get; set; }
        public string? KimlikNumarasi { get; set; }
        public string? Ad { get; set; }
        public string? Soyad { get; set; }
        public string? Gsm { get; set; }
        public bool UyelikDogrulama { get; set; }
        public string? SifreSifirlamaKod { get; set; }
        public bool KvkkOnayi { get; set; }
        public bool UyelikSozlesmeOnayi { get; set; }
        public bool AydinlatmaMetniOnayi { get; set; }

        public virtual KullaniciDurum KullaniciDurum { get; set; } = null!;
        public virtual KullaniciGrup KullaniciGrup { get; set; } = null!;
        public virtual ICollection<AracHareket> AracHareket { get; set; }
        public virtual ICollection<KullaniciKaraListe> KullaniciKaraListe { get; set; }
    }
}
