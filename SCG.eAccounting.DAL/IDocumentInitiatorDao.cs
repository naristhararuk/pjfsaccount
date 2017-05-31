
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SCG.eAccounting.DTO;
using SCG.eAccounting.DTO.ValueObject;
using System.Data;

namespace SCG.eAccounting.DAL
{
    public interface IDocumentInitiatorDao : IDao<DocumentInitiator,long>
    {
      DocumentInitiator FindByInitiatorSequence(short Sequence);
      IList<DocumentInitiator> FindByDocumentID(long DocumentID);
      void Persist(DataTable dtDocumentInitiator);
      void DeleteDocumentInitiatorByDocumentID(long documentId);
    }
}
