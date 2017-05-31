using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service.Implement;
using SCG.DB.DTO;
using SS.Standard.Data.NHibernate.Dao;
using SCG.DB.DAL;
using SS.Standard.Utilities;

namespace SCG.DB.BLL.Implement
{
    public partial class DbPbRateService : ServiceBase<DbPbRate, long>, IDbPbRateService
    {
        public override IDao<DbPbRate, long> GetBaseDao()
        {
            return ScgDbDaoProvider.DbPbRateDao;
        }
        public long AddPbRate(DbPbRate pb)
        {
            #region Validate Add Service
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            if (string.IsNullOrEmpty(pb.EffectiveDate.Value.ToString()))
            {
                errors.AddError("PB.Error", new Spring.Validation.ErrorMessage("Date Required"));
            }
            if (string.IsNullOrEmpty(pb.MainCurrencyID.ToString()))
            {
                errors.AddError("PB.Error", new Spring.Validation.ErrorMessage("Main Currency Required"));
            }
            if (string.IsNullOrEmpty(pb.CurrencyID.ToString()))
            {
                errors.AddError("PB.Error", new Spring.Validation.ErrorMessage("Currency Required"));
            }
            if (string.IsNullOrEmpty(pb.FromAmount.ToString()))
            {
                errors.AddError("PB.Error", new Spring.Validation.ErrorMessage("Amount Required"));
            }
            if (pb.FromAmount <= 0 )
            {
                errors.AddError("PB.Error", new Spring.Validation.ErrorMessage("AmountMustMoreThenZero"));
            }
            if (string.IsNullOrEmpty(pb.ToAmount.ToString()))
            {
                errors.AddError("PB.Error", new Spring.Validation.ErrorMessage("Amount Required"));
            }
            if (pb.ToAmount <= 0)
            {
                errors.AddError("PB.Error", new Spring.Validation.ErrorMessage("AmountMustMoreThenZero"));
            }
            if (pb.EffectiveDate > DateTime.Now)
            {
                errors.AddError("PB.Error", new Spring.Validation.ErrorMessage("EffectiveDate must less than current date"));
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
            #endregion
            return ScgDbDaoProvider.DbPbRateDao.Save(pb);

        }
    }
}
