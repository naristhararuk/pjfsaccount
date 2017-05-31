using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service;
using SS.Standard.Data.Query;
using SS.Standard.Data.Query.NHibernate;
using NHibernate;
using SCG.eAccounting.SAP.DTO;

namespace SCG.eAccounting.SAP.Query.Interface
{
    public interface IBapiactx09Query : IQuery<Bapiactx09, long>
    {
        IList<Bapiactx09> FindByDocID(long DocId, string DocKind);
        IList<Bapiactx09> FindByDocID(long DocId, string DocSeq, string DocKind);
        void DeleteByDocID(long DocId, string DocKind);
    }
}
