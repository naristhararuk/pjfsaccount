using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Security;
using SCG.eAccounting.BLL;
using SCG.eAccounting.Interface.Utilities;

namespace SCG.eAccounting.EmailResender.DAL
{
    public class Factory
    {
        public Factory()
        {
                    
        }
        public static IUserAccount UserAccount { get; set; }
        public static ISCGEmailService SCGEmailService { get; set; }
      

        public static void CreateObject()
        {
            UserAccount = (IUserAccount)ObjectManager.GetObject("UserAccount");
            SCGEmailService = (ISCGEmailService)ObjectManager.GetObject("SCGEmailService");
            UserAccount.CurrentProgramCode = "EmailResender";
        }
    }
}
