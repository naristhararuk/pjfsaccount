using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Query;
using SS.Standard.Data.NHibernate.QueryDao;
using SS.SU.DTO;
using NHibernate;
using NHibernate.Transform;

namespace SS.SU.Query.Hibernate
{
    public class SuEHRProfileLogQuery : NHibernateQueryBase<SueHrProfileLog, long>, ISuEHRProfileLogQuery
    {
        #region public ISQLQuery FindEHRProfileLog( bool isCount , string sortExpression)
        public ISQLQuery FindEHRProfileLog(bool isCount, string sortExpression)
        {


            StringBuilder sqlBuilder = new StringBuilder();


            if (!isCount)
            {
                sqlBuilder.Append(" SELECT eHrProfileLogID,PeopleID,UserName,Message");
                sqlBuilder.Append(" FROM SuEHrProfileLog");

                if (string.IsNullOrEmpty(sortExpression))
                    sqlBuilder.AppendLine(" ORDER BY eHrProfileLogID,peopleID");
                else
                    sqlBuilder.AppendLine(string.Format(" ORDER BY {0}", sortExpression));

            }
            else
            {
                sqlBuilder.Append("SELECT COUNT(1) as LogCount FROM SuEHrProFIleLog");
            }


            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());

            if (!isCount)
            {
                query.AddScalar("EHrProfileLogID", NHibernateUtil.Int64);
                query.AddScalar("PeopleID", NHibernateUtil.String);
                query.AddScalar("UserName", NHibernateUtil.String);
                query.AddScalar("Message", NHibernateUtil.String);

                query.SetResultTransformer(Transformers.AliasToBean(typeof(SueHrProfileLog)));


            }
            else
            {
                query.AddScalar("LogCount", NHibernateUtil.Int32);
                query.UniqueResult();
            }
            return query;

        }
        #endregion

        #region Query for basegridview section. 
        public int CountEHRProfileLogByCriteria()
        {
            return NHibernateQueryHelper.CountByCriteria(QueryProvider.SuEHRProfileLogQuery, "FindEHRProfileLog", new object[] {  true, string.Empty });
        }
        

        public IList<SueHrProfileLog> GeteHrProfileLogList(int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<SueHrProfileLog>(QueryProvider.SuEHRProfileLogQuery, "FindEHRProfileLog", new object[] { false, sortExpression }, firstResult, maxResult, sortExpression);
        }
        #endregion
    }
}