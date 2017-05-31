using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service;
using SCG.eAccounting.DTO;

namespace SCG.eAccounting.BLL
{
    public interface IFnEACAdvanceImportLogService:IService<FnEACAdvanceImportLog,long>
    {
        void SaveLog(string EACAdvanceNo, bool status, string message);
        void SetActiveFalse();
    }
}
