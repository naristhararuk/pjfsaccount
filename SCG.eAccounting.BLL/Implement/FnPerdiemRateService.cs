using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service.Implement;
using SCG.eAccounting.DTO;
using SCG.eAccounting.DAL;
using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Utilities;

namespace SCG.eAccounting.BLL.Implement
{
    public class FnPerdiemRateService : ServiceBase<FnPerdiemRate, long>, IFnPerdiemRateService
    {
        public override IDao<FnPerdiemRate, long> GetBaseDao()
        {
            return ScgeAccountingDaoProvider.FnPerdiemRateDao;
        }
        public void AddFnPerdiemRate(FnPerdiemRate fnPerdiemRate)
        {
            CheckDataValueUpdate(fnPerdiemRate, true);
            ScgeAccountingDaoProvider.FnPerdiemRateDao.Save(fnPerdiemRate);
        }

        public void UpdateFnPerdiemRate(FnPerdiemRate fnPerdiemRate)
        {
            CheckDataValueUpdate(fnPerdiemRate, false);
            ScgeAccountingDaoProvider.FnPerdiemRateDao.Update(fnPerdiemRate);
        }
        public void DeleteFnPerdiemRate(FnPerdiemRate fnPerdiemRate)
        {
            ScgeAccountingDaoProvider.FnPerdiemRateDao.Delete(fnPerdiemRate);
        }
        private void CheckDataValueUpdate(FnPerdiemRate fnPerdiemRate, bool newmode)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (string.IsNullOrEmpty(fnPerdiemRate.PersonalLevel))
            {
                errors.AddError("ForeignPerdiemRateProfileDetail.Error", new Spring.Validation.ErrorMessage("Perdiem Profile Name Required"));
            }
             if (fnPerdiemRate.ZoneID==-1)
            {
                errors.AddError("ForeignPerdiemRateProfileDetail.Error", new Spring.Validation.ErrorMessage("Zone Required"));
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

        }
    }
}
