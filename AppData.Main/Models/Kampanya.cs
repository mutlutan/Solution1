using System;
using System.Collections.Generic;

namespace AppData.Main.Models
{
    public partial class Kampanya
    {
        public Kampanya()
        {
            KampanyaUye = new HashSet<KampanyaUye>();
        }

        public int Id { get; set; }
        public bool Durum { get; set; }
        public int KampanyaIndirimTipiId { get; set; }
        public int KampanyaTurId { get; set; }
        public string? UyeGrupIds { get; set; }
        public string? Ad { get; set; }
        public string? AdEng { get; set; }
        public string? GorselUrl { get; set; }
        public string? SayfaIcerik { get; set; }
        public string? SayfaIcerikEng { get; set; }
        public DateTime? BaslangicTarihi { get; set; }
        public DateTime? BitisTarihi { get; set; }
        public decimal IndirimDegeri { get; set; }
        public bool CokluKullanim { get; set; }

        public virtual KampanyaIndirimTipi KampanyaIndirimTipi { get; set; } = null!;
        public virtual KampanyaTur KampanyaTur { get; set; } = null!;
        public virtual ICollection<KampanyaUye> KampanyaUye { get; set; }
    }
}
