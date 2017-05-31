using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;

using SCG.DB.DTO;
using SCG.DB.BLL;
using SCG.DB.DAL;
using SCG.DB.Query;
using SS.Standard.Utilities;
using SCG.DB.DTO.ValueObject;
using SS.Standard.Security;

namespace SCG.DB.BLL.Implement
{
    public partial class DbCompanyService : ServiceBase<DbCompany, long>, IDbCompanyService
    {
        public IUserAccount UserAccount { get; set; }
        public override IDao<DbCompany, long> GetBaseDao()
        {
            return ScgDbDaoProvider.DbCompanyDao;
        }
        public void AddCompany(DbCompany company)
        {
            #region Validate DbCompany
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            if (string.IsNullOrEmpty(company.CompanyCode))
            {
                errors.AddError("Company.Error", new Spring.Validation.ErrorMessage("Company_CodeRequired"));
            }
            if (ScgDbDaoProvider.DbCompanyDao.IsDuplicateCompanyCode(company))
            {
                errors.AddError("Company.Error", new Spring.Validation.ErrorMessage("UniqueCompanyCode"));
            }
            if (string.IsNullOrEmpty(company.CompanyName))
            {
                errors.AddError("Company.Error", new Spring.Validation.ErrorMessage("Company_NameRequired"));
            }
            if (string.IsNullOrEmpty(company.PaymentType))
            {
                errors.AddError("Company.Error", new Spring.Validation.ErrorMessage("Payment_TypeRequired"));
            }
            if (company.DefaultTaxID == 0)
            {
                errors.AddError("Company.Error", new Spring.Validation.ErrorMessage("DefaultTax_CodeRequired"));
            }
            if (string.IsNullOrEmpty(company.BU))
            {
                errors.AddError("Company.Error", new Spring.Validation.ErrorMessage("BU Required"));
            }
            if (company.UseEcc == true && string.IsNullOrEmpty(company.SapCode))
            {
                errors.AddError("Company.Error", new Spring.Validation.ErrorMessage("SAP Instance Required"));
            }

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            #endregion     
            
            company.CompanyID =  ScgDbDaoProvider.DbCompanyDao.Save(company);
            long perdiemprofileid = ScgDbQueryProvider.DbCompanyQuery.GetFnPerdiemProfileCompany(company.CompanyID);

            if (perdiemprofileid > 0 && company.PerdiemProfileID == 0)
            {
                ScgDbDaoProvider.DbCompanyDao.DeleteFnPerdiemProfileCompany(company.CompanyID);
            }
            else if (perdiemprofileid > 0 && company.PerdiemProfileID != 0 && perdiemprofileid != company.PerdiemProfileID)
            {
                ScgDbDaoProvider.DbCompanyDao.UpdateFnPerdiemProfileCompany(company.CompanyID, company.PerdiemProfileID, UserAccount.UserID);
            }
            else if (perdiemprofileid == 0 && company.PerdiemProfileID > 0)
            {
                ScgDbDaoProvider.DbCompanyDao.AddFnPerdiemProfileCompany(company.CompanyID, company.PerdiemProfileID, UserAccount.UserID);
            }
            ScgDbDaoProvider.DbCompanyDao.SyncNewCompany();
        }


        public void UpdateCompany(DbCompany company)
        {
            #region Validate DbCompany
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            if (string.IsNullOrEmpty(company.CompanyCode))
            {
                errors.AddError("Company.Error", new Spring.Validation.ErrorMessage("Company_CodeRequired"));
            }
            if (ScgDbDaoProvider.DbCompanyDao.IsDuplicateCompanyCode(company))
            {
                errors.AddError("Company.Error", new Spring.Validation.ErrorMessage("UniqueCompanyCode"));
            }
            if (string.IsNullOrEmpty(company.CompanyName))
            {
                errors.AddError("Company.Error", new Spring.Validation.ErrorMessage("Company_NameRequired"));
            }
            if (string.IsNullOrEmpty(company.PaymentType))
            {
                errors.AddError("Company.Error", new Spring.Validation.ErrorMessage("Payment_TypeRequired"));
            }
            if (company.DefaultTaxID == 0)
            {
                errors.AddError("Company.Error", new Spring.Validation.ErrorMessage("DefaultTax_CodeRequired"));
            }
            if (string.IsNullOrEmpty(company.BU))
            {
                errors.AddError("Company.Error", new Spring.Validation.ErrorMessage("BU Required"));
            }
            if (company.UseEcc == true && string.IsNullOrEmpty(company.SapCode))
            {
                errors.AddError("Company.Error", new Spring.Validation.ErrorMessage("SAP Instance Required"));
            }
            
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);


            #endregion
            long perdiemprofileid = ScgDbQueryProvider.DbCompanyQuery.GetFnPerdiemProfileCompany(company.CompanyID);

            if (perdiemprofileid > 0 && company.PerdiemProfileID == 0)
            {
                ScgDbDaoProvider.DbCompanyDao.DeleteFnPerdiemProfileCompany(company.CompanyID);
            }
            else if (perdiemprofileid > 0 && company.PerdiemProfileID != 0 && perdiemprofileid != company.PerdiemProfileID)
            {
                ScgDbDaoProvider.DbCompanyDao.DeleteFnPerdiemProfileCompany(company.CompanyID);
                ScgDbDaoProvider.DbCompanyDao.AddFnPerdiemProfileCompany(company.CompanyID, company.PerdiemProfileID, UserAccount.UserID);
                //ScgDbDaoProvider.DbCompanyDao.UpdateFnPerdiemProfileCompany(company.CompanyID, company.PerdiemProfileID, UserAccount.UserID);
            }
            else if (perdiemprofileid == 0 && company.PerdiemProfileID > 0)
            {
                ScgDbDaoProvider.DbCompanyDao.AddFnPerdiemProfileCompany(company.CompanyID, company.PerdiemProfileID, UserAccount.UserID);
            }
            ScgDbDaoProvider.DbCompanyDao.Update(company);
            ScgDbDaoProvider.DbCompanyDao.SyncUpdateCompany(company.CompanyCode);
        }
        public void DeleteCompany(DbCompany company)
        {
            ScgDbDaoProvider.DbCompanyDao.Delete(company);
            ScgDbDaoProvider.DbCompanyDao.SyncDeleteCompany(company.CompanyCode);
        }
        public long AddCompanyAndGetId(DbCompany company) 
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            if (string.IsNullOrEmpty(company.CompanyCode))
            {
                errors.AddError("Company.Error", new Spring.Validation.ErrorMessage("Company_CodeRequired"));
            }
            if (ScgDbDaoProvider.DbCompanyDao.IsDuplicateCompanyCode(company))
            {
                errors.AddError("Company.Error", new Spring.Validation.ErrorMessage("UniqueCompanyCode"));
            }
            if (string.IsNullOrEmpty(company.CompanyName))
            {
                errors.AddError("Company.Error", new Spring.Validation.ErrorMessage("Company_NameRequired"));
            }
            if (string.IsNullOrEmpty(company.PaymentType))
            {
                errors.AddError("Company.Error", new Spring.Validation.ErrorMessage("Payment_TypeRequired"));
            }
            if (company.PaymentMethodPetty.PaymentMethodID == 0)
            {
                errors.AddError("Company.Error", new Spring.Validation.ErrorMessage("PaymentMethod_PettyRequired"));
            }
            if (company.PaymentMethodTransfer.PaymentMethodID == 0)
            {
                errors.AddError("Company.Error", new Spring.Validation.ErrorMessage("PaymentMethod_TransferRequired"));
            }
            if (company.PaymentMethodCheque.PaymentMethodID == 0)
            {
                errors.AddError("Company.Error", new Spring.Validation.ErrorMessage("PaymentMethod_ChequeRequired"));
            }
            if (company.DefaultTaxID == 0)
            {
                errors.AddError("Company.Error", new Spring.Validation.ErrorMessage("DefaultTax_CodeRequired"));
            }
            
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            

            return ScgDbDaoProvider.DbCompanyDao.Save(company);
        }

        public DbCompany getCompanyByCode (string companyCode)
        {
            return ScgDbQueryProvider.DbCompanyQuery.getDbCompanyByCompanyCode(companyCode);
        }
        
        #region IDbCompanyService Members


        public DbCompany FindByCompanycode(string companyCode)
        {
            return ScgDbDaoProvider.DbCompanyDao.FindByCompanycode(companyCode);
        }

        #endregion
    }
}
