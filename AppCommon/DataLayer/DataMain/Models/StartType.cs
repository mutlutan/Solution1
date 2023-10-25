using System;
using System.Collections.Generic;

namespace AppCommon.DataLayer.DataMain.Models
{
    public partial class StartType
    {
        public StartType()
        {
            Job = new HashSet<Job>();
        }

        public int Id { get; set; }
        public bool IsActive { get; set; }
        public int LineNumber { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Job> Job { get; set; }
    }
}
