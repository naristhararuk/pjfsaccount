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


namespace SCG.DB.BLL.Implement
{
    public partial class TmpDbInternalOrderService : ServiceBase<TmpDbInternalOrder, long>, ITmpDbInternalOrderService
    {
        public override IDao<TmpDbInternalOrder, long> GetBaseDao()
        {
            return ScgDbDaoProvider.TmpDbInternalOrderDao;
        }

        public void addTmpInternalOrder(TmpDbInternalOrder tmpInternalOrder)
        {
            ScgDbDaoProvider.TmpDbInternalOrderDao.Save(tmpInternalOrder);
        }
        public void deleteAllInternalOrderTmp()
        {
            ScgDbDaoProvider.TmpDbInternalOrderDao.deleteAllInternalOrderTmp();
        }
        public void setInternalOrderActive(bool active,bool eccflag,string aliasName)
        {
            ScgDbDaoProvider.TmpDbInternalOrderDao.setActiveAll(active,eccflag,aliasName);
        }
        public void addNewInternalOrderFromTmp()
        {
            ScgDbDaoProvider.TmpDbInternalOrderDao.addNewInternalOrderFromTmp();
        }

        public void updateNewInternalOrder()
        {
            ScgDbDaoProvider.TmpDbInternalOrderDao.updateNewInternalOrder();
        }
        public void setCompanyIDAndCostCenterIDToTmp()
        {
            ScgDbDaoProvider.TmpDbInternalOrderDao.getCompanyIDAndCostCenterIDToTmp();
        }

        public void putMissingCompanyIDToLog()
        {
            ScgDbDaoProvider.TmpDbInternalOrderDao.putMissingCompanyIDToLog();
        }
        public void putMissingCostCenterIDToLog()
        {
            ScgDbDaoProvider.TmpDbInternalOrderDao.putMissingCostCenterIDToLog();
        }
        public void putMissingIONumberToLog()
        {
            ScgDbDaoProvider.TmpDbInternalOrderDao.putMissingIONumberToLog();
        }
        public void putMissingIOTypeToLog()
        {
            ScgDbDaoProvider.TmpDbInternalOrderDao.putMissingIOTypeToLog();
        }
        public void putMissingIOTextToLog()
        {
            ScgDbDaoProvider.TmpDbInternalOrderDao.putMissingIOTextToLog();
        }
        public void putMissingCompanyCodeToLog()
        {
            ScgDbDaoProvider.TmpDbInternalOrderDao.putMissingCompanyCodeToLog();
        }
        public void putMissingCostCenterCodeToLog()
        {
            ScgDbDaoProvider.TmpDbInternalOrderDao.putMissingCostCenterCodeToLog();
        }
        public void putMissingEffectiveDateToLog()
        {
            ScgDbDaoProvider.TmpDbInternalOrderDao.putMissingEffectiveDateToLog();
        }
        public void putMissingExpireDateDateToLog()
        {
            ScgDbDaoProvider.TmpDbInternalOrderDao.putMissingExpireDateDateToLog();
        }

        public void ImportNewInternalOrder(bool eccFlag,string aliasName)
        {
            try
            {

                // set all active = false
                ScgDbDaoProvider.TmpDbInternalOrderDao.setActiveAll(false, eccFlag,aliasName);

                ScgDbDaoProvider.TmpDbInternalOrderDao.addNewInternalOrderFromTmp();

                ScgDbDaoProvider.TmpDbInternalOrderDao.updateNewInternalOrder();

                //sync all internal to database exp 4.7
                ScgDbDaoProvider.TmpDbInternalOrderDao.SyncAllInternalOrder();

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
    }
}
