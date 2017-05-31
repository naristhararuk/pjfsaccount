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
    public interface IBapiacpc09Query : IQuery<Bapiacpc09, long>
    {
        IList<Bapiacpc09> FindByDocNo(string DocNo);
        void DeleteByDocNo(string DocNo);
    }
}
