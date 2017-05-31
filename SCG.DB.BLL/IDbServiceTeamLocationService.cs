using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;
using SCG.DB.DTO;
using SCG.DB.DTO.ValueObject;

namespace SCG.DB.BLL
{
    public interface IDbServiceTeamLocationService : IService <DbServiceTeamLocation , long>
    {
        void AddServiceTeamLocationList(IList<DbServiceTeamLocation> serviceTeamLocationList);
    }
}
