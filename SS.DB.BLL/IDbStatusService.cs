using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

using SS.DB.DTO;

namespace SS.DB.BLL
{
    public interface IDbStatusService : IService<DbStatus, short>
    {
        IList<DbStatus> FindByDbStatusCriteria(DbStatus criteria, int firstResult, int maxResults, string sortExpression);
        int CountByDbStatusCriteria(DbStatus criteria);
        short AddStatus(DbStatus status);
        void UpdateStatus(DbStatus status);
    }
}
