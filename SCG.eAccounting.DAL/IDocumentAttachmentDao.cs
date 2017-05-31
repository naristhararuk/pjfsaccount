using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using SS.Standard.Data.NHibernate.Dao;

using SCG.eAccounting.DTO;
using NHibernate;

namespace SCG.eAccounting.DAL
{
    public interface IDocumentAttachmentDao : IDao<DocumentAttachment, long>
    {
        ICriteria FindByActive(DocumentAttachment documentAttachment);
        void DeleteByDocumentId(long documentid);
        void Persist(DataTable dtDocumentAttachment);
    }
}
