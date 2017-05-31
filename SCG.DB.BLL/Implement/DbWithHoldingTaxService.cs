using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service.Implement;
using SCG.DB.DTO;
using SS.Standard.Data.NHibernate.Dao;
using SCG.DB.DAL;
using SS.Standard.Utilities;
using SCG.DB.Query;

namespace SCG.DB.BLL.Implement
{
    public partial class DbWithHoldingTaxService : ServiceBase<DbWithHoldingTax, long>, IDbWithHoldingTaxService
    {
        #region public override IDao<DbWithHoldingTax, long> GetBaseDao()
        public override IDao<DbWithHoldingTax, long> GetBaseDao()
        {
            return ScgDbDaoProvider.DbWithHoldingTaxDao;
        }
        #endregion public override IDao<DbWithHoldingTax, long> GetBaseDao()

        public string GetWHTCodeExpMapping(string WHTCode)
        {
            return ScgDbQueryProvider.DbWithHoldingTaxQuery.GetWHTCodeExpMapping(WHTCode);
        }
    }
}
