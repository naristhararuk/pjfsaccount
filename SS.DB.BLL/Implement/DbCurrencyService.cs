using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;
using SS.Standard.Utilities;

using SS.DB.DTO;
using SS.DB.DAL;
using SS.SU.BLL;

namespace SS.DB.BLL.Implement
{
    public partial class DbCurrencyService : ServiceBase<DbCurrency, short>, IDbCurrencyService
    {
        public override IDao<DbCurrency, short> GetBaseDao()
        {
            return SsDbDaoProvider.DbCurrencyDao;
        }

        public short AddCurrency(DbCurrency currency)
        {
            #region Validate SuAnnouncementGroup
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            if (string.IsNullOrEmpty(currency.Symbol))
            {
                errors.AddError("Currency.Error", new Spring.Validation.ErrorMessage("Currency_SymbolRequired"));
            }
            if (SsDbDaoProvider.DbCurrencyDao.IsDuplicateSymbol(currency))
            {
                errors.AddError("Currency.Error", new Spring.Validation.ErrorMessage("UniqueSymbol"));
            }

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
            #endregion

            return SsDbDaoProvider.DbCurrencyDao.Save(currency);
        }
        public void UpdateCurrency(DbCurrency currency)
        {
            #region Validate SuAnnouncementGroup
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            if (string.IsNullOrEmpty(currency.Symbol))
            {
                errors.AddError("Currency.Error", new Spring.Validation.ErrorMessage("Currency_SymbolRequired"));
            }
            if (SsDbDaoProvider.DbCurrencyDao.IsDuplicateSymbol(currency))
            {
                errors.AddError("Currency.Error", new Spring.Validation.ErrorMessage("UniqueSymbol"));
            }

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
            #endregion
          
            SsDbDaoProvider.DbCurrencyDao.SaveOrUpdate(currency);
        }

        public void DeleteCurrency(DbCurrency currency)
        {
            SsDbDaoProvider.DbCurrencyDao.Delete(currency);
        }
        

    }
}
