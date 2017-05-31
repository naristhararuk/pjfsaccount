using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.DB.BLL;
using SCG.eAccounting.Interface.Utilities;

namespace SCG.eAccounting.Interface.OrganizationImport.DAL
{
     public class Factory
    {
         public static void CreateObject()
         {
             TmpDbOrganizationChartService = (ITmpDbOrganizationChartService)ObjectManager.GetObject("TmpDbOrganizationChartService");
         }

         public static ITmpDbOrganizationChartService TmpDbOrganizationChartService { get; set; }
    }
}
