using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

using SCG.DB.DTO;
using SS.Standard.Data.Query;
using SS.Standard.Data.Query.NHibernate;
using NHibernate;
using SCG.DB.DTO.ValueObject;

namespace SCG.DB.Query
{
    public interface IDbIOQuery : IQuery<DbInternalOrder, long>
    {
        ISQLQuery FindDataByIOCriteria(DbInternalOrder io, bool isCount, string sortExpression);
        ISQLQuery FindByIOCriteria(DbInternalOrder io, bool isCount, string sortExpression);
        IList<DbInternalOrder> GetIOList(DbInternalOrder io, int firstResult, int maxResult, string sortExpression);
        IList<DbInternalOrder> GetInternalOrderList(DbInternalOrder io, int firstResult, int maxResult, string sortExpression);
        int CountByIOCriteria(DbInternalOrder io);
        int CountDataByIOCriteria(DbInternalOrder io);
        IList<DbInternalOrder> FindAutoComplete(string prefixText, IOAutoCompleteParameter param);
        DbInternalOrder getDbInternalOrderByIONumber(string ioNumber);
        IList<DbInternalOrder> FindIOByCompanyID(long companyId);
    }
}
