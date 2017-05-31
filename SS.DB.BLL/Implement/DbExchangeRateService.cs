using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;

using SS.DB.DTO;
using SS.DB.BLL;
using SS.DB.DAL;

namespace SS.DB.BLL.Implement
{
    public partial class DbExchangeRateService : ServiceBase<DbExchangeRate, short>, IDbExchangeRateService
    {
        public override IDao<DbExchangeRate, short> GetBaseDao()
        {
            return SsDbDaoProvider.DbExchangeRateDao;
        }

        public short AddExchangeRate(DbExchangeRate exchangeRate)
        {
            #region Validate SuAnnouncementGroup
            //Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            //if (exchangeRate.FromDate == null)
            //{
            //    errors.AddError("Currency.Error", new Spring.Validation.ErrorMessage("Currency_SymbolRequired"));
            //}
            //if (DbProvider.DbCurrencyDao.IsDuplicateSymbol(currency))
            //{
            //    errors.AddError("Currency.Error", new Spring.Validation.ErrorMessage("UniqueSymbol"));
            //}
            //if (!errors.IsEmpty) throw new ServiceValidationException(errors);
            #endregion

            return SsDbDaoProvider.DbExchangeRateDao.Save(exchangeRate);
        }
        public void UpdateExchangeRate(DbExchangeRate exchangeRate)
        {
            #region Validate SuAnnouncementGroup
            //Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            //if (string.IsNullOrEmpty(currency.Symbol))
            //{
            //    errors.AddError("Currency.Error", new Spring.Validation.ErrorMessage("Currency_SymbolRequired"));
            //}
            //if (DbProvider.DbCurrencyDao.IsDuplicateSymbol(currency))
            //{
            //    errors.AddError("Currency.Error", new Spring.Validation.ErrorMessage("UniqueSymbol"));
            //}

            //if (!errors.IsEmpty) throw new ServiceValidationException(errors);
            #endregion

            SsDbDaoProvider.DbExchangeRateDao.SaveOrUpdate(exchangeRate);
        }
    }
}
