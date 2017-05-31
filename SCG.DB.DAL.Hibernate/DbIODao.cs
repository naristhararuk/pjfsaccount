using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Dao;
using SCG.DB.DTO;
using NHibernate;

namespace SCG.DB.DAL.Hibernate
{
    public partial class DbIODao : NHibernateDaoBase<DbInternalOrder, long>, IDbIODao
    {
        public bool FindByIONumber(long IOID, string IONumber)
        {
            IList<DbInternalOrder> list = new List<DbInternalOrder>();
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine(" FROM DbInternalOrder internalOrder ");
            sqlBuilder.AppendLine(" WHERE internalOrder.IOID != :IOID and internalOrder.IONumber = :IONumber ");

            IQuery query = GetCurrentSession().CreateQuery(sqlBuilder.ToString());
            query.SetInt64("IOID", IOID);
            query.SetString("IONumber", IONumber);
            list = query.List<DbInternalOrder>();
            if (list.Count > 0)
                return true;
            else
                return false;

        }
        public void SyncNewIO()
        {
            ISQLQuery query = GetCurrentSession().CreateSQLQuery("exec [dbo].[SyncNewInternalOrderData]");
            query.AddScalar("count", NHibernateUtil.Int32).UniqueResult();
        }
        public void SyncUpdateIO(string iONumber)
        {
            ISQLQuery query = GetCurrentSession().CreateSQLQuery("exec [dbo].[SyncUpdateInternalOrderData] :IONumber ");
            query.SetString("IONumber", iONumber);
            query.AddScalar("count", NHibernateUtil.Int32).UniqueResult();
        }
        public void SyncDeleteIO(string iONumber)
        {
            ISQLQuery query = GetCurrentSession().CreateSQLQuery("exec [dbo].[SyncDeleteInternalOrderData] :IONumber ");
            query.SetString("IONumber", iONumber);
            query.AddScalar("count", NHibernateUtil.Int32).UniqueResult();
        }
    }
}
