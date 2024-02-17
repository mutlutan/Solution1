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
    }
}
