using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.DTO.ValueObject
{
    public class FnExpenseDataForEmail
    {
        public long DocumentID { get; set; }
        public long ExpenseID { get; set; }
        public string RequestNo { get; set; }
        public string Subject { get; set; }
        public double TotalExpense { get; set; }
        public string SymbolLocal { get; set; }
        public string SymbolMain { get; set; }
        public double TotalExpenseLocal { get; set; }
        public double TotalExpenseMain { get; set; }
        public bool IsRepOffice { get; set; }
        public long PBID { get; set; }
        public double DifferenceAmount { get; set; }
        public double DifferenceAmountLocalCurrency { get; set; }
        public double DifferenceAmountMainCurrency { get; set; }
    }

    public class InitiatorData
    {
        public short InitiatorSeq { get; set; }
        public string InitiatorName { get; set; }
        public string InitiatorEmail { get; set; }
        public string InitialType { get; set; }
        public bool IsSMS { get; set; }
    }

    public class InvoiceDataForEmail
    {
        public long InvoiceID { get; set; }
        public int ItemNo { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        //public string DisplayInvoiceDate
        //{
        //    get { return InvoiceDate.HasValue ? InvoiceDate.Value.ToString("dd/MM/yyyy") : string.Empty; }
        //}
        public string Vendor { get; set; }
        public string BaseAmount { get; set; }
        public string VatAmount { get; set; }
        public string WHTAmount { get; set; }
        public string NetAmount { get; set; }

        public IList<InvoiceItemDataForEmail> InvoiceItems { get; set; }
    }

    public class InvoiceItemDataForEmail
    {
        public string CostCenter { get; set; }
        public string ExpenseTH { get; set; }
        public string ExpenseEN { get; set; }
        public string InternalOrder { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; }
        public string AmountCurrency { get; set; }
        public string ExchangeRate { get; set; }
        public string AmountTHB { get; set; }
        public string AmountFinalCurrency { get; set; }
        public string ReferenceNo { get; set; }
    }
}
