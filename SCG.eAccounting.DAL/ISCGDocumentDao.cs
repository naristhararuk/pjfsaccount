using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;

using SCG.eAccounting.DTO;
using System.Data;
using SCG.eAccounting.DTO.ValueObject;

namespace SCG.eAccounting.DAL
{
	public interface ISCGDocumentDao : IDao<SCGDocument, long>
	{
        long Persist(DataTable dtDocument);
        void UpdateMarkDocument(IList<ReimbursementReportValueObj> obj);
	}
}
