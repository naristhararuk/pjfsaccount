using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.DTO.ValueObject
{
    [Serializable] 
    public class VOAdvanceOverDueReport
    {
        public long DocumentID { get; set; }
        public string DocumentNo { get; set; }
        public DateTime? DocumentDate { get; set; }
        public string RequesterName { get; set; }
        public string Description { get; set; }
        public Double? AdvanceAmt { get; set; }
        public Double? ExpenseAmt { get; set; }
        public Double? RemittanceAmt { get; set; }
        public Double? OutstandingAmt { get; set; }
        public DateTime? RequestDateofRemittance { get; set; }
        public string ExpenseNo { get; set; }
        public string ExpenseStatus { get; set; }
        public DateTime? DueDate { get; set; }
        public int OverDueDay { get; set; }
        public int Sendtime { get; set; }
        public long RequesterID { get; set; }
        //For Criteria
        public long CompanyID { get; set; }
        public long FromLocationID { get; set; }
        public long ToLocationID { get; set; }
        public string FromLocationCode { get; set; }
        public string ToLocationCode { get; set; }
        public string FromDueDate { get; set; }
        public string ToDueDate { get; set; }
        public double FromAdvanceAmount { get; set; }
        public double ToAdvanceAmount { get; set; }
        public int FromOverDue { get; set; }
        public int ToOverDue { get; set; }
        public int DifOverDue 
        {
            get 
            {
                if (RequestDateofRemittance != null)
                    return ( DateTime.Today - RequestDateofRemittance.Value).Days;
                else
                    return 0;
            } 
        }
        public long AdvanceID { get; set; }
        public long ExpenseID { get; set; }
        public string AdvanceType { get; set; }
        public short LanguageID { get; set; }
        public long OverdueDays { get; set; }
        public string ExpenseBoxID { get; set; }

        public VOAdvanceOverDueReport()
        {}
       
    }
}
