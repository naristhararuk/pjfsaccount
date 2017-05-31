using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.DTO.ValueObject
{
    [Serializable]
    public class Advance
    {
        public long WorkflowID { get; set; }
        public long DocumentID { get; set; }
        public long AdvanceID { get; set; }
        public long? CompanyID { get; set; }
        public string DocumentNo { get; set; }
	    public string AdvanceNo { get; set; }
        public string Description { get; set; }
        public string Requester { get; set; }
        public string Receiver { get; set; }
        public long? RequesterID { get; set; }
        public long? ReceiverID { get; set; }
        public DateTime? DueDateOfRemittance { get; set; }
        public DateTime? RequestDateOfRemittance { get; set; }
        public double? Amount { get; set; }
        public int? DocumentCount { get; set; }

        public string RequesterName { get; set; }
        public string ReceiverName { get; set; }
        public long RemittanceID { get; set; }
        public string AdvanceType { get; set; }

        public long? PBID { get; set; }
        public short? MainCurrencyID { get; set; }


        public double? MainCurrencyAmount { get; set; }
        public double? LocalCurrencyAmount { get; set; }
        public double? ExchangeRateMainToTHBCurrency { get; set; }
        public double? ExchangeRateForLocalCurrency { get; set; }
        public bool IsRepOffice { get; set; }

        //Sum properties
        public double? SumAmount { get; set; }
        public double? SumAmountTHB { get; set; }
        public double? SumExchangeRate { get; set; }
        public short? CurrencyID { get; set; }
        public string PaymentType { get; set; }
        //Sum properties

        public string ExpenseType { get; set; }

        // For Clearing Advance
        //-----------------------------------------------------------------
        
        // AdvanceNo at top.
        // Subject use Description at top.
        // DuedateOfRemittance at top.
        public string RemittanceNo { get; set; }
        // Payment Type at top.
        public string Currency { get; set; } // Is property symbol in DbCurrency Table.
        public double? ForeignCurrencyAdvanced { get; set; }//In remittanceItem.
        public double? ExchangeRate { get; set; } //In remittanceItem.
        public double? ForeignCurrencyRemitted { get; set; } //In remittanceItem.
        public double? RemittedAmountTHB { get; set; } //In remittance.
        public double? ForeignAmountTHBAdvanced { get; set; } //In remittanceItem (comment by Meaw).
        public double? RemittanceAmountMainCurrency { get; set; }

        public long? TADocumentID { get; set; }
        //-----------------------------------------------------------------
        // For Clearing Advance
        public long? CurrentUserID { get; set; }


        public Advance()
        {
        }
        public Advance(long advanceID)
        {
            this.AdvanceID = advanceID;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj == this) return true;
            if (obj.GetType() != this.GetType()) return false;

            Advance castedObj = (Advance)obj;

            if (this.AdvanceID == castedObj.AdvanceID) return true;
            else return false;
        }

        public override int GetHashCode()
        {
            return Convert.ToInt32(this.AdvanceID);
        }


    }
}
