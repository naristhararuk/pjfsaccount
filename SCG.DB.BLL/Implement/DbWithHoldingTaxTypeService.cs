using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service.Implement;
using SCG.DB.DTO;
using SS.Standard.Data.NHibernate.Dao;
using SCG.DB.DAL;
using SS.Standard.Utilities;

namespace SCG.DB.BLL.Implement
{
    public partial class DbWithHoldingTaxTypeService : ServiceBase<DbWithHoldingTaxType, long>, IDbWithHoldingTaxTypeService
    {
        #region public override IDao<DbWithHoldingTaxType, long> GetBaseDao()
        public override IDao<DbWithHoldingTaxType, long> GetBaseDao()
        {
            return ScgDbDaoProvider.DbWithHoldingTaxTypeDao;
        }
        #endregion public override IDao<DbWithHoldingTaxType, long> GetBaseDao()
    }
}
