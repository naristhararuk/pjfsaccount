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
    public partial class DbLocationLangService : ServiceBase<DbLocationLang, long>, IDbLocationLangService
    {
        public override IDao<DbLocationLang, long> GetBaseDao()
        {
            return ScgDbDaoProvider.DbLocationLangDao;
        }
        public void AddLocationLang(DbLocationLang locationLang)
        {
            #region Validate DbCompany
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            if (string.IsNullOrEmpty(locationLang.LocationID.LocationCode))
            {
                errors.AddError("LocationLang.Error", new Spring.Validation.ErrorMessage("Location Code Required"));
            }
         
   
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            #endregion

            ScgDbDaoProvider.DbLocationLangDao.Save(locationLang);

        }
        public void UpdateLocationLang(DbLocationLang locationLang)
        {
            #region Validate DbCompany
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            if (string.IsNullOrEmpty(locationLang.LocationID.LocationCode))
            {
                errors.AddError("LocationLang.Error", new Spring.Validation.ErrorMessage("Location Code Required"));
            }


            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            #endregion

            ScgDbDaoProvider.DbLocationLangDao.SaveOrUpdate(locationLang);

        }
        //public void AddCompany(DbCompany company)
        //{
        //    #region Validate DbCompany
        //    Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
        //    if (string.IsNullOrEmpty(company.CompanyCode))
        //    {
        //        errors.AddError("Company.Error", new Spring.Validation.ErrorMessage("Company_CodeRequired"));
        //    }
        //    if (ScgDbDaoProvider.DbCompanyDao.IsDuplicateCompanyCode(company))
        //    {
        //        errors.AddError("Company.Error", new Spring.Validation.ErrorMessage("UniqueCompanyCode"));
        //    }
        //    if (string.IsNullOrEmpty(company.CompanyName))
        //    {
        //        errors.AddError("Company.Error", new Spring.Validation.ErrorMessage("Company_NameRequired"));
        //    }
        //    if (string.IsNullOrEmpty(company.PaymentType))
        //    {
        //        errors.AddError("Company.Error", new Spring.Validation.ErrorMessage("Payment_TypeRequired"));
        //    }
        //    if (company.PaymentMethodPetty.PaymentMethodID == 0)
        //    {
        //        errors.AddError("Company.Error", new Spring.Validation.ErrorMessage("PaymentMethod_PettyRequired"));
        //    }
        //    if (company.PaymentMethodTransfer.PaymentMethodID == 0)
        //    {
        //        errors.AddError("Company.Error", new Spring.Validation.ErrorMessage("PaymentMethod_TransferRequired"));
        //    }
        //    if (company.PaymentMethodCheque.PaymentMethodID == 0)
        //    {
        //        errors.AddError("Company.Error", new Spring.Validation.ErrorMessage("PaymentMethod_ChequeRequired"));
        //    }
            

        //    if (!errors.IsEmpty) throw new ServiceValidationException(errors);

        //    #endregion

        //    ScgDbDaoProvider.DbCompanyDao.Save(company);


        public void UpdateLocationLang(IList<DbLocationLang> locationLangList)
        {
            if (locationLangList.Count > 0)
            {
                ScgDbDaoProvider.DbLocationLangDao.DeleteAllLocationLang(locationLangList[0].LocationID.LocationID);
            }
            foreach (DbLocationLang locationLang in locationLangList)
            {
                ScgDbDaoProvider.DbLocationLangDao.Save(locationLang);
            }
        }
    }
}
