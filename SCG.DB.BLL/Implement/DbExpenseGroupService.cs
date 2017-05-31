using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service.Implement;
using SCG.DB.DTO;
using SS.Standard.Data.NHibernate.Dao;
using SCG.DB.DAL;
using SS.Standard.Utilities;

namespace SCG.DB.BLL.Implement
{
    public partial class DbExpenseGroupService : ServiceBase<DbExpenseGroup, long>, IDbExpenseGroupService
    {
        public override IDao<DbExpenseGroup, long> GetBaseDao()
        {
            return ScgDbDaoProvider.DbExpenseGroupDao;
        }
        public long AddExpenseGroup(DbExpenseGroup ep)
        {
            #region Validate SuAnnouncementGroup
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            if (string.IsNullOrEmpty(ep.ExpenseGroupCode))
            {
                errors.AddError("Expense.Error", new Spring.Validation.ErrorMessage("Expense Group Code Required"));
            }


            else if (ScgDbDaoProvider.DbExpenseGroupDao.FindByExpenseGroupCode(ep.ExpenseGroupID, ep.ExpenseGroupCode))
            {

                errors.AddError("Expense.Error", new Spring.Validation.ErrorMessage("Expense Group Code already exist"));

            }
          
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
            #endregion
            return ScgDbDaoProvider.DbExpenseGroupDao.Save(ep);

        }
        public void UpdateExpenseGroup(DbExpenseGroup ep)
        {

            #region Validate SuAnnouncementGroup
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            if (string.IsNullOrEmpty(ep.ExpenseGroupCode))
            {
                errors.AddError("Expense.Error", new Spring.Validation.ErrorMessage("Expense Group Code Required"));
            }
            else if (ScgDbDaoProvider.DbExpenseGroupDao.FindByExpenseGroupCode(ep.ExpenseGroupID, ep.ExpenseGroupCode))
            {

                errors.AddError("Expense.Error", new Spring.Validation.ErrorMessage("Expense Group Code already exist"));

            }

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
            #endregion
            ScgDbDaoProvider.DbExpenseGroupDao.SaveOrUpdate(ep);
        }
        public void DeleteExpenseGroup(DbExpenseGroup ep)
        {
            ScgDbDaoProvider.DbExpenseGroupDao.Delete(ep);

        }


    }
}
