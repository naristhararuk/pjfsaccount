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
    public class DbSellingLetterService : ServiceBase<DbSellingLetter, long>, IDbSellingLetterService
    {
        public IUserAccount UserAccount { get; set; }
        public IDbSellingRunningService DbSellingRunningService { get; set; }
        public IDbSellingLetterDetailService DbSellingLetterDetailService { get; set; }

        public override IDao<DbSellingLetter, long> GetBaseDao()
        {
            return ScgDbDaoProvider.DbSellingLetterDao;
        }

        #region DbBuyingLetterService Members

        public void AddLetterAndDocument(long documentID, long letterID)
        {
            DbSellingLetter SellingLetter = new DbSellingLetter();

            SellingLetter.LetterID = letterID;
            SellingLetter.DocumentID = documentID;
            SellingLetter.CreBy = UserAccount.UserID;
            SellingLetter.CreDate = DateTime.Now;
            SellingLetter.UpdBy = UserAccount.UserID;
            SellingLetter.UpdDate = DateTime.Now;

            ScgDbDaoProvider.DbSellingLetterDao.InsertData(SellingLetter);
        }

        public string CheckDuplicateDocumentID(List<SellingRequestLetterParameter> generateList)
        {
            string documentIDs = string.Empty;
            foreach (SellingRequestLetterParameter r in generateList)
            {
                documentIDs += r.DocumentID + ",";
            }

            string resultMsg = string.Empty;
            IList<string> result = ScgDbDaoProvider.DbSellingLetterDao.CheckDuplicateDocumentID(documentIDs.Substring(0, documentIDs.Length - 1));
            foreach (string docNo in result)
            {
                resultMsg += docNo + ",";
            }
            return string.IsNullOrEmpty(resultMsg) ? resultMsg : resultMsg.Substring(0, resultMsg.Length - 1);
        }

        public string GenerateSellingLetter(List<SellingRequestLetterParameter> allList)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            string letterNoList = string.Empty;
            string prevCompanyCode = string.Empty;
            string letterNo = string.Empty;
            long letterID = 0;

            string resultMsg = CheckDuplicateDocumentID(allList);

            if (!string.IsNullOrEmpty(resultMsg))
            {
                errors.AddError("Generate.Error", new Spring.Validation.ErrorMessage("CannotGenerateSellingLetter", resultMsg));
            }

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            for (int i = 0; i < allList.Count; i++)
            {
                if (allList[i].CompanyCode != prevCompanyCode)
                {
                    prevCompanyCode = allList[i].CompanyCode;
                    letterNo = DbSellingRunningService.RetrieveNextSellingRunningNo(allList[i].CompanyCode, allList[i].Year);
                    letterID = DbSellingLetterDetailService.AddLetterDetail(allList[i], letterNo);

                    letterNoList += letterNo + ",";
                }
                AddLetterAndDocument(allList[i].DocumentID, letterID);
            }
            return string.IsNullOrEmpty(letterNoList) ? letterNoList : letterNoList.Substring(0, letterNoList.Length - 1); ;
        }

        #endregion
    }
}