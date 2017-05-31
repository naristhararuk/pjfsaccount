using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.DB.DTO.ValueObject
{
    [Serializable]
    public class VOPB
    {
       
        public long? Pbid { get; set; }
        public string PBCode { get; set; }
        public string Description{ get; set; }
        public double? PettyCashLimit { get; set; }
        public bool BlockPost { get; set; }
        public long? CompanyID { get; set; }
        public string CompanyCode { get; set; }
        public short? LanguageID{ get; set; }
        public string LanguageName{ get; set; }
        public long? PBLangID{ get; set; }
        public long? PBID{ get; set; }
        public bool Active { get; set; }
        public short? PBLanguageID{ get; set; }
        public string Comment { get; set; }
        public long? UserID { get; set; }
    }
}
