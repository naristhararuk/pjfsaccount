using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.eAccounting.DTO;
using SS.Standard.Data.NHibernate.Dao;
using SCG.eAccounting.DTO.DataSet;

namespace SCG.eAccounting.DAL
{
    public interface IMPAItemDao : IDao<MPAItem, long>
    {
        void Persist(MPADocumentDataSet.MPAItemDataTable mpaItemDT);
    }
}
