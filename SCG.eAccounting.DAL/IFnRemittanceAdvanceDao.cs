﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;

using SCG.eAccounting.DTO;
using SCG.eAccounting.DTO.DataSet;

namespace SCG.eAccounting.DAL
{
    public interface IFnRemittanceAdvanceDao : IDao<FnRemittanceAdvance, long>
	{
        void Persist(FnRemittanceDataset.FnRemittanceAdvanceDataTable dtRemittanceAdvance);
	}
}
