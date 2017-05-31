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
using Spring.Transaction.Interceptor;
using SCG.DB.DTO;
using SCG.DB.DAL;

namespace SCG.eAccounting.DAL.Hibernate
{
	public class FnExpenseInvoiceItemDao : NHibernateDaoBase<FnExpenseInvoiceItem, long>, IFnExpenseInvoiceItemDao
	{
        #region Save
        public void Persist(DataTable dtInvoiceItem)
        {
            NHibernateAdapter<FnExpenseInvoiceItem, long> adapter = new NHibernateAdapter<FnExpenseInvoiceItem, long>();
            adapter.UpdateChange(dtInvoiceItem, ScgeAccountingDaoProvider.FnExpenseInvoiceItemDao);

            //return dtInvoiceItem.Rows[0].Field<long>(dtInvoiceItem.Columns["ExpenseID"]);
        }

        [Transaction]
        public override long Save(FnExpenseInvoiceItem invoiceItem)
        {
            if (invoiceItem.CostCenter != null)
            {
                DbCostCenter costcenter = ScgDbDaoProvider.DbCostCenterDao.FindProxyByIdentity(invoiceItem.CostCenter.CostCenterID);
                invoiceItem.CostCenter = costcenter;
            }
            if (invoiceItem.Account != null)
            {
                DbAccount account = ScgDbDaoProvider.DbAccountDao.FindProxyByIdentity(invoiceItem.Account.AccountID);
                invoiceItem.Account = account;
            }
            if (invoiceItem.IO != null)
            {
                DbInternalOrder io = ScgDbDaoProvider.DbIODao.FindProxyByIdentity(invoiceItem.IO.IOID);
                invoiceItem.IO = io;
            }

            return base.Save(invoiceItem);
        }

        [Transaction]
        public override void SaveOrUpdate(FnExpenseInvoiceItem invoiceItem)
        {
            if (invoiceItem.CostCenter != null)
            {
                DbCostCenter costcenter = ScgDbDaoProvider.DbCostCenterDao.FindProxyByIdentity(invoiceItem.CostCenter.CostCenterID);
                invoiceItem.CostCenter = costcenter;
            }
            if (invoiceItem.Account != null)
            {
                DbAccount account = ScgDbDaoProvider.DbAccountDao.FindProxyByIdentity(invoiceItem.Account.AccountID);
                invoiceItem.Account = account;
            }
            if (invoiceItem.IO != null)
            {
                DbInternalOrder io = ScgDbDaoProvider.DbIODao.FindProxyByIdentity(invoiceItem.IO.IOID);
                invoiceItem.IO = io;
            }

            base.SaveOrUpdate(invoiceItem);
        }
        #endregion
	}
}