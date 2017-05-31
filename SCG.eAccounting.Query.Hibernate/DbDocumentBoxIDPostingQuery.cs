using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.QueryDao;
using SCG.eAccounting.DTO;
using NHibernate;
using NHibernate.Transform;
using SCG.eAccounting.DTO.ValueObject;
using SS.Standard.Data.NHibernate.QueryCreator;
namespace SCG.eAccounting.Query.Hibernate
{
    public class DbDocumentBoxIDPostingQuery : NHibernateQueryBase<DbDocumentBoxidPosting, long>, IDbDocumentBoxIDPostingQuery
    {
        public IList<ExportBoxID> GetExportBoxIDList(string sapCode)
        {
            StringBuilder sqlBuider = new StringBuilder();
            sqlBuider.Append("SELECT doc.DocumentID,");
            sqlBuider.Append("expDoc.BoxID,");
            sqlBuider.Append("Doc.DocumentDate,");
            sqlBuider.Append("Com.CompanyCode,");
            sqlBuider.Append("docBoxPost.Status,");
            sqlBuider.Append("imgDocPost.FIDocNumber,");
            sqlBuider.Append("imgDocPost.ImageDocID ");
            sqlBuider.Append("FROM   FnExpenseDocument expDoc ");
            sqlBuider.Append("LEFT JOIN [Document] doc     ");
            sqlBuider.Append("ON doc.DocumentID = expDoc.DocumentID ");
            sqlBuider.Append("LEFT JOIN DbCompany com ");
            sqlBuider.Append("ON com.CompanyID = doc.CompanyID  ");
            sqlBuider.Append("LEFT JOIN DbDocumentBoxIDPosting docBoxPost ");
            sqlBuider.Append("ON docBoxPost.DocumentID = expDoc.DocumentID ");
            sqlBuider.Append("INNER JOIN DbDocumentImagePosting imgDocPost ");
            sqlBuider.Append("ON imgDocPost.DocumentID = expDoc.DocumentID ");
            sqlBuider.Append("WHERE expDoc.BoxID <> '' ");
            sqlBuider.Append("AND (docBoxPost.Status is null or docBoxPost.Status = 'F') ");
            sqlBuider.Append("AND imgDocPost.Status = 'S' ");
            sqlBuider.Append("AND com.SapCode = :sapCode ");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuider.ToString());
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            parameterBuilder.AddParameterData("sapCode", typeof(string), sapCode);
            parameterBuilder.FillParameters(query);
            query.AddScalar("DocumentID", NHibernateUtil.Int64)
                .AddScalar("BoxID", NHibernateUtil.String)
                .AddScalar("DocumentDate", NHibernateUtil.Date)
                .AddScalar("CompanyCode", NHibernateUtil.String)
                .AddScalar("Status", NHibernateUtil.String)
                .AddScalar("FIDocNumber", NHibernateUtil.String)
                     .AddScalar("ImageDocID", NHibernateUtil.String);
            query.SetResultTransformer(Transformers.AliasToBean(typeof(ExportBoxID)));
            return query.List<ExportBoxID>();
        }
    }
}
