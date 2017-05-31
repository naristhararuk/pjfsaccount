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
    public interface IDbAccountQuery : IQuery<DbAccount, long>
    {
        ISQLQuery FindByAccountCriteria(bool isCount, short languageId, string sortExpression);
        IList<AccountLang> GetAccountList(short languageId, int firstResult, int maxResult, string sortExpression);
        int CountByAccountCriteria();

        ISQLQuery FindByAccountLovCriteria(bool isCount, short languageId, string sortExpression, string expenseGroupID, string accountCode, string description, long companyID, string withoutExpenseCode);
        int CountByAccountLovCriteria(short languageId, string expenseGroupID, string accountCode, string description, long companyID, string withoutExpenseCode);
        IList<AccountLang> GetAccountLovList(short languageId, int firstResult, int maxResult, string sortExpression, string expenseGroupID, string accountCode, string description, long companyID, string withoutExpenseCode);
        IList<AccountLang> FindAutoComplete(string prefixText, AccountAutoCompleteParameter param);
        IList<ExpenseRecommend> FindExpenseRecommendByExpenseGroup(ExpenseRecommend recommend);


        ISQLQuery FindAccountInfoByAccount(long acount, bool isCount, string sortExpression);
        int CountAccountInfo(long acount);
        DbAccount FindAccountByEGroupIDAID(long expenseID, long accoutID);


        IList<AccountLang> GetAccountInfoByAccount(long acount, int firstResult, int maxResult, string sortExpression);
        DbAccount FindDbAccountByAccountCode(string accountCode);
        DbAccount FindAccountByAccountCodeExpenseGroup(string accountCode, long? companyId);

        string GetAccountCodeExpMapping(string accountCode, string expenseGroupType);
    }
}
