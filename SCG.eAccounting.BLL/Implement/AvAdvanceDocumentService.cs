using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;
using SS.Standard.Utilities;
using SS.Standard.Security;
using SS.Standard.WorkFlow.Service;

using SCG.eAccounting.BLL;
using SCG.eAccounting.DAL;
using SCG.eAccounting.DTO;
using SCG.eAccounting.Query;
using SCG.eAccounting.DTO.DataSet;
using SS.Standard.WorkFlow.Query;
using Antlr.StringTemplate;
using SS.Standard.WorkFlow.DTO;

using SCG.eAccounting.DTO.ValueObject;
using SCG.DB.Query;
using SCG.DB.DTO.ValueObject;
using NHibernate;
using SS.DB.Query;
using SS.Standard.CommunicationService.Implement;
using SCG.DB.DTO;
using SCG.DB.BLL;

namespace SCG.eAccounting.BLL.Implement
{
    public class AvAdvanceDocumentService : ServiceBase<AvAdvanceDocument, long>, IAvAdvanceDocumentService
    {
        public IUserAccount UserAccount { get; set; }
        public ITransactionService TransactionService { get; set; }
        public IWorkFlowService WorkFlowService { get; set; }
        public ISCGDocumentService SCGDocumentService { get; set; }
        public IAvAdvanceItemService AvAdvanceItemService { get; set; }
        public ISCGEmailService SCGEmailService { get; set; }
        public IDbHolidayProfileService DbHolidayProfileService { get; set; }
        #region Override Method
        public override IDao<AvAdvanceDocument, long> GetBaseDao()
        {
            return ScgeAccountingDaoProvider.AvAdvanceDocumentDao;
        }
        #endregion

        public DataSet PrepareDS()
        {
            AdvanceDataSet ds = new AdvanceDataSet();
            return ds.Clone();

        }
        public DataSet PrepareDS(long documentId)
        {
            AdvanceDataSet advanceDs = (AdvanceDataSet)this.PrepareDataToDs(documentId);
            advanceDs.AcceptChanges();
            return advanceDs;
        }


        public AdvanceDataSet PrepareDataToDs(long documentId)
        {
            AdvanceDataSet advanceDs = new AdvanceDataSet();
            //ดึงจาก base มาใส่ใน DataSet
            AvAdvanceDocument avDb = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.GetAvAdvanceByDocumentID(documentId);
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (avDb == null)
            {
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("NoAvAdvanceFound"));
            }
            if (!errors.IsEmpty)
            {
                throw new ServiceValidationException(errors);
            }
            // Prepare Data to Document datatable.
            SCGDocumentService.PrepareDataToDataset(advanceDs, documentId);

            // Set data to Advance Document row in advanceDs.
            AdvanceDataSet.AvAdvanceDocumentRow avRow = advanceDs.AvAdvanceDocument.NewAvAdvanceDocumentRow();
            avRow.AdvanceID = avDb.AdvanceID;
            avRow.DocumentID = avDb.DocumentID.DocumentID;
            if (avDb.TADocumentID.HasValue)
                avRow.TADocumentID = avDb.TADocumentID.Value;
            else
                avRow.SetTADocumentIDNull();
            avRow.AdvanceType = avDb.AdvanceType;
            if (avDb.PBID != null && avDb.PBID.Pbid > 0)
                avRow.PBID = avDb.PBID.Pbid;
            else
                avRow.SetPBIDNull();
            if (avDb.ServiceTeamID != null && avDb.ServiceTeamID.ServiceTeamID > 0)
                avRow.ServiceTeamID = avDb.ServiceTeamID.ServiceTeamID;
            else
                avRow.SetServiceTeamIDNull();
            avRow.RequestDateOfAdvance = avDb.RequestDateOfAdvance;
            avRow.DueDateOfRemittance = avDb.DueDateOfRemittance;
            avRow.RequestDateOfRemittance = avDb.RequestDateOfRemittance;
            if (avDb.AdvanceType == ZoneType.Foreign)
                avRow.ArrivalDate = avDb.ArrivalDate.Value;
            avRow.Reason = avDb.Reason;
            avRow.PerDiemExRateUSD = (decimal)avDb.PerDiemExRateUSD;
            avRow.Amount = (decimal)avDb.Amount;
            avRow.RemittanceAmount = (decimal)avDb.RemittanceAmount;
            avRow.ExpenseAmount = (decimal)avDb.ExpenseAmount;
            avRow.Active = avDb.Active;
            avRow.CreBy = UserAccount.UserID;
            avRow.CreDate = DateTime.Now;
            avRow.UpdBy = UserAccount.UserID;
            avRow.UpdDate = DateTime.Now;
            avRow.UpdPgm = UserAccount.CurrentProgramCode;

            if (avDb.MainCurrencyID.HasValue && avDb.MainCurrencyID.Value > 0)
            {
                avRow.MainCurrencyID = avDb.MainCurrencyID.Value;
            }
            else
            {
                avRow.SetMainCurrencyIDNull();
            }

            if (avDb.MainCurrencyAmount.HasValue)
            {
                avRow.MainCurrencyAmount = (decimal)avDb.MainCurrencyAmount.Value;
            }
            else
            {
                avRow.SetMainCurrencyAmountNull();
            }

            if (avDb.ExchangeRateForLocalCurrency.HasValue)
            {
                avRow.ExchangeRateForLocalCurrency = (decimal)avDb.ExchangeRateForLocalCurrency.Value;
            }
            else
            {
                avRow.SetExchangeRateForLocalCurrencyNull();
            }

            if (avDb.ExchangeRateMainToTHBCurrency.HasValue)
            {
                avRow.ExchangeRateMainToTHBCurrency = (decimal)avDb.ExchangeRateMainToTHBCurrency.Value;
            }
            else
            {
                avRow.SetExchangeRateMainToTHBCurrencyNull();
            }

            if (avDb.LocalCurrencyAmount.HasValue)
            {
                avRow.LocalCurrencyAmount = (decimal)avDb.LocalCurrencyAmount.Value;
            }
            else
            {
                avRow.SetLocalCurrencyAmountNull();
            }

            if (avDb.IsRepOffice.HasValue)
            {
                avRow.IsRepOffice = avDb.IsRepOffice.Value;
            }
            else
            {
                avRow.SetIsRepOfficeNull();
            }


            if (avDb.RequestDateOfAdvanceApproved.HasValue)
            {
                avRow.RequestDateOfAdvanceApproved = avDb.RequestDateOfAdvanceApproved.Value;
            }
            else
            {
                avRow.SetRequestDateOfAdvanceApprovedNull();
            }

            advanceDs.AvAdvanceDocument.AddAvAdvanceDocumentRow(avRow);

            // Prepare Data to AvAdvanceItem Datatable.
            AvAdvanceItemService.PrepareDataToDataset(advanceDs, avRow.AdvanceID, false);

            advanceDs.AcceptChanges();
            return advanceDs;
        }
        public DataSet PrepareDSTA(long taDocumentID)
        {
            AdvanceDataSet advanceDs = (AdvanceDataSet)this.PrepareDataToDsTA(taDocumentID);
            return advanceDs;
        }
        public AdvanceDataSet PrepareDataToDsTA(long taDocumentID)
        {
            AdvanceDataSet advanceDs = new AdvanceDataSet();
            //ดึงจาก base มาใส่ใน DataSet
            TADocument taDoc = ScgeAccountingQueryProvider.TADocumentQuery.FindProxyByIdentity(taDocumentID);
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (taDoc == null)
            {
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("NoTADocumentFound"));
            }
            if (!errors.IsEmpty)
            {
                throw new ServiceValidationException(errors);
            }
            // Prepare Data to Document datatable.

            AdvanceDataSet.DocumentRow docRow = advanceDs.Document.NewDocumentRow();
            docRow.ApproverID = taDoc.DocumentID.ApproverID.Userid;
            docRow.RequesterID = taDoc.DocumentID.RequesterID.Userid;
            docRow.RequesterID = taDoc.DocumentID.RequesterID.Userid;
            docRow.CreatorID = taDoc.DocumentID.CreatorID.Userid;
            docRow.CompanyID = taDoc.DocumentID.CompanyID.CompanyID;
            advanceDs.Document.AddDocumentRow(docRow);


            IList<DocumentInitiator> documentInitiatorList = new List<DocumentInitiator>();
            documentInitiatorList = ScgeAccountingQueryProvider.DocumentInitiatorQuery.GetDocumentInitiatorByDocumentID(taDoc.DocumentID.DocumentID);
            foreach (DocumentInitiator initiator in documentInitiatorList)
            {
                // Set data to document initiator datatable.
                AdvanceDataSet.DocumentInitiatorRow docInitial = advanceDs.DocumentInitiator.NewDocumentInitiatorRow();
                //docInitial.ini = initiator.InitiatorID;
                //if (initiator.DocumentID != null)
                //{
                //    initiatorRow["DocumentID"] = initiator.DocumentID.DocumentID;
                //}
                docInitial.InitiatorType = initiator.InitiatorType;
                docInitial.Seq = initiator.Seq;
                if (initiator.UserID != null)
                {
                    docInitial.UserID = initiator.UserID.Userid;
                }
                docInitial.DoApprove = initiator.DoApprove;
                docInitial.Active = initiator.Active;
                docInitial.CreBy = initiator.CreBy;
                docInitial.CreDate = initiator.CreDate;
                docInitial.UpdBy = initiator.UpdBy;
                docInitial.UpdDate = initiator.UpdDate;
                docInitial.UpdPgm = initiator.UpdPgm;

                // Add document initiator to datatable budgetDocumentDS.
                advanceDs.DocumentInitiator.AddDocumentInitiatorRow(docInitial);
            }
            advanceDs.DocumentAttachment.NewDocumentAttachmentRow();

            // Set data to Advance Document row in advanceDs.
            AdvanceDataSet.AvAdvanceDocumentRow avRow = advanceDs.AvAdvanceDocument.NewAvAdvanceDocumentRow();
            //avRow.AdvanceID = -1;
            avRow.DocumentID = Convert.ToInt64(advanceDs.Document.Rows[0]["DocumentID"].ToString());
            avRow.TADocumentID = taDoc.TADocumentID;
            //avRow.AdvanceType = taDoc.;
            //if (avDb.PBID != null && avDb.PBID.Pbid > 0)
            //    avRow.PBID = avDb.PBID.Pbid;
            //else
            //    avRow.SetPBIDNull();
            //avRow.ServiceTeamID = avDb.ServiceTeamID.ServiceTeamID;
            if (taDoc.TravelBy.Equals(TravellBy.Foreign))
            {
                avRow.ArrivalDate = taDoc.ToDate;
                avRow.AdvanceType = ZoneType.Foreign;

            }
            else
                avRow.AdvanceType = ZoneType.Domestic;
            //avRow.RequestDateOfAdvance = avDb.RequestDateOfAdvance;
            //avRow.DueDateOfRemittance = avDb.DueDateOfRemittance;
            //avRow.RequestDateOfRemittance = avDb.RequestDateOfRemittance;
            //if (avDb.AdvanceType == ZoneType.Foreign)
            //    avRow.ArrivalDate = avDb.ArrivalDate.Value;
            //avRow.Reason = avDb.Reason;
            //avRow.PerDiemExRateUSD = (decimal)avDb.PerDiemExRateUSD;
            //avRow.Amount = (decimal)avDb.Amount;
            //avRow.RemittanceAmount = (decimal)avDb.RemittanceAmount;
            //avRow.ExpenseAmount = (decimal)avDb.ExpenseAmount;
            avRow.Active = true;
            avRow.CreBy = UserAccount.UserID;
            avRow.CreDate = DateTime.Now;
            avRow.UpdBy = UserAccount.UserID;
            avRow.UpdDate = DateTime.Now;
            avRow.UpdPgm = UserAccount.CurrentProgramCode;
            advanceDs.AvAdvanceDocument.AddAvAdvanceDocumentRow(avRow);

            // Prepare Data to AvAdvanceItem Datatable.
            AvAdvanceItemService.PrepareDataToDataset(advanceDs, avRow.AdvanceID, false);

            //advanceDs.AcceptChanges();
            return advanceDs;
        }

        public DataSet PrepareInternalDataToDataset(long documentId, bool isCopy)
        {
            AdvanceDataSet advanceDs = new AdvanceDataSet();
            //ดึงจาก base มาใส่ใน DataSet
            AvAdvanceDocument avDb = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.GetAvAdvanceByDocumentID(documentId);
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (avDb == null)
            {
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("NoAvAdvanceFound"));
            }
            if (!errors.IsEmpty)
            {
                throw new ServiceValidationException(errors);
            }
            // Prepare Data to Document datatable.
            if (!isCopy)
            {
                SCGDocumentService.PrepareDataToDataset(advanceDs, documentId);
            }
            else
            {
                SCGDocumentService.PrepareDataInternalToDataset(advanceDs, documentId, isCopy);
            }

            // Set data to Advance Document row in advanceDs.
            AdvanceDataSet.AvAdvanceDocumentRow avRow = advanceDs.AvAdvanceDocument.NewAvAdvanceDocumentRow();
            avRow.AdvanceID = avDb.AdvanceID;

            if (!isCopy)
            {
                avRow.DocumentID = avDb.DocumentID.DocumentID;
                if (avDb.TADocumentID.HasValue)
                    avRow.TADocumentID = avDb.TADocumentID.Value;
                else
                    avRow.SetTADocumentIDNull();
                avRow.PerDiemExRateUSD = (decimal)avDb.PerDiemExRateUSD;
                avRow.RemittanceAmount = (decimal)avDb.RemittanceAmount;
                avRow.ExpenseAmount = (decimal)avDb.ExpenseAmount;

                avRow.Amount = (decimal)avDb.Amount;

                if (avDb.MainCurrencyAmount.HasValue)
                {
                    avRow.MainCurrencyAmount = (decimal)avDb.MainCurrencyAmount.Value;
                }
                else
                {
                    avRow.SetMainCurrencyAmountNull();
                }

                if (avDb.ExchangeRateForLocalCurrency.HasValue)
                {
                    avRow.ExchangeRateForLocalCurrency = (decimal)avDb.ExchangeRateForLocalCurrency.Value;
                }
                else
                {
                    avRow.SetExchangeRateForLocalCurrencyNull();
                }

                if (avDb.ExchangeRateMainToTHBCurrency.HasValue)
                {
                    avRow.ExchangeRateMainToTHBCurrency = (decimal)avDb.ExchangeRateMainToTHBCurrency.Value;
                }
                else
                {
                    avRow.SetExchangeRateMainToTHBCurrencyNull();
                }


                if (avDb.LocalCurrencyAmount.HasValue)
                {
                    avRow.LocalCurrencyAmount = (decimal)avDb.LocalCurrencyAmount.Value;
                }
                else
                {
                    avRow.SetLocalCurrencyAmountNull();
                }

            }
            else
            {
                if (advanceDs.Document.Rows.Count > 0)
                {
                    AdvanceDataSet.DocumentRow docRow = (AdvanceDataSet.DocumentRow)advanceDs.Document.Rows[0];
                    avRow.DocumentID = docRow.DocumentID;
                }
            }

            avRow.AdvanceType = avDb.AdvanceType;
            avRow.Amount = (decimal)avDb.Amount;
            if (avDb.PBID != null && avDb.PBID.Pbid > 0)
                avRow.PBID = avDb.PBID.Pbid;
            else
                avRow.SetPBIDNull();
            if (avDb.ServiceTeamID != null && avDb.ServiceTeamID.ServiceTeamID > 0)
                avRow.ServiceTeamID = avDb.ServiceTeamID.ServiceTeamID;
            else
                avRow.SetServiceTeamIDNull();
            avRow.RequestDateOfAdvance = avDb.RequestDateOfAdvance;
            avRow.DueDateOfRemittance = avDb.DueDateOfRemittance;
            avRow.RequestDateOfRemittance = avDb.RequestDateOfRemittance;
            if (avDb.AdvanceType == ZoneType.Foreign)
                avRow.ArrivalDate = avDb.ArrivalDate.Value;
            avRow.Reason = avDb.Reason;
            

            if (avDb.MainCurrencyID.HasValue && avDb.MainCurrencyID.Value > 0)
            {
                avRow.MainCurrencyID = avDb.MainCurrencyID.Value;
            }
            else
            {
                avRow.SetMainCurrencyIDNull();
            }

            

            if (avDb.IsRepOffice.HasValue)
            {
                avRow.IsRepOffice = avDb.IsRepOffice.Value;
            }
            else
            {
                avRow.SetIsRepOfficeNull();
            }

            avRow.Active = avDb.Active;
            avRow.CreBy = UserAccount.UserID;
            avRow.CreDate = DateTime.Now;
            avRow.UpdBy = UserAccount.UserID;
            avRow.UpdDate = DateTime.Now;
            avRow.UpdPgm = UserAccount.CurrentProgramCode;
            advanceDs.AvAdvanceDocument.AddAvAdvanceDocumentRow(avRow);

            //// Prepare Data to FnExpenseAdvance Datatable.
            AvAdvanceItemService.PrepareDataToDataset(advanceDs, avRow.AdvanceID, isCopy);

            //if (avRow.IsAmountNull())
            //{

            //}

            return advanceDs;
        }


        public long AddAdvanceDocument(Guid txid, string advanceType)
        {
            AdvanceDataSet ds = (AdvanceDataSet)TransactionService.GetDS(txid);
            AdvanceDataSet.DocumentRow docRow = ds.Document.NewDocumentRow();//create newRow to dataset
            ds.Document.AddDocumentRow(docRow);

            AdvanceDataSet.AvAdvanceDocumentRow avRow = ds.AvAdvanceDocument.NewAvAdvanceDocumentRow();
            avRow.DocumentID = docRow.DocumentID;
            avRow.AdvanceType = advanceType;
            ds.AvAdvanceDocument.AddAvAdvanceDocumentRow(avRow);
            return avRow.AdvanceID;

        }

        public void UpdateAvDocumentTransaction(AvAdvanceDocument avAdvanceDoc, Guid txid)
        {
            bool isRepOffice = false;
            IList<object> advanceEditableField = new List<object>();
            string mode = string.Empty;
            if (UserAccount.CurrentProgramCode == "AdvanceForeignForm")
            {
                mode = "FR";
            }
            else if (UserAccount.CurrentProgramCode == "AdvanceDomesticForm")
            {
                mode = "DM";
            }
            if (avAdvanceDoc.DocumentID != null)
            {
                advanceEditableField = GetEditableFields(avAdvanceDoc.DocumentID.DocumentID, null);
            }
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            AdvanceDataSet ds = (AdvanceDataSet)TransactionService.GetDS(txid);
            AdvanceDataSet.AvAdvanceDocumentRow rowAdvance = (AdvanceDataSet.AvAdvanceDocumentRow)ds.AvAdvanceDocument.FindByAdvanceID(avAdvanceDoc.AdvanceID);
            SS.Standard.WorkFlow.DTO.WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(avAdvanceDoc.DocumentID.DocumentID);

            AdvanceDataSet.DocumentRow rowDocument = (AdvanceDataSet.DocumentRow)ds.Document.FindByDocumentID(rowAdvance.DocumentID);
            if (!rowAdvance.IsIsRepOfficeNull())
            {
                isRepOffice = rowAdvance.IsRepOffice;
            }
            //IList<AvAdvanceItem> advanceItem = ScgeAccountingQueryProvider.AvAdvanceItemQuery.FindByAvAdvanceItemAdvanceID(rowAdvance.AdvanceID);
            //ds.AvAdvanceItem.FindByAdvanceID(rowAdvance.AdvanceID);
            DataRow[] drItem = ds.AvAdvanceItem.Select(string.Format("AdvanceID={0}", rowAdvance.AdvanceID));
            rowAdvance.BeginEdit();
            // Validate Amount.
            if (avAdvanceDoc.AdvanceType == ZoneType.Domestic)
            {
                Dbpb pb = ScgDbQueryProvider.DbPBQuery.FindByIdentity(avAdvanceDoc.PBID == null ? 0 : avAdvanceDoc.PBID.Pbid);

                if (!isRepOffice)
                {
                    if (avAdvanceDoc.Amount == 0)
                    {
                        errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("AmountDomesticIsRequired"));
                    }
                    //เกินวงเงินของ counter cashier
                    if (avAdvanceDoc.PaymentType == PaymentType.CA)
                    {
                        if (pb != null && avAdvanceDoc.Amount > pb.PettyCashLimit && pb.PettyCashLimit != 0)
                        {
                            errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("AmountDomesticIsOverPettyCashLimit"));
                        }
                    }
                }
                else
                {
                    if (avAdvanceDoc.LocalCurrencyAmount == 0)
                    {
                        errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("AmountIsRequired"));
                        
                    }

                    if (avAdvanceDoc.PaymentType == PaymentType.CA)
                    {
                        if (avAdvanceDoc.MainCurrencyAmount > pb.PettyCashLimit && pb.PettyCashLimit != 0)
                        {
                            errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("AmountDomesticIsOverPettyCashLimit"));
                        }
                    }
                }
                //ถ้าเป็นไม่เป้นเงินโอน ต้องใส่ค่า dbpb
                if (drItem.Length > 0)
                {
                    if (!drItem[0]["PaymentType"].ToString().Equals(PaymentType.TR))
                    {
                        if (avAdvanceDoc.PBID == null || avAdvanceDoc.PBID.Pbid == 0)
                        {
                            errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("CounterCashierIsRequired"));
                        }
                    }
                }
                // Validate Service Team
                if (avAdvanceDoc.ServiceTeamID.ServiceTeamID == 0)
                {
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("ServiceTeamIsRequired"));
                }
            }
            else
            {
                if (avAdvanceDoc.ArrivalDate == null)
                {
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("ArrivalDateIsRequired"));
                }
                if (avAdvanceDoc.ArrivalDate.Equals(DateTime.MinValue))
                {
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("ArrivalDateIsMoreThanRequestDateofAdvance"));
                }
                // Validate Counter Cashier
                if (avAdvanceDoc.PBID == null || avAdvanceDoc.PBID.Pbid == 0)
                {
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("CounterCashierFNIsRequired"));
                }
                if ((workFlow != null && workFlow.CurrentState.Name == WorkFlowStateFlag.WaitVerify) && advanceEditableField.Contains(AdvanceFieldGroup.ExchangeRateForPerDiemCalculation) && avAdvanceDoc.PerDiemExRateUSD < 0)
                {
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("ExchangeRateforPerDiemCalculationIsRequired"));
                }
            }

            // Validate Request date of advance
            if (avAdvanceDoc.RequestDateOfAdvance.Equals(DateTime.MinValue))
            {
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("RequestDateofAdvanceIsRequired"));
            }
            else if ((workFlow != null && workFlow.CurrentState.Name == WorkFlowStateFlag.WaitVerify) && (!string.IsNullOrEmpty(rowAdvance["RequestDateOfAdvanceApproved"].ToString()) && avAdvanceDoc.RequestDateOfAdvance < rowAdvance.RequestDateOfAdvanceApproved))
            {
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("RequestDateofAdvanceMustMoreOrEqualThanApproved"));
            }
            else if (!avAdvanceDoc.RequestDateOfAdvance.Equals(DateTime.MinValue) && !avAdvanceDoc.RequestDateOfAdvance.Equals(DateTime.MaxValue))
            {
                if (DbHolidayProfileService.CheckDay(avAdvanceDoc.RequestDateOfAdvance,mode))
                {
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("SystemNotAllowRequestDateOfAdvanceOnHoliday"));
                }
            }
            // Validate Request date of advance

            if (avAdvanceDoc.DocumentID.DocumentID == -1 || workFlow.CurrentState.Name.Equals(WorkFlowStateFlag.Draft))
            {
                if (avAdvanceDoc.RequestDateOfAdvance.Date.CompareTo(DateTime.Today) < 0)
                {
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("RequestDateofAdvanceMustBeLaterThanToday"));
                }
            }

            // Validate Request date of Remittance
            if (avAdvanceDoc.RequestDateOfRemittance.Equals(DateTime.MinValue))
            {
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("RequestDateofRemittanceIsRequired"));
            }
            // Validate Reason
            if (avAdvanceDoc.RequestDateOfRemittance.CompareTo(avAdvanceDoc.DueDateOfRemittance) > 0)
            {
                if (string.IsNullOrEmpty(avAdvanceDoc.Reason))
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("ReasonIsRequired"));
            }
            // Validate request date of remittance ; check request date between advance date and advance date + x Day
            // use maxValue ในการ check ถ้าส่งมาเป้น max value แปลว่า วันที่เลือกมาอยู่นอกช่วงที่กำหนด
            if (avAdvanceDoc.RequestDateOfRemittance.Equals(DateTime.MaxValue))
            {
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("RequestDateofRemittanceIsNotIntime"));
            }
            else if (!avAdvanceDoc.RequestDateOfRemittance.Equals(DateTime.MaxValue) && !avAdvanceDoc.RequestDateOfRemittance.Equals(DateTime.MinValue))
            {
                if (DbHolidayProfileService.CheckDay(avAdvanceDoc.RequestDateOfRemittance, mode))
                {
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("SystemNotAllowRequestDateOfRemittanceOnHoliday"));
                }
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            rowAdvance.DocumentID = avAdvanceDoc.DocumentID.DocumentID;
            if (avAdvanceDoc.TADocumentID.HasValue)
                rowAdvance.TADocumentID = avAdvanceDoc.TADocumentID.Value;
            else
                rowAdvance.SetTADocumentIDNull();
            if (avAdvanceDoc.PBID != null)
                rowAdvance.PBID = avAdvanceDoc.PBID.Pbid;
            else
                rowAdvance.SetPBIDNull();
            rowAdvance.AdvanceType = avAdvanceDoc.AdvanceType;
            if (avAdvanceDoc.ServiceTeamID != null)
                rowAdvance.ServiceTeamID = avAdvanceDoc.ServiceTeamID.ServiceTeamID;
            else
                rowAdvance.SetServiceTeamIDNull();
            rowAdvance.RequestDateOfAdvance = avAdvanceDoc.RequestDateOfAdvance;
            rowAdvance.DueDateOfRemittance = avAdvanceDoc.DueDateOfRemittance;
            rowAdvance.RequestDateOfRemittance = avAdvanceDoc.RequestDateOfRemittance;
            if (avAdvanceDoc.ArrivalDate != null)
            {
                rowAdvance.ArrivalDate = avAdvanceDoc.ArrivalDate.Value;
            }
            else
            {
                rowAdvance.ArrivalDate = DateTime.MinValue;
            }
            rowAdvance.Reason = avAdvanceDoc.Reason;

            if (workFlow == null || workFlow.CurrentState.Name == WorkFlowStateFlag.Draft)
            {
                rowAdvance.PerDiemExRateUSD = 0;
            }
            else
            {
                rowAdvance.PerDiemExRateUSD = Math.Round((decimal)avAdvanceDoc.PerDiemExRateUSD, 5, MidpointRounding.AwayFromZero);
            }
            rowAdvance.Amount = Math.Round((decimal)avAdvanceDoc.Amount, 2, MidpointRounding.AwayFromZero);
            rowAdvance.RemittanceAmount = Math.Round((decimal)avAdvanceDoc.RemittanceAmount, 2, MidpointRounding.AwayFromZero);
            rowAdvance.ExpenseAmount = Math.Round((decimal)avAdvanceDoc.ExpenseAmount, 2, MidpointRounding.AwayFromZero);
            rowAdvance.Active = avAdvanceDoc.Active;
            rowAdvance.CreBy = UserAccount.UserID;
            rowAdvance.CreDate = DateTime.Now;
            rowAdvance.UpdBy = UserAccount.UserID;
            rowAdvance.UpdDate = DateTime.Now;
            rowAdvance.UpdPgm = UserAccount.CurrentProgramCode;

            if (avAdvanceDoc.IsRepOffice.HasValue)
            {
                rowAdvance.IsRepOffice = avAdvanceDoc.IsRepOffice.Value;
            }

            if (avAdvanceDoc.MainCurrencyID.HasValue && avAdvanceDoc.MainCurrencyID.Value > 0)
            {
                rowAdvance.MainCurrencyID = avAdvanceDoc.MainCurrencyID.Value;
            }
            else
            {
                rowAdvance.SetMainCurrencyIDNull();
            }

            if (avAdvanceDoc.MainCurrencyAmount.HasValue)
            {
                rowAdvance.MainCurrencyAmount = (decimal)avAdvanceDoc.MainCurrencyAmount.Value;
            }
            else
            {
                rowAdvance.SetMainCurrencyAmountNull();
            }

            if (avAdvanceDoc.ExchangeRateForLocalCurrency.HasValue)
            {
                rowAdvance.ExchangeRateForLocalCurrency = (decimal)avAdvanceDoc.ExchangeRateForLocalCurrency.Value;
            }
            else
            {
                rowAdvance.SetExchangeRateForLocalCurrencyNull();
            }

            if (avAdvanceDoc.ExchangeRateMainToTHBCurrency.HasValue)
            {
                rowAdvance.ExchangeRateMainToTHBCurrency = (decimal)avAdvanceDoc.ExchangeRateMainToTHBCurrency.Value;
            }
            else
            {
                rowAdvance.SetExchangeRateMainToTHBCurrencyNull();
            }


            if (avAdvanceDoc.LocalCurrencyAmount.HasValue)
            {
                rowAdvance.LocalCurrencyAmount = (decimal)avAdvanceDoc.LocalCurrencyAmount.Value;
            }
            else
            {
                rowAdvance.SetLocalCurrencyAmountNull();
            }

            rowAdvance.EndEdit();

        }

        public void UpdateTAdocumentIDTransaction(long advanceID)
        {
            AvAdvanceDocument avDocument = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.FindByIdentity(advanceID);
            avDocument.TADocumentID = null;
            avDocument.UpdBy = UserAccount.UserID;
            avDocument.UpdDate = DateTime.Now;
            avDocument.UpdPgm = UserAccount.CurrentProgramCode;
            this.SaveOrUpdate(avDocument);
        }

        public long SaveAvAdvance(Guid txid, long advanceId)
        {
            AdvanceDataSet advanceDataset = (AdvanceDataSet)TransactionService.GetDS(txid);

            AdvanceDataSet.AvAdvanceDocumentRow row = advanceDataset.AvAdvanceDocument.FindByAdvanceID(advanceId);
            long documentID = row.DocumentID;
            documentID = SCGDocumentService.SaveSCGDocument(txid, documentID);
            //insert update or delete AvAdvanveDocument
            advanceId = ScgeAccountingDaoProvider.AvAdvanceDocumentDao.Persist(advanceDataset.AvAdvanceDocument);

            //inset update or delete AvAdvanceItem
            AvAdvanceItemService.saveAdvanceItem(txid, advanceId);

            return advanceId;
        }

        #region Method for update Remittance and Clearing amount to Advance document by Anuwat S on 22/04/2009
        public void UpdateRemittanceAdvance(long remittanceId, double totalRemittanceAmt, bool isCancel)
        {
            FnRemittance remittance = ScgeAccountingQueryProvider.FnRemittanceQuery.FindByIdentity(remittanceId);
            IList<AdvanceData> remitAdvList = ScgeAccountingQueryProvider.FnRemittanceAdvanceQuery.FindByRemittanceIDForUpdateRemittanceAdvance(remittanceId);
            AvAdvanceDocument advDto;
            // Use RequestDateOfAdvance to store document date
            foreach (AdvanceData remitAdvObj in remitAdvList.OrderBy(a => a.RequestDateOfAdvance).ToList())
            {
                advDto = FindByIdentity(remitAdvObj.AdvanceID);
                if (!isCancel)
                {
                    if (remittance.IsFullClearing)
                    {
                        advDto.RemittanceAmount = advDto.Amount;
                    }
                    else if (totalRemittanceAmt >= advDto.Amount)
                    {
                        // if remaining remittance amount more than advance amount,
                        // full remittance on that advance.
                        advDto.RemittanceAmount = advDto.Amount;
                        totalRemittanceAmt -= advDto.RemittanceAmount;
                    }
                    else
                    {
                        // Remaining remittance amount less than advance amount,
                        // remittance only remaining remittance amount
                        advDto.RemittanceAmount = totalRemittanceAmt;
                        totalRemittanceAmt = 0;
                    }
                }
                else
                {
                    advDto.RemittanceAmount = totalRemittanceAmt;
                }
                advDto.UpdBy = remitAdvObj.UpdBy;
                advDto.UpdDate = DateTime.Now;
                advDto.UpdPgm = remitAdvObj.ProgramCode;
                SaveOrUpdate(advDto);
            }
        }

        public void UpdateRemittanceAdvanceForRepOffice(long remittanceId, double totalRemittanceAmtMain, bool isCancel)
        {
            FnRemittance remittance = ScgeAccountingQueryProvider.FnRemittanceQuery.FindByIdentity(remittanceId);
            IList<AdvanceData> remitAdvList = ScgeAccountingQueryProvider.FnRemittanceAdvanceQuery.FindByRemittanceIDForUpdateRemittanceAdvance(remittanceId);
            AvAdvanceDocument advDto;
            // Use RequestDateOfAdvance to store document date
            foreach (AdvanceData remitAdvObj in remitAdvList.OrderBy(a => a.RequestDateOfAdvance).ToList())
            {
                advDto = FindByIdentity(remitAdvObj.AdvanceID);
                if (!isCancel)
                {
                    if (remittance.IsFullClearing)
                    {
                        advDto.RemittanceAmountMainCurrency = advDto.MainCurrencyAmount;
                    }
                    else if (totalRemittanceAmtMain >= advDto.MainCurrencyAmount)
                    {
                        // if remaining remittance amount more than advance amount,
                        // full remittance on that advance.
                        advDto.RemittanceAmountMainCurrency = advDto.MainCurrencyAmount;
                        totalRemittanceAmtMain -= Convert.ToDouble(advDto.RemittanceAmountMainCurrency);
                    }
                    else
                    {
                        // Remaining remittance amount less than advance amount,
                        // remittance only remaining remittance amount
                        advDto.RemittanceAmountMainCurrency = totalRemittanceAmtMain;
                        totalRemittanceAmtMain = 0;
                    }
                }
                else
                {
                    advDto.RemittanceAmountMainCurrency = totalRemittanceAmtMain;
                }
                advDto.UpdBy = remitAdvObj.UpdBy;
                advDto.UpdDate = DateTime.Now;
                advDto.UpdPgm = remitAdvObj.ProgramCode;
                SaveOrUpdate(advDto);
            }
        }

        public void UpdateClearingAdvance(long expenseId, double totalClearingAmt)
        {
            IList<AdvanceData> expAdvList = ScgeAccountingQueryProvider.FnExpenseAdvanceQuery.FindByExpenseDocumentIDForUpdateClearingAdvance(expenseId);
            AvAdvanceDocument advDto;
            foreach (AdvanceData expAdvObj in expAdvList.OrderBy(a => a.RequestDateOfAdvance).ToList())
            {
                advDto = FindByIdentity(expAdvObj.AdvanceID);
                if ((totalClearingAmt + advDto.RemittanceAmount) >= advDto.Amount)
                {
                    // if remaining clearing amount + remittance more than advance amount,
                    // full clearing on that advance.
                    advDto.ExpenseAmount = advDto.Amount - advDto.RemittanceAmount;
                    totalClearingAmt -= advDto.ExpenseAmount;
                }
                else
                {
                    // Remaining clearing amount less than advance amount,
                    // clearing only remaining clearing amount
                    advDto.ExpenseAmount = totalClearingAmt;
                    totalClearingAmt = 0;
                }
                advDto.UpdBy = expAdvObj.UpdBy;
                advDto.UpdDate = DateTime.Now;
                advDto.UpdPgm = expAdvObj.ProgramCode;
                SaveOrUpdate(advDto);
            }
        }

        public void UpdateClearingAdvanceForRepOffice(long expenseId, double totalClearingAmt)
        {
            IList<AdvanceData> expAdvList = ScgeAccountingQueryProvider.FnExpenseAdvanceQuery.FindByExpenseDocumentIDForUpdateClearingAdvance(expenseId);
            AvAdvanceDocument advDto;
            foreach (AdvanceData expAdvObj in expAdvList.OrderBy(a => a.RequestDateOfAdvance).ToList())
            {
                advDto = FindByIdentity(expAdvObj.AdvanceID);
                if ((totalClearingAmt + (advDto.RemittanceAmountMainCurrency.HasValue ? advDto.RemittanceAmountMainCurrency.Value : 0)) >= advDto.MainCurrencyAmount)
                {
                    // if remaining clearing amount + remittance more than advance amount,
                    // full clearing on that advance.
                    advDto.ExpenseAmountMainCurrency = advDto.MainCurrencyAmount - (advDto.RemittanceAmountMainCurrency.HasValue ? advDto.RemittanceAmountMainCurrency.Value : 0);
                    totalClearingAmt -= advDto.ExpenseAmountMainCurrency.HasValue ? advDto.ExpenseAmountMainCurrency.Value : 0;
                }
                else
                {
                    // Remaining clearing amount less than advance amount,
                    // clearing only remaining clearing amount
                    advDto.ExpenseAmountMainCurrency = totalClearingAmt;
                    totalClearingAmt = 0;
                }
                advDto.UpdBy = expAdvObj.UpdBy;
                advDto.UpdDate = DateTime.Now;
                advDto.UpdPgm = expAdvObj.ProgramCode;
                SaveOrUpdate(advDto);
            }
        }
        #endregion

        #region GetEditableFields and GetVisibleFields
        public IList<object> GetVisibleFields(long? documentID)
        {
            IList<object> visibleFields = new List<object>();

            if (!documentID.HasValue) // Check whether new flag then return the default editableFields.
            {
                visibleFields.Add(AdvanceFieldGroup.Subject);
                visibleFields.Add(AdvanceFieldGroup.PaymentType);
                visibleFields.Add(AdvanceFieldGroup.RequestDateOfAdvance);
                visibleFields.Add(AdvanceFieldGroup.RequestDateOfRemittance);
                visibleFields.Add(AdvanceFieldGroup.Reason);
                visibleFields.Add(AdvanceFieldGroup.Initiator);
                visibleFields.Add(AdvanceFieldGroup.Attachment);
                visibleFields.Add(AdvanceFieldGroup.Memo);
                //visibleFields.Add(AdvanceFieldGroup.ExchangeRateForPerDiemCalculation);
                visibleFields.Add(AdvanceFieldGroup.Other);
                visibleFields.Add(AdvanceFieldGroup.AdvanceReferTA);
                visibleFields.Add(AdvanceFieldGroup.Company);
                visibleFields.Add(AdvanceFieldGroup.ServiceTeam);
                visibleFields.Add(AdvanceFieldGroup.BuActor);
                visibleFields.Add(AdvanceFieldGroup.CounterCashier);
                visibleFields.Add(AdvanceFieldGroup.DomesticAmountTHB);
                visibleFields.Add(AdvanceFieldGroup.ArrivalDate);
                visibleFields.Add(AdvanceFieldGroup.CurrencyAmount);
                visibleFields.Add(AdvanceFieldGroup.PaymentTypeFR);
                visibleFields.Add(AdvanceFieldGroup.CurrencyRepOffice);
            }
            else // Check whether view or edit flag then return editableFields from workflow state.
            {
                SS.Standard.WorkFlow.DTO.WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(documentID.Value);
                visibleFields = WorkFlowService.GetVisibleFields(workFlow.WorkFlowID);
            }

            return visibleFields;
        }
        public IList<object> GetEditableFields(long? documentID, long? taDocumentID)
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
            editableFields.Add(AdvanceFieldGroup.Subject);
            editableFields.Add(AdvanceFieldGroup.PaymentType);
            editableFields.Add(AdvanceFieldGroup.RequestDateOfAdvance);
            editableFields.Add(AdvanceFieldGroup.RequestDateOfRemittance);
            editableFields.Add(AdvanceFieldGroup.Reason);
            editableFields.Add(AdvanceFieldGroup.Initiator);
            editableFields.Add(AdvanceFieldGroup.Attachment);
            editableFields.Add(AdvanceFieldGroup.Memo);
            editableFields.Add(AdvanceFieldGroup.ExchangeRateForPerDiemCalculation);
            editableFields.Add(AdvanceFieldGroup.Other);
            editableFields.Add(AdvanceFieldGroup.ServiceTeam);
            editableFields.Add(AdvanceFieldGroup.BuActor);
            editableFields.Add(AdvanceFieldGroup.CounterCashier);
            editableFields.Add(AdvanceFieldGroup.DomesticAmountTHB);
            editableFields.Add(AdvanceFieldGroup.ArrivalDate);
            editableFields.Add(AdvanceFieldGroup.CurrencyAmount);
            editableFields.Add(AdvanceFieldGroup.CurrencyRepOffice);

            if (taDocumentID == null)
            {
                editableFields.Add(AdvanceFieldGroup.AdvanceReferTA);
                editableFields.Add(AdvanceFieldGroup.Company);
            }
            return editableFields;
        }
        #endregion

        #region unused
        //public string GetAdvanceFromIDLatex(int advanceID)
        //{
        //    AvAdvanceDocument avd = this.FindByIdentity(advanceID);
        //    StringTemplateGroup group = new StringTemplateGroup("group", AppDomain.CurrentDomain.BaseDirectory + "Resources\\Template");
        //    StringTemplate xml = group.GetInstanceOf("AdvanceForm");
        //    IList<AvAdvanceItem> avAdvanceItem = ScgeAccountingQueryProvider.AvAdvanceItemQuery.FindByAvAdvanceItemAdvanceID(advanceID);
        //    string paymentType = ScgDbQueryProvider.SCGDbStatusLangQuery.GetStatusLang("PaymentTypeDMT", 1, avAdvanceItem[0].PaymentType);

        //    string thaiAmount = SS.Standard.Utilities.Utilities.NumericToThai(avd.Amount);
        //    AdvanceFormDbPBData pbData = ScgDbQueryProvider.DbPBQuery.GetDescription(avd.PBID.Pbid, 1);
        //    xml.SetAttribute("companyCode", avd.DocumentID.CompanyID.CompanyCode);
        //    xml.SetAttribute("companyName", avd.DocumentID.CompanyID.CompanyName);
        //    xml.SetAttribute("documentNo", avd.DocumentID.DocumentNo);
        //    //xml.SetAttribute("refDoc", );
        //    xml.SetAttribute("employeeName", avd.DocumentID.RequesterID.EmployeeName);
        //    xml.SetAttribute("employeeCode", avd.DocumentID.RequesterID.EmployeeCode);
        //    xml.SetAttribute("section", avd.DocumentID.RequesterID.SectionName);
        //    xml.SetAttribute("date", avd.DocumentID.DocumentDate);
        //    xml.SetAttribute("receiverName", avd.DocumentID.ReceiverID.EmployeeName);
        //    xml.SetAttribute("receiverCode", avd.DocumentID.ReceiverID.EmployeeCode);
        //    xml.SetAttribute("dateAdvance", avd.RequestDateOfAdvance);
        //    xml.SetAttribute("dateRemittance", avd.DueDateOfRemittance);
        //    xml.SetAttribute("typeAdvance1", paymentType);
        //    xml.SetAttribute("typeAdvance2", pbData.PBCode);
        //    xml.SetAttribute("typeAdvance3", pbData.Description);
        //    xml.SetAttribute("subject", avd.DocumentID.Subject);
        //    xml.SetAttribute("amountTHB", avd.Amount.ToString("#,#.00"));
        //    xml.SetAttribute("numToString", thaiAmount);
        //    xml.SetAttribute("requestDateOfRemittance", avd.RequestDateOfRemittance);
        //    xml.SetAttribute("reason", avd.Reason);
        //    //xml.SetAttribute("treasury", );
        //    //xml.SetAttribute("receiverBy", );
        //    xml.SetAttribute("issueBy", avd.DocumentID.RequesterID.EmployeeName);
        //    xml.SetAttribute("approveBy", avd.DocumentID.ApproverID.EmployeeName);
        //    //xml.SetAttribute("verifyDoc", );
        //    //xml.SetAttribute("approveVerify", );

        //    return xml.ToString();
        //}
        #endregion

        #region public void DeleteAvAdvanceDocumentByAdvanceID(long advanceID)
        public void DeleteAvAdvanceDocumentByAdvanceID(long advanceID)
        {
            AvAdvanceDocument advance = this.FindProxyByIdentity(advanceID);
            this.Delete(advance);
        }
        #endregion public void DeleteAvAdvanceDocumentByAdvanceID(long advanceID)

        #region IAvAdvanceDocumentService Members


        public void SendEmailToOverDueDate()
        {
            IList<VOAdvanceOverDueReport> voList = ScgeAccountingQueryProvider.SCGDocumentQuery.GetAdvanceOverdueList();

            foreach (VOAdvanceOverDueReport item in voList)
            {
                SCGDocument document = SCGDocumentService.FindByIdentity(item.DocumentID);
                SCGEmailService.SendEmailEM10(item.AdvanceID, document.RequesterID.Userid, document.ApproverID.Email, "This is e-mail for autoreminder advancedocument system.", true);
            }
        }

        #endregion


        // User Update AvadvanceDocument by advance DTO
        public void UpdateAvAdvanceDocument(AvAdvanceDocument advance)
        {
            ScgeAccountingDaoProvider.AvAdvanceDocumentDao.Update(advance);
        }
    }

    #region AdvanceFieldGroup Enum
    public enum AdvanceFieldGroup
    {
        Subject,
        PaymentType,
        RequestDateOfAdvance,
        RequestDateOfRemittance,
        Reason,
        Attachment,
        Memo,
        ExchangeRateForPerDiemCalculation,
        Other,
        AdvanceReferTA,
        VerifyDetail,
        Initiator,
        ServiceTeam,
        Company,
        BuActor,
        CounterCashier,
        DomesticAmountTHB,
        ArrivalDate,
        CurrencyAmount,
        PaymentTypeFR,
        CurrencyRepOffice
    }
    #endregion
}
