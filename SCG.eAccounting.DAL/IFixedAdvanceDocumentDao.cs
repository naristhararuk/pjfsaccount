﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.eAccounting.DTO;
using SS.Standard.Data.NHibernate.Dao;
using SCG.eAccounting.DTO.DataSet;

namespace SCG.eAccounting.DAL
{
    public interface IFixedAdvanceDocumentDao : IDao<FixedAdvanceDocument, long>
    {
        long Persist(FixedAdvanceDataSet.FixedAdvanceDocumentDataTable dtFixedAdvanceDocument);
    }
}
