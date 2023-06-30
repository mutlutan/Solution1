using System;
using System.Collections.Generic;

namespace AppCommon.DataLayer.DataMain.Models
{
    public partial class Uye
    {
        public Uye()
        {
            AracHareket = new HashSet<AracHareket>();
            AracRezervasyon = new HashSet<AracRezervasyon>();
            KampanyaUye = new HashSet<KampanyaUye>();
            MobilBildirimUye = new HashSet<MobilBildirimUye>();
            SmsBildirimUye = new HashSet<SmsBildirimUye>();
            UyeCariHareket = new HashSet<UyeCariHareket>();
            UyeCuzdanHareket = new HashSet<UyeCuzdanHareket>();
            UyeFaturaBilgisi = new HashSet<UyeFaturaBilgisi>();
            UyeKaraListe = new HashSet<UyeKaraListe>();
        }

        public int Id { get; set; }
        public Guid UniqueId { get; set; }
        public int UyeDurumId { get; set; }
        public int UyeGrupId { get; set; }
        public DateTime? UyelikTarihi { get; set; }
        public string? Email { get; set; }
        public string? Sifre { get; set; }
        public string? KimlikNumarasi { get; set; }
        public string? Ad { get; set; }
        public string? Soyad { get; set; }
        public string? Gsm { get; set; }
        public DateTime DogumTarihi { get; set; }
        public int CinsiyetId { get; set; }
        public string? Avatar { get; set; }
        public bool UyelikDogrulama { get; set; }
        public string? SifreSifirlamaKod { get; set; }
        public bool KvkkOnayi { get; set; }
        public bool UyelikSozlesmeOnayi { get; set; }
        public bool AydinlatmaMetniOnayi { get; set; }
        public decimal? CuzdanBakiye { get; set; }
        public bool MsisdnDogrulama { get; set; }
        public string? FcmRegistrationToken { get; set; }
        public string? MobileAppState { get; set; }

        public virtual Cinsiyet Cinsiyet { get; set; } = null!;
        public virtual UyeDurum UyeDurum { get; set; } = null!;
        public virtual UyeGrup UyeGrup { get; set; } = null!;
        public virtual ICollection<AracHareket> AracHareket { get; set; }
        public virtual ICollection<AracRezervasyon> AracRezervasyon { get; set; }
        public virtual ICollection<KampanyaUye> KampanyaUye { get; set; }
        public virtual ICollection<MobilBildirimUye> MobilBildirimUye { get; set; }
        public virtual ICollection<SmsBildirimUye> SmsBildirimUye { get; set; }
        public virtual ICollection<UyeCariHareket> UyeCariHareket { get; set; }
        public virtual ICollection<UyeCuzdanHareket> UyeCuzdanHareket { get; set; }
        public virtual ICollection<UyeFaturaBilgisi> UyeFaturaBilgisi { get; set; }
        public virtual ICollection<UyeKaraListe> UyeKaraListe { get; set; }
    }
}
