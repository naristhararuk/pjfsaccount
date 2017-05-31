using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.DTO.ValueObject
{
    [Serializable]
    public class VOFixedAdvanceOverDueReport
    {
        public long DocumentID { get; set; }
        public long FixedAdvanceID { get; set; }
        public long RefFixedAdvanceID { get; set; }
        
        public string FixedAdvanceTypeName { get; set; }        
        public DateTime? RequestDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        /*grid column*/
        public string DocumentNo { get; set; }
        public DateTime? CreDate { get; set; }
        public DateTime? EffectiveFromDate { get; set; }
        public DateTime? EffectiveToDate { get; set; }
        public string RequesterName { get; set; }
        public string Subject { get; set; }
        public string Objective { get; set; }
        public Double? Amount { get; set; }
        public Double? NetAmount { get; set; }
        public string RefFixedAdvanceNo { get; set; }
        public string FixedAdvanceStatus { get; set; }
        public DateTime? DueDate { get; set; }
        public int OverDueDay { get; set; }
        public int Sendtime { get; set; }
        //-----------------------------------------
        
        //For Criteria
        public long CompanyID { get; set; }
        public long FromLocationID { get; set; }
        public long ToLocationID { get; set; }
        public string FromLocationCode { get; set; }
        public string ToLocationCode { get; set; }
        public string FromDueDate { get; set; }
        public string ToDueDate { get; set; }
        public double FromFixedAdvanceAmount { get; set; }
        public double ToFixedAdvanceAmount { get; set; }
        public int FromOverDue { get; set; }
        public int ToOverDue { get; set; }
        public long RequesterID { get; set; }
        public string FixedAdvanceType { get; set; }
        public short LanguageID { get; set; }
        //-----------------------------------------


        public VOFixedAdvanceOverDueReport()
        { }
    }
}
