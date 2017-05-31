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
    public class DbVendorQuery : NHibernateQueryBase<DbVendor, long>, IDbVendorQuery
    {
        #region public ISQLQuery FindByVendorCriteria(string taxNo, string vendorCode,string vendorName, bool isCount , string sortExpression)
        public ISQLQuery FindByVendorCriteria(string taxNo, string vendorCode, string vendorName, bool isCount, string sortExpression)
        {
            taxNo = "%" + taxNo + "%";
            vendorCode = "%" + vendorCode + "%";
            vendorName = "%" + vendorName + "%";
            StringBuilder sqlBuilder = new StringBuilder();
            if (!isCount)
            {
                sqlBuilder.Append(" SELECT VendorID,VendorCode,VendorTitle,VendorName1,VendorName2,TaxNo1,TaxNo2,Street, City,Country, PostalCode");
                sqlBuilder.Append(" FROM DbVendor ");
                sqlBuilder.Append(" WHERE  (VendorCode like :vendorCode) ");
                sqlBuilder.Append(" AND ((VendorName1 like :vendorName) or (VendorName2 like :vendorName))");
                sqlBuilder.Append(" AND ((ISNULL(TaxNo1,'') like :taxNo) or (ISNULL(TaxNo2,'') like :taxNo))");
                sqlBuilder.Append(" AND (TaxNo1 is not null or TaxNo2 is not null) ");
                if (string.IsNullOrEmpty(sortExpression))
                    sqlBuilder.AppendLine(" ORDER BY VendorCode,VendorTitle,VendorName1,VendorName2");
                else
                    sqlBuilder.AppendLine(string.Format(" ORDER BY {0}", sortExpression));

            }
            else
            {
                sqlBuilder.Append(" SELECT COUNT(VendorID) AS VendorCount FROM DbVendor ");
            }

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());

            if (!isCount)
            {
                QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
                queryParameterBuilder.AddParameterData("taxNo", typeof(string), taxNo);
                queryParameterBuilder.AddParameterData("vendorCode", typeof(string), vendorCode);
                queryParameterBuilder.AddParameterData("vendorName", typeof(string), vendorName);
                queryParameterBuilder.FillParameters(query);
                query.AddScalar("VendorID", NHibernateUtil.Int64);
                query.AddScalar("VendorCode", NHibernateUtil.String);
                query.AddScalar("VendorTitle", NHibernateUtil.String);
                query.AddScalar("VendorName1", NHibernateUtil.String);
                query.AddScalar("VendorName2", NHibernateUtil.String);
                query.AddScalar("TaxNo1", NHibernateUtil.String);
                query.AddScalar("TaxNo2", NHibernateUtil.String);
                query.AddScalar("Street", NHibernateUtil.String);
                query.AddScalar("City", NHibernateUtil.String);
                query.AddScalar("Country", NHibernateUtil.String);
                query.AddScalar("PostalCode", NHibernateUtil.String);

                query.SetResultTransformer(Transformers.AliasToBean(typeof(DbVendor)));
            }
            else
            {
                query.AddScalar("VendorCount", NHibernateUtil.Int32);
                query.UniqueResult();
            }
            return query;
        }

        public IList<DbVendor> GetVendorList(string taxNo, string vendorCode, string vendorName, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<DbVendor>(ScgDbQueryProvider.DbVendorQuery, "FindByVendorCriteria", new object[] { taxNo, vendorCode, vendorName, false, sortExpression }, firstResult, maxResult, sortExpression);
        }

        public int CountByCountryCriteria(string taxNo, string vendorCode, string vendorName)
        {
            return NHibernateQueryHelper.CountByCriteria(ScgDbQueryProvider.DbVendorQuery, "FindByVendorCriteria", new object[] { taxNo, vendorCode, vendorName, true, string.Empty });
        }
        #endregion

        #region IList<DbVendor> FindByDbVendor(long vendorID)
        public IList<DbVendor> FindByDbVendor(long vendorID)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" SELECT VendorID,VendorCode,VendorTitle,VendorName1,VendorName2,TaxNo1,TaxNo2,Street, City,Country, PostalCode");
            sqlBuilder.Append(" FROM DbVendor ");
            sqlBuilder.Append(" WHERE  (VendorID = :vendorID) ");
            sqlBuilder.Append(" AND (TaxNo1 is not null or TaxNo2 is not null)");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());

            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("vendorID", typeof(string), vendorID);
            queryParameterBuilder.FillParameters(query);
            query.AddScalar("VendorID", NHibernateUtil.Int64);
            query.AddScalar("VendorCode", NHibernateUtil.String);
            query.AddScalar("VendorTitle", NHibernateUtil.String);
            query.AddScalar("VendorName1", NHibernateUtil.String);
            query.AddScalar("VendorName2", NHibernateUtil.String);
            query.AddScalar("TaxNo1", NHibernateUtil.String);
            query.AddScalar("TaxNo2", NHibernateUtil.String);
            query.AddScalar("Street", NHibernateUtil.String);
            query.AddScalar("City", NHibernateUtil.String);
            query.AddScalar("Country", NHibernateUtil.String);
            query.AddScalar("PostalCode", NHibernateUtil.String);
            return query.SetResultTransformer(Transformers.AliasToBean(typeof(DbVendor))).List<DbVendor>();
        }
        #endregion

        #region IList<DbVendor> FindByDbVendorAutoComplete(string taxNo)
        /// <summary>
        /// query for auto complete
        /// </summary>
        /// <param name="taxNo"></param>
        /// <returns></returns>
        public IList<DbVendor> FindByDbVendorAutoComplete(string taxNo)
        {
            taxNo = "%" + taxNo + "%";
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" select VendorID, VendorCode,VendorTitle, Street, City,Country, PostalCode,");
            sqlBuilder.Append(" (case when TaxNo1 is null and  TaxNo2 is not null then  VendorName2 ");
            sqlBuilder.Append(" when TaxNo1 is not null and  TaxNo2 is null then  VendorName1  ");
            sqlBuilder.Append(" else VendorName2 end) as VendorName1 ");
            sqlBuilder.Append(" ,(case when TaxNo1 is null and  TaxNo2 is not null then  TaxNo2 ");
            sqlBuilder.Append(" when TaxNo1 is not null and  TaxNo2 is null then  TaxNo1 ");
            sqlBuilder.Append(" else TaxNo2 end) as TaxNo1 ");


            sqlBuilder.Append(" FROM DbVendor ");
            sqlBuilder.Append(" WHERE  (ISNULL(TaxNo1,'') like :taxNo) or (ISNULL(TaxNo2,'') like :taxNo)");
            //sqlBuilder.Append(" AND (TaxNo1 is not null or TaxNo2 is not null)");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());

            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("taxNo", typeof(string), taxNo);
            queryParameterBuilder.FillParameters(query);
            query.AddScalar("VendorID", NHibernateUtil.Int64);
            query.AddScalar("VendorCode", NHibernateUtil.String);
            query.AddScalar("VendorTitle", NHibernateUtil.String);
            query.AddScalar("VendorName1", NHibernateUtil.String);
            query.AddScalar("TaxNo1", NHibernateUtil.String);
            query.AddScalar("Street", NHibernateUtil.String);
            query.AddScalar("City", NHibernateUtil.String);
            query.AddScalar("Country", NHibernateUtil.String);
            query.AddScalar("PostalCode", NHibernateUtil.String);

            return query.SetResultTransformer(Transformers.AliasToBean(typeof(DbVendor))).List<DbVendor>();
        }
        #endregion


        #region bool isDuplicationVendorCode(string VendorCode)
        public bool isDuplicationVendorCode(string VendorCode)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" SELECT ");
            sqlBuilder.Append("     VendorID        AS VendorID     ,");
            sqlBuilder.Append("     VendorCode      AS VendorCode   ,");
            sqlBuilder.Append("     VendorTitle     AS VendorTitle  ,");
            sqlBuilder.Append("     VendorName1     AS VendorName1  ,");
            sqlBuilder.Append("     VendorName2     AS VendorName2  ,");
            sqlBuilder.Append("     Street          AS Street       ,");
            sqlBuilder.Append("     City            AS City         ,");
            sqlBuilder.Append("     Country         AS Country      ,");
            sqlBuilder.Append("     PostalCode      AS PostalCode   ,");
            sqlBuilder.Append("     TaxNo1          AS TaxNo1       ,");
            sqlBuilder.Append("     TaxNo2          AS TaxNo2       ,");
            sqlBuilder.Append("     BlockDelete     AS BlockDelete  ,");
            sqlBuilder.Append("     BlockPost       AS BlockPost    ,");
            sqlBuilder.Append("     Active          AS Active       ");

            sqlBuilder.Append(" FROM DbVendor ");
            sqlBuilder.Append(" WHERE VendorCode = :VendorCode ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.AddScalar("VendorID", NHibernateUtil.Int32);
            query.AddScalar("VendorCode", NHibernateUtil.String);
            query.AddScalar("VendorTitle", NHibernateUtil.String);
            query.AddScalar("VendorName1", NHibernateUtil.String);
            query.AddScalar("VendorName2", NHibernateUtil.String);
            query.AddScalar("Street", NHibernateUtil.String);
            query.AddScalar("City", NHibernateUtil.String);
            query.AddScalar("Country", NHibernateUtil.String);
            query.AddScalar("PostalCode", NHibernateUtil.String);
            query.AddScalar("TaxNo1", NHibernateUtil.String);
            query.AddScalar("TaxNo2", NHibernateUtil.String);
            query.AddScalar("BlockDelete", NHibernateUtil.Boolean);
            query.AddScalar("BlockPost", NHibernateUtil.Boolean);
            query.AddScalar("Active", NHibernateUtil.Boolean);

            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            parameterBuilder.AddParameterData("VendorCode", typeof(string), VendorCode);
            parameterBuilder.FillParameters(query);

            IList<DbVendor> DbVendor = query.SetResultTransformer(Transformers.AliasToBean(typeof(DbVendor))).List<DbVendor>();

            if (DbVendor.Count > 0)
                return true;
            else
                return false;
        }
        #endregion bool isDuplicationVendorCode(string VendorCode)

        #region public ISQLQuery FindVendorByCriteria(DbVendor Vendor, bool isCount, short languageId, string sortExpression, string VendorCode, string VendorName)
        public ISQLQuery FindVendorByCriteria(DbVendor Vendor, bool isCount, short languageId, string sortExpression, string VendorCode, string VendorName)
        {
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            StringBuilder sqlBuilder = new StringBuilder();
            if (!isCount)
            {
                sqlBuilder.Append(" SELECT ");
                sqlBuilder.Append("     VendorID        AS VendorID     ,");
                sqlBuilder.Append("     VendorCode      AS VendorCode   ,");
                sqlBuilder.Append("     VendorTitle     AS VendorTitle  ,");
                sqlBuilder.Append("     VendorName1     AS VendorName1  ,");
                sqlBuilder.Append("     VendorName2     AS VendorName2  ,");
                sqlBuilder.Append("     Street          AS Street       ,");
                sqlBuilder.Append("     City            AS City         ,");
                sqlBuilder.Append("     Country         AS Country      ,");
                sqlBuilder.Append("     PostalCode      AS PostalCode   ,");
                sqlBuilder.Append("     TaxNo1          AS TaxNo1       ,");
                sqlBuilder.Append("     TaxNo2          AS TaxNo2       ,");
                sqlBuilder.Append("     TaxNo3          AS TaxNo3       ,");
                sqlBuilder.Append("     TaxNo4          AS TaxNo4       ,");
                sqlBuilder.Append("     BlockDelete     AS BlockDelete  ,");
                sqlBuilder.Append("     BlockPost       AS BlockPost    ,");
                sqlBuilder.Append("     Active          AS Active       ");

                sqlBuilder.Append(" FROM DbVendor ");
                sqlBuilder.Append(" WHERE 1=1 ");

                if (VendorCode != null && VendorCode != "")
                {
                    sqlBuilder.Append(" AND VendorCode LIKE :VendorCode ");
                    parameterBuilder.AddParameterData("VendorCode", typeof(string), "%" + VendorCode + "%");
                }
                if (VendorName != null && VendorName != "")
                {
                    sqlBuilder.Append(" AND ( VendorName1 LIKE :VendorName1 ");
                    parameterBuilder.AddParameterData("VendorName1", typeof(string), "%" + VendorName + "%");
                    sqlBuilder.Append(" OR VendorName2 LIKE :VendorName2 )");
                    parameterBuilder.AddParameterData("VendorName2", typeof(string), "%" + VendorName + "%");
                }

                if (string.IsNullOrEmpty(sortExpression))
                    sqlBuilder.AppendLine(" ORDER BY VendorCode ");
                else
                    sqlBuilder.AppendLine(string.Format(" ORDER BY {0}", sortExpression));
            }
            else
            {
                sqlBuilder.Append(" SELECT COUNT(VendorCode) AS VendorCodeCount ");
                sqlBuilder.Append(" FROM DbVendor ");
                sqlBuilder.Append(" WHERE 1=1 ");

                if (VendorCode != null && VendorCode != "")
                {
                    sqlBuilder.Append(" AND VendorCode LIKE :VendorCode ");
                    parameterBuilder.AddParameterData("VendorCode", typeof(string), "%" + VendorCode + "%");
                }
                if (VendorName != null && VendorName != "")
                {
                    sqlBuilder.Append(" AND VendorName1 LIKE :VendorName1 ");
                    parameterBuilder.AddParameterData("VendorName1", typeof(string), "%" + VendorName + "%");
                    sqlBuilder.Append(" OR VendorName2 LIKE :VendorName2 ");
                    parameterBuilder.AddParameterData("VendorName2", typeof(string), "%" + VendorName + "%");
                }
            }

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            parameterBuilder.FillParameters(query);

            if (!isCount)
            {
                query.AddScalar("VendorID", NHibernateUtil.Int32);
                query.AddScalar("VendorCode", NHibernateUtil.String);
                query.AddScalar("VendorTitle", NHibernateUtil.String);
                query.AddScalar("VendorName1", NHibernateUtil.String);
                query.AddScalar("VendorName2", NHibernateUtil.String);
                query.AddScalar("Street", NHibernateUtil.String);
                query.AddScalar("City", NHibernateUtil.String);
                query.AddScalar("Country", NHibernateUtil.String);
                query.AddScalar("PostalCode", NHibernateUtil.String);
                query.AddScalar("TaxNo1", NHibernateUtil.String);
                query.AddScalar("TaxNo2", NHibernateUtil.String);
                query.AddScalar("TaxNo3", NHibernateUtil.String);
                query.AddScalar("TaxNo4", NHibernateUtil.String);
                query.AddScalar("BlockDelete", NHibernateUtil.Boolean);
                query.AddScalar("BlockPost", NHibernateUtil.Boolean);
                query.AddScalar("Active", NHibernateUtil.Boolean);

                query.SetResultTransformer(Transformers.AliasToBean(typeof(DbVendor)));
            }
            else
            {
                query.AddScalar("VendorCodeCount", NHibernateUtil.Int32);
                query.UniqueResult();
            }

            return query;
        }
        #endregion public ISQLQuery FindVendorByCriteria(DbVendor Vendor, bool isCount, short languageId, string sortExpression, string VendorCode, string VendorName)

        #region public IList<DbVendor> GetVendorList(DbVendor Vendor, short languageId, int firstResult, int maxResult, string sortExpression, string VendorCode, string VendorName)
        public IList<DbVendor> GetVendorList(DbVendor Vendor, short languageId, int firstResult, int maxResult, string sortExpression, string VendorCode, string VendorName)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<DbVendor>(ScgDbQueryProvider.DbVendorQuery, "FindVendorByCriteria", new object[] { Vendor, false, languageId, sortExpression, VendorCode, VendorName }, firstResult, maxResult, sortExpression);
        }
        #endregion public IList<DbVendor> GetVendorList(DbVendor Vendor, short languageId, int firstResult, int maxResult, string sortExpression, string VendorCode, string VendorName)

        #region public int CountVendorByCriteria(DbVendor Vendor, string VendorCode, string VendorName)
        public int CountVendorByCriteria(DbVendor Vendor, string VendorCode, string VendorName)
        {
            return NHibernateQueryHelper.CountByCriteria(ScgDbQueryProvider.DbVendorQuery, "FindVendorByCriteria", new object[] { Vendor, true, Convert.ToInt16(0), string.Empty, VendorCode, VendorName });
        }
        #endregion public int CountVendorByCriteria(DbVendor Vendor, string VendorCode, string VendorName)

        public ISQLQuery FindVendorByVendorCriteria(VOVendor criteria, bool isCount, string sortExpession)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            if (isCount)
            {
                sqlBuilder.Append(" select count(*) as VendorCount ");
            }
            else
            {
                sqlBuilder.Append(" select  VendorID, VendorCode, VendorBranch, Street, City, Country, PostalCode, VendorName, VendorTaxCode   ");
            }
            sqlBuilder.Append(" from (");
            sqlBuilder.Append(" select VendorID, VendorCode, TaxNo4 as VendorBranch, Street, City, Country, PostalCode, ");
            sqlBuilder.Append(" isnull(VendorTitle,'') +' '+ isnull(VendorName1,'') +' '+ isnull(VendorName2,'') as VendorName , isnull(TaxNo3,'') as VendorTaxCode");
            sqlBuilder.Append(" from DbVendor with (nolock) where Active = 1" + (string.IsNullOrEmpty(criteria.VendorTaxCode) ? string.Empty : " and TaxNo3 like :TaxCode"));
            sqlBuilder.Append(" union select null as VendorID, VendorCode, VendorBranch,Street, City, Country, PostalCode, VendorName, VendorTaxCode ");
            sqlBuilder.Append(" from DbOneTimeVendor with (nolock) where 1=1 " + (string.IsNullOrEmpty(criteria.VendorTaxCode) ? string.Empty : " and VendorTaxCode like :TaxCode "));
            sqlBuilder.Append(" union select null as VendorID, t.VendorCode, t.VendorBranch,t.Street, t.City, t.Country, t.PostalCode, t.VendorName, t.VendorTaxCode ");
            sqlBuilder.Append(" from FnExpenseInvoice t with (nolock) where t.Active = 1 and (convert(varchar,CreDate,103)= convert(varchar,:CreDate,103)) ");
            sqlBuilder.Append(" and NULLIF (t.VendorCode, '') <> '' and NULLIF (t.VendorName, '') <> '' ");
            sqlBuilder.Append(" and NULLIF (t.Street, '') <> '' and NULLIF (t.Country, '') <> '' and NULLIF (t.VendorTaxCode, '') <> '' " + (string.IsNullOrEmpty(criteria.VendorTaxCode) ? string.Empty : " and t.VendorTaxCode like :TaxCode "));
            sqlBuilder.Append(" and t.VendorID = null ");
            sqlBuilder.Append(" and not exists(SELECT 'X' FROM DbOneTimeVendor o WHERE o.VendorTaxCode = t.VendorTaxCode AND o.VendorBranch = t.VendorBranch) ");
            sqlBuilder.Append(" )t1 ");
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            parameterBuilder.AddParameterData("CreDate", typeof(DateTime), DateTime.Today);

            StringBuilder whereClause = new StringBuilder();
            if (!string.IsNullOrEmpty(criteria.VendorCode))
            {
                whereClause.Append(" and VendorCode like :VendorCode ");
                parameterBuilder.AddParameterData("VendorCode", typeof(string), String.Format("%{0}%", criteria.VendorCode));
            }
            if (!string.IsNullOrEmpty(criteria.VendorName))
            {
                whereClause.Append(" and VendorName like :VendorName ");
                parameterBuilder.AddParameterData("VendorName", typeof(string), String.Format("%{0}%", criteria.VendorName));
            }
            if (!string.IsNullOrEmpty(criteria.VendorTaxCode))
            {
                //whereClause.Append(" and VendorTaxCode like :TaxCode ");
                parameterBuilder.AddParameterData("TaxCode", typeof(string), String.Format("%{0}%", criteria.VendorTaxCode));
            }
            if (!string.IsNullOrEmpty(criteria.VendorBranch))
            {
                whereClause.Append(" and VendorBranch like :VendorBranch ");
                parameterBuilder.AddParameterData("VendorBranch", typeof(string), String.Format("%{0}%", criteria.VendorBranch));
            }
            if (criteria.ExcludeOneTime)
            {
                whereClause.Append(" and VendorID is not null ");
            }

            if (!string.IsNullOrEmpty(whereClause.ToString()))
            {
                sqlBuilder.Append(String.Format(" where 1=1 {0}", whereClause.ToString()));
            }

            if (!isCount)
            {
                if (!string.IsNullOrEmpty(sortExpession))
                {
                    sqlBuilder.Append(String.Format(" order by {0} ", sortExpession));
                }
                else
                {
                    sqlBuilder.Append(" order by VendorID, VendorCode, VendorName, VendorTaxCode");
                }
            }
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            parameterBuilder.FillParameters(query);

            if (!isCount)
            {
                query.AddScalar("VendorID", NHibernateUtil.Int64)
                    .AddScalar("VendorCode", NHibernateUtil.String)
                    .AddScalar("VendorBranch", NHibernateUtil.String)
                    .AddScalar("Street", NHibernateUtil.String)
                    .AddScalar("City", NHibernateUtil.String)
                    .AddScalar("Country", NHibernateUtil.String)
                    .AddScalar("PostalCode", NHibernateUtil.String)
                    .AddScalar("VendorName", NHibernateUtil.String)
                    .AddScalar("VendorTaxCode", NHibernateUtil.String);

                query.SetResultTransformer(Transformers.AliasToBean(typeof(VOVendor)));
            }
            else
            {
                query.AddScalar("VendorCount", NHibernateUtil.Int32);
                query.UniqueResult();
            }
            return query;
        }
        #region GetVendorListByCriteria
        public IList<VOVendor> GetVendorListByVendorCriteria(VOVendor criteria, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<VOVendor>(ScgDbQueryProvider.DbVendorQuery, "FindVendorByVendorCriteria", new object[] { criteria, false, sortExpression }, firstResult, maxResult, sortExpression);
        }
        #endregion

        #region CountVendorByVendorCriteria
        public int CountVendorByVendorCriteria(VOVendor criteria)
        {
            return NHibernateQueryHelper.CountByCriteria(ScgDbQueryProvider.DbVendorQuery, "FindVendorByVendorCriteria", new object[] { criteria, true, string.Empty });
        }
        #endregion

        public IList<VOVendor> FindByVendorAutoComplete(string taxNo)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" select VendorID, VendorCode, Street, City, Country, PostalCode, VendorTaxCode, VendorName ");
            sqlBuilder.Append(" from ( ");
            sqlBuilder.Append(" select VendorID, VendorCode, Street, City, Country, PostalCode, ");
            sqlBuilder.Append(" isnull(VendorTitle,'') +' '+ isnull(VendorName1,'') +' '+ isnull(VendorName2,'') as VendorName ");
            sqlBuilder.Append(" ,(case when TaxNo1 is null and  TaxNo2 is not null then  TaxNo2 ");
            sqlBuilder.Append(" when TaxNo1 is not null and  TaxNo2 is null then  TaxNo1 else TaxNo1 end) as VendorTaxCode ");
            sqlBuilder.Append(" from DbVendor with (nolock) where Active = 1 " + (string.IsNullOrEmpty(taxNo) ? string.Empty : " and (TaxNo1 like :TaxCode or TaxNo2 like :TaxCode)"));
            sqlBuilder.Append(" union select null as VendorID, VendorCode, Street, City, Country, PostalCode, VendorName, VendorTaxCode ");
            sqlBuilder.Append(" from DbOneTimeVendor with (nolock) where 1=1 " + (string.IsNullOrEmpty(taxNo) ? string.Empty : " and VendorTaxCode like :TaxCode "));
            sqlBuilder.Append(" union select VendorID, VendorCode, Street, City, Country, PostalCode, VendorName, VendorTaxCode ");
            sqlBuilder.Append(" from FnExpenseInvoice with (nolock) where Active = 1 and (convert(varchar,CreDate,103)= convert(varchar,:CreDate,103)) ");
            sqlBuilder.Append(" and NULLIF (VendorCode, '') <> '' and NULLIF (VendorName, '') <> '' ");
            sqlBuilder.Append(" and NULLIF (Street, '') <> '' and NULLIF (Country, '') <> '' and NULLIF (VendorTaxCode, '') <> '' " + (string.IsNullOrEmpty(taxNo) ? string.Empty : " and VendorTaxCode like :TaxCode "));
            sqlBuilder.Append(" )t1 ");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());

            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("TaxCode", typeof(string), String.Format("%{0}%", taxNo));
            queryParameterBuilder.AddParameterData("CreDate", typeof(DateTime), DateTime.Today);

            queryParameterBuilder.FillParameters(query);
            query.AddScalar("VendorID", NHibernateUtil.Int64);
            query.AddScalar("VendorCode", NHibernateUtil.String);
            //query.AddScalar("VendorTitle", NHibernateUtil.String);
            query.AddScalar("VendorName", NHibernateUtil.String);
            query.AddScalar("VendorTaxCode", NHibernateUtil.String);
            query.AddScalar("Street", NHibernateUtil.String);
            query.AddScalar("City", NHibernateUtil.String);
            query.AddScalar("Country", NHibernateUtil.String);
            query.AddScalar("PostalCode", NHibernateUtil.String);

            return query.SetResultTransformer(Transformers.AliasToBean(typeof(VOVendor))).List<VOVendor>();
        }

        public IList<VOVendor> FindVendorByVendorTaxNO(string taxNo, string branch)
        {
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.Append(" select VendorID, VendorCode, VendorBranch ,Street, City, Country, PostalCode, VendorName, VendorTaxCode   ");
            sqlBuilder.Append(" from (");
            sqlBuilder.Append(" select VendorID, VendorCode, TaxNo4 as VendorBranch,Street, City, Country, PostalCode, ");
            sqlBuilder.Append(" isnull(VendorTitle,'') +' '+ isnull(VendorName1,'') +' '+ isnull(VendorName2,'') as VendorName,isnull(TaxNo3,'') as VendorTaxCode");
            sqlBuilder.Append(" from DbVendor with (nolock) where Active = 1  and TaxNo3 like :taxNo");
            sqlBuilder.Append(" union select null as VendorID, VendorCode, VendorBranch,Street, City, Country, PostalCode, VendorName, VendorTaxCode ");
            sqlBuilder.Append(" from DbOneTimeVendor with (nolock) where 1=1 and VendorTaxCode like :taxNo ");
            sqlBuilder.Append(" union select null as VendorID, t.VendorCode, t.VendorBranch,t.Street, t.City, t.Country, t.PostalCode, t.VendorName, t.VendorTaxCode ");
            sqlBuilder.Append(" from FnExpenseInvoice t with (nolock) where t.Active = 1 and (convert(varchar,CreDate,103)= convert(varchar,:CreDate,103)) ");
            sqlBuilder.Append(" and NULLIF (t.VendorCode, '') <> '' and NULLIF (t.VendorName, '') <> '' ");
            sqlBuilder.Append(" and NULLIF (t.Street, '') <> '' and NULLIF (t.Country, '') <> '' and NULLIF (t.VendorTaxCode, '') <> '' " + (string.IsNullOrEmpty(taxNo) ? string.Empty : " and t.VendorTaxCode like :taxNo "));
            sqlBuilder.Append(" and t.VendorID = null ");
            sqlBuilder.Append(" and not exists(SELECT 'X' FROM DbOneTimeVendor o WHERE o.VendorTaxCode = t.VendorTaxCode AND o.VendorBranch = t.VendorBranch) ");
            sqlBuilder.Append(" )t1 where VendorBranch = :vendorBranch ");
            
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            parameterBuilder.AddParameterData("CreDate", typeof(DateTime), DateTime.Today);
            parameterBuilder.AddParameterData("taxNo", typeof(string), taxNo);
            parameterBuilder.AddParameterData("vendorBranch", typeof(string), branch);

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            parameterBuilder.FillParameters(query);

            query.AddScalar("VendorID", NHibernateUtil.Int64)
                              .AddScalar("VendorCode", NHibernateUtil.String)
                              .AddScalar("VendorBranch", NHibernateUtil.String)
                              .AddScalar("Street", NHibernateUtil.String)
                              .AddScalar("City", NHibernateUtil.String)
                              .AddScalar("Country", NHibernateUtil.String)
                              .AddScalar("PostalCode", NHibernateUtil.String)
                              .AddScalar("VendorName", NHibernateUtil.String)
                              .AddScalar("VendorTaxCode", NHibernateUtil.String);

            query.SetResultTransformer(Transformers.AliasToBean(typeof(VOVendor)));

            return query.List<VOVendor>();
        }
    }
}
