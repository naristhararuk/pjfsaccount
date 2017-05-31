using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;

using SCG.DB.DTO;
using SCG.DB.BLL;
using SCG.DB.DAL;
using SS.Standard.Utilities;

namespace SCG.DB.BLL.Implement
{
    public partial class DbCountryService : ServiceBase<DbCountry, short>, IDbCountryService
    {
        public override IDao<DbCountry, short> GetBaseDao()
        {
            return ScgDbDaoProvider.DbCountryDao;
        }
        public short AddCountry(DbCountry country)
        {
            #region Validate SuAnnouncementGroup
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            if (string.IsNullOrEmpty(country.CountryCode))
            {
                errors.AddError("Country.Error", new Spring.Validation.ErrorMessage("Country_CodeRequired"));
            }
            if (ScgDbDaoProvider.DbCountryDao.IsDuplicateProgramCode(country))
            {
                errors.AddError("Country.Error", new Spring.Validation.ErrorMessage("UniqueCountryCode"));
            }

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
           
           return ScgDbDaoProvider.DbCountryDao.Save(country);
            
               
            #endregion
           
        }

        public void UpdateCountry(DbCountry country)
        {
            ScgDbDaoProvider.DbCountryDao.SaveOrUpdate(country);
        }

        public short FindCountryId(string countryCode)
        {
            return  ScgDbDaoProvider.DbCountryDao.FindCountryId(countryCode);
        }
    }
}
