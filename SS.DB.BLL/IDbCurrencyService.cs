using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

using SS.DB.DTO;

namespace SS.DB.BLL
{
    public interface IDbCurrencyService : IService<DbCurrency, short>
    {
        new IList<DbCurrency> FindAll();
        new void Delete(DbCurrency domain);
        new DbCurrency FindByIdentity(short id);
        new short Save(DbCurrency domain);
        new void SaveOrUpdate(DbCurrency domain);
        new void Update(DbCurrency domain);
        short AddCurrency(DbCurrency currency);
        void UpdateCurrency(DbCurrency currency);
        void DeleteCurrency(DbCurrency currency);
    }
}
