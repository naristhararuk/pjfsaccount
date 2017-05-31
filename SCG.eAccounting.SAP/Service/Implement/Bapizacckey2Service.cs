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
    public partial class Bapizacckey2Service : ServiceBase<Bapizacckey2, long>, IBapizacckey2Service
    {
        public override IDao<Bapizacckey2, long> GetBaseDao()
        {
            return BapiDaoProvider.Bapizacckey2Dao;
        }
    }
}
