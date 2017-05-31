using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.QueryDao;
using SS.Standard.Data.NHibernate.QueryCreator;
using SCG.DB.DTO;
using SCG.DB.DTO.ValueObject;
using SCG.DB.Query;
using NHibernate;

namespace SCG.DB.Query.Hibernate
{
    public class DbDocumentRunningQuery : NHibernateQueryBase<DbDocumentRunning , int> , IDbDocumentRunningQuery
    {
        public DbDocumentRunning GetDocumentRunningByDocumentTypeID_CompanyID_Year(int documentTypeID, long companyID, int year)
        {
            DbDocumentRunning documentRunning = GetCurrentSession().CreateQuery("from DbDocumentRunning where DocumentTypeID = :DocumentTypeID and CompanyID = :CompanyID and Year = :Year and active = '1'")
            .SetInt32("DocumentTypeID", documentTypeID)
            .SetInt64("CompanyID", companyID)
            .SetInt32("Year", year)
            .UniqueResult<DbDocumentRunning>();

            return documentRunning;
        }
    }
}
