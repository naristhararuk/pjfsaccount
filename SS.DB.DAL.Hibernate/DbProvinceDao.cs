using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Transform;
using SS.Standard.Data.NHibernate.Dao;

using SS.DB.DTO;
using SS.DB.DAL;
using NHibernate;
using NHibernate.Expression;
using SS.Standard.Data.NHibernate.QueryCreator;

namespace SS.DB.DAL.Hibernate
{
    public partial class DbProvinceDao : NHibernateDaoBase<DbProvince, short>, IDbProvinceDao
    {
    }
}
