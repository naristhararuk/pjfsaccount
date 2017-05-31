using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.QueryDao;
using SS.SU.DTO;
using NHibernate;
using NHibernate.Transform;
using SS.SU.DTO.ValueObject;
using SS.Standard.Data.NHibernate.QueryCreator;

namespace SS.SU.Query.Hibernate
{
    public class SuUserPersonalLevelQuery : NHibernateQueryBase<SuUserPersonalLevel, string>, ISuUserPersonalLevelQuery
    {
        public int CountMaintainPersonalLevel(MaintainPersonalLevelCriteria criteria)
        {
            return NHibernateQueryHelper.CountByCriteria(QueryProvider.SuUserPersonalLevelQuery, "FindMaintainPersonalLevelByCriteria", new object[] { criteria, string.Empty, true });
        }
        public IList<SuUserPersonalLevel> GetMaintainPersonalLevelListByCriteria(MaintainPersonalLevelCriteria criteria, int startRow, int pageSize, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<SuUserPersonalLevel>(QueryProvider.SuUserPersonalLevelQuery, "FindMaintainPersonalLevelByCriteria", new object[] { criteria, sortExpression, false }, startRow, pageSize, sortExpression);
        }
        public ISQLQuery FindMaintainPersonalLevelByCriteria(MaintainPersonalLevelCriteria criteria, string sortExpression, bool isCount)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            QueryParameterBuilder paramBuilder = new QueryParameterBuilder();
            if (!isCount)
            {
                sqlBuilder.Append(" select Code As PersonalLevel ,Description,Active,Ordinal ");
            }
            else
            {
                sqlBuilder.Append(" select count(*) as Count ");
            }

            sqlBuilder.Append("  from SuUserPersonalLevel where 1=1  ");

            if (!string.IsNullOrEmpty(criteria.PersonalLevel))
            {
                sqlBuilder.Append(" and Code like :personalLevel ");
                paramBuilder.AddParameterData("personalLevel", typeof(string), "%"+criteria.PersonalLevel+"%");
            }
            if (!string.IsNullOrEmpty(criteria.Description))
            {
                sqlBuilder.Append(" and Description like :description ");
                paramBuilder.AddParameterData("description", typeof(string), "%"+criteria.Description+"%");
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
                query.AddScalar("PersonalLevel", NHibernateUtil.String);
                query.AddScalar("Description", NHibernateUtil.String);
                query.AddScalar("Active", NHibernateUtil.Boolean);
                query.AddScalar("Ordinal", NHibernateUtil.String);
                query.SetResultTransformer(Transformers.AliasToBean(typeof(SuUserPersonalLevel)));
            }
            return query;
        }

        public IList<SuUserPersonalLevel> GetPLList()
        {
            StringBuilder sqlBuilder = new StringBuilder();
            QueryParameterBuilder paramBuilder = new QueryParameterBuilder();
            sqlBuilder.Append(" select Code As PersonalLevel  ");
            sqlBuilder.Append("  from SuUserPersonalLevel where 1=1  ");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            paramBuilder.FillParameters(query);
            query.AddScalar("PersonalLevel", NHibernateUtil.String);
            return query.SetResultTransformer(Transformers.AliasToBean(typeof(SuUserPersonalLevel))).List<SuUserPersonalLevel>();
            
        }
    }
}
