using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.Query.NHibernate;
using SCG.DB.DTO;
using NHibernate;
using SCG.DB.DTO.ValueObject;
using SS.DB.DTO.ValueObject;

namespace SCG.DB.Query
{
    public interface IDbMoneyRequestQuery : IQuery<MoneyRequestSearchResult, long>
    {
        ISQLQuery FindMoneyBuyingRequestDocument(MoneyRequestSearchResult moneyRequestCriteria, bool isCount, string sortExpression);

        IList<MoneyRequestSearchResult> GetMoneyRequestList(MoneyRequestSearchResult moneyRequestCriteria, int firstResult, int maxResult, string sortExpression);

        int CountMoneyRequestByCriteria(MoneyRequestSearchResult moneyRequestCriteria);

        IList<SellingDetailResult> GetMoneySellingRequestList(SellingRequestLetterParameter criteria, int firstResult, int maxResult, string sortExpression);

        int CountMoneySellingRequestByCriteria(SellingRequestLetterParameter criteria);

        ISQLQuery FindMoneySellingRequestDocument(SellingRequestLetterParameter param, bool isCount, string sortExpression);

    }
}