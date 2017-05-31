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
using System.Collections;
using SS.Standard.Utilities;

namespace SCG.DB.Query.Hibernate
{
    public class DbPaymentMethodQuery : NHibernateQueryBase<DbPaymentMethod, long>, IDbPaymentMethodQuery
    {
        public IList<DbPaymentMethod> FindPaymentMethodNotAdd(IList<string> paymentMethodIDList)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            ISQLQuery query = null;
            sqlBuilder.Append("SELECT PaymentMethodID , PaymentMethodCode ,Active ");
            sqlBuilder.Append("FROM DbPaymentMethod ");
            if (paymentMethodIDList.Count == 0)
            {
                query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            }
            else
            {
                sqlBuilder.Append("WHERE PaymentMethodID NOT IN (:PaymentMethodIdList) ");
                query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
                query.SetParameterList("PaymentMethodIdList", paymentMethodIDList);
            }
            query.AddScalar("Active", NHibernateUtil.Boolean);
            query.AddScalar("PaymentMethodID", NHibernateUtil.Int64);
            query.AddScalar("PaymentMethodCode", NHibernateUtil.String);

            IList<DbPaymentMethod> dbPyamentMethodList = query.SetResultTransformer(Transformers.AliasToBean(typeof(DbPaymentMethod))).List<DbPaymentMethod>();

            return dbPyamentMethodList;
        }

        public DbPaymentMethod FindPaymentMethodByID(long paymentMethodID)
        {
            ICriteria criteria = GetCurrentSession().CreateCriteria(typeof(DbPaymentMethod), "p");
            criteria.Add(Expression.Eq("p.PaymentMethodID", paymentMethodID));

            return criteria.UniqueResult<DbPaymentMethod>();
        }

        public ISQLQuery FindPaymentMethodByCriteria(DbPaymentMethod paymentMethod, bool isCount, string sortExpression)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            if (!isCount)
            {
                sqlBuilder.Append("SELECT c.PaymentMethodID AS PaymentMethodID, c.PaymentMethodCode AS PaymentMethodCode, c.PaymentMethodName AS PaymentMethodName, c.Active AS Active ");
                sqlBuilder.Append("FROM DbPaymentMethod c ");
                sqlBuilder.Append("WHERE c.PaymentMethodCode Like :PaymentMethodCode ");
                sqlBuilder.Append("AND c.PaymentMethodName Like :PaymentMethodName ");

                if (string.IsNullOrEmpty(sortExpression))
                    sqlBuilder.AppendLine("ORDER BY c.PaymentMethodCode,c.PaymentMethodName,c.Active ");
                else
                    sqlBuilder.AppendLine(string.Format(" ORDER BY {0}", sortExpression));

            }
            else
            {
                sqlBuilder.Append("SELECT COUNT(PaymentMethodID) AS PaymentMethodCount ");
                sqlBuilder.Append("FROM DbPaymentMethod c ");
                sqlBuilder.Append("WHERE c.PaymentMethodCode Like :PaymentMethodCode ");
                sqlBuilder.Append("AND c.PaymentMethodName Like :PaymentMethodName ");
            }

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("PaymentMethodCode", typeof(string), string.Format("%{0}%", paymentMethod.PaymentMethodCode));
            queryParameterBuilder.AddParameterData("PaymentMethodName", typeof(string), string.Format("%{0}%", paymentMethod.PaymentMethodName));
            queryParameterBuilder.FillParameters(query);

            if (!isCount)
            {
                query.AddScalar("PaymentMethodID", NHibernateUtil.Int64);
                query.AddScalar("PaymentMethodCode", NHibernateUtil.String);
                query.AddScalar("PaymentMethodName", NHibernateUtil.String);
                query.AddScalar("Active", NHibernateUtil.Boolean);

                query.SetResultTransformer(Transformers.AliasToBean(typeof(DbPaymentMethod)));
            }
            else
            {
                query.AddScalar("PaymentMethodCount", NHibernateUtil.Int32);
                query.UniqueResult();
            }
            return query;
        }

        public int CountPaymentMethodByCriteria(DbPaymentMethod paymentMethod)
        {
            return NHibernateQueryHelper.CountByCriteria(ScgDbQueryProvider.DbPaymentMethodQuery, "FindPaymentMethodByCriteria", new object[] { paymentMethod, true, string.Empty });
        }

        public IList<DbPaymentMethod> GetPaymentMethodList(DbPaymentMethod paymentMethod, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<DbPaymentMethod>(ScgDbQueryProvider.DbPaymentMethodQuery, "FindPaymentMethodByCriteria", new object[] { paymentMethod, false, sortExpression }, firstResult, maxResult, sortExpression);
        }

        public IList<DbPaymentMethod> FindPaymentMethodActive()
        {
            StringBuilder sqlBuilder = new StringBuilder();
            ISQLQuery query = null;
            sqlBuilder.Append(" SELECT PaymentMethodID , '[' + PaymentMethodCode + ']  ' + PaymentMethodName AS PaymentMethodName ");
            sqlBuilder.Append(" FROM DbPaymentMethod ");
            sqlBuilder.Append(" WHERE Active = 'True' ");
            sqlBuilder.Append(" ORDER BY PaymentMethodCode ");
            
            query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.AddScalar("PaymentMethodID", NHibernateUtil.Int64);
            query.AddScalar("PaymentMethodName", NHibernateUtil.String);

            IList<DbPaymentMethod> dbPyamentMethodList = query.SetResultTransformer(Transformers.AliasToBean(typeof(DbPaymentMethod))).List<DbPaymentMethod>();

            return dbPyamentMethodList;
        }
        public IList<DbPaymentMethod> FindPaymentMethodActive(short ComID)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            ISQLQuery query = null;
            sqlBuilder.Append(" SELECT B.PaymentMethodID , '[' + B.PaymentMethodCode + ']  ' + B.PaymentMethodName AS PaymentMethodName ");
            sqlBuilder.Append(" FROM DbCompanyPaymentMethod AS A , DbPaymentMethod AS B ");
            sqlBuilder.Append(" WHERE A.PaymentMethodID = B.PaymentMethodID AND ");
            sqlBuilder.Append("       A.CompanyID = :CompanyID ");
            sqlBuilder.Append(" ORDER BY B.PaymentMethodCode ");

            query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.SetParameter("CompanyID", ComID);
            query.AddScalar("PaymentMethodID", NHibernateUtil.Int64);
            query.AddScalar("PaymentMethodName", NHibernateUtil.String);

            IList<DbPaymentMethod> dbPyamentMethodList = query.SetResultTransformer(Transformers.AliasToBean(typeof(DbPaymentMethod))).List<DbPaymentMethod>();

            return dbPyamentMethodList;
        }
    }
}
