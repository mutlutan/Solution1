using System;
using System.Collections.Generic;

namespace AppData.Main.Models
{
    public partial class EmailTemplate
    {
        public EmailTemplate()
        {
            EmailPool = new HashSet<EmailPool>();
        }

        public int Id { get; set; }
        public int EmailLetterheadId { get; set; }
        public string? Name { get; set; }
        public string? EmailCc { get; set; }
        public string? EmailBcc { get; set; }
        public string? EmailSubject { get; set; }
        public string? EmailContent { get; set; }
        public Guid UniqueId { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? CreatedUserId { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? UpdatedUserId { get; set; }

        public virtual EmailLetterhead EmailLetterhead { get; set; } = null!;
        public virtual ICollection<EmailPool> EmailPool { get; set; }
    }
}
