using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SCG.eAccounting.DTO;
using SCG.eAccounting.DTO.ValueObject;
using SS.Standard.Data.Query;
using SS.Standard.Data.Query.NHibernate;

namespace SCG.eAccounting.Query
{
    public interface IFnPerdiemRateQuery : IQuery<FnPerdiemRate, long>
    {
        IList<PerdiemRateValObj> GetPerdiemRateByRequesterID(long requesterId);
        IList<PerdiemRateValObj> GetPerdiemRateByRequesterIDForRepOffice(long requesterId,string personalGroup);
        IList<PerdiemRateValObj> FindByPerdiemProfileID(long perdiemProfileID, short languageID);
    }
}
