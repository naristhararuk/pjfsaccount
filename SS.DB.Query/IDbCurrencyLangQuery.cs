using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.Query.NHibernate;
using SS.DB.DTO;
using SS.DB.DTO.ValueObject;

namespace SS.DB.Query
{
    public interface IDbCurrencyLangQuery : IQuery<DbCurrencyLang, long> 
    {
        IList<VOUCurrencySetup> FindCurrencyLangByCurrencyID(long cid);
        IList<DbCurrencyLang> FindCurrencyLangByCID(short id);
    }
}
