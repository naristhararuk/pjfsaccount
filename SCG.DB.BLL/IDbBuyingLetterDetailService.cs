﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

using SCG.DB.DTO.ValueObject;
using SCG.DB.DTO;

namespace SCG.DB.BLL
{
    public interface IDbBuyingLetterDetailService : IService<DbBuyingLetterDetail, long>
    {
        long AddLetterDetail(MoneyRequestSearchResult moneyRequest, string letterNo);
    }
}