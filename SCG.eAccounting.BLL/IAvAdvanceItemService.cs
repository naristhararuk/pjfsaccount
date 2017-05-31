using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using SS.Standard.Data.NHibernate.Service;

using SCG.eAccounting.DTO;
using SCG.eAccounting.DTO.ValueObject;
using SCG.eAccounting.DTO.DataSet;

namespace SCG.eAccounting.BLL
{
	public interface IAvAdvanceItemService : IService<AvAdvanceItem, long>
	{
        void PrepareDataToDataset(AdvanceDataSet advanceDs, long advanceID,bool isCopy);
        long AddAdvanceItem(Guid TxID, AvAdvanceItem avAdvanceItem);
        void InsertAvAdvanceItem(Guid txID, long advanceID);
        void UpdateAdvanceItemTransaction(Guid txID, AvAdvanceItem avAdvanceItem, bool isRepOffice);
        void DeleteAdvanceDocument(Guid txID);
        void saveAdvanceItem(Guid txID, long advanceid);
        DataTable DeleteAdvanceItemFromTransaction(Guid txID, long advanceItemID);
        void updateGrid(Guid txID, AvAdvanceItem avAdvanceItem);
	}
}
