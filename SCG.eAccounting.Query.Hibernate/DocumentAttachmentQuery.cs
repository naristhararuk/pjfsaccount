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

namespace SCG.eAccounting.Query.Hibernate
{
    public class DocumentAttachmentQuery : NHibernateQueryBase<DocumentAttachment, short>, IDocumentAttachmentQuery
    {
        #region IDocumentAttachmentQuery Members

        public IList<DocumentAttachment> FindByActive()
        {
            StringBuilder strQuery = new StringBuilder();
            strQuery.AppendLine(" SELECT AttachmentID,DocumentID,AttachFileName,AttachFilePath,Active ");
            strQuery.AppendLine(" FROM DocumentAttachment ");
            strQuery.AppendLine(" Where Active = 1 ");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());

            query.AddScalar("AttachmentID", NHibernateUtil.Int64);
            //query.AddScalar("DocumentID", NHibernateUtil.Int16);
            query.AddScalar("AttachFileName", NHibernateUtil.String);
            query.AddScalar("AttachFilePath", NHibernateUtil.String);
            query.AddScalar("Active", NHibernateUtil.Boolean);

            IList<DocumentAttachment> list = query.SetResultTransformer(Transformers.AliasToBean(typeof(DocumentAttachment))).List<DocumentAttachment>();

            return list;
        }

        #endregion

        public IList<DocumentAttachment> GetDocumentAttachmentByDocumentID(long documentID)
        {
            //ICriteria criteria = GetCurrentSession().CreateCriteria(typeof(DocumentAttachment),"a");
            //criteria.Add(Expression.And(Expression.Eq("a.DocumentID.DocumentID",documentID),Expression.Eq("a.Active",true)));
            //return criteria.List<DocumentAttachment>();

            string sql = @"select * from DocumentAttachment with (nolock) where Active = 1 and DocumentID = :DocumentID";
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql);
            query.SetInt64("DocumentID", documentID);
            query.AddScalar("AttachmentID", NHibernateUtil.Int64);
            query.AddScalar("AttachFileName", NHibernateUtil.String);
            query.AddScalar("AttachFilePath", NHibernateUtil.String);
            query.AddScalar("Active", NHibernateUtil.Boolean);
            query.AddScalar("CreBy", NHibernateUtil.Int64);
            query.AddScalar("CreDate", NHibernateUtil.DateTime);
            query.AddScalar("UpdBy", NHibernateUtil.Int64);
            query.AddScalar("UpdDate", NHibernateUtil.DateTime);
            query.AddScalar("UpdPgm", NHibernateUtil.String);
            return query.SetResultTransformer(Transformers.AliasToBean(typeof(DocumentAttachment))).List<DocumentAttachment>();
        }
    }
}
