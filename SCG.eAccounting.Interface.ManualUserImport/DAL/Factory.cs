using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.SU.BLL;
using SCG.DB.BLL;
using SCG.eAccounting.Interface.Utilities;

namespace SCG.eAccounting.Interface.ManualUserImport.DAL
{
     public class Factory
    {
         public static void CreateObject()
         {
             TmpSuUserService = (ITmpSuUserService)ObjectManager.GetObject("TmpSuUserService");
             SuEHrProfileLogService = (ISuEHrProfileLogService)ObjectManager.GetObject("SuEHrProfileLogService");
         }

         public static ITmpSuUserService TmpSuUserService { get; set; }
         public static ISuEHrProfileLogService SuEHrProfileLogService { get; set; }
    }
}
