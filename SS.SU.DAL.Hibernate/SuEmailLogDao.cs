using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using SS.Standard.Data.NHibernate.Dao;

using SS.SU.DTO;
using SS.SU.DAL;
using NHibernate.Expression;
using SS.SU.DTO.ValueObject;
using NHibernate;
using SS.Standard.Data.NHibernate.QueryCreator;
using NHibernate.Transform;

using SS.SU.DTO;
namespace SS.SU.DAL.Hibernate
{
    public partial class SuEMailLogDao : NHibernateDaoBase<SuEMailLog, long>, ISuEMailLogDao
    {
    }
}
