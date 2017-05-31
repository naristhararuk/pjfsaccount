using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.SU.DTO;
using SS.Standard.Data.NHibernate.QueryDao;
using SS.SU.DTO.ValueObject;
using SS.Standard.Data.NHibernate.Query;
using NHibernate.Transform;
using SS.Standard.Data.NHibernate.QueryCreator;
using NHibernate.Impl;
using System.Reflection.Emit;
using NHibernate;


namespace SS.SU.Query.Hibernate
{
    public class SuEHRExpenseLogQuery : NHibernateQueryBase<SueHrExpenseLog, long>, ISuEHRExpenseLogQuery
    {


        public IList<SueHrExpenseLog> GeteHrExpenseLogList(SueHrExpenseLog query, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<SueHrExpenseLog>(
                 QueryProvider.SuEHRExpenseLogQuery
                , "FindEHRExpenseLogResult"
                , new object[] { query, false, sortExpression }
                , firstResult
                , maxResult
                , sortExpression);
        }

       


        public int CountEHRExpenseLogByCriteria(SueHrExpenseLog EHRExpenseLogObj)
        {
            return NHibernateQueryHelper.CountByCriteria(QueryProvider.SuEHRExpenseLogQuery, "FindEHRExpenseLogResult", new object[] { EHRExpenseLogObj, true, string.Empty });
        }

        public NHibernate.ISQLQuery FindEHRExpenseLogResult(SueHrExpenseLog EHRExpenseLogObj, bool isCount, string sortExpression)
        {
            ISQLQuery query = null;
            StringBuilder sqlBuilder = new StringBuilder();
            QueryParameterBuilder parabuilder = new QueryParameterBuilder();
            if (!isCount)
            {
                sqlBuilder.Append("Select EHrExpenseLogID,LastDate , EHrExpenseID , ExpenseRequestNo ,Status , Message ,ExpenseDate");
                sqlBuilder.Append(" FROM SuEHrExpenseLog ");
                sqlBuilder.Append(" WHERE Active=1 ");

                if (EHRExpenseLogObj.LastDate != new DateTime())
                {
                    //sqlBuilder.Append("AND LastDate like :LastDate ");
                    //sqlBuilder.Append("AND LastDate >= :LastDate ");
                    sqlBuilder.Append("AND Convert(DateTime,Convert(varchar(12),LastDate),103) = Convert(DateTime, :LastDate, 103) ");
                    parabuilder.AddParameterData("LastDate", typeof(DateTime), EHRExpenseLogObj.LastDate);

                }
                if ((!string.IsNullOrEmpty(EHRExpenseLogObj.ExpenseRequestNo)))
                {
                    sqlBuilder.Append("AND ExpenseRequestNo Like :ExpenseRequestNo ");
                    parabuilder.AddParameterData("ExpenseRequestNo", typeof(string), string.Format("%{0}%", EHRExpenseLogObj.ExpenseRequestNo));
                    
                }
                
                if (!string.IsNullOrEmpty(EHRExpenseLogObj.Status))
                {
                    sqlBuilder.Append("AND Status Like :Status ");
                    parabuilder.AddParameterData("Status", typeof(string), EHRExpenseLogObj.Status);

                }
                if (string.IsNullOrEmpty(sortExpression))
                {
                    sqlBuilder.Append(" order by LastDate desc, EHrExpenseLogID, EHrExpenseID ");
                }
                else
                {
                    sqlBuilder.Append(string.Format(" order by {0} ", sortExpression));
                }

                query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
                parabuilder.FillParameters(query);

                query.AddScalar("EHrExpenseLogID", NHibernateUtil.Int64);
                query.AddScalar("LastDate", NHibernateUtil.DateTime);
                query.AddScalar("EHrExpenseID", NHibernateUtil.String);
                query.AddScalar("ExpenseRequestNo", NHibernateUtil.String);
                query.AddScalar("Status", NHibernateUtil.String);
                query.AddScalar("Message", NHibernateUtil.String);
                query.AddScalar("ExpenseDate", NHibernateUtil.DateTime);

                query.SetResultTransformer(Transformers.AliasToBean(typeof(SueHrExpenseLog)));
            }
            else
            {
                sqlBuilder.Append("Select COUNT(1) AS LogCount");
                sqlBuilder.Append(" FROM SuEHrExpenseLog ");
                sqlBuilder.Append(" WHERE Active=1 ");

                if (EHRExpenseLogObj.LastDate != new DateTime())
                {
                    //sqlBuilder.Append("AND LastDate like :LastDate ");
                    //sqlBuilder.Append("AND LastDate >= :LastDate ");
                    sqlBuilder.Append("AND Convert(DateTime,Convert(varchar(12),LastDate),103) = Convert(DateTime, :LastDate, 103) ");       
                    parabuilder.AddParameterData("LastDate", typeof(DateTime), EHRExpenseLogObj.LastDate);
                }
                if (!string.IsNullOrEmpty(EHRExpenseLogObj.Status))
                {
                    sqlBuilder.Append("AND Status Like :Status ");
                    parabuilder.AddParameterData("Status", typeof(string), EHRExpenseLogObj.Status);
                }

                if ((!string.IsNullOrEmpty(EHRExpenseLogObj.ExpenseRequestNo)))
                {
                    sqlBuilder.Append("AND ExpenseRequestNo Like :ExpenseRequestNo ");
                    parabuilder.AddParameterData("ExpenseRequestNo", typeof(string), string.Format("%{0}%", EHRExpenseLogObj.ExpenseRequestNo));
                }

                query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
                parabuilder.FillParameters(query);
                query.AddScalar("LogCount", NHibernateUtil.Int32);              
            }

         return query;
        }
    }
}

