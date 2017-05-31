using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.SAP.DTO.ValueObject
{
    public class SapLog
    {
        public long?        Id          { get; set; }
        public long?        DocId       { get; set; }
        public string       PostingType { get; set; }
        public string       DocNo       { get; set; }
        public string       DocSeq      { get; set; }
        public string       DocYear     { get; set; }
        public DateTime?    PostingDate { get; set; }
        public string       FiDoc       { get; set; }
        public string       Employee    { get; set; }
        public string       EmployeeCode{ get; set; }
        public string       EmployeeName{ get; set; }
        public string       Company     { get; set; }
        public string       CompanyCode { get; set; }
        public string       CompanyName { get; set; }
        public string       Type        { get; set; }
        public string       Message     { get; set; }
        public int?         SapLogCount { get; set; }
    }
}
