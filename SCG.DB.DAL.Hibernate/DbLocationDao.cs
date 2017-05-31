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
    public partial class DbLocationDao : NHibernateDaoBase<DbLocation, long>, IDbLocationDao
    {
        public DbLocationDao()
        { 
        
        }
        public bool IsDuplicateLocationCode(DbLocation location)
        {
            IList<DbLocation> list = GetCurrentSession().CreateQuery("from DbLocation l where l.LocationID <> :LocationID and l.LocationCode = :LocationCode")
                  .SetInt64("LocationID", location.LocationID)
                  .SetString("LocationCode", location.LocationCode)
                  .List<DbLocation>();
            if (list.Count > 0)
            {
                return true;
            }
            return false;
        }
        public void SyncNewLocation()
        {
            ISQLQuery query = GetCurrentSession().CreateSQLQuery("exec [dbo].[SyncNewLocationData]");
            query.AddScalar("count", NHibernateUtil.Int32).UniqueResult();
        }
        public void SyncUpdateLocation(string locationCode)
        {
            ISQLQuery query = GetCurrentSession().CreateSQLQuery("exec [dbo].[SyncUpdateLocationData] :locationCode ");
            query.SetString("locationCode", locationCode);
            query.AddScalar("count", NHibernateUtil.Int32).UniqueResult();
        }
        public void SyncDeleteLocation(string locationCode)
        {
            ISQLQuery query = GetCurrentSession().CreateSQLQuery("exec [dbo].[SyncDeleteLocationData] :locationCode ");
            query.SetString("locationCode", locationCode);
            query.AddScalar("count", NHibernateUtil.Int32).UniqueResult();
        }
        //public short FindCountryId(string countryCode)
        //{
        //    IList<DbCountry> list = GetCurrentSession().CreateQuery("from DbCountry p where p.CountryCode = :CountryCode")
        //          .SetString("CountryCode", countryCode)
        //          .List<DbCountry>();
        //    short countyId = -1;
        //    if (list.Count > 0)
        //    {
        //        countyId = list.ElementAt<DbCountry>(0).CountryID;
        //    }
        //    return countyId;
        //}
    }
}
