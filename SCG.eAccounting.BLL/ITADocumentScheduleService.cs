using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service;
using SCG.eAccounting.DTO;
using SCG.eAccounting.DTO.DataSet;
using System.Data;

namespace SCG.eAccounting.BLL
{
    public interface ITADocumentScheduleService : IService<TADocumentSchedule, int>
    {
        void PrepareDataToDataset(TADocumentDataSet taDocumentDS, long taDocumentID);
        int AddTADocumentScheduleTransaction(Guid txID, TADocumentSchedule taDocumentSchedule);
        void UpdateTADocumentScheduleTransaction(Guid txID, TADocumentSchedule taDocumentSchedule);
        void SaveTADocumentSchedule(Guid txID, long taDocumentID);
        DataTable DeleteTADocumentScheduleTransaction(Guid txID, int scheduleID);
    }
}
