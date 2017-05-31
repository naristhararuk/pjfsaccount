using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.Query.NHibernate;
using SCG.eAccounting.DTO;

namespace SCG.eAccounting.Query
{
    public interface IFnEHRexpenseTempQuery : IQuery<FnehRexpenseTemp, long>
    {
        void ResolveID();
        void ResolvePBSERVICEID();
        void ClearTemporary();
        void CommitNewExpense();
        void ImportExpense();
    }
}
