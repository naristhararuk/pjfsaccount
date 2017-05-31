using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service.Implement;
using SCG.eAccounting.DTO;
using SCG.eAccounting.DAL;
using SS.DB.Query; 

namespace SCG.eAccounting.BLL.Implement
{
    public partial class FnEACAdvanceImportLogService : ServiceBase<FnEACAdvanceImportLog,long>,IFnEACAdvanceImportLogService
    {
        public override SS.Standard.Data.NHibernate.Dao.IDao<FnEACAdvanceImportLog, long> GetBaseDao()
        {
            return ScgeAccountingDaoProvider.FnEACAdvanceImportLogDao;
        }
        public void SetActiveFalse()
        {
            ScgeAccountingDaoProvider.FnEACAdvanceImportLogDao.SetAllActiveFalse();
        }

        public void SaveLog(string EACAdvanceNo, bool status, string message)
        {
            FnEACAdvanceImportLog log = new FnEACAdvanceImportLog();
            log.Active = true;
            log.CreBy = ParameterServices.SystemUserID;
            log.CreDate = DateTime.Now;
            log.EACRequestNo = EACAdvanceNo;
            log.Message = message;
            log.UpdBy = ParameterServices.SystemUserID;
            log.UpdDate = DateTime.Now;
            log.UpdPgm = "InterfaceImportAdvance";
            log.Status = "Fail";
          Save(log);
        }
    }
}
