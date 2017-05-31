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
    public partial class Bapiache09Service : ServiceBase<Bapiache09, long>, IBapiache09Service
    {
        public override IDao<Bapiache09, long> GetBaseDao()
        {
            return BapiDaoProvider.Bapiache09Dao;
        }
    }
}
