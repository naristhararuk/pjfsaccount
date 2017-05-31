using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;
using SS.Standard.Utilities;

using SCG.FN.DAL;
using SCG.FN.DTO;
using SCG.FN.BLL;
using SCG.FN.DTO.ValueObject;

namespace SCG.FN.BLL.Implement
{
    public partial class FnCashierService : ServiceBase<FnCashier,short>,IFnCashierService
    {
        public override IDao<FnCashier, short> GetBaseDao()
        {
            return DaoProvider.FnCashierDao;
        }
        public void AddCashier(FnCashier cashier)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            if (cashier.Organization == null)
            {
                errors.AddError("Cashier.Error", new Spring.Validation.ErrorMessage("OrganizationRequired"));
            }
            if (cashier.Division == null)
            {
                errors.AddError("Cashier.Error", new Spring.Validation.ErrorMessage("DivisionRequired"));
            }
            if (string.IsNullOrEmpty(cashier.CashierCode))
            {
                errors.AddError("Cashier.Error", new Spring.Validation.ErrorMessage("CashierCodeRequired"));
            }
            
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            DaoProvider.FnCashierDao.Save(cashier);
        }
        public void UpdateCashier(FnCashier cashier)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            if (cashier.Organization == null)
            {
                errors.AddError("Cashier.Error", new Spring.Validation.ErrorMessage("OrganizationRequired"));
            }
            if (cashier.Division == null)
            {
                errors.AddError("Cashier.Error", new Spring.Validation.ErrorMessage("DivisionRequired"));
            }
            if (string.IsNullOrEmpty(cashier.CashierCode))
            {
                errors.AddError("Cashier.Error", new Spring.Validation.ErrorMessage("CashierCodeRequired"));
            }

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            DaoProvider.FnCashierDao.SaveOrUpdate(cashier);
        }
    }
}
