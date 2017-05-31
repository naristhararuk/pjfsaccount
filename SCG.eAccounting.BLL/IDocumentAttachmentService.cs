using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using SS.Standard.Data.NHibernate.Service;

using SCG.eAccounting.DTO;
using System.Data;

namespace SCG.eAccounting.BLL
{
    public interface IDocumentAttachmentService : IService<DocumentAttachment, long>
    {
        IList<DocumentAttachment> FindByActive(DocumentAttachment criteria, int firstResult, int maxResults, string sortExpression);
        int CountBySDocumentAttachmentCriteria(DocumentAttachment criteria);
        void AddDocumentAttachment(DocumentAttachment documentAttachment, HttpPostedFile fileStream);
        void UpdateDocumentAttachment(long id);
        void InsertDocumentAttachment(Guid txID, long documentID);
        void DeleteDocumentAttachment(Guid txID);
        
        // Manage in Transaction.
        void PrepareDataToDataset(DataSet documentDataset, long documentID);
		long AddAttachmentToTransaction(Guid TxID, DocumentAttachment documentAttachment, HttpPostedFile attachFile);
		void DeleteAttachmentFromTransaction(Guid TxID, DocumentAttachment documentAttachment);
		
		// Insert, Delete to Database.
        void SaveDocumentAttachment(Guid txID, long documentID);
        void ValidateDocumentAttachment(Guid txID, long documentID);
    }
}
