using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

using SCG.DB.DTO;
using SS.Standard.Data.Query;
using SCG.DB.DTO.ValueObject;
using SS.Standard.Data.Query.NHibernate;
using NHibernate;

namespace SCG.DB.Query
{
    public interface IDbServiceTeamLocationQuery : IQuery <DbServiceTeamLocation , long>
    {
        ISQLQuery FindServiceTeamLocationByServiceTeamID(DbServiceTeam serviceTeam, bool isCount, string sortExpression);
        IList<ServiceTeamLocationResult> GetServiceTeamLocationList(DbServiceTeam serviceTeam, int firstResult, int maxResult, string sortExpression);
        int CountServiceTeamLocationByServiceTeamID(DbServiceTeam serviceTeam);

        IList<DbServiceTeamLocation> FindServiceTeamByLocationID(long locationId);
    }
}
