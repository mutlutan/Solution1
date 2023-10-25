using System;
using System.Collections.Generic;

namespace AppCommon.DataLayer.DataMain.Models
{
    public partial class Job
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; }
        public string MethodName { get; set; } = null!;
        public string? MethodParams { get; set; }
        public int StartTypeId { get; set; }
        public TimeSpan StartTime { get; set; }
        public string? StartDayNames { get; set; }
        public int StartMonthDayNumber { get; set; }

        public virtual StartType StartType { get; set; } = null!;
    }
}
