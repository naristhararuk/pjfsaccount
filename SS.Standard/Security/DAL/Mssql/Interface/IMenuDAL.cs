using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.Standard.Security.Mssql.DAL.Interface
{
    public interface IMenuDAL
    {

        void OpenConnection();
        void CloseConnection();
        List<UserMenu> GetMenuData();

    }
}
