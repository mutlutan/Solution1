using System;
using System.Collections.Generic;

namespace AppCommon.DataLayer.DataLog.Models
{
    public partial class SystemLog
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public string? UserType { get; set; }
        public string? UserName { get; set; }
        public string? UserIp { get; set; }
        public string? UserBrowser { get; set; }
        public string? UserSessionGuid { get; set; }
        public int ProcessTypeId { get; set; }
        public DateTime? ProcessDate { get; set; }
        public string? ProcessName { get; set; }
        public string? ProcessContent { get; set; }
        public TimeSpan? ProcessingTime { get; set; }
    }
}
