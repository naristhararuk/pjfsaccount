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
    public partial class DbCurrencyLangService : ServiceBase<DbCurrencyLang, long>, IDbCurrencyLangService
    {
        public override IDao<DbCurrencyLang, long> GetBaseDao()
        {
            return SsDbDaoProvider.DbCurrencyLangDao;
        }
        public long AddCurrencyLang(DbCurrencyLang currency)
        {
            long id = 0;
            id = SsDbDaoProvider.DbCurrencyLangDao.Save(currency);

            return id;
        }
        public void UpdateCurrencyLang(IList<DbCurrencyLang> currency)
        {

            foreach (DbCurrencyLang dbcurrency in currency)
            {
                
                SsDbDaoProvider.DbCurrencyLangDao.DeleteByCurrencyIdLanguageId(dbcurrency.Currency.CurrencyID, dbcurrency.Language.Languageid);
            }

            foreach (DbCurrencyLang c1 in currency)
            {
                DbCurrencyLang dbCurrencylang = new DbCurrencyLang();
                dbCurrencylang.Currency = SsDbDaoProvider.DbCurrencyDao.FindProxyByIdentity(c1.Currency.CurrencyID);
                dbCurrencylang.Language = SsDbDaoProvider.DbLanguageDao.FindProxyByIdentity(c1.Language.Languageid);
                dbCurrencylang.Active = c1.Active;
                dbCurrencylang.Comment = c1.Comment;
                dbCurrencylang.CreBy = c1.CreBy;
                dbCurrencylang.CreDate = DateTime.Now.Date;
                dbCurrencylang.Description = c1.Description;
                dbCurrencylang.UpdBy = c1.UpdBy;
                dbCurrencylang.UpdDate = DateTime.Now.Date;
                dbCurrencylang.UpdPgm = c1.UpdPgm;
                dbCurrencylang.MainUnit = c1.MainUnit;
                dbCurrencylang.SubUnit = c1.SubUnit;
                SsDbDaoProvider.DbCurrencyLangDao.Save(dbCurrencylang);

            }

        }
        public void AddCurrencyLang(IList<DbCurrencyLang> currency)
        {
            foreach (DbCurrencyLang c1 in currency)
            {
                DbCurrencyLang dbCurrencylang = new DbCurrencyLang();
                dbCurrencylang.Currency = SsDbDaoProvider.DbCurrencyDao.FindProxyByIdentity(c1.Currency.CurrencyID);
                dbCurrencylang.Language = SsDbDaoProvider.DbLanguageDao.FindProxyByIdentity(c1.Language.Languageid);
                dbCurrencylang.Active = c1.Active;
                dbCurrencylang.Comment = c1.Comment;
                dbCurrencylang.CreBy = c1.CreBy;
                dbCurrencylang.CreDate = DateTime.Now.Date;
                dbCurrencylang.Description = c1.Description;
                dbCurrencylang.UpdBy = c1.UpdBy;
                dbCurrencylang.UpdDate = DateTime.Now.Date;
                dbCurrencylang.UpdPgm = c1.UpdPgm;
                dbCurrencylang.MainUnit = c1.MainUnit;
                dbCurrencylang.SubUnit = c1.SubUnit;
                SsDbDaoProvider.DbCurrencyLangDao.Save(dbCurrencylang);

            }
        }
        public void DeleteCurrencyLang(IList<DbCurrencyLang> currencyLang)
        {
            foreach (DbCurrencyLang c1 in currencyLang)
            {
                SsDbDaoProvider.DbCurrencyLangDao.Delete(c1);

            }

        }
    }
}
