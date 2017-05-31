using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.eAccounting.Query;
using SS.DB.Query;
using SCG.eAccounting.Interface.Utilities;

namespace SCG.eAccounting.Interface.Payroll.DAL
{
    public class Factory
    {
        public static IFnExpenseDocumentQuery FnExpenseDocumentQuery { get; set; }
        public static ParameterServices ParameterServices { get; set; }
        public static void CreateObject()
        {
            FnExpenseDocumentQuery = (IFnExpenseDocumentQuery)ObjectManager.GetObject("FnExpenseDocumentQuery");
            ParameterServices = (ParameterServices)ObjectManager.GetObject("ParameterServices");
           
        }
    }
}
