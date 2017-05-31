using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.DB.DTO;
using SS.Standard.Data.Query.NHibernate;
using SCG.DB.DTO.ValueObject;
using NHibernate;

namespace SCG.DB.Query
{
    public interface IDbSapInstanceQuery : IQuery<DbSapInstance, string>
    {
        IList<DbSapInstance> FindALL();
        IList<SapInstanceData> GetSapInstanceList();
        ISQLQuery FindSapInstanceByCriteria(SapInstanceCriteria criteria, string sortExpression, bool isCount);
        IList<DbSapInstance> GetSapInstanceListByCriteria(SapInstanceCriteria criteria, int startRow, int pageSize, string sortExpression);
        int CountSapInstance(SapInstanceCriteria criteria);
    }
}
