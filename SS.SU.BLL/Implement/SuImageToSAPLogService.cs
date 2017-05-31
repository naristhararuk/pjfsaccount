using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service.Implement;
using SS.SU.DTO;
using SS.SU.DAL;

namespace SS.SU.BLL.Implement
{
   public partial class SuImageToSAPLogService :ServiceBase<SuImageTosapLog,long>,ISuImageToSAPLogService
    {
        public override SS.Standard.Data.NHibernate.Dao.IDao<SuImageTosapLog, long> GetBaseDao()
        {
            return DaoProvider.SuImageToSAPLogDao;
        }
    }
}
