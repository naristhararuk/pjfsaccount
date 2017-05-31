using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.SU.DTO
{
    [Serializable]
    public partial class UserMenu
    {
        public virtual short MenuID { get; set; }
        public virtual short MenuMainID { get; set; }
        public virtual string MenuName { get; set; }
        public virtual string ProgramPath { get; set; }
        public virtual short MenuLevel { get; set; }
        public virtual short MenuSeq { get; set; }
        public virtual short MenuLanguageID { get; set; }
        public virtual Nullable<short> RoleID { get; set; }

        public virtual short ProgramID { get; set; }
        public virtual string ProgramCode { get; set; }
        public virtual string ProgramsName { get; set; }
        public virtual short ProgramLanguageID { get; set; }

    }
}
