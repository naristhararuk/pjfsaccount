using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.DTO.ValueObject
{
        [Serializable]
        public class VOFixedAdvanceDuplicateReport
        {
            public long DocumentID { get; set; }
            public string DocumentNo { get; set; }
            public DateTime? CreDate { get; set; }
            public String CompanyID { get; set; }
            public long? AliasID { get; set; }
            public DateTime? FromDate { get; set; }
            public DateTime? ToDate { get; set; }
            public int FromOverDue { get; set; }
            public int ToOverDue { get; set; }
            public string RequesterID { get; set; }
            public string CurrenUserID { get; set; }
            public string ParameterList { get; set; }
            public string BuCode { get; set; }
        }
}
