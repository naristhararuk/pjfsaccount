using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;

using SCG.DB.DTO;

namespace SCG.DB.DAL
{
    public interface IDbBankLangDao : IDao<DbBankLang, long>
    {
        IList<SCG.DB.DTO.ValueObject.BankLang> FindByBankId(short bankId);
        void DeleteAllBankLang(short bankId);
    }
}
