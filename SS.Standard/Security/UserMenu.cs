using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

namespace SS.Standard.Security
{
    [Serializable]
    public class UserMenu
    {
        public string MenuGroupName { get; set; }
        public string MenuName { get; set; }
        public string ProgramsName { get; set; }
        public string ProgramPath { get; set; }
        public string ProgramCode { get; set; }
    }
}
