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
    public interface IDbAccountLangQuery : IQuery<DbAccountLang, long>
    {
        //ISQLQuery FindByDbAccountLangQuery(DbAccountLang accountLang, string accountCode, short languageId, bool isCount);
        //IList<AccountLang> FindByDbAccountLang(DbAccountLang criteria, string accountCode, short languageId, int firstResult, int maxResults, string sortExpression);
        //IList<AccountLang> FindAutoComplete(string accountName, long accountId, short languageId);
        //int CountByDbAccountLangCriteria(DbAccountLang criteria, string accountId, short languageId);
        IList<AccountLang> FindByDbAccountLangKey(long accountId, short languageId);
       IList<AccountLang> FindAccountLangByAccountID(long accountID);
    }
}
