using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;

using SCG.DB.DTO;
using SCG.DB.BLL;
using SCG.DB.DAL;
using SS.Standard.Utilities;

namespace SCG.DB.BLL.Implement
{
    public partial class DbReasonLangService : ServiceBase<DbScgReasonLang, long>, IDbReasonLangService
    {
        public override IDao<DbScgReasonLang, long> GetBaseDao()
        {
            return ScgDbDaoProvider.DbReasonLangDao;
        }

        public void UpdateReasonLang(IList<DbScgReasonLang> reasonLangList)
        {
            foreach (DbScgReasonLang rl in reasonLangList)
            {
                ScgDbDaoProvider.DbReasonLangDao.DeleteReasonLangByReasonId(rl.Reason.ReasonID);
            }
            foreach (DbScgReasonLang rl in reasonLangList)
            {
                ScgDbDaoProvider.DbReasonLangDao.Save(rl);
            }
        }
    }
}
