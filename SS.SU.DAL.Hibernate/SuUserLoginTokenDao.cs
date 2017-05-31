using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Dao;
using SS.SU.DTO;
using NHibernate;

namespace SS.SU.DAL.Hibernate
{
    public partial class SuUserLoginTokenDao : NHibernateDaoBase<SuUserLoginToken, Int64>, ISuUserLoginTokenDao
    {
        public void DeleteUserToken(string username)
        {
            ISQLQuery query = GetCurrentSession().CreateSQLQuery("DELETE FROM SuUserLoginToken WHERE UserName = :username ");
            query.SetString("username",username);
            query.AddScalar("count", NHibernateUtil.Int64).UniqueResult();
        }
    }
}
