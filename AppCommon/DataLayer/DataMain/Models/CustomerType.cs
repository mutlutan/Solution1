using System;
using System.Collections.Generic;

namespace AppCommon.DataLayer.DataMain.Models
{
    public partial class CustomerType
    {
        public CustomerType()
        {
            Customer = new HashSet<Customer>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<Customer> Customer { get; set; }
    }
}
