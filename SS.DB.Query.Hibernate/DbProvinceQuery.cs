using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.QueryDao;
using SS.Standard.Data.NHibernate.QueryCreator;
using SS.Standard.Utilities;
using SS.Standard.Security;

using SS.DB.DTO;
using SS.DB.Query;
using SS.DB.DTO.ValueObject;

using NHibernate;
using NHibernate.Transform;

namespace SS.DB.Query.Hibernate
{
    public class DbProvinceQuery : NHibernateQueryBase<DbProvince, short>, IDbProvinceQuery
    {
        public ISQLQuery FindByProvinceCriteria(ProvinceLang province, bool isCount, short languageId, string sortExpression)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            if (!isCount)
            {
                sqlBuilder.Append(" SELECT ");
                sqlBuilder.Append("     DbProvince.ProvinceID       AS ProvinceId   ,");
                sqlBuilder.Append("     DbProvince.RegionID         AS RegionId     ,");
                sqlBuilder.Append("     DbProvince.Comment          AS Comment      ,");
                sqlBuilder.Append("     DbProvince.Active           AS Active       ,");
                sqlBuilder.Append("     DbProvinceLang.ProvinceName AS ProvinceName ,");
                sqlBuilder.Append("     DbRegionLang.RegionName     AS RegionName   ");
                sqlBuilder.Append(" FROM DbProvince ");
                sqlBuilder.Append("     LEFT JOIN DbProvinceLang ON ");
                sqlBuilder.Append("         DbProvince.ProvinceId             = DbProvinceLang.ProvinceId AND ");
                sqlBuilder.Append("         DbProvinceLang.LanguageID   = :LanguageId ");
                sqlBuilder.Append("     LEFT JOIN DbRegionLang ON ");
                sqlBuilder.Append("         DbProvince.RegionId             = DbRegionLang.RegionId AND ");
                sqlBuilder.Append("         DbRegionLang.LanguageID     = :LanguageId ");

                if (string.IsNullOrEmpty(sortExpression))
                    sqlBuilder.AppendLine("ORDER BY DbProvince.ProvinceID,DbProvince.RegionID,DbProvince.Comment,DbProvince.Active ");
                else
                    sqlBuilder.AppendLine(string.Format(" ORDER BY {0}", sortExpression));
            }
            else
            {
                sqlBuilder.Append(" SELECT COUNT(ProvinceID) AS ProvinceCount FROM DbProvince ");
            }

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());

            if (!isCount)
            {
                QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
                queryParameterBuilder.AddParameterData("LanguageId", typeof(Int16), languageId);
                queryParameterBuilder.FillParameters(query);
                query.AddScalar("ProvinceId"    , NHibernateUtil.Int16);
                query.AddScalar("RegionId"      , NHibernateUtil.Int16);
                query.AddScalar("Comment"       , NHibernateUtil.String);
                query.AddScalar("Active"        , NHibernateUtil.Boolean);
                query.AddScalar("ProvinceName"  , NHibernateUtil.String);
                query.AddScalar("RegionName"    , NHibernateUtil.String);

                query.SetResultTransformer(Transformers.AliasToBean(typeof(ProvinceLang)));
            }
            else
            {
                query.AddScalar("ProvinceCount", NHibernateUtil.Int32);
                query.UniqueResult();
            }
            return query;
        }

        public IList<ProvinceLang> GetProvinceList(ProvinceLang province, short languageId, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<ProvinceLang>(SsDbQueryProvider.DbProvinceQuery, "FindByProvinceCriteria", new object[] { province , false, languageId, sortExpression }, firstResult, maxResult, sortExpression);
        }

        public int CountByProvinceCriteria(ProvinceLang province)
        {
            return NHibernateQueryHelper.CountByCriteria(SsDbQueryProvider.DbProvinceQuery, "FindByProvinceCriteria", new object[] { province, true, Convert.ToInt16(0), string.Empty });
        }

        // For Lov
        public ISQLQuery FindByProvinceLovCriteria(ProvinceLang province, bool isCount, short languageId, string sortExpression, string ProvinceID, string ProvinceName, string RegionID)
        {
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            StringBuilder sqlBuilder = new StringBuilder();
            if (!isCount)
            {
                sqlBuilder.Append(" SELECT ");
                sqlBuilder.Append("     DbProvince.ProvinceID       AS ProvinceId   ,");
                sqlBuilder.Append("     DbProvince.RegionID         AS RegionId     ,");
                sqlBuilder.Append("     DbProvince.Comment          AS Comment      ,");
                sqlBuilder.Append("     DbProvince.Active           AS Active       ,");
                sqlBuilder.Append("     DbProvinceLang.ProvinceName AS ProvinceName ,");
                sqlBuilder.Append("     DbRegionLang.RegionName     AS RegionName   ");
                sqlBuilder.Append(" FROM DbProvince ");
                sqlBuilder.Append("     LEFT JOIN DbProvinceLang ON ");
                sqlBuilder.Append("         DbProvince.ProvinceId             = DbProvinceLang.ProvinceId AND ");
                sqlBuilder.Append("         DbProvinceLang.LanguageID   = :LanguageId ");
                sqlBuilder.Append("     LEFT JOIN DbRegionLang ON ");
                sqlBuilder.Append("         DbProvince.RegionId             = DbRegionLang.RegionId AND ");
                sqlBuilder.Append("         DbRegionLang.LanguageID     = :LanguageId ");
                sqlBuilder.Append(" WHERE 1=1 ");

                if (ProvinceID != null && ProvinceID != "")
                {
                    sqlBuilder.Append(" AND DbProvince.ProvinceID = :ProvinceId ");
                    parameterBuilder.AddParameterData("ProvinceId", typeof(short), short.Parse(ProvinceID));
                }
                if (ProvinceName != null && ProvinceName != "")
                {
                    sqlBuilder.Append(" AND DbProvinceLang.ProvinceName LIKE :ProvinceName ");
                    parameterBuilder.AddParameterData("ProvinceName", typeof(string), "%" + ProvinceName + "%");
                }
                if (RegionID != null && RegionID != "")
                {
                    sqlBuilder.Append(" AND DbProvince.RegionID = :RegionId ");
                    parameterBuilder.AddParameterData("RegionId", typeof(short), short.Parse(RegionID));
                }

                parameterBuilder.AddParameterData("LanguageId", typeof(short), languageId);

                if (string.IsNullOrEmpty(sortExpression))
                    sqlBuilder.AppendLine(" ORDER BY DbProvince.ProvinceID,DbProvince.RegionID,DbProvince.Comment,DbProvince.Active ");
                else
                    sqlBuilder.AppendLine(string.Format(" ORDER BY {0}", sortExpression));

            }
            else
            {
                sqlBuilder.Append(" SELECT COUNT(DbProvince.ProvinceID) AS ProvinceCount ");
                sqlBuilder.Append(" FROM DbProvince ");
                sqlBuilder.Append("     LEFT JOIN DbProvinceLang ON ");
                sqlBuilder.Append("         DbProvince.ProvinceId             = DbProvinceLang.ProvinceId AND ");
                sqlBuilder.Append("         DbProvinceLang.LanguageID   = :LanguageId ");
                sqlBuilder.Append("     LEFT JOIN DbRegionLang ON ");
                sqlBuilder.Append("         DbProvince.RegionId             = DbRegionLang.RegionId AND ");
                sqlBuilder.Append("         DbRegionLang.LanguageID     = :LanguageId ");
                sqlBuilder.Append(" WHERE 1=1 ");

                if (ProvinceID != null && ProvinceID != "")
                {
                    sqlBuilder.Append(" AND DbProvince.ProvinceID = :ProvinceId ");
                    parameterBuilder.AddParameterData("ProvinceId", typeof(short), short.Parse(ProvinceID));
                }
                if (ProvinceName != null && ProvinceName != "")
                {
                    sqlBuilder.Append(" AND DbProvinceLang.ProvinceName LIKE :ProvinceName ");
                    parameterBuilder.AddParameterData("ProvinceName", typeof(string), "%" + ProvinceName + "%");
                }
                if (RegionID != null && RegionID != "")
                {
                    sqlBuilder.Append(" AND DbProvince.RegionID = :RegionId ");
                    parameterBuilder.AddParameterData("RegionId", typeof(short), short.Parse(RegionID));
                }

                parameterBuilder.AddParameterData("LanguageId", typeof(short), languageId);
            }


            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            parameterBuilder.FillParameters(query);

            if (!isCount)
            {
                query.AddScalar("ProvinceId"    , NHibernateUtil.Int16);
                query.AddScalar("RegionId"      , NHibernateUtil.Int16);
                query.AddScalar("Comment"       , NHibernateUtil.String);
                query.AddScalar("Active"        , NHibernateUtil.Boolean);
                query.AddScalar("ProvinceName"  , NHibernateUtil.String);
                query.AddScalar("RegionName"    , NHibernateUtil.String);

                query.SetResultTransformer(Transformers.AliasToBean(typeof(ProvinceLang)));
            }
            else
            {
                query.AddScalar("ProvinceCount", NHibernateUtil.Int32);
                query.UniqueResult();
            }

            return query;
        }

        public IList<ProvinceLang> GetProvinceLovList(ProvinceLang province, short languageId, int firstResult, int maxResult, string sortExpression, string ProvinceID, string ProvinceName, string RegionID)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<ProvinceLang>(SsDbQueryProvider.DbProvinceQuery, "FindByProvinceLovCriteria", new object[] { province, false, languageId, sortExpression, ProvinceID, ProvinceName, RegionID }, firstResult, maxResult, sortExpression);
        }

        public int CountByProvinceLovCriteria(ProvinceLang province, string ProvinceID, string ProvinceName, string RegionID)
        {
            return NHibernateQueryHelper.CountByCriteria(SsDbQueryProvider.DbProvinceQuery, "FindByProvinceLovCriteria", new object[] { province, true, Convert.ToInt16(0), string.Empty, ProvinceID, ProvinceName, RegionID });
        }

        //AUTO Complete
        public IList<DBProvinceLovReturn> FindByProvinceAutoComplete(string ProvincePrefix, string languageId, string RegionID)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" SELECT ");
            sqlBuilder.Append("     DbProvince.ProvinceID       AS ProvinceId   ,");
            sqlBuilder.Append("     DbProvince.RegionID         AS RegionId     ,");
            sqlBuilder.Append("     DbProvince.Comment          AS Comment      ,");
            sqlBuilder.Append("     DbProvince.Active           AS Active       ,");
            sqlBuilder.Append("     DbProvinceLang.ProvinceName AS ProvinceName ,");
            sqlBuilder.Append("     DbRegionLang.RegionName     AS RegionName   ");
            sqlBuilder.Append(" FROM DbProvince ");
            sqlBuilder.Append("     LEFT JOIN DbProvinceLang ON ");
            sqlBuilder.Append("         DbProvince.ProvinceId             = DbProvinceLang.ProvinceId AND ");
            sqlBuilder.Append("         DbProvinceLang.LanguageID   = :LanguageId ");
            sqlBuilder.Append("     LEFT JOIN DbRegionLang ON ");
            sqlBuilder.Append("         DbProvince.RegionId             = DbRegionLang.RegionId AND ");
            sqlBuilder.Append("         DbRegionLang.LanguageID     = :LanguageId ");
            sqlBuilder.Append(" WHERE 1=1 ");

            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            parameterBuilder.AddParameterData("LanguageId", typeof(short), languageId);
            if (ProvincePrefix != null && ProvincePrefix != "")
            {
                sqlBuilder.Append(" AND DbProvinceLang.ProvinceName LIKE :ProvinceName ");
                parameterBuilder.AddParameterData("ProvinceName", typeof(string), ProvincePrefix + "%");
            }
            if (RegionID != null && RegionID != "")
            {
                sqlBuilder.Append(" AND DbProvince.RegionID = :RegionId ");
                parameterBuilder.AddParameterData("RegionId", typeof(short), short.Parse(RegionID));
            }
            sqlBuilder.AppendLine(" ORDER BY DbProvinceLang.ProvinceName ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            parameterBuilder.FillParameters(query);

            query.AddScalar("ProvinceID"    , NHibernateUtil.Int16);
            query.AddScalar("RegionID"      , NHibernateUtil.Int16);
            query.AddScalar("ProvinceName"  , NHibernateUtil.String);
            query.AddScalar("RegionName"    , NHibernateUtil.String);

            return query.SetResultTransformer(Transformers.AliasToBean(typeof(DBProvinceLovReturn))).List<DBProvinceLovReturn>();
        }
    }
}
