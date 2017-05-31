using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;
using SS.Standard.Utilities;
using SS.SU.DTO;
using SS.SU.BLL;
using SS.SU.DAL;
namespace SS.SU.BLL.Implement
{
    public partial class SuEHrProfileLogService : ServiceBase<SueHrProfileLog, long>, ISuEHrProfileLogService
    {
        public override IDao<SueHrProfileLog, long> GetBaseDao()
        {
            return DaoProvider.SuEHRProfileLogDao;
        }

        #region ISuEHrProfileLogService Members

        public void DeleteAll()
        {
            DaoProvider.SuEHRProfileLogDao.DeleteAll();
        }

        public void AddLog(SueHrProfileLog eHrLog)
        {
            DaoProvider.SuEHRProfileLogDao.Save(eHrLog);
        }

        #endregion

    }
}
