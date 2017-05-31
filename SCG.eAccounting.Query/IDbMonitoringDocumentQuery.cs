using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using SS.Standard.Data.Query.NHibernate;
using SCG.eAccounting.DTO;

namespace SCG.eAccounting.Query
{
    public interface IDbMonitoringDocumentQuery: IQuery<DbMonitoringDocument, string>
    {
        int CountMonitoringDocumentQuery(string comCode, string colnumber, string BuName, short languageID, string sortExpression);
        IList<DbMonitoringDocument> DataMonitoringDocumentQuery(string comCode, string colnumber, string BuName, short languageID, string sortExpression, int startRow, int pageSize);
    }
}
