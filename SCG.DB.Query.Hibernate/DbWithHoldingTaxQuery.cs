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
    public class DbWithHoldingTaxQuery : NHibernateQueryBase<DbWithHoldingTax, long>, IDbWithHoldingTaxQuery
    {
        #region public bool isDuplicationWHTCode(string WHTCode)
        public bool isDuplicationWHTCode(string WHTCode)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" SELECT ");
            sqlBuilder.Append("     WHTID           AS Whtid    ,");
            sqlBuilder.Append("     WHTCode         AS WhtCode  ,");
            sqlBuilder.Append("     WHTName         AS WhtName  ,");
            sqlBuilder.Append("     Rate            AS Rate     ,");
            sqlBuilder.Append("     Active          AS Active    ");

            sqlBuilder.Append(" FROM DbWithHoldingTax ");
            sqlBuilder.Append(" WHERE WHTCode = :WHTCode ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.AddScalar("Whtid", NHibernateUtil.Int32);
            query.AddScalar("WhtCode", NHibernateUtil.String);
            query.AddScalar("WhtName", NHibernateUtil.String);
            query.AddScalar("Rate", NHibernateUtil.Double);
            query.AddScalar("Active", NHibernateUtil.Boolean);

            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            parameterBuilder.AddParameterData("WHTCode", typeof(string), WHTCode);
            parameterBuilder.FillParameters(query);

            IList<DbWithHoldingTax> DbWHT = query.SetResultTransformer(Transformers.AliasToBean(typeof(DbWithHoldingTax))).List<DbWithHoldingTax>();

            if (DbWHT.Count > 0)
                return true;
            else
                return false;
        }
        #endregion public bool isDuplicationWHTCode(string WHTCode)

        #region public ISQLQuery FindWithHoldingTaxByCriteria(DbWithHoldingTax WHTTax, bool isCount, short languageId, string sortExpression, string WHTCode, string WHTName)
        public ISQLQuery FindWithHoldingTaxByCriteria(DbWithHoldingTax WHTTax, bool isCount, short languageId, string sortExpression, string WHTCode, string WHTName)
        {
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            StringBuilder sqlBuilder = new StringBuilder();
            if (!isCount)
            {
                sqlBuilder.Append(" SELECT ");
                sqlBuilder.Append("     WHTID           AS Whtid    ,");
                sqlBuilder.Append("     WHTCode         AS WhtCode  ,");
                sqlBuilder.Append("     WHTName         AS WhtName  ,");
                sqlBuilder.Append("     Rate            AS Rate     ,");
                sqlBuilder.Append("     Active          AS Active   ,");
                sqlBuilder.Append("     Seq             AS Seq       ");

                sqlBuilder.Append(" FROM DbWithHoldingTax ");
                sqlBuilder.Append(" WHERE 1=1 ");

                if (WHTCode != null && WHTCode != "")
                {
                    sqlBuilder.Append(" AND WHTCode LIKE :WHTCode ");
                    parameterBuilder.AddParameterData("WHTCode", typeof(string), "%" + WHTCode + "%");
                }
                if (WHTName != null && WHTName != "")
                {
                    sqlBuilder.Append(" AND WHTName LIKE :WHTName ");
                    parameterBuilder.AddParameterData("WHTName", typeof(string), "%" + WHTName + "%");
                }

                if (string.IsNullOrEmpty(sortExpression))
                    sqlBuilder.AppendLine(" ORDER BY WHTID,WHTCode,Rate,Active ");
                else
                    sqlBuilder.AppendLine(string.Format(" ORDER BY {0}", sortExpression));

            }
            else
            {
                sqlBuilder.Append(" SELECT COUNT(WHTCode) AS WHTCodeCount ");
                sqlBuilder.Append(" FROM DbWithHoldingTax ");
                sqlBuilder.Append(" WHERE 1=1 ");

                if (WHTCode != null && WHTCode != "")
                {
                    sqlBuilder.Append(" AND WHTCode LIKE :WHTCode ");
                    parameterBuilder.AddParameterData("WHTCode", typeof(string), "%" + WHTCode + "%");
                }
                if (WHTName != null && WHTName != "")
                {
                    sqlBuilder.Append(" AND WHTName LIKE :WHTName ");
                    parameterBuilder.AddParameterData("WHTName", typeof(string), "%" + WHTName + "%");
                }
            }


            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            parameterBuilder.FillParameters(query);

            if (!isCount)
            {
                query.AddScalar("Whtid", NHibernateUtil.Int64);
                query.AddScalar("WhtCode", NHibernateUtil.String);
                query.AddScalar("WhtName", NHibernateUtil.String);
                query.AddScalar("Rate", NHibernateUtil.Double);
                query.AddScalar("Active", NHibernateUtil.Boolean);
                query.AddScalar("Seq", NHibernateUtil.Int32);

                query.SetResultTransformer(Transformers.AliasToBean(typeof(DbWithHoldingTax)));
            }
            else
            {
                query.AddScalar("WHTCodeCount", NHibernateUtil.Int32);
                query.UniqueResult();
            }

            return query;
        }
        #endregion public ISQLQuery FindWithHoldingTaxByCriteria(DbWithHoldingTax WHTTax, bool isCount, short languageId, string sortExpression, string WHTCode, string WHTName)

        #region public IList<DbWithHoldingTax> GetWithHoldingTaxList(DbWithHoldingTax WHTTax, short languageId, int firstResult, int maxResult, string sortExpression, string strWHTCode, string strWHTName)
        public IList<DbWithHoldingTax> GetWithHoldingTaxList(DbWithHoldingTax WHTTax, short languageId, int firstResult, int maxResult, string sortExpression, string strWHTCode, string strWHTName)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<DbWithHoldingTax>(ScgDbQueryProvider.DbWithHoldingTaxQuery, "FindWithHoldingTaxByCriteria", new object[] { WHTTax, false, languageId, sortExpression, strWHTCode, strWHTName }, firstResult, maxResult, sortExpression);
        }
        #endregion public IList<DbWithHoldingTax> GetWithHoldingTaxList(DbWithHoldingTax WHTTax, short languageId, int firstResult, int maxResult, string sortExpression, string strWHTCode, string strWHTName)

        #region public int CountWithHoldingTaxByCriteria(DbWithHoldingTax WHTTax, string strWHTCode, string strWHTName)
        public int CountWithHoldingTaxByCriteria(DbWithHoldingTax WHTTax, string strWHTCode, string strWHTName)
        {
            return NHibernateQueryHelper.CountByCriteria(ScgDbQueryProvider.DbWithHoldingTaxQuery, "FindWithHoldingTaxByCriteria", new object[] { WHTTax, true, Convert.ToInt16(0), string.Empty, strWHTCode, strWHTName });
        }
        #endregion public int CountWithHoldingTaxByCriteria(DbWithHoldingTax WHTTax, string strWHTCode, string strWHTName)

        public IList<VOWHTRate> FindAllWHTRateActive()
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" select t.WhtID as ID, t.Rate as Rate, t.WhtName as Description ");
            sqlBuilder.Append(" from DbWithHoldingTax t where t.Active = 1  Order by Seq ASC");
            return GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString())
                .AddScalar("ID", NHibernateUtil.Int64)
                .AddScalar("Rate", NHibernateUtil.Double)
                .AddScalar("Description", NHibernateUtil.String)
                .SetResultTransformer(Transformers.AliasToBean(typeof(VOWHTRate)))
                .List<VOWHTRate>();
        }

        public DbWithHoldingTax FindWithHoldingTaxByWhtCode(string WHTCode)
        {
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            string sql = "SELECT  Whtid, WhtCode, WhtName, Rate, Active FROM DbWithHoldingTax WHERE WHTCode =:WHTCode";
            parameterBuilder.AddParameterData("WHTCode", typeof(string), WHTCode);

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql);
            parameterBuilder.FillParameters(query);
            query.AddScalar("Whtid", NHibernateUtil.Int64);
            query.AddScalar("WhtCode", NHibernateUtil.String);
            query.AddScalar("WhtName", NHibernateUtil.String);
            query.AddScalar("Rate", NHibernateUtil.Double);
            query.AddScalar("Active", NHibernateUtil.Boolean);
            query.SetResultTransformer(Transformers.AliasToBean(typeof(DbWithHoldingTax)));

            return query.UniqueResult<DbWithHoldingTax>();
        }
        public string GetWHTCodeExpMapping(string WHTCode)
        {
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            string sql = "SELECT TOP 1 WHTCodeExp FROM DbWHTMapping WHERE WHTCodeEcc = :WHTCode ";
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql);
            parameterBuilder.AddParameterData("WHTCode", typeof(string), WHTCode);
            parameterBuilder.FillParameters(query);
            query.AddScalar("WHTCodeExp", NHibernateUtil.String);
            return query.UniqueResult<string>();
        }
    }
}
