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
    public class DbCostCenterQuery : NHibernateQueryBase<DbCostCenter, long>, IDbCostCenterQuery
    {
        #region public ISQLQuery FindByCostCenterCriteria(string costCenterCode, string description, bool isCount, string sortExpression)
        public ISQLQuery FindByCostCenterCriteria(string costCenterCode, string description,long companyID, bool isCount, string sortExpression)
        {
            costCenterCode  = "%" + costCenterCode + "%";
            description     = "%" + description + "%";
            StringBuilder sqlBuilder = new StringBuilder();
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();

            if (!isCount)
            {
                sqlBuilder.Append(" SELECT CostCenterID,CompanyID,CostCenterCode,Description");
                sqlBuilder.Append(" FROM DbCostCenter ");
                sqlBuilder.Append(" WHERE Active = 1 ");
                //sqlBuilder.Append(" AND Description like :description");
                //sqlBuilder.Append(" And Active = 1 ");
                //if (companyID != null && companyID > 0)
                //{
                //    sqlBuilder.Append(" And CompanyID = :companyID ");
                //}
                //if (string.IsNullOrEmpty(sortExpression))
                //    sqlBuilder.AppendLine(" ORDER BY CostCenterCode");
                //else
                //    sqlBuilder.AppendLine(string.Format(" ORDER BY {0}", sortExpression));

            }
            else
            {
                sqlBuilder.Append(" SELECT COUNT(CostCenterID) AS CostCenterCount FROM DbCostCenter WHERE Active = 1 ");

            }

            sqlBuilder.Append(" AND CostCenterCode  like :costCenterCode ");
            queryParameterBuilder.AddParameterData("costCenterCode", typeof(string), costCenterCode);

            sqlBuilder.Append(" AND Description     like :description");
            queryParameterBuilder.AddParameterData("description", typeof(string), description);

            if (companyID > 0)
            {
                sqlBuilder.Append(" And CompanyID = :companyID ");
                queryParameterBuilder.AddParameterData("companyID", typeof(long), companyID);
            }

            if (!isCount)
            {
                if (string.IsNullOrEmpty(sortExpression))
                    sqlBuilder.AppendLine(" ORDER BY CostCenterCode");
                else
                    sqlBuilder.AppendLine(string.Format(" ORDER BY {0}", sortExpression));
            }

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            queryParameterBuilder.FillParameters(query);

            if (!isCount)
            {
                queryParameterBuilder.FillParameters(query);
                query.AddScalar("CostCenterID",     NHibernateUtil.Int64);
                //query.AddScalar("CompanyID",        NHibernateUtil.Int64);
                query.AddScalar("CostCenterCode",   NHibernateUtil.String);
                query.AddScalar("Description",      NHibernateUtil.String);

                query.SetResultTransformer(Transformers.AliasToBean(typeof(DbCostCenter)));
            }
            else
            {
                query.AddScalar("CostCenterCount", NHibernateUtil.Int32);
                query.UniqueResult();
            }
            return query;
        }

        public IList<DbCostCenter> GetCostCenterList(string costCenterCode, string description, long companyID, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<DbCostCenter>(ScgDbQueryProvider.DbCostCenterQuery, "FindByCostCenterCriteria", new object[] { costCenterCode, description, companyID, false, sortExpression }, firstResult, maxResult, sortExpression);
        }

        public int CountByCostCenterCriteria(string costCenterCode, string description, long companyID)
        {
            return NHibernateQueryHelper.CountByCriteria(ScgDbQueryProvider.DbCostCenterQuery, "FindByCostCenterCriteria", new object[] { costCenterCode, description, companyID, true, string.Empty });
        }
        #endregion

        #region IList<DbCostCenter> FindByDbCostCenter(long costCenterID, long companyID)
        public IList<DbCostCenter> FindByDbCostCenter(long costCenterID, long companyID)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" SELECT CostCenterID,CompanyID,CostCenterCode,Description");
            sqlBuilder.Append(" FROM DbCostCenter ");
            sqlBuilder.Append(" WHERE  (CostCenterID = :costCenterID) ");
            sqlBuilder.Append(" And Active ='1' ");
            if (companyID > 0)
            {
                sqlBuilder.Append(" And CompanyID = :companyID ");
            }
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());

            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("costCenterID", typeof(string), costCenterID);
            if ( companyID > 0)
                queryParameterBuilder.AddParameterData("companyID", typeof(long), companyID);
            queryParameterBuilder.FillParameters(query);
            query.AddScalar("CostCenterID", NHibernateUtil.Int64);
            //query.AddScalar("CompanyID", NHibernateUtil.Int64);
            query.AddScalar("CostCenterCode", NHibernateUtil.String);
            query.AddScalar("Description", NHibernateUtil.String);

            return query.SetResultTransformer(Transformers.AliasToBean(typeof(DbCostCenter))).List<DbCostCenter>();
        }
        #endregion

        #region IList<AutocompleteField> FindByDbCostCenterAutoComplete(string costCenterCode, long companyID)
        /// <summary>
        /// query for auto complete
        /// </summary>
        /// <param name="taxNo"></param>
        /// <returns></returns>
        public IList<AutocompleteField> FindByDbCostCenterAutoComplete(string costCenterCode, long companyID)
        {
            costCenterCode = "%" + costCenterCode + "%";
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" SELECT CostCenterID as ID, CostCenterCode as Code, Description as Description");
            sqlBuilder.Append(" FROM DbCostCenter ");
            sqlBuilder.Append(" WHERE CostCenterCode LIKE :costCenterCode");
            sqlBuilder.Append(" AND Active = '1'");
            if (companyID > 0)
            {
                sqlBuilder.Append(" And CompanyID = :companyID ");
            }
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());

            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("costCenterCode", typeof(string), costCenterCode);
            if (companyID > 0)
            {
                queryParameterBuilder.AddParameterData("companyID", typeof(long), companyID);
            }
            queryParameterBuilder.FillParameters(query);
            query.AddScalar("ID", NHibernateUtil.Int64);
            query.AddScalar("Code", NHibernateUtil.String);
            query.AddScalar("Description", NHibernateUtil.String);

            return query.SetResultTransformer(Transformers.AliasToBean(typeof(AutocompleteField))).List<AutocompleteField>();
        }
        #endregion


        public DbCostCenter getDbCostCenterByCostCenterCode(string costCenterCode)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT CostCenterID From DbCostCenter WHERE Active = 1 and CostCenterCode = :costcode");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            parameterBuilder.AddParameterData("costcode", typeof(string), costCenterCode);
            parameterBuilder.FillParameters(query);
            query.AddScalar("CostCenterID", NHibernateUtil.Int64);
            IList<DbCostCenter> list = query.SetResultTransformer(Transformers.AliasToBean(typeof(DbCostCenter))).List<DbCostCenter>();
            if (list.Count <= 0)
                return null;
            else
                return list.ElementAt<DbCostCenter>(0);
        }
        

        public ISQLQuery FindCostCenterCompanyByCriteria(DbCostCenter costCenter, bool IsCount,string sortExpression )
        {
            StringBuilder sqlBuilder = new StringBuilder();
            QueryParameterBuilder paramBuilder = new QueryParameterBuilder();
            if (!IsCount)
            {
                sqlBuilder.AppendLine(" SELECT cstcntr.CostCenterID as CostCenterID, ");
                sqlBuilder.AppendLine("cstcntr.CostCenterCode as CostCenterCode, ");
                sqlBuilder.AppendLine("cstcntr.Description as Description, ");
                sqlBuilder.AppendLine("cstcntr.Valid as Valid,");
                sqlBuilder.AppendLine("cstcntr.Expire as Expire,");
                sqlBuilder.AppendLine("cstcntr.ActualPrimaryCosts as ActualPrimaryCosts,");
                sqlBuilder.AppendLine("cstcntr.Active as Active,");
                sqlBuilder.AppendLine("cmpn.CompanyCode as CompanyCode ");
                sqlBuilder.AppendLine(" FROM dbCostCenter cstcntr ");
                sqlBuilder.AppendLine("LEFT JOIN dbCompany cmpn ");
                sqlBuilder.AppendLine("ON cstcntr.CompanyID = cmpn.CompanyID  ");
                sqlBuilder.AppendLine(" WHERE 1=1 ");

               }

            else
            {
                sqlBuilder.AppendLine("SELECT COUNT(1) AS CostCenterCount");
                sqlBuilder.AppendLine("FROM dbCostCenter cstcntr ");
                sqlBuilder.AppendLine("LEFT JOIN dbCompany cmpn ");
                sqlBuilder.AppendLine("ON cstcntr.CompanyID = cmpn.CompanyID  ");
                sqlBuilder.AppendLine("WHERE 1=1 ");

            }

            if (!string.IsNullOrEmpty(costCenter.CostCenterCode))
            {
                sqlBuilder.AppendLine("AND cstcntr.CostCenterCode LIKE :CostCenterCode ");
                paramBuilder.AddParameterData("CostCenterCode", typeof(string), string.Format("%{0}%", costCenter.CostCenterCode));
            }
            if (!string.IsNullOrEmpty(costCenter.Description))
            {
                sqlBuilder.AppendLine("AND cstcntr.Description LIKE :Description ");
                paramBuilder.AddParameterData("Description", typeof(string), string.Format("%{0}%", costCenter.Description));
            }
            if (!IsCount)
            {
                //Incomplete with unresolvable bug.
                if (!string.IsNullOrEmpty(sortExpression))
                    sqlBuilder.AppendLine(string.Format(" ORDER BY {0} ", sortExpression));
                else
                    sqlBuilder.AppendLine(" ORDER BY CostCenterID,CostCenterCode,Description ");
                    

            }
  
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            paramBuilder.FillParameters(query);
            if (!IsCount)
            {
                query.AddScalar("CostCenterID", NHibernateUtil.Int64)
                    .AddScalar("CostCenterCode", NHibernateUtil.String)
                    .AddScalar("Description", NHibernateUtil.String)
                    .AddScalar("Valid", NHibernateUtil.DateTime)
                    .AddScalar("Expire", NHibernateUtil.DateTime)
                    .AddScalar("ActualPrimaryCosts", NHibernateUtil.Boolean)
                    .AddScalar("Active", NHibernateUtil.Boolean)
                    .AddScalar("CompanyCode", NHibernateUtil.String)
                    ;
                query.SetResultTransformer(Transformers.AliasToBean(typeof(CostCenterCompany)));
            }
            else
            {
                query.AddScalar("CostCenterCount", NHibernateUtil.Int32);
                query.UniqueResult();
            }
            return query;
        }

        public IList<CostCenterCompany> FindCostCenterCompany(DbCostCenter costCenter, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<CostCenterCompany>(ScgDbQueryProvider.DbCostCenterQuery, "FindCostCenterCompanyByCriteria", new object[] { costCenter, false, sortExpression }, firstResult, maxResult, sortExpression);
        }

        public int CountCostCenterCompany(DbCostCenter costCenter)
        {
            return NHibernateQueryHelper.CountByCriteria(ScgDbQueryProvider.DbCostCenterQuery, "FindCostCenterCompanyByCriteria", new object[] { costCenter, true, string.Empty });
        }

        public DbCostCenter getCostCenterCodeByCostCenterID(long CostCenterID)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(" Select substring(b.CostCenterCode,4,1) as CostCenterCode ");
            sql.AppendLine(" From SuUser a inner join DbCostCenter b ");
            sql.AppendLine(" on a.CompanyID = b.CompanyID ");
            sql.AppendLine(" Where b.CostCenterID = :CostCenterID ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();

            parameterBuilder.AddParameterData("CostCenterID", typeof(long), CostCenterID);
            parameterBuilder.FillParameters(query);

            query.AddScalar("CostCenterCode", NHibernateUtil.String);

            IList<DbCostCenter> list = query.SetResultTransformer(Transformers.AliasToBean(typeof(DbCostCenter))).List<DbCostCenter>();
            if (list.Count <= 0)
                return null;
            else
                return list.ElementAt<DbCostCenter>(0);            
        }

        public bool IsDuplicateCostCenterCode(DbCostCenter costCenter)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("select count(1) as CostCenterCount ");
            sqlBuilder.Append("from dbcostcenter ");
            sqlBuilder.Append("where costcentercode = :costcentercode ");
            sqlBuilder.Append("and costcenterid <> :costcenterid ");
            QueryParameterBuilder param = new QueryParameterBuilder();
            param.AddParameterData("costcentercode", typeof(string), costCenter.CostCenterCode);
            param.AddParameterData("costcenterid", typeof(long), costCenter.CostCenterID);
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            param.FillParameters(query);
            query.AddScalar("CostCenterCount", NHibernateUtil.Int32);
            int i = (int)query.UniqueResult();
            if (i > 0)
            {
                return true;
            }
            return false;
        }

        public IList<DbCostCenter> FindCostCenterByCompanyID(long comId)
        {
            return GetCurrentSession().CreateQuery(" from DbCostCenter where Active = 1 and CompanyID = :CompanyID")
                .SetInt64("CompanyID", comId)
                .List<DbCostCenter>();
        }

		public IList<DbCostCenter> FindByCostCenterIDList(IList<long> costCenterIDList)
		{
			return GetCurrentSession().CreateQuery(" FROM DbCostCenter WHERE CostCenterID in :costCenterIDList ")
					.SetParameterList("costCenterIDList", costCenterIDList)
					.List<DbCostCenter>();
		}
	}
}
