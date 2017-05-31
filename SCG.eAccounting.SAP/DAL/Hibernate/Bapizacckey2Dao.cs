using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Expression;
using NHibernate.Transform;
using SS.Standard.Data.NHibernate.QueryCreator;
using SS.Standard.Data.NHibernate.Dao;
using SCG.eAccounting.SAP.DTO;
using SCG.eAccounting.SAP.DAL.Interface;

namespace SCG.eAccounting.SAP.DAL.Hibernate
{
    public partial class Bapizacckey2Dao : NHibernateDaoBase<Bapizacckey2, long>, IBapizacckey2Dao
    {
    }
}
