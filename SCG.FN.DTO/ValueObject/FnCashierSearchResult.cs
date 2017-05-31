using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.SU.DTO;

namespace SCG.FN.DTO
{
    [Serializable]
    public class FnCashierSearchResult
    {

        #region Property

        public long Id { get; set; }
        public short CashierId { get; set; }
        public string CashierCode { get; set; }
        public string CashierLevel { get; set; }
        public short DivisionId { get; set; }
        public string DivisionName { get; set; }
        public string CashierName { get; set; }
        public Boolean Active { get; set; }
        public string LanguageName { get; set; }
        public short LanguageID { get; set; }
        public string Comment { get; set; }
        #endregion
    }
}
