using System;
using System.Collections.Generic;

namespace AppCommon.DataLayer.DataMain.Models
{
    public partial class CustomerTransaction
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int CustomerWalletId { get; set; }
        public decimal? Debit { get; set; }
        public decimal? Credit { get; set; }
        public Guid UniqueId { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? CreatedUserId { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? UpdatedUserId { get; set; }

        public virtual Customer Customer { get; set; } = null!;
        public virtual CustomerWallet CustomerWallet { get; set; } = null!;
    }
}
