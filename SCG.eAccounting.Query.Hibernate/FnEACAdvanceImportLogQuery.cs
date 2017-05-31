using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.QueryDao;
using SCG.eAccounting.DTO;
using SCG.eAccounting.DTO.ValueObject;
using NHibernate;
using NHibernate.Transform;
using SS.Standard.Data.NHibernate.QueryCreator;
using NHibernate.Expression;


namespace SCG.eAccounting.Query.Hibernate
{
    public class FnEACAdvanceImportLogQuery : NHibernateQueryBase<FnEACAdvanceImportLog, long>, IFnEACAdvanceImportLogQuery
    {
        public ISQLQuery FindFnEACAdvanceImportLogSearchResult(FnEACAdvanceImportLog fnEACAdvanceImportLog, string sortExpression, bool isCount)
        {
            StringBuilder strQuery = new StringBuilder();
            ISQLQuery query;
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();

            if (!isCount)
            {
                strQuery.AppendLine(" Select CreDate as CreDate , EACRequestNo as EACRequestNo , EXPRequestNo as EXPRequestNo , ");
                strQuery.AppendLine(" Status as Status , Message as Message ");
                strQuery.AppendLine(" FROM FnEACAdvanceImportLog ");
                strQuery.AppendLine(" WHERE 1=1 ");


            }
            else
            {
                strQuery.AppendLine(" select count(*) as FnEACAdvanceImportLogCount ");
                strQuery.AppendLine(" FROM FnEACAdvanceImportLog ");
                strQuery.AppendLine(" WHERE 1=1 ");
            }
            #region Criteria
            if (!string.IsNullOrEmpty(fnEACAdvanceImportLog.EACRequestNo))
            {
                strQuery.AppendLine(" AND EACRequestNo Like :EACRequestNo ");
                parameterBuilder.AddParameterData("EACRequestNo", typeof(string), string.Format("%{0}%", fnEACAdvanceImportLog.EACRequestNo));
            }
            if (!string.IsNullOrEmpty(fnEACAdvanceImportLog.EXPRequestNo))
            {
                strQuery.AppendLine(" AND EXPRequestNo Like :EXPRequestNo ");
                parameterBuilder.AddParameterData("EXPRequestNo", typeof(string), string.Format("%{0}%", fnEACAdvanceImportLog.EXPRequestNo));
            }
            if (!string.IsNullOrEmpty(fnEACAdvanceImportLog.Status) && !fnEACAdvanceImportLog.Status.Equals("All"))
            {
                strQuery.AppendLine(" AND Status Like :Status ");
                parameterBuilder.AddParameterData("Status", typeof(string), string.Format("%{0}%", fnEACAdvanceImportLog.Status));
            }
            if (!string.IsNullOrEmpty(fnEACAdvanceImportLog.Message))
            {
                strQuery.AppendLine(" AND Message Like :Message ");
                parameterBuilder.AddParameterData("Message", typeof(string), string.Format("%{0}%", fnEACAdvanceImportLog.Message));
            }
            
            #endregion

            if (isCount)
            {
                query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());
                parameterBuilder.FillParameters(query);

                query.AddScalar("FnEACAdvanceImportLogCount", NHibernateUtil.Int32);
            }
            else
            {
                if (string.IsNullOrEmpty(sortExpression))
                {
                    strQuery.AppendLine(" ORDER BY CreDate desc , EACRequestNo , EXPRequestNo , Status , Message ");
                }
                else
                {
                    strQuery.Append(string.Format(" ORDER BY {0} ", sortExpression));
                }
                query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());
                parameterBuilder.FillParameters(query);

                query.AddScalar("CreDate", NHibernateUtil.DateTime);
                query.AddScalar("EACRequestNo", NHibernateUtil.String);
                query.AddScalar("EXPRequestNo", NHibernateUtil.String);
                query.AddScalar("Status", NHibernateUtil.String);
                query.AddScalar("Message", NHibernateUtil.String);
                query.SetResultTransformer(Transformers.AliasToBean(typeof(FnEACAdvanceImportLog)));

            }

            return query;

        }

        public IList<FnEACAdvanceImportLog> FindFnEACAdvanceImportLogListQuery(FnEACAdvanceImportLog fnEACAdvanceImportLog, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<FnEACAdvanceImportLog>(
                ScgeAccountingQueryProvider.FnEACAdvanceImportLogQuery,
                "FindFnEACAdvanceImportLogSearchResult",
                new object[] { fnEACAdvanceImportLog, sortExpression, false },
                firstResult,
                maxResult,
                sortExpression);
        }

        public int GetCountFnEACAdvanceImportLoglist(FnEACAdvanceImportLog fnEACAdvanceImportLog)
        {
            return NHibernateQueryHelper.CountByCriteria(
                ScgeAccountingQueryProvider.FnEACAdvanceImportLogQuery,
                "FindFnEACAdvanceImportLogSearchResult",
                new object[] { fnEACAdvanceImportLog, string.Empty, true });
        }
    }
}
