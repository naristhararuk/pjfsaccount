using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Dao;
using SCG.eAccounting.DTO;

namespace SCG.eAccounting.DAL
{
    public interface IFnEACAdvanceImportTempDao : IDao<FneacAdvanceImportTemp,long>
    {
        void ClearTempData();
  
        void ResolveRequesterID();
        void ResolveReceiver();
        void ResolveApproverID();
        void ResolveCompany();
        void ResolveExpenseNo();
        void DeleteFailFromLog();
        void ResolveDocumentNo();
        void SaveToDataBase();
        void SaveSuccessToLog();
        void ResolvePBCode();
    }
}
