using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.FN.DTO.ValueObject
{
    public class FnReceiptTypeResult
    {
        
        public short ReceiptTypeID { get; set; }
        public short AccID { get; set; }
        public string ReceiptTypeCode { get; set; }
        public string ReceiptTypeName { get; set; }
        public string RecFlag { get; set; }
        public string AccNo { get; set; }
        public string AccName { get; set; }
        public string Comment { get; set; }
        public Boolean Active { get; set; }
        public string LanguageName { get; set; }
        public short LanguageID { get; set; }
        public FnReceiptTypeResult()
        {
        }
    }
}
