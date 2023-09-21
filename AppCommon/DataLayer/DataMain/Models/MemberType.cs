using System;
using System.Collections.Generic;

namespace AppCommon.DataLayer.DataMain.Models
{
    public partial class MemberType
    {
        public MemberType()
        {
            Member = new HashSet<Member>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<Member> Member { get; set; }
    }
}
