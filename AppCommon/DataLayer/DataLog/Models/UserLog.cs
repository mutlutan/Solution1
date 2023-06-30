using System;
using System.Collections.Generic;

namespace AppCommon.DataLayer.DataLog.Models
{
    public partial class UserLog
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public string? UserType { get; set; }
        public string? UserName { get; set; }
        public string? UserIp { get; set; }
        public string? UserBrowser { get; set; }
        public string? UserSessionGuid { get; set; }
        public DateTime? LoginDate { get; set; }
        public DateTime? LogoutDate { get; set; }
        public string? EkAlan1 { get; set; }
        public string? EkAlan2 { get; set; }
        public string? EkAlan3 { get; set; }
    }
}
