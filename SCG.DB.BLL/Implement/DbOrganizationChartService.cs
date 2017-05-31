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
    public partial class DbOrganizationChartService : ServiceBase<DbOrganizationchart, long>, IDbOrganizationChartService
    {
        public override IDao<DbOrganizationchart, long> GetBaseDao()
        {
            return ScgDbDaoProvider.DbOrganizationChartDao;
        }
    }
}
