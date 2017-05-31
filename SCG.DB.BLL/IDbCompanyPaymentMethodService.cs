using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

using SCG.DB.DTO;

namespace SCG.DB.BLL
{
    public interface IDbCompanyPaymentMethodService : IService<DbCompanyPaymentMethod, long>
    {
        void AddCompanyPaymentMethod(DbCompanyPaymentMethod companyPaymentMethod);
        void UpdateCompanyPaymentMethod(DbCompanyPaymentMethod companyPaymentMethod);
    }
}
