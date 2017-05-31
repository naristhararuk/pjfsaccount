using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.SAP.BAPI.Service.Const
{
    public static class PaymentTypeConst
    {
        public const string DomesticCash            = "CA";
        public const string DomesticCheque          = "CQ";
        public const string DomesticTransfer        = "TR";
        public const string ForeignBankNote         = "BN";
        public const string ForeignTravellerCheque  = "TQ";
        public const string ForeignDraft            = "DF";
    }
}
