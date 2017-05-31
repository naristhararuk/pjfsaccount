using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Dao;
using SCG.eAccounting.DTO;
using NHibernate;

namespace SCG.eAccounting.DAL.Hibernate
{
    public partial class FnEACAdvanceImportLogDao : NHibernateDaoBase<FnEACAdvanceImportLog, long>, IFnEACAdvanceImportLogDao
    {
        public void SetAllActiveFalse()
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("update FnEACAdvanceImportLog set active = 0 ");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.AddScalar("Count", NHibernateUtil.Int32);
            query.UniqueResult();
        }
    }
}

