using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;

using SCG.GL.DTO;
using SCG.GL.BLL;
using SCG.GL.DAL;
using SS.Standard.Utilities;

namespace SCG.GL.BLL.Implement
{
    public partial class GlAccountService : ServiceBase<GlAccount, short>, IGlAccountService
    {
        public override IDao<GlAccount, short> GetBaseDao()
        {
            return GlDaoProvider.GlAccountDao;
        }

        public void AddAccount(GlAccount account, GlAccountLang accountLang)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (string.IsNullOrEmpty(accountLang.AccountName))
            {
                errors.AddError("Account.Error", new Spring.Validation.ErrorMessage("RequiredAccountName"));
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            GlDaoProvider.GlAccountDao.Save(account);
            GlDaoProvider.GlAccountLangDao.Save(accountLang);
        }

        public void UpdateAccount(GlAccount account)
        {
            GlDaoProvider.GlAccountDao.SaveOrUpdate(account);
        }

        #region IGlAccountService Members


        public GlAccount FindAccountByAccountNo(string accNo)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
