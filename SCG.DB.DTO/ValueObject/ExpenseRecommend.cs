using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.DB.DTO.ValueObject
{
    [Serializable]
    public class ExpenseRecommend
    {
        public long UserID { get; set; }
        public bool IsDomesticRecommend { get; set; }
        public bool IsForegnRecommend { get; set; }
        public long CostCenterID { get; set; }
        public string CostCenterCode { get; set; }
        public long AccountID { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public short LanguageID { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
        public double CurrencyAmount { get; set; }
        public double ExchangeRate { get; set; }
        public string ReferenceNo { get; set; }
    }
}
