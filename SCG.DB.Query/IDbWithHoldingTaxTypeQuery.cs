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
    public interface IDbWithHoldingTaxTypeQuery : IQuery<DbWithHoldingTaxType, long>
    {
        bool isDuplicationWHTTypeCode(string WHTCode);
        ISQLQuery FindWithHoldingTaxTypeByCriteria(DbWithHoldingTaxType WHTTaxType, bool isCount, short languageId, string sortExpression, string WHTCode, string WHTName);
        IList<DbWithHoldingTaxType> GetWithHoldingTaxTypeList(DbWithHoldingTaxType WHTTaxType, short languageId, int firstResult, int maxResult, string sortExpression , string strWHTTypeCode , string strWHTTypeName);
        int CountWithHoldingTaxTypeByCriteria(DbWithHoldingTaxType WHTTaxType, string strWHTTypeCode, string strWHTTypeName);
        IList<VOWHTType> FindAllWHTTypeActive();
    }
}
