using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

using SS.DB.DTO;

namespace SS.DB.BLL
{
    public interface IDbExchangeRateService : IService<DbExchangeRate, short>
    {
        new IList<DbExchangeRate> FindAll();
        new void Delete(DbExchangeRate domain);
        new DbExchangeRate FindByIdentity(short id);
        new short Save(DbExchangeRate domain);
        new void SaveOrUpdate(DbExchangeRate domain);
        new void Update(DbExchangeRate domain);
        short AddExchangeRate(DbExchangeRate exchangeRate);
        void UpdateExchangeRate(DbExchangeRate exchangeRate);
    }
}
