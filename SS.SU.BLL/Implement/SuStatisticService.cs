using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service.Implement;
using SS.SU.DTO;
using SS.SU.DAL;

namespace SS.SU.BLL.Implement
{
    public class SuStatisticService :  ServiceBase<SuStatistic,int>,ISuStatisticService
    {
        public override SS.Standard.Data.NHibernate.Dao.IDao<SuStatistic, int> GetBaseDao()
        {
            return DaoProvider.SuStatisticDao;
        }

        #region ISuStatisticService Members

        public void IncreaseUser()
        {
    
            SuStatistic s = DaoProvider.SuStatisticDao.FindByIdentity(1);
            s.StatisticNumber++;
            SuStatistic ss = DaoProvider.SuStatisticDao.FindByIdentity(2);
            if (ss.StatisticDate.Date.Day == DateTime.Now.Day)
            {
                ss.StatisticNumber++;
            }
            else
            {
                ss.StatisticDate = DateTime.Now.Date;
                ss.StatisticNumber = 1;
            }
            Update(s);
            Update(ss);
        }

        #endregion
    }
}
