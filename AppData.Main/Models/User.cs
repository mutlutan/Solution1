using System;
using System.Collections.Generic;

namespace AppData.Main.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public string? IdentificationNumber { get; set; }
        public string? NameSurname { get; set; }
        public string? ResidenceAddress { get; set; }
        public string? Avatar { get; set; }
        public string? UserName { get; set; }
        public string? UserPassword { get; set; }
        public string? RoleIds { get; set; }
        public string? GaSecretKey { get; set; }
        public string? SessionGuid { get; set; }
        public DateTime? ValidityDate { get; set; }
        public Guid UniqueId { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? CreatedUserId { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? UpdatedUserId { get; set; }
    }
}
