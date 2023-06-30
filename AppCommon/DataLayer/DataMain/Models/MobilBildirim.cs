using System;
using System.Collections.Generic;

namespace AppCommon.DataLayer.DataMain.Models
{
    public partial class MobilBildirim
    {
        public MobilBildirim()
        {
            MobilBildirimUye = new HashSet<MobilBildirimUye>();
        }

        public int Id { get; set; }
        public Guid UniqueId { get; set; }
        public bool GonderildiMi { get; set; }
        public DateTime Tarih { get; set; }
        public DateTime? GonderimTarihi { get; set; }
        public string? UyeGrupIds { get; set; }
        public string? Baslik { get; set; }
        public string? Mesaj { get; set; }
        public string? ResimUrl { get; set; }
        public string? Link { get; set; }

        public virtual ICollection<MobilBildirimUye> MobilBildirimUye { get; set; }
    }
}
