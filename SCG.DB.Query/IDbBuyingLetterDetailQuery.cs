﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.Query.NHibernate;
using SCG.DB.DTO;

namespace SCG.DB.Query
{
    public interface IDbBuyingLetterDetailQuery : IQuery<DbBuyingLetterDetail, long>
    {
        long GetLetterIDByLetterNo(string letterNo);
    }
}