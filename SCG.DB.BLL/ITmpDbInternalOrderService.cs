using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service;

using SCG.DB.DTO;

namespace SCG.DB.BLL
{
    public interface  ITmpDbInternalOrderService :IService<TmpDbInternalOrder , long>
    {
        void addTmpInternalOrder(TmpDbInternalOrder tmpInternalOrder);
        void deleteAllInternalOrderTmp();
        void setInternalOrderActive(bool active, bool eccflag, string aliasName);
        void addNewInternalOrderFromTmp();
        void updateNewInternalOrder();
        void setCompanyIDAndCostCenterIDToTmp();
        void putMissingCompanyIDToLog();
        void putMissingCostCenterIDToLog();
        void putMissingIONumberToLog();
        void putMissingIOTypeToLog();
        void putMissingIOTextToLog();
        void putMissingCompanyCodeToLog();
        void putMissingCostCenterCodeToLog();
        void putMissingEffectiveDateToLog();
        void putMissingExpireDateDateToLog();
        void ImportNewInternalOrder(bool eccFlag,string aliasName);
    }
}
