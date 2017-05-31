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
    public class DbSellingRunningService : ServiceBase<DbSellingRunning, long>, IDbSellingRunningService
    {
        public IUserAccount UserAccount { get; set; }
        public override IDao<DbSellingRunning, long> GetBaseDao()
        {
            return ScgDbDaoProvider.DbSellingRunningDao;
        }

        public string RetrieveNextSellingRunningNo(string companyCode, int year)
        {
            DbCompany company = ScgDbQueryProvider.DbCompanyQuery.getDbCompanyByCompanyCode(companyCode);
            DbSellingRunning sellingRunning = ScgDbQueryProvider.DbSellingRunningQuery.GetSellingRunningByCompanyCode_Year(company.CompanyID, year);
            if (sellingRunning == null)
            {
                sellingRunning = new DbSellingRunning();
                sellingRunning.Year = year;

                sellingRunning.CompanyID = company.CompanyID;
                sellingRunning.RunningNo = 1; 
                sellingRunning.CreBy = UserAccount.UserID;
                sellingRunning.CreDate = DateTime.Now;
                sellingRunning.UpdBy = UserAccount.UserID;
                sellingRunning.UpdDate = DateTime.Now;
                ScgDbDaoProvider.DbSellingRunningDao.Save(sellingRunning);
            }
            else
            {
                sellingRunning.RunningNo+=1;
                sellingRunning.UpdBy = UserAccount.UserID;
                sellingRunning.UpdDate = DateTime.Now;
                ScgDbDaoProvider.DbSellingRunningDao.Update(sellingRunning);
            }

            long nextRunningNo = sellingRunning.RunningNo;

            string requestYear = year.ToString().Substring(2, 2);
            //string paddingRunningNo = nextRunningNo.ToString().PadLeft(runningDigit, '0');
            string nextRunningNoStr = nextRunningNo.ToString("00000");

            return companyCode+"-"+requestYear+"-"+nextRunningNoStr;
        }

        //public void AddSellingRuning(long companyID, int year) 
        //{

        //    DbSellingRunning sellingRunning = new DbSellingRunning();

        //    sellingRunning.CompanyID = companyID;
        //    sellingRunning.Year = year;
        //    sellingRunning.CreBy = UserAccount.UserID;
        //    sellingRunning.CreDate = DateTime.Now;
        //    sellingRunning.UpdBy = UserAccount.UserID;
        //    sellingRunning.UpdDate = DateTime.Now;

        //    ScgDbDaoProvider.DbSellingRunningDao.Save(sellingRunning);
        //}

        }
}
