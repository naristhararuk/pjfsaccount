using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.Query.NHibernate;
using SCG.DB.DTO;

namespace SCG.DB.Query
{
    public interface IDbBuQuery : IQuery<DbBU, string> 
    {
        IList<DbBU> FindBUALL();
    }
}
