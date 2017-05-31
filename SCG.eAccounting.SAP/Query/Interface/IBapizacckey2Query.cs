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
    public interface IBapizacckey2Query : IQuery<Bapizacckey2, long>
    {
        IList<Bapizacckey2> FindByDocNo(string DocNo);
        void DeleteByDocNo(string DocNo);
    }
}
