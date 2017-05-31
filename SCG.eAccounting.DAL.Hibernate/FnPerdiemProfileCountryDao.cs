using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Dao;
using SCG.eAccounting.DTO;

namespace SCG.eAccounting.DAL.Hibernate
{
    public class FnPerdiemProfileCountryDao : NHibernateDaoBase<FnPerdiemProfileCountry, long>, IFnPerdiemProfileCountryDao
    {
        public bool IsDuplicateCode(FnPerdiemProfileCountry ppc)
        {
            IList<FnPerdiemProfileCountry> list = GetCurrentSession().CreateQuery("from FnPerdiemProfileCountry ppc where ppc.PerdiemProfileID = :perdiemProfileID and ppc.ZoneID = :zoneID and ppc.CountryID = :countryID ")
                  .SetInt64("perdiemProfileID", ppc.PerdiemProfileID)
                  .SetInt16("zoneID",ppc.ZoneID)
                  .SetInt16("countryID",ppc.CountryID)
                  .List<FnPerdiemProfileCountry>();
            if (list.Count > 0)
            {
                return true;
            }
            return false;
        }
    }
}
