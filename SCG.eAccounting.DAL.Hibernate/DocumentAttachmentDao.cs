using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using SS.Standard.Data.NHibernate.Dao;

using SCG.eAccounting.DTO;
using SCG.eAccounting.DAL;
using SCG.eAccounting.DTO.DataSet;
using NHibernate;
using NHibernate.Expression;
using NHibernate.Transform;
using SS.Standard.Data.NHibernate.QueryCreator;

namespace SCG.eAccounting.DAL.Hibernate
{
    public partial class DocumentAttachmentDao : NHibernateDaoBase<DocumentAttachment, long>, IDocumentAttachmentDao
    {
        public DocumentAttachmentDao()
        {
        }
    
        #region IDocumentAttachmentDao Members

        public ICriteria FindByActive(DocumentAttachment documentAttachment)
        {
            ICriteria criteria = GetCurrentSession().CreateCriteria(typeof(DocumentAttachment), "da");
            criteria.Add(Expression.Eq("da.Active", true));
            return criteria;
        }
        public void DeleteByDocumentId(long attachmentid)
        {
            GetCurrentSession()
            .Delete(" FROM DocumentAttachment da WHERE da.AttachmentID = :attachmentid ",
            new object[] { attachmentid },
            new NHibernate.Type.IType[] { NHibernateUtil.Int64 });
        }

        #endregion

        #region Save DocumentAttachment.
        public void Persist(DataTable dtDocumentAttachment)
        {
			NHibernateAdapter<DocumentAttachment, long> adapter = new NHibernateAdapter<DocumentAttachment, long>();
			adapter.UpdateChange(dtDocumentAttachment, ScgeAccountingDaoProvider.DocumentAttachmentDao);
			
			#region Old Comment By AO 07/04/2009
			//DataTable insertTable = dtDocumentAttachment.GetChanges(DataRowState.Added);
			//DataTable updateTable = dtDocumentAttachment.GetChanges(DataRowState.Modified);
			//DataTable deleteTable = dtDocumentAttachment.GetChanges(DataRowState.Deleted);

			//#region Persist
			//// Delete DocumentAttachment.
			//if (deleteTable != null)
			//{
			//    foreach (DataRow documentAttachmentRow in deleteTable.Rows)
			//    {
			//        long attachmentID = Convert.ToInt64(documentAttachmentRow["AttachmentID", DataRowVersion.Original].ToString());
			//        DocumentAttachment documentAttachment = new DocumentAttachment(attachmentID);
			//        this.Delete(documentAttachment);
			//    }
			//}

			//#region Update DocumentAttachment.
			//#region Update TA
			//if (updateTable != null)
			//{
			//    if (updateTable is TADocumentDataSet.DocumentAttachmentDataTable)
			//    {
			//        foreach (TADocumentDataSet.DocumentAttachmentRow documentAttachmentRow in (TADocumentDataSet.DocumentAttachmentDataTable)updateTable)
			//        {
			//            DocumentAttachment documentAttachment = new DocumentAttachment(documentAttachmentRow.AttachmentID);
			//            this.SaveOrUpdate(documentAttachment);
			//        }
			//    }
			//}
			//#endregion

			//#region Update Advance
			//if (updateTable != null)
			//{
			//    if (updateTable is AdvanceDataSet.DocumentAttachmentDataTable)
			//    {
			//        foreach (AdvanceDataSet.DocumentAttachmentRow documentAttachmentRow in (AdvanceDataSet.DocumentAttachmentDataTable)updateTable)
			//        {
			//            DocumentAttachment documentAttachment = new DocumentAttachment(documentAttachmentRow.AttachmentID);
			//            this.SaveOrUpdate(documentAttachment);
			//        }
			//    }
			//}
			//#endregion

			//#region Update Remittance
			//if (updateTable != null)
			//{
			//    if (updateTable is FnRemittanceDataset.DocumentAttachmentDataTable)
			//    {
			//        foreach (FnRemittanceDataset.DocumentAttachmentRow documentAttachmentRow in (FnRemittanceDataset.DocumentAttachmentDataTable)updateTable)
			//        {
			//            DocumentAttachment documentAttachment = new DocumentAttachment(documentAttachmentRow.AttachmentID);
			//            this.SaveOrUpdate(documentAttachment);
			//        }
			//    }
			//}
			//#endregion

			//#region Update Expense
			//if (updateTable != null)
			//{
			//    if (updateTable is ExpenseDataSet.DocumentAttachmentDataTable)
			//    {
			//        foreach (ExpenseDataSet.DocumentAttachmentRow documentAttachmentRow in (ExpenseDataSet.DocumentAttachmentDataTable)updateTable)
			//        {
			//            DocumentAttachment documentAttachment = new DocumentAttachment(documentAttachmentRow.AttachmentID);
			//            this.SaveOrUpdate(documentAttachment);
			//        }
			//    }
			//}
			//#endregion
			//#endregion

			//#region Insert DocumentAttachment.
			//#region Insert TA
			//if (insertTable != null)
			//{
			//    if (insertTable is TADocumentDataSet.DocumentAttachmentDataTable)
			//    {
			//        foreach (TADocumentDataSet.DocumentAttachmentRow documentAttachmentRow in (TADocumentDataSet.DocumentAttachmentDataTable)insertTable)
			//        {
			//            DocumentAttachment documentAttachement = new DocumentAttachment(documentAttachmentRow.AttachmentID);
			//            this.Save(documentAttachement);
			//        }
			//    }
			//}
			//#endregion

			//#region Insert Advance
			//if (insertTable != null)
			//{
			//    if (insertTable is AdvanceDataSet.DocumentAttachmentDataTable)
			//    {
			//        foreach (AdvanceDataSet.DocumentAttachmentRow documentAttachmentRow in (AdvanceDataSet.DocumentAttachmentDataTable)insertTable)
			//        {
			//            DocumentAttachment documentAttachement = new DocumentAttachment(documentAttachmentRow.AttachmentID);
			//            this.Save(documentAttachement);
			//        }
			//    }
			//}
			//#endregion

			//#region Insert Remittance
			//if (insertTable != null)
			//{
			//    if (insertTable is FnRemittanceDataset.DocumentAttachmentDataTable)
			//    {
			//        foreach (FnRemittanceDataset.DocumentAttachmentRow documentAttachmentRow in (FnRemittanceDataset.DocumentAttachmentDataTable)insertTable)
			//        {
			//            DocumentAttachment documentAttachement = new DocumentAttachment(documentAttachmentRow.AttachmentID);
			//            this.Save(documentAttachement);
			//        }
			//    }
			//}
			//#endregion

			//#region Insert Expense
			//if (insertTable != null)
			//{
			//    if (insertTable is ExpenseDataSet.DocumentAttachmentDataTable)
			//    {
			//        foreach (ExpenseDataSet.DocumentAttachmentRow documentAttachmentRow in (ExpenseDataSet.DocumentAttachmentDataTable)insertTable)
			//        {
			//            DocumentAttachment documentAttachement = new DocumentAttachment(documentAttachmentRow.AttachmentID);
			//            this.Save(documentAttachement);
			//        }
			//    }
			//}
			//#endregion
			//#endregion
			//#endregion
			#endregion
        }
        #endregion

    }
}
