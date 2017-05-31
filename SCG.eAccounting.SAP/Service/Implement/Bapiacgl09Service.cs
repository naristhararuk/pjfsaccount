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
    public partial class Bapiacgl09Service : ServiceBase<Bapiacgl09, long>, IBapiacgl09Service
    {
        public override IDao<Bapiacgl09, long> GetBaseDao()
        {
            return BapiDaoProvider.Bapiacgl09Dao;
        }
    }
}
