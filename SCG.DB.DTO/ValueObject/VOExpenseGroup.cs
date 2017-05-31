using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.DB.DTO.ValueObject
{
    public class VOExpenseGroup
    {
        public short LanguageID { get; set; }
        public string LanguageName { get; set; }
        public long ExpenseGroupID { get; set; }
        public long ExpenseGroupLangID { get; set; }
        public string ExpenseGroupName { get; set; }
        public string ExpenseGroupCode { get; set; }
        public short ExpenseLang { get; set; }
        public string Description { get; set; }
        public string Comment { get; set; }
        public bool Active { get; set; }
    }
}
