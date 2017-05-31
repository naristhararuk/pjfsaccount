using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Query;

using SS.DB.DTO;
using SS.DB.DTO.ValueObject;

using NHibernate;

namespace SS.DB.Query
{
    public interface IDbCurrencyQuery : IQuery<DbCurrency, short>
    {

        ISQLQuery FindByCurrencyCriteria(DbCurrency currency, string sortExpression, bool isCount);
        IList<DbCurrency> GetCurrencyList(DbCurrency currency, int firstResult, int maxResult, string sortExpression);
        int CountByCurrencyCriteria(DbCurrency currency);

        DbCurrency FindByCurrencySymbol(string symbol, bool isExpense, bool isAdvanceFR);

        IList<TranslatedListItem> GetTranslatedListItem();
        int CountCurrencyByCriteria(VOUCurrencySetup criteria);
        ISQLQuery FindCurrencyByCriteria(VOUCurrencySetup criteria, bool isCount, string sortExpression);
        IList<VOUCurrencySetup> GetCurrencyListByCriteria(VOUCurrencySetup criteria, int firstResult, int maxResult, string sortExpression);
        DbCurrency FindCurrencyById(short CurrencyID);
        IList<VOUCurrencySetup> GetCurrencyListItem(string prefix, CurrencyAutoCompleteParameter param);
        VOUCurrencySetup GetCurrencyLangByCurrencyID(short currencyId, short languageId);

    }
}
