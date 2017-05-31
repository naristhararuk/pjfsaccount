using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;

using SS.DB.DTO;
using SS.DB.DAL;
using NHibernate;
using NHibernate.Expression;


namespace SS.DB.DAL.Hibernate
{
    public partial class DbCurrencyLangDao : NHibernateDaoBase<DbCurrencyLang, long>, IDbCurrencyLangDao
    {
        public DbCurrencyLangDao()
        {
        }
        public void DeleteByCurrencyIdLanguageId(short currencyId, short languageId)
        {
            GetCurrentSession()
                .Delete("from DbCurrencyLang c1 where c1.Currency.CurrencyID = :currencyId and c1.Language.Languageid = :languageId"
                , new object[] { currencyId, languageId }
                , new NHibernate.Type.IType[] { NHibernateUtil.Int16, NHibernateUtil.Int16 });
        }
    }
}
