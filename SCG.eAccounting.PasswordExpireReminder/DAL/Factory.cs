using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.DB.Query;
using SCG.eAccounting.Interface.Utilities;
using SCG.eAccounting.BLL;
using SCG.eAccounting.Query;
using SS.SU.BLL;
using SS.SU.Query;
using SS.Standard.Security;

namespace SCG.eAccounting.PasswordExpireReminder.DAL
{
    public class Factory
    {
        public Factory()
        {

        }

        public static ISCGEmailService SCGEmailService { get; set; }
        public static ISuUserService SuUserService { get; set; }
        public static ISuUserQuery SuUserQuery { get; set; }
        public static IUserAccount UserAccount 
        { get; set; }

        
         public static void CreateObject()
         {
             SuUserService = (ISuUserService)ObjectManager.GetObject("SuUserService");
             SCGEmailService = (ISCGEmailService)ObjectManager.GetObject("SCGEmailService");
             SuUserQuery = (ISuUserQuery)ObjectManager.GetObject("SuUserQuery");
             UserAccount = (IUserAccount)ObjectManager.GetObject("UserAccount");
             UserAccount.CurrentProgramCode = "PasswordExpire";
         

   
         }
    }
}
