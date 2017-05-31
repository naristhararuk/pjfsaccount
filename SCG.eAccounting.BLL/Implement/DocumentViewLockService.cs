using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.eAccounting.DTO;
using SS.Standard.Data.NHibernate.Service.Implement;
using SS.Standard.Data.NHibernate.Dao;
using SCG.eAccounting.DAL;
using SS.Standard.Data.NHibernate.QueryCreator;
using SCG.eAccounting.DTO.ValueObject;
using System.Collections;
using SS.Standard.Security;
using SS.Standard.UI;
using SS.SU.Query;
using SCG.eAccounting.Query;
using SS.Standard.Utilities;
using System.Data;
using SS.DB.Query;
using SS.SU.DTO;

namespace SCG.eAccounting.BLL.Implement
{
    public class DocumentViewLockService : ServiceBase<DocumentViewLock, long>, IDocumentViewLockService
    {

        public IDocumentViewLockQuery DocumentViewLockQuery { get; set; }
        public ISCGDocumentQuery SCGDocumentQuery { get; set; }
        public ISuUserQuery SuUserQuery { get; set; }
        public IUserAccount UserAccount { get; set; }

        public override IDao<DocumentViewLock, long> GetBaseDao()
        {
            return ScgeAccountingDaoProvider.DocumentViewLockDao;
        }

        public bool IsDocumentLocked(long documentID, long userID, ref string lockByEmployeeName, ref bool IsOwner,ref DateTime lockDate)
        {

            bool processFlag = false;
            try
            {
                DocumentViewLock document = DocumentViewLockQuery.GetDocumentViewLockByDocumentID(documentID);
                if (ParameterServices.EnableDocumentLock)
                {
                    if(document != null && document.DocumentID>0)
                    {
                        SuUser userLock = SuUserQuery.FindByIdentity(document.UserID.Userid);
                        if (userID == userLock.Userid)
                        {
                            IsOwner = true;
                        }
                        else
                        {
                            IsOwner = false;
                        }
                        lockByEmployeeName = userLock.EmployeeName;
                        lockDate = document.CreDate;
                        processFlag = true;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            return processFlag;
        }

        public bool TryLock(long documentID, long userID, bool LockFlag)
        {
            bool processFlag = false;
            try
            {
                //Lookup DocumentViewLock by documentID 
                DocumentViewLock document = DocumentViewLockQuery.GetDocumentViewLockByDocumentID(documentID);
                if (ParameterServices.EnableDocumentLock)
                {
                    if (document != null && document.Locked)
                    {
                        if (!LockFlag)
                        {
                            SetUnlock(documentID, userID);
                        }
                        else
                        {
                            SuUser userLock = SuUserQuery.FindByIdentity(document.UserID.Userid);
                            throw new Exception(userLock.EmployeeName);
                        }

                    }
                    else
                    {
                        SetLock(documentID, userID, false, LockFlag);
                    }
                }
                processFlag = true;
            }
            catch (Exception)
            {
                
                throw;
            }

            return processFlag;
        }

        public bool ForceLock(long documentID, long userID)
        {
            bool processFlag = false;
            try
            {
                DocumentViewLock document = DocumentViewLockQuery.GetDocumentViewLockByDocumentID(documentID);
                if (ParameterServices.EnableDocumentLock)
                {
                    if (document != null && document.Locked)
                    {
                        SetLock(documentID, userID, true,true);
                    }
                }
                processFlag = true;
            }
            catch (Exception)
            {

                throw;
            }

            return processFlag;
        }

        public bool UnLock(long documentID, long userID)
        {
            bool processFlag = false;
            try
            {
                SetUnlock(documentID, userID);
                processFlag = true;
            }
            catch (Exception)
            {

                throw;
            }

            return processFlag;
        }

        private void SetLock(long documentID, long userID, bool DocumentExist, bool LockFlag)
        {
            try
            {
                DateTime date = DateTime.Now;
                long userid = UserAccount.UserID;
                if (DocumentExist)
                {
                    DocumentViewLock document = DocumentViewLockQuery.GetDocumentViewLockByDocumentID(documentID);
                    document.CreBy = userid;
                    document.CreDate = date;
                    document.UpdBy = userid;
                    document.UpdDate = date;
                    document.UpdPgm = UserAccount.CurrentProgramCode;
                    document.UserID = SuUserQuery.FindByIdentity(userID);
                    document.DocumentID = documentID;
                    document.Locked = LockFlag;
                    this.Update(document);

                }
                else
                {
                    DocumentViewLock document = new DocumentViewLock();
                    document.CreBy = userid;
                    document.CreDate = date;
                    document.UpdBy = userid;
                    document.UpdDate = date;
                    document.UpdPgm = UserAccount.CurrentProgramCode;
                    document.UserID = SuUserQuery.FindByIdentity(userID);
                    document.DocumentID = documentID;
                    document.Locked = true;
                    this.Save(document);
                }
            }
            catch (Exception)
            {
                
                throw;
            }
           
        }

        private void SetUnlock(long documentID, long userID)
        {
            try
            {
                DocumentViewLock document = DocumentViewLockQuery.GetDocumentViewLockByDocumentID(documentID);
                if (document != null && document.UserID.Userid == userID)
                {
                    this.Delete(document);
                }


            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
