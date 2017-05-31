using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Transform;
using SS.Standard.Data.NHibernate.Query;
using SS.Standard.Data.NHibernate.QueryDao;
using SS.Standard.Data.NHibernate.QueryCreator;
using SCG.DB.DTO;
using SCG.DB.Query;
using SCG.DB.DTO.ValueObject;

namespace SCG.DB.Query.Hibernate
{
    public class DbWithHoldingTaxTypeQuery : NHibernateQueryBase<DbWithHoldingTaxType, long>, IDbWithHoldingTaxTypeQuery
    {
        public bool isDuplicationWHTTypeCode(string WHTCode)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" SELECT ");
            sqlBuilder.Append("     WHTTypeID       AS WhtTypeID    ,");
            sqlBuilder.Append("     WHTTypeCode     AS WhtTypeCode  ,");
            sqlBuilder.Append("     WHTTypeName     AS WhtTypeName  ,");
            sqlBuilder.Append("     Active          AS Active      , ");
            sqlBuilder.Append("     IsPeople        AS IsPeople       ");

            sqlBuilder.Append(" FROM DbWithHoldingTaxType ");
            sqlBuilder.Append(" WHERE WHTTypeCode = :WHTTypeCode ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.AddScalar("WhtTypeID", NHibernateUtil.Int32);
            query.AddScalar("WhtTypeCode", NHibernateUtil.String);
            query.AddScalar("WhtTypeName", NHibernateUtil.String);
            query.AddScalar("Active", NHibernateUtil.Boolean);
            query.AddScalar("IsPeople", NHibernateUtil.Boolean);

            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            parameterBuilder.AddParameterData("WHTTypeCode", typeof(string), WHTCode);
            parameterBuilder.FillParameters(query);

            IList<DbWithHoldingTaxType> DbWHTType = query.SetResultTransformer(Transformers.AliasToBean(typeof(DbWithHoldingTaxType))).List<DbWithHoldingTaxType>();

            if (DbWHTType.Count > 0)
                return true;
            else
                return false;
        }

        public ISQLQuery FindWithHoldingTaxTypeByCriteria(DbWithHoldingTaxType WHTTaxType, bool isCount, short languageId, string sortExpression, string WHTCode, string WHTName)
        {
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            StringBuilder sqlBuilder = new StringBuilder();
            if (!isCount)
            {
                sqlBuilder.Append(" SELECT ");
                sqlBuilder.Append("     WHTTypeID       AS WhtTypeID    ,");
                sqlBuilder.Append("     WHTTypeCode     AS WhtTypeCode  ,");
                sqlBuilder.Append("     WHTTypeName     AS WhtTypeName  ,");
                sqlBuilder.Append("     IsPeople        AS IsPeople     ,");
                sqlBuilder.Append("     Active          AS Active       ");
                sqlBuilder.Append(" FROM DbWithHoldingTaxType ");
                sqlBuilder.Append(" WHERE 1=1 ");

                if (WHTCode != null && WHTCode != "")
                {
                    sqlBuilder.Append(" AND WhtTypeCode LIKE :WHTTypeCode ");
                    parameterBuilder.AddParameterData("WHTTypeCode", typeof(string), "%" + WHTCode + "%");
                }
                if (WHTName != null && WHTName != "")
                {
                    sqlBuilder.Append(" AND WHTTypeName LIKE :WHTTypeName ");
                    parameterBuilder.AddParameterData("WHTTypeName", typeof(string), "%" + WHTName + "%");
                }

                if (string.IsNullOrEmpty(sortExpression))
                    sqlBuilder.AppendLine(" ORDER BY WhtTypeCode,WhtTypeName,Active,IsPeople  ");
                else
                    sqlBuilder.AppendLine(string.Format(" ORDER BY {0}", sortExpression));

            }
            else
            {
                sqlBuilder.Append(" SELECT COUNT(WhtTypeCode) AS WHTTypeCodeCount ");
                sqlBuilder.Append(" FROM DbWithHoldingTaxType ");
                sqlBuilder.Append(" WHERE 1=1 ");

                if (WHTCode != null && WHTCode != "")
                {
                    sqlBuilder.Append(" AND WhtTypeCode LIKE :WHTTypeCode ");
                    parameterBuilder.AddParameterData("WHTTypeCode", typeof(string), "%" + WHTCode + "%");
                }
                if (WHTName != null && WHTName != "")
                {
                    sqlBuilder.Append(" AND WhtTypeName LIKE :WHTTypeName ");
                    parameterBuilder.AddParameterData("WHTTypeName", typeof(string), "%" + WHTName + "%");
                }
            }


            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            parameterBuilder.FillParameters(query);

            if (!isCount)
            {
                query.AddScalar("WhtTypeID", NHibernateUtil.Int32);
                query.AddScalar("WhtTypeCode", NHibernateUtil.String);
                query.AddScalar("WhtTypeName", NHibernateUtil.String);
                query.AddScalar("IsPeople", NHibernateUtil.Boolean);
                query.AddScalar("Active", NHibernateUtil.Boolean);
                query.AddScalar("IsPeople", NHibernateUtil.Boolean);

                query.SetResultTransformer(Transformers.AliasToBean(typeof(DbWithHoldingTaxType)));
            }
            else
            {
                query.AddScalar("WHTTypeCodeCount", NHibernateUtil.Int32);
                query.UniqueResult();
            }

            return query;
        }

        #region public IList<DbWithHoldingTaxType> GetWithHoldingTaxTypeList(DbWithHoldingTaxType WHTTaxType, short languageId, int firstResult, int maxResult, string sortExpression, string strWHTTypeCpde, string strWHTTypeName)
        public IList<DbWithHoldingTaxType> GetWithHoldingTaxTypeList(DbWithHoldingTaxType WHTTaxType, short languageId, int firstResult, int maxResult, string sortExpression, string strWHTTypeCode, string strWHTTypeName)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<DbWithHoldingTaxType>(ScgDbQueryProvider.DbWithHoldingTaxTypeQuery, "FindWithHoldingTaxTypeByCriteria", new object[] { WHTTaxType, false, languageId, sortExpression, strWHTTypeCode, strWHTTypeName }, firstResult, maxResult, sortExpression);
        }
        #endregion public IList<DbWithHoldingTaxType> GetWithHoldingTaxTypeList(DbWithHoldingTaxType WHTTaxType, short languageId, int firstResult, int maxResult, string sortExpression, string strWHTTypeCpde, string strWHTTypeName)

        #region public int CountWithHoldingTaxTypeByCriteria(DbWithHoldingTaxType WHTTaxType)
        public int CountWithHoldingTaxTypeByCriteria(DbWithHoldingTaxType WHTTaxType, string strWHTTypeCode, string strWHTTypeName)
        {
            return NHibernateQueryHelper.CountByCriteria(ScgDbQueryProvider.DbWithHoldingTaxTypeQuery, "FindWithHoldingTaxTypeByCriteria", new object[] { WHTTaxType, true, Convert.ToInt16(0), string.Empty, strWHTTypeCode, strWHTTypeName });
        }
        #endregion public int CountWithHoldingTaxTypeByCriteria(DbWithHoldingTaxType WHTTaxType)

        public IList<VOWHTType> FindAllWHTTypeActive()
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" select t.WhtTypeID as ID, t.WhtTypeCode as Code, t.WhtTypeName as Name, t.isPeople as IsPeople ");
            sqlBuilder.Append(" from DbWithHoldingTaxType t where t.Active = 1 ");

            return GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString())
                .AddScalar("ID", NHibernateUtil.Int64)
                .AddScalar("Code", NHibernateUtil.String)
                .AddScalar("Name", NHibernateUtil.String)
                .AddScalar("IsPeople", NHibernateUtil.Boolean)
                .SetResultTransformer(Transformers.AliasToBean(typeof(VOWHTType)))
                .List<VOWHTType>();
        }
    }
}
