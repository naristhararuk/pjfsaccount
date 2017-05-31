using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service;
using SS.Standard.Data.Query;
using SS.Standard.Data.Query.NHibernate;
using NHibernate;
using SCG.eAccounting.SAP.DTO;
using System.Data.SqlClient;
using System.Data;
using System.Collections;

namespace SCG.eAccounting.SAP.Query.Interface
{
    public interface IBapiache09Query : IQuery<Bapiache09, long>
    {
        IList<Bapiache09> FindByDocID(long DocId, string DocKind);
        IList<Bapiache09> FindByDocID(long DocId, string DocSeq, string DocKind);

        void DeleteByDocID(long DocId, string DocKind);
    }
}
