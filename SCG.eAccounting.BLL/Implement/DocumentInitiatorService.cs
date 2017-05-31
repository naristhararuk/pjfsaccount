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


namespace SCG.eAccounting.BLL.Implement
{
    public class DocumentInitiatorService : ServiceBase<DocumentInitiator, long>, IDocumentInitiatorService
    {

        public IUserAccount UserAccount { get; set; }
        public ISuUserQuery SuUserQuery { get; set; }
        public ISCGDocumentQuery SCGDocumentQuery { get; set; }
        public ITransactionService TransactionService { get; set; }

        #region Overrid Method
        public override IDao<DocumentInitiator, long> GetBaseDao()
        {
            return ScgeAccountingDaoProvider.DocumentInitiatorDao;
        }
        #endregion
        public void UpdateInitiator(long DocumentID, IList<DocumentInitiatorLang> DocumentInitiatorList, IList<DocumentInitiatorLang> DocumentInitiatorDeleteList)
        {
            long documentID = DocumentID;
            IList<DocumentInitiator> Initiator = ScgeAccountingDaoProvider.DocumentInitiatorDao.FindByDocumentID(documentID);
            DocumentInitiator newInitiator;

            if (Initiator.Count > 0)
            {
                #region Delete
                if (DocumentInitiatorDeleteList != null)
                {
                    foreach (DocumentInitiatorLang item in DocumentInitiatorDeleteList)
                    {
                        if (item.InitiatorID > 0)
                        {
                            DocumentInitiator doc = ScgeAccountingDaoProvider.DocumentInitiatorDao.FindProxyByIdentity(item.InitiatorID);
                            if (doc.InitiatorID > 0)
                                ScgeAccountingDaoProvider.DocumentInitiatorDao.Delete(doc);
                        }
                    }
                }
                #endregion Delete

                #region Insert & Update
                foreach (DocumentInitiatorLang item in DocumentInitiatorList)
                {
                    //select old initiator
                    var oldInitiatorItem = from oldInitiatorObject in Initiator
                                           where oldInitiatorObject.UserID.Userid == item.UserID
                                           select oldInitiatorObject;


                    if (oldInitiatorItem.Count() > 0)
                    {
                        //new initiator
                        foreach (DocumentInitiator itemDocumentInitiator in oldInitiatorItem)
                        {
                            itemDocumentInitiator.Seq = item.Seq;
                            itemDocumentInitiator.InitiatorType = item.InitiatorType;
                            itemDocumentInitiator.UpdBy = UserAccount.UserID;
                            itemDocumentInitiator.UpdDate = DateTime.Now;
                            itemDocumentInitiator.UpdPgm = UserAccount.CurrentProgramCode;
                            ScgeAccountingDaoProvider.DocumentInitiatorDao.Update(itemDocumentInitiator);
                        }
                    }
                    else
                    {
                        // old initiator
                        newInitiator = new DocumentInitiator();
                        newInitiator.Seq = item.Seq;
                        newInitiator.Active = true;
                        newInitiator.CreBy = UserAccount.UserID;
                        newInitiator.CreDate = DateTime.Now;
                        newInitiator.DocumentID = SCGDocumentQuery.GetSCGDocumentByDocumentID(documentID);
                        newInitiator.UserID = SuUserQuery.FindByIdentity(item.UserID);
                        newInitiator.UpdDate = DateTime.Now;
                        newInitiator.UpdPgm = UserAccount.CurrentProgramCode;
                        ScgeAccountingDaoProvider.DocumentInitiatorDao.SaveOrUpdate(newInitiator);
                    }
                }
                #endregion

            }
            else
            {
                #region Insert only case dont have Initiator on this DocumentID
                foreach (DocumentInitiatorLang item in DocumentInitiatorList)
                {
                    newInitiator = new DocumentInitiator();
                    newInitiator.Seq = item.Seq;
                    newInitiator.Active = true;
                    newInitiator.CreBy = UserAccount.UserID;
                    newInitiator.CreDate = DateTime.Now;
                    newInitiator.DocumentID = SCGDocumentQuery.GetSCGDocumentByDocumentID(documentID);
                    newInitiator.UserID = SuUserQuery.FindByIdentity(item.UserID);
                    newInitiator.UpdDate = DateTime.Now;
                    newInitiator.UpdPgm = UserAccount.CurrentProgramCode;
                    newInitiator.UpdBy = UserAccount.UserID;
                    ScgeAccountingDaoProvider.DocumentInitiatorDao.SaveOrUpdate(newInitiator);

                }
                #endregion
            }


        }

        public void UpdateInitiatorSequence(int OldSequence, int NewSequence)
        {
            int SeqOld = OldSequence + 1;
            int SeqNew = NewSequence + 1;

            //get old data before change seq  like switch seq between old and new
            DocumentInitiator InitiatorReorderMoveHost = ScgeAccountingDaoProvider.DocumentInitiatorDao.FindByInitiatorSequence(Convert.ToInt16(SeqOld));
            DocumentInitiator InitiatorReorderMoveGuest = ScgeAccountingDaoProvider.DocumentInitiatorDao.FindByInitiatorSequence(Convert.ToInt16(SeqNew));

            InitiatorReorderMoveHost.Seq = Convert.ToInt16(SeqNew);
            InitiatorReorderMoveHost.UpdDate = DateTime.Now;
            InitiatorReorderMoveHost.UpdBy = UserAccount.UserID;
            InitiatorReorderMoveHost.UpdPgm = UserAccount.CurrentProgramCode;

            InitiatorReorderMoveGuest.Seq = Convert.ToInt16(SeqOld);
            InitiatorReorderMoveGuest.UpdDate = DateTime.Now;
            InitiatorReorderMoveGuest.UpdBy = UserAccount.UserID;
            InitiatorReorderMoveGuest.UpdPgm = UserAccount.CurrentProgramCode;


            GetBaseDao().Update(InitiatorReorderMoveHost);
            GetBaseDao().Update(InitiatorReorderMoveGuest);




        }

        public void AddInitiator(DocumentInitiator Initiator)
        {
            //DocumentInitiator Initiator = ScgeAccountingDaoProvider.DocumentInitiatorDao.FindByIdentity(InitiatorID);
            Initiator.CreBy = UserAccount.UserID;
            Initiator.CreDate = DateTime.Now;
            Initiator.UpdBy = UserAccount.UserID;
            Initiator.UpdDate = DateTime.Now;
            Initiator.Active = true;
            Initiator.UpdPgm = UserAccount.CurrentProgramCode;

            ScgeAccountingDaoProvider.DocumentInitiatorDao.Save(Initiator);
        }

        public void DeleteInitiator(long DocumentID, long InitiatorID)
        {

            short delSeq = 0;
            int seqUpdate = 0;
            int seqTemp = 0;

            IList<DocumentInitiator> AllDocumentInitiator = this.FindByDocumentID(DocumentID);

            var initiator = from abc in AllDocumentInitiator
                            where abc.InitiatorID == InitiatorID
                            select abc;


            foreach (DocumentInitiator item in initiator)
            {
                delSeq = item.Seq;
            }

            foreach (DocumentInitiator item in AllDocumentInitiator)
            {
                seqUpdate = item.Seq;
                if (seqUpdate > 1 && seqUpdate > delSeq)
                {
                    seqTemp = seqUpdate - 1;
                    item.Seq = Convert.ToInt16(seqTemp);
                    item.UpdDate = DateTime.Now;
                    item.UpdBy = UserAccount.UserID;
                    item.UpdPgm = UserAccount.CurrentProgramCode;
                }
            }
            foreach (DocumentInitiator item in AllDocumentInitiator)
            {
                //if (item.Seq != delSeq)
                seqUpdate = item.Seq;
                //if (seqUpdate > 1 && seqUpdate >= delSeq)
                if (item.Seq != delSeq)
                {
                    ScgeAccountingDaoProvider.DocumentInitiatorDao.Update(item);
                }

            }

            DocumentInitiator Initiator = ScgeAccountingDaoProvider.DocumentInitiatorDao.FindByIdentity(InitiatorID);
            ScgeAccountingDaoProvider.DocumentInitiatorDao.Delete(Initiator);
        }
        public IList<DocumentInitiator> FindByDocumentID(long DocumentID)
        {
            return ScgeAccountingDaoProvider.DocumentInitiatorDao.FindByDocumentID(DocumentID);
        }

        #region Prepare Data To Dataset
        public void PrepareDataToDataset(DataSet documentDataset, long documentID)
        {
            IList<DocumentInitiator> documentInitiatorList = ScgeAccountingQueryProvider.DocumentInitiatorQuery.GetDocumentInitiatorByDocumentID(documentID);
            foreach (DocumentInitiator initiator in documentInitiatorList)
            {
                // Set data to document initiator datatable.
                DataRow initiatorRow = documentDataset.Tables["DocumentInitiator"].NewRow();
                initiatorRow["InitiatorID"] = initiator.InitiatorID;
                if (initiator.DocumentID != null)
                {
                    initiatorRow["DocumentID"] = initiator.DocumentID.DocumentID;
                }
                initiatorRow["InitiatorType"] = initiator.InitiatorType;
                initiatorRow["Seq"] = initiator.Seq;
                if (initiator.UserID != null)
                {
                    initiatorRow["UserID"] = initiator.UserID.Userid;
                }
                initiatorRow["DoApprove"] = initiator.DoApprove;
                initiatorRow["Active"] = initiator.Active;
                initiatorRow["CreBy"] = initiator.CreBy;
                initiatorRow["CreDate"] = initiator.CreDate;
                initiatorRow["UpdBy"] = initiator.UpdBy;
                initiatorRow["UpdDate"] = initiator.UpdDate;
                initiatorRow["UpdPgm"] = initiator.UpdPgm;

                // Add document initiator to datatable budgetDocumentDS.
                documentDataset.Tables["DocumentInitiator"].Rows.Add(initiatorRow);
            }
        }
        /// <summary>
        /// Create Advance From TA
        /// Initiator of TA = Initiator of Advance
        /// </summary>
        /// <param name="documentDataset">AdvanceDocumentDataSet</param>
        /// <param name="documentID">documentid = documentid of tadocument</param>
        public void PrepareDataToDatasetAdvance(DataSet documentDataset, long documentID)
        {
            IList<DocumentInitiator> documentInitiatorList = ScgeAccountingQueryProvider.DocumentInitiatorQuery.GetDocumentInitiatorByDocumentID(documentID);
            foreach (DocumentInitiator initiator in documentInitiatorList)
            {
                // Set data to document initiator datatable.
                DataRow initiatorRow = documentDataset.Tables["DocumentInitiator"].NewRow();
                //initiatorRow["InitiatorID"] = initiator.InitiatorID;
                initiatorRow["DocumentID"] = -1;
                initiatorRow["InitiatorType"] = initiator.InitiatorType;
                initiatorRow["Seq"] = initiator.Seq;
                if (initiator.UserID != null)
                {
                    initiatorRow["UserID"] = initiator.UserID.Userid;
                }
                initiatorRow["DoApprove"] = false;
                initiatorRow["Active"] = initiator.Active;
                initiatorRow["CreBy"] = UserAccount.UserID;
                initiatorRow["CreDate"] = DateTime.Now;
                initiatorRow["UpdBy"] = UserAccount.UserID;
                initiatorRow["UpdDate"] = DateTime.Now;
                initiatorRow["UpdPgm"] = UserAccount.CurrentProgramCode;

                // Add document initiator to datatable budgetDocumentDS.
                documentDataset.Tables["DocumentInitiator"].Rows.Add(initiatorRow);
            }
        }
        #endregion

        #region Insert/Update/Delete to Database.
        public void InsertDocumentInitiator(Guid txID, long documentID)
        {
            DataSet ds = TransactionService.GetDS(txID);
            SCGDocument document = ScgeAccountingQueryProvider.SCGDocumentQuery.FindProxyByIdentity(documentID);
            DataTable insertTable = ds.Tables["DocumentInitiator"].GetChanges(DataRowState.Added);

            if (insertTable != null)
            {
                foreach (DataRow row in insertTable.Rows)
                {
                    if (Convert.ToInt64(row["InitiatorID"].ToString()) < 0)
                    {
                        DocumentInitiator documentInitiator = new DocumentInitiator();
                        documentInitiator.DocumentID = document;
                        documentInitiator.Seq = (short)row["InitiatorSeq"];
                        documentInitiator.UserID = QueryProvider.SuUserQuery.FindProxyByIdentity((long)row["UserID"]);
                        documentInitiator.InitiatorType = row["InitiatorType"].ToString();
                        documentInitiator.DoApprove = false;
                        documentInitiator.Active = (bool)row["Active"];
                        documentInitiator.CreBy = (long)row["CreBy"];
                        documentInitiator.CreDate = DateTime.Parse(row["CreDate"].ToString());
                        documentInitiator.UpdBy = (long)row["UpdBy"];
                        documentInitiator.UpdDate = DateTime.Parse(row["UpdDate"].ToString());
                        documentInitiator.UpdPgm = row["UpdPgm"].ToString();

                        ScgeAccountingDaoProvider.DocumentInitiatorDao.Save(documentInitiator);
                    }
                }
            }
        }
        public void UpdateDocumentInitiator(Guid txID, long documentID)
        {
            DataSet ds = TransactionService.GetDS(txID);
            SCGDocument document = ScgeAccountingQueryProvider.SCGDocumentQuery.FindProxyByIdentity(documentID);
            DataTable updateTable = ds.Tables["DocumentInitiator"].GetChanges(DataRowState.Modified);

            if (updateTable != null)
            {
                foreach (DataRow row in updateTable.Rows)
                {
                    long documentInitiatorID = Convert.ToInt64(row["InitiatorID"].ToString());
                    DocumentInitiator documentInitiator;
                    if (documentInitiatorID < 0)
                    {
                        documentInitiator = new DocumentInitiator();
                    }
                    else
                    {
                        documentInitiator = ScgeAccountingQueryProvider.DocumentInitiatorQuery.FindProxyByIdentity(documentInitiatorID);
                    }

                    documentInitiator.DocumentID = document;
                    documentInitiator.Seq = (short)row["InitiatorSeq"];
                    documentInitiator.UserID = QueryProvider.SuUserQuery.FindProxyByIdentity((long)row["UserID"]);
                    documentInitiator.InitiatorType = row["InitiatorType"].ToString();
                    documentInitiator.DoApprove = false;
                    documentInitiator.Active = (bool)row["Active"];
                    documentInitiator.CreBy = (long)row["CreBy"];
                    documentInitiator.CreDate = DateTime.Parse(row["CreDate"].ToString());
                    documentInitiator.UpdBy = (long)row["UpdBy"];
                    documentInitiator.UpdDate = DateTime.Parse(row["UpdDate"].ToString());
                    documentInitiator.UpdPgm = row["UpdPgm"].ToString();

                    if (documentInitiatorID < 0)
                    {
                        ScgeAccountingDaoProvider.DocumentInitiatorDao.Save(documentInitiator);
                    }
                    else
                    {
                        ScgeAccountingDaoProvider.DocumentInitiatorDao.SaveOrUpdate(documentInitiator);
                    }
                }
            }
        }
        public void DeleteDocumentInitiator(Guid txID)
        {
            DataSet ds = TransactionService.GetDS(txID);
            DataTable deleteTable = ds.Tables["DocumentInitiator"].GetChanges(DataRowState.Deleted);

            if (deleteTable != null)
            {
                foreach (DataRow row in deleteTable.Rows)
                {
                    long documentInitiatorID = Convert.ToInt64(row["InitiatorID", DataRowVersion.Original].ToString());
                    if (documentInitiatorID > 0)
                    {
                        DocumentInitiator documentInitiator = ScgeAccountingQueryProvider.DocumentInitiatorQuery.FindProxyByIdentity(documentInitiatorID);
                        if (documentInitiator != null)
                        {
                            ScgeAccountingDaoProvider.DocumentInitiatorDao.Delete(documentInitiator);
                        }
                    }
                }
            }
        }
        #endregion

        public void SaveDocumentInitiator(Guid txID, long documentID)
        {
            DataSet ds = TransactionService.GetDS(txID);

            // Insert, Update, Delete DocumentInitiator.
            ScgeAccountingDaoProvider.DocumentInitiatorDao.Persist(ds.Tables["DocumentInitiator"]);
        }
        public void UpdateDocumentInitiatorWhenOverRole(IList<DocumentInitiator> documentInitiators)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (documentInitiators.Count == 0)
            {
                errors.AddError("WorkFlow.Error", new Spring.Validation.ErrorMessage("WorkFlow_ValidateOverrole_Skip"));
            }
            else
            {
                foreach (DocumentInitiator initiator in documentInitiators)
                {
                    if (initiator.IsSkip && string.IsNullOrEmpty(initiator.SkipReason))
                    {
                        errors.AddError("WorkFlow.Error", new Spring.Validation.ErrorMessage("WorkFlow_ValidateOverrole_SkipReasonIsRequired", new object[]{initiator.Seq}));
                    }
                }
            }

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            foreach (DocumentInitiator initiator in documentInitiators)
            {
                ScgeAccountingDaoProvider.DocumentInitiatorDao.SaveOrUpdate(initiator);
            }
        }

        public void ValidateDocumentInitiator(Guid txID, long documentID)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            DataSet ds = TransactionService.GetDS(txID);
            DataTable dt = ds.Tables["DocumentInitiator"];
            DataRow[] intiators = dt.Select();

            if (intiators.Count() == 0)
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("InitiatorIsRequired"));

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
        }
    }
}
