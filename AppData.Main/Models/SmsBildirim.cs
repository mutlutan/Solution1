using System;
using System.Collections.Generic;

namespace AppData.Main.Models
{
    public partial class SmsBildirim
    {
        public SmsBildirim()
        {
            SmsBildirimUye = new HashSet<SmsBildirimUye>();
        }

        public int Id { get; set; }
        public Guid UniqueId { get; set; }
        public bool GonderildiMi { get; set; }
        public DateTime Tarih { get; set; }
        public DateTime? GonderimTarihi { get; set; }
        public string? UyeGrupIds { get; set; }
        public string? Baslik { get; set; }
        public string? Mesaj { get; set; }

        public virtual ICollection<SmsBildirimUye> SmsBildirimUye { get; set; }
    }
}
