using System;
using System.Collections.Generic;

namespace AppData.Main.Models
{
    public partial class AuditLog
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public string? UserType { get; set; }
        public string? UserName { get; set; }
        public string? UserIp { get; set; }
        public string? UserBrowser { get; set; }
        public string? UserSessionGuid { get; set; }
        public string? OperationType { get; set; }
        public DateTime? OperationDate { get; set; }
        public string? TableName { get; set; }
        public string? PrimaryKeyField { get; set; }
        public string? PrimaryKeyValue { get; set; }
        public string? CurrentValues { get; set; }
        public string? OriginalValues { get; set; }
    }
}
