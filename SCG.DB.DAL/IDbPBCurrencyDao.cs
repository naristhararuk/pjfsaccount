using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.DB.DTO;
using SS.Standard.Data.NHibernate.Dao;
using System.Data;

namespace SCG.DB.DAL
{
    public interface IDbPBCurrencyDao : IDao<DbPBCurrency, long> 
    {
        void Persist(DataTable dtPbCurrency);
    }
}
