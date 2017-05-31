using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service;
using NHibernate;
using SCG.DB.DTO;

namespace SCG.DB.BLL
{
    public interface IDbAccountLangService : IService<DbAccountLang, long>
    {
        new DbAccountLang FindByIdentity(long accountId);
        IList<SCG.DB.DTO.ValueObject.AccountLang> FindByAccountId(long accountId);
        void UpdateAccountLang(IList<DbAccountLang> accountLangList);

        long AddAccountLang(DbAccountLang account);
         void UpdateListAccountLang(IList<DbAccountLang> account);
         void AddListAccountLang(IList<DbAccountLang> account);


    }
}
