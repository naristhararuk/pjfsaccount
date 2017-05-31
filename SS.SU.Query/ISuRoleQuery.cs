using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

using SS.SU.DTO;
using SS.Standard.Data.Query;
using SS.Standard.Data.Query.NHibernate;
using NHibernate;
using SS.SU.DTO.ValueObject;

namespace SS.SU.Query
{
    public interface ISuRoleQuery : IQuery<SuRole, short>
    {
        new IList<SuRole> FindAll();
        //new SuRole FindByIdentity(short id);
        ISQLQuery FindByRoleCriteria(SuRole role, string sortExpression, bool isCount, short languageId);

        IList<SuRole> GetRoleList(SuRole role, short languageId, int firstResult, int maxResult, string sortExpression);

        int CountByRoleCriteria(SuRole role);

        //User Group
        ISQLQuery FindUserGroupByCriteria(SuRole role, bool isCount, string sortExpression);
        IList<SuRole> GetUserGroupList(SuRole userGroup, int firstResult, int maxResult, string sortExpression);
       
        int CountUserGroupByCriteria(SuRole userGroup);
        IList<SuRole> FindUserRoleCriteria(long userID);
    }
}
