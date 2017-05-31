using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web;


using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;
using SS.Standard.Security;
using SS.Standard.Utilities;
using SS.Standard.WorkFlow.Service;
using SS.Standard.WorkFlow.Query;

using SCG.DB.DTO;
using SS.DB.Query;

using SCG.eAccounting.DAL;
using SCG.eAccounting.DTO;
using SCG.eAccounting.DTO.DataSet;
using SCG.eAccounting.BLL;
using SCG.eAccounting.DTO.ValueObject;
using SCG.eAccounting.Query;

using Antlr.StringTemplate;
using SCG.DB.Query;
using SCG.DB.DTO.ValueObject;
using SS.SU.Query;
using Spring.Transaction.Interceptor;
using log4net;
using SS.DB.DTO;
using SS.SU.DTO;


namespace SCG.eAccounting.BLL.Implement
{
    public partial class FnExpenseDocumentService : ServiceBase<FnExpenseDocument, long>, IFnExpenseDocumentService
    {
        private static ILog logger = log4net.LogManager.GetLogger(typeof(FnExpenseDocumentService));

        public IUserAccount UserAccount { get; set; }
        public ITransactionService TransactionService { get; set; }
        public ISCGDocumentService SCGDocumentService { get; set; }
        public IWorkFlowService WorkFlowService { get; set; }
        public IFnExpenseInvoiceService FnExpenseInvoiceService { get; set; }
        public IFnExpensePerdiemService FnExpensePerdiemService { get; set; }
        public IFnExpenseMileageService FnExpenseMileageService { get; set; }
        public IFnExpenseAdvanceService FnExpenseAdvanceService { get; set; }
        public IAvAdvanceDocumentService AvAdvanceDocumentService { get; set; }
        public IFnExpenseInvoiceItemService FnExpenseInvoiceItemService { get; set; }
        public IMPADocumentService MPADocumentService { get; set; }
        public IFnExpensMPAService FnExpensMPAService { get; set; }
        public IFnExpensCAService FnExpensCAService { get; set; }
        public IFnExpenseMileageItemService FnExpenseMileageItemService { get; set; }

        public override IDao<FnExpenseDocument, long> GetBaseDao()
        {
            return ScgeAccountingDaoProvider.FnExpenseDocumentDao;
        }

        #region
        public DataSet PrepareExpenseDS()
        {
            ExpenseDataSet ds = new ExpenseDataSet();
            return ds;
        }
        public DataSet PrepareExpenseDS(long documentId)
        {
            ExpenseDataSet expDs = (ExpenseDataSet)this.PrepareInternalDataToDataset(documentId, false);
            expDs.AcceptChanges();

            return expDs;
        }
        public DataSet PrepareInternalDataToDataset(long documentID, bool isCopy)
        {
            ExpenseDataSet expDS = new ExpenseDataSet();
            FnExpenseDocument expDocument = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseDocumentByDocumentID(documentID);

            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            if (expDocument == null)
            {
                errors.AddError("ValidationError", new Spring.Validation.ErrorMessage("NoDocumentFound"));
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            // Prepare Data to Document datatable.
            if (!isCopy)
            {
                SCGDocumentService.PrepareDataToDataset(expDS, documentID);
            }
            else
            {
                SCGDocumentService.PrepareDataInternalToDataset(expDS, documentID, isCopy);
            }
            // Set data to Expense Document row in ExpenseDocumentDS.
            ExpenseDataSet.FnExpenseDocumentRow expDocumentRow = expDS.FnExpenseDocument.NewFnExpenseDocumentRow();
            expDocumentRow.ExpenseID = expDocument.ExpenseID;
            if (!isCopy)
            {
                expDocumentRow.DocumentID = expDocument.Document.DocumentID;
                expDocumentRow.TotalAdvance = (decimal)expDocument.TotalAdvance;
                expDocumentRow.TotalExpense = (decimal)expDocument.TotalExpense;

                if (expDocument.TADocument != null)
                {
                    expDocumentRow.TADocumentID = expDocument.TADocument.TADocumentID;
                }
                if (expDocument.IsBusinessPurpose.HasValue)
                {
                    expDocumentRow.IsBusinessPurpose = expDocument.IsBusinessPurpose.Value;
                }
                else
                {
                    expDocumentRow.IsBusinessPurpose = false;
                }
                if (expDocument.IsTrainningPurpose.HasValue)
                {
                    expDocumentRow.IsTrainningPurpose = expDocument.IsTrainningPurpose.Value;
                }
                else
                {
                    expDocumentRow.IsTrainningPurpose = false;
                }
                if (expDocument.IsOtherPurpose.HasValue)
                {
                    expDocumentRow.IsOtherPurpose = expDocument.IsOtherPurpose.Value;
                }
                else
                {
                    expDocumentRow.IsOtherPurpose = false;
                }
                expDocumentRow.OtherPurposeDescription = expDocument.OtherPurposeDescription;
                if (expDocument.FromDate.HasValue)
                {
                    expDocumentRow.FromDate = expDocument.FromDate.Value;
                }
                if (expDocument.ToDate.HasValue)
                {
                    expDocumentRow.ToDate = expDocument.ToDate.Value;
                }
                expDocumentRow.Country = expDocument.Country;
                expDocumentRow.PersonalLevel = expDocument.PersonalLevel;
                if (expDocument.ExchangeRateForUSD.HasValue)
                {
                    expDocumentRow.ExchangeRateForUSD = (decimal)expDocument.ExchangeRateForUSD.Value;
                }
                if (expDocument.ExchangeRateForUSDAdvance.HasValue)
                {
                    expDocumentRow.ExchangeRateForUSDAdvance = (decimal)expDocument.ExchangeRateForUSDAdvance.Value;
                }
                expDocumentRow.TotalRemittance = (decimal)expDocument.TotalRemittance;
                expDocumentRow.DifferenceAmount = (decimal)expDocument.DifferenceAmount;

                expDocumentRow.BoxID = expDocument.BoxID;
                expDocumentRow.RemittancePostingStatus = expDocument.RemittancePostingStatus;

                if (expDocument.MainCurrencyID.HasValue)
                {
                    expDocumentRow.MainCurrencyID = expDocument.MainCurrencyID.Value;
                }

                if (expDocument.LocalCurrencyID.HasValue)
                {
                    expDocumentRow.LocalCurrencyID = expDocument.LocalCurrencyID.Value;
                }

                if (expDocument.TotalExpenseLocalCurrency.HasValue)
                {
                    expDocumentRow.TotalExpenseLocalCurrency = (decimal)expDocument.TotalExpenseLocalCurrency.Value;
                }

                if (expDocument.TotalAdvanceLocalCurrency.HasValue)
                {
                    expDocumentRow.TotalAdvanceLocalCurrency = (decimal)expDocument.TotalAdvanceLocalCurrency.Value;
                }

                if (expDocument.TotalRemittanceLocalCurrency.HasValue)
                {
                    expDocumentRow.TotalRemittanceLocalCurrency = (decimal)expDocument.TotalRemittanceLocalCurrency.Value;
                }

                if (expDocument.DifferenceAmountLocalCurrency.HasValue)
                {
                    expDocumentRow.DifferenceAmountLocalCurrency = (decimal)expDocument.DifferenceAmountLocalCurrency.Value;
                }

                if (expDocument.TotalExpenseMainCurrency.HasValue)
                {
                    expDocumentRow.TotalExpenseMainCurrency = (decimal)expDocument.TotalExpenseMainCurrency.Value;
                }

                if (expDocument.TotalAdvanceMainCurrency.HasValue)
                {
                    expDocumentRow.TotalAdvanceMainCurrency = (decimal)expDocument.TotalAdvanceMainCurrency.Value;
                }

                if (expDocument.TotalRemittanceMainCurrency.HasValue)
                {
                    expDocumentRow.TotalRemittanceMainCurrency = (decimal)expDocument.TotalRemittanceMainCurrency.Value;
                }

                if (expDocument.DifferenceAmountMainCurrency.HasValue)
                {
                    expDocumentRow.DifferenceAmountMainCurrency = (decimal)expDocument.DifferenceAmountMainCurrency.Value;
                }

                if (expDocument.ExchangeRateForLocalCurrency.HasValue)
                {
                    expDocumentRow.ExchangeRateForLocalCurrency = (decimal)expDocument.ExchangeRateForLocalCurrency.Value;
                }

                if (expDocument.ExchangeRateMainToTHBCurrency.HasValue)
                {
                    expDocumentRow.ExchangeRateMainToTHBCurrency = (decimal)expDocument.ExchangeRateMainToTHBCurrency.Value;
                }

                if (expDocument.IsRepOffice.HasValue)
                {
                    expDocumentRow.IsRepOffice = expDocument.IsRepOffice.Value;
                }
            }
            else
            {
                if (expDS.Document.Rows.Count > 0)
                {
                    ExpenseDataSet.DocumentRow docRow = (ExpenseDataSet.DocumentRow)expDS.Document.Rows[0];
                    expDocumentRow.DocumentID = docRow.DocumentID;
                }
                if (expDocument.IsRepOffice.HasValue)
                {
                    expDocumentRow.IsRepOffice = expDocument.IsRepOffice.Value;
                }
                if (expDocument.MainCurrencyID.HasValue)
                {
                    expDocumentRow.MainCurrencyID = expDocument.MainCurrencyID.Value;
                }
                if (expDocument.LocalCurrencyID.HasValue)
                {
                    expDocumentRow.LocalCurrencyID = expDocument.LocalCurrencyID.Value;
                }
                if (expDocument.ExchangeRateForLocalCurrency.HasValue)
                {
                    expDocumentRow.ExchangeRateForLocalCurrency = (decimal)expDocument.ExchangeRateForLocalCurrency.Value;
                }
                if (expDocument.ExchangeRateMainToTHBCurrency.HasValue)
                {
                    expDocumentRow.ExchangeRateMainToTHBCurrency = (decimal)expDocument.ExchangeRateMainToTHBCurrency.Value;
                }

            }

            if (expDocument.ServiceTeam != null)
            {
                expDocumentRow.ServiceTeamID = expDocument.ServiceTeam.ServiceTeamID;
            }
            if (expDocument.PB != null)
            {
                expDocumentRow.PBID = expDocument.PB.Pbid;
            }

            expDocumentRow.ExpenseType = expDocument.ExpenseType;
            expDocumentRow.PaymentType = expDocument.PaymentType;

            if (expDocument.AmountApproved.HasValue)
                expDocumentRow.AmountApproved = (decimal)expDocument.AmountApproved;
            else
                expDocumentRow.SetAmountApprovedNull();

            expDocumentRow.Active = expDocument.Active;
            expDocumentRow.CreBy = expDocument.CreBy;
            expDocumentRow.CreDate = expDocument.CreDate;
            expDocumentRow.UpdBy = expDocument.UpdBy;
            expDocumentRow.UpdDate = expDocument.UpdDate;
            expDocumentRow.UpdPgm = expDocument.UpdPgm;

            // Add expense document row to ExpenseDataSet.
            expDS.FnExpenseDocument.AddFnExpenseDocumentRow(expDocumentRow);
            if (!isCopy)
            {
                //// Prepare Data to FnExpenseAdvance Datatable.
                FnExpenseAdvanceService.PrepareDataToDataset(expDS, expDocument.ExpenseID);
            }

            FnExpensMPAService.PrepareDataToDataset(expDS, expDocument.ExpenseID);

            FnExpensCAService.PrepareDataToDataset(expDS, expDocument.ExpenseID);

            // Prepare Data to FnExpenseInvoice, FnExpenseInvoiceItem Datatable.
            FnExpenseInvoiceService.PrepareDataToDataset(expDS, expDocument.ExpenseID);

            // Prepare Data to FnExpensePerdiem, FnExpensePerdiemItem Datatable.
            FnExpensePerdiemService.PrepareDataToDataset(expDS, expDocument.ExpenseID);

            // Prepare Data to FnExpenseMileage, FnExpenseMileageItem, FnExpenseMileageInvoice Datatable.
            FnExpenseMileageService.PrepareDataToDataset(expDS, expDocument.ExpenseID);

            return expDS;
        }
        public long AddExpenseDocumentTransaction(FnExpenseDocument exp, Guid txId)
        {
            ExpenseDataSet ds = (ExpenseDataSet)TransactionService.GetDS(txId);
            ExpenseDataSet.DocumentRow docRow = ds.Document.NewDocumentRow();
            ds.Document.AddDocumentRow(docRow);

            ExpenseDataSet.FnExpenseDocumentRow expRow = ds.FnExpenseDocument.NewFnExpenseDocumentRow();
            expRow.ExpenseType = exp.ExpenseType;
            expRow.DocumentID = docRow.DocumentID;
            expRow.Active = true;
            expRow.CreDate = DateTime.Now;
            expRow.CreBy = UserAccount.UserID;
            expRow.UpdDate = DateTime.Now;
            expRow.UpdBy = UserAccount.UserID;
            expRow.UpdPgm = UserAccount.CurrentProgramCode;
            ds.FnExpenseDocument.AddFnExpenseDocumentRow(expRow);

            return expRow.ExpenseID;
        }
        public void UpdateExpenseDocumentTransaction(FnExpenseDocument exp, Guid txId)
        {
            this.ValidateExpense(exp);
            ExpenseDataSet ds = (ExpenseDataSet)TransactionService.GetDS(txId);
            ExpenseDataSet.FnExpenseDocumentRow expRow = ds.FnExpenseDocument.FindByExpenseID(exp.ExpenseID);

            expRow.BeginEdit();
            if (exp.TADocument != null)
            {
                expRow.TADocumentID = exp.TADocument.TADocumentID;
            }
            if (exp.ServiceTeam != null)
            {
                expRow.ServiceTeamID = exp.ServiceTeam.ServiceTeamID;
            }
            if (exp.PaymentType != PaymentType.TR && exp.PB != null)
            {
                expRow.PBID = exp.PB.Pbid;
            }
            else
            {
                expRow.SetPBIDNull();
            }

            expRow.PaymentType = exp.PaymentType;

            if (exp.MainCurrencyID.HasValue)
            {
                expRow.MainCurrencyID = exp.MainCurrencyID.Value;
            }

            if (exp.LocalCurrencyID.HasValue)
            {
                expRow.LocalCurrencyID = exp.LocalCurrencyID.Value;
            }

            if (exp.TotalExpenseLocalCurrency.HasValue)
            {
                expRow.TotalExpenseLocalCurrency = (decimal)exp.TotalExpenseLocalCurrency.Value;
            }

            if (exp.TotalAdvanceLocalCurrency.HasValue)
            {
                expRow.TotalAdvanceLocalCurrency = (decimal)exp.TotalAdvanceLocalCurrency.Value;
            }

            if (exp.TotalRemittanceLocalCurrency.HasValue)
            {
                expRow.TotalRemittanceLocalCurrency = (decimal)exp.TotalRemittanceLocalCurrency.Value;
            }

            if (exp.DifferenceAmountLocalCurrency.HasValue)
            {
                expRow.DifferenceAmountLocalCurrency = (decimal)exp.DifferenceAmountLocalCurrency.Value;
            }

            if (exp.TotalExpenseMainCurrency.HasValue)
            {
                expRow.TotalExpenseMainCurrency = (decimal)exp.TotalExpenseMainCurrency.Value;
            }

            if (exp.TotalAdvanceMainCurrency.HasValue)
            {
                expRow.TotalAdvanceMainCurrency = (decimal)exp.TotalAdvanceMainCurrency.Value;
            }

            if (exp.TotalRemittanceMainCurrency.HasValue)
            {
                expRow.TotalRemittanceMainCurrency = (decimal)exp.TotalRemittanceMainCurrency.Value;
            }

            if (exp.DifferenceAmountMainCurrency.HasValue)
            {
                expRow.DifferenceAmountMainCurrency = (decimal)exp.DifferenceAmountMainCurrency.Value;
            }

            if (exp.ExchangeRateForLocalCurrency.HasValue)
            {
                expRow.ExchangeRateForLocalCurrency = (decimal)exp.ExchangeRateForLocalCurrency.Value;
            }

            if (exp.ExchangeRateMainToTHBCurrency.HasValue)
            {
                expRow.ExchangeRateMainToTHBCurrency = (decimal)exp.ExchangeRateMainToTHBCurrency.Value;
            }

            DataRow[] advanceList = ds.FnExpenseAdvance.Select();

            if (advanceList.Length > 0)
            {
                if (exp.IsBusinessPurpose.HasValue)
                {
                    expRow.IsBusinessPurpose = exp.IsBusinessPurpose.Value;
                }
                else
                {
                    expRow.IsBusinessPurpose = false;
                }
                if (exp.IsTrainningPurpose.HasValue)
                {
                    expRow.IsTrainningPurpose = exp.IsTrainningPurpose.Value;
                }
                else
                {
                    expRow.IsTrainningPurpose = false;
                }
                if (exp.IsOtherPurpose.HasValue)
                {
                    expRow.IsOtherPurpose = exp.IsOtherPurpose.Value;
                }
                else
                {
                    expRow.IsOtherPurpose = false;
                }
                expRow.OtherPurposeDescription = exp.OtherPurposeDescription;
                if (exp.FromDate.HasValue)
                {
                    expRow.FromDate = exp.FromDate.Value;
                }
                if (exp.ToDate.HasValue)
                {
                    expRow.ToDate = exp.ToDate.Value;
                }
                expRow.Country = exp.Country;
            }
            expRow.PersonalLevel = exp.PersonalLevel;
            if (exp.ExchangeRateForUSD.HasValue)
            {
                expRow.ExchangeRateForUSD = (decimal)exp.ExchangeRateForUSD.Value;
            }
            if (exp.ExchangeRateForUSDAdvance.HasValue)
            {
                expRow.ExchangeRateForUSDAdvance = (decimal)exp.ExchangeRateForUSDAdvance.Value;
            }

            expRow.ExpenseType = exp.ExpenseType;
            expRow.Active = true;
            //expRow.CreDate = DateTime.Now;
            //expRow.CreBy = UserAccount.UserID;
            expRow.UpdDate = DateTime.Now;
            expRow.UpdBy = UserAccount.UserID;
            expRow.UpdPgm = UserAccount.CurrentProgramCode;
            /*เพิ่ม Save FixedAdvanceID*/
            if (exp.FixedAdvanceDocument != null)
            {
                expRow.FixedAdvanceDocumentID = exp.FixedAdvanceDocument.FixedAdvanceID;
            }

            expRow.EndEdit();
        }
        #endregion

        #region ****** DataBase
        //[Transaction]
        public long SaveExpenseDocument(Guid txId, long expDocumentId)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            FnExpenseInvoiceItemService.ValidateInvoiceItem(txId, expDocumentId);
            FnExpenseMileageService.ValidRemaining(txId, expDocumentId);
            this.ValidateClearingAdvance(txId, expDocumentId);
            bool isRepOffice = false;
            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(txId);
            if (expDS == null)
            {
                logger.Error("SaveExpenseDocument(Guid txId, long expDocumentId) : cannot get expense dataset : ExpenseDataSet is null");
            }
            ExpenseDataSet.FnExpenseDocumentRow row = expDS.FnExpenseDocument.FindByExpenseID(expDocumentId);

            long tempDocumentId = row.DocumentID;

            if (!row.IsIsRepOfficeNull())
            {
                isRepOffice = row.IsRepOffice;
            }

            if (!isRepOffice)
            {
                if (row.TotalExpense == (decimal)0 && row.TotalAdvance == (decimal)0)
                {
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("CannotSave_TotalExpenseIsZero"));
                }
                if (row.DifferenceAmount < (decimal)0 && !row.PaymentType.Equals(PaymentType.CA))
                {
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("อนุญาตให้เลือกเฉพาะเงินสดเท่านั้น"));
                }
            }
            else
            {
                if (row.TotalExpenseMainCurrency == (decimal)0 && row.TotalAdvanceMainCurrency == (decimal)0)
                {
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("CannotSave_TotalExpenseIsZero"));
                }
                if (row.DifferenceAmountMainCurrency < (decimal)0 && !row.PaymentType.Equals(PaymentType.CA))
                {
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("อนุญาตให้เลือกเฉพาะเงินสดเท่านั้น"));
                }
            }
                SS.Standard.WorkFlow.DTO.WorkFlow workflow = SS.Standard.WorkFlow.Query.WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(row.DocumentID);

            if (ParameterServices.EnableLockExpenseChangeAmount)
            {
                if (workflow != null && workflow.CurrentState.Name == WorkFlowStateFlag.WaitVerify)
                {
                    FnExpenseDocument expDoc = new FnExpenseDocument();
                    expDoc.LoadFromDataRow(row);
                    if ((!row.IsAmountApprovedNull() && row.AmountApproved != row.TotalExpense) && !CanChangeAmountExpenseDocument(expDoc))
                    {
                        errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("WarningDonotChangeAmount"));
                    }
                }
            }
            if (workflow != null && workflow.CurrentState.Name == WorkFlowStateFlag.Hold)
            {
                if ((!row.IsAmountApprovedNull() && row.AmountApproved < row.TotalExpense))
                {
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("WarningDonotChangeAmount"));
                }
            }

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            // Save SCG Document to Database.
            long scgDocumentID = SCGDocumentService.SaveSCGDocument(txId, tempDocumentId);

            row.BeginEdit();
            row.DocumentID = scgDocumentID;
            row.EndEdit();

            long expId = 0;

            expId = ScgeAccountingDaoProvider.FnExpenseDocumentDao.Persist(expDS.FnExpenseDocument);

            ScgeAccountingDaoProvider.FnExpenseAdvanceDao.Persist(expDS.FnExpenseAdvance);
            ScgeAccountingDaoProvider.ExpensesMPADao.Persist(expDS.FnExpenseMPA);
            ScgeAccountingDaoProvider.ExpensesCADao.Persist(expDS.FnExpenseCA);
            FnExpenseInvoiceService.SaveExpenseInvoice(txId, expId);
            FnExpensePerdiemService.SaveExpensePerdiem(txId);
            FnExpenseMileageService.SaveExpenseMileage(txId, expId);

            // Add code for update clearing advance by Anuwat S on 19/04/2009
            AvAdvanceDocumentService.UpdateClearingAdvance(expId, double.Parse(string.IsNullOrEmpty(expDS.FnExpenseDocument.Rows[0]["TotalExpense"].ToString()) ? "0" : expDS.FnExpenseDocument.Rows[0]["TotalExpense"].ToString()));
            if (isRepOffice)
            {
                AvAdvanceDocumentService.UpdateClearingAdvanceForRepOffice(expId, double.Parse(string.IsNullOrEmpty(expDS.FnExpenseDocument.Rows[0]["TotalExpenseMainCurrency"].ToString()) ? "0" : expDS.FnExpenseDocument.Rows[0]["TotalExpenseMainCurrency"].ToString()));
            }
            return expId;
        }
        #endregion

        #region ****** Validation ExpenseDocument
        public void ValidateExpense(FnExpenseDocument exp)
        {
            #region Validate
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            //if (exp.FixedAdvanceDocument != null)
            //{
            //    SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(exp.Document.DocumentID);
            //    SS.Standard.WorkFlow.DTO.WorkFlow workflow = SS.Standard.WorkFlow.Query.WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(exp.Document.DocumentID);
            //    FixedAdvanceDocument b = ScgeAccountingQueryProvider.FixedAdvanceDocumentQuery.GetFixedAdvanceByFixedAdvanceID(exp.FixedAdvanceDocument.FixedAdvanceID);
            //    if (scgDocument != null && workflow != null && workflow.CurrentState.Name == WorkFlowStateFlag.Draft)
            //    {
            //        if (!String.IsNullOrEmpty(scgDocument.DocumentNo))
            //        {
            //            TimeSpan difference = exp.CreDate - b.EffectiveToDate;
            //            var days = difference.TotalDays;
            //            if (days > 30)
            //            {
            //                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("FixedAdvanceOver30day"));
            //            }
            //        }
            //        else
            //        {
            //            TimeSpan difference2 = DateTime.Now - b.EffectiveToDate;
            //            var days = difference2.TotalDays;
            //            if (days > 30)
            //            {
            //                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("FixedAdvanceOver30day"));
            //            }
            //        }
            //    }
            //    else if (workflow == null)
            //    {
            //        TimeSpan difference2 = DateTime.Now - b.EffectiveToDate;
            //        var days = difference2.TotalDays;
            //        if (days > 30)
            //        {
            //            errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("FixedAdvanceOver30day"));
            //        }
            //    }
            //}

            if (exp.ServiceTeam == null)
            {
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("ServiceTeamIsRequired"));
            }
            if (string.IsNullOrEmpty(exp.PaymentType))
            {
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("PaymentTypeIsRequired"));
            }
            else
            {
                if (!exp.PaymentType.Equals(PaymentType.TR))
                {
                    if (exp.PB == null)
                    {
                        errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("CounterCashierIsRequired"));
                    }
                }
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
            #endregion
        }
        public void ValidateClearingAdvance(Guid txID, long expId)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            ExpenseDataSet expDs = (ExpenseDataSet)TransactionService.GetDS(txID);
            ExpenseDataSet.FnExpenseAdvanceRow[] advList = (ExpenseDataSet.FnExpenseAdvanceRow[])expDs.FnExpenseAdvance.Select(string.Format("ExpenseID = '{0}'", expId));
            ExpenseDataSet.FnExpenseDocumentRow expRow = expDs.FnExpenseDocument.FindByExpenseID(expId);
            if (expRow != null)
            {
                long requesterId = expRow.DocumentRow == null ? 0 : expRow.DocumentRow.RequesterID;
                foreach (ExpenseDataSet.FnExpenseAdvanceRow row in advList)
                {
                    AvAdvanceDocument adv = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.FindByIdentity(row.AdvanceID);
                    if (adv != null && adv.DocumentID != null && adv.DocumentID.RequesterID != null)
                    {
                        if (requesterId != 0 && adv.DocumentID.RequesterID.Userid != requesterId)
                            errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("CanNotClearingAdvance_RequesterIsNotMatch"));  // Requester of ADV-xxxxxxx is not match
                    }
                }
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
        }
        #endregion

        #region ****** GetEditableFields and GetVisibleFields
        public IList<object> GetVisibleFields(long? documentID)
        {
            IList<object> visibleFields = new List<object>();

            if (!documentID.HasValue) // Check whether new flag then return the default editableFields.
            {
                visibleFields.Add(ExpenseFieldGroup.CostCenter);
                visibleFields.Add(ExpenseFieldGroup.AccountCode);
                visibleFields.Add(ExpenseFieldGroup.InternalOrder);
                visibleFields.Add(ExpenseFieldGroup.Subject);
                visibleFields.Add(ExpenseFieldGroup.ReferenceNo);
                visibleFields.Add(ExpenseFieldGroup.VAT);
                visibleFields.Add(ExpenseFieldGroup.WHTax);
                visibleFields.Add(ExpenseFieldGroup.Other);
                visibleFields.Add(ExpenseFieldGroup.Initiator);
                visibleFields.Add(ExpenseFieldGroup.Company);
                visibleFields.Add(ExpenseFieldGroup.BuActor);
                visibleFields.Add(ExpenseFieldGroup.CounterCashier);
                visibleFields.Add(ExpenseFieldGroup.PerdiemRate);
                visibleFields.Add(ExpenseFieldGroup.LocalCurrency);
            }
            else // Check whether view or edit flag then return editableFields from workflow state.
            {
                SS.Standard.WorkFlow.DTO.WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(documentID.Value);
                visibleFields = WorkFlowService.GetVisibleFields(workFlow.WorkFlowID);
            }

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
            editableFields.Add(ExpenseFieldGroup.CostCenter);
            editableFields.Add(ExpenseFieldGroup.AccountCode);
            editableFields.Add(ExpenseFieldGroup.InternalOrder);
            editableFields.Add(ExpenseFieldGroup.Subject);
            editableFields.Add(ExpenseFieldGroup.ReferenceNo);
            editableFields.Add(ExpenseFieldGroup.VAT);
            editableFields.Add(ExpenseFieldGroup.WHTax);
            editableFields.Add(ExpenseFieldGroup.Other);
            editableFields.Add(ExpenseFieldGroup.Initiator);
            editableFields.Add(ExpenseFieldGroup.Company);
            editableFields.Add(ExpenseFieldGroup.BuActor);
            editableFields.Add(ExpenseFieldGroup.CounterCashier);
            editableFields.Add(ExpenseFieldGroup.PerdiemRate);
            editableFields.Add(ExpenseFieldGroup.LocalCurrency);

            return editableFields;
        }
        #endregion

        #region ****** Use in Clearing Advance

        public void SetTA(Guid txID, long expID, Advance avCriteria)
        {
            ExpenseDataSet expenseDS = (ExpenseDataSet)TransactionService.GetDS(txID);
            ExpenseDataSet.FnExpenseAdvanceDataTable expenseAdvanceDT = expenseDS.FnExpenseAdvance;

            //Delete all advance in ExpenseAdvance Datatable.
            expenseAdvanceDT.Clear();

            // Query only advance that
            // 1. Status = OutStanding
            // 2. Do not used in Expense that has flag <> 'Cancel'
            IList<Advance> advanceList = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.GetAdvanceListRelateWithRemittanceButNotInExpense(avCriteria, 0, 100, string.Empty);
            double totalAdvance = this.AddExpenseAdvanceToTransaction(txID, expID, advanceList, avCriteria.TADocumentID.Value);
            this.SetTotalAdvance(txID, avCriteria.TADocumentID.Value, totalAdvance);
            this.RefreshRemittance(txID, expID);
        }

        #region public void RefreshRemittance(Guid txID, long expID)
        public void RefreshRemittance(Guid txID, long expID)
        {
            double? totalRemittanceAmount = 0;
            double? totalRemittanceMainAmount = 0;
            //double? totalRemittanceLocalAmount = 0;
            ExpenseDataSet expenseDS = (ExpenseDataSet)TransactionService.GetDS(txID);
            //ExpenseDataSet.FnExpenseRemittanceDataTable expenseRemittanceDT = expenseDS.FnExpenseRemittance;
            ExpenseDataSet.FnExpenseDocumentRow expenseDocumentRow = (ExpenseDataSet.FnExpenseDocumentRow)expenseDS.FnExpenseDocument.FindByExpenseID(expID);
            bool isRepOffice = expenseDocumentRow.IsIsRepOfficeNull() ? false : expenseDocumentRow.IsRepOffice;
            // Clear all remittance in ExpenseRemittance
            //expenseRemittanceDT.Clear();

            List<long> advanceIdList = new List<long>();
            ExpenseDataSet.FnExpenseAdvanceRow[] rows = (ExpenseDataSet.FnExpenseAdvanceRow[])expenseDS.FnExpenseAdvance.Select();
            foreach (ExpenseDataSet.FnExpenseAdvanceRow row in rows)
            {
                advanceIdList.Add(row.AdvanceID);
            }

            // Find Advance, Remittance and content for add to ExpenseRemittance
            if (advanceIdList.Count != 0)
            {
                IList<Advance> advanceList = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.FindRemittanceAmountByAdvanceIDs(advanceIdList);
                foreach (Advance adv in advanceList)
                {
                    totalRemittanceAmount += adv.RemittedAmountTHB;
                    if (adv.RemittanceAmountMainCurrency.HasValue)
                    {
                        totalRemittanceMainAmount += adv.RemittanceAmountMainCurrency;
                    }
                }
            }

            //Set total remittance amount.
            if (expenseDocumentRow != null)
            {
                expenseDocumentRow.BeginEdit();
                expenseDocumentRow.TotalRemittance = Convert.ToDecimal(totalRemittanceAmount);

                if (isRepOffice)
                {
                    if (totalRemittanceMainAmount.HasValue)
                    {
                        expenseDocumentRow.TotalRemittanceMainCurrency = Convert.ToDecimal(totalRemittanceMainAmount);
                        if (!expenseDocumentRow.IsMainCurrencyIDNull() && !expenseDocumentRow.IsLocalCurrencyIDNull() && (expenseDocumentRow.MainCurrencyID == expenseDocumentRow.LocalCurrencyID))
                        {
                            expenseDocumentRow.TotalRemittanceLocalCurrency = Convert.ToDecimal(totalRemittanceMainAmount);
                        }
                        else
                        {
                            expenseDocumentRow.TotalRemittanceLocalCurrency = Math.Round((expenseDocumentRow.TotalRemittanceMainCurrency) * (expenseDocumentRow.IsExchangeRateForLocalCurrencyNull() ? 0 : expenseDocumentRow.ExchangeRateForLocalCurrency), 2, MidpointRounding.AwayFromZero);
                        }
                    }
                }
                expenseDocumentRow.EndEdit();
            }
        }
        #endregion public void RefreshRemittance(Guid txID, long expID)

        #region public void RefreshAdvance(Guid txID, long expID)
        public void RefreshAdvance(Guid txID, long expID)
        {
            double? totalAdvanceAmount = 0;
            double? totalAdvanceAmountMainCurrency = 0;
            double? totalAdvanceAmountLocalCurrency = 0;

            ExpenseDataSet expenseDS = (ExpenseDataSet)TransactionService.GetDS(txID);
            ExpenseDataSet.FnExpenseDocumentRow expenseDocumentRow = (ExpenseDataSet.FnExpenseDocumentRow)expenseDS.FnExpenseDocument.FindByExpenseID(expID);
            bool isRepOffice = expenseDocumentRow.IsIsRepOfficeNull() ? false : expenseDocumentRow.IsRepOffice;

            string expenseType = this.GetExpenseType(expID, txID);

            // Clear all remittance in ExpenseRemittance
            List<long> advanceIdList = new List<long>();
            ExpenseDataSet.FnExpenseAdvanceRow[] rows = (ExpenseDataSet.FnExpenseAdvanceRow[])expenseDS.FnExpenseAdvance.Select();
            foreach (ExpenseDataSet.FnExpenseAdvanceRow row in rows)
            {
                AvAdvanceDocument avAdvance = AvAdvanceDocumentService.FindByIdentity(row.AdvanceID);
                if (avAdvance != null)
                {
                    totalAdvanceAmount += avAdvance.Amount;

                    if (Convert.ToBoolean(avAdvance.IsRepOffice))
                    {
                        if (expenseType.Equals(ZoneType.Domestic))
                        {
                            IList<AvAdvanceItem> items = ScgeAccountingQueryProvider.AvAdvanceItemQuery.FindByAvAdvanceItemAdvanceID(avAdvance.AdvanceID);

                            if (items.Count > 0)
                            {
                                if (items[0].CurrencyID.CurrencyID == avAdvance.MainCurrencyID && items[0].CurrencyID.CurrencyID != expenseDocumentRow.LocalCurrencyID)
                                    totalAdvanceAmountLocalCurrency += (Convert.ToDouble(avAdvance.LocalCurrencyAmount) * Convert.ToDouble(avAdvance.ExchangeRateForLocalCurrency));
                                else
                                    totalAdvanceAmountLocalCurrency += Convert.ToDouble(avAdvance.LocalCurrencyAmount);  // final currency
                            }

                            totalAdvanceAmountMainCurrency += Convert.ToDouble(avAdvance.MainCurrencyAmount);
                        }
                        else
                        {
                            totalAdvanceAmountMainCurrency += Convert.ToDouble(avAdvance.MainCurrencyAmount);
                        }
                    }
                }
            }

            //Set total advance amount.
            expenseDocumentRow.BeginEdit();
            expenseDocumentRow.TotalAdvance = Convert.ToDecimal(totalAdvanceAmount);
            if (isRepOffice)
            {
                if (expenseType.Equals(ZoneType.Domestic))
                {
                    expenseDocumentRow.TotalAdvanceLocalCurrency = Convert.ToDecimal(totalAdvanceAmountLocalCurrency);
                    expenseDocumentRow.TotalAdvanceMainCurrency = Convert.ToDecimal(totalAdvanceAmountMainCurrency);
                }
                else
                {
                    expenseDocumentRow.TotalAdvanceMainCurrency = Convert.ToDecimal(totalAdvanceAmountMainCurrency);
                    if (expenseDocumentRow.MainCurrencyID == expenseDocumentRow.LocalCurrencyID)
                    {
                        expenseDocumentRow.TotalAdvanceLocalCurrency = Convert.ToDecimal(totalAdvanceAmountMainCurrency);
                    }
                    else
                    {
                        expenseDocumentRow.TotalAdvanceLocalCurrency = Math.Round(expenseDocumentRow.TotalAdvanceMainCurrency * (expenseDocumentRow.IsExchangeRateForLocalCurrencyNull() ? 0 : expenseDocumentRow.ExchangeRateForLocalCurrency), 2, MidpointRounding.AwayFromZero);
                    }
                }
            }

            expenseDocumentRow.EndEdit();
        }
        #endregion public void RefreshAdvance(Guid txID, long expID)

        #region public double AddExpenseAdvanceToTransaction(Guid txID, long expID, IList<Advance> advanceList)
        public double AddExpenseAdvanceToTransaction(Guid txID, long expID, IList<Advance> advanceList, long taID)
        {
            ExpenseDataSet expenseDS = (ExpenseDataSet)TransactionService.GetDS(txID);
            ExpenseDataSet.FnExpenseDocumentRow expenseDocumentRow = (ExpenseDataSet.FnExpenseDocumentRow)expenseDS.FnExpenseDocument.FindByExpenseID(expID);
            string expenseType = this.GetExpenseType(expID, txID);
            foreach (Advance advance in advanceList)
            {
                string str = "AdvanceID = {0}";
                ExpenseDataSet.FnExpenseAdvanceRow[] dr = (ExpenseDataSet.FnExpenseAdvanceRow[])expenseDS.FnExpenseAdvance.Select(string.Format(str, advance.AdvanceID));
                if (dr.Length == 0)
                {
                    ExpenseDataSet.FnExpenseAdvanceRow expenseAdvanceRow = expenseDS.FnExpenseAdvance.NewFnExpenseAdvanceRow();
                    expenseAdvanceRow.ExpenseID = expID;
                    expenseAdvanceRow.AdvanceID = advance.AdvanceID;
                    expenseAdvanceRow.Active = true;
                    expenseAdvanceRow.CreBy = UserAccount.UserID;
                    expenseAdvanceRow.CreDate = DateTime.Now;
                    expenseAdvanceRow.UpdBy = UserAccount.UserID;
                    expenseAdvanceRow.UpdDate = DateTime.Now;
                    expenseAdvanceRow.UpdPgm = UserAccount.CurrentProgramCode;
                    expenseDS.FnExpenseAdvance.AddFnExpenseAdvanceRow(expenseAdvanceRow);
                }
            }

            // ทำการรวมค่า Advance ใหม่
            double totalAdvance = 0;
            double totalLocalAdvance = 0; //final currency
            double totalMainAdvance = 0;
            ExpenseDataSet.FnExpenseAdvanceRow[] rows = (ExpenseDataSet.FnExpenseAdvanceRow[])expenseDS.FnExpenseAdvance.Select();
            foreach (ExpenseDataSet.FnExpenseAdvanceRow row in rows)
            {
                if (row.RowState != DataRowState.Deleted)
                {
                    AvAdvanceDocument avAdvance = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.FindByIdentity(row.AdvanceID);
                    if (avAdvance != null)
                    {
                        if (Convert.ToBoolean(avAdvance.IsRepOffice))
                        {
                            if (expenseType.Equals(ZoneType.Domestic))
                            {
                                IList<AvAdvanceItem> items = ScgeAccountingQueryProvider.AvAdvanceItemQuery.FindByAvAdvanceItemAdvanceID(avAdvance.AdvanceID);

                                if (items.Count > 0)
                                {
                                    if (items[0].CurrencyID.CurrencyID == avAdvance.MainCurrencyID && items[0].CurrencyID.CurrencyID != expenseDocumentRow.LocalCurrencyID)
                                        totalLocalAdvance += (Convert.ToDouble(avAdvance.LocalCurrencyAmount) * Convert.ToDouble(avAdvance.ExchangeRateForLocalCurrency));
                                    else
                                        totalLocalAdvance += Convert.ToDouble(avAdvance.LocalCurrencyAmount);  // final currency
                                }

                                totalMainAdvance += Convert.ToDouble(avAdvance.MainCurrencyAmount);
                            }
                            else
                            {
                                totalMainAdvance += Convert.ToDouble(avAdvance.MainCurrencyAmount);
                            }
                        }
                        totalAdvance += avAdvance.Amount;
                    }
                }
            }

            if (expenseDocumentRow != null)
            {
                bool isRepOffice = expenseDocumentRow.IsIsRepOfficeNull() ? false : expenseDocumentRow.IsRepOffice;
                expenseDocumentRow.BeginEdit();
                if (!taID.Equals(0))
                {
                    expenseDocumentRow.TADocumentID = taID;
                }
                expenseDocumentRow.TotalAdvance = Convert.ToDecimal(totalAdvance);

                if (isRepOffice)
                {
                    if (expenseType.Equals(ZoneType.Domestic))
                    {
                        expenseDocumentRow.TotalAdvanceLocalCurrency = Convert.ToDecimal(totalLocalAdvance);
                        expenseDocumentRow.TotalAdvanceMainCurrency = Convert.ToDecimal(totalMainAdvance);
                    }
                    else
                    {
                        expenseDocumentRow.TotalAdvanceMainCurrency = Convert.ToDecimal(totalMainAdvance);
                        if (expenseDocumentRow.MainCurrencyID == expenseDocumentRow.LocalCurrencyID)
                        {
                            expenseDocumentRow.TotalAdvanceLocalCurrency = Convert.ToDecimal(totalMainAdvance);
                        }
                        else
                        {
                            expenseDocumentRow.TotalAdvanceLocalCurrency = Math.Round(expenseDocumentRow.TotalAdvanceMainCurrency * (expenseDocumentRow.IsExchangeRateForLocalCurrencyNull() ? 0 : expenseDocumentRow.ExchangeRateForLocalCurrency), 2, MidpointRounding.AwayFromZero);
                        }
                    }
                }
                expenseDocumentRow.EndEdit();
                //------------------------------------------------
            }

            return totalAdvance;
        }
        #endregion public double AddExpenseAdvanceToTransaction(Guid txID, long expID, IList<Advance> advanceList)

        #region public double DeleteExpenseAdvanceFromTransaction(Guid txID, long advanceID, double amount)
        public double DeleteExpenseAdvanceFromTransaction(Guid txID, long advanceID, double amount)
        {
            double totalAdvance = 0;
            ExpenseDataSet expenseDS = (ExpenseDataSet)TransactionService.GetDS(txID);
            ExpenseDataSet.FnExpenseAdvanceRow[] rows = (ExpenseDataSet.FnExpenseAdvanceRow[])expenseDS.FnExpenseAdvance.Select();

            foreach (ExpenseDataSet.FnExpenseAdvanceRow row in rows)
            {
                if (row.AdvanceID == advanceID)
                {
                    row.Delete();
                }
                else if (row.RowState != DataRowState.Deleted)
                {
                    AvAdvanceDocument avAdvance = AvAdvanceDocumentService.FindByIdentity(row.AdvanceID);
                    if (avAdvance != null)
                        totalAdvance += avAdvance.Amount;
                }
            }
            return totalAdvance;
        }
        #endregion public double DeleteExpenseAdvanceFromTransaction(Guid txID, long advanceID, double amount)

        #region public void SetTotalAdvance(Guid txID, long taID, double totalAdvance)
        public void SetTotalAdvance(Guid txID, long taID, double totalAdvance)
        {
            ExpenseDataSet expenseDS = (ExpenseDataSet)TransactionService.GetDS(txID);
            ExpenseDataSet.FnExpenseDocumentRow[] expenseDocumentRows = (ExpenseDataSet.FnExpenseDocumentRow[])expenseDS.FnExpenseDocument.Select();
            //Set total amount in expense
            if (expenseDocumentRows.Length > 0)
            {
                ExpenseDataSet.FnExpenseDocumentRow expenseDocumentRow = (ExpenseDataSet.FnExpenseDocumentRow)expenseDocumentRows[0];
                expenseDocumentRow.BeginEdit();
                if (!taID.Equals(0))
                {
                    expenseDocumentRow.TADocumentID = taID;
                }
                expenseDocumentRow.TotalAdvance = Convert.ToDecimal(totalAdvance);
                expenseDocumentRow.EndEdit();
                //------------------------------------------------
            }
        }
        #endregion public void SetTotalAdvance(Guid txID, long taID, double totalAdvance)

        public decimal CalculateTotalExpense(Guid txID, long expId, bool isRepOffice)
        {
            decimal totalExpense = 0;
            decimal totalExpenseLocalCurrency = 0;
            decimal totalExpenseMainCurrency = 0;
            ExpenseDataSet expenseDS = (ExpenseDataSet)TransactionService.GetDS(txID);
            if (expenseDS != null)
            {
                ExpenseDataSet.FnExpenseDocumentRow expRow = expenseDS.FnExpenseDocument.FindByExpenseID(expId);

                string filter = String.Format("ExpenseID = {0}", expId);
                ExpenseDataSet.FnExpenseInvoiceRow[] rows = (ExpenseDataSet.FnExpenseInvoiceRow[])expenseDS.FnExpenseInvoice.Select(filter);

                foreach (ExpenseDataSet.FnExpenseInvoiceRow row in rows)
                {
                    if (!isRepOffice)
                    {
                        if (!row.IsNetAmountNull())
                        {
                            totalExpense += row.NetAmount;
                        }
                    }
                    else
                    {
                        if (!row.IsNetAmountLocalCurrencyNull())
                        {
                            totalExpenseLocalCurrency += row.NetAmountLocalCurrency;
                        }

                        ExpenseDataSet.FnExpenseInvoiceItemRow[] rowItems = (ExpenseDataSet.FnExpenseInvoiceItemRow[])expenseDS.FnExpenseInvoiceItem.Select(string.Format("InvoiceID = {0}", row.InvoiceID));
                        foreach (ExpenseDataSet.FnExpenseInvoiceItemRow itemRow in rowItems)
                        {
                            totalExpenseMainCurrency += (decimal)itemRow.MainCurrencyAmount;
                        }
                    }
                }

                expRow.BeginEdit();

                if (isRepOffice)
                {
                    expRow.TotalExpenseLocalCurrency = totalExpenseLocalCurrency;
                    expRow.TotalExpenseMainCurrency = totalExpenseMainCurrency;

                    if (!expRow.IsExchangeRateMainToTHBCurrencyNull())
                    {
                        expRow.TotalExpense = Math.Round((expRow.IsTotalExpenseMainCurrencyNull() ? 0 : expRow.TotalExpenseMainCurrency) * expRow.ExchangeRateMainToTHBCurrency, 2, MidpointRounding.AwayFromZero);
                    }
                }
                else
                {
                    expRow.TotalExpense = totalExpense;
                }
                expRow.EndEdit();
            }
            else
            {
                logger.Error("CalculateTotalExpense(Guid txID, long expId) : cannot get expense dataset : ExpenseDataSet is null");
            }
            return totalExpense;
        }

        public void CalculateDifferenceAmount(Guid txID, long expId, bool isRepOffice)
        {
            this.RefreshRemittance(txID, expId);
            this.RefreshAdvance(txID, expId);

            decimal difAmount = 0;
            decimal difAmountLocalCurrency = 0;
            decimal difAmountMainCurrency = 0;
            ExpenseDataSet expenseDS = (ExpenseDataSet)TransactionService.GetDS(txID);
            if (expenseDS != null)
            {
                ExpenseDataSet.FnExpenseDocumentRow expRow = expenseDS.FnExpenseDocument.FindByExpenseID(expId);
                if (expRow != null)
                {
                    expRow.BeginEdit();
                    difAmount = (expRow.TotalExpense - expRow.TotalAdvance);
                    if (expRow.ExpenseType.Equals(ZoneType.Foreign))
                        difAmount += expRow.TotalRemittance;

                    expRow.DifferenceAmount = difAmount;
                    if (isRepOffice)
                    {
                        difAmountLocalCurrency = expRow.TotalExpenseLocalCurrency - expRow.TotalAdvanceLocalCurrency;
                        difAmountMainCurrency = expRow.TotalExpenseMainCurrency - expRow.TotalAdvanceMainCurrency;
                        if (expRow.ExpenseType.Equals(ZoneType.Foreign))
                        {
                            difAmountLocalCurrency += expRow.TotalRemittanceLocalCurrency;
                            difAmountMainCurrency += expRow.TotalRemittanceMainCurrency;
                        }

                        expRow.DifferenceAmountLocalCurrency = difAmountLocalCurrency;
                        expRow.DifferenceAmountMainCurrency = difAmountMainCurrency;
                    }
                    expRow.EndEdit();
                }
            }
            else
            {
                logger.Error("CalculateDifferenceAmount(Guid txID, long expId) : cannot get expense dataset : ExpenseDataSet is null");
            }
            //return difAmount;
        }

        #endregion


        #region public InvoiceExchangeRate GetAdvanceExchangeRate(Guid txID, short currencyID)
        public InvoiceExchangeRate GetAdvanceExchangeRate(Guid txID, short currencyID)
        {
            ExpenseDataSet expenseDs = (ExpenseDataSet)TransactionService.GetDS(txID);
            IList<long> advanceIDList = new List<long>();
            ExpenseDataSet.FnExpenseAdvanceRow[] rows = (ExpenseDataSet.FnExpenseAdvanceRow[])expenseDs.FnExpenseAdvance.Select();
            foreach (ExpenseDataSet.FnExpenseAdvanceRow row in rows)
            {
                advanceIDList.Add(row.AdvanceID);
            }

            if (advanceIDList.Count > 0)
            {
                InvoiceExchangeRate invoiceExchangeRate = ScgeAccountingQueryProvider.AvAdvanceItemQuery.GetAdvanceExchangeRate(advanceIDList, currencyID);
                if (invoiceExchangeRate != null)
                {
                    if (!invoiceExchangeRate.TotalAmount.Equals(0))
                    {
                        invoiceExchangeRate.AdvanceExchangeRateAmount = (double)Math.Round((decimal)(invoiceExchangeRate.TotalAmountTHB / invoiceExchangeRate.TotalAmount), 5, MidpointRounding.AwayFromZero);
                        return invoiceExchangeRate;
                    }
                }
                else
                {
                    if (currencyID == ParameterServices.USDCurrencyID)
                    {
                        InvoiceExchangeRate perdiemExRate = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.GetPerdiemExchangeRateUSD(advanceIDList);
                        if (perdiemExRate != null)
                        {
                            perdiemExRate.AdvanceExchangeRateAmount = (double)Math.Round((decimal)(perdiemExRate.AdvanceExchangeRateAmount / advanceIDList.Count), 5, MidpointRounding.AwayFromZero);
                            return perdiemExRate;
                        }
                    }
                }
            }

            return null;
        }
        #endregion

        #region unused
        //public string GetExpenseDocumentLatex(int documentID)
        //{
        //    SCGDocument doc = ScgeAccountingQueryProvider.SCGDocumentQuery.FindByIdentity(documentID);
        //    FnExpenseDocument ex = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseDocumentByDocumentID(documentID);
        //    IList<InvoiceData> invoiceData = ScgeAccountingQueryProvider.FnExpenseInvoiceQuery.FindInvoiceDataByExpenseID(ex.ExpenseID);

        //    int seq = 1;
        //    int prevInvoiceId = -1;
        //    foreach (InvoiceData item in invoiceData)
        //    {
        //        if (item.InvoiceId != prevInvoiceId)
        //        {
        //            prevInvoiceId = item.InvoiceId;
        //            item.Seq = seq;
        //            seq++;

        //        }
        //    }


        //    //IList<FnExpenseDocument> fnExpenseDocument = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetPaymentType(documentID); //get paymentType
        //    string paymentType = ScgDbQueryProvider.SCGDbStatusLangQuery.GetStatusLang("PaymentTypeDMT", 1, ex.PaymentType);
        //    //AvAdvanceDocument doc1 = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.GetAvAdvanceByDocumentID(documentID);//get PBID
        //    AdvanceFormDbPBData pbData = ScgDbQueryProvider.DbPBQuery.GetDescription(ex.PB.Pbid, 1);
        //    IList<AdvanceData> advanceData = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.FindAdvanceDataByExpenseID(ex.ExpenseID);

        //    StringTemplateGroup group = new StringTemplateGroup("group", AppDomain.CurrentDomain.BaseDirectory + "Resources\\Template");
        //    StringTemplate xml;
        //    if (advanceData.Count == 1)
        //    {
        //        xml = new StringTemplate();
        //        xml = group.GetInstanceOf("Petty");
        //    }

        //    else
        //    {
        //        xml = new StringTemplate();
        //        xml = group.GetInstanceOf("AdvanceRemittance");
        //    }

        //    //InvoiceData[] invoice = new InvoiceData[invoiceData.Count];
        //    for (int i = 0; i < invoiceData.Count; i++)
        //    {
        //        invoiceData[i].InvoiceNo = Latex.Escape(invoiceData[i].InvoiceNo);
        //        //invoiceData[i].InvoiceDate = invoiceData[i].InvoiceDate;
        //        //invoiceData[i].TotalAmount = invoiceData[i].TotalAmount;
        //        //invoiceData[i].VatAmount = invoiceData[i].VatAmount;
        //        //invoiceData[i].WHTAmount = invoiceData[i].WHTAmount;
        //        //invoiceData[i].NetAmount = invoiceData[i].NetAmount;
        //        invoiceData[i].Description = Latex.Escape(invoiceData[i].Description);
        //        //invoiceData[i].Amount = invoiceData[i].Amount;
        //        invoiceData[i].VendorName1 = Latex.Escape(invoiceData[i].VendorName1);
        //        invoiceData[i].VendorName2 = Latex.Escape(invoiceData[i].VendorName2);
        //        invoiceData[i].AccountCode = Latex.Escape(invoiceData[i].AccountCode);
        //        invoiceData[i].AccountName = Latex.Escape(invoiceData[i].AccountName);
        //        invoiceData[i].IONumber = Latex.Escape(invoiceData[i].IONumber);
        //    }

        //    //AdvanceData[] advance = new AdvanceData[advanceData.Count];

        //    for (int i = 0; i < advanceData.Count; i++)
        //    {
        //        advanceData[i].DocumentNo = Latex.Escape(advanceData[i].DocumentNo);
        //        //advanceData[i].RequestDateOfAdvance = advanceData[i].RequestDateOfAdvance;
        //        //advanceData[i].Amount = advanceData[i].Amount;
        //    }

        //    xml.SetAttribute("DataList", invoiceData);
        //    xml.SetAttribute("DataList2", advanceData);

        //    //double totalAmount = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.FindAvAdvanceDocumentByDocumentID(documentID);
        //    double totalAmount = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetSumAmountOfAdvanceDocument(ex.ExpenseID);
        //    //double totalNetAmount = ScgeAccountingQueryProvider.FnExpenseInvoiceQuery.FindExpenseInvoiceByDocumentID(documentID);
        //    double difference = ex.TotalExpense - totalAmount;
        //    string thaiAmount = SS.Standard.Utilities.Utilities.NumericToThai(ex.TotalExpense);

        //    if (difference == 0)
        //    {
        //        xml.SetAttribute("difference1", Latex.Escape("0.00"));
        //    }

        //    else
        //    {
        //        xml.SetAttribute("difference1", difference.ToString("#,#.00"));

        //        if (difference < 0)
        //        {
        //            xml.SetAttribute("difference2", Latex.Escape("Pay Back to Company"));
        //            xml.SetAttribute("difference3", Latex.Escape("จ่ายคืนบริษัท"));
        //        }
        //        else
        //        {
        //            xml.SetAttribute("difference2", Latex.Escape("Receive from Company"));
        //            xml.SetAttribute("difference3", Latex.Escape("รับจากบริษัท"));
        //        }

        //    }

        //    xml.SetAttribute("companyCode", Latex.Escape(doc.CompanyID.CompanyCode));
        //    xml.SetAttribute("companyName", Latex.Escape(doc.CompanyID.CompanyName));
        //    xml.SetAttribute("documentNo", Latex.Escape(doc.DocumentNo));
        //    CostCenterData costcenter = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetCostCenterData(ex.ExpenseID);
        //    xml.SetAttribute("costCenterName", Latex.Escape(costcenter.Description));
        //    xml.SetAttribute("costCenterCode", Latex.Escape(costcenter.CostCenterCode));
        //    xml.SetAttribute("date", doc.DocumentDate);
        //    xml.SetAttribute("beneficName", Latex.Escape(doc.ReceiverID.EmployeeName));
        //    xml.SetAttribute("employeeCode", Latex.Escape(doc.ReceiverID.EmployeeCode));
        //    xml.SetAttribute("phoneNo", Latex.Escape(doc.ReceiverID.PhoneNo));
        //    xml.SetAttribute("subject", Latex.Escape(doc.Subject));
        //    //xml.SetAttribute("seq", );
        //    //xml.SetAttribute("invoiceNo", );
        //    //xml.SetAttribute("invoiceDate", avd.);
        //    //xml.SetAttribute("vendor1", avd.);
        //    //xml.SetAttribute("vendor2", avd.);
        //    //xml.SetAttribute("totalAmount", avd.);
        //    //xml.SetAttribute("vatAmount", avd.);
        //    //xml.SetAttribute("whtAmount", avd.);
        //    //xml.SetAttribute("netAmount", avd.);
        //    //xml.SetAttribute("description", avd.);
        //    //xml.SetAttribute("accountCode", avd.);
        //    //xml.SetAttribute("accountName", avd.);
        //    //xml.SetAttribute("ioNumber", avd.);
        //    //xml.SetAttribute("amountTHB", avd.);
        //    xml.SetAttribute("numToString", Latex.Escape(thaiAmount));
        //    xml.SetAttribute("sumNetAmount", ex.TotalExpense);
        //    //xml.SetAttribute("sumAmount", totalAmount.ToString("#,#.00"));

        //    xml.SetAttribute("paymentType", Latex.Escape(ex.PaymentType));
        //    xml.SetAttribute("paymentType2", Latex.Escape(pbData.PBCode));
        //    xml.SetAttribute("paymentType3", Latex.Escape(pbData.Description));
        //    //xml.SetAttribute("treasury", );
        //    //xml.SetAttribute("receiverBy", );
        //    xml.SetAttribute("userNameCre", Latex.Escape(doc.CreatorID.UserName));
        //    xml.SetAttribute("employeeNameCre", Latex.Escape(doc.CreatorID.EmployeeName));
        //    xml.SetAttribute("positionNameCre", Latex.Escape(doc.CreatorID.PositionName));
        //    xml.SetAttribute("userNameApp", Latex.Escape(doc.ApproverID.UserName));
        //    xml.SetAttribute("employeeNameApp", Latex.Escape(doc.ApproverID.EmployeeName));
        //    xml.SetAttribute("positionNameApp", Latex.Escape(doc.ApproverID.PositionName));
        //    //xml.SetAttribute("dueDateOfRemittance", avd);
        //    //xml.SetAttribute("reason", avd);
        //    //xml.SetAttribute("issueBy", avd);
        //    //xml.SetAttribute("approveBy", avd);
        //    //xml.SetAttribute("verifyDoc", );
        //    //xml.SetAttribute("approveVerify", );

        //    return xml.ToString();
        //}
        #endregion

        #region public void UpdateExpenseDocumentAdvanceToTransaction(FnExpenseDocument exp, Guid txId)
        public void UpdateExpenseDocumentAdvanceToTransaction(FnExpenseDocument exp, Guid txId)
        {
            bool isRepOffice = false;
            if (exp != null)
            {
                ExpenseDataSet ds = (ExpenseDataSet)TransactionService.GetDS(txId);

                if (ds == null)
                {
                    logger.Error("UpdateExpenseDocumentAdvanceToTransaction(FnExpenseDocument exp, Guid txId) : cannot get expense dataset : ExpenseDataSet is null");
                }
                ExpenseDataSet.FnExpenseDocumentRow expRow = ds.FnExpenseDocument.FindByExpenseID(exp.ExpenseID);
                ExpenseDataSet.DocumentRow doc = ds.Document.FindByDocumentID(expRow.DocumentID);

                if (!expRow.IsIsRepOfficeNull())
                    isRepOffice = expRow.IsRepOffice;

                Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
                if (expRow.ExpenseType.Equals(ZoneType.Foreign) && (!expRow.IsTADocumentIDNull() && expRow.TADocumentID > 0))
                {
                    if ((!exp.IsBusinessPurpose.HasValue) && (!exp.IsTrainningPurpose.HasValue) && (!exp.IsOtherPurpose.HasValue))
                    {
                        errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("PurposeIsRequired"));
                    }
                    if (!exp.FromDate.HasValue)
                    {
                        errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("FromDateIsRequired"));
                    }
                    if (!exp.ToDate.HasValue)
                    {
                        errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("ToDateIsRequired"));
                    }
                    if (string.IsNullOrEmpty(exp.Country))
                    {
                        errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("CountryIsRequired"));
                    }
                }

                if (!errors.IsEmpty) throw new ServiceValidationException(errors);

                expRow.BeginEdit();
                if (exp.IsBusinessPurpose.HasValue)
                    expRow.IsBusinessPurpose = exp.IsBusinessPurpose.Value;

                if (exp.IsTrainningPurpose.HasValue)
                    expRow.IsTrainningPurpose = exp.IsTrainningPurpose.Value;

                if (exp.IsOtherPurpose.HasValue)
                    expRow.IsOtherPurpose = exp.IsOtherPurpose.Value;
                expRow.OtherPurposeDescription = exp.OtherPurposeDescription;

                if (exp.FromDate.HasValue)
                    expRow.FromDate = exp.FromDate.Value;

                if (exp.ToDate.HasValue)
                    expRow.ToDate = exp.ToDate.Value;

                expRow.Country = exp.Country;
                if (!doc.IsRequesterIDNull() && doc.RequesterID > 0)
                {
                    SS.SU.DTO.SuUser user = QueryProvider.SuUserQuery.FindProxyByIdentity(doc.RequesterID);
                    expRow.PersonalLevel = user.PersonalLevel;
                }
                InvoiceExchangeRate invoiceExchangeRate = null;

                if (!isRepOffice)
                {
                    invoiceExchangeRate = GetAdvanceExchangeRate(txId, ParameterServices.USDCurrencyID);
                }
                else
                {
                    invoiceExchangeRate = GetAdvanceExchangeRateRepOffice(txId, ParameterServices.USDCurrencyID, exp.ExpenseID);
                }
                expRow.ExchangeRateForUSDAdvance = invoiceExchangeRate == null ? (decimal)0 : (decimal)invoiceExchangeRate.AdvanceExchangeRateAmount;

                expRow.EndEdit();
            }
        }
        #endregion public void UpdateExpenseDocumentAdvanceToTransaction(FnExpenseDocument exp, Guid txId)

        #region public void UpdateExpenseDocument(Guid txID, FnExpenseDocument exp)
        public void UpdateExpenseDocument(Guid txID, FnExpenseDocument exp)
        {
            bool isRepOffice = false;
            if (exp != null)
            {
                if (exp.IsRepOffice.HasValue)
                    isRepOffice = exp.IsRepOffice.Value;
                ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(txID);

                string filter = String.Format("ExpenseID = {0} and InvoiceDocumentType = 'G' ", exp.ExpenseID);

                DataRow[] rowsInvoice = expDS.FnExpenseInvoice.Select(filter);
                ExpenseDataSet.FnExpenseDocumentRow rowExp = expDS.FnExpenseDocument.FindByExpenseID(exp.ExpenseID);
                long documentId = exp == null ? 0 : rowExp.DocumentID;
                //Dbpb pb = ScgDbQueryProvider.DbPBQuery.FindByIdentity(rowExp.PBID);

                SS.Standard.WorkFlow.DTO.WorkFlow workflow = SS.Standard.WorkFlow.Query.WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(documentId);

                exp.LoadFromDataRow(rowExp);

                string filterPerdiem = String.Format("ExpenseID = {0}", exp.ExpenseID);

                DataRow[] rowsPerdiem = expDS.FnExpensePerdiem.Select(filterPerdiem);

                foreach (DataRow row in rowsPerdiem)
                {
                    FnExpensePerdiem perdiem = new FnExpensePerdiem();
                    FnExpensePerdiemItem perdiemItem = new FnExpensePerdiemItem();
                    perdiem.LoadFromDataRow(row);

                    if (workflow == null || (workflow != null && workflow.CurrentState.Name == WorkFlowStateFlag.Draft) || workflow.CurrentState.Name.Equals(WorkFlowStateFlag.WaitVerify))
                    {
                        if (isRepOffice && (exp.LocalCurrencyID != exp.MainCurrencyID) && exp.ExchangeRateForLocalCurrency.HasValue)
                        {
                            perdiem.ExchangeRate = exp.ExchangeRateForLocalCurrency.Value;
                        }
                    }                   
                    FnExpensePerdiemService.UpdateExpensePerdiemTransaction(perdiem, null, txID);
                }

                //validation Mileage
                string filterMileage = String.Format("ExpenseID = {0}", exp.ExpenseID);
                DataRow[] rowsMileage = expDS.FnExpenseMileage.Select(filterMileage);

                foreach (DataRow row in rowsInvoice)
                {
                    FnExpenseInvoice invoice = new FnExpenseInvoice();

                    FnExpenseInvoiceItemService.UpdateInvoiceItemByInvoiceID(txID, Convert.ToInt64(row["InvoiceID"].ToString()), exp);

                    invoice.LoadFromDataRow(row);
                    double? rate = null;
                    double? rateNondeduct = null;
                    DbTax tax = null;

                    invoice.TotalBaseAmount = FnExpenseInvoiceService.CalculateBaseAmount(txID, invoice.InvoiceID);

                    if (workflow == null || (workflow != null && workflow.CurrentState.Name == WorkFlowStateFlag.Draft))
                    {
                        if (invoice.IsVAT.HasValue && invoice.IsVAT.Value)
                        {
                            if (!invoice.TaxID.HasValue)
                            {
                                tax = ScgDbQueryProvider.DbTaxQuery.FindbyTaxCode(ParameterServices.DefaultTaxCode);
                            }
                            else
                            {
                                tax = ScgDbQueryProvider.DbTaxQuery.FindByIdentity(invoice.TaxID.Value);
                            }

                            if (tax != null)
                            {
                                rate = tax.Rate;

                                if (tax.RateNonDeduct > 0)
                                    rateNondeduct = tax.RateNonDeduct;
                            }

                            invoice.VatAmount = FnExpenseInvoiceService.CalculateVATAmount(invoice.TotalBaseAmount, rate);
                            invoice.NonDeductAmount = FnExpenseInvoiceService.CalculateVATAverage(invoice.TotalBaseAmount, rateNondeduct);

                        }
                        if (invoice.IsWHT.HasValue && invoice.IsWHT.Value)
                        {
                            invoice.WHTAmount1 = FnExpenseInvoiceService.CalculateWHTAmount(invoice.BaseAmount1.Value, invoice.WHTRate1.Value);
                            invoice.WHTAmount2 = FnExpenseInvoiceService.CalculateWHTAmount(invoice.BaseAmount2.Value, invoice.WHTRate2.Value);
                            invoice.WHTAmount = invoice.WHTAmount1.Value + invoice.WHTAmount2.Value;
                        }
                    }
                    else if (workflow.CurrentState.Name.Equals(WorkFlowStateFlag.WaitVerify))
                    {
                        if (isRepOffice)
                        {
                            invoice.ExchangeRateForLocalCurrency = exp.ExchangeRateForLocalCurrency;
                            invoice.ExchangeRateMainToTHBCurrency = exp.ExchangeRateMainToTHBCurrency;
                        }
                    }

                    invoice.TotalBaseAmount = invoice.TotalBaseAmount + invoice.NonDeductAmount;
                    invoice.NetAmount = FnExpenseInvoiceService.CalculateNetAmount(invoice.TotalBaseAmount, invoice.VatAmount, invoice.WHTAmount);
                    //FnExpenseInvoiceService.UpdateInvoiceOnTransaction(invoice, txID);
                    FnExpenseInvoiceService.UpdateInvoiceOnTransactionNonValidation(invoice, txID);
                }

                this.CalculateTotalExpense(txID, exp.ExpenseID, isRepOffice);
                this.CalculateDifferenceAmount(txID, exp.ExpenseID, isRepOffice);
            }
        }
        #endregion public void UpdateExpenseDocument(Guid txID, FnExpenseDocument exp)


        #region ****** GetExchangeRateForUSD && GetExpenseType
        public double GetExchangeRateForUSD(long expenseID, Guid txId)
        {
            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(txId);
            if (expDS == null)
                logger.Error("GetExchangeRateForUSD(long expenseID, Guid txId) : cannot get expense dataset : ExpenseDataSet is null");
            ExpenseDataSet.FnExpenseDocumentRow row = expDS.FnExpenseDocument.FindByExpenseID(expenseID);
            double result = (double)0;

            if (!row.IsExchangeRateForUSDNull())
            {
                result = (double)row.ExchangeRateForUSD;
            }
            return result;
        }
        public string GetExpenseType(long expenseID, Guid txId)
        {
            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(txId);
            ExpenseDataSet.FnExpenseDocumentRow row = expDS.FnExpenseDocument.FindByExpenseID(expenseID);
            string result = string.Empty;

            if (!row.IsExpenseTypeNull())
            {
                result = row.ExpenseType;
            }

            return row.ExpenseType;
        }
        public double GetExpenseTotalVatAmount(Guid txId, long expenseId)
        {
            double totalVatAmount = 0;
            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(txId);
            DataRow[] invoiceRows = expDS.FnExpenseInvoice.Select(string.Format("ExpenseID={0}", expenseId));
            foreach (ExpenseDataSet.FnExpenseInvoiceRow row in invoiceRows)
            {
                totalVatAmount += (double)row.VatAmount;
            }
            return totalVatAmount;
        }
        public double GetExpenseTotalWHTAmount(Guid txId, long expenseId)
        {
            double totalWHTAmount = 0;
            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(txId);
            DataRow[] invoiceRows = expDS.FnExpenseInvoice.Select(string.Format("ExpenseID={0}", expenseId));
            foreach (ExpenseDataSet.FnExpenseInvoiceRow row in invoiceRows)
            {
                totalWHTAmount += (double)row.WHTAmount;
            }
            return totalWHTAmount;
        }
        public double GetExpenseTotalNetAmount(Guid txId, long expenseId)
        {
            double totalNetAmount = 0;
            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(txId);
            DataRow[] invoiceRows = expDS.FnExpenseInvoice.Select(string.Format("ExpenseID={0}", expenseId));

            ExpenseDataSet.FnExpenseDocumentRow expRow = expDS.FnExpenseDocument.FindByExpenseID(expenseId);
            bool isRepOffice = expRow.IsIsRepOfficeNull() ? false : expRow.IsRepOffice;

            foreach (ExpenseDataSet.FnExpenseInvoiceRow row in invoiceRows)
            {
                if (!isRepOffice)
                {
                    totalNetAmount += (double)row.NetAmount;
                }
                else
                {
                    totalNetAmount += (double)row.NetAmountLocalCurrency;
                }
            }
            return totalNetAmount;
        }
        public double GetExpenseTotalBaseAmount(Guid txId, long expenseId)
        {
            double totalBaseAmount = 0;
            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(txId);
            DataRow[] invoiceRows = expDS.FnExpenseInvoice.Select(string.Format("ExpenseID={0}", expenseId));

            ExpenseDataSet.FnExpenseDocumentRow expRow = expDS.FnExpenseDocument.FindByExpenseID(expenseId);
            bool isRepOffice = expRow.IsIsRepOfficeNull() ? false : expRow.IsRepOffice;

            foreach (ExpenseDataSet.FnExpenseInvoiceRow row in invoiceRows)
            {
                if (!isRepOffice)
                {
                    totalBaseAmount += (double)row.TotalBaseAmount;
                }
                else
                {
                    totalBaseAmount += (double)row.TotalBaseAmountLocalCurrency;
                }
            }
            return totalBaseAmount;
        }
        #endregion

        public void CanChangeCompany(Guid txId, long expenseId)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            string ErrorMessage;
            bool canChangeCompany = true;
            ExpenseDataSet expDs = (ExpenseDataSet)TransactionService.GetDS(txId);

            DataRow[] invoiceRows = expDs.FnExpenseInvoice.Select(String.Format("ExpenseID = {0}", expenseId));

            DataRow[] advRows = expDs.FnExpenseAdvance.Select(String.Format("ExpenseID = {0}", expenseId));

            if (invoiceRows.Length > 0)
            {
                canChangeCompany = false;
            }
            else if (advRows.Length > 0)
            {
                canChangeCompany = false;
            }

            if (expDs.FnExpenseCA.Count() != 0 || expDs.FnExpenseMPA.Count() != 0)
            {
                canChangeCompany = false;
                ErrorMessage = "MPA or CA Document Attachment has already exist. Company can't be change";
            }
            else
            {
                ErrorMessage = "Company can be changed on a blank form only.";
            }

            if (!canChangeCompany)
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage(ErrorMessage));

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
        }

        public string GenerateDefaultBoxID(long documentId)
        {
            SCGDocument doc = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(documentId);
            string boxId = " ";
            if (doc.CompanyID != null)
            {
                boxId = doc.CompanyID.CompanyCode.ToString() + "P2PEMP" + (DateTime.Now.Year.ToString().Substring(2, 2)) + "0";
                Console.WriteLine(boxId);
            }
            return boxId;

        }

        #region public InvoiceExchangeRate GetAdvanceExchangeRateRepOffice(Guid txID, short currencyID)
        public InvoiceExchangeRate GetAdvanceExchangeRateRepOffice(Guid txID, short currencyID, long expId)
        {
            ExpenseDataSet expenseDs = (ExpenseDataSet)TransactionService.GetDS(txID);
            ExpenseDataSet.FnExpenseDocumentRow expRow = expenseDs.FnExpenseDocument.FindByExpenseID(expId);
            //bool isRepOffice = expRow.IsIsRepOfficeNull() ? false : expRow.IsRepOffice;
            IList<long> advanceIDList = new List<long>();
            ExpenseDataSet.FnExpenseAdvanceRow[] rows = (ExpenseDataSet.FnExpenseAdvanceRow[])expenseDs.FnExpenseAdvance.Select();
            foreach (ExpenseDataSet.FnExpenseAdvanceRow row in rows)
            {
                advanceIDList.Add(row.AdvanceID);
            }

            if (advanceIDList.Count > 0)
            {
                InvoiceExchangeRate invoiceExchangeRate = ScgeAccountingQueryProvider.AvAdvanceItemQuery.GetAdvanceExchangeRate(advanceIDList, currencyID);
                if (invoiceExchangeRate != null)
                {
                    if (!invoiceExchangeRate.TotalAmount.Equals(0))
                    {
                        if (expRow.LocalCurrencyID == expRow.MainCurrencyID)
                        {
                            invoiceExchangeRate.AdvanceExchangeRateAmount = (double)Math.Round((decimal)(invoiceExchangeRate.TotalAmountMainCurrency / invoiceExchangeRate.TotalAmount), 5, MidpointRounding.AwayFromZero);
                        }
                        else
                        {
                            invoiceExchangeRate.AdvanceExchangeRateAmount = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.GetExchangeRateLocalCurrencyToMainCurrency(advanceIDList);
                        }
                        return invoiceExchangeRate;
                    }
                }
                else
                {
                    if (currencyID == ParameterServices.USDCurrencyID)
                    {
                        InvoiceExchangeRate perdiemExRate = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.GetPerdiemExchangeRateUSD(advanceIDList);
                        if (perdiemExRate != null)
                        {
                            perdiemExRate.AdvanceExchangeRateAmount = (double)Math.Round((decimal)(perdiemExRate.AdvanceExchangeRateAmount / advanceIDList.Count), 5, MidpointRounding.AwayFromZero);
                            return perdiemExRate;
                        }
                    }
                }
            }
            return null;
        }
        #endregion

        private bool CanChangeAmountExpenseDocument(FnExpenseDocument expenseDocument)
        {
            if (expenseDocument.TotalExpense > expenseDocument.AmountApproved)
            {
                IList<SuRole> documentPermitRoles = new List<SuRole>();

                //Check user role can Verify document
                IList<SuRole> permitRoles = QueryProvider.SuUserRoleQuery.GetRoleVerifyDocument();

                //Check limit verify amount
                var roles = from role in permitRoles
                            where expenseDocument.TotalExpense >= role.VerifyMinLimit
                                  && expenseDocument.TotalExpense <= role.VerifyMaxLimit
                            select role;

                documentPermitRoles = roles.ToList<SuRole>();

                if (permitRoles.Count > 0)
                {
                    //Check Service Team
                    IList<SuRole> serviceTeamRoles = QueryProvider.SuRoleServiceQuery.GetRoleByServiceTeamID(expenseDocument.ServiceTeam.ServiceTeamID);
                    var documentRoles = from role in permitRoles
                                        join serviceTeamRole in serviceTeamRoles
                                        on role.RoleID equals serviceTeamRole.RoleID
                                        select role;

                    documentPermitRoles = documentRoles.ToList<SuRole>();
                }

                IList<SuRole> currentUserRoles = new List<SuRole>();
                IList<SuUserRole> userRoles = SS.SU.Query.QueryProvider.SuUserRoleQuery.FindUserRoleByUserId(UserAccount.UserID);
                foreach (SuUserRole userRole in userRoles)
                {
                    currentUserRoles.Add(userRole.Role);
                }

                // check limit amount verifier can change
                foreach (SuRole role in currentUserRoles)
                {
                    var matchRole = from p in documentPermitRoles
                                    where p.RoleID == role.RoleID
                                    && expenseDocument.TotalExpense > (expenseDocument.AmountApproved + (role.UseCustomizationLimitAmount ? role.LimitAmountForVerifierChange : ParameterServices.LimitAmountForVerifierChange))
                                    select p;
                    if (matchRole.Count<SuRole>() > 0)
                        return false;
                }
            }

            return true;
        }


        #region public double AddExpenseMPAToTransaction(Guid txID, long expID, IList<ExpensesMPA> advanceList)
        public void AddExpenseMPAToTransaction(Guid txID, long expID, IList<ExpensesMPA> ExpenseMPAList)
        {
            ExpenseDataSet expenseDS = (ExpenseDataSet)TransactionService.GetDS(txID);
            ExpenseDataSet.FnExpenseDocumentRow expenseDocumentRow = (ExpenseDataSet.FnExpenseDocumentRow)expenseDS.FnExpenseDocument.FindByExpenseID(expID);
            //string expenseType = this.GetExpenseType(expID, txID);
            foreach (ExpensesMPA ExpenseMPA in ExpenseMPAList)
            {
                string str = "MPADocumentID = {0}";
                ExpenseDataSet.FnExpenseMPARow[] dr = (ExpenseDataSet.FnExpenseMPARow[])expenseDS.FnExpenseMPA.Select(string.Format(str, ExpenseMPA.MPADocumentID));
                if (dr.Length == 0)
                {
                    ExpenseDataSet.FnExpenseMPARow expenseExpenseMPARow = expenseDS.FnExpenseMPA.NewFnExpenseMPARow();
                    expenseExpenseMPARow.MPADocumentID = (long)ExpenseMPA.MPADocumentID;
                    expenseExpenseMPARow.ExpenseID = expID;
                    expenseExpenseMPARow.Active = true;
                    expenseExpenseMPARow.CreBy = UserAccount.UserID;
                    expenseExpenseMPARow.CreDate = DateTime.Now;
                    expenseExpenseMPARow.UpdBy = UserAccount.UserID;
                    expenseExpenseMPARow.UpdDate = DateTime.Now;
                    expenseExpenseMPARow.UpdPgm = UserAccount.CurrentProgramCode;
                    expenseDS.FnExpenseMPA.AddFnExpenseMPARow(expenseExpenseMPARow);
                }
            }
        }
        #endregion public double AddExpenseAdvanceToTransaction(Guid txID, long expID, IList<Advance> advanceList)
        public void DeleteExpenseMPAFromTransaction(Guid txID, long MPADocumentID)
        {
            ExpenseDataSet expenseDS = (ExpenseDataSet)TransactionService.GetDS(txID);
            ExpenseDataSet.FnExpenseMPARow[] rows = (ExpenseDataSet.FnExpenseMPARow[])expenseDS.FnExpenseMPA.Select();

            foreach (ExpenseDataSet.FnExpenseMPARow row in rows)
            {
                if (row.MPADocumentID == MPADocumentID)
                {
                    row.Delete();
                }
            }

        }

        #region public double AddExpenseCAToTransaction(Guid txID, long expID, IList<ExpensesCA> advanceList)
        public void AddExpenseCAToTransaction(Guid txID, long expID, IList<ExpenseCA> ExpenseCAList)
        {
            ExpenseDataSet expenseDS = (ExpenseDataSet)TransactionService.GetDS(txID);
            ExpenseDataSet.FnExpenseDocumentRow expenseDocumentRow = (ExpenseDataSet.FnExpenseDocumentRow)expenseDS.FnExpenseDocument.FindByExpenseID(expID);
            //string expenseType = this.GetExpenseType(expID, txID);
            foreach (ExpenseCA ExpenseCA in ExpenseCAList)
            {
                string str = "CADocumentID = {0}";
                ExpenseDataSet.FnExpenseCARow[] dr = (ExpenseDataSet.FnExpenseCARow[])expenseDS.FnExpenseCA.Select(string.Format(str, ExpenseCA.CADocumentID));
                if (dr.Length == 0)
                {
                    ExpenseDataSet.FnExpenseCARow expenseExpenseCARow = expenseDS.FnExpenseCA.NewFnExpenseCARow();
                    expenseExpenseCARow.CADocumentID = (long)ExpenseCA.CADocumentID;
                    expenseExpenseCARow.ExpenseID = expID;
                    expenseExpenseCARow.Active = true;
                    expenseExpenseCARow.CreBy = UserAccount.UserID;
                    expenseExpenseCARow.CreDate = DateTime.Now;
                    expenseExpenseCARow.UpdBy = UserAccount.UserID;
                    expenseExpenseCARow.UpdDate = DateTime.Now;
                    expenseExpenseCARow.UpdPgm = UserAccount.CurrentProgramCode;
                    expenseDS.FnExpenseCA.AddFnExpenseCARow(expenseExpenseCARow);
                }
            }
        }
        #endregion public double AddExpenseAdvanceToTransaction(Guid txID, long expID, IList<Advance> advanceList)

        public void DeleteExpenseCAFromTransaction(Guid txID, long CADocumentID)
        {
            ExpenseDataSet expenseDS = (ExpenseDataSet)TransactionService.GetDS(txID);
            ExpenseDataSet.FnExpenseCARow[] rows = (ExpenseDataSet.FnExpenseCARow[])expenseDS.FnExpenseCA.Select();

            foreach (ExpenseDataSet.FnExpenseCARow row in rows)
            {
                if (row.CADocumentID == CADocumentID)
                {
                    row.Delete();
                }
            }

        }
    }
    #region ExpenseFieldGroup Enum
    public enum ExpenseFieldGroup
    {
        CostCenter,
        AccountCode,
        InternalOrder,
        Subject,
        ReferenceNo,
        VAT,
        WHTax,
        Other,
        InvoiceVerifier,
        VerifyDetail,
        Initiator,
        Company,
        BoxID,
        RemittantDetail,
        BuActor,
        CounterCashier,
        PerdiemRate,
        ExpenseRemittanceDetail,
        LocalCurrency,
        Attachment
    }
    #endregion
}
