using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service.Implement;
using SCG.eAccounting.DTO;
using SCG.eAccounting.DAL;
namespace SCG.eAccounting.BLL.Implement
{
    public class FnAutoPaymentService : ServiceBase<FnAutoPayment,long>,IFnAutoPaymentService
    {
        public override SS.Standard.Data.NHibernate.Dao.IDao<FnAutoPayment, long> GetBaseDao()
        {
            return ScgeAccountingDaoProvider.FnAutoPaymentDao;
        }

        public void ImportFreshNewImagePosting()
        {
            ScgeAccountingDaoProvider.DbDocumentImagePostingDao.ImportFreshNewImagePosting();
        }
       
    }
}
