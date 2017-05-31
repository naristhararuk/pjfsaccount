using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service.Implement;
using SCG.eAccounting.DTO;
using SCG.eAccounting.DAL; 

namespace SCG.eAccounting.BLL.Implement
{
    public partial class FnEHRexpenseTempService : ServiceBase<FnehRexpenseTemp,long>,IFnEHRexpenseTempService
    {

        public override SS.Standard.Data.NHibernate.Dao.IDao<FnehRexpenseTemp, long> GetBaseDao()
        {
            return ScgeAccountingDaoProvider.FnEHRexpenseTempDao;
        }

    }
}
