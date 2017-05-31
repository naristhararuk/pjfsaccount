using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;

using SS.SU.DTO;
using SS.SU.DTO.ValueObject;
using SS.SU.DAL;

using NHibernate;
using NHibernate.Expression;
using NHibernate.Transform;

namespace SS.SU.DAL.Hibernate
{
    public class SuRTEContentDao : NHibernateDaoBase<SuRTEContent, short>, ISuRTEContentDao
    {
       
    }
}
