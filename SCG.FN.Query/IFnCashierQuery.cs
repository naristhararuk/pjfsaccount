using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Query;
using SS.Standard.Data.NHibernate.QueryDao;
using SS.Standard.Data.NHibernate.QueryCreator;

using SS.DB.DTO;

using SCG.FN.DTO;
using SCG.FN.DTO.ValueObject;
using NHibernate;

namespace SCG.FN.Query
{
    public interface IFnCashierQuery : IQuery<FnCashier, short>
    {
        ISQLQuery FindCashierByOrganization(short organizationId, short languageId, bool isCount, string sortExpression);
        IList<FnCashierSearchResult> GetCashierList(short organizationId, short languageId, int firstResult, int maxResult, string sortExpression);
        int GetCashierCount(short organizationId, short languageId);
    }
}
