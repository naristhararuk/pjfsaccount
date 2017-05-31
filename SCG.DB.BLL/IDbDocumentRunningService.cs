using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

using SCG.DB.DTO;

namespace SCG.DB.BLL
{
    public interface IDbDocumentRunningService : IService<DbDocumentRunning , int>
    {
        string RetrieveNextDocumentRunningNo(int year, int documentTypeID, long companyID);
        
    }
}
