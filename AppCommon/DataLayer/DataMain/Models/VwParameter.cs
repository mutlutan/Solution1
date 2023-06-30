using System;
using System.Collections.Generic;

namespace AppCommon.DataLayer.DataMain.Models
{
    public partial class VwParameter
    {
        public int Id { get; set; }
        public string SiteAddress { get; set; } = null!;
        public string? InstitutionEmail { get; set; }
        public bool AuditLog { get; set; }
        public string? AuditLogTables { get; set; }
        public string? EmailHost { get; set; }
        public int EmailPort { get; set; }
        public bool EmailEnableSsl { get; set; }
        public string? EmailUserName { get; set; }
        public string? EmailPassword { get; set; }
        public string? SmsServiceUrl { get; set; }
        public string? SmsServiceUserName { get; set; }
        public string? SmsServicePassword { get; set; }
    }
}
