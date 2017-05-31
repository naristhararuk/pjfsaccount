using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service.Implement;
using SCG.DB.DTO;
using SS.Standard.Data.NHibernate.Dao;
using SCG.DB.DAL;
using SS.Standard.Utilities;

namespace SCG.DB.BLL.Implement
{
    public partial class DbVendorService : ServiceBase<DbVendor, long>, IDbVendorService
    {
        #region public override IDao<DbVendor, long> GetBaseDao()
        public override IDao<DbVendor, long> GetBaseDao()
        {
            return ScgDbDaoProvider.DbVendorDao;
        }
        #endregion public override IDao<DbVendor, long> GetBaseDao()
    }
}
