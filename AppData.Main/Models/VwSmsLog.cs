using System;
using System.Collections.Generic;

namespace AppData.Main.Models
{
    public partial class VwSmsLog
    {
        public Guid Id { get; set; }
        public int SmsBildirimId { get; set; }
        public bool Durum { get; set; }
        public DateTime Tarih { get; set; }
        public string? MesajData { get; set; }
        public string? ResponseData { get; set; }
    }
}
