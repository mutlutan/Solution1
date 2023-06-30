using System;
using System.Collections.Generic;

namespace AppData.Main.Models
{
    public partial class EmailLetterhead
    {
        public EmailLetterhead()
        {
            EmailTemplate = new HashSet<EmailTemplate>();
        }

        public int Id { get; set; }
        public string? Description { get; set; }
        public string? Content { get; set; }
        public Guid UniqueId { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? CreatedUserId { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? UpdatedUserId { get; set; }

        public virtual ICollection<EmailTemplate> EmailTemplate { get; set; }
    }
}
