﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Dao;
using SCG.eAccounting.DTO;

namespace SCG.eAccounting.DAL.Hibernate
{
    public class FnPerdiemRateDao : NHibernateDaoBase<FnPerdiemRate, long>, IFnPerdiemRateDao
    {
        public FnPerdiemRateDao()
        {
        }
    }
}
