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
    public class DbMileageRateRevisionDetailQuery : NHibernateQueryBase<DbMileageRateRevisionDetail, Guid>, IDbMileageRateRevisionDetailQuery
    {

        #region public ISQLQuery FindByCountryCriteria(bool isCount)
        public ISQLQuery FindByMileageRateRevisionItem(Guid Id, bool isCount)
        {

            StringBuilder sqlBuilder = new StringBuilder();
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();

            if (!isCount)
            {
                sqlBuilder.Append(" SELECT [Id], [MileageProfileId],[ProfileName],[PersonalLevelGroupCode],[MotocycleRate],[MotocycleRate2],[CarRate] , [CarRate2] ,[PickUpRate] , [PickUpRate2] FROM  (  ");
            }
            else
            {
                sqlBuilder.Append(" SELECT count(*) AS Total FROM ( ");
            }

            sqlBuilder.Append(" SELECT ISNULL(d.Id, '00000000-0000-0000-0000-000000000000') AS Id,  b.id as MileageProfileId, b.ProfileName,a.PersonalLevelGroupCode, ISNULL(d.MotocycleRate,0) AS MotocycleRate, ISNULL(d.MotocycleRate2,0) AS MotocycleRate2, ");
            sqlBuilder.Append(" ISNULL(d.CarRate,0) AS CarRate, ISNULL(d.CarRate2, 0) AS CarRate2 , ISNULL(d.PickUpRate, 0) AS PickUpRate , ISNULL(d.PickUpRate2,0) AS PickUpRate2  From SuUserPersonalLevel a ");
            sqlBuilder.Append(" cross join MileageProfile b cross join MileageRateRevision c left join MileageRateRevisionItem d on c.Id = d.MileageRateRevisionId AND b.Id = d.MileageProfileId AND d.PersonalLevelGroupCode = a.PersonalLevelGroupCode ");
            sqlBuilder.Append(" where ( a.PersonalLevelGroupCode = 'จ' OR a.PersonalLevelGroupCode = 'ป' OR a.PersonalLevelGroupCode = 'บ') ");
            sqlBuilder.Append(" AND c.Id = :MiRvsId ");
            sqlBuilder.Append(" group by a.PersonalLevelGroupCode, b.ProfileName , b.Id ,d.Id, d.MotocycleRate , d.MotocycleRate2, ");
            sqlBuilder.Append(" d.CarRate , d.CarRate2 , d.PickUpRate , d.PickUpRate2 ");
            sqlBuilder.Append(" )t1 ");

            if (!isCount)
                sqlBuilder.Append(" Order by ProfileName ");


            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            queryParameterBuilder.AddParameterData("MiRvsId", typeof(Guid), Id);
            queryParameterBuilder.FillParameters(query);


            if (!isCount)
            {
                query.AddScalar("Id", NHibernateUtil.Guid);
                query.AddScalar("MileageProfileId", NHibernateUtil.Guid);
                query.AddScalar("ProfileName", NHibernateUtil.String);
                query.AddScalar("PersonalLevelGroupCode", NHibernateUtil.String);
                query.AddScalar("MotocycleRate", NHibernateUtil.Double);
                query.AddScalar("MotocycleRate2", NHibernateUtil.Double);
                query.AddScalar("CarRate", NHibernateUtil.Double);
                query.AddScalar("CarRate2", NHibernateUtil.Double);
                query.AddScalar("PickUpRate", NHibernateUtil.Double);
                query.AddScalar("PickUpRate2", NHibernateUtil.Double);
                query.SetResultTransformer(Transformers.AliasToBean(typeof(DbMileageRateRevisionDetail)));
            }
            else
            {
                query.AddScalar("Total", NHibernateUtil.Int32);
                query.UniqueResult();
            }

            return query;
        }
        #endregion public ISQLQuery FindByCountryCriteria(DbMileageRateRevision MileageRateRevision, bool isCount, string sortExpression)


        public IList<DbMileageRateRevisionDetail> GetMileageRateRevisionListItem(Guid Id, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<DbMileageRateRevisionDetail>(ScgDbQueryProvider.DbMileageRateRevisionDetailQuery, "FindByMileageRateRevisionItem", new object[] { Id, false }, firstResult, maxResult, sortExpression);
        }

        public int CountByMileageRateRevisionItem(Guid Id)
        {
            return NHibernateQueryHelper.CountByCriteria(ScgDbQueryProvider.DbMileageRateRevisionDetailQuery, "FindByMileageRateRevisionItem", new object[] { Id, true });
        }

        public IList<DbMileageRateRevisionDetail> FindPositionLevel()
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" Select PersonalLevelGroupCode from [dbo].[SuUserPersonalLevel] ");
            sqlBuilder.Append(" Where PersonalLevelGroupCode = 'จ' OR PersonalLevelGroupCode = 'บ' OR PersonalLevelGroupCode = 'ป' ");
            sqlBuilder.Append(" Group By PersonalLevelGroupCode ");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.AddScalar("PersonalLevelGroupCode", NHibernateUtil.String);
            return query.SetResultTransformer(Transformers.AliasToBean(typeof(DbMileageRateRevisionDetail))).List<DbMileageRateRevisionDetail>();
        }

        public string FindPositionLevelByCode(string personalLevelCode)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" Select TOP(1)PersonalLevelGroupCode from [dbo].[SuUserPersonalLevel] ");
            sqlBuilder.Append(" Where Code = :personalLevelCode ");
            sqlBuilder.Append(" Group By PersonalLevelGroupCode ");
            return GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString())
                .AddScalar("PersonalLevelGroupCode", NHibernateUtil.String)
                .SetString("personalLevelCode", personalLevelCode)
                .UniqueResult<string>();
        }

        public DbMileageRateRevisionDetail FindMileageRateRevision(Guid mlpID, string code, DateTime date)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            sqlBuilder.Append(" Select TOP(1) d.MotocycleRate,d.MotocycleRate2,d.PickUpRate,d.PickUpRate2,d.CarRate,d.CarRate2,c.EffectiveFromDate,c.EffectiveToDate,c.ApprovedDate ");
            sqlBuilder.Append(" from [dbo].[MileageRateRevision] c  ");
            sqlBuilder.Append(" inner Join [dbo].[MileageRateRevisionItem] d on d.MileageRateRevisionId = c.Id ");
            sqlBuilder.Append(" WHERE d.MileageProfileId = :mlpID AND d.PersonalLevelGroupCode = :code AND c.[Status] = 1 AND c.Active = 1 AND (DATEADD(day , -1 ,c.EffectiveFromDate) < :date AND DATEADD(day , 1 ,c.EffectiveToDate) > :date) ");
            sqlBuilder.Append(" ORDER BY c.ApprovedDate DESC ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            queryParameterBuilder.AddParameterData("mlpID", typeof(Guid), mlpID);
            queryParameterBuilder.AddParameterData("code", typeof(string), code);
            queryParameterBuilder.AddParameterData("date", typeof(DateTime), date);

            queryParameterBuilder.FillParameters(query);
            query.AddScalar("MotocycleRate", NHibernateUtil.Double);
            query.AddScalar("MotocycleRate2", NHibernateUtil.Double);
            query.AddScalar("PickUpRate", NHibernateUtil.Double);
            query.AddScalar("PickUpRate2", NHibernateUtil.Double);
            query.AddScalar("CarRate", NHibernateUtil.Double);
            query.AddScalar("CarRate2", NHibernateUtil.Double);
            query.AddScalar("EffectiveFromDate", NHibernateUtil.DateTime);
            query.AddScalar("EffectiveToDate", NHibernateUtil.DateTime);
            query.AddScalar("ApprovedDate", NHibernateUtil.DateTime);
            return query.SetResultTransformer(Transformers.AliasToBean(typeof(DbMileageRateRevisionDetail))).List<DbMileageRateRevisionDetail>().FirstOrDefault();
        }

        public IList<DbMileageRateRevisionDetail> FindForRemoveMileageRateRevisionItem(Guid Id)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();

            sqlBuilder.Append(" Select Id ");
            sqlBuilder.Append(" From [dbo].[MileageRateRevisionItem] ");
            sqlBuilder.Append(" WHERE MileageRateRevisionId = :MiRvsId ");

            queryParameterBuilder.AddParameterData("MiRvsId", typeof(Guid), Id);

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            queryParameterBuilder.FillParameters(query);
            query.AddScalar("Id", NHibernateUtil.Guid);

            return query.SetResultTransformer(Transformers.AliasToBean(typeof(DbMileageRateRevisionDetail))).List<DbMileageRateRevisionDetail>();

        }

    }
}
