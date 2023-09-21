using System;
using System.Collections.Generic;

namespace AppCommon.DataLayer.DataMain.Models
{
    public partial class Dashboard
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public int LineNumber { get; set; }
        public string TemplateName { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? IconClass { get; set; }
        public string? IconStyle { get; set; }
        public string? DetailUrl { get; set; }
        public string? Query { get; set; }
        public Guid UniqueId { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? CreatedUserId { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? UpdatedUserId { get; set; }
    }
}
