using System;
using System.Collections.Generic;

namespace AppCommon.DataLayer.DataMain.Models
{
    public partial class VwUserLog
    {
        public Guid Id { get; set; }
        public string? TableName { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserIp { get; set; }
        public string? UserBrowser { get; set; }
        public string? SessionGuid { get; set; }
        public DateTime? LoginDate { get; set; }
        public DateTime? LogoutDate { get; set; }
        public string? ExtraSpace1 { get; set; }
        public string? ExtraSpace2 { get; set; }
        public string? ExtraSpace3 { get; set; }
    }
}
