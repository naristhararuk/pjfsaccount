using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Transform;
using SS.Standard.Data.NHibernate.Dao;

using SS.DB.DTO;
using SS.DB.DAL;
using NHibernate;
using NHibernate.Expression;
using SS.Standard.Data.NHibernate.QueryCreator;
namespace SS.DB.DAL.Hibernate
{
    public partial class DbZoneDao : NHibernateDaoBase<DbZone, short>, IDbZoneDao
    {
        

        //public bool IsDuplicate(DbStatus status)
        //{
        //    IList<DbStatus> list = GetCurrentSession().CreateQuery("from DbStatus s where s.StatusID = :StatusID ")
        //          .SetInt64("StatusID", status.StatusID)
        //          .List<DbStatus>();
        //    if (list.Count > 0)
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        //public ICriteria FindByDbStatusCriteria(DbStatus status)
        //{
        //    ICriteria criteria = GetCurrentSession().CreateCriteria(typeof(DbStatus), "s");
        //    return criteria;
        //}

        //public IList<DbStatus> FindByStatusCriteria(DbStatus status)
        //{
        //    StringBuilder sql = new StringBuilder();
        //    sql.Append("Select  StatusID,GroupStatus,Status,Comment,Active ");
        //    sql.Append("from DbStatus ");
        //    sql.Append("where GroupStatus = :GroupStatus ");
        //    sql.Append("and Status = :Status ");

        //    ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
        //    QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
        //    queryParameterBuilder.AddParameterData("GroupStatus", typeof(String), status.GroupStatus);
        //    queryParameterBuilder.AddParameterData("Status", typeof(String), status.Status);
        //    queryParameterBuilder.FillParameters(query);
        //    query.AddScalar("StatusID", NHibernateUtil.Int16);
        //    query.AddScalar("GroupStatus", NHibernateUtil.String);
        //    query.AddScalar("Status", NHibernateUtil.String);
        //    query.AddScalar("Comment", NHibernateUtil.String);
        //    query.AddScalar("Active", NHibernateUtil.Boolean);

        //    IList<SS.DB.DTO.DbStatus> list = query.SetResultTransformer(Transformers.AliasToBean(typeof(SS.DB.DTO.DbStatus)))
        //        .List<SS.DB.DTO.DbStatus>();

        //    return list;
        //}

        
    }
}
