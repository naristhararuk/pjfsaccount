using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

using SCG.DB.DTO;

namespace SCG.DB.BLL
{
    public interface IDbBankService : IService<DbBank, short>
    {
        void AddBank(DbBank bank, DbBankLang bankLang);
        void UpdateBank(DbBank bank);
    }
}
