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
    public interface IDbBankQuery : IQuery<DbBank, short>
    {
        ISQLQuery FindByBankCriteria(BankLang bank, bool isCount, short languageId, string sortExpression);
        IList<BankLang> GetBankList(BankLang bank, short languageId, int firstResult, int maxResult, string sortExpression);
        int CountByBankCriteria(BankLang bank);
    }
}
