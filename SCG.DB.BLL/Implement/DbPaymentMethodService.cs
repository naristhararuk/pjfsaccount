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
    public partial class DbPaymentMethodService : ServiceBase<DbPaymentMethod, long>, IDbPaymentMethodService
    {
        public override IDao<DbPaymentMethod, long> GetBaseDao()
        {
            return ScgDbDaoProvider.DbPaymentMethodDao;
        }
        public void AddPaymentMethod(DbPaymentMethod paymentMethod)
        {
            #region Validate DbPaymentMethod

            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (string.IsNullOrEmpty(paymentMethod.PaymentMethodCode))
            {
                errors.AddError("PaymentMethod.Error", new Spring.Validation.ErrorMessage("PaymentMethod_CodeRequired"));
            }
            if (ScgDbDaoProvider.DbPaymentMethodDao.IsDuplicatePaymentMethodCode(paymentMethod))
            {
                errors.AddError("PaymentMethod.Error", new Spring.Validation.ErrorMessage("UniquePaymentMethodCode"));
            }
          
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            #endregion

            ScgDbDaoProvider.DbPaymentMethodDao.Save(paymentMethod);
        }

        public void UpdatePaymentMethod(DbPaymentMethod paymentMethod)
        {
            #region Validate DbPaymentMethod

            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (string.IsNullOrEmpty(paymentMethod.PaymentMethodCode))
            {
                errors.AddError("PaymentMethod.Error", new Spring.Validation.ErrorMessage("PaymentMethod_CodeRequired"));
            }
            if (ScgDbDaoProvider.DbPaymentMethodDao.IsDuplicatePaymentMethodCode(paymentMethod))
            {
                errors.AddError("PaymentMethod.Error", new Spring.Validation.ErrorMessage("UniquePaymentMethodCode"));
            }
   
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            #endregion

            ScgDbDaoProvider.DbPaymentMethodDao.Update(paymentMethod);
        }

    }
}
