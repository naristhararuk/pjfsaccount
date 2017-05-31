using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.QueryDao;
using SCG.DB.DTO;
using NHibernate;
using NHibernate.Transform;

namespace SCG.DB.Query.Hibernate
{
    public class DbBuQuery : NHibernateQueryBase<DbBU, string>, IDbBuQuery
    {
        public IList<DbBU> FindBUALL()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT BuCode, BuName From DbBu ");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            
            query.AddScalar("BuCode", NHibernateUtil.String)
            .AddScalar("BuName", NHibernateUtil.String);
            
                return query.SetResultTransformer(Transformers.AliasToBean(typeof(DbBU))).List<DbBU>();
        }
    }
}
