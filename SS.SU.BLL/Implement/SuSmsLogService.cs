using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;

using SS.SU.DTO;
using SS.SU.DTO.ValueObject;
using SS.SU.DAL;
using SS.SU.BLL;
using SS.Standard.Utilities;

namespace SS.SU.BLL.Implement
{
    public class SuSmsLogService : ServiceBase<SuSmsLog, long>, ISuSmsLogService
    {
        #region Override Method
        public override IDao<SuSmsLog, long> GetBaseDao()
        {
            return DaoProvider.SuSmsLogDao;
        }
        #endregion
    }
}
