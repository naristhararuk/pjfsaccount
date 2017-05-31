using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

using SCG.DB.DTO;
using SS.Standard.Data.Query;
using SCG.DB.DTO.ValueObject;
using SS.Standard.Data.Query.NHibernate;
using NHibernate;

namespace SCG.DB.Query
{
    public interface IDbCompanyPaymentMethodQuery : IQuery<DbCompanyPaymentMethod, long>
    {
        IList<CompanyPaymentMethodResult> FindCompanyPaymentMethodByCompanyID(long companyID);
        DbCompanyPaymentMethod FindCompanyPaymentMethodByCompanyIdAndPaymentMethodId(long companyId, long paymentMethodId);
        IList<CompanyPaymentMethodResult> GetPaymentMethod(long companyID);
    }
}
