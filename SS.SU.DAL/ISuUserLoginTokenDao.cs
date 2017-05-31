using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Dao;
using SS.SU.DTO;

namespace SS.SU.DAL
{
    public interface ISuUserLoginTokenDao : IDao<SuUserLoginToken, Int64>
    {
        void DeleteUserToken(string username);
    }
}
