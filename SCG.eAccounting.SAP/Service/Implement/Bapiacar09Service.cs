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
    public partial class Bapiacar09Service : ServiceBase<Bapiacar09, long>, IBapiacar09Service
    {
        public override IDao<Bapiacar09, long> GetBaseDao()
        {
            return BapiDaoProvider.Bapiacar09Dao;
        }
    }
}
