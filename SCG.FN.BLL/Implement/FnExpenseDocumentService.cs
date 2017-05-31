using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;
using SS.Standard.Utilities;

using SCG.FN.DAL;
using SCG.FN.DTO;
using SCG.FN.DTO.DataSet;
using SCG.FN.BLL;
using SCG.FN.DTO.ValueObject;
using System.Data;
using SCG.eAccounting.BLL;
using System.Web;

namespace SCG.FN.BLL.Implement
{
    public partial class FnExpenseDocumentService : ServiceBase<FnExpenseDocument, long>, IFnExpenseDocumentService
    {
        public ITransactionService TransactionService { get; set; }

        public override IDao<FnExpenseDocument, long> GetBaseDao()
        {
            return DaoProvider.FnExpenseDocumentDao;
        }
        public DataSet PrepareExpenseDS()
        {
            ExpenseDataSet ds = new ExpenseDataSet();
            ds.EnforceConstraints = false;
            ds.Document.DocumentIDColumn.AutoIncrement = true;
            ds.Document.DocumentIDColumn.AutoIncrementSeed = -1;
            ds.Document.DocumentIDColumn.AutoIncrementStep = -1;

            ds.FnExpenseDocument.ExpenseIDColumn.AutoIncrement = true;
            ds.FnExpenseDocument.ExpenseIDColumn.AutoIncrementSeed = -1;
            ds.FnExpenseDocument.ExpenseIDColumn.AutoIncrementStep = -1;

            ds.FnExpenseAdvance.AdvanceIDColumn.AutoIncrement = true;
            ds.FnExpenseAdvance.AdvanceIDColumn.AutoIncrementSeed = -1;
            ds.FnExpenseAdvance.AdvanceIDColumn.AutoIncrementStep = -1;

            ds.FnExpenseInvoice.InvoiceIDColumn.AutoIncrement = true;
            ds.FnExpenseInvoice.InvoiceIDColumn.AutoIncrementSeed = -1;
            ds.FnExpenseInvoice.InvoiceIDColumn.AutoIncrementStep = -1;

            ds.FnExpenseInvoiceItem.InvoiceItemIDColumn.AutoIncrement = true;
            ds.FnExpenseInvoiceItem.InvoiceItemIDColumn.AutoIncrementSeed = -1;
            ds.FnExpenseInvoiceItem.InvoiceItemIDColumn.AutoIncrementStep = -1;

            ds.FnExpenseMileage.ExpenseMileageIDColumn.AutoIncrement = true;
            ds.FnExpenseMileage.ExpenseMileageIDColumn.AutoIncrementSeed = -1;
            ds.FnExpenseMileage.ExpenseMileageIDColumn.AutoIncrementStep = -1;

            ds.FnExpenseMileageItem.ExpenseMileageItemIDColumn.AutoIncrement = true;
            ds.FnExpenseMileageItem.ExpenseMileageItemIDColumn.AutoIncrementSeed = -1;
            ds.FnExpenseMileageItem.ExpenseMileageItemIDColumn.AutoIncrementStep = -1;

            ds.FnExpensePerdiem.ExpensePerdiemIDColumn.AutoIncrement = true;
            ds.FnExpensePerdiem.ExpensePerdiemIDColumn.AutoIncrementSeed = -1;
            ds.FnExpensePerdiem.ExpensePerdiemIDColumn.AutoIncrementStep = -1;

            ds.FnExpensePerdiemItem.PerdiemItemIDColumn.AutoIncrement = true;
            ds.FnExpensePerdiemItem.PerdiemItemIDColumn.AutoIncrementSeed = -1;
            ds.FnExpensePerdiemItem.PerdiemItemIDColumn.AutoIncrementStep = -1;

            ds.DocumentAttachment.AttachmentIDColumn.AutoIncrement = true;
            ds.DocumentAttachment.AttachmentIDColumn.AutoIncrementSeed = -1;
            ds.DocumentAttachment.AttachmentIDColumn.AutoIncrementStep = -1;

            ds.DocumentInitiator.InitiatorIDColumn.AutoIncrement = true;
            ds.DocumentInitiator.InitiatorIDColumn.AutoIncrementSeed = -1;
            ds.DocumentInitiator.InitiatorIDColumn.AutoIncrementStep = -1;

            ds.WorkFlow.WorkFlowIDColumn.AutoIncrement = true;
            ds.WorkFlow.WorkFlowIDColumn.AutoIncrementSeed = -1;
            ds.WorkFlow.WorkFlowIDColumn.AutoIncrementStep = -1;

            return ds.Clone();
        }
        public DataSet PrepareExpenseDS(long documentId)
        {
            ExpenseDataSet expDs = (ExpenseDataSet)this.PrepareExpenseDS();


            return expDs;
        }
        public Guid BeginExpenseTransaction()
        {
            ExpenseDataSet ds = (ExpenseDataSet)PrepareExpenseDS();
            Guid txId = TransactionService.Begin(ds);

            return txId;
        }

        public long AddNewExpenseDocumentOnTransaction(FnExpenseDocument exp, Guid txId)
        {
            ExpenseDataSet ds = (ExpenseDataSet)TransactionService.GetDS(txId);
            ExpenseDataSet.DocumentRow docRow = ds.Document.NewDocumentRow();


            ExpenseDataSet.FnExpenseDocumentRow expRow = ds.FnExpenseDocument.NewFnExpenseDocumentRow();
            expRow.DocumentID = docRow.DocumentID;
            ds.FnExpenseDocument.AddFnExpenseDocumentRow(expRow);

            return expRow.ExpenseID;
        }
        public void UpdateExpenseDocumentOnTransaction(FnExpenseDocument exp, Guid txId)
        {
            ExpenseDataSet ds = (ExpenseDataSet)TransactionService.GetDS(txId);

            if (exp.Document != null)
            {
                ExpenseDataSet.DocumentRow docRow = ds.Document.FindByDocumentID(exp.Document.DocumentID);
                docRow.BeginEdit();
                docRow.DocumentNo = exp.Document.DocumentNo;
                //docRow.DocumentStatus = exp.Document.DocumentStatus;
                docRow.Subject = exp.Document.Subject;
                docRow.Memo = exp.Document.Memo;
                if (exp.Document.DocumentType != null)
                {
                    docRow.DocumentTypeID = exp.Document.DocumentType.DocumentTypeID;
                }
                if (exp.Document.CompanyID != null)
                {
                    docRow.CompanyID = exp.Document.CompanyID.CompanyID;
                }
                if (exp.Document.CreatorID != null)
                {
                    docRow.CreatorID = exp.Document.CreatorID.Userid;
                }
                if (exp.Document.RequesterID != null)
                {
                    docRow.RequesterID = exp.Document.RequesterID.Userid;
                }
                if (exp.Document.ReceiverID != null)
                {
                    docRow.ReceiverID = exp.Document.ReceiverID.Userid;
                }
                if (exp.Document.ApproverID != null)
                {
                    docRow.ApproverID = exp.Document.ApproverID.Userid;
                }

                docRow.Active = exp.Document.Active;
                docRow.CreDate = exp.CreDate;
                docRow.CreBy = exp.CreBy;
                docRow.UpdDate = exp.UpdDate;
                docRow.UpdBy = exp.UpdBy;
                docRow.UpdPgm = exp.UpdPgm;
                
                docRow.EndEdit();
                docRow.AcceptChanges();
            }
            ExpenseDataSet.FnExpenseDocumentRow expRow = ds.FnExpenseDocument.FindByExpenseID(exp.ExpenseID);
            expRow.BeginEdit();

            if(exp.ServiceTeam != null)
            {
                expRow.ServiceTeamID = exp.ServiceTeam.ServiceTeamID;
            }
            if (exp.PB != null)
            {
                expRow.PBID = exp.PB.Pbid;
            }
            expRow.PaymentType = exp.PaymentType;
            expRow.TotalAdvance = (decimal)exp.TotalAdvance;
            expRow.TotalExpense = (decimal)exp.TotalExpense;
            expRow.CreDate = exp.CreDate;
            expRow.CreBy = exp.CreBy;
            expRow.UpdDate = exp.UpdDate;
            expRow.UpdBy = exp.UpdBy;
            expRow.UpdPgm = exp.UpdPgm;

            expRow.EndEdit();
            expRow.AcceptChanges();
        }

        public long AddExpenseDocument(FnExpenseDocument exp, Guid txId)
        {

            return 0;
        }
    }
}
