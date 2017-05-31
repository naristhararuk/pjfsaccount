using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Query;
using SCG.eAccounting.DTO;
using SCG.eAccounting.DTO.ValueObject;

namespace SCG.eAccounting.Query
{
    public interface IFnPerdiemProfileCountryQuery : IQuery<FnPerdiemProfileCountry, long>
    {
        IList<PerdiemProfileCountryValObj> FindByPerdiemProfileID(long perdiemProfileID, short languageID);
        FnPerdiemProfileCountry FindByID(long ID);
        short FindCountryZoneID(short countryID, long requesterId);
    }
}
