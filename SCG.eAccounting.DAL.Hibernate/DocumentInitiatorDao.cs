using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SCG.eAccounting.DAL;
using SCG.eAccounting.DTO;
using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.QueryCreator;
using NHibernate;
using SCG.eAccounting.DTO.ValueObject;
using NHibernate.Transform;
using System.Data;
using SCG.eAccounting.DTO.DataSet;

namespace SCG.eAccounting.DAL.Hibernate
{
    public class DocumentInitiatorDao : NHibernateDaoBase<DocumentInitiator, long>, IDocumentInitiatorDao
    {
        #region IDocumentInitiatorDao Members

        public DocumentInitiator FindByInitiatorSequence(short Sequence)
        {
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT  * FROM DocumentInitiator ");
            sqlBuilder.Append("   WHERE Seq = :Sequence ");

            parameterBuilder.AddParameterData("Sequence", typeof(short), Sequence);

           // ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.SetInt16("Sequence", Sequence);
            query.AddEntity(typeof(DocumentInitiator));
            IList<DocumentInitiator> documentInitiator = query.List<DocumentInitiator>();
            return documentInitiator.Count > 0 ? documentInitiator[0] : null;

            //throw new NotImplementedException();
        }


        public IList<DocumentInitiator> FindByDocumentID(long DocumentID)
        {
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT  * FROM DocumentInitiator ");
            sqlBuilder.Append("   WHERE DocumentID = :DocumentID ");

            parameterBuilder.AddParameterData("DocumentID", typeof(long), DocumentID);

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.SetInt64("DocumentID", DocumentID);
            query.AddEntity(typeof(DocumentInitiator));
            IList<DocumentInitiator> documentInitiator = query.List<DocumentInitiator>();
            return documentInitiator;

        }

        #endregion

        #region Save DocumentInitiator.
        public void Persist(DataTable dtDocumentInitiator)
        {
            NHibernateAdapter<DocumentInitiator, long> adapter = new NHibernateAdapter<DocumentInitiator, long>();
            adapter.UpdateChange(dtDocumentInitiator, ScgeAccountingDaoProvider.DocumentInitiatorDao);
        }
        #endregion

        public void DeleteDocumentInitiatorByDocumentID(long documentId)
        {
            GetCurrentSession().Delete(
                " from DocumentInitiator where DocumentID = :DocumentID"
                , new object[] { documentId }, new NHibernate.Type.IType[] { NHibernateUtil.Int64 });
        }
    }
}
