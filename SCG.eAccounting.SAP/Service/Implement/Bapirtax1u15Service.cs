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
    public partial class Bapirtax1u15Service : ServiceBase<Bapirtax1u15, long>, IBapirtax1u15Service
    {
        public override IDao<Bapirtax1u15, long> GetBaseDao()
        {
            return BapiDaoProvider.Bapirtax1u15Dao;
        }
    }
}
