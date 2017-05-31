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
    public class DbMileageRateRevisionQuery : NHibernateQueryBase<DbMileageRateRevision, Guid>, IDbMileageRateRevisionQuery
    {

        #region public ISQLQuery FindByCountryCriteria(bool isCount)
        public ISQLQuery FindByMileageRateRevision(bool isCount)
        {
            
            StringBuilder sqlBuilder = new StringBuilder();
            if (!isCount)
            {               
                sqlBuilder.Append("SELECT  Id , ApprovedDate , EffectiveFromDate , EffectiveToDate,");
                sqlBuilder.Append("CASE [Status]");
                sqlBuilder.Append("WHEN 0 THEN 'Draft'");
                sqlBuilder.Append("WHEN 1 THEN 'Approve'");
                sqlBuilder.Append("WHEN 2 THEN 'Cancel'");
                sqlBuilder.Append("END AS [StatusDesc]");
                sqlBuilder.Append(" FROM MileageRateRevision ");
                sqlBuilder.Append(" ORDER BY ApprovedDate DESC ");
            }
            else 
            {
                sqlBuilder.Append(" SELECT  count(Id)  AS Id");
                sqlBuilder.Append(" FROM MileageRateRevision ");
            }
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());

            if (!isCount)
            {
                query.AddScalar("Id", NHibernateUtil.Guid);
                query.AddScalar("ApprovedDate", NHibernateUtil.DateTime);
                query.AddScalar("EffectiveFromDate", NHibernateUtil.DateTime);
                query.AddScalar("EffectiveToDate",NHibernateUtil.DateTime);
               // query.AddScalar("Status",NHibernateUtil.Byte);
                query.AddScalar("StatusDesc",NHibernateUtil.String);
                query.SetResultTransformer(Transformers.AliasToBean(typeof(DbMileageRateRevision)));
            }
            else
            {
                query.AddScalar("Id", NHibernateUtil.Int32);
                query.UniqueResult();
            }

            return query;
        }
        #endregion public ISQLQuery FindByCountryCriteria(DbMileageRateRevision MileageRateRevision, bool isCount, string sortExpression)


        public IList<DbMileageRateRevision> GetMileageRateRevisionList(int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<DbMileageRateRevision>(ScgDbQueryProvider.DbMileageRateRevisionQuery, "FindByMileageRateRevision", new object[] { false }, firstResult, maxResult, sortExpression);
        }

        public int CountByMileageRateRevision()
        {
            return NHibernateQueryHelper.CountByCriteria(ScgDbQueryProvider.DbMileageRateRevisionQuery, "FindByMileageRateRevision", new object[] { true });
        }

        public ISQLQuery FindMieagePermission(long UserID)
        {

            StringBuilder sqlBuilder = new StringBuilder();
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();

            sqlBuilder.Append(" SELECT count(*) AS Total  FROM SuUser a  ");
            sqlBuilder.Append(" INNER JOIN SuUserRole b ON a.UserID = b.UserID ");
            sqlBuilder.Append(" INNER JOIN SuRole c ON b.RoleID = c.RoleID ");
            sqlBuilder.Append(" LEFT JOIN SuProgramRole d ON c.RoleID = d.RoleID ");
            sqlBuilder.Append(" LEFT JOIN SuProgram e ON d.ProgramID = e.ProgramID ");
            sqlBuilder.Append(" WHERE  a.UserID = :UserID  AND e.ProgramCode = 'MileageApproveCancel' "); 

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            queryParameterBuilder.AddParameterData("UserID", typeof(long), UserID);
            queryParameterBuilder.FillParameters(query);

            query.AddScalar("Total", NHibernateUtil.Int32);
            query.UniqueResult();

            return query;
        }

        public bool ChekPermission(long UerID)
        {
            int result = NHibernateQueryHelper.CountByCriteria(ScgDbQueryProvider.DbMileageRateRevisionQuery, "FindMieagePermission", new object[] { UerID });

            if (result >= 1) { 
                return true;
            }
                return false;
        }

        public DbMileageRateRevision FindEffectiveDate(Guid Id)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();

            sqlBuilder.Append(" Select EffectiveFromDate , EffectiveToDate from MileageRateRevision ");
            sqlBuilder.Append(" WHERE  Active = 1 AND Id = :id");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            queryParameterBuilder.AddParameterData("id", typeof(Guid), Id);

            queryParameterBuilder.FillParameters(query);
            query.AddScalar("EffectiveFromDate", NHibernateUtil.DateTime);
            query.AddScalar("EffectiveToDate", NHibernateUtil.DateTime);
            return query.SetResultTransformer(Transformers.AliasToBean(typeof(DbMileageRateRevision))).List<DbMileageRateRevision>().FirstOrDefault() ;
        }

       
    }
}
