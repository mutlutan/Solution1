using System;
using System.Collections.Generic;

namespace AppData.Main.Models
{
    public partial class EmailPoolStatus
    {
        public EmailPoolStatus()
        {
            EmailPool = new HashSet<EmailPool>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<EmailPool> EmailPool { get; set; }
    }
}
