using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Dao;
using SCG.DB.DTO;

namespace SCG.DB.DAL
{
    public interface ITmpDbInternalOrderDao :IDao<TmpDbInternalOrder ,long>
    {
        void deleteAllInternalOrderTmp();
        void setActiveAll(bool active, bool eccflag, string aliasName);
        void addTmpInternalOrder(TmpDbInternalOrder tmpDbInternalOrder);
        void addNewInternalOrderFromTmp();
        void updateNewInternalOrder();
        void getCompanyIDAndCostCenterIDToTmp();
        void putMissingCompanyIDToLog();
        void putMissingCostCenterIDToLog();
        void putMissingIONumberToLog();
        void putMissingIOTypeToLog();
        void putMissingIOTextToLog();
        void putMissingCompanyCodeToLog();
        void putMissingCostCenterCodeToLog();
        void putMissingEffectiveDateToLog();
        void putMissingExpireDateDateToLog();
        void SyncAllInternalOrder();
    }
}
