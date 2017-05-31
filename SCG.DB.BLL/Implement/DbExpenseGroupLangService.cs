using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service.Implement;
using SCG.DB.DTO;
using SCG.DB.DAL;
using SS.Standard.Data.NHibernate.Dao;
using SS.DB.DTO;

namespace SCG.DB.BLL.Implement
{

    public partial class DbExpenseGroupLangService : ServiceBase<DbExpenseGroupLang, long>, IDbExpenseGroupLangService
    {

        public override IDao<DbExpenseGroupLang, long> GetBaseDao()
        {
            return ScgDbDaoProvider.DbExpenseGroupLangDao;
        }
        public long AddExpenseGroupLang(DbExpenseGroupLang expenseGroup)
        {
            long id = 0;
            id = ScgDbDaoProvider.DbExpenseGroupLangDao.Save(expenseGroup);

            return id;
        }
        public void UpdateExpenseGroupLang(IList<DbExpenseGroupLang> expenseGroup)
        {

            foreach (DbExpenseGroupLang ep in expenseGroup)
            {

                ScgDbDaoProvider.DbExpenseGroupLangDao.DeleteExpenseGroup(ep.ExpenseGroupID.ExpenseGroupID, ep.LanguageID.Languageid);
            }

            foreach (DbExpenseGroupLang ep in expenseGroup)
            {
                DbExpenseGroupLang expenseLang = new DbExpenseGroupLang();
                expenseLang.ExpenseGroupID = new DbExpenseGroup(ep.ExpenseGroupID.ExpenseGroupID);
                expenseLang.LanguageID = new DbLanguage(ep.LanguageID.Languageid);
                expenseLang.Active = ep.Active;
                expenseLang.Comment = ep.Comment;
                expenseLang.CreBy = ep.CreBy;
                expenseLang.CreDate = DateTime.Now;
                expenseLang.Description = ep.Description;
                expenseLang.UpdBy = ep.UpdBy;
                expenseLang.UpdDate = DateTime.Now;
                expenseLang.UpdPgm = ep.UpdPgm;
                ScgDbDaoProvider.DbExpenseGroupLangDao.Save(expenseLang);
            }

        }
        public void AddExpenseGroupLang(IList<DbExpenseGroupLang> expenseGroup)
        {
            
            foreach (DbExpenseGroupLang ep in expenseGroup)
            {
                DbExpenseGroupLang expenseLang = new DbExpenseGroupLang();
                expenseLang.ExpenseGroupID = new DbExpenseGroup(ep.ExpenseGroupID.ExpenseGroupID);
                expenseLang.LanguageID = new DbLanguage(ep.LanguageID.Languageid);
                expenseLang.Active = ep.Active;
                expenseLang.Comment = ep.Comment;
                expenseLang.CreBy = ep.CreBy;
                expenseLang.CreDate = DateTime.Now;
                expenseLang.Description = ep.Description;
                expenseLang.UpdBy = ep.UpdBy;
                expenseLang.UpdDate = DateTime.Now;
                expenseLang.UpdPgm = ep.UpdPgm;
                ScgDbDaoProvider.DbExpenseGroupLangDao.Save(expenseLang);

            }
        }

    }
}
