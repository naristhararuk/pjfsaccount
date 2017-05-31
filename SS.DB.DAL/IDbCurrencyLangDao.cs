using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.DB.DTO;
using SS.Standard.Data.NHibernate.Dao;
using NHibernate;
using SS.DB.DTO;

namespace SS.DB.DAL
{
    public interface IDbCurrencyLangDao : IDao<DbCurrencyLang, long>  
    {
        //short AddCurrencyLang(DbCurrencyLang currency);
        void DeleteByCurrencyIdLanguageId(short currencyId, short languageId);
    }
}
