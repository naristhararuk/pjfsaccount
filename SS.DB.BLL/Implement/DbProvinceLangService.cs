using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;

using SS.DB.DTO;
using SS.DB.DAL;
using SS.SU.BLL;
using SS.Standard.Utilities;

namespace SS.DB.BLL.Implement
{
    public partial class DbProvinceLangService : ServiceBase<DbProvinceLang, long>, IDbProvinceLangService
    {
        public override IDao<DbProvinceLang, long> GetBaseDao()
        {
            return SsDbDaoProvider.DbProvinceLangDao;
        }

        public void UpdateProvinceLang(IList<DbProvinceLang> provinceLangList)
        {
            if (provinceLangList.Count > 0)
            {
                SsDbDaoProvider.DbProvinceLangDao.DeleteAllProvinceLang(provinceLangList[0].Province.Provinceid);
            }
            foreach (DbProvinceLang provinceLang in provinceLangList)
            {
                this.Save(provinceLang);
                //SsDbDaoProvider.DbProvinceLangDao.Save(provinceLang);
            }

        }
    }
}
