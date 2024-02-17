using System;
using System.Collections.Generic;

namespace AppCommon.DataLayer.DataMain.Models
{
    public partial class ProductCategory
    {
        public ProductCategory()
        {
            Product = new HashSet<Product>();
        }

        public int Id { get; set; }
        public int? ParentId { get; set; }
        public bool IsActive { get; set; }
        public string? Name { get; set; }
        public Guid UniqueId { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? CreatedUserId { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? UpdatedUserId { get; set; }

        public virtual ICollection<Product> Product { get; set; }
    }
}
