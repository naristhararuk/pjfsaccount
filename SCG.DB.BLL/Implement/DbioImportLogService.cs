using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;
using SS.Standard.Utilities;

using SCG.DB.DTO;
using SCG.DB.BLL;
using SCG.DB.DAL;

namespace SCG.DB.BLL.Implement
{
    public partial class DbioImportLogService : ServiceBase<DbioImportLog , long> ,IDbioImportLogService
    {
        public override IDao<DbioImportLog, long> GetBaseDao()
        {
            return ScgDbDaoProvider.DbioImportLogDao;
        }
        public void addIOImportLog(DbioImportLog log)
        {
            ScgDbDaoProvider.DbioImportLogDao.Save(log);
        }
    }
}
