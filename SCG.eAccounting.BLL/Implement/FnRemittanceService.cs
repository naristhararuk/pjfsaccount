using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;

using SCG.eAccounting.DAL;
using SCG.eAccounting.DTO;
using System.Data;
using SCG.eAccounting.DTO.DataSet;
using SCG.DB.DTO;
using SS.SU.DTO;
using SS.Standard.Utilities;
using SCG.eAccounting.Query;
using SS.Standard.Security;
using SS.Standard.WorkFlow.Query;
using SS.Standard.WorkFlow.Service;
using SCG.DB.Query;

namespace SCG.eAccounting.BLL.Implement
{
    public class FnRemittanceService : ServiceBase<FnRemittance, long>, IFnRemittanceService
    {

        #region Override Method
        public override IDao<FnRemittance, long> GetBaseDao()
        {
            return ScgeAccountingDaoProvider.FnRemittanceDao;
        }
        #endregion

        #region Properties
        public IUserAccount UserAccount { get; set; }
        public ISCGDocumentService SCGDocumentService { get; set; }
        public ITransactionService TransactionService { get; set; }
        public IFnRemittanceAdvanceService FnRemittanceAdvanceService { get; set; }
        public IFnRemittanceItemService FnRemittanceItemService { get; set; }
        public IWorkFlowService WorkFlowService { get; set; }
        public IAvAdvanceDocumentService AvAdvanceDocumentService { get; set; }
        #endregion

        #region PrepareDataset
        public DataSet PrepareDS()
        {
            FnRemittanceDataset FnRemittanceDS = new FnRemittanceDataset();
            return FnRemittanceDS;

        }
        public DataSet PrepareDS(long documentID)
        {
            FnRemittanceDataset FnRemittanceDS = this.PrepareDataToDataset(documentID, false);
            FnRemittanceDS.AcceptChanges();

            return FnRemittanceDS;
        }
        public FnRemittanceDataset PrepareDataToDataset(long documentID, bool isCopy)
        {
            FnRemittanceDataset remittanceDS = new FnRemittanceDataset();
            FnRemittance remittance = ScgeAccountingQueryProvider.FnRemittanceQuery.GetFnRemittanceByDocumentID(documentID);

            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            if (remittance == null)
            {
                errors.AddError("Remittance.Error", new Spring.Validation.ErrorMessage("NoRemittanceFound"));
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);


            // Prepare Data to Document datatable.
            if (!isCopy)
            {
                SCGDocumentService.PrepareDataToDataset(remittanceDS, documentID);
            }
            else
            {
                SCGDocumentService.PrepareDataInternalToDataset(remittanceDS, documentID, isCopy);
            }
            // Set data to Remittance Document row in FnremittanceDS.
            FnRemittanceDataset.FnRemittanceRow remittanceRow = remittanceDS.FnRemittance.NewFnRemittanceRow();
            remittanceRow.RemittanceID = remittance.RemittanceID;
            //remittanceRow.SAPIONo = remittance.SAPIONo;
            if (!isCopy)
            {
                if (remittance.Document != null)
                {
                    remittanceRow.DocumentID = remittance.Document.DocumentID;
                }
                //remittanceRow.PBID = remittance.PB.Pbid;
                remittanceRow.TotalAmount = remittance.TotalAmount;
                remittanceRow.isFullClearing = remittance.IsFullClearing;
                remittanceRow.TADocumentID = remittance.TADocumentID;
            }
            else
            {
                if (remittanceDS.Document.Rows.Count > 0)
                {
                    FnRemittanceDataset.DocumentRow docRow = (FnRemittanceDataset.DocumentRow)remittanceDS.Document.Rows[0];
                    remittanceRow.DocumentID = docRow.DocumentID;
                }
            }
            remittanceRow.PBID = remittance.PB.Pbid;
            remittanceRow.MainCurrencyAmount = remittance.MainCurrencyAmount;
            remittanceRow.IsRepOffice = Convert.ToBoolean(remittance.IsRepOffice);
            remittanceRow.Active = remittance.Active;
            remittanceRow.CreBy = remittance.CreBy;
            remittanceRow.CreDate = remittance.CreDate;
            remittanceRow.UpdBy = remittance.UpdBy;
            remittanceRow.UpdDate = remittance.UpdDate;
            remittanceRow.UpdPgm = remittance.UpdPgm;
            // Add Remittance document row to FnremittanceDS.
            remittanceDS.FnRemittance.AddFnRemittanceRow(remittanceRow);

            // Prepare Data to RemittanceItem Datatable.
            FnRemittanceItemService.PrepareDataToDataset(remittanceDS, remittance.RemittanceID);

            // Prepare Data to RemittacneAdvance Datatable.
            FnRemittanceAdvanceService.PrepareDataToDataset(remittanceDS, remittance.RemittanceID);

            return remittanceDS;
        }
        #endregion

        public long AddFnRemittanceDocumentTransaction(Guid txID)
        {
            FnRemittanceDataset fnRemittanceDocumentDS = (FnRemittanceDataset)TransactionService.GetDS(txID);
            FnRemittanceDataset.DocumentRow documentRow = fnRemittanceDocumentDS.Document.NewDocumentRow(); //create new row to dataset.
            fnRemittanceDocumentDS.Document.AddDocumentRow(documentRow); //add new row to dataset.

            FnRemittanceDataset.FnRemittanceRow fnRemittanceDocumentRow = fnRemittanceDocumentDS.FnRemittance.NewFnRemittanceRow();
            fnRemittanceDocumentRow.DocumentID = documentRow.DocumentID;
            fnRemittanceDocumentDS.FnRemittance.AddFnRemittanceRow(fnRemittanceDocumentRow);

            return fnRemittanceDocumentRow.RemittanceID;
        }

        public void UpdateHeaderAndFooterToTransaction(Guid txid, SCGDocument document, FnRemittance remittance)
        {
            SCGDocumentService.UpdateTransactionDocument(txid, document, false, false);
            this.UpdateRemittanceDocumentTransaction(txid, remittance, true);
        }

        public void UpdateRemittanceDocumentTransaction(Guid txtID, FnRemittance fnRemittance, bool IsHeaderAndFooter)
        {
            this.ValidateRemittance(fnRemittance, IsHeaderAndFooter); // function to validateion of remittance.
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            FnRemittanceDataset fnRemittanceDS = (FnRemittanceDataset)TransactionService.GetDS(txtID);

            #region FnRemittanceDocument
            FnRemittanceDataset.FnRemittanceRow fnremittanceRow = fnRemittanceDS.FnRemittance.FindByRemittanceID(fnRemittance.RemittanceID);

            if (!IsHeaderAndFooter && fnremittanceRow.TotalAmount == 0)
            {
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("CannotSaveRemittance_TotalRemittanceAmountMustMoreThanZero"));
            }

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);


            fnremittanceRow.BeginEdit();
            if (fnRemittance.Document != null)
            {
                fnremittanceRow.DocumentID = fnRemittance.Document.DocumentID;
            }
            if (fnRemittance.TADocumentID > 0)
            {
                fnremittanceRow.TADocumentID = fnRemittance.TADocumentID;
            }
            if (fnRemittance.PB != null)
            {
                fnremittanceRow.PBID = fnRemittance.PB.Pbid;
            }
            if (fnRemittance.MainCurrencyID != null)
            {
                fnremittanceRow.MainCurrencyID = fnRemittance.MainCurrencyID;
            }
            //fnremittanceRow.TotalAmount = (double)Math.Round((decimal)fnRemittance.TotalAmount, 2, MidpointRounding.AwayFromZero);
            fnremittanceRow.isFullClearing = fnRemittance.IsFullClearing;
            fnremittanceRow.IsRepOffice = Convert.ToBoolean(fnRemittance.IsRepOffice);
            fnremittanceRow.Active = fnRemittance.Active;
            fnremittanceRow.CreBy = UserAccount.UserID;
            fnremittanceRow.CreDate = DateTime.Now;
            fnremittanceRow.UpdBy = UserAccount.UserID;
            fnremittanceRow.UpdDate = DateTime.Now; ;
            fnremittanceRow.UpdPgm = UserAccount.CurrentProgramCode;

            fnremittanceRow.EndEdit();
            #endregion
        }

        public long SaveRemittanceDocument(Guid txID, long remittanceID)
        {
            FnRemittanceDataset remittanceDS = (FnRemittanceDataset)TransactionService.GetDS(txID);
            FnRemittanceDataset.FnRemittanceRow remittanceRow = remittanceDS.FnRemittance.FindByRemittanceID(remittanceID);
            long documentID = remittanceRow.DocumentID;

            documentID = SCGDocumentService.SaveSCGDocument(txID, documentID);

            remittanceID = ScgeAccountingDaoProvider.FnRemittanceDao.Persist(remittanceDS.FnRemittance);

            ScgeAccountingDaoProvider.FnRemittanceDao.DeleteAllRemittanceItemAndRemittanceAdvance(remittanceRow.RemittanceID);

            FnRemittanceItemService.SaveRemittanceItem(txID, remittanceID);

            FnRemittanceAdvanceService.SaveRemittanceAdvance(txID, remittanceID);

            // Add code for update clearing advance by Anuwat S on 19/04/2009
            
            AvAdvanceDocumentService.UpdateRemittanceAdvance(remittanceID, double.Parse("0" + remittanceDS.FnRemittance.Rows[0]["TotalAmount"].ToString()), false);

            if (!remittanceRow.IsIsRepOfficeNull() && remittanceRow.IsRepOffice)
            {
                AvAdvanceDocumentService.UpdateRemittanceAdvanceForRepOffice(remittanceID, double.Parse("0" + remittanceDS.FnRemittance.Rows[0]["MainCurrencyAmount"].ToString()), false);

            }
            return remittanceID;
        }

        public void ValidateRemittanceAdvance(Guid txID, long remittnceID)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            
            FnRemittanceDataset remittanceDs = (FnRemittanceDataset)TransactionService.GetDS(txID);
            DataRow[] rowAdvList = remittanceDs.FnRemittanceAdvance.Select(string.Format("RemittanceID = '{0}'", remittnceID));
            FnRemittanceDataset.FnRemittanceRow remittanceRow = remittanceDs.FnRemittance.FindByRemittanceID(remittnceID);

            if (remittanceRow != null)
            {
                long companyId = remittanceRow.DocumentRow == null ? 0 : remittanceRow.DocumentRow.CompanyID;
                long requesterId = remittanceRow.DocumentRow == null ? 0 : remittanceRow.DocumentRow.RequesterID;

                foreach (FnRemittanceDataset.FnRemittanceAdvanceRow row in rowAdvList)
                {
                    AvAdvanceDocument adv = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.FindByIdentity(row.AdvanceID);
                    if (adv != null && adv.DocumentID != null && adv.DocumentID.CompanyID != null)
                    {
                        if (companyId != 0 && adv.DocumentID.CompanyID.CompanyID != companyId)
                            errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("CanNotSaveRemittance_CompanyIsNotMatch", adv.DocumentID.DocumentNo));  // Company of ADV-xxxxxxx is not match
                    }
                    if (adv != null && adv.DocumentID != null && adv.DocumentID.RequesterID != null)
                    {
                        if (requesterId != 0 && adv.DocumentID.RequesterID.Userid != requesterId)
                            errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("CanNotAddAdvance_RequesterIsNotMatch"));  // Requester of ADV-xxxxxxx is not match
                    }
                }
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
        }

        public void ValidateRemittance(FnRemittance remittance, bool isHeaderAndFooter)
        {
            #region Validate
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            if (!isHeaderAndFooter)
            {
                //if (remittance.TADocumentID == 0)
                //{
                //    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("TaDocument_Required"));
                //}
            }
            if (isHeaderAndFooter)
            {
                if (remittance.PB == null)
                {
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("CounterCahsier_Required"));
                }
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
            #endregion
        }

        public void UpdateTotalRemittanceAmount(Guid txID, long remittanceId)
        {
            FnRemittanceDataset fnRemittanceDS = (FnRemittanceDataset)TransactionService.GetDS(txID);
            if (fnRemittanceDS != null)
            {
                double totalRemittanceAmount = 0;
                double totalRemittanceMainAmount = 0;
                FnRemittanceDataset.FnRemittanceItemRow[] items = (FnRemittanceDataset.FnRemittanceItemRow[])fnRemittanceDS.FnRemittanceItem.Select();
                FnRemittanceDataset.FnRemittanceRow fnremittanceRow = fnRemittanceDS.FnRemittance.FindByRemittanceID(remittanceId);
                if (items.Length > 0)
                {
                    totalRemittanceAmount = items.Sum(x => x.AmountTHB);
                    if ( (!fnremittanceRow.IsIsRepOfficeNull() && fnremittanceRow.IsRepOffice))
                    {
                        totalRemittanceMainAmount = items.Sum(x => x.MainCurrencyAmount);
                    }
                }

                fnremittanceRow.BeginEdit();
                fnremittanceRow.TotalAmount = (double)Math.Round((decimal)totalRemittanceAmount, 2, MidpointRounding.AwayFromZero);
                fnremittanceRow.MainCurrencyAmount = (double)Math.Round((decimal)totalRemittanceMainAmount, 2, MidpointRounding.AwayFromZero);
                //fnremittanceRow.IsRepOffice = isRepOffice;
                fnremittanceRow.EndEdit();
            }
        }



        #region GetEditableFields and GetVisibleFields
        public IList<object> GetVisibleFields(long? documentID)
        {
            IList<object> visibleFields = new List<object>();

            if (documentID.HasValue) // Check whether new flag then return the default editableFields.
            {
                SS.Standard.WorkFlow.DTO.WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(documentID.Value);
                if (workFlow != null)
                {
                    return WorkFlowService.GetVisibleFields(workFlow.WorkFlowID);
                }
            }

            //Because of Remiitance Document does not has Hold State , 
            //And It don't have spacial group , then return group All
            visibleFields.Add(RemittanceFieldGroup.All);
            visibleFields.Add(RemittanceFieldGroup.Company);
            visibleFields.Add(RemittanceFieldGroup.FullClearing);

            return visibleFields;
        }
        public IList<object> GetEditableFields(long? documentID)
        {
            IList<object> editableFields = new List<object>();

            if (documentID.HasValue) // Check whether new flag then return the default editableFields.
            {
                SS.Standard.WorkFlow.DTO.WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(documentID.Value);
                if (workFlow != null)
                {
                    return WorkFlowService.GetEditableFields(workFlow.WorkFlowID);
                }
            }
            editableFields.Add(RemittanceFieldGroup.All);
            editableFields.Add(RemittanceFieldGroup.Company);
            editableFields.Add(RemittanceFieldGroup.FullClearing);
            return editableFields;
        }
        #endregion
    }

    #region RemittanceFieldGroup Enum
    public enum RemittanceFieldGroup
    {
        All,
        VerifyDetail,
        Company,
        FullClearing
    }
    #endregion
}
