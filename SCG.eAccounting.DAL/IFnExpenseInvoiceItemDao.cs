﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SCG.eAccounting.DTO;
using NHibernate;
using System.Data;

namespace SCG.eAccounting.DAL
{
    public interface IFnExpenseInvoiceItemDao : IDao<FnExpenseInvoiceItem, long>
    {
        void Persist(DataTable dtInvoiceItem);
    }
}
