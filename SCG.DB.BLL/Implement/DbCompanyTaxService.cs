using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service.Implement;
using SCG.DB.DTO;
using SCG.DB.DAL;
using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Utilities;

namespace SCG.DB.BLL.Implement
{
    public partial class DbCompanyTaxService : ServiceBase<DbCompanyTax, long>, IDbCompanyTaxService
    {
        public override IDao<DbCompanyTax, long> GetBaseDao()
        {
            return ScgDbDaoProvider.DbCompanyTaxDao;
        }

        public void AddCompanyTax(DbCompanyTax company)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            if (company.CompanyID == 0)
            {
                errors.AddError("CompanyTax.Error", new Spring.Validation.ErrorMessage("CompanyCodeRequired"));
            }
            //if (ScgDbDaoProvider.DbCompanyTaxDao.IsDuplicateCompanyCode(company))
            //{
            //    errors.AddError("CompanyTax.Error", new Spring.Validation.ErrorMessage("UniqueCompanyCode"));
            //}
            if (company.Rate < 0)
            {
                errors.AddError("CompanyTax.Error", new Spring.Validation.ErrorMessage("RateRequired"));
            }
            if (company.Rate > 100)
            {
                errors.AddError("CompanyTax.Error", new Spring.Validation.ErrorMessage("RateMustBeLessThanHundred"));
            }

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            ScgDbDaoProvider.DbCompanyTaxDao.Save(company);
        }

        public void UpdateCompanyTax(DbCompanyTax company)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            if (company.Rate < 0)
            {
                errors.AddError("CompanyTax.Error", new Spring.Validation.ErrorMessage("RateRequired"));
            }
            if (company.Rate > 100)
            {
                errors.AddError("CompanyTax.Error", new Spring.Validation.ErrorMessage("RateMustBeLessThanHundred"));
            }

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            ScgDbDaoProvider.DbCompanyTaxDao.SaveOrUpdate(company);
        }

        public void DeleteCompanyTax(DbCompanyTax company)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (company.ID == 0)
            {
                errors.AddError("CompanyTax.Error", new Spring.Validation.ErrorMessage("RequiedID"));
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            ScgDbDaoProvider.DbCompanyTaxDao.Delete(company);
        }
    }
}
