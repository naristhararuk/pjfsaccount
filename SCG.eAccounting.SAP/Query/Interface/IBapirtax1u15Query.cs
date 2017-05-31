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
    public interface IBapirtax1u15Query : IQuery<Bapirtax1u15, long>
    {
        IList<Bapirtax1u15> FindByDocNo(string DocNo);
        void DeleteByDocNo(string DocNo);
    }
}
