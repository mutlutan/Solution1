using System;
using System.Collections.Generic;

namespace AppData.Main.Models
{
    public partial class EmailPool
    {
        public int Id { get; set; }
        public int EmailTemplateId { get; set; }
        public int EmailPoolStatusId { get; set; }
        public int TryQuantity { get; set; }
        public DateTime? LastTryDate { get; set; }
        public string? Description { get; set; }
        public string? EmailTo { get; set; }
        public string? EmailCc { get; set; }
        public string? EmailBcc { get; set; }
        public string? EmailSubject { get; set; }
        public string? EmailContent { get; set; }
        public Guid UniqueId { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? CreatedUserId { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? UpdatedUserId { get; set; }

        public virtual EmailPoolStatus EmailPoolStatus { get; set; } = null!;
        public virtual EmailTemplate EmailTemplate { get; set; } = null!;
    }
}
