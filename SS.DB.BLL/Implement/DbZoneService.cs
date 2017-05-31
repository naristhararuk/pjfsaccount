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
    public partial class DbZoneService : ServiceBase<DbZone, short>, IDbZoneService
    {

        
        public override IDao<DbZone, short> GetBaseDao()
        {
            return SsDbDaoProvider.DbZoneDao;
        }

        //public IList<DbStatus> FindByDbStatusCriteria(DbStatus criteria, int firstResult, int maxResults, string sortExpression)
        //{
        //    return NHibernateDaoHelper.FindPagingByCriteria<DbStatus>(SsDbDaoProvider.DbStatusDao, "FindBySuGolbalTranslateCriteria", new object[] { criteria }, firstResult, maxResults, sortExpression);
        //}

        //public int CountByDbStatusCriteria(DbStatus criteria)
        //{
        //    return NHibernateDaoHelper.CountByCriteria(SsDbDaoProvider.DbStatusDao, "FindBySuGolbalTranslateCriteria", new object[] { criteria });
        //}

        

    }
}
