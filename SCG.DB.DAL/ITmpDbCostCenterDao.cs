using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Dao;

using SCG.DB.DTO;

namespace SCG.DB.DAL
{
    public interface ITmpDbCostCenterDao : IDao<TmpDbCostCenter,long>
    {
        void deleteAllTmpDbCostCenter();
        void addTmpCostCenter(TmpDbCostCenter tmpDbCosCenter);
        void setActiveAll(bool active, bool eccflag, string aliasName);
        void insertTmpToDbCostCenter();
        void updateTmpToDbCostCenter();
        void getCompanyIdToTmpCostCenter();
        void checkMissingCostCenterCode();
        void checkMissingCompanyCode();
        void checkMissingDescription();
        void checkMissingCompanyID();
        void checkValidDateFormat();
        void checkExpireDateFormat();
        bool CheckDuplicateCostCenterCodeFromTmpCostCenter(string costCenterCode);
        void SyncAllCostCenter();
    }
}
