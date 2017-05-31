using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.DB.DTO;
using SS.Standard.Data.NHibernate.Dao;

namespace SCG.DB.DAL.Hibernate
{
    public partial class DbAccountCompanyDao : NHibernateDaoBase<DbAccountCompany, long>, IDbAccountCompanyDao
    {
    }
}
