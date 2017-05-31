using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.Query.NHibernate;
using SCG.DB.DTO;
using NHibernate;
using SCG.DB.DTO.ValueObject;

namespace SCG.DB.Query
{
    public interface IDbPbRateQuery : IQuery<DbPbRate, long>
    {
        ISQLQuery FindPbByCriteria(VOPB pb, string sortExpression, bool isCount);
        IList<VOPB> GetPbList(VOPB pb, int firstResult, int maxResult, string sortExpression);
        IList<VOPB> GetPbList(VOPB pb,string sortExpression);
        int CountPbByCriteria(VOPB pb);

        ISQLQuery FindPbInfoByCriteria(long pbId, string sortExpression, bool isCount);
        IList<VOPbRate> GetPbInfoList(long pbId, int firstResult, int maxResult, string sortExpression);
        int CountPbInfoByCriteria(long pbId);
        double GetExchangeRate(long pbID, short mainCurrencyId, short toCurrencyID);
    }
}
