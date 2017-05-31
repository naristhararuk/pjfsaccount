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
    public interface IFnRemittanceAdvanceService : IService<FnRemittanceAdvance, long>
    {
        long AddFnRemittanceAdvanceTransaction(Guid txID, FnRemittanceAdvance fnRemittanceAdvance);
        void UpdateRemittanceAdvanceTransaction(Guid txtID, FnRemittanceAdvance fnRemittanceAdvance);
        DataTable DeleteRemittanceAdvanceFromTransaction(Guid txID, long advanceID, long remittanceID);
        //void InsertRemittanceAdvance(Guid txID, long tempfnRemittanceID);
        //void UpdateRemittanceAdvance(Guid txID, long tempfnRemittanceID);
        //void DeleteRemittanceAdvance(Guid txID);
        void PrepareDataToDataset(FnRemittanceDataset remittanceDataset, long remittanceID);
        void ValidateRemittanceAdvance(FnRemittanceAdvance remittanceAdvance);
        void SaveRemittanceAdvance(Guid txID, long remittanceID);
    }
}
