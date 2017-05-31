using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using NHibernate.Expression;
using NHibernate.Transform;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.QueryCreator;

using SCG.eAccounting.DAL;
using SCG.eAccounting.DTO;
using SCG.eAccounting.DTO.ValueObject;
using System.Data;
using SCG.DB.DTO;
using SCG.DB.DAL;
using Spring.Transaction.Interceptor;

namespace SCG.eAccounting.DAL.Hibernate
{
    public class FnExpensePerdiemDao : NHibernateDaoBase<FnExpensePerdiem, long>, IFnExpensePerdiemDao
    {
        #region Save
        public void Persist(DataTable dtExpensePerdiem)
        {
            NHibernateAdapter<FnExpensePerdiem, long> adapter = new NHibernateAdapter<FnExpensePerdiem, long>();
            adapter.UpdateChange(dtExpensePerdiem, ScgeAccountingDaoProvider.FnExpensePerdiemDao);
        }
        #endregion

        [Transaction]
        public override long Save(FnExpensePerdiem perdiem)
        {
            if (perdiem.CostCenter != null)
            {
                DbCostCenter costcenter = ScgDbDaoProvider.DbCostCenterDao.FindProxyByIdentity(perdiem.CostCenter.CostCenterID);
                perdiem.CostCenter = costcenter;
            }
            if (perdiem.Account != null)
            {
                DbAccount account = ScgDbDaoProvider.DbAccountDao.FindProxyByIdentity(perdiem.Account.AccountID);
                perdiem.Account = account;
            }
            if (perdiem.IO != null)
            {
                DbInternalOrder io = ScgDbDaoProvider.DbIODao.FindProxyByIdentity(perdiem.IO.IOID);
                perdiem.IO = io;
            }

            return base.Save(perdiem);
        }

        [Transaction]
        public override void SaveOrUpdate(FnExpensePerdiem perdiem)
        {
            if (perdiem.CostCenter != null)
            {
                DbCostCenter costcenter = ScgDbDaoProvider.DbCostCenterDao.FindProxyByIdentity(perdiem.CostCenter.CostCenterID);
                perdiem.CostCenter = costcenter;
            }
            if (perdiem.Account != null)
            {
                DbAccount account = ScgDbDaoProvider.DbAccountDao.FindProxyByIdentity(perdiem.Account.AccountID);
                perdiem.Account = account;
            }
            if (perdiem.IO != null)
            {
                DbInternalOrder io = ScgDbDaoProvider.DbIODao.FindProxyByIdentity(perdiem.IO.IOID);
                perdiem.IO = io;
            }

            base.SaveOrUpdate(perdiem);
        }
    }
}
