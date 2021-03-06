﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.Query.NHibernate;
using SCG.DB.DTO;

namespace SCG.DB.Query
{
    public interface IDbPBCurrencyQuery : IQuery<DbPBCurrency,long>
    {
        IList<DbPBCurrency> FindPBCurrencyByPBID(long pbID);
        IList<DbPBCurrency> FindPBLocalCurrencyByPBID(long pbID,string currencyType);
    }
}
