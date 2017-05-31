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
    public class FixedAdvanceDocumentService : ServiceBase<FixedAdvanceDocument, long>, IFixedAdvanceDocumentService
    {
        public IUserAccount UserAccount { get; set; }
        public ITransactionService TransactionService { get; set; }
        public IWorkFlowService WorkFlowService { get; set; }
        public ISCGDocumentService SCGDocumentService { get; set; }
        public IAvAdvanceItemService AvAdvanceItemService { get; set; }
        public ISCGEmailService SCGEmailService { get; set; }
        public IDbHolidayProfileService DbHolidayProfileService { get; set; }
        public IWorkFlowResponseTokenService WorkFlowResponseTokenService { get; set; }

        #region Override Method
        public override IDao<FixedAdvanceDocument, long> GetBaseDao()
        {
            return ScgeAccountingDaoProvider.FixedAdvanceDocumentDao;
        }
        #endregion

        public DataSet PrepareDS()
        {
            FixedAdvanceDataSet ds = new FixedAdvanceDataSet();
            return ds.Clone();

        }
        public DataSet PrepareDS(long documentId, bool isCopy)
        {
            FixedAdvanceDataSet fixedadvanceDs = (FixedAdvanceDataSet)this.PrepareInternalDataToDataset(documentId, isCopy);
            fixedadvanceDs.AcceptChanges();
            return fixedadvanceDs;
        }


        public DataSet PrepareInternalDataToDataset(long documentId, bool isCopy)
        {
            FixedAdvanceDataSet fixedadvanceDs = new FixedAdvanceDataSet();
            //ดึงจาก base มาใส่ใน DataSet
            FixedAdvanceDocument favDb = ScgeAccountingQueryProvider.FixedAdvanceDocumentQuery.GetFixedAdvanceByDocumentID(documentId);
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (favDb == null)
            {
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("NoFixedAdvanceFound"));
            }
            if (!errors.IsEmpty)
            {
                throw new ServiceValidationException(errors);
            }
            // Prepare Data to Document datatable.
            if (!isCopy)
            {
                SCGDocumentService.PrepareDataToDataset(fixedadvanceDs, documentId);
            }
            else
            {
                SCGDocumentService.PrepareDataInternalToDataset(fixedadvanceDs, documentId, isCopy);
            }

            // Set data to Advance Document row in advanceDs.
            FixedAdvanceDataSet.FixedAdvanceDocumentRow favRow = fixedadvanceDs.FixedAdvanceDocument.NewFixedAdvanceDocumentRow();
            if (!isCopy)
            {
                favRow.DocumentID = favDb.DocumentID.DocumentID;
                favRow.FixedAdvanceID = favDb.FixedAdvanceID;
                favRow.EffectiveFromDate = favDb.EffectiveFromDate;
                favRow.EffectiveToDate = favDb.EffectiveToDate;
                favRow.RequestDate = favDb.RequestDate;
                if (favDb.ReturnRequestDate != null)
                    favRow.ReturnRequestDate = (DateTime)favDb.ReturnRequestDate;
                /*Check RefFixedAdvanceID is null*/
                favRow.Objective = favDb.Objective;
                if (favDb.RefFixedAdvanceID != null)
                {
                    favRow.RefFixedAdvanceID = (long)favDb.RefFixedAdvanceID;
                }
                favRow.FixedAdvanceType = favDb.FixedAdvanceType;
                favRow.Amount = (decimal)favDb.Amount;
                favRow.NetAmount = (decimal)favDb.NetAmount;
            }
            else
            {
                if (fixedadvanceDs.Document.Rows.Count > 0)
                {
                    FixedAdvanceDataSet.DocumentRow docRow = (FixedAdvanceDataSet.DocumentRow)fixedadvanceDs.Document.Rows[0];
                    favRow.DocumentID = docRow.DocumentID;
                }
                favRow.EffectiveFromDate = favDb.EffectiveFromDate;
                favRow.EffectiveToDate = favDb.EffectiveToDate;
                favRow.RequestDate = favDb.RequestDate;
                if (favDb.ReturnRequestDate != null)
                    favRow.ReturnRequestDate = (DateTime)favDb.ReturnRequestDate;
                favRow.Objective = favDb.Objective;
                if (favDb.RefFixedAdvanceID != null)
                {
                    favRow.RefFixedAdvanceID = (long)favDb.RefFixedAdvanceID;
                }
                favRow.FixedAdvanceType = favDb.FixedAdvanceType;
                favRow.Amount = (decimal)favDb.Amount;
                favRow.NetAmount = (decimal)favDb.NetAmount;
            }

            favRow.PaymentType = favDb.PaymentType;
            favRow.ReturnPaymentType = favDb.ReturnPaymentType;

            if (favDb.PBID != null && favDb.PBID.Pbid > 0)
                favRow.PBID = favDb.PBID.Pbid;
            else
                favRow.SetPBIDNull();

            if (favDb.ReturnPBID != null && favDb.ReturnPBID.Pbid > 0)
                favRow.ReturnPBID = favDb.ReturnPBID.Pbid;
            else
                favRow.SetReturnPBIDNull();

            if (favDb.ReturnServiceTeamID != null && favDb.ReturnServiceTeamID.ServiceTeamID > 0)
                favRow.ReturnServiceTeamID = favDb.ReturnServiceTeamID.ServiceTeamID;
            else
                favRow.SetReturnServiceTeamIDNull();

            favRow.PostingStatusReturn = favDb.PostingStatusReturn;
            favRow.BranchCodeReturn = favDb.BranchCodeReturn;
            favRow.FixedAdvanceBankAccount = favDb.FixedAdvanceBankAccount;
            if (favDb.PaymentMethodIDReturn != null)
            {
                favRow.PaymentMethodIDReturn = (long)favDb.PaymentMethodIDReturn;
            }

            if (favDb.PostingDateReturn != null)
            {
                favRow.PostingDateReturn = (DateTime)favDb.PostingDateReturn;
            }
            else
            {
                favRow.PostingDateReturn = DateTime.MinValue;
            }

            if (favDb.BaseLineDateReturn != null)
            {
                favRow.BaseLineDateReturn = (DateTime)favDb.BaseLineDateReturn;
            }
            else
            {
                favRow.BaseLineDateReturn = DateTime.MinValue;
            }

            favRow.Active = favDb.Active;
            favRow.CreBy = UserAccount.UserID;
            favRow.CreDate = DateTime.Now;
            favRow.UpdBy = UserAccount.UserID;
            favRow.UpdDate = DateTime.Now;
            favRow.UpdPgm = UserAccount.CurrentProgramCode;
            fixedadvanceDs.FixedAdvanceDocument.AddFixedAdvanceDocumentRow(favRow);
            return fixedadvanceDs;
        }

        public long AddFixedAdvanceDocument(Guid txid, string fixedadvanceType)
        {
            FixedAdvanceDataSet ds = (FixedAdvanceDataSet)TransactionService.GetDS(txid);
            FixedAdvanceDataSet.DocumentRow docRow = ds.Document.NewDocumentRow();//create newRow to dataset
            ds.Document.AddDocumentRow(docRow);

            FixedAdvanceDataSet.FixedAdvanceDocumentRow fixedavRow = ds.FixedAdvanceDocument.NewFixedAdvanceDocumentRow();
            fixedavRow.DocumentID = docRow.DocumentID;
            ds.FixedAdvanceDocument.AddFixedAdvanceDocumentRow(fixedavRow);
            return fixedavRow.FixedAdvanceID;

        }

        public void UpdatePostingStatusFixedAdvanceDocument(long DocumentID, string Status)
        {
            FixedAdvanceDocument docFixedAdvance = ScgeAccountingQueryProvider.FixedAdvanceDocumentQuery.GetFixedAdvanceByDocumentID(DocumentID);

            if (docFixedAdvance != null)
            {
                docFixedAdvance.PostingStatusReturn = Status;
                ScgeAccountingDaoProvider.FixedAdvanceDocumentDao.SaveOrUpdate(docFixedAdvance);
            }
        }

        public void UpdateFixedDocumentTransaction(FixedAdvanceDocument fixedAvAdvanceDoc, Guid txid)
        {
            DateTime dateAndTime = DateTime.Now;
            DateTime CurDate = dateAndTime.Date;

            IList<object> fixedadvanceEditableField = new List<object>();
            string mode = string.Empty;
            mode = "DM";
            if (fixedAvAdvanceDoc.DocumentID != null)
            {
                fixedadvanceEditableField = GetEditableFields(fixedAvAdvanceDoc.DocumentID.DocumentID);
            }
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            FixedAdvanceDataSet ds = (FixedAdvanceDataSet)TransactionService.GetDS(txid);
            FixedAdvanceDataSet.FixedAdvanceDocumentRow rowFixedAdvance = (FixedAdvanceDataSet.FixedAdvanceDocumentRow)ds.FixedAdvanceDocument.FindByFixedAdvanceID(fixedAvAdvanceDoc.FixedAdvanceID);
            SS.Standard.WorkFlow.DTO.WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(fixedAvAdvanceDoc.DocumentID.DocumentID);
            FixedAdvanceDataSet.DocumentRow rowDocument = (FixedAdvanceDataSet.DocumentRow)ds.Document.FindByDocumentID(rowFixedAdvance.DocumentID);
            rowFixedAdvance.BeginEdit();
            // Validate Amount.
            Dbpb pb = ScgDbQueryProvider.DbPBQuery.FindByIdentity(fixedAvAdvanceDoc.PBID == null ? 0 : fixedAvAdvanceDoc.PBID.Pbid);

            #region Validate value

            if (workFlow == null)
            {
                if (fixedAvAdvanceDoc.NetAmount > 0)
                {
                    if (fixedAvAdvanceDoc.PaymentType == "CA")
                        errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("Can't receiv payment type cash."));
                }
                else
                {
                    if (fixedAvAdvanceDoc.PaymentType == "CQ")
                        errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("Return payment can't receiv payment type cheque."));
                }

                if (String.IsNullOrEmpty(fixedAvAdvanceDoc.PaymentType))
                {
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("Payment Type is Required."));
                }

                if (fixedAvAdvanceDoc.Amount == 0)
                {
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("PleaseFillAmount"));
                }

                if (String.IsNullOrEmpty(fixedAvAdvanceDoc.Objective))
                {
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("ObjectiveIsRequired"));
                }
                if (fixedAvAdvanceDoc.EffectiveFromDate > fixedAvAdvanceDoc.EffectiveToDate)
                {
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("EffectiveFromdateOverEffectiveTodate"));
                }
                if (fixedAvAdvanceDoc.RequestDate == DateTime.MinValue)
                {
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("RequestDateIsRequired"));
                }
                else
                {
                    if (fixedAvAdvanceDoc.RequestDate < fixedAvAdvanceDoc.EffectiveFromDate)
                    {
                        errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("RequestDateIsLessThanEffectDateFrom"));
                    }
                    if (fixedAvAdvanceDoc.RequestDate < CurDate )
                    {
                        errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("EffectDateFromOrRequestDateLessToday"));
                    }
                    if (fixedAvAdvanceDoc.RequestDate > fixedAvAdvanceDoc.EffectiveToDate)
                    {
                        errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("RequestDateOverEffectiveTodate"));
                    }
                }
                if (fixedAvAdvanceDoc.PaymentType == "CQ" && (fixedAvAdvanceDoc.PBID == null ||fixedAvAdvanceDoc.PBID.Pbid == 0))
                {
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("Counter Cashier is Required."));
                }
                if (fixedAvAdvanceDoc.PaymentType == "CA" && (fixedAvAdvanceDoc.PBID == null || fixedAvAdvanceDoc.PBID.Pbid == 0))
                {
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("Counter Cashier is Required."));
                }
                if (fixedAvAdvanceDoc.FixedAdvanceType == 1)
                {
                    if (fixedAvAdvanceDoc.EffectiveFromDate < CurDate )
                    {
                        errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("EffectiveFromIsLessThanToday"));
                    }
                }
                else
                {
                    if (fixedAvAdvanceDoc.RefFixedAdvanceID == 0)
                        errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("RefFixedAdvanceIsRequired"));
                }
            }
            else
            {

                if (workFlow.CurrentState.Name == "Outstanding")
                {

                    if (fixedAvAdvanceDoc.ReturnRequestDate == DateTime.MinValue)
                    {
                        errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("ReturnDateIsRequired"));
                    }
                    else
                    {
                        if (fixedAvAdvanceDoc.ReturnRequestDate < fixedAvAdvanceDoc.EffectiveFromDate)
                        {
                            errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("EffectDateFromOverRequestDate"));
                        }
                        if (fixedAvAdvanceDoc.ReturnRequestDate < CurDate)
                        {
                            errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("EffectDateFromOrRequestDateLessToday"));
                        }
                        if (fixedAvAdvanceDoc.ReturnRequestDate > fixedAvAdvanceDoc.EffectiveToDate)
                        {
                            errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("RequestDateOverEffectiveTodate"));
                        }
                    }
                    if (String.IsNullOrEmpty(fixedAvAdvanceDoc.ReturnPaymentType))
                    {
                        errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("Payment Type is Required."));
                    }
                    else
                    {
                        if (fixedAvAdvanceDoc.ReturnPBID == null || fixedAvAdvanceDoc.ReturnPBID.Pbid == 0)
                        {
                            errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("Return Counter Cashier is Required."));
                        }
                    }
                    if (fixedAvAdvanceDoc.ReturnPaymentType == "CQ")
                        errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("Return payment can't receiv payment type cheque."));
                }
                else if (workFlow.CurrentState.Name == "WaitReturnComplete")
                {
                    /*N-Addnew*/

                    if (fixedAvAdvanceDoc.ReturnPaymentType == "")
                    {
                        errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("Return Payment Type is Required."));
                    }
                    else
                    {
                        if (fixedAvAdvanceDoc.ReturnPBID == null || fixedAvAdvanceDoc.ReturnPBID.Pbid == 0)
                            errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("Return Counter Cashier is Required."));
                    }
                }
                else if (workFlow.CurrentState.Name == "WaitReturn")
                {
                    /*N-Addnew*/
                    if (fixedAvAdvanceDoc.FixedAdvanceType == 1)
                    {
                        /*New*/
                        if (fixedAvAdvanceDoc.PaymentType == "")
                        {
                            errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("Payment Type is Required."));
                        }
                        else if (fixedAvAdvanceDoc.PaymentType == "CQ")
                        {
                            if (fixedAvAdvanceDoc.PBID == null || fixedAvAdvanceDoc.PBID.Pbid == 0)
                                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("Counter Cashier is Required."));
                        }
                        /*check change Requestdate*/
                        if (fixedAvAdvanceDoc.RequestDate == DateTime.MinValue)
                        {
                            errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("RequestDateIsRequired"));
                        }
                        else
                        {
                            if (fixedAvAdvanceDoc.RequestDate < fixedAvAdvanceDoc.EffectiveFromDate)
                            {
                                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("RequestDateIsLessThanEffectDateFrom"));
                            }
                            if (fixedAvAdvanceDoc.RequestDate < CurDate)
                            {
                                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("EffectDateFromOrRequestDateLessToday"));
                            }
                            if (fixedAvAdvanceDoc.RequestDate > fixedAvAdvanceDoc.EffectiveToDate)
                            {
                                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("RequestDateOverEffectiveTodate"));
                            }
                        }
                    }
                    else
                    {
                        /*Adjust*/
                        if (fixedAvAdvanceDoc.NetAmount < 0)
                        {
                            /*PaybackCompany*/
                            if (fixedAvAdvanceDoc.PaymentType == "")
                            {
                                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("Payment Type is Required."));
                            }
                            else
                            {
                                if (fixedAvAdvanceDoc.PBID == null || fixedAvAdvanceDoc.PBID.Pbid == 0)
                                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("Counter Cashier is Required."));
                            }
                        }
                        else
                        {
                            if (fixedAvAdvanceDoc.PaymentType == "")
                            {
                                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("Payment Type is Required."));
                            }
                            else if (fixedAvAdvanceDoc.PaymentType == "CQ")
                            {
                                if (fixedAvAdvanceDoc.PBID == null || fixedAvAdvanceDoc.PBID.Pbid == 0)
                                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("Counter Cashier is Required."));
                            }
                        }
                    }
                    /*check change Requestdate*/
                    if (fixedAvAdvanceDoc.RequestDate == DateTime.MinValue)
                    {
                        errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("RequestDateIsRequired"));
                    }
                    else
                    {
                        if (fixedAvAdvanceDoc.RequestDate < fixedAvAdvanceDoc.EffectiveFromDate)
                        {
                            errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("RequestDateIsLessThanEffectDateFrom"));
                        }
                        if (fixedAvAdvanceDoc.RequestDate < CurDate)
                        {
                            errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("EffectDateFromOrRequestDateLessToday"));
                        }
                        if (fixedAvAdvanceDoc.RequestDate > fixedAvAdvanceDoc.EffectiveToDate)
                        {
                            errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("RequestDateOverEffectiveTodate"));
                        }
                    }
                }
                else if (workFlow.CurrentState.Name == "WaitVerify")
                {
                    /*N-Addnew*/
                   //check verifyer edit valueCounter Cashier is Required.
                    if (fixedAvAdvanceDoc.FixedAdvanceType == 1)
                    {
                        /*New*/
                        if (fixedAvAdvanceDoc.PaymentType == "")
                        {
                            errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("Payment Type is Required."));
                        }
                        else if(fixedAvAdvanceDoc.PaymentType == "CQ")
                        {
                            if (fixedAvAdvanceDoc.PBID == null || fixedAvAdvanceDoc.PBID.Pbid == 0)
                                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("Counter Cashier is Required."));
                        }
                        /*check change Requestdate*/
                        if (fixedAvAdvanceDoc.RequestDate == DateTime.MinValue)
                        {
                            errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("RequestDateIsRequired"));
                        }
                        else
                        {
                            if (fixedAvAdvanceDoc.RequestDate < fixedAvAdvanceDoc.EffectiveFromDate)
                            {
                                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("RequestDateIsLessThanEffectDateFrom"));
                            }
                            //if (fixedAvAdvanceDoc.RequestDate < CurDate)
                            //{
                            //    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("EffectDateFromOrRequestDateLessToday"));
                            //}
                            if (fixedAvAdvanceDoc.RequestDate > fixedAvAdvanceDoc.EffectiveToDate)
                            {
                                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("RequestDateOverEffectiveTodate"));
                            }
                        }
                    }
                    else
                    {
                        /*Adjust*/
                        if (fixedAvAdvanceDoc.NetAmount < 0)
                        {
                            /*PaybackCompany*/
                            if (fixedAvAdvanceDoc.PaymentType == "")
                            {
                                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("Payment Type is Required."));
                            }
                            else 
                            {
                                if (fixedAvAdvanceDoc.PBID == null || fixedAvAdvanceDoc.PBID.Pbid == 0)
                                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("Counter Cashier is Required."));
                            }
                        }
                        else
                        {
                            if (fixedAvAdvanceDoc.PaymentType == "")
                            {
                                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("Payment Type is Required."));
                            }
                            else if (fixedAvAdvanceDoc.PaymentType == "CQ")
                            {
                                if (fixedAvAdvanceDoc.PBID == null || fixedAvAdvanceDoc.PBID.Pbid == 0)
                                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("Counter Cashier is Required."));
                            }
                        }
                        if (fixedAvAdvanceDoc.RequestDate == DateTime.MinValue)
                        {
                            errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("RequestDateIsRequired"));
                        }
                        else
                        {
                            if (fixedAvAdvanceDoc.RequestDate < fixedAvAdvanceDoc.EffectiveFromDate)
                            {
                                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("RequestDateIsLessThanEffectDateFrom"));
                            }
                            //if (fixedAvAdvanceDoc.RequestDate < CurDate)
                            //{
                            //    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("EffectDateFromOrRequestDateLessToday"));
                            //}
                            if (fixedAvAdvanceDoc.RequestDate > fixedAvAdvanceDoc.EffectiveToDate)
                            {
                                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("RequestDateOverEffectiveTodate"));
                            }
                        }
                    }
                }
                else
                {
                    if (fixedAvAdvanceDoc.EffectiveFromDate < CurDate && fixedAvAdvanceDoc.FixedAdvanceType == 1)
                    {
                        errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("EffectiveFromIsLessThanToday"));
                    }
                    if (fixedAvAdvanceDoc.EffectiveFromDate > fixedAvAdvanceDoc.EffectiveToDate)
                    {
                        errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("EffectiveFromdateOverEffectiveTodate"));
                    }

                    /*check change Requestdate*/
                    if (fixedAvAdvanceDoc.RequestDate == DateTime.MinValue)
                    {
                        errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("RequestDateIsRequired"));
                    }
                    else
                    {
                        if (fixedAvAdvanceDoc.RequestDate < fixedAvAdvanceDoc.EffectiveFromDate)
                        {
                            errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("RequestDateIsLessThanEffectDateFrom"));
                        }
                        if (fixedAvAdvanceDoc.RequestDate < CurDate)
                        {
                            errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("EffectDateFromOrRequestDateLessToday"));
                        }
                        if (fixedAvAdvanceDoc.RequestDate > fixedAvAdvanceDoc.EffectiveToDate)
                        {
                            errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("RequestDateOverEffectiveTodate"));
                        }
                    }
                   
                    if (fixedAvAdvanceDoc.RefFixedAdvanceID == 0 && fixedAvAdvanceDoc.FixedAdvanceType == 2)
                    {
                        errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("RefFixedAdvanceIsRequired"));
                    }

                    if (fixedAvAdvanceDoc.NetAmount < 0)
                    {
                        /*PaybackCompany*/
                        if (fixedAvAdvanceDoc.PaymentType == "")
                        {
                            errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("Payment Type is Required."));
                        }
                        else
                        {
                            if (fixedAvAdvanceDoc.PBID == null || fixedAvAdvanceDoc.PBID.Pbid == 0)
                                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("Counter Cashier is Required."));
                        }
                    }
                    else
                    {
                        if (fixedAvAdvanceDoc.PaymentType == "")
                        {
                            errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("Payment Type is Required."));
                        }
                        else if (fixedAvAdvanceDoc.PaymentType == "CQ")
                        {
                            if (fixedAvAdvanceDoc.PBID == null || fixedAvAdvanceDoc.PBID.Pbid == 0)
                                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("Counter Cashier is Required."));
                        }
                    }
                }
            }

            var result = fixedAvAdvanceDoc.EffectiveToDate - fixedAvAdvanceDoc.EffectiveFromDate;
            //var a = Convert.ToString(result.Days);

            //DateTime datenow = DateTime.Now;
            //DateTime setyearnow = new DateTime(datenow.Year, 1, 1);
            /*N-edited*/
            DateTime setyearnow = DateTime.Now;
            DateTime setaddmonth = setyearnow.AddMonths(int.Parse(ParameterServices.FixedAdvanceConfigEffectiveDate));
            var result2 = setaddmonth - setyearnow;
            int paramdate = result2.Days;

            if (Convert.ToInt32(result.Days) > paramdate)
            {
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("EffectiveDateToIsOverMaximumRange"));
            }


            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
            #endregion

            rowFixedAdvance.DocumentID = fixedAvAdvanceDoc.DocumentID.DocumentID;

            if (fixedAvAdvanceDoc.PBID != null)
                rowFixedAdvance.PBID = fixedAvAdvanceDoc.PBID.Pbid;
            else
                rowFixedAdvance.SetPBIDNull();

            if (fixedAvAdvanceDoc.ReturnPBID != null)
                rowFixedAdvance.ReturnPBID = fixedAvAdvanceDoc.ReturnPBID.Pbid;
            else
                rowFixedAdvance.SetReturnPBIDNull();

            if (fixedAvAdvanceDoc.ServiceTeamID != null)
                rowFixedAdvance.ServiceTeamID = fixedAvAdvanceDoc.ServiceTeamID.ServiceTeamID;
            else
                rowFixedAdvance.SetServiceTeamIDNull();

            if (fixedAvAdvanceDoc.ReturnServiceTeamID != null)
                rowFixedAdvance.ReturnServiceTeamID = fixedAvAdvanceDoc.ReturnServiceTeamID.ServiceTeamID;
            else
                rowFixedAdvance.SetReturnServiceTeamIDNull();

            if (fixedAvAdvanceDoc.RefFixedAdvanceID != null)
                rowFixedAdvance.RefFixedAdvanceID = (long)fixedAvAdvanceDoc.RefFixedAdvanceID;
            else
                rowFixedAdvance.SetRefFixedAdvanceIDNull();

            rowFixedAdvance.FixedAdvanceType = fixedAvAdvanceDoc.FixedAdvanceType;
            rowFixedAdvance.PaymentType = fixedAvAdvanceDoc.PaymentType;
            rowFixedAdvance.ReturnPaymentType = fixedAvAdvanceDoc.ReturnPaymentType;
            rowFixedAdvance.FixedAdvanceBankAccount = fixedAvAdvanceDoc.FixedAdvanceBankAccount;

            rowFixedAdvance.RequestDate = fixedAvAdvanceDoc.RequestDate;
            if (fixedAvAdvanceDoc.ReturnRequestDate != null)
                rowFixedAdvance.ReturnRequestDate = (DateTime)fixedAvAdvanceDoc.ReturnRequestDate;
            else
                rowFixedAdvance.SetReturnRequestDateNull();

            if (fixedAvAdvanceDoc.BaseLineDateReturn != null)
                rowFixedAdvance.BaseLineDateReturn = (DateTime)fixedAvAdvanceDoc.BaseLineDateReturn;
            else
                rowFixedAdvance.SetBaseLineDateReturnNull();

            if (fixedAvAdvanceDoc.PaymentMethodIDReturn != null)
                rowFixedAdvance.PaymentMethodIDReturn = (long)fixedAvAdvanceDoc.PaymentMethodIDReturn;
            else
                rowFixedAdvance.SetPaymentMethodIDReturnNull();

            if (fixedAvAdvanceDoc.PostingDateReturn != null)
                rowFixedAdvance.PostingDateReturn = (DateTime)fixedAvAdvanceDoc.PostingDateReturn;

            rowFixedAdvance.BranchCodeReturn = fixedAvAdvanceDoc.BranchCodeReturn;

            rowFixedAdvance.EffectiveFromDate = fixedAvAdvanceDoc.EffectiveFromDate;
            rowFixedAdvance.EffectiveToDate = fixedAvAdvanceDoc.EffectiveToDate;
            rowFixedAdvance.Objective = fixedAvAdvanceDoc.Objective;
            rowFixedAdvance.Amount = Math.Round((decimal)fixedAvAdvanceDoc.Amount, 2, MidpointRounding.AwayFromZero);
            rowFixedAdvance.NetAmount = Math.Round((decimal)fixedAvAdvanceDoc.NetAmount, 2, MidpointRounding.AwayFromZero);
            rowFixedAdvance.Active = fixedAvAdvanceDoc.Active;
            rowFixedAdvance.CreBy = UserAccount.UserID;
            rowFixedAdvance.CreDate = DateTime.Now;
            rowFixedAdvance.UpdBy = UserAccount.UserID;
            rowFixedAdvance.UpdDate = DateTime.Now;
            rowFixedAdvance.UpdPgm = UserAccount.CurrentProgramCode;
            rowFixedAdvance.EndEdit();
        }

        public long SaveFixedAdvance(Guid txid, long fixedadvanceId)
        {
            FixedAdvanceDataSet fixedadvanceDataset = (FixedAdvanceDataSet)TransactionService.GetDS(txid);
            FixedAdvanceDataSet.FixedAdvanceDocumentRow row = fixedadvanceDataset.FixedAdvanceDocument.FindByFixedAdvanceID(fixedadvanceId);
            long documentID = row.DocumentID;
            documentID = SCGDocumentService.SaveSCGDocument(txid, documentID);
            fixedadvanceId = ScgeAccountingDaoProvider.FixedAdvanceDocumentDao.Persist(fixedadvanceDataset.FixedAdvanceDocument);
            return fixedadvanceId;
        }

        public void UpdateFixedAdvanceDocument(FixedAdvanceDocument fixedadvance)
        {
            ScgeAccountingDaoProvider.FixedAdvanceDocumentDao.Update(fixedadvance);
        }

        public FixedAdvanceDocument FindNetAmount(long docId)
        {
            return ScgeAccountingQueryProvider.FixedAdvanceDocumentQuery.FindNetAmountQuery(docId);
        }

        public IList<FixedAdvanceRefObjectValues> FindRefFixedAdvance(long comId, long userId, long requesterId, long docId, string seachType)
        {
            IList<FixedAdvanceRefObjectValues> result = ScgeAccountingQueryProvider.FixedAdvanceDocumentQuery.FindRefFixedAdvance(comId, userId, requesterId, docId, seachType);
            return result;
        }
        public IList<FixedAdvanceCanRefObjectValues> FindFixedAdvanceCanRef(long comId, long userId, long requesterId, long docId, long currentfixedid)
        {
            //IList<FixedAdvanceCanRefObjectValues> result = ScgeAccountingQueryProvider.FixedAdvanceDocumentQuery.FindFixedAdvanceCanRef(comId, userId, docId, editmode);
            IList<FixedAdvanceCanRefObjectValues> result = ScgeAccountingQueryProvider.FixedAdvanceDocumentQuery.FindFixedAdvanceCanRef(comId, userId, requesterId, docId,currentfixedid);
            return result;
        }

        #region GetEditableFields and GetVisibleFields
        public IList<object> GetVisibleFields(long? documentID)
        {
            IList<object> visibleFields = new List<object>();

            if (!documentID.HasValue) // Check whether new flag then return the default editableFields.
            {
                visibleFields.Add(FixedAdvanceFieldGroup.PaymentType);
                visibleFields.Add(FixedAdvanceFieldGroup.Initiator);
                visibleFields.Add(FixedAdvanceFieldGroup.Attachment);
                visibleFields.Add(FixedAdvanceFieldGroup.Memo);
                visibleFields.Add(FixedAdvanceFieldGroup.Other);
                visibleFields.Add(FixedAdvanceFieldGroup.RequestDate);
                visibleFields.Add(FixedAdvanceFieldGroup.Company);
                visibleFields.Add(FixedAdvanceFieldGroup.ServiceTeam);
                visibleFields.Add(FixedAdvanceFieldGroup.BuActor);
                visibleFields.Add(FixedAdvanceFieldGroup.CounterCashier);
                visibleFields.Add(FixedAdvanceFieldGroup.Subject);
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
            editableFields.Add(FixedAdvanceFieldGroup.PaymentType);
            editableFields.Add(FixedAdvanceFieldGroup.Initiator);
            editableFields.Add(FixedAdvanceFieldGroup.Attachment);
            editableFields.Add(FixedAdvanceFieldGroup.Memo);
            editableFields.Add(FixedAdvanceFieldGroup.Other);
            editableFields.Add(FixedAdvanceFieldGroup.RequestDate);
            editableFields.Add(FixedAdvanceFieldGroup.ServiceTeam);
            editableFields.Add(FixedAdvanceFieldGroup.BuActor);
            editableFields.Add(FixedAdvanceFieldGroup.CounterCashier);
            editableFields.Add(FixedAdvanceFieldGroup.Subject);
            editableFields.Add(FixedAdvanceFieldGroup.Company);
            return editableFields;
        }
        #endregion

        public void SendEmailToOverDueDate()
        {
            IList<FixedAdvanceOverDue> voList = ScgeAccountingQueryProvider.SCGDocumentQuery.GetFixedAdvanceOverdueList();

            foreach (FixedAdvanceOverDue item in voList)
            {
                SCGEmailService.SendEmailEM15(item.CacheWorkflowID, item.RequesterID);
            }
        }
        /*เพิ่มแจ้งเตือนก่อนครบกำหนด fixedadvance*/
        public void SendEmailToBeforeDueDate()
        {
            /*Criteria ก่อน7วัน กับก่อน30วัน ค่าจากใน dbparameter*/
            IList<FixedAdvanceBeforeDue> mailLogList = ScgeAccountingQueryProvider.SCGDocumentQuery.GetFixedAdvanceBeforedueList();

            foreach (FixedAdvanceBeforeDue item in mailLogList)
            {
                SCGEmailService.SendEmailEM16(item.DocumentId);
            }
        }
    }
    #region FixedAdvanceFieldGroup Enum
    public enum FixedAdvanceFieldGroup
    {
        PaymentType,
        Attachment,
        Memo,
        Other,
        VerifyDetail,
        Initiator,
        ServiceTeam,
        Company,
        BuActor,
        CounterCashier,
        Subject,
        Return,
        ClearingReturn,
        RequestDate
    }
    
    #endregion
}
