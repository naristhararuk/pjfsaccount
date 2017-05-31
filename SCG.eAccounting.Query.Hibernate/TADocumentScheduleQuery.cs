using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.eAccounting.DTO;
using NHibernate;
using SS.Standard.Data.NHibernate.QueryDao;
using SS.Standard.Data.NHibernate.QueryCreator;
using NHibernate.Transform;
using SCG.eAccounting.DTO.ValueObject;
using NHibernate.Expression;

namespace SCG.eAccounting.Query.Hibernate
{
    public class TADocumentScheduleQuery : NHibernateQueryBase<TADocumentSchedule, int>, ITADocumentScheduleQuery
    {
        #region public ISQLQuery FindByTADocumentScheduleCriteria(bool isCount, string sortExpression)
        public ISQLQuery FindByTADocumentScheduleCriteria(bool isCount, string sortExpression)
        {
            ISQLQuery query;
            StringBuilder sqlBuilder = new StringBuilder();

            if (!isCount)
            {
                sqlBuilder.Append(" SELECT ");
                sqlBuilder.Append("     ScheduleID      AS ScheduleID, ");
                sqlBuilder.Append("     TADocumentID    AS TADocumentID, ");
                sqlBuilder.Append("     Date            AS Date, ");
                sqlBuilder.Append("     DepartureFrom   AS DepartureFrom, ");
                sqlBuilder.Append("     ArrivalAt       AS ArrivalAt, ");
                sqlBuilder.Append("     TravelBy        AS TravelBy, ");
                sqlBuilder.Append("     Time            AS Time, ");
                sqlBuilder.Append("     FlightNo        AS FlightNo, ");
                sqlBuilder.Append("     TravellingDetail AS TravellingDetail ");                
                sqlBuilder.Append(" FROM TADocumentSchedule ");

                if (string.IsNullOrEmpty(sortExpression))
                    sqlBuilder.AppendLine("ORDER BY ScheduleID,TADocumentID,Date,DepartureFrom,ArrivalAt");
                else
                    sqlBuilder.AppendLine(string.Format(" ORDER BY {0}", sortExpression));

                query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());

                QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
                queryParameterBuilder.FillParameters(query);
                query.AddScalar("ScheduleID", NHibernateUtil.Int32);
                query.AddScalar("TADocumentID", NHibernateUtil.Int64);
                query.AddScalar("Date", NHibernateUtil.DateTime);
                query.AddScalar("DepartureFrom", NHibernateUtil.String);
                query.AddScalar("ArrivalAt", NHibernateUtil.String);
                query.AddScalar("TravelBy", NHibernateUtil.String);
                query.AddScalar("Time", NHibernateUtil.String);
                query.AddScalar("FlightNo", NHibernateUtil.String);
                query.AddScalar("TravellingDetail", NHibernateUtil.String);

                query.SetResultTransformer(Transformers.AliasToBean(typeof(TADocumentObj)));
            }
            else
            {
                sqlBuilder.Append(" SELECT COUNT(*) AS Count FROM TADocumentSchedule ");

                query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());

                query.AddScalar("Count", NHibernateUtil.Int32);
                query.UniqueResult();
            }

            return query;
        }
        #endregion public ISQLQuery FindByTADocumentScheduleCriteria(bool isCount, string sortExpression)

        #region public IList<TADocumentSchedule> GetTADocumentScheduleList(int firstResult, int maxResult, string sortExpression)
        public IList<TADocumentObj> GetTADocumentScheduleList(int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<TADocumentObj>(ScgeAccountingQueryProvider.TADocumentScheduleQuery, "FindByTADocumentScheduleCriteria", new object[] { false, sortExpression }, firstResult, maxResult, sortExpression);
        }
        #endregion public IList<TADocumentSchedule> GetTADocumentScheduleList(int firstResult, int maxResult, string sortExpression)

        #region public int CountByTADocumentScheduleCriteria()
        public int CountByTADocumentScheduleCriteria()
        {
            return NHibernateQueryHelper.CountByCriteria(ScgeAccountingQueryProvider.TADocumentScheduleQuery, "FindByTADocumentScheduleCriteria", new object[] { true, string.Empty });
        }
        #endregion public int CountByTADocumentScheduleCriteria()

        #region public IList<TADocumentSchedule> FindTADocumentScheduleByTADocumentID(long taDocumentID)
        public IList<TADocumentSchedule> FindTADocumentScheduleByTADocumentID(long taDocumentID)
        {
            ICriteria criteria = GetCurrentSession().CreateCriteria(typeof(TADocumentSchedule), "t");
            criteria.Add(Expression.Eq("t.TADocumentID.TADocumentID", taDocumentID));

            return criteria.List<TADocumentSchedule>();
        }
        #endregion public IList<TADocumentSchedule> FindTADocumentScheduleByTADocumentID(long taDocumentID)
    }
}
