using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;
using SS.Standard.Utilities;

using SCG.DB.DTO;
using SCG.DB.BLL;
using SCG.DB.DAL;
using SS.DB.DTO;

namespace SCG.DB.BLL.Implement
{
    public partial class DbAccountLangService : ServiceBase<DbAccountLang, long>, IDbAccountLangService
    {
        public override IDao<DbAccountLang, long> GetBaseDao()
        {
            return ScgDbDaoProvider.DbAccountLangDao;
        }
        public IList<SCG.DB.DTO.ValueObject.AccountLang> FindByAccountId(long AccountId)
        {
            return ScgDbDaoProvider.DbAccountLangDao.FindByAccountId(AccountId);
        }
        public IList<DbAccountLang> FindByDbAccountLangQuery(DbAccountLang criteria, long accountId, short languageId, int firstResult, int maxResults, string sortExpression)
        {
            return NHibernateDaoHelper.FindPagingByCriteria<DbAccountLang>(ScgDbDaoProvider.DbAccountLangDao, "FindByDbAccountLangQuery", new object[] { criteria, accountId, languageId }, firstResult, maxResults, sortExpression);
        }
        public int CountByDbAccountLangCriteria(DbAccountLang criteria, long accountId, short languageId)
        {
            return NHibernateDaoHelper.CountByCriteria(ScgDbDaoProvider.DbAccountLangDao, "FindByDbAccountLangQuery", new object[] { criteria, accountId, languageId });
        }
        public void UpdateAccountLang(IList<DbAccountLang> accountLangList)
        {
            
            if (accountLangList.Count > 0)
            {
                ScgDbDaoProvider.DbAccountLangDao.DeleteAllAccountLang(accountLangList[0].Account.AccountID);
            }
            foreach (DbAccountLang accountLang in accountLangList)
            {
                ScgDbDaoProvider.DbAccountLangDao.Save(accountLang);
            }
        }


        public long AddAccountLang(DbAccountLang account)
        {
            long id = 0;
            id = ScgDbDaoProvider.DbAccountLangDao.Save(account);

            return id;
        }
        public void UpdateListAccountLang(IList<DbAccountLang> account)
        {

            foreach (DbAccountLang ac in account)
            {
                ScgDbDaoProvider.DbAccountLangDao.DeleteByAccountIdLanguageId(ac.Account.AccountID, ac.LanguageID.Languageid);
            
            }

            foreach (DbAccountLang ac in account)
            {
                DbAccountLang acountLang = new DbAccountLang();
                acountLang.Account = new DbAccount(ac.Account.AccountID);
                acountLang.AccountLangID = ac.AccountLangID;
                acountLang.AccountName = ac.AccountName;
                acountLang.Active = ac.Active;
                acountLang.Comment = ac.Comment;
                acountLang.CreBy = ac.CreBy;
                acountLang.CreDate = DateTime.Now;
                acountLang.LanguageID = new DbLanguage(ac.LanguageID.Languageid);
                acountLang.UpdBy = ac.UpdBy;
                acountLang.UpdDate = DateTime.Now;
                acountLang.UpdPgm = ac.UpdPgm;

                ScgDbDaoProvider.DbAccountLangDao.Save(acountLang);
            }

        }
        public void AddListAccountLang(IList<DbAccountLang> account)
        {
            foreach (DbAccountLang ac in account)
            {
                DbAccountLang acountLang = new DbAccountLang();
                acountLang.Account = new DbAccount(ac.Account.AccountID);
                acountLang.AccountLangID = ac.AccountLangID;
                acountLang.AccountName = ac.AccountName;
                acountLang.Active = ac.Active;
                acountLang.Comment = ac.Comment;
                acountLang.CreBy = ac.CreBy;
                acountLang.CreDate = DateTime.Now;
                acountLang.LanguageID = new DbLanguage(ac.LanguageID.Languageid);
                acountLang.UpdBy = ac.UpdBy;
                acountLang.UpdDate = DateTime.Now;
                acountLang.UpdPgm = ac.UpdPgm;

                ScgDbDaoProvider.DbAccountLangDao.Save(acountLang);
            }
        }
    }
}
