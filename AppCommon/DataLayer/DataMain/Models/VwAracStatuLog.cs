using System;
using System.Collections.Generic;

namespace AppCommon.DataLayer.DataMain.Models
{
    public partial class VwAracStatuLog
    {
        public Guid Id { get; set; }
        public string? ImeiNo { get; set; }
        public int ReportId { get; set; }
        public bool Durum { get; set; }
        public DateTime Tarih { get; set; }
        public string? RequestData { get; set; }
        public string? ResponseData { get; set; }
    }
}
