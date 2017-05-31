using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.QueryDao;
using NHibernate;
using NHibernate.Expression;
using NHibernate.Transform;
using SCG.DB.DTO;
using SS.Standard.Data.NHibernate.QueryCreator;

namespace SCG.DB.Query.Hibernate
{
    public class DbProfileListQuery : NHibernateQueryBase<DbProfileList, Guid>, IDbProfileListQuery
    {

        #region public ISQLQuery FindByCountryCriteria(bool isCount)
        public ISQLQuery FindByProfileListCriteria(bool isCount)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            if (!isCount)
            {
                sqlBuilder.Append(" SELECT  ProfileName , Id ");
                sqlBuilder.Append(" FROM MileageProfile ");
            }
            else
            {
                sqlBuilder.Append(" SELECT  count(ProfileName)  AS ProfileNameCount");
                sqlBuilder.Append(" FROM MileageProfile ");
            }
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());

            if (!isCount)
            {
                query.AddScalar("ProfileName", NHibernateUtil.String);
                query.AddScalar("Id", NHibernateUtil.Guid);
                query.SetResultTransformer(Transformers.AliasToBean(typeof(DbProfileList)));
            }
            else
            {
                query.AddScalar("ProfileNameCount", NHibernateUtil.Int32);
                query.UniqueResult();
            }

            return query;
        }
        #endregion public ISQLQuery FindByCountryCriteria(IDbProfileList profileList, bool isCount, string sortExpression)


        public IList<DbProfileList> GetProfileList(int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<DbProfileList>(ScgDbQueryProvider.DbProfileListQuery, "FindByProfileListCriteria", new object[] { false }, firstResult, maxResult, sortExpression);
        }

        public int CountByProfileCriteria()
        {
            return NHibernateQueryHelper.CountByCriteria(ScgDbQueryProvider.DbProfileListQuery, "FindByProfileListCriteria", new object[] { true });
        }

        public Guid? GetProfileListIdByName(string Name)
        {
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.Append(" SELECT Id ");
            sqlBuilder.Append(" FROM MileageProfile ");
            sqlBuilder.Append(" WHERE ProfileName = :Name ");

            var result = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString()).AddScalar("Id", NHibernateUtil.Guid).SetString("Name", Name).UniqueResult();
            if (result != null)
                return new Guid(result.ToString());
            else
                return new Guid();    
        }


        public IList<DbCompany> FindProfileToUse(Guid Id)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();

            sqlBuilder.Append(" Select CompanyCode from DbCompany ");
            sqlBuilder.Append(" WHERE [MileageProfileId] = :profileId AND Active = 1");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            queryParameterBuilder.AddParameterData("profileId", typeof(Guid), Id);

            queryParameterBuilder.FillParameters(query);
            query.AddScalar("CompanyCode", NHibernateUtil.String);
            return query.SetResultTransformer(Transformers.AliasToBean(typeof(DbCompany))).List<DbCompany>();
        }
    }
}
