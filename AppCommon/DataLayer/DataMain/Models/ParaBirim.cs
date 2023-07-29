using System;
using System.Collections.Generic;

namespace AppCommon.DataLayer.DataMain.Models
{
    public partial class ParaBirim
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public int LineNumber { get; set; }
        public string Icon { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string SubName { get; set; } = null!;
    }
}
