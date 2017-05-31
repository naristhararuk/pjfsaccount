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
    public interface ITADocumentTravellerService : IService<TADocumentTraveller, int>
    {
        void PrepareDataToDataset(TADocumentDataSet taDocumentDS, long taDocumentID);
        int AddTADocumentTravellerTransaction(Guid txID, TADocumentTraveller taDocumentTraveller);
        void UpdateTADocumentTravellerTransaction(Guid txID, TADocumentTraveller taDocumentTraveller);
        void SaveTADocumentTraveller(Guid txID, long taDocumentID);
        DataTable DeleteTADocumentTravellerTransaction(Guid txID, int travellerID);
        void ChangeRequesterTraveller(Guid txID, TADocumentTraveller taDocumentTraveller);
    }
}
