using System;
using System.Collections.Generic;

namespace AppCommon.DataLayer.DataMain.Models
{
    public partial class VwAccessLog
    {
        public Guid Id { get; set; }
        public int AccountId { get; set; }
        public string? AccountType { get; set; }
        public string? AccountName { get; set; }
        public string? IpAddress { get; set; }
        public string? Browser { get; set; }
        public string? SessionGuid { get; set; }
        public DateTime? LoginDate { get; set; }
        public DateTime? LogoutDate { get; set; }
        public string? ExtraSpace { get; set; }
    }
}
