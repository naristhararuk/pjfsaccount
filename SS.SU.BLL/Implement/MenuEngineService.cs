using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service.Implement;
using SS.Standard.Data.NHibernate.Dao;

using SS.SU.DTO;
using SS.SU.DAL;
using SS.SU.Query;


using SS.SU.Helper;

namespace SS.SU.BLL.Implement
{
    public partial class MenuEngineService : ServiceBase<SuMenu, short>, IMenuEngineService
    {
        public override IDao<SuMenu, short> GetBaseDao()
        {
            return DaoProvider.MenuEngineDao;
        }

        #region IMenuService Members



        #endregion




       
    }
}
