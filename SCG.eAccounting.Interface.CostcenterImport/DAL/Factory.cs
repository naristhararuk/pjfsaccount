using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.DB.BLL;
using SCG.eAccounting.Interface.Utilities;

namespace SCG.eAccounting.Interface.CostcenterImport.DAL
{
    public class Factory
    {
        public static void CreateTmpDbCostCenterObject()
        {
            TmpDbCostCenterService = (ITmpDbCostCenterService)ObjectManager.GetObject("TmpDbCostCenterService");

        }
        public static void CreateCompanyObject()
        {
            DbCompanyService = (IDbCompanyService)ObjectManager.GetObject("DbCompanyService");
        }
        public static void CreateDbCostCenterImportLogObject()
        {
            DbCostCenterImportLogService = (IDbCostCenterImportLogService)ObjectManager.GetObject("DbCostCenterImportLogService");
        }

        public static ITmpDbCostCenterService TmpDbCostCenterService { get; set; }
        public static IDbCompanyService DbCompanyService { get; set; }
        public static IDbCostCenterImportLogService DbCostCenterImportLogService { get; set; }
        //public static IDbCompanyService DbCompanyService { get; set; }
        //public static IDbCompanyService DbCompanyService { get; set; }
        
    }
}
