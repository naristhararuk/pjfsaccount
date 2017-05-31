using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.SU.DTO.ValueObject
{
    public class MenuPath
    {
        public string MenuName { get; set; }
        public short? MenuID { get; set; }
        public short? MenuMainID { get; set; }
        public short? ProgramID { get; set; }
        public short? MenuLevel { get; set; }

        public MenuPath()
        { }
    }
}
