using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service.Implement;
using SCG.DB.DTO;
using SS.Standard.Data.NHibernate.Dao;
using SCG.DB.DAL;
using SS.Standard.Utilities;
using SCG.DB.Query;

namespace SCG.DB.BLL.Implement
{
    public partial class DbAccountService : ServiceBase<DbAccount, long>, IDbAccountService
    {
        #region public override IDao<DbAccount, long> GetBaseDao()
        public override IDao<DbAccount, long> GetBaseDao()
        {
           return ScgDbDaoProvider.DbAccountDao;
        }
        #endregion public override IDao<DbAccount, long> GetBaseDao()

        #region public void AddAccount(DbAccount dbAccount)
        public void AddAccount(DbAccount dbAccount)
        {
            #region Validate SuAnnouncementGroup
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            if (string.IsNullOrEmpty(dbAccount.AccountCode))
            {
                errors.AddError("Account.Error", new Spring.Validation.ErrorMessage("AccountCodeRequired"));
            }
            if (ScgDbDaoProvider.DbAccountDao.IsDuplicateAccountCode(dbAccount))
            {
                errors.AddError("Account.Error", new Spring.Validation.ErrorMessage("UniqueAccountCode"));
            }

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            ScgDbDaoProvider.DbAccountDao.Save(dbAccount);
            #endregion Validate SuAnnouncementGroup
        }
        #endregion public void AddAccount(DbAccount dbAccount)

        public long AddNewAccount(DbAccount account)
        {
            #region Validate SuAnnouncementGroup
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            if (string.IsNullOrEmpty(account.AccountCode))
            {
                errors.AddError("Account.Error", new Spring.Validation.ErrorMessage("AccountCodeRequired"));
            }
           
            if (ScgDbDaoProvider.DbAccountDao.FindByAccountCode(account.AccountID,account.AccountCode, account.ExpenseGroupID))
            {
                errors.AddError("Account.Error", new Spring.Validation.ErrorMessage("Expense Group Code already exist"));
            }

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
            #endregion

            return ScgDbDaoProvider.DbAccountDao.Save(account);

        }


        
        public void UpdateAccount(DbAccount account)
        {
            //expense code = account code
            #region Validate SuAnnouncementGroup
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            if (string.IsNullOrEmpty(account.AccountCode))
            {
                errors.AddError("Account.Error", new Spring.Validation.ErrorMessage("AccountCodeRequired"));
            }

            if (ScgDbDaoProvider.DbAccountDao.FindByAccountCode(account.AccountID, account.AccountCode, account.ExpenseGroupID))
            {
                errors.AddError("Account.Error", new Spring.Validation.ErrorMessage("Expense Group Code already exist"));
            }
           
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
            #endregion
            ScgDbDaoProvider.DbAccountDao.SaveOrUpdate(account);
        }

        public void DeleteAccount(DbAccount account)
        {
            ScgDbDaoProvider.DbAccountDao.Delete(account);
        }

        #region public long FindAccountId(string accountCode)
        public long FindAccountId(string accountCode)
        {
            return ScgDbDaoProvider.DbAccountDao.FindAccountId(accountCode);
        }
        #endregion public long FindAccountId(string accountCode)

        public string GetAccountCodeExpMapping(string accountCode, string expenseGroupType)
        {
            return ScgDbQueryProvider.DbAccountQuery.GetAccountCodeExpMapping(accountCode, expenseGroupType);
        }
    }
}
