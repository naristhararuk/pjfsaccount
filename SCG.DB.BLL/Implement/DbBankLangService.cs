using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;

using SCG.DB.DTO;
using SCG.DB.BLL;
using SCG.DB.DAL;

namespace SCG.DB.BLL.Implement
{
    public partial class DbBankLangService : ServiceBase<DbBankLang, long>, IDbBankLangService
    {
        public override IDao<DbBankLang, long> GetBaseDao()
        {
            return ScgDbDaoProvider.DbBankLangDao;
        }
        public IList<SCG.DB.DTO.ValueObject.BankLang> FindByBankId(short bankId)
        {
            return ScgDbDaoProvider.DbBankLangDao.FindByBankId(bankId);
        }
        public void UpdateBankLang(IList<DbBankLang> bankLangList)
        {
            if (bankLangList.Count > 0)
            {
                ScgDbDaoProvider.DbBankLangDao.DeleteAllBankLang(bankLangList[0].Bank.Bankid);
            }
            foreach (DbBankLang bankLang in bankLangList)
            {
                ScgDbDaoProvider.DbBankLangDao.Save(bankLang);
            }
        }
    }
}
