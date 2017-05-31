using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service.Implement;
using SCG.DB.DTO;
using SCG.DB.DAL;

namespace SCG.DB.BLL.Implement
{
    public class DbVendorTempService : ServiceBase<DbVendorTemp, long>, IDbVendorTempService
    {

        public override SS.Standard.Data.NHibernate.Dao.IDao<DbVendorTemp, long> GetBaseDao()
        {
            return ScgDbDaoProvider.DbVendorTempDao;
        }

        public void CommitTempToVendor()
        {
            ScgDbDaoProvider.DbVendorTempDao.UpdateTempToVendor();
            ScgDbDaoProvider.DbVendorTempDao.SaveTempToVendor();
        }

        #region IDbVendorTempService Members

        public void DeleteAll()
        {
            ScgDbDaoProvider.DbVendorTempDao.DeleteAll();
        }

        #endregion
    }
}
