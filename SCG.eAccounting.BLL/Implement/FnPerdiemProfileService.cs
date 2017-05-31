using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service.Implement;
using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Utilities;
using SCG.eAccounting.DAL;
using SCG.eAccounting.DTO;

namespace SCG.eAccounting.BLL.Implement
{
    public class FnPerdiemProfileService : ServiceBase<FnPerdiemProfile, long>, IFnPerdiemProfileService
    {
        public override IDao<FnPerdiemProfile, long> GetBaseDao()
        {
            return ScgeAccountingDaoProvider.FnPerdiemProfileDao;
        }
        public void AddFnPerdiemProfile(FnPerdiemProfile fnPerdiemProfile)
        {
            CheckDataValueUpdate(fnPerdiemProfile, true);
            ScgeAccountingDaoProvider.FnPerdiemProfileDao.Save(fnPerdiemProfile);
        }

        public void UpdateFnPerdiemProfile(FnPerdiemProfile fnPerdiemProfile)
        {
            CheckDataValueUpdate(fnPerdiemProfile, false);
            ScgeAccountingDaoProvider.FnPerdiemProfileDao.Update(fnPerdiemProfile);
        }
        private void CheckDataValueUpdate(FnPerdiemProfile fnPerdiemProfile, bool newmode)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (string.IsNullOrEmpty(fnPerdiemProfile.PerdiemProfileName))
            {
                errors.AddError("FnPerdiemProfile.Error", new Spring.Validation.ErrorMessage("Perdiem Profile Name Required"));
            }
            if (ScgeAccountingDaoProvider.FnPerdiemProfileDao.IsDuplicateCode(fnPerdiemProfile) && newmode)
            {
                errors.AddError("FnPerdiemProfile.Error", new Spring.Validation.ErrorMessage("Perdiem Profile Name is Duplicate"));
            }
            if (string.IsNullOrEmpty(fnPerdiemProfile.Description))
            {
                errors.AddError("FnPerdiemProfile.Error", new Spring.Validation.ErrorMessage("Description Required"));
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

        }
    }
}
