using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;
using SS.Standard.Utilities;
using SCG.eAccounting.SAP.DTO;
using SCG.eAccounting.SAP.Service.Interface;
using SCG.eAccounting.SAP.DAL;

namespace SCG.eAccounting.SAP.Service.Implement
{
    public partial class Bapiaccr09Service : ServiceBase<Bapiaccr09, long>, IBapiaccr09Service
    {
        public override IDao<Bapiaccr09, long> GetBaseDao()
        {
            return BapiDaoProvider.Bapiaccr09Dao;
        }
    }
}
