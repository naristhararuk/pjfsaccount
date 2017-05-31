using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;

using SS.SU.DTO;
using SS.SU.BLL;
using SS.SU.DAL;



namespace SS.SU.BLL.Implement
{
    public class SuUserLogService : ServiceBase<SuUserLog, long>, ISuUserLogService
    {
        #region Override Method
        public override IDao<SuUserLog, long> GetBaseDao()
        {
            return DaoProvider.SuUserLogDao;
        }
        #endregion
    }
}
