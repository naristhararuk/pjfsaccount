using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service;
using SS.Standard.Data.Query;
using SS.Standard.Data.Query.NHibernate;
using NHibernate;
using SCG.eAccounting.SAP.DTO;
using SCG.eAccounting.SAP.DTO.ValueObject;

namespace SCG.eAccounting.SAP.Query.Interface
{
    public interface IBapiret2Query : IQuery<Bapiret2, long>
    {
        IList<Bapiret2> FindByDocID(long DocId, string DocKind);
        IList<Bapiret2> FindByDocID(long DocId, string DocSeq, string DocKind);
        void DeleteByDocID(long DocId, string DocKind);

        ISQLQuery FindBapiret2ByCriteria(SapLog ret2, bool isCount, string sortExpression, string DocNo, string PostingDate, string Type);
        IList<SapLog> GetBapiret2ByCriteria(SapLog ret2, int firstResult, int maxResult, string sortExpression, string DocNo, string PostingDate, string Type);
        int CountBapiret2ByCriteria(SapLog ret2, string DocNo, string PostingDate, string Type);
    }
}
