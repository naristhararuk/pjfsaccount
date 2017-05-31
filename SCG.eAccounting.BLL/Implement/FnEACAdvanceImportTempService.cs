using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service.Implement;
using SCG.eAccounting.DTO;
using SCG.eAccounting.DAL; 
namespace SCG.eAccounting.BLL.Implement
{
    class FnEACAdvanceImportTempService : ServiceBase<FneacAdvanceImportTemp,long>,IFnEACAdvanceImportTempService
    {
        public override SS.Standard.Data.NHibernate.Dao.IDao<FneacAdvanceImportTemp, long> GetBaseDao()
        {
            return ScgeAccountingDaoProvider.FnEACAdvanceImportTempDao;
        }
        public void ClearAll()
        {
            ScgeAccountingDaoProvider.FnEACAdvanceImportTempDao.ClearTempData();
            ScgeAccountingDaoProvider.FnEACAdvanceImportLogDao.SetAllActiveFalse();
        }



        #region IFnEACAdvanceImportTempService Members


        public void ImportIntoDatabase()
        {
            ScgeAccountingDaoProvider.FnEACAdvanceImportTempDao.ResolvePBCode();
            ScgeAccountingDaoProvider.FnEACAdvanceImportTempDao.ResolveRequesterID();
            ScgeAccountingDaoProvider.FnEACAdvanceImportTempDao.ResolveReceiver();
            ScgeAccountingDaoProvider.FnEACAdvanceImportTempDao.ResolveApproverID();
            ScgeAccountingDaoProvider.FnEACAdvanceImportTempDao.ResolveCompany();
            ScgeAccountingDaoProvider.FnEACAdvanceImportTempDao.ResolveExpenseNo();
            ScgeAccountingDaoProvider.FnEACAdvanceImportTempDao.DeleteFailFromLog();
            ScgeAccountingDaoProvider.FnEACAdvanceImportTempDao.ResolveDocumentNo();
            ScgeAccountingDaoProvider.FnEACAdvanceImportTempDao.SaveToDataBase();
            ScgeAccountingDaoProvider.FnEACAdvanceImportTempDao.SaveSuccessToLog();
        }

        #endregion
    }
}
