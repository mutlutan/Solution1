﻿using System;
using System.Collections.Generic;

namespace AppData.Main.Models
{
    public partial class Version
    {
        public int Id { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? Description { get; set; }
        public string? CommandText { get; set; }
    }
}
