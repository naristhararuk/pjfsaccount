using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.QueryCreator;

using SCG.eAccounting.DTO;
using SCG.eAccounting.DAL;
using SCG.eAccounting.DTO.DataSet;

using NHibernate;
using NHibernate.Expression;
using NHibernate.Transform;

namespace SCG.eAccounting.DAL.Hibernate
{
	public class SuEmailLogDao : NHibernateDaoBase<SuEmailLog, long>, ISuEmailLogDao
    {
        public SuEmailLogDao()
        {
        }
	}
}
