using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.eAccounting.DTO;
using SS.Standard.Data.Query.NHibernate;
using NHibernate;
using SS.Standard.Data.NHibernate.QueryDao;
using NHibernate.Transform;

namespace SCG.eAccounting.Query.Hibernate
{
    public class DbMonitoringInboxQuery : NHibernateQueryBase<DocumentMonitoringInbox, string>, IDbMonitoringInboxQuery
    {
        public IList<DocumentMonitoringInbox> DataMonitoringInBox(MonitoringInBoxSearchCriteria criteria) 
        {
            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery("EXECUTE  FindMonitoringInbox :CompanyCode,:BUCode,:DateFrom,:DateTo");
            query.SetString("CompanyCode", criteria.Company)
                 .SetString("BUCode", criteria.BusinessGroup)
                 .SetString("DateFrom", !criteria.FromDate.HasValue ? string.Empty : criteria.FromDate.Value.ToString(culture))
                 .SetString("DateTo", !criteria.ToDate.HasValue ? string.Empty : criteria.ToDate.Value.ToString(culture));

            query.AddScalar("CompanyCode", NHibernateUtil.String)
                 .AddScalar("BuName", NHibernateUtil.String)
                 .AddScalar("Col1", NHibernateUtil.Int32)
                 .AddScalar("Col2", NHibernateUtil.Int32)
                 .AddScalar("Col3", NHibernateUtil.Int32)
                 .AddScalar("Col4", NHibernateUtil.Int32)
                 .AddScalar("Col5", NHibernateUtil.Int32);
            query.SetResultTransformer(Transformers.AliasToBean(typeof(DocumentMonitoringInbox)));
            DocumentMonitoringInbox result = new DocumentMonitoringInbox();
            IList<DocumentMonitoringInbox> list = query.List<DocumentMonitoringInbox>();
            return list;
        }
    }
}
