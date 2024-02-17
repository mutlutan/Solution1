using System;
using System.Collections.Generic;

namespace AppCommon.DataLayer.DataMain.Models
{
    public partial class UserStatus
    {
        public UserStatus()
        {
            Customer = new HashSet<Customer>();
            User = new HashSet<User>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<Customer> Customer { get; set; }
        public virtual ICollection<User> User { get; set; }
    }
}
