using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using NHibernate.Expression;
using NHibernate.Transform;

using SS.Standard.Data.NHibernate.Query;
using SS.Standard.Data.NHibernate.QueryDao;
using SS.Standard.Data.NHibernate.QueryCreator;

using SCG.eAccounting.DTO;
using SCG.eAccounting.Query;
using SCG.eAccounting.DTO.ValueObject;

namespace SCG.eAccounting.Query.Hibernate
{
    public class FnAutoPaymentQuery : NHibernateQueryBase<FnAutoPayment, long>, IFnAutoPaymentQuery
    {
        public FnAutoPayment GetFnAutoPaymentByDocumentID(long documentID)
        {
            ICriteria criteria = GetCurrentSession().CreateCriteria(typeof(FnAutoPayment), "a");
            criteria.Add(Expression.And(Expression.Eq("a.Document.DocumentID", documentID), Expression.Eq("a.Active", true)));

            return criteria.UniqueResult<FnAutoPayment>();
        }

        public long GetDocumentIDByFIDOCID(string FIDOC, string COMCODE, string YEAR)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT BAPIACHE09.DOC_ID ");
            sqlBuilder.Append("FROM BAPIACHE09 ");
            sqlBuilder.Append("WHERE BAPIACHE09.FI_DOC = :FIDOC AND BAPIACHE09.COMP_CODE = :COMCODE AND DOC_YEAR = :YEAR" );
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            QueryParameterBuilder p = new QueryParameterBuilder();
            p.AddParameterData("FIDOC", typeof(string), FIDOC);
            p.AddParameterData("COMCODE", typeof(string), COMCODE);
            p.AddParameterData("YEAR", typeof(string), YEAR);
            p.FillParameters(query);
            return query.AddScalar("DOC_ID", NHibernateUtil.Int64).UniqueResult<long>();

        }

        public IList<ExportClearing> GetExportClearingListByDate(string sapCode)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT bap.DOC_ID as DocumentID, bap.FI_DOC as FIDOC, comp.CompanyCode ,bap.doc_year as Year  ");
            sqlBuilder.Append("FROM BAPIACHE09 bap ");
            sqlBuilder.Append("INNER JOIN [Document] doc ");
            sqlBuilder.Append("ON doc.DocumentID = bap.Doc_ID ");
            sqlBuilder.Append("INNER JOIN [WorkFlow] wf ");
            sqlBuilder.Append("ON wf.DocumentID = doc.DocumentID ");
            sqlBuilder.Append("LEFT JOIN dbCompany comp ");
            sqlBuilder.Append("ON comp.CompanyID  = doc.CompanyID ");
            sqlBuilder.Append("WHERE doc.CacheCurrentStateName = 'WaitPaymentFromSAP' ");
            sqlBuilder.Append("AND bap.DOC_SEQ = 'M' ");
            sqlBuilder.Append("AND comp.SapCode = :sapCode ");
            //sqlBuilder.Append("AND NOT EXISTS  ");
            //sqlBuilder.Append("(SELECT * FROM FNAUTOPAYMENT fn  ");
            //sqlBuilder.Append("WHERE fn.FIdoc = bap.fi_doc   ");
            //sqlBuilder.Append("and fn.year = bap.doc_year   ");
            //sqlBuilder.Append("and comp.CompanyCode = fn.companycode)  ");

            //sqlBuilder.Append("AND doc.DocumentDate > '2017-04-23 00:00:00.000' "); /*n-test*/
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            parameterBuilder.AddParameterData("sapCode", typeof(string), sapCode);
            parameterBuilder.FillParameters(query);

            query.AddScalar("DocumentID", NHibernateUtil.String);
            query.AddScalar("CompanyCode", NHibernateUtil.String);
            query.AddScalar("FIDOC", NHibernateUtil.String);
            query.AddScalar("Year", NHibernateUtil.String);

            return query.SetResultTransformer(Transformers.AliasToBean(typeof(ExportClearing))).List<ExportClearing>();



        }

        public FnAutoPayment GetFnAutoPaymentSuccessByDocumentID(long documentID, int status)
        {
            ICriteria criteria = GetCurrentSession().CreateCriteria(typeof(FnAutoPayment), "a");
            criteria.Add(Expression.And(Expression.Eq("a.Document.DocumentID", documentID), Expression.Eq("a.Active", true)));
            criteria.Add(Expression.Eq("a.Status", status));

            return criteria.UniqueResult<FnAutoPayment>();
        }

    }
}
