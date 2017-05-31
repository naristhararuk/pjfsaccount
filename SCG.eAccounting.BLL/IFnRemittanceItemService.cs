using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

using SCG.eAccounting.DTO;
using System.Data;
using SCG.eAccounting.DTO.DataSet;

namespace SCG.eAccounting.BLL
{
    public interface IFnRemittanceItemService : IService<FnRemittanceItem, long>
    {
        void PrepareDataToDataset(FnRemittanceDataset remittanceDataset, long remittanceID);
        long AddFnRemittanceItemTransaction(Guid txID, FnRemittanceItem fnRemittanceItem, bool isFromAddvance, double totalAdvance);
        void UpdateRemittanceItemTransaction(Guid txtID, FnRemittanceItem fnRemittanceItem, bool isFromAddItemIngrid);
        DataTable DeleteRemittanceItemFromTransaction(Guid txID, long fnremittrnceItemID, bool isFromAdvanceGrid);

        void ValidateRemittanceItem(FnRemittanceItem remittanceItem, bool isFromAddItemInGrid);
        void SaveRemittanceItem(Guid txID, long remittanceID);
        //void DeleteRemittanceItemTransaction(Guid txID, FnRemittanceItem fnRemittanceItem, bool isFromAdvanceGrid);
        void UpdateRemittanceItemList(Guid txID, long remittanceId, List<FnRemittanceItem> remittanceItemList, bool isFullClearing);
    }
}
