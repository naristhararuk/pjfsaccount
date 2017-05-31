using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.DB.Query;

namespace SCG.eAccounting.SAP.BAPI.Service.Const
{
    public static class PostingConst
    {
        public const string BusAct      = "RFBU";
        public static string UserCPIC
        {
            get 
            {
                return ParameterServices.BAPI_UserCPIC;
            }
        }
        public const string BRNCH       = "BRNCH";
        public const string VAT         = "VAT";
        public const string Currency    = "THB";
        public const string Pmnttrms    = "NT00";
        public const string PCADVCL     = "PCADVCL";

        public const string PBID        = "001";
        public const string PmntBlock   = "B";

        public static string ARCustomer
        {
            get
            {
                return ParameterServices.BAPI_ARAccount;
            }
        }
        public static string GLAccount
        { 
            get
            {
                return ParameterServices.BAPI_GLAccount;
            }
        }
        public static string WHTAccount
        {
            get
            {
                return ParameterServices.BAPI_WHTAccount;
            }
        }
    }
}
