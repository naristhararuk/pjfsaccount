using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service.Implement;
using SCG.DB.DTO;
using SS.Standard.Security;
using SS.Standard.Data.NHibernate.Dao;
using SCG.DB.DAL;
using SCG.DB.Query;

namespace SCG.DB.BLL.Implement
{
    public class DbBuyingRunningService : ServiceBase<DbBuyingRunning, long>, IDbBuyingRunningService
    {
        public IUserAccount UserAccount { get; set; }
        public override IDao<DbBuyingRunning, long> GetBaseDao()
        {
            return ScgDbDaoProvider.DbBuyingRunningDao;
        }

        #region IDbBuyingRunningService Members

        public string RetrieveNextBuyingRunningNo(string companyCode, int year)
        {
            DbBuyingRunning buyingRunning = ScgDbQueryProvider.DbBuyingRunningQuery.GetBuyingRunningByCompanyCode_Year(companyCode, year);
            if (buyingRunning == null)
            {
                buyingRunning = new DbBuyingRunning();
                buyingRunning.Year = year;
                DbCompany company = ScgDbQueryProvider.DbCompanyQuery.getDbCompanyByCompanyCode(companyCode);
                buyingRunning.CompanyID = company.CompanyID;
                buyingRunning.RunningNo = 1;
                buyingRunning.CreBy = UserAccount.UserID;
                buyingRunning.CreDate = DateTime.Now;
                buyingRunning.UpdBy = UserAccount.UserID;
                buyingRunning.UpdDate = DateTime.Now;
                ScgDbDaoProvider.DbBuyingRunningDao.Save(buyingRunning);
            }
            else
            {
                buyingRunning.RunningNo++;
                buyingRunning.UpdBy = UserAccount.UserID;
                buyingRunning.UpdDate = DateTime.Now;
                ScgDbDaoProvider.DbBuyingRunningDao.Update(buyingRunning);
            }

            long nextRunningNo = buyingRunning.RunningNo;

            string requestYear = year.ToString().Substring(2, 2);
            //string paddingRunningNo = nextRunningNo.ToString().PadLeft(runningDigit, '0');
            string nextRunningNoStr = nextRunningNo.ToString("00000");

            return companyCode+"-"+requestYear+"-"+nextRunningNoStr;
        }

        #endregion
    }
}
