using System;
using System.Collections.Generic;

namespace AppCommon.DataLayer.DataMain.Models
{
    public partial class UserStatus
    {
        public UserStatus()
        {
            Member = new HashSet<Member>();
            User = new HashSet<User>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<Member> Member { get; set; }
        public virtual ICollection<User> User { get; set; }
    }
}
