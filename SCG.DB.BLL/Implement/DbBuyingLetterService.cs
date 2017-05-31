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
using SS.Standard.Utilities;

namespace SCG.DB.BLL.Implement
{
    public class DbBuyingLetterService : ServiceBase<DbBuyingLetter, long>, IDbBuyingLetterService
    {
        public IUserAccount UserAccount { get; set; }
        public IDbBuyingRunningService DbBuyingRunningService { get; set; }
        public IDbBuyingLetterDetailService DbBuyingLetterDetailService { get; set; }

        public override IDao<DbBuyingLetter, long> GetBaseDao()
        {
            return ScgDbDaoProvider.DbBuyingLetterDao;
        }

        #region DbBuyingLetterService Members

        public void AddLetterAndDocument(long documentID, long letterID)
        {
            DbBuyingLetter buyingLetter = new DbBuyingLetter();

            buyingLetter.LetterID = letterID;
            buyingLetter.DocumentID = documentID;
            buyingLetter.CreBy = UserAccount.UserID;
            buyingLetter.CreDate = DateTime.Now;
            buyingLetter.UpdBy = UserAccount.UserID;
            buyingLetter.UpdDate = DateTime.Now;

            ScgDbDaoProvider.DbBuyingLetterDao.InsertData(buyingLetter);
        }

        #endregion


        public void DeleteLetter(long documentID)
        {
            DbBuyingLetter letter = ScgDbQueryProvider.DbBuyingLetterQuery.FindLetterByDocumentID(documentID);
            if (letter != null)
            {
                ScgDbDaoProvider.DbBuyingLetterDao.DeleteLetter(letter);
            }
        }

        public string CheckDuplicateDocumentID(List<MoneyRequestSearchResult> generateList)
        {
            string documentIDs = string.Empty;
            foreach (MoneyRequestSearchResult r in generateList)
            {
                documentIDs += r.DocumentID + ",";
            }

            string resultMsg = string.Empty;
            IList<string> result = ScgDbDaoProvider.DbBuyingLetterDao.CheckDuplicateDocumentID(documentIDs);
            foreach (string docNo in result)
            {
                resultMsg += docNo + ",";
            }
            return string.IsNullOrEmpty(resultMsg) ? resultMsg : resultMsg.Substring(0, resultMsg.Length - 1);
        }

        public string GenerateBuyingLetter(List<MoneyRequestSearchResult> allList)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            string letterNoList = string.Empty;
            string prevCompanyCode = string.Empty;
            string letterNo = string.Empty;
            long letterID = 0;

            string resultMsg = CheckDuplicateDocumentID(allList);

            if (!string.IsNullOrEmpty(resultMsg))
            {
                errors.AddError("Generate.Error", new Spring.Validation.ErrorMessage("CannotGenerateBuyingLetter", resultMsg));
            }

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            for (int i = 0; i < allList.Count; i++)
            {
                if (allList[i].CompanyCode != prevCompanyCode)
                {
                    prevCompanyCode = allList[i].CompanyCode;
                    letterNo = DbBuyingRunningService.RetrieveNextBuyingRunningNo(allList[i].CompanyCode, allList[i].Year);
                    letterID = DbBuyingLetterDetailService.AddLetterDetail(allList[i], letterNo);

                    letterNoList += letterNo + ",";
                }
                AddLetterAndDocument(allList[i].DocumentID, letterID);
            }
            return string.IsNullOrEmpty(letterNoList) ? letterNoList : letterNoList.Substring(0, letterNoList.Length - 1); ;
        }
    }
}