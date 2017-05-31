using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Dao;
using SCG.DB.DTO;

namespace SCG.DB.DAL
{
    public interface IDbAccountLangDao : IDao<DbAccountLang, long>
    {
        IList<SCG.DB.DTO.ValueObject.AccountLang> FindByAccountId(long accountID);
        void DeleteAllAccountLang(long accountID);
        void DeleteByAccountIdLanguageId(long accountId, short languageId);
    }
}
