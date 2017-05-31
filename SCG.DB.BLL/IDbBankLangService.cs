using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

using SCG.DB.DTO;

namespace SCG.DB.BLL
{
    public interface IDbBankLangService : IService<DbBankLang, long>
    {
        IList<SCG.DB.DTO.ValueObject.BankLang> FindByBankId(short bankId);
        void UpdateBankLang(IList<DbBankLang> bankLangList);
    }
}
