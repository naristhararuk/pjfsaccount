using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.DB.DTO.ValueObject
{
    public class MoneyRequestSearchResult : IComparable<MoneyRequestSearchResult>
    {
        public long CompanyID { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string BankName { get; set; }
        public string BankBranch { get; set; }
        public string AccountNo { get; set; }
        public string AccountType { get; set; }

        public long DocumentID { get; set; }
        public string DocumentNo { get; set; }
        public long RequesterID { get; set; }

        public string LetterNo { get; set; }

        public DateTime? RequestDateOfAdvance { get; set; }

        public string EmployeeName { get; set; }

        public double Amount { get; set; }

        public string Currency { get; set; }

        public bool IsIncludeGeneratedLetter { get; set; }
        public int MoneyRequestCount { get; set; }
        public int Year { get; set; }

        public static Comparison<MoneyRequestSearchResult>
            PriceComparison = delegate(MoneyRequestSearchResult m1, MoneyRequestSearchResult m2)
        {
            return m1.CompanyCode.CompareTo(m2.CompanyCode);
        };

        public int CompareTo(MoneyRequestSearchResult other) 
        {
            return CompanyCode.CompareTo(other.CompanyCode); 
        }

    }
}
