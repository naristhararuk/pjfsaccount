using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.Query.NHibernate;
using SS.SU.DTO;
using SS.SU.DTO.ValueObject;

namespace SS.SU.Query
{
    public interface ISuRoleServiceQuery : IQuery<SuRoleService, long>
    {
        IList<ServiceTeamInformation> GetServiceTeamInformation(short roleID, string sortExpression);
        IList<SuRole> GetRoleByServiceTeamID(long serviceTeamID);
    }
}
