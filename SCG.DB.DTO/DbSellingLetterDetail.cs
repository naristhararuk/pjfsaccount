using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.DB.DTO
{
    public class DbSellingLetterDetail
    {
        public virtual long LetterID { get; set; }
        public virtual string LetterNo { get; set; }
        public virtual DateTime BuyingDate { get; set; }
        public virtual string CompanyName { get; set; }
        public virtual string BankName { get; set; }
        public virtual string BankBranch { get; set; }
        public virtual string AccountType { get; set; }
        public virtual string AccountNo { get; set; }
        public virtual long CreBy { get; set; }
        public virtual DateTime CreDate { get; set; }
        public virtual long UpdBy { get; set; }
        public virtual DateTime UpdDate { get; set; }
        public virtual Byte[] RowVersion
        {
            get;
            set;
        }
    }
}
