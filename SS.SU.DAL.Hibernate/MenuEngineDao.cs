using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using NHibernate.Transform;

using SS.Standard.Security;

using SS.Standard.Data.NHibernate.Dao;

using SS.SU.DTO;
namespace SS.SU.DAL.Hibernate
{
    public partial class MenuEngineDao : NHibernateDaoBase<SuMenu, short>, IMenuEngineDao
    {
    }
}
