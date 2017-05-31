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
    public interface IDbPaymentMethodQuery : IQuery<DbPaymentMethod, long>
    {
        IList<DbPaymentMethod> FindPaymentMethodNotAdd(IList<string> paymentMethodIDList);
        DbPaymentMethod FindPaymentMethodByID(long paymentMethodID);
        ISQLQuery FindPaymentMethodByCriteria(DbPaymentMethod paymentMethod, bool isCount, string sortExpression);
        IList<DbPaymentMethod> GetPaymentMethodList(DbPaymentMethod paymentMethod, int firstResult, int maxResult, string sortExpression);
        int CountPaymentMethodByCriteria(DbPaymentMethod paymentMethod);

        IList<DbPaymentMethod> FindPaymentMethodActive();
        IList<DbPaymentMethod> FindPaymentMethodActive(short ComID);
    }
}
