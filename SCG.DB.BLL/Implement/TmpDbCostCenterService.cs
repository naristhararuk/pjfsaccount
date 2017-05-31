using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;
using SCG.DB.DTO;
using SCG.DB.BLL;
using SCG.DB.DAL;
using SCG.DB.Query;
using SS.Standard.Utilities;
using SCG.DB.DTO.ValueObject;
using log4net;

namespace SCG.DB.BLL.Implement
{
    public partial class TmpDbCostCenterService : ServiceBase<TmpDbCostCenter, long>, ITmpDbCostCenterService
    {
        private static ILog logger = log4net.LogManager.GetLogger(typeof(TmpDbCostCenterService));

        public override IDao<TmpDbCostCenter, long> GetBaseDao()
        {
            return ScgDbDaoProvider.TmpDbCostCenterDao;
        }
        public void addTmpDbCostCenter(TmpDbCostCenter tmpDbCostCenter)
        {

            ScgDbDaoProvider.TmpDbCostCenterDao.addTmpCostCenter(tmpDbCostCenter);

        }
        public void deleteAllTmpDbCostCenter()
        {
            ScgDbDaoProvider.TmpDbCostCenterDao.deleteAllTmpDbCostCenter();

        }
        public void setActiveDbCosCenter(bool active, bool eccflag, string aliasName)
        {
            ScgDbDaoProvider.TmpDbCostCenterDao.setActiveAll(active, eccflag, aliasName);
        }

        public void addNewCostCenterFromTmp()
        {
            ScgDbDaoProvider.TmpDbCostCenterDao.insertTmpToDbCostCenter();
        }

        public void updateCostCenterFromTmp()
        {
            ScgDbDaoProvider.TmpDbCostCenterDao.updateTmpToDbCostCenter();
        }

        public void setCompanyIDToTmpCostCenter()
        {
            ScgDbDaoProvider.TmpDbCostCenterDao.getCompanyIdToTmpCostCenter();
        }

        public void checkMissingCostCenterCode()
        {
            ScgDbDaoProvider.TmpDbCostCenterDao.checkMissingCostCenterCode();
        }
        public void checkMissingCompany()
        {
            ScgDbDaoProvider.TmpDbCostCenterDao.checkMissingCompanyCode();
            ScgDbDaoProvider.TmpDbCostCenterDao.checkMissingCompanyID();
        }
        public void checkMissingDescription()
        {
            ScgDbDaoProvider.TmpDbCostCenterDao.checkMissingDescription();
        }

        public void checkValidDateFormat()
        {
            ScgDbDaoProvider.TmpDbCostCenterDao.checkValidDateFormat();
        }
        public void checkExpireDateFormat()
        {
            ScgDbDaoProvider.TmpDbCostCenterDao.checkExpireDateFormat();
        }

        public void ImportNewCostCenter(bool eccFlag, string aliasName)
        {
            try
            {
                //set all record in  DbCostCenter active = false
                ScgDbDaoProvider.TmpDbCostCenterDao.setActiveAll(false, eccFlag, aliasName);

                //insert new costcenter from Tmp_DbCostCentere to DbCostCenter
                ScgDbDaoProvider.TmpDbCostCenterDao.insertTmpToDbCostCenter();

                //update costcenter 
                ScgDbDaoProvider.TmpDbCostCenterDao.updateTmpToDbCostCenter();

                //sync all costcenter to database exp 4.7
                ScgDbDaoProvider.TmpDbCostCenterDao.SyncAllCostCenter();

                Console.WriteLine("Import Success");
            }
            catch (Exception ex)
            {
                //logger.Error("ImportNewCostCenter");
                //logger.Error(string.Format("Error Message : {0}", ex.Message));
                //logger.Error(string.Format("Stack Trace : {0}", ex.StackTrace));
                Console.WriteLine(ex.ToString());
                Console.WriteLine("Import UnSuccess!!");
            }
        }

        public bool IsDuplicateCostCenterCode(string costCenterCode)
        {
            return ScgDbDaoProvider.TmpDbCostCenterDao.CheckDuplicateCostCenterCodeFromTmpCostCenter(costCenterCode);
        }
    }
}
