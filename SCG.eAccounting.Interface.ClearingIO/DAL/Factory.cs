using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.DB.Query;
using SCG.eAccounting.Interface.Utilities;
using SCG.eAccounting.BLL;
using SCG.eAccounting.Query;
using SS.SU.BLL;
using SCG.DB.Query;

namespace SCG.eAccounting.Interface.ClearingIO.DAL 
{
    public class Factory
    {
        public Factory()
        {

        }

        public static IDbParameterQuery DbParameterQuery { get; set; }
        public static IFnAutoPaymentQuery FnAutoPaymentQuery { get; set; }
        public static IFnAutoPaymentTempService FnAutoPaymentTempService { get; set; }
        public static IFnAutoPaymentService FnAutoPaymentService { get; set; }
        public static IDbSapInstanceQuery DbSapInstanceQuery { get; set; }

        public static void CreateObject()
        {
            DbParameterQuery = (IDbParameterQuery)ObjectManager.GetObject("DbParameterQuery");
            FnAutoPaymentService = (IFnAutoPaymentService)ObjectManager.GetObject("FnAutoPaymentService");
            FnAutoPaymentQuery = (IFnAutoPaymentQuery)ObjectManager.GetObject("FnAutoPaymentQuery");
            FnAutoPaymentTempService = (IFnAutoPaymentTempService)ObjectManager.GetObject("FnAutoPaymentTempService");
            DbSapInstanceQuery = (IDbSapInstanceQuery)ObjectManager.GetObject("DbSapInstanceQuery");
        }
    }
}
