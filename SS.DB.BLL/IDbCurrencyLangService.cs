using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.DB.DTO;
using SS.Standard.Data.NHibernate.Service;

namespace SS.DB.BLL
{
    public interface IDbCurrencyLangService : IService<DbCurrencyLang, long>
    {
        long AddCurrencyLang(DbCurrencyLang currency);
        void UpdateCurrencyLang(IList<DbCurrencyLang> currency);
        void AddCurrencyLang(IList<DbCurrencyLang> currency);
        void DeleteCurrencyLang(IList<DbCurrencyLang> currencyLang);
    }
}
