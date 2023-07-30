using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp1.Models
{
    public class MyCodeGenInfo
    {
        public string TableName { get; set; } = "";
        public List<string> TableOptions { get; set; } = new List<string>();

        public List<string> DataTransferObjects { get; set; } = new List<string>();
        public List<string> DataManipulationObjects { get; set; } = new List<string>();
        public List<string> Controllers { get; set; } = new List<string>();

        public List<string> Dictionaries { get; set; } = new List<string>(); 

        public List<string> FormViews { get; set; } = new List<string>();
        public List<string> GridViews { get; set; } = new List<string>();

        public List<string> TreeLists { get; set; } = new List<string>();

        public List<string> SearchViews { get; set; } = new List<string>();

    }
}
