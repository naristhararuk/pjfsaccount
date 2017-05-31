using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using NHibernate.Expression;
using NHibernate.Transform;

using SS.Standard.Data.NHibernate.Query;
using SS.Standard.Data.NHibernate.QueryDao;
using SS.Standard.Data.NHibernate.QueryCreator;

using SCG.eAccounting.DTO;
using SCG.eAccounting.DTO.ValueObject;
using SCG.eAccounting.Query;

namespace SCG.eAccounting.Query.Hibernate
{
    public class AvAdvanceDocumentQuery : NHibernateQueryBase<AvAdvanceDocument, long>, IAvAdvanceDocumentQuery
    {
        #region query Advance
        #region ISQLQuery FindByAdvanceCriteria(string advanceNo, string description, bool isCount, string sortExpression)
        //public ISQLQuery FindByAdvanceCriteria(string advanceNo, string description, bool isCount, string sortExpression)
        public ISQLQuery FindByAdvanceCriteria(Advance advance, bool isCount, string sortExpression)
        {
            ISQLQuery query;
            StringBuilder sqlBuilder = new StringBuilder();
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            if (!isCount)
            {
                //Comment at 29-March-2009
                //sqlBuilder.Append(" SELECT Document.DocumentNo as DocumentNo");
                //sqlBuilder.Append(" , AvAdvanceDocument.Description as Description");
                //sqlBuilder.Append(" , AvAdvanceDocument.DueDateOfRemittance as DueDateOfRemittance");
                //sqlBuilder.Append(" , AvAdvanceItem.Amount as Amount");
                //sqlBuilder.Append(" , AvAdvanceDocument.DocumentID as DocumentID");
                //sqlBuilder.Append(" , AvAdvanceDocument.AdvanceID as AdvanceID");
                //sqlBuilder.Append(" FROM Document ");
                //sqlBuilder.Append(" INNER JOIN AvAdvanceDocument");
                //sqlBuilder.Append(" ON Document.DocumentID = AvAdvanceDocument.DocumentID");
                //sqlBuilder.Append(" INNER JOIN FnCashier");
                //sqlBuilder.Append(" ON FnCashier.CashierID = AvAdvanceDocument.CashierID");
                //sqlBuilder.Append(" INNER JOIN AvAdvanceItem");
                //sqlBuilder.Append(" ON AvAdvanceItem.AdvanceID = AvAdvanceDocument.AdvanceID");
                //sqlBuilder.Append(" WHERE  Document.DocumentNo like :advanceNo ");
                //sqlBuilder.Append(" AND AvAdvanceDocument.Description  like :description ");

                sqlBuilder.Append(" SELECT Document.DocumentNo as DocumentNo");
                sqlBuilder.Append(" , Document.Subject as Description");
                sqlBuilder.Append(" , Document.RequesterID as RequesterID , Document.ReceiverID as ReceiverID ");
                sqlBuilder.Append(" , AvAdvanceDocument.RequestDateOfRemittance as RequestDateOfRemittance");
                sqlBuilder.Append(" , AvAdvanceDocument.Amount as Amount");
                sqlBuilder.Append(" , AvAdvanceDocument.DocumentID as DocumentID");
                sqlBuilder.Append(" , AvAdvanceDocument.AdvanceID as AdvanceID");
                sqlBuilder.Append(" , AvAdvanceDocument.MainCurrencyAmount as MainCurrencyAmount");
                sqlBuilder.Append(" , AvAdvanceDocument.LocalCurrencyAmount as LocalCurrencyAmount");
                sqlBuilder.Append(" , req.UserName as RequesterName");
                sqlBuilder.Append(" , rec.UserName as ReceiverName");
            }
            else
            {
                //sqlBuilder.Append(" SELECT COUNT(AdvanceID) AS DocumentCount FROM AvAdvanceDocument ");
                sqlBuilder.Append(" SELECT COUNT(*) AS DocumentCount ");
            }

            sqlBuilder.Append(" FROM Document ");
            sqlBuilder.Append(" INNER JOIN AvAdvanceDocument ");
            sqlBuilder.Append(" ON Document.DocumentID = AvAdvanceDocument.DocumentID ");
            if (advance.CompanyID != null)
            {
                sqlBuilder.Append(" and Document.CompanyID =:companyID ");
                queryParameterBuilder.AddParameterData("companyID", typeof(long), advance.CompanyID);
            }
            sqlBuilder.Append(" INNER JOIN WorkFlow on WorkFlow.DocumentID = Document.DocumentID ");
            sqlBuilder.Append(" INNER JOIN SuUser req on req.UserID = Document.RequesterID ");
            sqlBuilder.Append(" INNER JOIN SuUser rec on rec.UserID = Document.ReceiverID ");
            //sqlBuilder.Append(" INNER JOIN DBPB ");
            //sqlBuilder.Append(" ON DBPB.PBID = AvAdvanceDocument.PBID ");
            //sqlBuilder.Append(" INNER JOIN AvAdvanceItem ");
            //sqlBuilder.Append(" ON AvAdvanceItem.AdvanceID = AvAdvanceDocument.AdvanceID ");
            sqlBuilder.Append(" WHERE Document.DocumentNo like :advanceNo ");
            sqlBuilder.Append(" AND Document.Subject like :description ");
            sqlBuilder.Append(" AND AvAdvanceDocument.AdvanceType =:advanceType ");
            if (advance.CurrentUserID != null && advance.CurrentUserID > 0)
            {
                sqlBuilder.Append(" AND (Document.RequesterID = :currentUserID or Document.CreatorID= :currentUserID) ");
                queryParameterBuilder.AddParameterData("currentUserID", typeof(long), advance.CurrentUserID);
            }
            if (advance.RequesterID != null)
            {
                sqlBuilder.Append(" AND Document.RequesterID =:requesterID ");
                queryParameterBuilder.AddParameterData("requesterID", typeof(long), advance.RequesterID);
            }
            if (advance.TADocumentID != null)
            {
                sqlBuilder.Append(" AND AvAdvanceDocument.TADocumentID = :taDocumentID ");
                queryParameterBuilder.AddParameterData("taDocumentID", typeof(long), advance.TADocumentID);
            }
            sqlBuilder.Append(" AND WorkFlow.CurrentState = 25 "); // Current state=25 is OutStanding.
            sqlBuilder.Append(" AND AvAdvanceDocument.AdvanceID not in ");
            sqlBuilder.Append(" ( ");
            sqlBuilder.Append(" SELECT expAdv.AdvanceID ");
            sqlBuilder.Append(" FROM FnExpenseAdvance expAdv ");
            sqlBuilder.Append(" INNER JOIN FnExpenseDocument expDoc ");
            sqlBuilder.Append(" ON expDoc.ExpenseID = expAdv.ExpenseID ");
            sqlBuilder.Append(" INNER JOIN WorkFlow wf ");
            sqlBuilder.Append(" ON wf.DocumentID = expDoc.DocumentID ");
            sqlBuilder.Append(" INNER JOIN WorkFlowState wfs ");
            sqlBuilder.Append(" ON wfs.WorkFlowStateID = wf.CurrentState ");
            sqlBuilder.Append(" WHERE wfs.Name <> 'Cancel' ");
            sqlBuilder.Append(" ) ");
            sqlBuilder.Append(" AND AvAdvanceDocument.AdvanceID not in ");
            sqlBuilder.Append(" ( ");
            sqlBuilder.Append(" SELECT rmAdv.AdvanceID ");
            sqlBuilder.Append(" FROM FnRemittanceAdvance rmAdv ");
            sqlBuilder.Append(" INNER JOIN FnRemittance rmDoc ");
            sqlBuilder.Append(" ON rmDoc.RemittanceID = rmAdv.RemittanceID ");
            sqlBuilder.Append(" INNER JOIN WorkFlow rmwf ");
            sqlBuilder.Append(" ON rmwf.DocumentID = rmDoc.DocumentID ");
            sqlBuilder.Append(" INNER JOIN WorkFlowState rmwfs ");
            sqlBuilder.Append(" ON rmwfs.WorkFlowStateID = rmwf.CurrentState ");
            sqlBuilder.Append(" WHERE rmwfs.Name <> 'Cancel' ");
            sqlBuilder.Append(" ) ");
            
            if (advance.IsRepOffice)
            {
                if (advance.MainCurrencyID.HasValue && advance.MainCurrencyID.Value != 0)
                {
                    sqlBuilder.Append(" And AvAdvanceDocument.MainCurrencyID = :mainCurrencyID");
                    queryParameterBuilder.AddParameterData("mainCurrencyID", typeof(long), advance.MainCurrencyID);
                }
                if (advance.PBID.HasValue && advance.PBID.Value != 0)
                {
                    sqlBuilder.Append(" And AvAdvanceDocument.PBID = :pBID");
                    queryParameterBuilder.AddParameterData("pBID", typeof(long), advance.PBID);
                }

                sqlBuilder.Append(" And AvAdvanceDocument.IsRepOffice = :isRepOffice");
                queryParameterBuilder.AddParameterData("isRepOffice", typeof(bool), advance.IsRepOffice);
            }
            if (!isCount)
            {
                if (string.IsNullOrEmpty(sortExpression))
                    sqlBuilder.AppendLine(" ORDER BY Document.DocumentNo");
                else
                    sqlBuilder.AppendLine(string.Format(" ORDER BY {0}", sortExpression));
            }

            query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());

            //if (advance.CompanyID != null)
            //{
            //    queryParameterBuilder.AddParameterData("companyID", typeof(long), advance.CompanyID);
            //}
            queryParameterBuilder.AddParameterData("advanceNo", typeof(string), string.Format("%{0}%", advance.DocumentNo));
            queryParameterBuilder.AddParameterData("description", typeof(string), string.Format("%{0}%", advance.Description));
            queryParameterBuilder.AddParameterData("advanceType", typeof(string), advance.AdvanceType);
            //if (advance.RequesterID != null)
            //{
            //    queryParameterBuilder.AddParameterData("requesterID", typeof(long), advance.RequesterID);
            //}
            //if (advance.TADocumentID != null)
            //{
            //    queryParameterBuilder.AddParameterData("taDocumentID", typeof(long), advance.TADocumentID);
            //}

            queryParameterBuilder.FillParameters(query);

            if (!isCount)
            {
                query.AddScalar("DocumentID", NHibernateUtil.Int64);
                query.AddScalar("AdvanceID", NHibernateUtil.Int64);
                query.AddScalar("DocumentNo", NHibernateUtil.String);
                query.AddScalar("Description", NHibernateUtil.String);
                query.AddScalar("RequesterID", NHibernateUtil.Int64);
                query.AddScalar("RequesterName", NHibernateUtil.String);
                query.AddScalar("ReceiverID", NHibernateUtil.Int64);
                query.AddScalar("ReceiverName", NHibernateUtil.String);
                query.AddScalar("RequestDateOfRemittance", NHibernateUtil.DateTime);
                query.AddScalar("Amount", NHibernateUtil.Double);
                query.AddScalar("MainCurrencyAmount", NHibernateUtil.Double);
                query.AddScalar("LocalCurrencyAmount", NHibernateUtil.Double);
                query.SetResultTransformer(Transformers.AliasToBean(typeof(Advance)));
            }
            else
            {
                query.AddScalar("DocumentCount", NHibernateUtil.Int32);
                query.UniqueResult();
            }
            return query;
        }
        #endregion
        #region IList<Advance> GetAdvanceList(string advanceNo, string description, int firstResult, int maxResult, string sortExpression)
        public IList<Advance> GetAdvanceList(Advance advance, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<Advance>(ScgeAccountingQueryProvider.AvAdvanceDocumentQuery, "FindByAdvanceCriteria", new object[] { advance, false, sortExpression }, firstResult, maxResult, sortExpression);
        }
        #endregion

        #region ISQLQuery FindAdvanceRelateWithRemittanceButNotInExpenseByAdvanceCriteria(string advanceNo, string description, bool isCount, string sortExpression)
        public ISQLQuery FindAdvanceRelateWithRemittanceButNotInExpenseByAdvanceCriteria(Advance advance, bool isCount, string sortExpression)
        {
            ISQLQuery query;
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            StringBuilder sqlBuilder = new StringBuilder();
            if (!isCount)
            {
                sqlBuilder.Append(" SELECT Document.DocumentNo as DocumentNo");
                sqlBuilder.Append(" , Document.Subject as Description");
                sqlBuilder.Append(" , Document.RequesterID as RequesterID , Document.ReceiverID as ReceiverID ");
                sqlBuilder.Append(" , AvAdvanceDocument.RequestDateOfRemittance as RequestDateOfRemittance");
                sqlBuilder.Append(" , AvAdvanceDocument.Amount as Amount");
                sqlBuilder.Append(" , AvAdvanceDocument.DocumentID as DocumentID");
                sqlBuilder.Append(" , AvAdvanceDocument.AdvanceID as AdvanceID");
                sqlBuilder.Append(" , AvAdvanceDocument.MainCurrencyAmount as MainCurrencyAmount");
                sqlBuilder.Append(" , AvAdvanceDocument.LocalCurrencyAmount as LocalCurrencyAmount");
                sqlBuilder.Append(" , req.UserName as RequesterName");
                sqlBuilder.Append(" , rec.UserName as ReceiverName");
            }
            else
            {
                //sqlBuilder.Append(" SELECT COUNT(AdvanceID) AS DocumentCount FROM AvAdvanceDocument ");
                sqlBuilder.Append(" SELECT COUNT(*) AS DocumentCount ");
            }

            sqlBuilder.Append(" FROM Document ");
            sqlBuilder.Append(" INNER JOIN AvAdvanceDocument ");
            sqlBuilder.Append(" ON Document.DocumentID = AvAdvanceDocument.DocumentID ");
            if (advance.CompanyID != null)
            {
                sqlBuilder.Append(" and Document.CompanyID =:companyID ");
            }
            sqlBuilder.Append(" INNER JOIN WorkFlow on WorkFlow.DocumentID = Document.DocumentID ");
            sqlBuilder.Append(" INNER JOIN SuUser req on req.UserID = Document.RequesterID ");
            sqlBuilder.Append(" INNER JOIN SuUser rec on rec.UserID = Document.ReceiverID ");
            sqlBuilder.Append(" WHERE Document.DocumentNo like :advanceNo ");
            sqlBuilder.Append(" AND Document.Subject like :description ");
            sqlBuilder.Append(" AND AvAdvanceDocument.AdvanceType =:advanceType ");
            if (advance.CurrentUserID != null && advance.CurrentUserID > 0)
            {
                sqlBuilder.Append(" AND (Document.RequesterID = :currentUserID or Document.CreatorID= :currentUserID) ");
                queryParameterBuilder.AddParameterData("currentUserID", typeof(long), advance.CurrentUserID);
            }
            if (advance.RequesterID != null)
            {
                sqlBuilder.Append(" AND Document.RequesterID =:requesterID ");
                queryParameterBuilder.AddParameterData("requesterID", typeof(long), advance.RequesterID);
            }
            if (advance.TADocumentID != null)
            {
                sqlBuilder.Append(" AND AvAdvanceDocument.TADocumentID = :taDocumentID ");
                queryParameterBuilder.AddParameterData("taDocumentID", typeof(long), advance.TADocumentID);
            }
            sqlBuilder.Append(" AND WorkFlow.CurrentState = 25 "); // Current state=25 is OutStanding.
            sqlBuilder.Append(" AND AvAdvanceDocument.AdvanceID not in ");
            sqlBuilder.Append(" ( ");
            sqlBuilder.Append(" SELECT expAdv.AdvanceID ");
            sqlBuilder.Append(" FROM FnExpenseAdvance expAdv ");
            sqlBuilder.Append(" INNER JOIN FnExpenseDocument expDoc ");
            sqlBuilder.Append(" ON expDoc.ExpenseID = expAdv.ExpenseID ");
            sqlBuilder.Append(" INNER JOIN WorkFlow wf ");
            sqlBuilder.Append(" ON wf.DocumentID = expDoc.DocumentID ");
            sqlBuilder.Append(" INNER JOIN WorkFlowState wfs ");
            sqlBuilder.Append(" ON wfs.WorkFlowStateID = wf.CurrentState ");
            sqlBuilder.Append(" WHERE wfs.Name <> 'Cancel' ");
            sqlBuilder.Append(" ) ");

            if (advance.AdvanceType.Equals("FR"))
            {
                sqlBuilder.Append(@" AND	(
	                AvAdvanceDocument.AdvanceID in
	                    (
	                        SELECT remittedAdv.AdvanceID FROM FnRemittanceAdvance remittedAdv
	                        INNER JOIN FnRemittance remitted ON remitted.RemittanceID = remittedAdv.RemittanceID
	                        INNER JOIN Document doc ON remitted.DocumentID = doc.DocumentID
	                        INNER JOIN Workflow wf ON wf.DocumentID = remitted.DocumentID
	                        INNER JOIN WorkFlowState wfs 
	                        ON wfs.WorkFlowStateID = wf.CurrentState
	                        WHERE wfs.Name in ('Complete','Cancel')
	                    )
	                OR
	                AvAdvanceDocument.AdvanceID not in
	                    (
	                        SELECT remittedAdv.AdvanceID FROM FnRemittanceAdvance remittedAdv
	                        INNER JOIN FnRemittance remitted ON remitted.RemittanceID = remittedAdv.RemittanceID
	                        INNER JOIN Document doc ON remitted.DocumentID = doc.DocumentID
	                        INNER JOIN Workflow wf ON wf.DocumentID = remitted.DocumentID
	                        INNER JOIN WorkFlowState wfs 
	                        ON wfs.WorkFlowStateID = wf.CurrentState
	                        WHERE wfs.Name not in ('Complete','Cancel')
	                    )
                ) ");
            }
            
            
            if (advance.IsRepOffice)
            {
                if (advance.MainCurrencyID.HasValue && advance.MainCurrencyID.Value != 0)
                {
                    sqlBuilder.Append(" And AvAdvanceDocument.MainCurrencyID = :mainCurrencyID");
                    queryParameterBuilder.AddParameterData("mainCurrencyID", typeof(long), advance.MainCurrencyID);
                }

                if (advance.PBID.HasValue && advance.PBID.Value != 0)
                {
                    sqlBuilder.Append(" And AvAdvanceDocument.PBID = :pBID");
                    queryParameterBuilder.AddParameterData("pBID", typeof(long), advance.PBID);
                }

                sqlBuilder.Append(" And AvAdvanceDocument.IsRepOffice = :isRepOffice");
                queryParameterBuilder.AddParameterData("isRepOffice", typeof(bool), advance.IsRepOffice);
            }
            if (!isCount)
            {
                if (string.IsNullOrEmpty(sortExpression))
                    sqlBuilder.AppendLine(" ORDER BY Document.DocumentNo");
                else
                    sqlBuilder.AppendLine(string.Format(" ORDER BY {0}", sortExpression));
            }

            query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());

            if (advance.CompanyID != null)
            {
                queryParameterBuilder.AddParameterData("companyID", typeof(long), advance.CompanyID);
            }
            queryParameterBuilder.AddParameterData("advanceNo", typeof(string), string.Format("%{0}%", advance.DocumentNo));
            queryParameterBuilder.AddParameterData("description", typeof(string), string.Format("%{0}%", advance.Description));
            queryParameterBuilder.AddParameterData("advanceType", typeof(string), advance.AdvanceType);
            queryParameterBuilder.FillParameters(query);

            if (!isCount)
            {
                query.AddScalar("DocumentID", NHibernateUtil.Int64);
                query.AddScalar("AdvanceID", NHibernateUtil.Int64);
                query.AddScalar("DocumentNo", NHibernateUtil.String);
                query.AddScalar("Description", NHibernateUtil.String);
                query.AddScalar("RequesterID", NHibernateUtil.Int64);
                query.AddScalar("RequesterName", NHibernateUtil.String);
                query.AddScalar("ReceiverID", NHibernateUtil.Int64);
                query.AddScalar("ReceiverName", NHibernateUtil.String);
                query.AddScalar("RequestDateOfRemittance", NHibernateUtil.DateTime);
                query.AddScalar("Amount", NHibernateUtil.Double);
                query.AddScalar("MainCurrencyAmount", NHibernateUtil.Double);
                query.AddScalar("LocalCurrencyAmount", NHibernateUtil.Double);
                query.SetResultTransformer(Transformers.AliasToBean(typeof(Advance)));
            }
            else
            {
                query.AddScalar("DocumentCount", NHibernateUtil.Int32);
                query.UniqueResult();
            }
            return query;
        }
        #endregion
        #region IList<Advance> GetAdvanceListRelateWithRemittanceButNotInExpense(string advanceNo, string description, int firstResult, int maxResult, string sortExpression)
        public IList<Advance> GetAdvanceListRelateWithRemittanceButNotInExpense(Advance advance, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<Advance>(ScgeAccountingQueryProvider.AvAdvanceDocumentQuery, "FindAdvanceRelateWithRemittanceButNotInExpenseByAdvanceCriteria", new object[] { advance, false, sortExpression }, firstResult, maxResult, sortExpression);
        }
        #endregion

        #region int CountByAdvanceCriteria(string advanceNo, string description)
        public int CountByAdvanceCriteria(Advance advance)
        {
            return NHibernateQueryHelper.CountByCriteria(ScgeAccountingQueryProvider.AvAdvanceDocumentQuery, "FindByAdvanceCriteria", new object[] { advance, true, string.Empty });
        }
        #endregion
        #endregion

        #region IList<Advance> FindByAdvanceID(long advanceID)
        public IList<Advance> FindByAdvanceID(long advanceID)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            //sqlBuilder.Append(" SELECT Document.DocumentNo as DocumentNo");
            //sqlBuilder.Append(" , Document.Subject as Description");
            //sqlBuilder.Append(" , AvAdvanceDocument.DueDateOfRemittance as DueDateOfRemittance");
            //sqlBuilder.Append(" , AvAdvanceItem.Amount as Amount");
            //sqlBuilder.Append(" , AvAdvanceDocument.DocumentID as DocumentID");
            //sqlBuilder.Append(" , AvAdvanceDocument.AdvanceID as AdvanceID");
            //sqlBuilder.Append(" , Requester.Username as RequesterName");
            //sqlBuilder.Append(" , Receiver.Username as ReceiverName");
            //sqlBuilder.Append(" FROM Document ");
            //sqlBuilder.Append(" INNER JOIN AvAdvanceDocument");
            //sqlBuilder.Append(" ON Document.DocumentID = AvAdvanceDocument.DocumentID");
            //sqlBuilder.Append(" INNER JOIN SuUser as Requester");
            //sqlBuilder.Append(" ON Document.RequesterID = Requester.UserID");
            //sqlBuilder.Append(" INNER JOIN SuUser as Receiver");
            //sqlBuilder.Append(" ON Document.ReceiverID = Receiver.UserID");
            //sqlBuilder.Append(" INNER JOIN DBPB");
            //sqlBuilder.Append(" ON DBPB.PBID = AvAdvanceDocument.PBID");
            //sqlBuilder.Append(" INNER JOIN AvAdvanceItem");
            //sqlBuilder.Append(" ON AvAdvanceItem.AdvanceID = AvAdvanceDocument.AdvanceID");
            //sqlBuilder.Append(" WHERE  (AvAdvanceDocument.AdvanceID = :advanceID) ");
            sqlBuilder.Append("SELECT ad.advanceID as AdvanceID,d.DocumentID as DocumentID,d.documentNo as DocumentNo, d.documentNo as AdvanceNo, d.Subject as Description");
            sqlBuilder.Append(" , u1.username as RequesterName, u2.username as ReceiverName");
            sqlBuilder.Append(" ,ad.duedateofremittance as DueDateOfRemittance,ad.requestdateofremittance as RequestDateOfRemittance,ad.maincurrencyamount as MainCurrencyAmount");
            sqlBuilder.Append(" ,ad.amount as Amount,ad.exchangeratemaintothbCurrency as ExchangeRateMainToTHBCurrency");
            sqlBuilder.Append(" FROM document  as d");
            sqlBuilder.Append(" INNER JOIN avadvancedocument ad ON d.documentID = ad.documentID");
            sqlBuilder.Append(" INNER JOIN suuser u1 ON d.requesterID = u1.userID");
            sqlBuilder.Append(" INNER JOIN suuser u2 ON d.receiverID = u2.userID");
            sqlBuilder.Append(" WHERE  (ad.AdvanceID = :advanceID) ");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());

            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("advanceID", typeof(long), advanceID);
            queryParameterBuilder.FillParameters(query);
            query.AddScalar("DocumentID", NHibernateUtil.Int64);
            query.AddScalar("DocumentNo", NHibernateUtil.String);
            query.AddScalar("AdvanceID", NHibernateUtil.Int64);
            query.AddScalar("AdvanceNo", NHibernateUtil.String);
            query.AddScalar("Description", NHibernateUtil.String);
            query.AddScalar("RequesterName", NHibernateUtil.String);
            query.AddScalar("ReceiverName", NHibernateUtil.String);
            query.AddScalar("DueDateOfRemittance", NHibernateUtil.DateTime);
            query.AddScalar("RequestDateOfRemittance", NHibernateUtil.DateTime);
            query.AddScalar("MainCurrencyAmount", NHibernateUtil.Double);
            query.AddScalar("Amount", NHibernateUtil.Double);
            query.AddScalar("ExchangeRateMainToTHBCurrency", NHibernateUtil.Double);
            return query.SetResultTransformer(Transformers.AliasToBean(typeof(Advance))).List<Advance>();
        }
        #endregion
        #region IList<Advance> FindRemittanceTAAdvanceByTADocumentID(long taID)
        public IList<Advance> FindRemittanceTAAdvanceByTADocumentID(long taID)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT d.DocumentID as DocumentID , d.DocumentNo as DocumentNo , d.Subject as Description , u1.UserName as RequesterName , ");
            sqlBuilder.Append("u2.UserName as ReceiverName ,ad.RequestDateOfRemittance as RequestDateOfRemittance , ad.Amount as Amount , ");
            sqlBuilder.Append("ad.AdvanceID as AdvanceID ");
            sqlBuilder.Append("FROM AVAdvanceDocument as ad ");
            //sqlBuilder.Append("LEFT JOIN AvAdvanceItem as ai on ai.AdvanceID = ad.AdvanceID ");
            sqlBuilder.Append("LEFT JOIN Document as d on d.DocumentID = ad.DocumentID ");
            sqlBuilder.Append("LEFT JOIN SuUser as u1 on d.RequesterID = u1.UserID ");
            sqlBuilder.Append("LEFT JOIN SuUser as u2 on d.ReceiverID = u2.UserID ");
            sqlBuilder.Append("WHERE ad.TADocumentID =:taDocumentID");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());

            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("taDocumentID", typeof(long), taID);
            queryParameterBuilder.FillParameters(query);
            query.AddScalar("DocumentID", NHibernateUtil.Int64);
            query.AddScalar("AdvanceID", NHibernateUtil.Int64);
            query.AddScalar("DocumentNo", NHibernateUtil.String);
            query.AddScalar("Description", NHibernateUtil.String);
            query.AddScalar("RequesterName", NHibernateUtil.String);
            query.AddScalar("ReceiverName", NHibernateUtil.String);
            query.AddScalar("RequestDateOfRemittance", NHibernateUtil.DateTime);
            query.AddScalar("Amount", NHibernateUtil.Double);
            return query.SetResultTransformer(Transformers.AliasToBean(typeof(Advance))).List<Advance>();
        }
        #endregion

        #region query for tab outstatnding
        #region ISQLQuery FindOutstandingQuery(long requesterID, long currentWorkFlowID, short languareID, bool isCount, string sortExpression)
        public ISQLQuery FindOutstandingQuery(long requesterID, long currentWorkFlowID, int documentTypeAdvance, int documentTypeExpense,
            IList<int> documentStatus, short languageID, bool isCount, string sortExpression)
        {
            ISQLQuery query;
            if (!isCount)
            {
                StringBuilder sqlBuilder = new StringBuilder();

                sqlBuilder.AppendLine(" select b.documentno as AdvanceNo,b.DocumentID as avDocumentID ,h.DocumentID as expenseDocumentID, b.subject as Description ,e.displayName as AdvanceStatus,h.documentNo as ExpenseNo ,k.displayName as ExpenseStatus, a.RequestDateOfRemittance as DueDate  ,a.amount as Amount ");
                sqlBuilder.AppendLine(" from avadvancedocument a inner join [document] b on a.documentId = b.documentID and b.documentTypeID = :documentTypeAdvance ");
                sqlBuilder.AppendLine(" inner join workflow c on b.documentID = c.documentID  ");
                sqlBuilder.AppendLine(" inner join workflowstate d on c.currentstate = d.workflowstateID  ");
                sqlBuilder.AppendLine(" inner join workflowstatelang e on d.workflowstateID = e.workflowstateID and e.languageID = :languageID ");
                sqlBuilder.AppendLine(" left outer join fnexpenseadvance f on f.advanceID = a.advanceID ");
                sqlBuilder.AppendLine(" left join fnexpensedocument g on g.expenseID = f.expenseID  ");
                sqlBuilder.AppendLine(" left join [document] h on h.documentId = g.documentID and h.documentTypeID = :documentTypeExpense ");
                sqlBuilder.AppendLine(" left join workflow i on i.documentID = h.documentID  ");
                sqlBuilder.AppendLine(" left join workflowstate j on i.currentstate = j.workflowstateID  ");
                sqlBuilder.AppendLine(" left join workflowstatelang k on k.workflowstateID = j.workflowstateID and k.languageID = :languageID ");
                sqlBuilder.AppendLine(" where b.requesterID = :requesterID and a.active = 1 ");
                sqlBuilder.AppendLine(" and c.workflowID <> :currentWorkFlowID ");
                sqlBuilder.AppendLine(string.Format("and c.currentstate not in ({0},{1},{2})", documentStatus[0], documentStatus[1], documentStatus[2]));

                #region old query Not use
                /*
                StringBuilder sqlBuilder = new StringBuilder();
                sqlBuilder.Append(" select a.No as No, a.advanceID as AdvanceID,");
                sqlBuilder.Append(" a.AdvanceNo as AdvanceNo, ");
                sqlBuilder.Append(" a.Description as Description, ");
                sqlBuilder.Append(" a.AdvanceStatus as AdvanceStatus, ");
                sqlBuilder.Append(" b.ExpenseNo as ExpenseNo, ");
                sqlBuilder.Append(" b.ExpenseStatus as ExpenseStatus, ");
                sqlBuilder.Append(" a.DueDate as DueDate, a.DocumentID as DocumentID, ");
                sqlBuilder.Append(" a.Amount as Amount  from( ");
                sqlBuilder.Append(" select  1 as No ");//ROW_NUMBER() OVER(ORDER BY Document.DocumentID)
                sqlBuilder.Append(" ,AvAdvanceDocument.AdvanceID ");
                sqlBuilder.Append(" ,document.DocumentNo as AdvanceNo ");
                sqlBuilder.Append(" ,Document.Subject as Description ");
                sqlBuilder.Append(" ,WorkFlowStateLang.DisplayName as AdvanceStatus ");
                sqlBuilder.Append(" ,AvAdvanceDocument.DueDateOfRemittance as DueDate ");
                sqlBuilder.Append(" ,AvAdvanceDocument.Amount,document.documentID as documentID ");
                sqlBuilder.Append(" from document ");
                sqlBuilder.Append(" inner join AvAdvanceDocument ");
                sqlBuilder.Append(" on document.DocumentID = AvAdvanceDocument.DocumentID ");
                sqlBuilder.Append(" inner join WorkFlow ");
                sqlBuilder.Append(" on document.DocumentID = WorkFlow.DocumentID ");
                sqlBuilder.Append(" inner join WorkFlowState ");
                sqlBuilder.Append(" on WorkFlow.WorkFlowTypeID = WorkFlowState.WorkFlowTypeID ");
                sqlBuilder.Append(" and WorkFlow.CurrentState = WorkFlowState.WorkFlowStateID ");
                sqlBuilder.Append(" inner join WorkFlowStateLang ");
                sqlBuilder.Append(" on WorkFlowState.WorkFlowTypeID = WorkFlowStateLang.WorkFlowStateID ");
                sqlBuilder.Append(" and WorkFlowStateLang.LanguageID = :languareID ");
                sqlBuilder.Append(" where document.RequesterID = :requesterID ");
                sqlBuilder.Append(" )a ");
                sqlBuilder.Append(" left outer  join( ");
                sqlBuilder.Append(" select AvAdvanceDocument.AdvanceID ");
                sqlBuilder.Append(" ,Document.DocumentNo as ExpenseNo ");
                sqlBuilder.Append(" ,WorkFlowStateLang.DisplayName as ExpenseStatus ");
                sqlBuilder.Append(" from dbo.AvAdvanceDocument  ");
                sqlBuilder.Append(" inner join FnExpenseAdvance  ");
                sqlBuilder.Append(" on AvAdvanceDocument.AdvanceID = FnExpenseAdvance.AdvanceID  ");
                sqlBuilder.Append(" inner join FnExpenseDocument  ");
                sqlBuilder.Append(" on FnExpenseDocument.ExpenseID = FnExpenseAdvance.ExpenseID  ");
                sqlBuilder.Append(" inner join Document  ");
                sqlBuilder.Append(" on Document.DocumentID = FnExpenseDocument.DocumentID ");
                sqlBuilder.Append(" inner join WorkFlow ");
                sqlBuilder.Append(" on document.DocumentID = WorkFlow.DocumentID ");
                sqlBuilder.Append(" inner join WorkFlowState ");
                sqlBuilder.Append(" on WorkFlow.WorkFlowTypeID = WorkFlowState.WorkFlowTypeID ");
                sqlBuilder.Append(" and WorkFlow.CurrentState = WorkFlowState.WorkFlowStateID ");
                sqlBuilder.Append(" and WorkFlow.CurrentState not in (select WorkFlowStateID from WorkFlowState where Name in ('Complete','Cancel')) ");
                sqlBuilder.Append(" inner join WorkFlowStateLang ");
                sqlBuilder.Append(" on WorkFlowState.WorkFlowTypeID = WorkFlowStateLang.WorkFlowStateID ");
                sqlBuilder.Append(" and WorkFlowStateLang.LanguageID = :languareID  ");
                sqlBuilder.Append(" where document.RequesterID = :requesterID ");
                sqlBuilder.Append(" )b on a.AdvanceID = b.AdvanceID ");
                */
                #endregion
                if (string.IsNullOrEmpty(sortExpression))
                    sqlBuilder.AppendLine(" ORDER BY b.documentno ");
                else
                    sqlBuilder.AppendLine(string.Format(" ORDER BY {0}", sortExpression));
                query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
                //query.SetParameterList("documentStatus", documentStatus);

                QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
                queryParameterBuilder.AddParameterData("requesterID", typeof(long), requesterID);
                queryParameterBuilder.AddParameterData("currentWorkFlowID", typeof(long), currentWorkFlowID);
                queryParameterBuilder.AddParameterData("languageID", typeof(short), languageID);
                queryParameterBuilder.AddParameterData("documentTypeAdvance", typeof(int), documentTypeAdvance);
                queryParameterBuilder.AddParameterData("documentTypeExpense", typeof(int), documentTypeExpense);


                queryParameterBuilder.FillParameters(query);
                //query.AddScalar("No", NHibernateUtil.Int16);
                query.AddScalar("avDocumentID", NHibernateUtil.Int64);
                query.AddScalar("expenseDocumentID", NHibernateUtil.Int64);
                query.AddScalar("AdvanceNo", NHibernateUtil.String);
                query.AddScalar("Description", NHibernateUtil.String);
                query.AddScalar("AdvanceStatus", NHibernateUtil.String);
                query.AddScalar("ExpenseNo", NHibernateUtil.String);
                query.AddScalar("ExpenseStatus", NHibernateUtil.String);
                query.AddScalar("DueDate", NHibernateUtil.DateTime);
                query.AddScalar("Amount", NHibernateUtil.Double);
                query.SetResultTransformer(Transformers.AliasToBean(typeof(AdvanceOutstanding)));
            }
            else
            {
                StringBuilder sqlBuilder = new StringBuilder();
                sqlBuilder.Append(" select count(a.AdvanceID) as count");
                sqlBuilder.AppendLine(" from avadvancedocument a inner join [document] b on a.documentId = b.documentID and b.documentTypeID = :documentTypeAdvance ");
                sqlBuilder.AppendLine(" inner join workflow c on b.documentID = c.documentID  ");
                sqlBuilder.AppendLine(" inner join workflowstate d on c.currentstate = d.workflowstateID  ");
                sqlBuilder.AppendLine(" inner join workflowstatelang e on d.workflowstateID = e.workflowstateID and e.languageID = :languageID ");
                sqlBuilder.AppendLine(" left outer join fnexpenseadvance f on f.advanceID = a.advanceID ");
                sqlBuilder.AppendLine(" left join fnexpensedocument g on g.expenseID = f.expenseID  ");
                sqlBuilder.AppendLine(" left join [document] h on h.documentId = g.documentID and h.documentTypeID = :documentTypeExpense ");
                sqlBuilder.AppendLine(" left join workflow i on i.documentID = h.documentID  ");
                sqlBuilder.AppendLine(" left join workflowstate j on i.currentstate = j.workflowstateID  ");
                sqlBuilder.AppendLine(" left join workflowstatelang k on k.workflowstateID = j.workflowstateID and k.languageID = :languageID ");
                sqlBuilder.AppendLine(" where b.requesterID = :requesterID and a.active = 1 ");
                sqlBuilder.AppendLine(" and c.workflowID <> :currentWorkFlowID ");
                sqlBuilder.AppendLine(string.Format("and c.currentstate not in ({0},{1},{2})", documentStatus[0], documentStatus[1], documentStatus[2]));
                query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
                //query.SetParameterList("documentStatus", documentStatus);

                QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
                queryParameterBuilder.AddParameterData("requesterID", typeof(long), requesterID);
                queryParameterBuilder.AddParameterData("currentWorkFlowID", typeof(long), currentWorkFlowID);
                queryParameterBuilder.AddParameterData("languageID", typeof(short), languageID);
                queryParameterBuilder.AddParameterData("documentTypeAdvance", typeof(int), documentTypeAdvance);
                queryParameterBuilder.AddParameterData("documentTypeExpense", typeof(int), documentTypeExpense);

                queryParameterBuilder.FillParameters(query);
                query.AddScalar("count", NHibernateUtil.Int32);
                //query.UniqueResult();
            }
            return query;
        }
        #endregion
        #region Count Advance Outstanding for fixedadvance
        public int CountAdvanceForFixedAdvance(long companyID, long requesterID, long currentWorkFlowID, int documentTypeAdvance,DateTime curdate)
        {
            return NHibernateQueryHelper.CountByCriteria(ScgeAccountingQueryProvider.AvAdvanceDocumentQuery, "CountAdvanceOutstandingForFixedAdvance", new object[] { companyID, requesterID, currentWorkFlowID, documentTypeAdvance, curdate });
        }
        public ISQLQuery CountAdvanceOutstandingForFixedAdvance(long companyID, long requesterID, long currentWorkFlowID, int documentTypeAdvance,DateTime curdate)
        {
            ISQLQuery query;
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine(" SELECT COUNT(a.AdvanceID) as count FROM AvAdvanceDocument a ");
            sqlBuilder.AppendLine(" JOIN Document b on a.DocumentID = b.DocumentID AND b.DocumentTypeID = :documentTypeAdvance ");
            sqlBuilder.AppendLine(" JOIN WorkFlow c on b.DocumentID = c.DocumentID ");
            sqlBuilder.AppendLine(" WHERE a.active = 1 AND b.RequesterID = :requesterID AND b.CompanyID = :companyID ");
            sqlBuilder.AppendLine(" AND c.WorkFlowID <> :currentWorkFlowID AND b.CacheCurrentState = 25 AND b.DocumentDate < :curdate "); /*currentstate 25 = Outstanding*/

            query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());

            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("companyID", typeof(long), companyID);
            queryParameterBuilder.AddParameterData("requesterID", typeof(long), requesterID);
            queryParameterBuilder.AddParameterData("currentWorkFlowID", typeof(long), currentWorkFlowID);
            queryParameterBuilder.AddParameterData("documentTypeAdvance", typeof(int), documentTypeAdvance);
            queryParameterBuilder.AddParameterData("curdate", typeof(DateTime), curdate);
            

            queryParameterBuilder.FillParameters(query);
            query.AddScalar("count", NHibernateUtil.Int32);
            return query;
        }
        #endregion 
        #region IList<AdvanceOutstanding> FindOutstandingRequest(long requesterID, long currentWorkFlowID, short languareID, int firstResult, int maxResults, string sortExpression)
        public IList<AdvanceOutstanding> FindOutstandingRequest(long requesterID, long currentWorkFlowID, int documentTypeAdvance, int documentTypeExpense, IList<int> documentStatus, short languageID, int firstResult, int maxResults, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<AdvanceOutstanding>(ScgeAccountingQueryProvider.AvAdvanceDocumentQuery, "FindOutstandingQuery", new object[] { requesterID, currentWorkFlowID, documentTypeAdvance, documentTypeExpense, documentStatus, languageID, false, sortExpression }, firstResult, maxResults, sortExpression);
        }
        #endregion
        #region int CountFindOutstandingRequest(long requesterID, long currentWorkFlowID, int documentTypeAdvance, int documentTypeExpense, IList<int> documentStatus, short languageID)
        public int CountFindOutstandingRequest(long requesterID, long currentWorkFlowID, int documentTypeAdvance, int documentTypeExpense, IList<int> documentStatus, short languageID)
        {
            return NHibernateQueryHelper.CountByCriteria(ScgeAccountingQueryProvider.AvAdvanceDocumentQuery, "FindOutstandingQuery", new object[] { requesterID, currentWorkFlowID, documentTypeAdvance, documentTypeExpense, documentStatus, languageID, true, string.Empty });
        }
        #endregion
        #endregion

        #region bool isSeeHistoryReject(long documentID)
        /// <summary>
        /// check show See History for Reject Details
        /// </summary>
        /// <param name="documentID"></param>
        /// <returns></returns>
        public bool isSeeHistoryReject(long documentID)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" SELECT  (case when count(*) > 0 then 1 else 0 end) as isSeeHistory");
            sqlBuilder.Append(" FROM WorkFlow with(nolock) INNER JOIN");
            sqlBuilder.Append(" WorkFlowResponse with(nolock) ON WorkFlow.WorkFlowID = WorkFlowResponse.WorkFlowID");
            sqlBuilder.Append(" AND WorkFlowResponse.WorkFlowStateEventID IN (SELECT WorkFlowStateEventID FROM WorkFlowStateEvent with(nolock) WHERE LOWER(NAME) = 'Reject')");
            sqlBuilder.Append(" WHERE WorkFlow.DocumentID = :documentID and WorkFlow.Active = 1 ");
            return GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString())
                .AddScalar("isSeeHistory", NHibernateUtil.Boolean)
                .SetInt64("documentID", documentID)
                .UniqueResult<Boolean>();
        }
        #endregion

        public AvAdvanceDocument GetAvAdvanceByDocumentID(long documentID)
        {
            ICriteria criteria = GetCurrentSession().CreateCriteria(typeof(AvAdvanceDocument), "av");
            criteria.Add(Expression.Eq("av.DocumentID.DocumentID", documentID));

            return criteria.UniqueResult<AvAdvanceDocument>();
        }

        public AdvanceDataForEmail GetAvAdvanceforEmailByDocumentID(long documentID)
        {

            StringBuilder sql = new StringBuilder();
            #region query อันเก่า
            //sql.AppendLine(" select doc.DocumentNo ,advdoc.AdvanceID,doc.Subject ,advdoc.RequestDateOfAdvance , ");
            //sql.AppendLine(" advdoc.Amount,advdoc.UpdBy,advdoc.IsRepOffice,advitem.CurrencyID,cr.Symbol, ");
            //sql.AppendLine(" advitem.MainCurrencyAmount as advitemMainCurrencyAmount,advdoc.MainCurrencyAmount as advdocMainCurrencyAmount,advdoc.LocalCurrencyAmount as advdocLocalCurrencyAmount,advdoc.AdvanceType ");
            //sql.AppendLine(" from AvAdvanceDocument advdoc ");
            //sql.AppendLine(" inner join Document doc on doc.DocumentID = advdoc.DocumentID");
            //sql.AppendLine(" inner join AvAdvanceItem advitem on advdoc.AdvanceID = advitem.AdvanceID");
            //sql.AppendLine(" left join DbCurrency cr on cr.CurrencyID = advitem.CurrencyID");
            //sql.AppendLine(" where doc.DocumentID = :documentID");
            #endregion
            sql.AppendLine("select DocumentID,DocumentNo ,AdvanceID,Subject ,RequestDateOfAdvance ,  ");
            sql.AppendLine(" Amount,UpdBy,IsRepOffice,CurrencyID,SymbolLocal, SymbolMain, ");
            sql.AppendLine("advitemMainCurrencyAmount,advdocMainCurrencyAmount,advdocLocalCurrencyAmount,AdvanceType,PBID ");
            sql.AppendLine(" from(  ");
            sql.AppendLine("select doc.DocumentID,doc.DocumentNo ,advdoc.AdvanceID,doc.Subject ,advdoc.RequestDateOfAdvance ,  ");
            sql.AppendLine(" advdoc.Amount,advdoc.UpdBy,advdoc.IsRepOffice,advitem.CurrencyID,cr.Symbol as SymbolLocal, null as SymbolMain, ");
            sql.AppendLine(" advitem.MainCurrencyAmount as advitemMainCurrencyAmount,advdoc.MainCurrencyAmount as advdocMainCurrencyAmount,advdoc.LocalCurrencyAmount as advdocLocalCurrencyAmount,advdoc.AdvanceType ,  advdoc.PBID ");
            sql.AppendLine(" from AvAdvanceDocument advdoc  ");
            sql.AppendLine(" inner join Document doc on doc.DocumentID = advdoc.DocumentID ");
            sql.AppendLine(" inner join AvAdvanceItem advitem on advdoc.AdvanceID = advitem.AdvanceID ");
            sql.AppendLine(" left join DbCurrency cr on cr.CurrencyID = advitem.CurrencyID ");
            sql.AppendLine(" where advdoc.AdvanceType = 'DM' ");
 
            sql.AppendLine(" union ");
 
            sql.AppendLine(" select doc.DocumentID, doc.DocumentNo , advdoc.AdvanceID, doc.Subject , advdoc.RequestDateOfAdvance ,  ");
            sql.AppendLine(" advdoc.Amount, advdoc.UpdBy, advdoc.IsRepOffice,null as CurrencyID,null as SymbolLocal, cr.Symbol as SymbolMain, ");
            sql.AppendLine(" null as advitemMainCurrencyAmount,null as advdocMainCurrencyAmount, ");
            sql.AppendLine(" null as advdocLocalCurrencyAmount, ");
            sql.AppendLine(" advdoc.AdvanceType ,  advdoc.PBID ");
            sql.AppendLine(" from AvAdvanceDocument advdoc  ");
            sql.AppendLine(" inner join Document doc on doc.DocumentID = advdoc.DocumentID ");
            sql.AppendLine("  left join DbCurrency cr on cr.CurrencyID = advdoc.MainCurrencyID ");
            sql.AppendLine("   where advdoc.AdvanceType = 'FR' ");
            sql.AppendLine(" ) as t1 ");
            sql.AppendLine(" where DocumentID =:documentID ");
            

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            query.SetParameter("documentID", documentID);

            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.FillParameters(query);
            query.AddScalar("DocumentNo", NHibernateUtil.String);
            query.AddScalar("AdvanceID", NHibernateUtil.Int64);
            query.AddScalar("Subject",NHibernateUtil.String);
            query.AddScalar("RequestDateOfAdvance",NHibernateUtil.DateTime);
            query.AddScalar("Amount",NHibernateUtil.Double);
            query.AddScalar("UpdBy",NHibernateUtil.Int64);
            query.AddScalar("IsRepOffice",NHibernateUtil.Boolean);
            query.AddScalar("CurrencyID", NHibernateUtil.Int16);
            query.AddScalar("SymbolLocal", NHibernateUtil.String);
            query.AddScalar("SymbolMain", NHibernateUtil.String);
            query.AddScalar("advitemMainCurrencyAmount", NHibernateUtil.Double);
            query.AddScalar("advdocMainCurrencyAmount", NHibernateUtil.Double);
            query.AddScalar("advdocLocalCurrencyAmount", NHibernateUtil.Double);
            query.AddScalar("AdvanceType", NHibernateUtil.String);
            query.AddScalar("PBID", NHibernateUtil.Int64);

            return query.SetResultTransformer(Transformers.AliasToBean(typeof(AdvanceDataForEmail))).UniqueResult<AdvanceDataForEmail>();
    
        }

        public IList<Advance> SumAmountByPaymentTypeAndCurrency(IList<long> advanceIDList)
        {
            // Change this method to query complete result for RemitItem by Anuwat S on 12/05/2009
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("SELECT PaymentType, avItem.CurrencyID, SUM(Amount) AS ForeignCurrencyAdvanced, ");
            sql.AppendLine("	exchTab.ExchangeRate,");
            sql.AppendLine("	SUM(AmountTHB) AS RemittedAmountTHB");
            sql.AppendLine(" FROM AvAdvanceItem avItem");
            sql.AppendLine("		INNER JOIN (SELECT b.CurrencyID,");
            sql.AppendLine("						ROUND(SUM(AmountTHB) / (CASE SUM(Amount) WHEN 0 THEN 1 ELSE SUM(Amount) END),5) AS ExchangeRate");
            sql.AppendLine("						FROM AvAdvanceItem b WHERE b.AdvanceID IN (:advanceIDList) GROUP BY b.CurrencyID) exchTab");
            sql.AppendLine("			ON exchTab.CurrencyID = avItem.CurrencyID");
            sql.AppendLine(" WHERE avItem.AdvanceID IN (:advanceIDList) and avItem.Active = 1");
            sql.AppendLine(" GROUP BY PaymentType, avItem.CurrencyID, exchTab.ExchangeRate");
            sql.AppendLine(" ORDER BY paymenttype,currencyid");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            query.SetParameterList("advanceIDList", advanceIDList);

            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.FillParameters(query);
            query.AddScalar("PaymentType", NHibernateUtil.String);
            query.AddScalar("CurrencyID", NHibernateUtil.Int16);
            query.AddScalar("ForeignCurrencyAdvanced", NHibernateUtil.Double);
            query.AddScalar("ExchangeRate", NHibernateUtil.Double);
            query.AddScalar("RemittedAmountTHB", NHibernateUtil.Double);

            return query.SetResultTransformer(Transformers.AliasToBean(typeof(Advance))).List<Advance>();
        }

        public IList<Advance> SumAmountByPaymentTypeAndCurrencyForRepOffice(IList<long> advanceIDList)
        {
            // Change this method to query complete result for RemitItem by Anuwat S on 12/05/2009
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("SELECT PaymentType, avItem.CurrencyID, SUM(ISNULL(Amount,0)) AS ForeignCurrencyAdvanced, ");
            sql.AppendLine("	exchTab.ExchangeRate,");
            sql.AppendLine("	SUM(ISNULL(AmountTHB,0)) AS RemittedAmountTHB,");
            sql.AppendLine("	SUM(ISNULL(MainCurrencyAmount,0)) AS MainCurrencyAmount");
            sql.AppendLine(" FROM AvAdvanceItem avItem");
            sql.AppendLine("		INNER JOIN (SELECT b.CurrencyID,");
            sql.AppendLine("						ROUND(SUM(ISNULL(MainCurrencyAmount,0)) / (CASE SUM(ISNULL(Amount,0)) WHEN 0 THEN 1 ELSE SUM(ISNULL(Amount,0)) END),5) AS ExchangeRate");
            sql.AppendLine("						FROM AvAdvanceItem b WHERE b.AdvanceID IN (:advanceIDList) GROUP BY b.CurrencyID) exchTab");
            sql.AppendLine("			ON exchTab.CurrencyID = avItem.CurrencyID");
            sql.AppendLine(" WHERE avItem.AdvanceID IN (:advanceIDList) and avItem.Active = 1");
            sql.AppendLine(" GROUP BY PaymentType, avItem.CurrencyID, exchTab.ExchangeRate");
            sql.AppendLine(" ORDER BY paymenttype,currencyid");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            query.SetParameterList("advanceIDList", advanceIDList);

            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.FillParameters(query);
            query.AddScalar("PaymentType", NHibernateUtil.String);
            query.AddScalar("CurrencyID", NHibernateUtil.Int16);
            query.AddScalar("ForeignCurrencyAdvanced", NHibernateUtil.Double);
            query.AddScalar("ExchangeRate", NHibernateUtil.Double);
            query.AddScalar("RemittedAmountTHB", NHibernateUtil.Double);
            query.AddScalar("MainCurrencyAmount", NHibernateUtil.Double);

            return query.SetResultTransformer(Transformers.AliasToBean(typeof(Advance))).List<Advance>();
        }

        #region GetPerdiemExchangeRate for ExpenseDocument
        public InvoiceExchangeRate GetPerdiemExchangeRateUSD(IList<long> advanceIDList)
        {
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.AppendLine(" SELECT SUM(ISNULL(PerDiemExRateUSD,0)) as AdvanceExchangeRateAmount ");
            queryBuilder.AppendLine(" FROM  AvAdvanceDocument ");
            queryBuilder.AppendLine(" WHERE AdvanceID in ");
            queryBuilder.AppendLine(" ( :advanceIDList ) ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(queryBuilder.ToString());
            query.SetParameterList("advanceIDList", advanceIDList);
            query.AddScalar("AdvanceExchangeRateAmount", NHibernateUtil.Double);
            query.SetResultTransformer(Transformers.AliasToBean(typeof(InvoiceExchangeRate)));

            return query.UniqueResult<InvoiceExchangeRate>();
        }
        #endregion

        public IList<Advance> FindAdvanceDocumentByTADocumentID(long taDocumentID)
        {
            //ICriteria criteria = GetCurrentSession().CreateCriteria(typeof(AvAdvanceDocument), "a");
            //criteria.Add(Expression.Eq("a.TADocumentID", taDocumentID));
            //return criteria.List<Advance>();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("SELECT a.DocumentID as DocumentID, a.AdvanceID as AdvanceID,a.Amount AS Amount, doc.RequesterID, doc.DocumentNo, doc.Subject AS Description,");
            sql.AppendLine(" (SELECT UserName FROM SuUser WHERE UserID = doc.RequesterID) AS RequesterName, ");
            sql.AppendLine(" (SELECT UserName FROM SuUser WHERE UserID = doc.ReceiverID) AS ReceiverName, ");
            sql.AppendLine(" a.RequestDateOfRemittance, a.DueDateOfRemittance ");
            sql.AppendLine("FROM AvAdvanceDocument AS a ");
            sql.AppendLine("    INNER JOIN Document AS doc ON a.DocumentID = doc.DocumentID ");
            sql.AppendLine("WHERE a.TADocumentID =:taDocumentID");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("taDocumentID", typeof(long), taDocumentID);
            queryParameterBuilder.FillParameters(query);
            query.AddScalar("DocumentID", NHibernateUtil.Int64);
            query.AddScalar("AdvanceID", NHibernateUtil.Int64);
            query.AddScalar("RequesterID", NHibernateUtil.Int64);
            query.AddScalar("DocumentNo", NHibernateUtil.String);
            query.AddScalar("Description", NHibernateUtil.String);
            query.AddScalar("RequesterName", NHibernateUtil.String);
            query.AddScalar("ReceiverName", NHibernateUtil.String);
            query.AddScalar("DueDateOfRemittance", NHibernateUtil.DateTime);
            query.AddScalar("RequestDateOfRemittance", NHibernateUtil.DateTime);
            query.AddScalar("Amount", NHibernateUtil.Double);

            return query.SetResultTransformer(Transformers.AliasToBean(typeof(Advance))).List<Advance>();
        }

        public IList<Advance> FindAdvanceDocumentForRequesterByTADocumentID(long requesterId, long taDocumentID)
        {
            //ICriteria criteria = GetCurrentSession().CreateCriteria(typeof(AvAdvanceDocument), "a");
            //criteria.Add(Expression.Eq("a.TADocumentID", taDocumentID));
            //return criteria.List<Advance>();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("SELECT a.DocumentID as DocumentID, a.AdvanceID as AdvanceID,a.Amount AS Amount, doc.RequesterID, doc.DocumentNo, doc.Subject AS Description,");
            sql.AppendLine(" (SELECT UserName FROM SuUser WHERE UserID = doc.RequesterID) AS RequesterName, ");
            sql.AppendLine(" (SELECT UserName FROM SuUser WHERE UserID = doc.ReceiverID) AS ReceiverName, ");
            sql.AppendLine(" a.RequestDateOfRemittance, a.DueDateOfRemittance ");
            sql.AppendLine("FROM AvAdvanceDocument AS a ");
            sql.AppendLine("    INNER JOIN Document AS doc ON a.DocumentID = doc.DocumentID ");
            sql.AppendLine("WHERE a.TADocumentID =:taDocumentID AND doc.RequesterID = :RequesterID");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("taDocumentID", typeof(long), taDocumentID);
            queryParameterBuilder.AddParameterData("RequesterID", typeof(long), requesterId);
            queryParameterBuilder.FillParameters(query);
            query.AddScalar("DocumentID", NHibernateUtil.Int64);
            query.AddScalar("AdvanceID", NHibernateUtil.Int64);
            query.AddScalar("RequesterID", NHibernateUtil.Int64);
            query.AddScalar("DocumentNo", NHibernateUtil.String);
            query.AddScalar("Description", NHibernateUtil.String);
            query.AddScalar("RequesterName", NHibernateUtil.String);
            query.AddScalar("ReceiverName", NHibernateUtil.String);
            query.AddScalar("DueDateOfRemittance", NHibernateUtil.DateTime);
            query.AddScalar("RequestDateOfRemittance", NHibernateUtil.DateTime);
            query.AddScalar("Amount", NHibernateUtil.Double);

            return query.SetResultTransformer(Transformers.AliasToBean(typeof(Advance))).List<Advance>();
        }

        public IList<AvAdvanceDocument> FindByTADocumentID(long taDocumentID)
        {
            return GetCurrentSession().CreateQuery("from AvAdvanceDocument where TADocumentID = :TADocumentID and active = '1'")
                .SetInt64("TADocumentID", taDocumentID)
                .List<AvAdvanceDocument>();
        }

        public IList<Advance> FindAdvancDocumentByAdvanceIds(IList<long> advanceIdList)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT w.WorkflowID, a.AdvanceID, d.DocumentNo AS AdvanceNo , d.Subject AS Description , a.RequestDateOfRemittance as RequestDateOfRemittance , ");
            sql.Append("a.Amount AS Amount ");
            sql.Append(" , a.MainCurrencyAmount as MainCurrencyAmount ");
            sql.Append(" , a.LocalCurrencyAmount as LocalCurrencyAmount ");
            sql.Append("FROM AvAdvanceDocument AS a ");
            sql.Append("INNER JOIN Document AS d ON d.DocumentID = a.DocumentID ");
            sql.Append("INNER JOIN Workflow w ON w.DocumentID = d.DocumentID ");
            sql.Append("WHERE AdvanceID in(:advanceIdList) ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            query.SetParameterList("advanceIdList", advanceIdList);
            query.AddScalar("WorkflowID", NHibernateUtil.Int64);
            query.AddScalar("AdvanceID", NHibernateUtil.Int64);
            query.AddScalar("AdvanceNo", NHibernateUtil.String);
            query.AddScalar("Description", NHibernateUtil.String);
            query.AddScalar("RequestDateOfRemittance", NHibernateUtil.DateTime);
            query.AddScalar("Amount", NHibernateUtil.Double);
            query.AddScalar("MainCurrencyAmount", NHibernateUtil.Double);
            query.AddScalar("LocalCurrencyAmount", NHibernateUtil.Double);
            return query.SetResultTransformer(Transformers.AliasToBean(typeof(Advance))).List<Advance>();
        }

        public IList<AvAdvanceDocument> FindAdvancDocumentByWorkFlowID(long workFlowID)
        {
            StringBuilder sql = new StringBuilder();

            sql.AppendLine(" select c.* ");
            sql.AppendLine(" from WorkFlow a inner join TADocument b ");
            sql.AppendLine(" on a.DocumentID = b.DocumentID ");
            sql.AppendLine(" inner join AvAdvanceDocument c ");
            sql.AppendLine(" on c.TADocumentID = b.TADocumentID ");
            sql.AppendLine(" where a.WorkFlowID = :WorkFlowID ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("WorkFlowID", typeof(long), workFlowID);
            queryParameterBuilder.FillParameters(query);

            query.AddEntity(typeof(AvAdvanceDocument));

            return query.List<AvAdvanceDocument>();
        }

        public AvAdvanceDocument FindAdvanceByWorkFlowID(long workFlowID)
        {
            StringBuilder sql = new StringBuilder();

            sql.AppendLine(" select c.* ");
            sql.AppendLine(" from WorkFlow a ");
            sql.AppendLine(" inner join AvAdvanceDocument c ");
            sql.AppendLine(" on c.DocumentID = a.DocumentID ");
            sql.AppendLine(" where a.WorkFlowID = :WorkFlowID ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("WorkFlowID", typeof(long), workFlowID);
            queryParameterBuilder.FillParameters(query);

            query.AddEntity(typeof(AvAdvanceDocument));

            return query.List<AvAdvanceDocument>()[0];
        }

        #region bool isDuplicateAdvanceFromTA(long tadocument,long requesterId)
        /// <summary>
        /// for check duplicate key ; create advance from ta
        /// </summary>
        /// <param name="tadocument"></param>
        /// <param name="requesterId"></param>
        /// <returns></returns>
        public IList<VoAdvanceFromTA> FindAdvanceFromTA(long tadocument)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" select advance.tadocumentid,doc.requesterid, advance.advanceid , ta.documentid");
            sqlBuilder.Append(" from AvAdvanceDocument advance ");
            sqlBuilder.Append(" inner join  TADocument ta ");
            sqlBuilder.Append(" on ta.TADocumentID = advance.TADocumentID ");
            sqlBuilder.Append(" inner join [Document] doc ");
            sqlBuilder.Append(" on doc.documentID = advance.documentID ");
            sqlBuilder.Append(" where advance.tadocumentid= :tadocument ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("tadocument", typeof(long), tadocument);
            queryParameterBuilder.FillParameters(query);
            query.AddScalar("TADocumentID", NHibernateUtil.Int64);
            query.AddScalar("RequesterID", NHibernateUtil.Int64);
            query.AddScalar("AdvanceID", NHibernateUtil.Int64);
            query.AddScalar("DocumentID", NHibernateUtil.Int64);

            return query.SetResultTransformer(Transformers.AliasToBean(typeof(VoAdvanceFromTA))).List<VoAdvanceFromTA>();
        }
        #endregion

        public IList<AvAdvanceDocument> FindAdvanceReferenceTAByTADocumentID(long taDocumentID)
        {
            return GetCurrentSession().CreateQuery("from AvAdvanceDocument where Active = 1 and isnull(TADocumentID,'') <> '' and TADocumentID = :TADocumentID ")
                .SetInt64("TADocumentID", taDocumentID)
                .List<AvAdvanceDocument>();
        }
        #region bool isSeeMessage(long documentID)
        public bool isSeeMessage(long documentID)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine(" SELECT  (case when count(*) > 0 then 1 else 0 end) as isSeeMessage");
            sqlBuilder.AppendLine(" FROM WorkFlow wf with(nolock) inner join");
            sqlBuilder.AppendLine(" Document d with(nolock)");
            sqlBuilder.AppendLine(" on wf.DocumentID = d.DocumentID and wf.Active = 1");
            sqlBuilder.AppendLine(" inner join WorkFlowState wfs with(nolock)");
            sqlBuilder.AppendLine(" on wfs.WorkFlowStateID = wf.CurrentState");
            sqlBuilder.AppendLine(" and wfs.WorkFlowTypeID = wf.WorkFlowTypeID");
            sqlBuilder.AppendLine(" and wfs.Ordinal >= 5 ");
            sqlBuilder.AppendLine(" WHERE wf.DocumentID = :documentID and wf.WorkFlowTypeID in (1,7)");
            sqlBuilder.AppendLine(" and (d.ApproverID = d.RequesterID or d.ApproverID = d.ReceiverID)");
            return GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString())
                .AddScalar("isSeeMessage", NHibernateUtil.Boolean)
                .SetInt64("documentID", documentID)
                .UniqueResult<Boolean>();
        }
        #endregion
        public int isMessage(long documentID)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine(" SELECT  (case when d.ApproverID = d.RequesterID and d.ApproverID <> d.ReceiverID then 1");
            sqlBuilder.AppendLine(" when d.ApproverID <> d.RequesterID and d.ApproverID = d.ReceiverID then 2");
            sqlBuilder.AppendLine(" when d.ApproverID = d.RequesterID and d.ApproverID = d.ReceiverID then 3");
            sqlBuilder.AppendLine(" else 0 end) as isMessage");
            sqlBuilder.AppendLine(" FROM WorkFlow wf with(nolock) inner join");
            sqlBuilder.AppendLine(" Document d with(nolock)");
            sqlBuilder.AppendLine(" on wf.DocumentID = d.DocumentID and wf.Active = 1");
            sqlBuilder.AppendLine(" inner join WorkFlowState wfs with(nolock)");
            sqlBuilder.AppendLine(" on wfs.WorkFlowStateID = wf.CurrentState");
            sqlBuilder.AppendLine(" and wfs.WorkFlowTypeID = wf.WorkFlowTypeID");
            sqlBuilder.AppendLine(" and wfs.Ordinal >= 5 ");
            sqlBuilder.AppendLine(" WHERE wf.DocumentID = :documentID and wf.WorkFlowTypeID in (1,7)");
            sqlBuilder.AppendLine(" and (d.ApproverID = d.RequesterID or d.ApproverID = d.ReceiverID)");
            return GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString())
                .AddScalar("isMessage", NHibernateUtil.Int32)
                .SetInt64("documentID", documentID)
                .UniqueResult<int>();
        }

        public IList<Advance> FindRemittanceAmountByAdvanceIDs(List<long> advanceIdList)
        {
            //  get Remittance Amount 
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT adv.AdvanceID as AdvanceID , adv.RemittanceAmount as RemittedAmountTHB, adv.RemittanceAmountMainCurrency as RemittanceAmountMainCurrency, adv.IsRepOffice as IsRepOffice ");
            sql.Append("FROM AvAdvanceDocument as adv ");
            sql.Append("WHERE adv.AdvanceID in (:advanceList) ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            query.SetParameterList("advanceList", advanceIdList);

            query.AddScalar("AdvanceID", NHibernateUtil.Int64);
            query.AddScalar("RemittedAmountTHB", NHibernateUtil.Double);
            query.AddScalar("RemittanceAmountMainCurrency", NHibernateUtil.Double);
            query.AddScalar("IsRepOffice", NHibernateUtil.Boolean);

            return query.SetResultTransformer(Transformers.AliasToBean(typeof(Advance))).List<Advance>();
        }

        public double GetExchangeRateMainCurrencyToTHB(IList<long> advanceIdList)
        { 
            double exchangeRate = 0;
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT SUM(ISNULL(adv.ExchangeRateMainToTHBCurrency,0)) as ExchangeRateMainToTHBCurrency ");
            sql.Append("FROM AvAdvanceDocument as adv ");
            sql.Append("WHERE adv.AdvanceID in (:advanceList) GROUP BY adv.AdvanceID");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            query.SetParameterList("advanceList", advanceIdList);

            query.AddScalar("ExchangeRateMainToTHBCurrency", NHibernateUtil.Double);

            exchangeRate = query.UniqueResult<double>() / advanceIdList.Count;

            return exchangeRate;
        }

        public double GetExchangeRateLocalCurrencyToMainCurrency(IList<long> advanceIdList)
        {
            double exchangeRate = 0;
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT SUM(ISNULL(adv.ExchangeRateForLocalCurrency,0)) as ExchangeRateLocalToMainCurrency ");
            sql.Append("FROM AvAdvanceDocument as adv ");
            sql.Append("WHERE adv.AdvanceID in (:advanceList) GROUP BY adv.AdvanceID");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            query.SetParameterList("advanceList", advanceIdList);

            query.AddScalar("ExchangeRateLocalToMainCurrency", NHibernateUtil.Double);

            exchangeRate = query.UniqueResult<double>() / advanceIdList.Count;

            return exchangeRate;
        }
    }
}
