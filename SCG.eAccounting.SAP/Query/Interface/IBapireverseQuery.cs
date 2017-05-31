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
    public interface IBapireverseQuery : IQuery<Bapireverse, long>
    {
        IList<Bapireverse> FindByDocID(long DocId, string DocKind);
        IList<Bapireverse> FindByDocID(long DocId, string DocSeq, string DocKind);
        void DeleteByDocID(long DocId, string DocKind);
    }
}
