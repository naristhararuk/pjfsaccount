using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;

using SCG.DB.DTO;
using SCG.DB.DAL;
using NHibernate;
using NHibernate.Expression;
using NHibernate.Transform;
using SS.Standard.Data.NHibernate.QueryCreator;

namespace SCG.DB.DAL.Hibernate
{
    public partial class DbServiceTeamDao : NHibernateDaoBase <DbServiceTeam , long >, IDbServiceTeamDao
    {
        public bool IsDuplicateServiceTeamCode(DbServiceTeam serviceTeam)
        {
            IList<DbServiceTeam> list = GetCurrentSession().CreateQuery("from DbServiceTeam p where p.ServiceTeamID <> :ServiceTeamID and p.ServiceTeamCode = :ServiceTeamCode")
                  .SetInt64("ServiceTeamID", serviceTeam.ServiceTeamID)
                  .SetString("ServiceTeamCode", serviceTeam.ServiceTeamCode)
                  .List<DbServiceTeam>();
            if (list.Count > 0)
            {
                return true;
            }
            return false;
        }
    }
}
