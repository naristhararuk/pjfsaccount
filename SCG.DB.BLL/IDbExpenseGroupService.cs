﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service;
using SCG.DB.DTO;

namespace SCG.DB.BLL
{
    public interface IDbExpenseGroupService : IService<DbExpenseGroup, long>
    {
        long AddExpenseGroup(DbExpenseGroup ep);
        void UpdateExpenseGroup(DbExpenseGroup ep);
        void DeleteExpenseGroup(DbExpenseGroup ep);
    }
}
