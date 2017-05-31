using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;

using SS.Standard.Data.Query;
using SS.Standard.Data.Query.NHibernate;

using SS.SU.DTO.ValueObject;
using SS.SU.DTO;

namespace SS.SU.Query
{
    public interface ISuRolePBQuery : IQuery<SuRolepb,long>
    {
        ISQLQuery FindRolePBByCriteria(SuRole rolePB, bool isCount, string sortExpression, short languageID);
        IList<PBInformation> FindPBInfoByRole(SuRole role, int firstResult, int maxResult, string sortExpression, short languageID);
        int FindCount(SuRole role,short languageID);
        IList<SuRole> FindRoleByPBID(long pbID);
    }
}
