using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;
using SCG.DB.DTO;

namespace SCG.DB.BLL
{
    public interface  IDbPaymentMethodService : IService <DbPaymentMethod, long>
    {
        void AddPaymentMethod(DbPaymentMethod paymentMethod);
        void UpdatePaymentMethod(DbPaymentMethod paymentMethod);
    }
}
