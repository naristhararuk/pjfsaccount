using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.DB.DTO;
using SS.Standard.Data.NHibernate.Dao;
using NHibernate;

namespace SCG.DB.DAL.Hibernate
{
    public partial class DbAccountLangDao : NHibernateDaoBase<DbAccountLang, long>, IDbAccountLangDao
    {
        #region public void DeleteAllProvinceLang(long accountID)
        public void DeleteAllAccountLang(long accountID)
        {
            GetCurrentSession()
            .Delete(" FROM DbAccountLang a WHERE a.AccountID = :AccountID ",
            new object[] { accountID },
            new NHibernate.Type.IType[] { NHibernateUtil.Int32 });
        }
        #endregion public void DeleteAllProvinceLang(long accountID)

        public IList<SCG.DB.DTO.ValueObject.AccountLang> FindByAccountId(long AccountId)
        {
            return ScgDbDaoProvider.DbAccountLangDao.FindByAccountId(AccountId);
        }
        public void DeleteByAccountIdLanguageId(long accountId, short languageId)
        {
            GetCurrentSession()
                .Delete("from DbAccountLang ac1 where ac1.Account.AccountID = :accountId and ac1.LanguageID.Languageid = :languageId"
                , new object[] { accountId, languageId }
                , new NHibernate.Type.IType[] { NHibernateUtil.Int64, NHibernateUtil.Int16 });
        }
    }
}
