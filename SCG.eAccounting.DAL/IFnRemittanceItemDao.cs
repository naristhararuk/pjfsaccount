using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;

using SCG.eAccounting.DTO;
using SCG.eAccounting.DTO.DataSet;
using System.Data;

namespace SCG.eAccounting.DAL
{
	public interface IFnRemittanceItemDao : IDao<FnRemittanceItem, long>
	{
        void Persist(FnRemittanceDataset.FnRemittanceItemDataTable dtRemittanceItem);
        
	}
}
