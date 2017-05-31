using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Query;
using SS.SU.DTO;
using NHibernate;

namespace SS.SU.Query
{
    public interface ISuEHRProfileLogQuery : IQuery <SueHrProfileLog,long>
    {

        ISQLQuery FindEHRProfileLog(bool isCount, string sortExpression);
        int CountEHRProfileLogByCriteria();
        IList<SueHrProfileLog> GeteHrProfileLogList(int firstResult, int maxResult, string sortExpression);
    }
}
