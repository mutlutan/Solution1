﻿using System;
using System.Collections.Generic;

namespace AppCommon.DataLayer.DataLog.Models
{
    public partial class MobilBildirimLog
    {
        public Guid Id { get; set; }
        public int MobilBildirimId { get; set; }
        public bool Durum { get; set; }
        public DateTime Tarih { get; set; }
        public string? MesajData { get; set; }
        public string? ResponseData { get; set; }
    }
}
