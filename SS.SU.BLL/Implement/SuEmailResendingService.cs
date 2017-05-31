using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service.Implement;
using SS.SU.DTO;
using SS.SU.DAL;

namespace SS.SU.BLL.Implement
{
    public class SuEmailResendingService : ServiceBase<SuEmailResending,long>,ISuEmailResendingService
    {
        public override SS.Standard.Data.NHibernate.Dao.IDao<SuEmailResending, long> GetBaseDao()
        {
            return DaoProvider.SuEmailResendingDao;
        }
    }
}
