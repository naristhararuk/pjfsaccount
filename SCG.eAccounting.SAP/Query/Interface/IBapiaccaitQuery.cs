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
    public interface IBapiaccaitQuery : IQuery<Bapiaccait, long>
    {
        IList<Bapiaccait> FindByDocNo(string DocNo);
        void DeleteByDocNo(string DocNo);
    }
}
