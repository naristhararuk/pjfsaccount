using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;
using SS.Standard.Security;
using SS.Standard.Utilities;
using SS.Standard.WorkFlow.Query;
using SS.Standard.WorkFlow.DTO;

using SCG.DB.DTO;
using SCG.DB.BLL;
using SCG.DB.DAL;
using SCG.DB.Query;


namespace SCG.DB.BLL.Implement
{
    public class DbDocumentRunningService : ServiceBase<DbDocumentRunning , int> , IDbDocumentRunningService
    {
        public IUserAccount UserAccount { get; set; }
        public override IDao<DbDocumentRunning, int> GetBaseDao()
        {
            return ScgDbDaoProvider.DbDocumentRunningDao;
        }

        #region IDbDocumentRunningService Members

        public string RetrieveNextDocumentRunningNo(int year, int documentTypeID, long companyID)
        {
            string documentNo = string.Empty;
            try
            {
                string documentNoTemplate = "{0}-{1}{2}{3}";

                DocumentType documentType = WorkFlowQueryProvider.DocumentTypeQuery.FindByIdentityRunningDocument(documentTypeID);
                int runningDigit = documentType.DocumentNoRunningDigit;
                string documentNoPrefix = documentType.DocumentNoPrefix;

                DbDocumentRunning documentRunning = ScgDbQueryProvider.DbDocumentRunningQuery.GetDocumentRunningByDocumentTypeID_CompanyID_Year(documentTypeID, companyID, year);
                if (documentRunning == null)
                {
                    documentRunning = new DbDocumentRunning();
                    documentRunning.Year = year;
                    documentRunning.DocumentTypeID = documentTypeID;
                    documentRunning.CompanyID = ScgDbQueryProvider.DbCompanyQuery.FindProxyByIdentity(companyID);
                    documentRunning.RunningNo = 0;
                    documentRunning.Active = true;
                    documentRunning.CreBy = UserAccount.UserID;
                    documentRunning.CreDate = DateTime.Now;
                    documentRunning.UpdBy = UserAccount.UserID;
                    documentRunning.UpdDate = DateTime.Now;
                    documentRunning.UpdPgm = UserAccount.CurrentProgramCode;
                }
                documentRunning.RunningNo++;

                long nextRunningNo = documentRunning.RunningNo;

                string companyCode = ScgDbQueryProvider.DbCompanyQuery.FindByIdentity(companyID).CompanyCode;
                string currentYear = year.ToString().Substring(2, 2);
                string paddingRunningNo = nextRunningNo.ToString().PadLeft(runningDigit, '0');

                documentNo = string.Format(documentNoTemplate, new object[] { documentNoPrefix, companyCode, currentYear, paddingRunningNo });

                ScgDbDaoProvider.DbDocumentRunningDao.SaveOrUpdate(documentRunning);
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return documentNo;
        }

        #endregion
    }
}
