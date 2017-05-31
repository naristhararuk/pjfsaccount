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
    public partial class DbProvinceService : ServiceBase<DbProvince, short>, IDbProvinceService
    {
        public override IDao<DbProvince, short> GetBaseDao()
        {
            return SsDbDaoProvider.DbProvinceDao;
        }
    }
}
