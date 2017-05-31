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
using SCG.DB.DTO.ValueObject;

namespace SCG.DB.BLL.Implement
{
    public partial class DbCostCenterImportLogService : ServiceBase<DbCostCenterImportLog , long > ,IDbCostCenterImportLogService
    {
        public override IDao<DbCostCenterImportLog, long> GetBaseDao()
        {
            return ScgDbDaoProvider.DbCostCenterImportLogDao;
        }
        public void addCostCenterImportLog(DbCostCenterImportLog dbCostCenterImportLog)
        {
            ScgDbDaoProvider.DbCostCenterImportLogDao.Save(dbCostCenterImportLog);
        }
    }
}
