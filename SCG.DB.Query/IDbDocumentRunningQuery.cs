using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;

using SS.Standard.Data.Query.NHibernate;
using SS.Standard.Data.NHibernate.Service;

using SCG.DB.DTO;
using SCG.DB.DTO.ValueObject;

namespace SCG.DB.Query
{
    public interface IDbDocumentRunningQuery : IQuery<DbDocumentRunning , int>
    {
        DbDocumentRunning GetDocumentRunningByDocumentTypeID_CompanyID_Year(int documentTypeID, long companyID, int year);
    }
}
