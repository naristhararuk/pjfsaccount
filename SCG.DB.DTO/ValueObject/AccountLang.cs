using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.DB.DTO.ValueObject
{
    public class AccountLang
    {
        public long AccountID { get; set; }
        public string AccountCode { get; set; }

        public long AccountLangID { get; set; }
        public long ExpenseGroupID { get; set; }
        public string Description { get; set; }

        public string LanguageName { get; set; }
        public short? LanguageId { get; set; }
        public string AccountName { get; set; }
        public string Comment { get; set; }
        public bool Active { get; set; }
    }
}
