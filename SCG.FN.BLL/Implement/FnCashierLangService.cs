using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;
using SS.Standard.Utilities;

using SCG.FN.DAL;
using SCG.FN.DTO;
using SCG.FN.BLL;
using SCG.FN.DTO.ValueObject;

namespace SCG.FN.BLL.Implement
{
    public partial class FnCashierLangService : ServiceBase<FnCashierLang, long>, IFnCashierLangService
    {
        public override IDao<FnCashierLang, long> GetBaseDao()
        {
            return DaoProvider.FnCashierLangDao;
        }
        public void UpdateCashierLang(IList<FnCashierLang> cashierLangList)
        {
            foreach (FnCashierLang c in cashierLangList)
            {
                DaoProvider.FnCashierLangDao.DeleteCashierLangByCashierId(c.CashierID);
            }

            foreach (FnCashierLang c in cashierLangList)
            {
                DaoProvider.FnCashierLangDao.Save(c);
            }
        }
    }
}
