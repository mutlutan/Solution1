using System;
using System.Collections.Generic;

namespace AppCommon.DataLayer.DataLog.Models
{
    public partial class AracStatuLog
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
