using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

using SCG.DB.DTO;

namespace SCG.DB.BLL
{
    public interface IDbCountryService : IService<DbCountry, short>
    {
        short AddCountry(DbCountry country);
        void UpdateCountry(DbCountry country);
        short FindCountryId(string countryCode);
    }
}
