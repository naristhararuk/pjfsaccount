using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using NHibernate.Expression;
using NHibernate.Transform;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.QueryCreator;

using SCG.eAccounting.DTO;
using System.Data;
using SCG.eAccounting.DTO.DataSet;
using SCG.eAccounting.DTO.ValueObject;


namespace SCG.eAccounting.DAL.Hibernate
{
    public partial class SCGDocumentDao : NHibernateDaoBase<SCGDocument, long>, ISCGDocumentDao
    {
        public SCGDocumentDao()
        {
        }
        public long Persist(DataTable dtDocument)
        {
            NHibernateAdapter<SCGDocument, long> adapter = new NHibernateAdapter<SCGDocument, long>();
            adapter.UpdateChange(dtDocument, ScgeAccountingDaoProvider.SCGDocumentDao);
            #region Old 24-March-2009
            //Old 24-March-2009
            //DataTable updateTable = dtDocument.GetChanges(DataRowState.Modified);
            //DataTable insertTable = dtDocument.GetChanges(DataRowState.Added);
            //long documentID = 0;

            //#region Update
            //if (updateTable != null)
            //{
            //    #region Remittance
            //    if (updateTable is FnRemittanceDataset.FnRemittanceDataTable)
            //    {
            //        if (updateTable.Rows[0] is FnRemittanceDataset.DocumentRow)
            //        {
            //            FnRemittanceDataset.DocumentRow row = (FnRemittanceDataset.DocumentRow)updateTable.Rows[0];

            //            SCGDocument document = this.FindByIdentity(row.DocumentID);
            //            document.CompanyID = new SCG.DB.DTO.DbCompany(row.CompanyID);
            //            document.CreatorID = new SS.SU.DTO.SuUser(row.CreatorID);
            //            document.RequesterID = new SS.SU.DTO.SuUser(row.RequesterID);
            //            if (row.ApproverID != null)
            //            {
            //                document.ApproverID = new SS.SU.DTO.SuUser(row.ApproverID);
            //            }
            //            else
            //            {
            //                document.ApproverID = null;
            //            }
            //            document.DocumentNo = row.DocumentNo;
            //            document.DocumentDate = row.DocumentDate;
            //            document.DocumentType = new SS.Standard.WorkFlow.DTO.DocumentType(row.DocumentTypeID);
            //            document.Subject = row.Subject;
            //            document.Memo = row.Memo;
            //            if (row.ReceiverID != null)
            //            {
            //                document.ReceiverID = new SS.SU.DTO.SuUser(row.ReceiverID);
            //            }
            //            else
            //            {
            //                document.ReceiverID = null;
            //            }
            //            document.Active = row.Active;
            //            document.CreBy = row.CreBy;
            //            document.CreDate = row.CreDate;
            //            document.UpdBy = row.UpdBy;
            //            document.UpdDate = row.UpdDate;
            //            document.UpdPgm = row.UpdPgm;

            //            this.SaveOrUpdate(document);
            //            documentID = document.DocumentID;
            //        }
            //    }
            //    #endregion

            //    #region TADocument
            //    if (updateTable is TADocumentDataSet.TADocumentDataTable)
            //    {
            //        if (updateTable.Rows[0] is TADocumentDataSet.DocumentRow)
            //        {
            //            TADocumentDataSet.DocumentRow row = (TADocumentDataSet.DocumentRow)updateTable.Rows[0];

            //            SCGDocument document = this.FindByIdentity(row.DocumentID);
            //            document.CompanyID = new SCG.DB.DTO.DbCompany(row.CompanyID);
            //            document.CreatorID = new SS.SU.DTO.SuUser(row.CreatorID);
            //            document.RequesterID = new SS.SU.DTO.SuUser(row.RequesterID);
            //            if (row.ApproverID != null)
            //            {
            //                document.ApproverID = new SS.SU.DTO.SuUser(row.ApproverID);
            //            }
            //            else
            //            {
            //                document.ApproverID = null;
            //            }
            //            document.DocumentNo = row.DocumentNo;
            //            document.DocumentDate = row.DocumentDate;
            //            document.DocumentType = new SS.Standard.WorkFlow.DTO.DocumentType(row.DocumentTypeID);
            //            document.Subject = row.Subject;
            //            document.Memo = row.Memo;
            //            if (row.ReceiverID != null)
            //            {
            //                document.ReceiverID = new SS.SU.DTO.SuUser(row.ReceiverID);
            //            }
            //            else
            //            {
            //                document.ReceiverID = null;
            //            }
            //            document.Active = row.Active;
            //            document.CreBy = row.CreBy;
            //            document.CreDate = row.CreDate;
            //            document.UpdBy = row.UpdBy;
            //            document.UpdDate = row.UpdDate;
            //            document.UpdPgm = row.UpdPgm;

            //            this.SaveOrUpdate(document);
            //            documentID = document.DocumentID;
            //        }
            //    }
            //    #endregion

            //    #region AdvanceDocument
            //    if (updateTable is AdvanceDataSet.AvAdvanceDocumentDataTable)
            //    {
            //        if (updateTable.Rows[0] is AdvanceDataSet.DocumentRow)
            //        {
            //            AdvanceDataSet.DocumentRow row = (AdvanceDataSet.DocumentRow)updateTable.Rows[0];

            //            SCGDocument document = this.FindByIdentity(row.DocumentID);
            //            document.CompanyID = new SCG.DB.DTO.DbCompany(row.CompanyID);
            //            document.CreatorID = new SS.SU.DTO.SuUser(row.CreatorID);
            //            document.RequesterID = new SS.SU.DTO.SuUser(row.RequesterID);
            //            if (row.ApproverID != null)
            //            {
            //                document.ApproverID = new SS.SU.DTO.SuUser(row.ApproverID);
            //            }
            //            else
            //            {
            //                document.ApproverID = null;
            //            }
            //            document.DocumentNo = row.DocumentNo;
            //            document.DocumentDate = row.DocumentDate;
            //            document.DocumentType = new SS.Standard.WorkFlow.DTO.DocumentType(row.DocumentTypeID);
            //            document.Subject = row.Subject;
            //            document.Memo = row.Memo;
            //            if (row.ReceiverID != null)
            //            {
            //                document.ReceiverID = new SS.SU.DTO.SuUser(row.ReceiverID);
            //            }
            //            else
            //            {
            //                document.ReceiverID = null;
            //            }
            //            document.Active = row.Active;
            //            document.CreBy = row.CreBy;
            //            document.CreDate = row.CreDate;
            //            document.UpdBy = row.UpdBy;
            //            document.UpdDate = row.UpdDate;
            //            document.UpdPgm = row.UpdPgm;

            //            this.SaveOrUpdate(document);
            //            documentID = document.DocumentID;
            //        }
            //    }
            //    #endregion

            //    #region ExpenseDataset Wait from Meaw
            //    if (updateTable is ExpenseDataSet.FnExpenseDocumentDataTable)
            //    {
            //        if (updateTable.Rows[0] is ExpenseDataSet.DocumentRow)
            //        {
            //            ExpenseDataSet.DocumentRow row = (ExpenseDataSet.DocumentRow)updateTable.Rows[0];

            //            SCGDocument document = this.FindByIdentity(row.DocumentID);
            //            document.CompanyID = new SCG.DB.DTO.DbCompany(row.CompanyID);
            //            document.CreatorID = new SS.SU.DTO.SuUser(row.CreatorID);
            //            document.RequesterID = new SS.SU.DTO.SuUser(row.RequesterID);
            //            if (row.ApproverID != null)
            //            {
            //                document.ApproverID = new SS.SU.DTO.SuUser(row.ApproverID);
            //            }
            //            else
            //            {
            //                document.ApproverID = null;
            //            }
            //            document.DocumentNo = row.DocumentNo;
            //            document.DocumentDate = row.DocumentDate;
            //            document.DocumentType = new SS.Standard.WorkFlow.DTO.DocumentType(row.DocumentTypeID);
            //            document.Subject = row.Subject;
            //            document.Memo = row.Memo;
            //            if (row.ReceiverID != null)
            //            {
            //                document.ReceiverID = new SS.SU.DTO.SuUser(row.ReceiverID);
            //            }
            //            else
            //            {
            //                document.ReceiverID = null;
            //            }
            //            document.Active = row.Active;
            //            document.CreBy = row.CreBy;
            //            document.CreDate = row.CreDate;
            //            document.UpdBy = row.UpdBy;
            //            document.UpdDate = row.UpdDate;
            //            document.UpdPgm = row.UpdPgm;

            //            this.SaveOrUpdate(document);
            //            documentID = document.DocumentID;
            //        }
            //    }
            //    #endregion
            //}
            //#endregion

            //#region Insert
            //if (insertTable != null)
            //{
            //    #region Remittance
            //    if (insertTable is FnRemittanceDataset.FnRemittanceDataTable)
            //    {
            //        if (insertTable.Rows[0] is FnRemittanceDataset.DocumentRow)
            //        {
            //            FnRemittanceDataset.DocumentRow documentRow = (FnRemittanceDataset.DocumentRow)insertTable.Rows[0];
            //            SCGDocument document = new SCGDocument(documentRow);
            //            documentID = this.Save(document);
            //        }

            //        #region Update New DocumentID to Dataset
            //        dtDocument.Columns["DocumentID"].ReadOnly = false;
            //        dtDocument.Rows[0].BeginEdit();
            //        dtDocument.Rows[0]["DocumentID"] = documentID;
            //        dtDocument.Rows[0].EndEdit();
            //        dtDocument.Columns["DocumentID"].ReadOnly = true;
            //        #endregion
            //    }
            //    #endregion

            //    #region TADocument
            //    if (insertTable is TADocumentDataSet.TADocumentDataTable)
            //    {
            //        if (insertTable.Rows[0] is TADocumentDataSet.DocumentRow)
            //        {
            //            TADocumentDataSet.DocumentRow documentRow = (TADocumentDataSet.DocumentRow)insertTable.Rows[0];
            //            SCGDocument document = new SCGDocument(documentRow);
            //            documentID = this.Save(document);
            //        }

            //        #region Update New DocumentID to Dataset
            //        dtDocument.Columns["DocumentID"].ReadOnly = false;
            //        dtDocument.Rows[0].BeginEdit();
            //        dtDocument.Rows[0]["DocumentID"] = documentID;
            //        dtDocument.Rows[0].EndEdit();
            //        dtDocument.Columns["DocumentID"].ReadOnly = true;
            //        #endregion
            //    }
            //    #endregion

            //    #region Advance
            //    if (insertTable is AdvanceDataSet.AvAdvanceDocumentDataTable)
            //    {
            //        if (insertTable.Rows[0] is AdvanceDataSet.DocumentRow)
            //        {
            //            AdvanceDataSet.DocumentRow documentRow = (AdvanceDataSet.DocumentRow)insertTable.Rows[0];
            //            SCGDocument document = new SCGDocument(documentRow);
            //            documentID = this.Save(document);
            //        }

            //        #region Update New DocumentID to Dataset
            //        dtDocument.Columns["DocumentID"].ReadOnly = false;
            //        dtDocument.Rows[0].BeginEdit();
            //        dtDocument.Rows[0]["DocumentID"] = documentID;
            //        dtDocument.Rows[0].EndEdit();
            //        dtDocument.Columns["DocumentID"].ReadOnly = true;
            //        #endregion
            //    }
            //    #endregion

            //    #region Expense
            //    if (insertTable is ExpenseDataSet.FnExpenseDocumentDataTable)
            //    {
            //        if (insertTable.Rows[0] is ExpenseDataSet.DocumentRow)
            //        {
            //            ExpenseDataSet.DocumentRow documentRow = (ExpenseDataSet.DocumentRow)insertTable.Rows[0];
            //            SCGDocument document = new SCGDocument(documentRow);
            //            documentID = this.Save(document);
            //        }

            //        #region Update New DocumentID to Dataset
            //        dtDocument.Columns["DocumentID"].ReadOnly = false;
            //        dtDocument.Rows[0].BeginEdit();
            //        dtDocument.Rows[0]["DocumentID"] = documentID;
            //        dtDocument.Rows[0].EndEdit();
            //        dtDocument.Columns["DocumentID"].ReadOnly = true;
            //    }
            //        #endregion


            //    #endregion
            //}

            //return documentID;

            //#endregion
            #endregion
            return dtDocument.Rows[0].Field<long>(dtDocument.Columns["DocumentID"]);
        }
        public void UpdateMarkDocument(IList<ReimbursementReportValueObj> obj)
        {
            foreach (ReimbursementReportValueObj item in obj)
            {
                StringBuilder sqlBuilder = new StringBuilder();
                QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
                sqlBuilder.Append("UPDATE Document ");
                sqlBuilder.Append("SET Mark = :Mark ");
                sqlBuilder.Append("WHERE DocumentID = :DocumentID ");
                queryParameterBuilder.AddParameterData("Mark", typeof(string), String.Format("{0}", item.Mark));
                queryParameterBuilder.AddParameterData("DocumentID", typeof(string), String.Format("{0}", item.DocumentID));
                ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
                queryParameterBuilder.FillParameters(query);
                query.AddScalar("count", NHibernateUtil.Int32);
                query.UniqueResult();
            }
        }
    }
}

