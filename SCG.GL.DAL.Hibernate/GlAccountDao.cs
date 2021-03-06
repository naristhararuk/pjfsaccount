using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;

using SCG.GL.DTO;
using SCG.GL.DAL;
using NHibernate;
using NHibernate.Expression;
using NHibernate.Transform;
using SS.Standard.Data.NHibernate.QueryCreator;

namespace SCG.GL.DAL.Hibernate
{
	/// <summary>
	/// POJO for GlAccount. This class is autogenerated
	/// </summary>
	public partial class GlAccountDao : NHibernateDaoBase<GlAccount , short>, IGlAccountDao
	{
        public GlAccountDao()
        { 
        
        }

        public GlAccount FindAccountByAccountNo(string accNo)
        {
            IQuery query = GetCurrentSession().CreateQuery("from GlAccount a Where a.AccNo =:AccNo");
            query.SetString("AccNo", accNo);
            return query.UniqueResult<GlAccount>();
        }
	}
}
