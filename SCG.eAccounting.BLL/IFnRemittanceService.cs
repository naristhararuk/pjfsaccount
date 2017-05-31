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
    public interface IFnRemittanceService : IService<FnRemittance, long>
	{
        DataSet PrepareDS();
        DataSet PrepareDS(long documentID);
        long AddFnRemittanceDocumentTransaction(Guid txID);
        void UpdateRemittanceDocumentTransaction(Guid txtID, FnRemittance fnRemittance, bool IsHeaderAndFooter);
        void ValidateRemittance(FnRemittance remittance, bool isHeaderAndFooter);
        FnRemittanceDataset PrepareDataToDataset(long documentID, bool isCopy);
        IList<object> GetVisibleFields(long? documentID);
        IList<object> GetEditableFields(long? documentID);
        void UpdateHeaderAndFooterToTransaction(Guid txid, SCGDocument document, FnRemittance remittance);
        void ValidateRemittanceAdvance(Guid txID, long remittnceID);
        long SaveRemittanceDocument(Guid txID, long remittanceID);
        void UpdateTotalRemittanceAmount(Guid txID, long remittanceId);
	}
}
