using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Query;

using SS.DB.DTO;
using NHibernate;

namespace SS.DB.Query
{
    public interface IDbParameterGroupQuery : IQuery<DbParameterGroup, short>
    {
        IList<DbParameterGroup> GetParameterGroupList(DbParameterGroup parameterGroup, int firstResult, int maxResult, string sortExpression);
        int CountByParameterGroupCriteria(DbParameterGroup parameterGroup);
        ISQLQuery FindByParameterGroupCriteria(DbParameterGroup parameterGroup, bool isCount, string sortExpression);
    }
}