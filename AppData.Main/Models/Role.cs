using System;
using System.Collections.Generic;

namespace AppData.Main.Models
{
    public partial class Role
    {
        public int Id { get; set; }
        public Guid UniqueId { get; set; }
        public bool IsActive { get; set; }
        public int LineNumber { get; set; }
        public string Name { get; set; } = null!;
        public string? Authority { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? CreatedUserId { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? UpdatedUserId { get; set; }
    }
}
