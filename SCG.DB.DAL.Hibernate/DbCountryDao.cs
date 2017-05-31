using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;

using SCG.DB.DTO;
using SCG.DB.DAL;
using NHibernate;
using NHibernate.Expression;
using NHibernate.Transform;
using SS.Standard.Data.NHibernate.QueryCreator;

namespace SCG.DB.DAL.Hibernate
{
    public partial class DbCountryDao : NHibernateDaoBase<DbCountry , short>, IDbCountryDao
    {
        public DbCountryDao()
        { 
        
        }
        public bool IsDuplicateProgramCode(DbCountry country)
        {
            IList<DbCountry> list = GetCurrentSession().CreateQuery("from DbCountry p where p.CountryID <> :CountryID and p.CountryCode = :CountryCode")
                  .SetInt64("CountryID", country.CountryID)
                  .SetString("CountryCode", country.CountryCode)
                  .List<DbCountry>();
            if (list.Count > 0)
            {
                return true;
            }
            return false;
        }
        public short FindCountryId(string countryCode)
        {
            IList<DbCountry> list = GetCurrentSession().CreateQuery("from DbCountry p where p.CountryCode = :CountryCode")
                  .SetString("CountryCode", countryCode)
                  .List<DbCountry>();
            short countyId = -1;
            if (list.Count > 0)
            {
                countyId = list.ElementAt<DbCountry>(0).CountryID;
            }
            return countyId;
        }
    }
}
