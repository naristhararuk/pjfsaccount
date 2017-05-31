using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.Standard.Data.Interfaces
{
    public interface IDBManager
    {
        void OpenConnection();
        void CloseConnection();
        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();

    }
}
