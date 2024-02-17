using System;
using System.Collections.Generic;

namespace AppCommon.DataLayer.DataMain.Models
{
    public partial class CustomerWallet
    {
        public CustomerWallet()
        {
            CustomerTransaction = new HashSet<CustomerTransaction>();
        }

        public int Id { get; set; }
        public bool IsActive { get; set; }
        public int CustomerId { get; set; }
        public int CurrencyId { get; set; }
        public string? WalletNumber { get; set; }
        public Guid UniqueId { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? CreatedUserId { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? UpdatedUserId { get; set; }

        public virtual Customer Customer { get; set; } = null!;
        public virtual ICollection<CustomerTransaction> CustomerTransaction { get; set; }
    }
}
