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
using NHibernate.Expression;

namespace SCG.DB.Query.Hibernate
{
    public class DbMoneyRequestQuery : NHibernateQueryBase<MoneyRequestSearchResult, long>, IDbMoneyRequestQuery
    {
        //public SS.Standard.Security.IUserAccount UserAccount { get; set; }

        public ISQLQuery FindMoneyBuyingRequestDocument(MoneyRequestSearchResult param, bool isCount, string sortExpression)
        {
            StringBuilder sql = new StringBuilder();
            if (isCount)
            {
                sql.Append("SELECT count(*) as MoneyRequestCount from ( ");
            }
    
            sql.Append("SELECT d.DocumentID , d.DocumentNo ,s.EmployeeName,sum(ai.Amount) as Amount,a.RequestDateOfAdvance,c.CompanyID,bd.LetterNo ");
            sql.Append("FROM AvAdvanceDocument a  INNER JOIN Document d  ON d.DocumentID = a.DocumentID ");
            sql.Append("INNER JOIN DbCompany c  ON d.CompanyID = c.CompanyID ");
            sql.Append("INNER JOIN WorkFlow w ON w.DocumentID = a.DocumentID ");
            sql.Append("INNER JOIN SuUser s  ON s.UserID = d.RequesterID ");
            sql.Append("INNER JOIN AvAdvanceItem ai  ON ai.AdvanceID = a.AdvanceID ");
            sql.Append("LEFT JOIN DbBuyingLetter bl ON bl.DocumentID = d.DocumentID ");
            sql.Append("LEFT JOIN DbBuyingLetterDetail bd ON bl.LetterID = bd.LetterID ");
            sql.Append("WHERE w.CurrentState = '5' AND a.AdvanceType = 'FR' ");
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
          
            //parameterBuilder.AddParameterData("year", typeof(int), year);
            //parameterBuilder.FillParameters(query);
            if (param.RequestDateOfAdvance != null)
            {
                sql.Append("AND a.RequestDateOfAdvance = :requestDateOfAdvance ");
                parameterBuilder.AddParameterData("requestDateOfAdvance", typeof(DateTime), param.RequestDateOfAdvance);
            }
            if (!string.IsNullOrEmpty(param.CompanyCode))
            {
                sql.Append("AND c.CompanyCode IN (" + param.CompanyCode + ") " );
            }
            if (!string.IsNullOrEmpty(param.LetterNo))
            {
                sql.Append("AND bd.LetterNo = :LetterNo " );
                parameterBuilder.AddParameterData("LetterNo", typeof(string), param.LetterNo);
            }
            else if (!param.IsIncludeGeneratedLetter)
            {
                sql.Append("AND bd.LetterNo is null ");
            }

            sql.Append("group by d.DocumentNo,d.DocumentID ,s.EmployeeName,a.RequestDateOfAdvance,c.CompanyID,bd.LetterNo ");

            if (!string.IsNullOrEmpty(sortExpression))
            {
                sql.Append("Order by " + sortExpression);
            }
            else  if(!isCount) 
            {
                sql.Append("Order by d.DocumentNo");
            }

            if (isCount)
            {
                sql.Append(") table1");
            }

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            parameterBuilder.FillParameters(query);


            if (!isCount)
            {
                query.AddScalar("DocumentID", NHibernateUtil.Int64);
                query.AddScalar("DocumentNo", NHibernateUtil.String);
                query.AddScalar("CompanyID", NHibernateUtil.Int64);
                query.AddScalar("EmployeeName", NHibernateUtil.String);
                query.AddScalar("Amount", NHibernateUtil.Double);
                query.AddScalar("RequestDateOfAdvance", NHibernateUtil.DateTime);
                query.AddScalar("LetterNo", NHibernateUtil.String);
                query.SetResultTransformer(Transformers.AliasToBean(typeof(MoneyRequestSearchResult)));
            }
            else
            {
                query.AddScalar("MoneyRequestCount", NHibernateUtil.Int32);
                query.UniqueResult();
            }
            return query;
        }

        public IList<MoneyRequestSearchResult> GetMoneyRequestList(MoneyRequestSearchResult criteria, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<MoneyRequestSearchResult>(ScgDbQueryProvider.DbMoneyRequestQuery, "FindMoneyBuyingRequestDocument", new object[] { criteria, false, sortExpression }, firstResult, maxResult, sortExpression);
        }

        public int CountMoneyRequestByCriteria(MoneyRequestSearchResult criteria)
        {
            return NHibernateQueryHelper.CountByCriteria(ScgDbQueryProvider.DbMoneyRequestQuery, "FindMoneyBuyingRequestDocument", new object[] { criteria, true, string.Empty });
        }



        public ISQLQuery FindMoneySellingRequestDocument(SellingRequestLetterParameter param, bool isCount, string sortExpression)
        {
            StringBuilder sql = new StringBuilder();

            if (isCount)
            {
                sql.Append("SELECT count(*) as MoneyRequestCount ");
            }
            else
            {
                sql.Append("SELECT SUM(b.ForeignCurrencyRemitted) as Amount, d.DocumentNo ,s.EmployeeName as RequestName,bd.LetterNo,d.DocumentID ");
            }
            sql.Append("From FnRemittance a  ");
            sql.Append("INNER JOIN FnRemittanceItem b on a.RemittanceID = b.RemittanceID ");
            sql.Append("INNER JOIN Document d ON d.DocumentID = a.DocumentID ");
            sql.Append("INNER JOIN DbCompany c  ON d.CompanyID = c.CompanyID  ");
            sql.Append("INNER JOIN WorkFlow w ON w.DocumentID = a.DocumentID ");
            sql.Append("INNER JOIN SuUser s  ON s.UserID = d.RequesterID  ");
            sql.Append("LEFT JOIN DbSellingLetter bl ON bl.DocumentID = d.DocumentID ");
            sql.Append("LEFT JOIN DbSellingLetterDetail bd ON bl.LetterID = bd.LetterID ");
            sql.Append("where w.CurrentState = '21' and w.WorkFlowTypeID = '5' ");

            if (!string.IsNullOrEmpty(param.CompanyCode))
            {
                sql.Append("and c.CompanyCode IN (" + param.CompanyCode+") ");
            }
            if (!string.IsNullOrEmpty(param.LetterNo))
            {
                sql.Append("AND bd.LetterNo =  '" + param.LetterNo + "' ");
            }
            else if (!param.IsIncludeGeneratedLetter)
            {
                sql.Append("AND bd.LetterNo is null ");
            }
            if (!isCount)
            {
                sql.Append("group by d.DocumentNo,s.EmployeeName,c.CompanyCode,bd.LetterNo,d.DocumentID ");
            }
            if (!string.IsNullOrEmpty(sortExpression) ) 
            {
                sql.Append("Order by " + sortExpression);
            }
            else if(!isCount)
            {
                sql.Append("Order by d.DocumentNo");
            }

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            if (!isCount)
            {
                query.AddScalar("Amount", NHibernateUtil.Double);
                query.AddScalar("DocumentNo", NHibernateUtil.String);
                query.AddScalar("RequestName", NHibernateUtil.String);
                query.AddScalar("LetterNo", NHibernateUtil.String);
                query.AddScalar("DocumentID", NHibernateUtil.Int64);
                query.SetResultTransformer(Transformers.AliasToBean(typeof(SellingDetailResult)));
            }
            else
            {
                query.AddScalar("MoneyRequestCount", NHibernateUtil.Int32);
                query.UniqueResult<Int32>();
            }

            return query;
        }

        public IList<SellingDetailResult> GetMoneySellingRequestList(SellingRequestLetterParameter criteria, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<SellingDetailResult>(ScgDbQueryProvider.DbMoneyRequestQuery, "FindMoneySellingRequestDocument", new object[] { criteria, false, sortExpression }, firstResult, maxResult, sortExpression);
        }

        public int CountMoneySellingRequestByCriteria(SellingRequestLetterParameter criteria)
        {
            return NHibernateQueryHelper.CountByCriteria(ScgDbQueryProvider.DbMoneyRequestQuery, "FindMoneySellingRequestDocument", new object[] { criteria, true, string.Empty });
        }


    }
}
