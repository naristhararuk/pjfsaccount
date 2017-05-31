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
using SCG.DB.DTO.ValueObject;

namespace SCG.DB.BLL.Implement
{
    public partial class DbCompanyPaymentMethodService : ServiceBase<DbCompanyPaymentMethod, long>, IDbCompanyPaymentMethodService
    {
        public override IDao<DbCompanyPaymentMethod, long> GetBaseDao()
        {
            return ScgDbDaoProvider.DbCompanyPaymentMethodDao;
        }
        public void AddCompanyPaymentMethod(DbCompanyPaymentMethod companyPaymentMethod)
        {
            //#region Validate DbCompany
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            if (companyPaymentMethod.PaymentMethod == null)
            {
                errors.AddError("CompanyPaymentMethod.Error", new Spring.Validation.ErrorMessage("Company_PaymentMethod_PleaseChoode"));
            }
            //if (ScgDbDaoProvider.DbCompanyDao.IsDuplicateCompanyCode(company))
            //{
            //    errors.AddError("Company.Error", new Spring.Validation.ErrorMessage("UniqueCompanyCode"));
            //}
            //if (string.IsNullOrEmpty(company.CompanyName))
            //{
            //    errors.AddError("Company.Error", new Spring.Validation.ErrorMessage("Company_NameRequired"));
            //}
            //if (string.IsNullOrEmpty(company.PaymentType))
            //{
            //    errors.AddError("Company.Error", new Spring.Validation.ErrorMessage("Payment_TypeRequired"));
            //}
            //if (company.PaymentMethodPetty.PaymentMethodID == 0)
            //{
            //    errors.AddError("Company.Error", new Spring.Validation.ErrorMessage("PaymentMethod_PettyRequired"));
            //}
            //if (company.PaymentMethodTransfer.PaymentMethodID == 0)
            //{
            //    errors.AddError("Company.Error", new Spring.Validation.ErrorMessage("PaymentMethod_TransferRequired"));
            //}
            //if (company.PaymentMethodCheque.PaymentMethodID == 0)
            //{
            //    errors.AddError("Company.Error", new Spring.Validation.ErrorMessage("PaymentMethod_ChequeRequired"));
            //}
            

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            //#endregion

            ScgDbDaoProvider.DbCompanyPaymentMethodDao.Save(companyPaymentMethod);
            
               
            
           
        }
        public void UpdateCompanyPaymentMethod(DbCompanyPaymentMethod companyPaymentMethod)
        {
            ScgDbDaoProvider.DbCompanyPaymentMethodDao.Update(companyPaymentMethod);
        }

        
    }
}
