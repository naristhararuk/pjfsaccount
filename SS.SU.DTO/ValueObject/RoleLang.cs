using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.SU.DTO.ValueObject
{
    public class RoleLang
    {
        public short? RoleLangId { get; set; }
        public short? RoleId { get; set; }
        public short? LanguageId { get; set; }
        public string LanguageName { get; set; }
        public DateTime UpdDate { get; set; }
        public string RoleName { get; set; }
        public string RoleCode { get; set; }
        public string Comment { get; set; }
        public bool Active { get; set; }
        public int? RoleCount { get; set; }

        public RoleLang()
        {
        }
    }
}
