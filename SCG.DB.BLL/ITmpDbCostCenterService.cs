using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service;
using SCG.DB.DTO;

namespace SCG.DB.BLL
{
    public interface ITmpDbCostCenterService : IService<TmpDbCostCenter,long>
    {
        void addTmpDbCostCenter(TmpDbCostCenter tmpDbCostCenter);
        void deleteAllTmpDbCostCenter();
        void setActiveDbCosCenter(bool active,bool eccflag,string aliasName);
        void addNewCostCenterFromTmp();
        void updateCostCenterFromTmp();
        void setCompanyIDToTmpCostCenter();
        void checkMissingCostCenterCode();
        void checkMissingCompany();
        void checkMissingDescription();
        void checkValidDateFormat();
        void checkExpireDateFormat();
        void ImportNewCostCenter(bool eccflag, string aliasName);
        bool IsDuplicateCostCenterCode(string costCenterCode);
    }
}
