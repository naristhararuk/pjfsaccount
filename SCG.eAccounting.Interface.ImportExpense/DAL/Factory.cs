using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.DB.Query;
using SCG.eAccounting.Interface.Utilities;
using SCG.eAccounting.BLL;
using SCG.eAccounting.Query;
using SS.SU.BLL;

namespace SCG.eAccounting.Interface.ImportExpense.DAL
{
    public class Factory
    {
        public Factory()
        {

        }

        public static IDbParameterQuery DbParameterQuery { get; set; }
        public static IFnEHRexpenseTempService FnEHRexpenseTempService { get; set; }
        public static IFnEHRexpenseTempQuery FnEHRexpenseTempQuery { get; set; }

        public static void CreateObject()
        {
            FnEHRexpenseTempService = (IFnEHRexpenseTempService)ObjectManager.GetObject("FnEHRexpenseTempService");
            DbParameterQuery = (IDbParameterQuery)ObjectManager.GetObject("DbParameterQuery");
            FnEHRexpenseTempQuery = (IFnEHRexpenseTempQuery)ObjectManager.GetObject("FnEHRexpenseTempQuery");
        }
    }
}
