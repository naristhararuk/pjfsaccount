using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.SU.DTO.ValueObject
{
    public class ProgramRole
    {
        public short? ID { get; set; }
        public short? RoleId { get; set; }
        public short? ProgramId { get; set; }
        public short? LanguageId { get; set; }
        public string ProgramsName { get; set; }
        public Boolean AddState { get; set; }
        public Boolean EditState { get; set; }
        public Boolean DeleteState { get; set; }
        public Boolean DisplayState { get; set; }
        public string Comment { get; set; }
        public Boolean Active { get; set; }
        //public string ProgramRoleSymbol { get; set; }
        public ProgramRole()
        { }
    }
}
