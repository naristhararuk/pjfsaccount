using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.DB.BLL;
using SCG.eAccounting.Interface.Utilities;

namespace SCG.eAccounting.Interface.InternalOrderImport.DAL
{
    public class Factory
    {

        public static void CreateTmpDbInternalOrderObject()
        {
            TmpDbInternalOrderService = (ITmpDbInternalOrderService)ObjectManager.GetObject("TmpDbInternalOrderService");
        }

        public static void CreateDbioImportLogObject()
        {
            DbioImportLogService = (IDbioImportLogService)ObjectManager.GetObject("DbioImportLogService");
        }
        public static void CreateCompanyObject()
        {
            DbCompanyService = (IDbCompanyService)ObjectManager.GetObject("DbCompanyService");
        }

        public static void CreateCostCenterObject()
        {
            DbCostCenterService = (IDbCostCenterService)ObjectManager.GetObject("DbCostCenterService");
        }


        public static ITmpDbInternalOrderService TmpDbInternalOrderService { get; set; }
        public static IDbioImportLogService DbioImportLogService  { get; set; }
        public static IDbCompanyService DbCompanyService { get; set; }
        public static IDbCostCenterService DbCostCenterService { get; set; }
    }
}
