using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.DB.DTO;
using SS.Standard.Data.NHibernate.Service.Implement;
using SCG.DB.DAL;
using SS.Standard.Data.NHibernate.Dao;

namespace SCG.DB.BLL.Implement
{
    public partial class DbAccountCompanyService : ServiceBase<DbAccountCompany, long>, IDbAccountCompanyService
    {
        public override IDao<DbAccountCompany, long> GetBaseDao()
        {
            return ScgDbDaoProvider.DbAccountCompanyDao;
        }
        public void AddAccountCompanyList(IList<DbAccountCompany> account)
        {
            foreach (DbAccountCompany ac in account)
            {
                ScgDbDaoProvider.DbAccountCompanyDao.Save(ac);
            }
        }
    }
}
