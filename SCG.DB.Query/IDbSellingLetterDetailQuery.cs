﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.DB.DTO.ValueObject;
using SS.Standard.Data.Query.NHibernate;
using SCG.DB.DTO;

namespace SCG.DB.Query
{
    public interface IDbSellingLetterDetailQuery : IQuery<DbSellingLetterDetail, long>
    {
        long GetLetterIDByLetterNo(string letterNo);
    }
}
