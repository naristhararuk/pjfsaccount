using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using NHibernate.Expression;
using NHibernate.Transform;

using SS.Standard.Data.NHibernate.Dao;

using SS.SU.DTO;
using SS.SU.DTO.ValueObject;
using SS.SU.DAL;
using SCG.DB.DTO;
namespace SS.SU.DAL.Hibernate
{
    public partial class SuEHrProfileLogDao : NHibernateDaoBase<SueHrProfileLog, long>, ISuEHrProfileLogDao
    {
        public void DeleteAll()
        {
            ISQLQuery query = GetCurrentSession().CreateSQLQuery("truncate table SueHrProfileLog");
            query.AddScalar("result", NHibernateUtil.Int32);
            query.UniqueResult();
        }
    }
}
