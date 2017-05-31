using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

using SCG.DB.DTO;
using SS.Standard.Data.Query;
using SS.Standard.Data.Query.NHibernate;
using NHibernate;

namespace SCG.DB.Query
{
    public interface ITmpDbCostCenterQuery :IQuery<TmpDbCostCenter ,long>
    {
        
    }
}
