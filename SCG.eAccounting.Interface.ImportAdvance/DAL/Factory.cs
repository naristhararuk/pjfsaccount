using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.DB.Query;
using SCG.eAccounting.Interface.Utilities;
using SCG.eAccounting.BLL;
using SCG.eAccounting.Query;
using SS.SU.BLL;

namespace SCG.eAccounting.Interface.ImportAdvance.DAL
{
    public class Factory
    {
        public Factory()
        {

        }

        public static IDbParameterQuery DbParameterQuery { get; set; }
        public static IFnEACAdvanceImportLogService FnEACAdvanceImportLogService { get; set; }
        public static IFnEACAdvanceImportTempService FnEACAdvanceImportTempService { get; set; }


        public static void CreateObject()
        {
            DbParameterQuery = (IDbParameterQuery)ObjectManager.GetObject("DbParameterQuery");
            FnEACAdvanceImportLogService = (IFnEACAdvanceImportLogService)ObjectManager.GetObject("FnEACAdvanceImportLogService");
            FnEACAdvanceImportTempService = (IFnEACAdvanceImportTempService)ObjectManager.GetObject("FnEACAdvanceImportTempService");
        }
    }
}
