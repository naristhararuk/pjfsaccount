using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;
using SS.Standard.Utilities;

using SCG.eAccounting.DTO;
using SCG.eAccounting.BLL;
using SCG.eAccounting.DTO.ValueObject;
using System.Data;
using SCG.eAccounting.DTO.DataSet;
using SCG.eAccounting.DAL;
using SS.Standard.Security;
using System.IO;
using SCG.eAccounting.Query;
using Spring.Validation;
using SCG.DB.Query;
using SCG.DB.DTO;
using SS.DB.Query;
using System.Globalization;

namespace SCG.eAccounting.BLL.Implement
{
    public partial class FnExpenseInvoiceService : ServiceBase<FnExpenseInvoice, long>, IFnExpenseInvoiceService
    {
        public IUserAccount UserAccount { get; set; }
        public ITransactionService TransactionService { get; set; }
        public IFnExpenseDocumentService FnExpenseDocumentService { get; set; }
        public IFnExpenseInvoiceItemService FnExpenseInvoiceItemService { get; set; }

        public override IDao<FnExpenseInvoice, long> GetBaseDao()
        {
            return ScgeAccountingDaoProvider.FnExpenseInvoiceDao;
        }

        #region Transaction
        public long AddInvoiceOnTransaction(FnExpenseInvoice invoice, Guid txId)
        {
            ExpenseDataSet ds = (ExpenseDataSet)TransactionService.GetDS(txId);
            ExpenseDataSet.FnExpenseInvoiceRow row = ds.FnExpenseInvoice.NewFnExpenseInvoiceRow();

            ExpenseDataSet.FnExpenseDocumentRow expRow = null;

            if (invoice != null)
            {
                if (invoice.Expense != null)
                {
                    row.ExpenseID = invoice.Expense.ExpenseID;
                    expRow = ds.FnExpenseDocument.FindByExpenseID(row.ExpenseID);
                }

                row.TotalAmount = (decimal)invoice.TotalAmount;
                row.TotalBaseAmount = (decimal)invoice.TotalBaseAmount;
                row.NetAmount = (decimal)invoice.NetAmount;
                row.InvoiceDocumentType = invoice.InvoiceDocumentType;
                row.isVAT = invoice.IsVAT.HasValue ? invoice.IsVAT.Value : false;
                row.isWHT = invoice.IsWHT.HasValue ? invoice.IsWHT.Value : false;

                if (invoice.TotalAmountLocalCurrency.HasValue)
                {
                    row.TotalAmountLocalCurrency = (decimal)invoice.TotalAmountLocalCurrency.Value;
                }

                if (invoice.TotalBaseAmountLocalCurrency.HasValue)
                {
                    row.TotalBaseAmountLocalCurrency = (decimal)invoice.TotalBaseAmountLocalCurrency.Value;
                }

                if (invoice.NetAmountLocalCurrency.HasValue)
                {
                    row.NetAmountLocalCurrency = (decimal)invoice.NetAmountLocalCurrency.Value;
                }

                if (expRow != null)
                {
                    if (!expRow.IsExchangeRateForLocalCurrencyNull())
                    {
                        row.ExchangeRateForLocalCurrency = expRow.ExchangeRateForLocalCurrency;
                    }

                    if (!expRow.IsExchangeRateMainToTHBCurrencyNull())
                    {
                        row.ExchangeRateMainToTHBCurrency = expRow.ExchangeRateMainToTHBCurrency;
                    }
                }
            }

            row.Active = true;
            row.CreBy = UserAccount.UserID;
            row.CreDate = DateTime.Now;
            row.UpdBy = UserAccount.UserID;
            row.UpdDate = DateTime.Now;
            row.UpdPgm = UserAccount.CurrentProgramCode;

            ds.FnExpenseInvoice.AddFnExpenseInvoiceRow(row);

            return row.InvoiceID;
        }

        public void DeleteInvoiceOnTransaction(long invoiceId, Guid txId)
        {
            ExpenseDataSet expDs = (ExpenseDataSet)TransactionService.GetDS(txId);
            ExpenseDataSet.FnExpenseInvoiceRow row = expDs.FnExpenseInvoice.FindByInvoiceID(invoiceId);
            row.Delete();
        }
        public void UpdateInvoiceOnTransaction(FnExpenseInvoice invoice, Guid txId)
        {
            ValidationErrors errors = new ValidationErrors();
            ExpenseDataSet ds = (ExpenseDataSet)TransactionService.GetDS(txId);
            ExpenseDataSet.FnExpenseInvoiceRow row = ds.FnExpenseInvoice.FindByInvoiceID(invoice.InvoiceID);
            ExpenseDataSet.FnExpenseDocumentRow expRow = null;
            bool isRepOffice = false;
            #region Old Code
            //ExpenseDataSet.FnExpenseDocumentRow exRow = ds.FnExpenseDocument.FindByExpenseID(invoice.Expense.ExpenseID);
            //ExpenseDataSet.DocumentRow documentRow = ds.Document.FindByDocumentID(exRow.DocumentID);
            //IList<object> editableFields;
            //// if current mode is edit mode.
            //if (documentRow.RowState == DataRowState.Modified)
            //{
            //    // Get Editable Fields for validate.
            //    editableFields = FnExpenseDocumentService.GetEditableFields(documentRow.DocumentID);
            //}
            //else // if current mode is new mode.
            //{
            //    editableFields = FnExpenseDocumentService.GetEditableFields(null);
            //}
            #endregion
            long documentId = 0;
            if (invoice != null && invoice.Expense != null)
            {
                expRow = ds.FnExpenseDocument.FindByExpenseID(invoice.Expense.ExpenseID);
                documentId = expRow == null ? 0 : expRow.DocumentID;
                if (!expRow.IsIsRepOfficeNull())
                    isRepOffice = expRow.IsRepOffice;
            }

            SS.Standard.WorkFlow.DTO.WorkFlow workflow = SS.Standard.WorkFlow.Query.WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(documentId);



            // Validate ExpenseDocument.
            if (invoice.Expense == null)
                errors.AddError("ValidationError", new ErrorMessage("ExpenseDocumentRequired"));

            // Validate InvoiceDocumentType.
            if (string.IsNullOrEmpty(invoice.InvoiceDocumentType))
                errors.AddError("ValidationError", new ErrorMessage("InvoiceDocumentType"));

            if (invoice.InvoiceDate.HasValue && invoice.InvoiceDate.Value.Date.CompareTo(DateTime.Today) > 0)
                errors.AddError("ValidationError", new ErrorMessage("DateMustBeLessThanNow"));

            if (!invoice.IsVAT.HasValue)
                errors.AddError("ValidationError", new ErrorMessage("VATRequired"));

            if (!invoice.IsWHT.HasValue)
                errors.AddError("ValidationError", new ErrorMessage("WHTRequired"));

            if ((invoice.IsVAT.HasValue && invoice.IsVAT.Value) || (invoice.IsWHT.HasValue && invoice.IsWHT.Value))
            {
                // Validate InvoiceNo.
                if (string.IsNullOrEmpty(invoice.InvoiceNo))
                    errors.AddError("ValidationError", new ErrorMessage("InvoiceNoRequired"));

                // Valdiate InvocieDate.
                if (!invoice.InvoiceDate.HasValue)
                    errors.AddError("ValidationError", new ErrorMessage("InvoiceDateRequired"));

                // Validate Vendor Tax ID
                //if (invoice.VendorTaxCode.Length != 10 && invoice.VendorTaxCode.Length != 13)
                if (invoice.VendorTaxCode.Length != 13)
                {
                    errors.AddError("ValidationError", new ErrorMessage("TaxNo_IsInvalid"));
                }

                // Valdiate VendorTaxCode.
                if (string.IsNullOrEmpty(invoice.VendorTaxCode))
                    errors.AddError("ValidationError", new ErrorMessage("VendorTaxCodeRequired"));

                // Validate VendorName.
                if (string.IsNullOrEmpty(invoice.VendorName))
                    errors.AddError("ValidationError", new ErrorMessage("VendorNameRequired"));

                // Validate Street.
                if (string.IsNullOrEmpty(invoice.Street))
                    errors.AddError("ValidationError", new ErrorMessage("StreetRequired"));

                // Validate City.
                if (string.IsNullOrEmpty(invoice.City))
                    errors.AddError("ValidationError", new ErrorMessage("CityRequired"));

                // Validate Country.
                if (string.IsNullOrEmpty(invoice.Country))
                    errors.AddError("ValidationError", new ErrorMessage("CountryRequired"));
                if (ParameterServices.RequireVendorBranchCode)
                {
                    if (string.IsNullOrEmpty(invoice.VendorBranch))
                        errors.AddError("ValidationError", new ErrorMessage("VendorBranchRequired"));
                    else if(invoice.VendorBranch.Length != 5)
                        errors.AddError("ValidationError", new ErrorMessage("VendorBranch_Mustbe_5_digit"));
                    else
                    {
                        int num;
                        bool isNumeric = int.TryParse(invoice.VendorBranch, out num);
                        if (!isNumeric)
                            errors.AddError("ValidationError", new ErrorMessage("VendorBranch_Invalid"));
                    }
                }
            }
            if (!isRepOffice)
            {
                // Validate Vat.
                if (invoice.IsVAT.HasValue && invoice.IsVAT.Value)
                {
                    if (invoice.VatAmount == 0)
                        errors.AddError("ValidationError", new ErrorMessage("VatAmountIsRequired"));
                }

                // Validate WitholdingTax.
                if (invoice.IsWHT.HasValue && invoice.IsWHT.Value)
                {
                    if (invoice.WHTAmount == 0)
                        errors.AddError("ValidationError", new ErrorMessage("WHTAmountIsRequired"));
                    if (workflow != null && !(workflow.CurrentState.Name.Equals(WorkFlowStateFlag.Draft) || workflow.CurrentState.Name.Equals(WorkFlowStateFlag.Hold)))
                    {
                        if (invoice.WHTTypeID2 != null)
                        {
                            if (invoice.WHTTypeID1 == invoice.WHTTypeID2)
                                errors.AddError("ValidationError", new ErrorMessage("WHTTypeIsDuplicate"));
                        }
                    }
                }

                // Validate TotalAmount.
                if (invoice.TotalAmount == 0)
                    errors.AddError("ValidationError", new ErrorMessage("TotalAmountIsRequired"));

                // Validate TotalBaseAmount.
                if (invoice.TotalBaseAmount == 0)
                    errors.AddError("ValidationError", new ErrorMessage("TotalBaseAmountIsRequired"));

                // Validate NetAmount.
                if (invoice.NetAmount == 0)
                    errors.AddError("ValidationError", new ErrorMessage("NetAmountIsRequired"));
            }
            else
            {
                // Validate TotalAmount.
                if (invoice.TotalAmountLocalCurrency == 0)
                    errors.AddError("ValidationError", new ErrorMessage("TotalAmountIsRequired"));

                // Validate TotalBaseAmount.
                if (invoice.TotalBaseAmountLocalCurrency == 0)
                    errors.AddError("ValidationError", new ErrorMessage("TotalBaseAmountIsRequired"));

                // Validate NetAmount.
                if (invoice.NetAmountLocalCurrency == 0)
                    errors.AddError("ValidationError", new ErrorMessage("NetAmountIsRequired"));
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            row.ExpenseID = invoice.Expense.ExpenseID;
            row.InvoiceDocumentType = invoice.InvoiceDocumentType;
            row.InvoiceNo = invoice.InvoiceNo;
            row.Description = invoice.Description;
            row.BranchCode = invoice.BranchCode;

            // Validate InvoiceDate.
            if (!(invoice.InvoiceDate.HasValue))
            {
                row.SetInvoiceDateNull();
            }
            else
            {
                row.InvoiceDate = invoice.InvoiceDate.Value;
            }

            // Assign value to VendorID Column.
            if (invoice.IsVAT == true || invoice.IsWHT == true)
            {
                if (invoice.VendorID.HasValue)
                {
                    row.VendorID = invoice.VendorID.Value;
                }
                else
                {
                    row.SetVendorIDNull();
                }
                row.VendorTaxCode = invoice.VendorTaxCode;
                row.VendorCode = invoice.VendorCode;
                row.VendorBranch = invoice.VendorBranch;
                row.VendorName = invoice.VendorName;
                row.Street = invoice.Street;
                row.City = invoice.City;
                row.Country = invoice.Country;
                row.PostalCode = invoice.PostalCode;
            }
            else
            {
                row.SetVendorIDNull();
                row.VendorTaxCode = string.Empty;
                row.VendorCode = string.Empty;
                row.VendorBranch = string.Empty;
                row.VendorName = string.Empty;
                row.Street = string.Empty;
                row.City = string.Empty;
                row.Country = string.Empty;
                row.PostalCode = string.Empty;
                row.SetInvoiceDateNull();
                row.InvoiceNo = string.Empty;
            }

            if (invoice.IsVAT.HasValue)
                row.isVAT = invoice.IsVAT.Value;
            else
                //row.SetisVATNull();
                row.isVAT = false;

            if (invoice.IsWHT.HasValue)
                row.isWHT = invoice.IsWHT.Value;
            else
                //row.SetisWHTNull();
                row.isWHT = false;

            if (invoice.TaxID.HasValue)
                row.TaxID = invoice.TaxID.Value;
            else
                row.SetTaxIDNull();

            row.VatAmount = (decimal)invoice.VatAmount;

            if (invoice.WHTAmount1.HasValue)
                row.WHTAmount1 = (decimal)invoice.WHTAmount1;

            if (invoice.WHTAmount2.HasValue)
                row.WHTAmount2 = (decimal)invoice.WHTAmount2;

            row.WHTAmount = (decimal)invoice.WHTAmount;

            row.TotalAmount = (decimal)invoice.TotalAmount;
            row.TotalBaseAmount = (decimal)invoice.TotalBaseAmount;
            row.NetAmount = (decimal)invoice.NetAmount;
            row.NonDeductAmount = (decimal)invoice.NonDeductAmount;

            if (invoice.WHTID1.HasValue)
                row.WHTID1 = invoice.WHTID1.Value;

            if (invoice.WHTRate1.HasValue)
                row.WHTRate1 = (decimal)invoice.WHTRate1.Value;

            if (invoice.WHTTypeID1.HasValue)
                row.WHTTypeID1 = invoice.WHTTypeID1.Value;

            if (invoice.BaseAmount1.HasValue)
                row.BaseAmount1 = (decimal)invoice.BaseAmount1.Value;

            if (invoice.WHTAmount1.HasValue)
                row.WHTAmount1 = (decimal)invoice.WHTAmount1.Value;

            if (invoice.WHTID2.HasValue)
                row.WHTID2 = invoice.WHTID2.Value;

            if (invoice.WHTRate2.HasValue)
                row.WHTRate2 = (decimal)invoice.WHTRate2.Value;

            if (invoice.WHTTypeID2.HasValue)
                row.WHTTypeID2 = invoice.WHTTypeID2.Value;

            if (invoice.BaseAmount1.HasValue)
                row.BaseAmount2 = (decimal)invoice.BaseAmount2.Value;

            if (invoice.WHTAmount2.HasValue)
                row.WHTAmount2 = (decimal)invoice.WHTAmount2.Value;

            if (invoice.TotalAmountLocalCurrency.HasValue)
            {
                row.TotalAmountLocalCurrency = (decimal)invoice.TotalAmountLocalCurrency.Value;
            }

            if (invoice.TotalBaseAmountLocalCurrency.HasValue)
            {
                row.TotalBaseAmountLocalCurrency = (decimal)invoice.TotalBaseAmountLocalCurrency.Value;
            }

            if (invoice.NetAmountLocalCurrency.HasValue)
            {
                row.NetAmountLocalCurrency = (decimal)invoice.NetAmountLocalCurrency.Value;
            }

            if (expRow != null)
            {
                if (!expRow.IsExchangeRateForLocalCurrencyNull())
                {
                    row.ExchangeRateForLocalCurrency = expRow.ExchangeRateForLocalCurrency;
                }

                if (!expRow.IsExchangeRateMainToTHBCurrencyNull())
                {
                    row.ExchangeRateMainToTHBCurrency = expRow.ExchangeRateMainToTHBCurrency;
                }
            }

            row.Active = true;
            //row.CreBy = UserAccount.UserID;
            //row.CreDate = DateTime.Now;
            row.UpdBy = UserAccount.UserID;
            row.UpdDate = DateTime.Now;
            row.UpdPgm = UserAccount.CurrentProgramCode;
        }

        public void UpdateInvoiceOnTransactionNonValidation(FnExpenseInvoice invoice, Guid txId)
        {
            ExpenseDataSet ds = (ExpenseDataSet)TransactionService.GetDS(txId);
            ExpenseDataSet.FnExpenseInvoiceRow row = ds.FnExpenseInvoice.FindByInvoiceID(invoice.InvoiceID);

            ExpenseDataSet.FnExpenseDocumentRow expRow = ds.FnExpenseDocument.FindByExpenseID(invoice.Expense.ExpenseID);

            row.ExpenseID = invoice.Expense.ExpenseID;
            row.InvoiceDocumentType = invoice.InvoiceDocumentType;
            row.InvoiceNo = invoice.InvoiceNo;
            row.Description = invoice.Description;
            row.BranchCode = invoice.BranchCode;

            if (!(invoice.InvoiceDate.HasValue))
            {
                row.SetInvoiceDateNull();
            }
            else
            {
                row.InvoiceDate = invoice.InvoiceDate.Value;
            }

            // Assign value to VendorID Column.
            if (invoice.IsVAT == true || invoice.IsWHT == true)
            {
                if (invoice.VendorID.HasValue)
                {
                    row.VendorID = invoice.VendorID.Value;
                }
                else
                {
                    row.SetVendorIDNull();
                }
                row.VendorTaxCode = invoice.VendorTaxCode;
                row.VendorCode = invoice.VendorCode;
                row.VendorBranch = invoice.VendorBranch;
                row.VendorName = invoice.VendorName;
                row.Street = invoice.Street;
                row.City = invoice.City;
                row.Country = invoice.Country;
                row.PostalCode = invoice.PostalCode;
            }
            else
            {
                row.SetVendorIDNull();
                row.VendorTaxCode = string.Empty;
                row.VendorCode = string.Empty;
                row.VendorBranch = string.Empty;
                row.VendorName = string.Empty;
                row.Street = string.Empty;
                row.City = string.Empty;
                row.Country = string.Empty;
                row.PostalCode = string.Empty;
                row.SetInvoiceDateNull();
                row.InvoiceNo = string.Empty;
            }
            if (invoice.IsVAT.HasValue)
                row.isVAT = invoice.IsVAT.Value;
            else
                row.isVAT = false;

            if (invoice.IsWHT.HasValue)
                row.isWHT = invoice.IsWHT.Value;
            else
                row.isWHT = false;

            if (invoice.TaxID.HasValue)
                row.TaxID = invoice.TaxID.Value;
            else
                row.SetTaxIDNull();

            row.VatAmount = (decimal)invoice.VatAmount;

            if (invoice.WHTAmount1.HasValue)
                row.WHTAmount1 = (decimal)invoice.WHTAmount1;

            if (invoice.WHTAmount2.HasValue)
                row.WHTAmount2 = (decimal)invoice.WHTAmount2;

            row.WHTAmount = (decimal)invoice.WHTAmount;

            row.TotalAmount = (decimal)invoice.TotalAmount;
            row.TotalBaseAmount = (decimal)invoice.TotalBaseAmount;
            row.NetAmount = (decimal)invoice.NetAmount;
            row.NonDeductAmount = (decimal)invoice.NonDeductAmount;

            if (invoice.WHTID1.HasValue)
                row.WHTID1 = invoice.WHTID1.Value;

            if (invoice.WHTRate1.HasValue)
                row.WHTRate1 = (decimal)invoice.WHTRate1.Value;

            if (invoice.WHTTypeID1.HasValue)
                row.WHTTypeID1 = invoice.WHTTypeID1.Value;

            if (invoice.BaseAmount1.HasValue)
                row.BaseAmount1 = (decimal)invoice.BaseAmount1.Value;

            if (invoice.WHTAmount1.HasValue)
                row.WHTAmount1 = (decimal)invoice.WHTAmount1.Value;

            if (invoice.WHTID2.HasValue)
                row.WHTID2 = invoice.WHTID2.Value;

            if (invoice.WHTRate2.HasValue)
                row.WHTRate2 = (decimal)invoice.WHTRate2.Value;

            if (invoice.WHTTypeID2.HasValue)
                row.WHTTypeID2 = invoice.WHTTypeID2.Value;

            if (invoice.BaseAmount1.HasValue)
                row.BaseAmount2 = (decimal)invoice.BaseAmount2.Value;

            if (invoice.WHTAmount2.HasValue)
                row.WHTAmount2 = (decimal)invoice.WHTAmount2.Value;

            if (invoice.TotalAmountLocalCurrency.HasValue)
            {
                row.TotalAmountLocalCurrency = (decimal)invoice.TotalAmountLocalCurrency.Value;
            }

            if (invoice.TotalBaseAmountLocalCurrency.HasValue)
            {
                row.TotalBaseAmountLocalCurrency = (decimal)invoice.TotalBaseAmountLocalCurrency.Value;
            }

            if (invoice.NetAmountLocalCurrency.HasValue)
            {
                row.NetAmountLocalCurrency = (decimal)invoice.NetAmountLocalCurrency.Value;
            }

            if (expRow != null)
            {
                if (!expRow.IsExchangeRateForLocalCurrencyNull())
                {
                    row.ExchangeRateForLocalCurrency = expRow.ExchangeRateForLocalCurrency;
                }

                if (!expRow.IsExchangeRateMainToTHBCurrencyNull())
                {
                    row.ExchangeRateMainToTHBCurrency = expRow.ExchangeRateMainToTHBCurrency;
                }
            }

            row.Active = true;
            row.UpdBy = UserAccount.UserID;
            row.UpdDate = DateTime.Now;
            row.UpdPgm = UserAccount.CurrentProgramCode;
        }
        #endregion

        public void PrepareDataToDataset(ExpenseDataSet ds, long expenseId)
        {
            IList<FnExpenseInvoice> invoiceList = ScgeAccountingQueryProvider.FnExpenseInvoiceQuery.GetInvoiceByExpenseID(expenseId);

            foreach (FnExpenseInvoice invoice in invoiceList)
            {
                // Set data to invoice row in Dataset.
                ExpenseDataSet.FnExpenseInvoiceRow invoiceRow = ds.FnExpenseInvoice.NewFnExpenseInvoiceRow();
                invoiceRow.InvoiceID = invoice.InvoiceID;
                invoiceRow.ExpenseID = expenseId;
                invoiceRow.InvoiceDocumentType = invoice.InvoiceDocumentType;
                invoiceRow.InvoiceNo = invoice.InvoiceNo;

                if (invoice.InvoiceDate.HasValue)
                    invoiceRow.InvoiceDate = invoice.InvoiceDate.Value;

                invoiceRow.BranchCode = invoice.BranchCode;

                if (invoice.VendorID.HasValue)
                {
                    invoiceRow.VendorID = invoice.VendorID.Value;
                }
                else
                {
                    invoiceRow.SetVendorIDNull();
                }
                invoiceRow.TotalAmount = (decimal)invoice.TotalAmount;
                invoiceRow.VatAmount = (decimal)invoice.VatAmount;
                invoiceRow.WHTAmount = (decimal)invoice.WHTAmount;
                invoiceRow.NetAmount = (decimal)invoice.NetAmount;
                invoiceRow.Description = invoice.Description;

                if (invoice.IsVAT.HasValue)
                    invoiceRow.isVAT = invoice.IsVAT.Value;
                else
                    invoiceRow.isVAT = false;

                if (invoice.IsWHT.HasValue)
                    invoiceRow.isWHT = invoice.IsWHT.Value;
                else
                    invoiceRow.isWHT = false;

                if (invoice.TaxID.HasValue)
                    invoiceRow.TaxID = invoice.TaxID.Value;

                invoiceRow.NonDeductAmount = (decimal)invoice.NonDeductAmount;
                invoiceRow.TotalBaseAmount = (decimal)invoice.TotalBaseAmount;

                if (invoice.WHTAmount1.HasValue)
                    invoiceRow.WHTAmount1 = (decimal)invoice.WHTAmount1.Value;
                if (invoice.WHTID1.HasValue)
                    invoiceRow.WHTID1 = invoice.WHTID1.Value;
                if (invoice.WHTRate1.HasValue)
                    invoiceRow.WHTRate1 = (decimal)invoice.WHTRate1.Value;
                if (invoice.WHTTypeID1.HasValue)
                    invoiceRow.WHTTypeID1 = invoice.WHTTypeID1.Value;
                if (invoice.BaseAmount1.HasValue)
                    invoiceRow.BaseAmount1 = (decimal)invoice.BaseAmount1.Value;

                if (invoice.WHTAmount2.HasValue)
                    invoiceRow.WHTAmount2 = (decimal)invoice.WHTAmount2.Value;
                if (invoice.WHTID2.HasValue)
                    invoiceRow.WHTID2 = invoice.WHTID2.Value;
                if (invoice.WHTRate2.HasValue)
                    invoiceRow.WHTRate2 = (decimal)invoice.WHTRate2.Value;
                if (invoice.WHTTypeID2.HasValue)
                    invoiceRow.WHTTypeID2 = invoice.WHTTypeID2.Value;
                if (invoice.BaseAmount2.HasValue)
                    invoiceRow.BaseAmount2 = (decimal)invoice.BaseAmount2.Value;

                invoiceRow.VendorCode = invoice.VendorCode;
                invoiceRow.VendorBranch = invoice.VendorBranch;
                invoiceRow.VendorName = invoice.VendorName;
                invoiceRow.VendorTaxCode = invoice.VendorTaxCode;
                invoiceRow.Street = invoice.Street;
                invoiceRow.Country = invoice.Country;
                invoiceRow.City = invoice.City;
                invoiceRow.PostalCode = invoice.PostalCode;

                if (invoice.TotalAmountLocalCurrency.HasValue)
                {
                    invoiceRow.TotalAmountLocalCurrency = (decimal)invoice.TotalAmountLocalCurrency.Value;
                }
                if (invoice.TotalBaseAmountLocalCurrency.HasValue)
                {
                    invoiceRow.TotalBaseAmountLocalCurrency = (decimal)invoice.TotalBaseAmountLocalCurrency.Value;
                }
                if (invoice.NetAmountLocalCurrency.HasValue)
                {
                    invoiceRow.NetAmountLocalCurrency = (decimal)invoice.NetAmountLocalCurrency.Value;
                }

                if (invoice.ExchangeRateForLocalCurrency.HasValue)
                {
                    invoiceRow.ExchangeRateForLocalCurrency = (decimal)invoice.ExchangeRateForLocalCurrency.Value;
                }

                if (invoice.ExchangeRateMainToTHBCurrency.HasValue)
                {
                    invoiceRow.ExchangeRateMainToTHBCurrency = (decimal)invoice.ExchangeRateMainToTHBCurrency.Value;
                }

                invoiceRow.Active = invoice.Active;
                invoiceRow.CreBy = invoice.CreBy;
                invoiceRow.CreDate = invoice.CreDate;
                invoiceRow.UpdBy = invoice.UpdBy;
                invoiceRow.UpdDate = invoice.UpdDate;
                invoiceRow.UpdPgm = invoice.UpdPgm;

                // Add invoice row to documentDataset.
                ds.FnExpenseInvoice.AddFnExpenseInvoiceRow(invoiceRow);

                // Prepare Data to FnExpenseInvoiceItem Datatable. 
                FnExpenseInvoiceItemService.PrepareDataToDataset(ds, invoiceRow.InvoiceID);
            }
        }
        public void SaveExpenseInvoice(Guid txId, long expenseId)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(txId);

            ScgeAccountingDaoProvider.FnExpenseInvoiceDao.Persist(expDS.FnExpenseInvoice);
            ScgeAccountingDaoProvider.FnExpenseInvoiceItemDao.Persist(expDS.FnExpenseInvoiceItem);
        }

        #region ExportPayroll
        public string ExportFilePayroll(string strMonth, string strYear, string comCode, string Ordinal)
        {
            DateTime date;
            string result = "";
            #region validate

            ValidationErrors errors = new ValidationErrors();
            if (string.IsNullOrEmpty(comCode))
            {
                errors.AddError("Export.Error", new ErrorMessage("Company is required."));
                throw new ServiceValidationException(errors);
            }
            try
            {
                int iMonth = int.Parse(strMonth);
                int iyear = int.Parse(strYear);
                date = new DateTime(iyear, iMonth, 01);
            }
            catch (FormatException)
            {
                errors.AddError("Export.Error", new ErrorMessage("Invalid input."));
                throw new ValidationException(errors);

            }
            #endregion


            IList<ExportPayroll> PayrollList = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExportPayrollList(date, comCode, Ordinal);
            string stringDate;
            //Next two variable for stringdate Mechanism.
            int count = 0;
            //string currentUser = "";
            foreach (ExportPayroll item in PayrollList)
            {
                #region stringDate Mechanism

                count++;

                if (item.PayrollType.ToUpper().Equals(PayrollType.Mileage))
                    stringDate = date.ToString("ddMMyyyy", new CultureInfo("en-US"));
                else
                    stringDate = date.AddDays(1).ToString("ddMMyyyy", new CultureInfo("en-US"));
                #endregion

                #region define variable
                string textLine = "{0}{1}{2}{3}{4}{5}                         {6}       ";
                string line1;

                #endregion

                #region format representation
                //format ExportPayroll representation
                try
                {
                    item.CompanyCode = item.CompanyCode.Substring(0, 4);
                }
                catch (ArgumentOutOfRangeException)
                {
                    item.CompanyCode = item.CompanyCode.PadLeft(4);
                }

                try
                {
                    item.EmployeeCode = item.EmployeeCode.Insert(4, "-");
                    item.EmployeeCode = item.EmployeeCode.Substring(0, 11);

                }
                catch (ArgumentOutOfRangeException)
                {
                    item.EmployeeCode = item.EmployeeCode.PadLeft(11);
                }

                try
                {
                    item.CostCenterCode = string.Empty;
                }
                catch (ArgumentOutOfRangeException)
                {
                    item.CostCenterCode = item.CostCenterCode.PadLeft(10);
                }
                #endregion

                line1 = string.Format(textLine, item.CompanyCode, stringDate, item.EmployeeCode, item.CostCenterCode.PadLeft(10), item.wagecode, item.totalAmount.ToString().PadRight(16), stringDate);
                result += line1;
                if (count != PayrollList.Count)
                    result += Environment.NewLine;

            }
            return result;
        }
        #endregion

        #region Validate
        /// <summary>
        /// For Check the total base amount in FnExpenseInvoice table.
        /// That must equal with summary of Amount + NonDeductAmount of all item in this invoice in FnExpenseInvoiceItem.
        /// </summary>
        /// <param name="txId">TransactionID for GetDS from TransactionService</param>
        /// <param name="invoiceId"></param>
        /// <param name="invoice"></param>
        public void ValidateTotalBaseAmount(Guid txId, long invoiceId, FnExpenseInvoice invoice)
        {
            ValidationErrors errors = new ValidationErrors();
            ExpenseDataSet expenseDs = (ExpenseDataSet)TransactionService.GetDS(txId);
            double summaryInvItemAmount = 0;

            DataRow[] drArr = expenseDs.FnExpenseInvoiceItem.Select(string.Format("{0} = '{1}'", expenseDs.FnExpenseInvoiceItem.InvoiceIDColumn, invoiceId));
            foreach (ExpenseDataSet.FnExpenseInvoiceItemRow row in drArr)
            {
                double amount = Convert.ToDouble(row.Amount);
                double nonDeductAmount = Convert.ToDouble(row.NonDeductAmount);
                double totalItemAmount = amount + nonDeductAmount;

                summaryInvItemAmount += totalItemAmount;
            }

            if (invoice.TotalBaseAmount != summaryInvItemAmount)
            {
                errors.AddError("ValidationError", new ErrorMessage("TotalBaseAmountIncorrect"));
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
        }
        public void ValidateCalculateInvoice(FnExpenseInvoice invoice)
        {
            ValidationErrors errors = new ValidationErrors();
            // Validate TotalAmount.
            if ((invoice.TotalAmount == 0))
            {
                errors.AddError("ValidationError", new ErrorMessage("TotalAmountIsRequired"));
            }

            // Validate WHTRate1.
            if (!(invoice.WHTRate1.HasValue) && (invoice.WHTRate1.Value == 0))
            {
                errors.AddError("ValidationError", new ErrorMessage("WHTRate1IsRequired"));
            }

            // Validate BaseAmount1.
            if (!(invoice.BaseAmount1.HasValue) && (invoice.BaseAmount1 == 0))
            {
                errors.AddError("ValidationError", new ErrorMessage("BaseAmount1IsRequired"));
            }

            // Validate WHTAmount1.
            if (!(invoice.WHTAmount1.HasValue) && (invoice.WHTAmount1.Value == 0))
            {
                errors.AddError("ValidationError", new ErrorMessage("WHTAmount1IsRequired"));
            }

            // Validate BaseAmount2 and WHTRate2.
            if ((invoice.BaseAmount2.HasValue) && (invoice.BaseAmount2.Value != 0))
            {
                if (!(invoice.WHTRate2.HasValue) && (invoice.WHTRate2.Value == 0))
                {
                    errors.AddError("ValidationError", new ErrorMessage("WHTRate2IsRequired"));
                }
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
        }
        #endregion

        #region For Calculate
        public double CalculateVATAverage(double amount, double? rateNonDeduct)
        {
            double nonDeductAmount = 0.0;
            if (rateNonDeduct.HasValue)
            {
                nonDeductAmount = (amount * rateNonDeduct.Value) / 100;
            }
            return (double)Math.Round(Convert.ToDecimal(nonDeductAmount), 2, MidpointRounding.AwayFromZero);
        }
        public double CalculateVATAmount(double baseAmount, double? rate)
        {
            double vatAmount = 0.0;
            if (rate.HasValue)
            {
                vatAmount = (baseAmount * rate.Value) / 100;
            }
            else
            {
                DbTax tax = ScgDbQueryProvider.DbTaxQuery.FindbyTaxCode(ParameterServices.DefaultTaxCode);
                if (tax != null)
                    vatAmount = (baseAmount * tax.Rate) / 100;
            }

            return (double)Math.Round(Convert.ToDecimal(vatAmount), 2, MidpointRounding.AwayFromZero);
        }
        public double CalculateWHTAmount(double[] baseAmount, double[] whtRate)
        {
            double whtAmount = 0.0;
            for (int i = 0; i < baseAmount.Length; i++)
                whtAmount += (double)Math.Round(Convert.ToDecimal((baseAmount[i] * whtRate[i]) / 100), 2, MidpointRounding.AwayFromZero);

            return whtAmount;
        }
        public double CalculateNetAmount(double baseAmount, double vatAmount, double whtAmount)
        {
            double netAmount = 0.0;
            netAmount = (baseAmount + vatAmount) - whtAmount;

            return netAmount;
        }

        public double CalculateWHTAmount(double baseAmount, double whtRate)
        {
            double whtAmount = (baseAmount * (whtRate / 100));

            return (double)Math.Round(Convert.ToDecimal(whtAmount), 2, MidpointRounding.AwayFromZero);
        }

        public double CalculateBaseAmount(Guid txID, long invoiceID)
        {
            ExpenseDataSet expDs = (ExpenseDataSet)TransactionService.GetDS(txID);

            string filter = String.Format("InvoiceID = {0}", invoiceID);
            DataRow[] rows = expDs.FnExpenseInvoiceItem.Select(filter);
            double total = 0.0;

            foreach (DataRow row in rows)
            {
                FnExpenseInvoiceItem invoiceItem = new FnExpenseInvoiceItem();
                invoiceItem.LoadFromDataRow(row);

                total += invoiceItem.Amount.Value;
            }

            return (double)Math.Round(Convert.ToDecimal(total), 2, MidpointRounding.AwayFromZero);
        }

        #endregion


        public void UpdateInvoiceByExpenseID(Guid txID, long expenseId)
        {
            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(txID);

            string filter = String.Format("ExpenseID = {0}", expenseId);
            DataRow[] rows = expDS.FnExpenseInvoice.Select(filter);

            foreach (DataRow row in rows)
            {
                FnExpenseInvoice invoice = new FnExpenseInvoice();
                invoice.LoadFromDataRow(row);

                this.UpdateInvoiceOnTransaction(invoice, txID);
            }
        }

        public void DeleteMileageInvoice(Guid txID)
        {
            ExpenseDataSet expDs = (ExpenseDataSet)TransactionService.GetDS(txID);
            string filter = String.Format("InvoiceDocumentType = '{0}'", InvoiceType.Mileage);
            DataRow[] rows = expDs.FnExpenseInvoice.Select(filter);
            //long invoiceId = 0;
            if (rows.Length > 0)
            {
                foreach (DataRow inv in rows)
                {
                    inv.Delete();
                }
            }
        }
    }
}

