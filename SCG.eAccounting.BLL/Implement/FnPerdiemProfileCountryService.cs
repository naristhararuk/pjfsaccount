using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service.Implement;
using SCG.eAccounting.DTO;
using SS.Standard.Data.NHibernate.Dao;
using SCG.eAccounting.DAL;
using SS.Standard.Utilities;

namespace SCG.eAccounting.BLL.Implement
{
    public partial class FnPerdiemProfileCountryService : ServiceBase<FnPerdiemProfileCountry, long>, IFnPerdiemProfileCountryService
    {
        public override IDao<FnPerdiemProfileCountry, long> GetBaseDao()
        {
            return ScgeAccountingDaoProvider.FnPerdiemProfileCountryDao;
        }
        public void AddFnPerdiemProfileCountry(FnPerdiemProfileCountry ppc)
        {
            CheckDataValueUpdate(ppc);
            ScgeAccountingDaoProvider.FnPerdiemProfileCountryDao.Save(ppc);
        }
        private void CheckDataValueUpdate(FnPerdiemProfileCountry ppc)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (ppc.ZoneID == 0)
            {
                errors.AddError("ForeignPerdiemRateProfileCountry.Error", new Spring.Validation.ErrorMessage("Zone Required"));
            }
            if (ppc.CountryID == 0)
            {
                errors.AddError("ForeignPerdiemRateProfileCountry.Error", new Spring.Validation.ErrorMessage("Country Required"));
            }
            if (ScgeAccountingDaoProvider.FnPerdiemProfileCountryDao.IsDuplicateCode(ppc))
            {
                errors.AddError("FnPerdiemProfile.Error", new Spring.Validation.ErrorMessage("Duplicate"));
            }
            
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

        }
    }
}
