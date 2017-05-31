using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;
using SCG.eAccounting.DTO;
using SCG.eAccounting.DTO.ValueObject;
using SCG.eAccounting.DTO.DataSet;

namespace SCG.eAccounting.BLL
{
    public interface IFnExpenseMileageInvoiceService : IService<FnExpenseMileageInvoice, long>
    {
        void AddMileageInvoice(Guid txID, long mileageId, long invoiceId);
        void DeleteMileageInvoice(Guid txID, long mileageId);
        void SaveMileageInvoice(Guid txID, long mileageId);
        void PrepareDataToDataset(ExpenseDataSet ds, long mileageId);
    }
}
