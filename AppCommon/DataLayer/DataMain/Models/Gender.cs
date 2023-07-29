using System;
using System.Collections.Generic;

namespace AppCommon.DataLayer.DataMain.Models
{
    public partial class Gender
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public int LineNumber { get; set; }
        public string Name { get; set; } = null!;
    }
}
