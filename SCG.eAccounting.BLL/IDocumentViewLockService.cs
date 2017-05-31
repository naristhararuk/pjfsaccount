using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.eAccounting.DTO;
using SS.Standard.Data.NHibernate.Service;

using SCG.eAccounting.Query;
using SCG.eAccounting.DTO.ValueObject;
using System.Data;

namespace SCG.eAccounting.BLL
{
    public interface IDocumentViewLockService : IService<DocumentViewLock, long>
    {
        bool IsDocumentLocked(long documentID, long userID, ref string lockByEmployeeName, ref bool IsOwner, ref DateTime lockDate);
        bool TryLock(long documentID, long userID, bool LockFlag);
        bool ForceLock(long documentID, long userID);
        bool UnLock(long documentID, long userID);
    }
}
