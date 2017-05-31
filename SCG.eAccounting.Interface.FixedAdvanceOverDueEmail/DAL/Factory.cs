using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.DB.Query;
using SCG.eAccounting.Interface.Utilities;
using SCG.eAccounting.BLL;
using SCG.eAccounting.Query;
using SS.SU.BLL;
using SS.Standard.CommunicationService;
using SS.Standard.Data.Query.NHibernate;
using SS.Standard.Security;

namespace SCG.eAccounting.Interface.FixedAdvanceOverDueEmail.DAL
{
    public class Factory
    {
        public Factory()
        {

        }

        public static IFixedAdvanceDocumentService FixedAdvanceDocumentService { get; set; }
        public static IUserAccount UserAccount { get; set; }

        public static void CreateObject()
        {
            FixedAdvanceDocumentService = (IFixedAdvanceDocumentService)ObjectManager.GetObject("FixedAdvanceDocumentService");
            UserAccount = (IUserAccount)ObjectManager.GetObject("UserAccount");
            UserAccount.CurrentProgramCode = "FixedAdvanceOverDueEmail";
        }
    }
}
