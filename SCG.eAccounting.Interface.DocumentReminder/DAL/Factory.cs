using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.DB.Query;
using SCG.eAccounting.Interface.Utilities;
using SCG.eAccounting.BLL;
using SCG.eAccounting.Query;
using SS.SU.BLL;

namespace SCG.eAccounting.Interface.DocumentReminder.DAL
{
    public class Factory
    {
        public Factory()
        {

        }

        public static IDbParameterQuery DbParameterQuery { get; set; }
        public static ISCGDocumentQuery SCGDocumentQuery { get; set; }
        public static ISCGEmailService SCGEmailService { get; set; }

        public static void CreateObject()
        {
            DbParameterQuery = (IDbParameterQuery)ObjectManager.GetObject("DbParameterQuery");
            SCGDocumentQuery = (ISCGDocumentQuery)ObjectManager.GetObject("SCGDocumentQuery");
            SCGEmailService = (ISCGEmailService)ObjectManager.GetObject("SCGEmailService");
        }
    }
}
