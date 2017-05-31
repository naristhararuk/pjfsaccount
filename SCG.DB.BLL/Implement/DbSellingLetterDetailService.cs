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
using SCG.DB.DTO.ValueObject;

namespace SCG.DB.BLL.Implement
{
    public partial class DbSellingLetterDetailService : ServiceBase<DbSellingLetterDetail, long>, IDbSellingLetterDetailService
    {
        public IUserAccount UserAccount { get; set; }
        //public IDbSellingRunningService DbSellingRunningService { get; set; }
        //public IDbSellingLetterDetailService DbSellingLetterDetailService { get; set; }
        //public IDbSellingLetterService DbSellingLetterService { get; set; }

        public override IDao<DbSellingLetterDetail, long> GetBaseDao()
        {
            return ScgDbDaoProvider.DbSellingLetterDetailDao;
        }

        #region IDbBuyingLetterDetailService Members

        public long AddLetterDetail(SellingRequestLetterParameter moneyRequest,string letterNo)
        {
            //DbSellingRunningService.RetrieveNextSellingRunningNo(moneyRequest.CompanyCode, moneyRequest.Year);
            DbCompany company = ScgDbQueryProvider.DbCompanyQuery.getDbCompanyBankAccountByCompanyCode(moneyRequest.CompanyCode);

            DbSellingLetterDetail letterDetail = new DbSellingLetterDetail();
            letterDetail.AccountNo = company.AccountNo;
            letterDetail.AccountType = company.AccountType;
            letterDetail.BuyingDate = moneyRequest.RequestDate;
            letterDetail.CompanyName = company.CompanyName;
            letterDetail.LetterNo = letterNo;
            letterDetail.BankName = company.BankName;
            letterDetail.BankBranch = company.BankBranch;
            letterDetail.CreBy = UserAccount.UserID;
            letterDetail.CreDate = DateTime.Now;
            letterDetail.UpdBy = UserAccount.UserID;
            letterDetail.UpdDate = DateTime.Now;

            return ScgDbDaoProvider.DbSellingLetterDetailDao.Save(letterDetail);
        }

        #endregion
    }
}
