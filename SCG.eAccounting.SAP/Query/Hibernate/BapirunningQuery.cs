using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Transform;
using SS.Standard.Data.NHibernate.Query;
using SS.Standard.Data.NHibernate.QueryDao;
using SS.Standard.Data.NHibernate.QueryCreator;
using SCG.eAccounting.SAP.DTO;
using SCG.eAccounting.SAP.Query.Interface;

namespace SCG.eAccounting.SAP.Query.Hibernate
{
    public class BapirunningQuery : NHibernateQueryBase<Bapirunning, long>, IBapirunningQuery
    {
        #region public IList<Bapirunning> FindByYearPeriod(string Year, string Period)
        public IList<Bapirunning> FindByYearPeriod(string Year, string Period)
        {
            return GetCurrentSession().CreateQuery("FROM Bapirunning A WHERE  A.Year = :YEAR AND A.Period = :PERIOD")
                .SetString("YEAR", Year)
                .SetString("PERIOD", Period)
                .List<Bapirunning>();
        }
        #endregion public IList<Bapirunning> FindByYearPeriod(string Year, string Period)
    }
}
