using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.DB.Query;
using SCG.eAccounting.Interface.Utilities;
using SCG.DB.BLL;

namespace SCG.eAccounting.Interface.ImportVendor.DAL
{
    public class Factory
    {
        public Factory()
        {

        }

        public static IDbParameterQuery DbParameterQuery { get; set; }
        public static  IDbVendorTempService DbVendorTempService { get; set; }
        
        public static void CreateObject()
        {
            DbParameterQuery = (IDbParameterQuery)ObjectManager.GetObject("DbParameterQuery");
            DbVendorTempService = (IDbVendorTempService)ObjectManager.GetObject("DbVendorTempService");
        }
    }
}
