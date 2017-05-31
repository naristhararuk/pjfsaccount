using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;

using SCG.DB.DTO;
using SCG.DB.BLL;
using SCG.DB.DAL;
using SS.Standard.Utilities;

namespace SCG.DB.BLL.Implement
{
    public partial class DbBankService : ServiceBase<DbBank, short>, IDbBankService
    {
        public override IDao<DbBank, short> GetBaseDao()
        {
            return ScgDbDaoProvider.DbBankDao;
        }
        public void AddBank(DbBank bank, DbBankLang bankLang)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (string.IsNullOrEmpty(bankLang.BankName))
            {
                errors.AddError("Bank.Error", new Spring.Validation.ErrorMessage("RequiredBankName"));
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            ScgDbDaoProvider.DbBankDao.Save(bank);
            ScgDbDaoProvider.DbBankLangDao.Save(bankLang);
        }

        public void UpdateBank(DbBank bank)
        {
            ScgDbDaoProvider.DbBankDao.SaveOrUpdate(bank);
        }
    }
}
