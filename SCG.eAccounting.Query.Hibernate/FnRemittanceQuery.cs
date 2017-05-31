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
    public class FnRemittanceQuery : NHibernateQueryBase<FnRemittance, long>, IFnRemittanceQuery
	{
        public FnRemittance GetFnRemittanceByDocumentID(long documentID)
        {
            ICriteria criteria = GetCurrentSession().CreateCriteria(typeof(FnRemittance), "r");
            criteria.Add(Expression.Eq("r.Document.DocumentID", documentID));

            return criteria.UniqueResult<FnRemittance>();
        }
        public IList<FnRemittance> FindRemittanceReferenceTAByTADocumentID(long taDocumentID)
        {
            return GetCurrentSession().CreateQuery(" from FnRemittance where Active = 1 and isnull(TADocumentID,'') <> '' and TADocumentID = :TADocumentID ")
                .SetInt64("TADocumentID", taDocumentID)
                .List<FnRemittance>();
        }

        public IList<FnRemittance> FindRemittanceReferenceTAForRequesterByTADocumentID(long requesterID, long taDocumentID)
        {
            return GetCurrentSession().CreateQuery(@" from FnRemittance rmt where rmt.Active = 1 and rmt.TADocumentID = :TADocumentID and rmt.Document.RequesterID.Userid = :RequesterID")
                    .SetInt64("TADocumentID", taDocumentID)
                    .SetInt64("RequesterID", requesterID)
                    .List<FnRemittance>();
        }
	}
}
