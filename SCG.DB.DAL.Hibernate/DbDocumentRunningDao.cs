using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using NHibernate.Expression;
using NHibernate.Transform;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.QueryCreator;

using SCG.DB.DTO;
using SCG.DB.DAL;

namespace SCG.DB.DAL.Hibernate
{
    public class DbDocumentRunningDao : NHibernateDaoBase<DbDocumentRunning , int> , IDbDocumentRunningDao
    {
    }
}
