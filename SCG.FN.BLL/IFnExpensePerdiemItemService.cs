using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;
using SCG.FN.DTO;
using SCG.FN.DTO.ValueObject;

namespace SCG.FN.BLL
{ 
    public interface IFnExpensePerdiemItemService : IService<FnExpensePerdiemItem , long>
    {
        void AddExpensePerdiemItemOnTransaction(FnExpensePerdiemItem item, Guid txId);
        void UpdateExpensePerdiemItemOnTransaction(FnExpensePerdiemItem item, Guid txId);
        void DeleteExpensePerdiemItemOnTransaction(Guid txId, long itemId);
        decimal ComputeTotalDay(DateTime fromDateTime, DateTime toDateTime);
        decimal ComputeNetDay(DateTime fromDateTime, DateTime toDateTime, decimal adjustedDay);
        decimal ComputeAmount(DateTime fromDateTime, DateTime toDateTime, decimal  adjustedDay,decimal perdiemRate);
    }
}
