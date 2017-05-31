using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using SCG.eAccounting.DTO;
using SS.Standard.Data.Query.NHibernate;

namespace SCG.eAccounting.Query
{
    public interface IDbMonitoringInboxQuery : IQuery<DocumentMonitoringInbox, string>
    {
            //int CountMonitoringQuery(MonitoringInBoxSearchCriteria Criteria);
            IList<DocumentMonitoringInbox> DataMonitoringInBox(MonitoringInBoxSearchCriteria Criteria);
    }
}
