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
    public partial class DbBuyingLetterDetailService : ServiceBase<DbBuyingLetterDetail, long>, IDbBuyingLetterDetailService
    {
        public IUserAccount UserAccount { get; set; }
        public override IDao<DbBuyingLetterDetail, long> GetBaseDao()
        {
            return ScgDbDaoProvider.DbBuyingLetterDetailDao;
        }

        #region IDbBuyingLetterDetailService Members

        public long AddLetterDetail(MoneyRequestSearchResult moneyRequest,string letterNo)
        {
            DbCompany company = ScgDbQueryProvider.DbCompanyQuery.getDbCompanyBankAccountByCompanyCode(moneyRequest.CompanyCode);

            DbBuyingLetterDetail letterDetail = new DbBuyingLetterDetail();
            letterDetail.AccountNo = company.AccountNo;
            letterDetail.AccountType = company.AccountType;
            letterDetail.BuyingDate = moneyRequest.RequestDateOfAdvance;
            letterDetail.CompanyName = company.CompanyName;
            letterDetail.LetterNo = letterNo;
            letterDetail.BankName = company.BankName;
            letterDetail.BankBranch = company.BankBranch;
            letterDetail.CreBy = UserAccount.UserID;
            letterDetail.CreDate = DateTime.Now;
            letterDetail.UpdBy = UserAccount.UserID;
            letterDetail.UpdDate = DateTime.Now;

            return ScgDbDaoProvider.DbBuyingLetterDetailDao.Save(letterDetail);
        }

        #endregion
    }
}
