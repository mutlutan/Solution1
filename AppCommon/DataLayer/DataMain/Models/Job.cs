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
        public string? MethodComment { get; set; }
        public string? CronExpression { get; set; }
        public bool IsPeriodic { get; set; }
        public TimeSpan StartTime { get; set; }
        public string? DaysOfTheWeek { get; set; }
        public string? Months { get; set; }
        public string? DaysOfTheMonth { get; set; }
    }
}
