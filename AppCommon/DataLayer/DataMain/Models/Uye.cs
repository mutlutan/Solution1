using System;
using System.Collections.Generic;

namespace AppCommon.DataLayer.DataMain.Models
{
    public partial class Uye
    {
        public int Id { get; set; }
        public int UyeDurumId { get; set; }
        public int UyeGrupId { get; set; }
        public bool IsConfirmed { get; set; }
        public string? NameSurname { get; set; }
        public string? CountryCode { get; set; }
        public string? Avatar { get; set; }
        public string? UserName { get; set; }
        public string? UserPassword { get; set; }
        public string? SessionGuid { get; set; }
        public DateTime? ValidityDate { get; set; }
        public Guid UniqueId { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? CreatedUserId { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? UpdatedUserId { get; set; }

        public virtual UyeDurum UyeDurum { get; set; } = null!;
        public virtual UyeGrup UyeGrup { get; set; } = null!;
    }
}
