using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.DB.DTO;
using SS.Standard.Data.NHibernate.Dao;
using NHibernate;

namespace SS.DB.DAL
{
    public interface IDbRegionDao : IDao<DbRegion, short>
    {
    }
}
