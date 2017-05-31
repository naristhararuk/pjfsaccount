using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;

using SCG.DB.DTO;
using SCG.DB.DAL;
using NHibernate;
using NHibernate.Expression;
using NHibernate.Transform;
using SS.Standard.Data.NHibernate.QueryCreator;

namespace SCG.DB.DAL.Hibernate
{
    public partial class DbPaymentMethodDao : NHibernateDaoBase<DbPaymentMethod, long>, IDbPaymentMethodDao
    {
        public DbPaymentMethodDao()
        { 
        
        }
        public bool IsDuplicatePaymentMethodCode(DbPaymentMethod paymentMethod)
        {
            IList<DbPaymentMethod> list = GetCurrentSession().CreateQuery("from DbPaymentMethod p where p.PaymentMethodID <> :PaymentMethodID and p.PaymentMethodCode = :PaymentMethodCode")
                  .SetInt64("PaymentMethodID", paymentMethod.PaymentMethodID)
                  .SetString("PaymentMethodCode", paymentMethod.PaymentMethodCode)
                  .List<DbPaymentMethod>();
            if (list.Count > 0)
            {
                return true;
            }
            return false;
        }
    }
}
