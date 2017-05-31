﻿using System;
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
    public partial class DbRegionService : ServiceBase<DbRegion, short>, IDbRegionService
    {
        public override IDao<DbRegion, short> GetBaseDao()
        {
            return SsDbDaoProvider.DbRegionDao;
        }
    }
}
