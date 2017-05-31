using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service;
using SS.SU.DTO;

namespace SS.SU.BLL
{
    public interface ISuUserLoginTokenService : IService<SuUserLoginToken, Int64>
    {
        string InsertToken();
        void DeleteToken(string userName);
        SuUserLoginToken CheckUserAndToken(string userName, string token);
    }
}
