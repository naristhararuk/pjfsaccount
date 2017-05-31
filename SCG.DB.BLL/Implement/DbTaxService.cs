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
    public partial class DbTaxService : ServiceBase<DbTax, long>, IDbTaxService
    {
        public override IDao<DbTax, long> GetBaseDao()
        {
            return ScgDbDaoProvider.DbTaxDao;
        }
        public void AddTax(DbTax dbTax)
        {
            #region Validate SuAnnouncementGroup
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            if (string.IsNullOrEmpty(dbTax.TaxCode))
            {
                errors.AddError("Tax.Error", new Spring.Validation.ErrorMessage("TaxCodeRequired"));
            }
            if (ScgDbDaoProvider.DbTaxDao.IsDuplicateTaxCode(dbTax))
            {
                errors.AddError("Tax.Error", new Spring.Validation.ErrorMessage("UniqueTaxCode"));
            }
            if (string.IsNullOrEmpty(dbTax.GL))
            {
                errors.AddError("Tax.Error", new Spring.Validation.ErrorMessage("GLRequired"));
            }
            if (dbTax.Rate < 0)
            {
                errors.AddError("Tax.Error", new Spring.Validation.ErrorMessage("RateRequired"));
            }
            if (dbTax.RateNonDeduct > 0 && dbTax.RateNonDeduct > 100)
            {
                errors.AddError("Tax.Error", new Spring.Validation.ErrorMessage("RateNonDeductMustBeLessThanHundred"));
            }
            if (dbTax.Rate > 100)
            {
                errors.AddError("Tax.Error", new Spring.Validation.ErrorMessage("RateMustBeLessThanHundred"));
            }

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
           
             ScgDbDaoProvider.DbTaxDao.Save(dbTax);
            
               
            #endregion
           
        }

        public void UpdateTax(DbTax tax)
        {
            //#region Validate SuAnnouncementGroup
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (string.IsNullOrEmpty(tax.TaxCode))
            {
                errors.AddError("Tax.Error", new Spring.Validation.ErrorMessage("TaxCodeRequired"));
            }
            if (string.IsNullOrEmpty(tax.GL))
            {
                errors.AddError("Tax.Error", new Spring.Validation.ErrorMessage("GLRequired"));
            }
            if (tax.Rate <0 )
            {
                errors.AddError("Tax.Error", new Spring.Validation.ErrorMessage("RateRequired"));
            }
            if (ScgDbDaoProvider.DbTaxDao.IsDuplicateTaxCode(tax))
            {
                errors.AddError("Tax.Error", new Spring.Validation.ErrorMessage("UniqueTaxCode"));
            }

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
            ScgDbDaoProvider.DbTaxDao.SaveOrUpdate(tax);
        }

        public long FindTaxId(string taxCode)
        {
            return ScgDbDaoProvider.DbTaxDao.FindTaxId(taxCode);
        }
    }
}
