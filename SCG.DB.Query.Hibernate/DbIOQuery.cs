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
    public class DbIOQuery : NHibernateQueryBase<DbInternalOrder, long>, IDbIOQuery
    {
        #region Search ByCriteria
        public ISQLQuery FindDataByIOCriteria(DbInternalOrder io, bool isCount, string sortExpression)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            if (!isCount)
            {
                sqlBuilder.Append(" SELECT  io.IOID as IOID , io.IONumber as IONumber, io.IOType as IOType, io.IOText as IOText, ");
                sqlBuilder.Append(" io.CostCenterID as CostCenterID , io.CostCenterCode as CostCenterCode, io.CompanyID as CompanyID, ");
                sqlBuilder.Append(" io.CompanyCode as CompanyCode, io.EffectiveDate as EffectiveDate, io.ExpireDate as ExpireDate, ");
                sqlBuilder.Append(" io.Active as Active ");
            }
            else
            {
                sqlBuilder.Append(" select count(io.IOID) as Count ");
            }
            sqlBuilder.Append(" from DbInternalOrder io ");

            #region WhereClause
            StringBuilder whereClauseBuilder = new StringBuilder();
            if (!string.IsNullOrEmpty(io.IONumber))
            {
                whereClauseBuilder.Append(" and io.IONumber Like :IONumber ");
                queryParameterBuilder.AddParameterData("IONumber", typeof(string), String.Format("%{0}%", io.IONumber));
            }
            if (!string.IsNullOrEmpty(io.IOText))
            {
                whereClauseBuilder.Append(" and io.IOText Like :IOText ");
                queryParameterBuilder.AddParameterData("IOText", typeof(string), String.Format("%{0}%", io.IOText));
            }

            if (!string.IsNullOrEmpty(whereClauseBuilder.ToString()))
            {
                sqlBuilder.Append(String.Format(" where 1=1 {0} and io.Active = 1 ", whereClauseBuilder.ToString()));
            }
            #endregion
            #region Order By
            if (!isCount)
            {
                if (!string.IsNullOrEmpty(sortExpression))
                {
                    sqlBuilder.Append(String.Format(" order by {0} ", sortExpression));
                }
                else
                {
                    sqlBuilder.Append(" order by IOID, IONumber, IOText,IOType,CostCenterID,CostCenterCode, ");
                    sqlBuilder.Append(" CompanyID,CompanyCode,EffectiveDate,ExpireDate ");
                }
            }
            #endregion

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            queryParameterBuilder.FillParameters(query);

            if (isCount)
            {
                query.AddScalar("Count", NHibernateUtil.Int32);
            }
            else
            {

                query.AddScalar("IOID", NHibernateUtil.Int64)
                    .AddScalar("IONumber", NHibernateUtil.String)
                    .AddScalar("IOText", NHibernateUtil.String)
                    .AddScalar("IOType", NHibernateUtil.String)
                    .AddScalar("CostCenterID", NHibernateUtil.Int64)
                    .AddScalar("CostCenterCode", NHibernateUtil.String)
                    .AddScalar("CompanyID", NHibernateUtil.Int64)
                    .AddScalar("CompanyCode", NHibernateUtil.String)
                    .AddScalar("EffectiveDate", NHibernateUtil.DateTime)
                    .AddScalar("ExpireDate", NHibernateUtil.DateTime)
                    .AddScalar("Active", NHibernateUtil.Boolean);

                query.SetResultTransformer(Transformers.AliasToBean(typeof(DbInternalOrder)));
            }

            return query;
        }
        #endregion
        #region public ISQLQuery FindByCriteria(DbTax tax, bool isCount, string sortExpression)
        public ISQLQuery FindByIOCriteria(DbInternalOrder io, bool isCount, string sortExpression)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            if (!isCount)
            {
                sqlBuilder.Append(" select io.IOID as IOID, io.IONumber as IONumber, io.IOText as IOText ");
            }
            else
            {
                sqlBuilder.Append(" select count(io.IOID) as Count ");
            }
            sqlBuilder.Append(" from DbInternalOrder io ");

            #region WhereClause
            StringBuilder whereClauseBuilder = new StringBuilder();
            if (!string.IsNullOrEmpty(io.IONumber))
            {
                whereClauseBuilder.Append(" and io.IONumber Like :IONumber ");
                queryParameterBuilder.AddParameterData("IONumber", typeof(string), String.Format("%{0}%", io.IONumber));
            }
            if (!string.IsNullOrEmpty(io.IOText))
            {
                whereClauseBuilder.Append(" and io.IOText Like :IOText ");
                queryParameterBuilder.AddParameterData("IOText", typeof(string), String.Format("%{0}%", io.IOText));
            }
            //if (io.CompanyID.HasValue || (io.CompanyID.Value != 0))
            if (io.CompanyID.Value > 0)
            {
                //sqlBuilder.Append(" inner join DbCompany com on com.CompanyCode = io.CompanyCode ");
                whereClauseBuilder.Append(" and io.CompanyID = :CompanyID ");
                queryParameterBuilder.AddParameterData("CompanyID", typeof(long), io.CompanyID);
            }
            //if (!string.IsNullOrEmpty(io.CostCenterCode) || (io.CostCenterID != 0))
            if(io.CostCenterID != null)
            {
                //sqlBuilder.Append(" inner join DbCostCenter cost on cost.CostCenterCode = io.CostCenterCode ");
                whereClauseBuilder.Append(" and (io.CostCenterID = :CostCenterID or io.CostCenterID is null) ");
                queryParameterBuilder.AddParameterData("CostCenterID", typeof(long), io.CostCenterID);
            }

            if (!string.IsNullOrEmpty(whereClauseBuilder.ToString()))
            {
                sqlBuilder.Append(String.Format(" where 1=1 {0} and io.Active = 1 ", whereClauseBuilder.ToString()));
            }
            #endregion
            #region Order By
            if (!isCount)
            {
                if (!string.IsNullOrEmpty(sortExpression))
                {
                    sqlBuilder.Append(String.Format(" order by {0} ", sortExpression));
                }
                else
                {
                    sqlBuilder.Append(" order by IOID, IONumber, IOText ");
                }
            }
            #endregion

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            queryParameterBuilder.FillParameters(query);

            if (isCount)
            {
                query.AddScalar("Count", NHibernateUtil.Int32);
            }
            else
            {
                query.AddScalar("IOID", NHibernateUtil.Int64)
                    .AddScalar("IONumber", NHibernateUtil.String)
                    .AddScalar("IOText", NHibernateUtil.String);

                query.SetResultTransformer(Transformers.AliasToBean(typeof(DbInternalOrder)));
            }

            return query;
        }
        #endregion

        public IList<DbInternalOrder> GetIOList(DbInternalOrder io, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<DbInternalOrder>(ScgDbQueryProvider.DbIOQuery, "FindByIOCriteria", new object[] { io, false, sortExpression }, firstResult, maxResult, sortExpression);
        }
        public IList<DbInternalOrder> GetInternalOrderList(DbInternalOrder io, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<DbInternalOrder>(ScgDbQueryProvider.DbIOQuery, "FindDataByIOCriteria", new object[] { io, false, sortExpression }, firstResult, maxResult, sortExpression);
        }

        public int CountByIOCriteria(DbInternalOrder io)
        {
            return NHibernateQueryHelper.CountByCriteria(ScgDbQueryProvider.DbIOQuery, "FindByIOCriteria", new object[] { io, true, string.Empty });
        }
        public int CountDataByIOCriteria(DbInternalOrder io)
        {
            return NHibernateQueryHelper.CountByCriteria(ScgDbQueryProvider.DbIOQuery, "FindDataByIOCriteria", new object[] { io, true, string.Empty });
        }

        public IList<DbInternalOrder> FindAutoComplete(string prefixText, IOAutoCompleteParameter param)
        {
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" select io.IOID as IOID, io.IONumber as IONumber, io.IOText as IOText ");
            sqlBuilder.Append(" from DbInternalOrder io ");

            StringBuilder whereClauseBuilder = new StringBuilder();
            whereClauseBuilder.Append(" and io.Active = 1 ");
            if (!string.IsNullOrEmpty(prefixText))
            {
                whereClauseBuilder.Append(" and ((io.IONumber Like :IO) or (io.IOText Like :IO)) ");
                queryParameterBuilder.AddParameterData("IO", typeof(string), String.Format("{0}%", prefixText));
            }
            if (param.CompanyId.HasValue && param.CompanyId.Value != 0)
            {
                //sqlBuilder.Append(" inner join DbCompany com on com.CompanyCode = io.CompanyCode ");
                whereClauseBuilder.Append(" and io.CompanyID = :CompanyID ");
                queryParameterBuilder.AddParameterData("CompanyID", typeof(long), param.CompanyId.Value);
            }
            if (param.CostCenterId.HasValue && param.CostCenterId.Value != 0)
            {
                //sqlBuilder.Append(" inner join DbCostCenter cost on cost.CostCenterCode = io.CostCenterCode ");
                whereClauseBuilder.Append(" and (io.CostCenterID = :CostCenterID or io.CostCenterID is null) ");
                queryParameterBuilder.AddParameterData("CostCenterID", typeof(long), param.CostCenterId);
            }

            if (!string.IsNullOrEmpty(whereClauseBuilder.ToString()))
            {
                sqlBuilder.Append(String.Format(" where 1=1 {0} ", whereClauseBuilder.ToString()));
            }

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            queryParameterBuilder.FillParameters(query);

            query.AddScalar("IOID", NHibernateUtil.Int64)
                .AddScalar("IONumber", NHibernateUtil.String)
                .AddScalar("IOText", NHibernateUtil.String);

            query.SetResultTransformer(Transformers.AliasToBean(typeof(DbInternalOrder)));
            IList<DbInternalOrder> list = query.List<DbInternalOrder>();

            return list;
        }

        public DbInternalOrder getDbInternalOrderByIONumber(string ioNumber)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT IOID From DbInternalOrder WHERE IONumber = :ioNumber");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            parameterBuilder.AddParameterData("ioNumber", typeof(string), ioNumber);
            parameterBuilder.FillParameters(query);
            query.AddScalar("IOID", NHibernateUtil.Int64);
            IList<DbInternalOrder> list = query.SetResultTransformer(Transformers.AliasToBean(typeof(DbInternalOrder))).List<DbInternalOrder>();
            if (list.Count <= 0)
                return null;
            else
                return list.ElementAt<DbInternalOrder>(0);
        }

        public IList<DbInternalOrder> FindIOByCompanyID(long companyId)
        {
            return GetCurrentSession().CreateQuery(" from DbInternalOrder where Active =1 and CompanyID = :CompanyID ")
                .SetInt64("CompanyID", companyId)
                .List<DbInternalOrder>();
        }
    }
}
