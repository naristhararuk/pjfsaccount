using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Dao;
using SCG.DB.DTO;
using System.Data;

namespace SCG.DB.DAL.Hibernate
{
    public partial class DbPBCurrencyDao : NHibernateDaoBase<DbPBCurrency, long>, IDbPBCurrencyDao
    {
        #region Save
        public void Persist(DataTable dtPbCurrency)
        {
            NHibernateAdapter<DbPBCurrency, long> adapter = new NHibernateAdapter<DbPBCurrency, long>();
            adapter.UpdateChange(dtPbCurrency, ScgDbDaoProvider.DbPBCurrencyDao);

            //return dtPbCurrency.Rows[0].Field<long>(dtPbCurrency.Columns["ID"]);
        }
        #endregion
    }
}
