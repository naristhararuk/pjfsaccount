using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using NHibernate.Transform;

using SS.Standard.Data.NHibernate.Query;
using SS.Standard.Data.NHibernate.QueryDao;
using SS.Standard.Data.NHibernate.QueryCreator;

using SS.DB.DTO;
using SS.DB.Query;
using SS.DB.DTO.ValueObject;

namespace SS.DB.Query.Hibernate
{
    public class DbZoneQuery : NHibernateQueryBase<DbZone, short>, IDbZoneQuery
    {
        public IList<DbZoneResult> GetZoneList(short languageId, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<DbZoneResult>(SsDbQueryProvider.DbZoneQuery, "FindZoneByCriteria", new object[] { languageId, sortExpression, false }, firstResult, maxResult, sortExpression);
        }
        public int CountZoneByCriteria()
        {
            return NHibernateQueryHelper.CountByCriteria(SsDbQueryProvider.DbZoneQuery, "FindZoneByCriteria", new object[] { Convert.ToInt16(0), string.Empty, true });
        }

        public ISQLQuery FindZoneByCriteria(short languageId, string sortExpression, bool isCount)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            if (!isCount)
            {
                sqlBuilder.Append("select z.ZoneID as ZoneID,zl.ZoneName as ZoneName,z.Comment as Comment,z.Active as Active ");
                sqlBuilder.Append("from DbZone as z ");
                sqlBuilder.Append("left join DbZoneLang as zl on z.ZoneID = zl.ZoneID and zl.LanguageID =:LanguageID ");
                if (string.IsNullOrEmpty(sortExpression))
                {
                    sqlBuilder.AppendLine("ORDER BY ZoneID,ZoneName,Comment,Active ");
                }
                else
                {
                    sqlBuilder.AppendLine(string.Format(" ORDER BY {0}", sortExpression));
                }
            }
            else
            {
                sqlBuilder.Append("select count(z.ZoneID) as ZoneCount from DbZone as z ");
            }

            ISQLQuery query;

            if (!isCount)
            {
                query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
                QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
                queryParameterBuilder.AddParameterData("LanguageID", typeof(Int16), languageId);
                queryParameterBuilder.FillParameters(query);
                query.AddScalar("ZoneID", NHibernateUtil.Int16);
                query.AddScalar("ZoneName", NHibernateUtil.String);
                query.AddScalar("Comment", NHibernateUtil.String);
                query.AddScalar("Active", NHibernateUtil.Boolean);


                query.SetResultTransformer(Transformers.AliasToBean(typeof(DbZoneResult)));

                //IList<SS.SU.DTO.ValueObject.RoleLang> list = query.SetResultTransformer(Transformers.AliasToBean(typeof(SS.SU.DTO.ValueObject.RoleLang)))
                //    .List<SS.SU.DTO.ValueObject.RoleLang>();
            }
            else
            {
                query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
                query.AddScalar("ZoneCount", NHibernateUtil.Int32);
                query.UniqueResult();
            }
            return query;
        }

        public IList<DbZoneResult> FindZoneByID(short zoneId,short languageId)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("select z.ZoneID as ZoneID,zl.ID as ZoneLangID,zl.ZoneName as ZoneName,z.Comment as Comment,z.Active as Active ");
            sqlBuilder.Append("from DbZone as z ");
            sqlBuilder.Append("left join DbZoneLang as zl on z.ZoneID = zl.ZoneID and zl.LanguageID =:LanguageID ");
            sqlBuilder.Append("Where z.ZoneID =:ZoneID");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("ZoneID", typeof(Int16), zoneId);
            queryParameterBuilder.AddParameterData("LanguageID", typeof(Int16), languageId);
            queryParameterBuilder.FillParameters(query);
            query.AddScalar("ZoneID", NHibernateUtil.Int16);
            query.AddScalar("ZoneLangID", NHibernateUtil.Int16);
            query.AddScalar("ZoneName", NHibernateUtil.String);
            query.AddScalar("Comment", NHibernateUtil.String);
            query.AddScalar("Active", NHibernateUtil.Boolean);
            query.SetResultTransformer(Transformers.AliasToBean(typeof(DbZoneResult)));

            return query.List<DbZoneResult>();
        }
        public IList<DbZoneResult> FindZone(short languageId)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("select z.ZoneID as ZoneID,zl.ID as ZoneLangID,zl.ZoneName as ZoneName,z.Comment as Comment,z.Active as Active ");
            sqlBuilder.Append("from DbZone as z ");
            sqlBuilder.Append("left join DbZoneLang as zl on z.ZoneID = zl.ZoneID and zl.LanguageID =:LanguageID ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("LanguageID", typeof(Int16), languageId);
            queryParameterBuilder.FillParameters(query);
            query.AddScalar("ZoneID", NHibernateUtil.Int16);
            query.AddScalar("ZoneLangID", NHibernateUtil.Int16);
            query.AddScalar("ZoneName", NHibernateUtil.String);
            query.AddScalar("Comment", NHibernateUtil.String);
            query.AddScalar("Active", NHibernateUtil.Boolean);
            query.SetResultTransformer(Transformers.AliasToBean(typeof(DbZoneResult)));

            return query.List<DbZoneResult>();
        }
    }
}
