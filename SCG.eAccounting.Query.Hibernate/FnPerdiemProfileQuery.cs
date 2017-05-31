using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using NHibernate.Expression;
using NHibernate.Transform;

using SCG.eAccounting.DTO;
using SCG.eAccounting.DTO.ValueObject;
using SCG.eAccounting.Query;
using SS.Standard.Data.NHibernate.Query;
using SS.Standard.Data.NHibernate.QueryDao;
using SS.Standard.Data.NHibernate.QueryCreator;

namespace SCG.eAccounting.Query.Hibernate
{
    public class FnPerdiemProfileQuery : NHibernateQueryBase<FnPerdiemProfile, long>, IFnPerdiemProfileQuery
    {
        public int CountForeignPerdiemRateProfile(ForeignPerdiemRateProfileCriteria criteria)
        {
            return NHibernateQueryHelper.CountByCriteria(ScgeAccountingQueryProvider.FnPerdiemProfileQuery, "FindForeignPerdiemRateProfileByCriteria", new object[] { criteria, string.Empty, true });
        }
        public IList<FnPerdiemProfile> GetForeignPerdiemRateProfileListByCriteria(ForeignPerdiemRateProfileCriteria criteria, int startRow, int pageSize, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<FnPerdiemProfile>(ScgeAccountingQueryProvider.FnPerdiemProfileQuery, "FindForeignPerdiemRateProfileByCriteria", new object[] { criteria, sortExpression, false }, startRow, pageSize, sortExpression);
        }
        public ISQLQuery FindForeignPerdiemRateProfileByCriteria(ForeignPerdiemRateProfileCriteria criteria, string sortExpression, bool isCount)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            QueryParameterBuilder paramBuilder = new QueryParameterBuilder();
            if (!isCount)
            {
                sqlBuilder.Append(" select PerdiemProfileID,PerdiemProfileName ,Description,Active ");
            }
            else
            {
                sqlBuilder.Append(" select count(*) as Count ");
            }

            sqlBuilder.Append("  from FnPerdiemProfile where 1=1  ");

            if (!string.IsNullOrEmpty(criteria.PerdiemProfileName))
            {
                sqlBuilder.Append(" and PerdiemProfileName like :perdiemProfileName ");
                paramBuilder.AddParameterData("perdiemProfileName", typeof(string), "%" + criteria.PerdiemProfileName + "%");
            }
            if (!string.IsNullOrEmpty(criteria.Description))
            {
                sqlBuilder.Append(" and Description like :description ");
                paramBuilder.AddParameterData("description", typeof(string), "%" + criteria.Description + "%");
            }
            if (!string.IsNullOrEmpty(sortExpression))
            {

                sqlBuilder.Append(String.Format(" order by {0} ", sortExpression));
            }

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            paramBuilder.FillParameters(query);
            if (isCount)
            {
                query.AddScalar("Count", NHibernateUtil.Int32);
            }
            else
            {
                query.AddScalar("PerdiemProfileID", NHibernateUtil.Int64);
                query.AddScalar("PerdiemProfileName", NHibernateUtil.String);
                query.AddScalar("Description", NHibernateUtil.String);
                query.AddScalar("Active", NHibernateUtil.Boolean);

                query.SetResultTransformer(Transformers.AliasToBean(typeof(FnPerdiemProfile)));
            }
            return query;
        }
        public IList<FnPerdiemProfile> GetPRList()
        {
            StringBuilder sqlBuilder = new StringBuilder();
            QueryParameterBuilder paramBuilder = new QueryParameterBuilder();
            sqlBuilder.Append(" select PerdiemProfileID,PerdiemProfileName ");

            sqlBuilder.Append("  from FnPerdiemProfile where 1=1  ");


            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            paramBuilder.FillParameters(query);
            query.AddScalar("PerdiemProfileID", NHibernateUtil.Int64);
            query.AddScalar("PerdiemProfileName", NHibernateUtil.String);
            return query.SetResultTransformer(Transformers.AliasToBean(typeof(FnPerdiemProfile))).List<FnPerdiemProfile>().ToList();
        }
    }
}
