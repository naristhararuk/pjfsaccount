using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service;
using SCG.DB.DTO;
using SS.Standard.Data.Query;
using SS.Standard.Data.Query.NHibernate;
using NHibernate;
using SCG.DB.DTO.ValueObject;

namespace SCG.DB.Query
{
    public interface IDbWithHoldingTaxQuery : IQuery<DbWithHoldingTax, long>
    {
        bool isDuplicationWHTCode(string WHTCode);
        ISQLQuery FindWithHoldingTaxByCriteria(DbWithHoldingTax WHTTax, bool isCount, short languageId, string sortExpression, string WHTCode, string WHTName);
        IList<DbWithHoldingTax> GetWithHoldingTaxList(DbWithHoldingTax WHTTax, short languageId, int firstResult, int maxResult, string sortExpression, string strWHTCode, string strWHTName);
        int CountWithHoldingTaxByCriteria(DbWithHoldingTax WHTTax, string strWHTCode, string strWHTName);
        IList<VOWHTRate> FindAllWHTRateActive();
        DbWithHoldingTax FindWithHoldingTaxByWhtCode(string WHTCode);
        string GetWHTCodeExpMapping(string WHTCode);
    }
}
