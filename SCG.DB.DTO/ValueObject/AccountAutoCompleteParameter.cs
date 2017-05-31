using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.DB.DTO.ValueObject
{
    public class AccountAutoCompleteParameter
    {
        public short LanguageID { get; set; }
        public long? ExpenseGroupID { get; set; }
        public long? AccountID { get; set; }
        public string CostCenterCode { get; set; }
        public long? CompanyID { get; set; }
        public string WithoutExpenseCode { get; set; }
    }
}
