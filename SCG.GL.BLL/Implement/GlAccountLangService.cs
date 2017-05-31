using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;

using SCG.GL.DTO;
using SCG.GL.BLL;
using SCG.GL.DAL;

namespace SCG.GL.BLL.Implement
{
    public partial class GlAccountLangService : ServiceBase<GlAccountLang, long>, IGlAccountLangService
    {
        public override IDao<GlAccountLang, long> GetBaseDao()
        {
            return GlDaoProvider.GlAccountLangDao;
        }

        public IList<SCG.GL.DTO.ValueObject.AccountLang> FindByAccountId(short accountId)
        {
            return GlDaoProvider.GlAccountLangDao.FindByAccountId(accountId);
        }
        public void UpdateAccountLang(IList<GlAccountLang> accountLangList)
        {
            if (accountLangList.Count > 0)
            {
                GlDaoProvider.GlAccountLangDao.DeleteAllAccountLang(accountLangList[0].Acc.AccId);
            }
            foreach (GlAccountLang accLang in accountLangList)
            {
                GlDaoProvider.GlAccountLangDao.Save(accLang);
            }

        }

        #region IGlAccountLangService Members


        public GlAccountLang FindAccountLangByAccountID(short accID, short languageId)
        {
            return GlDaoProvider.GlAccountLangDao.FindAccountLangByAccountID(accID, languageId);
        }

        #endregion
    }
}
