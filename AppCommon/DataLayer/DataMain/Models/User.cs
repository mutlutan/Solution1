using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace AppCommon.DataLayer.DataMain.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public int UserStatusId { get; set; }
        public int UserTypeId { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public string? NameSurname { get; set; }
        public string? ResidenceAddress { get; set; }
        public string? Avatar { get; set; }
        public Geometry? GeoLocation { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? RoleIds { get; set; }
        public string? Authority { get; set; }
        public string? GaSecretKey { get; set; }
        public string? SessionGuid { get; set; }
        public DateTime? ValidityDate { get; set; }
        public Guid UniqueId { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? CreatedUserId { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? UpdatedUserId { get; set; }

        public virtual UserStatus UserStatus { get; set; } = null!;
        public virtual UserType UserType { get; set; } = null!;
    }
}
