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
    public class FixedAdvanceDocumentQuery : NHibernateQueryBase<FixedAdvanceDocument, long>, IFixedAdvanceDocumentQuery
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

        public bool FindFixedAdvanceOutStandingFromAleart(long requesterID, long CompanyID)
        {

            //ICriteria criteria = GetCurrentSession().CreateCriteria(typeof(FixedAdvanceDocument), "fav");
            StringBuilder sqlBuilder = new StringBuilder();
            ISQLQuery query;
            sqlBuilder.AppendLine("select a.EffectiveFromDate as effecttiveDateFrom ,a.EffectiveToDate as effecttiveDateTo ");
            sqlBuilder.AppendLine(" from FixedAdvanceDocument a inner join Document b on a.DocumentID = b.DocumentID ");
            sqlBuilder.AppendLine(" where b.requesterID = :requesterID and a.active = 1 ");
            sqlBuilder.AppendLine(" and b.CacheCurrentStateName = 'outstanding' and b.CompanyID = :companyID ");
            query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());

            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("requesterID", typeof(long), requesterID);
            queryParameterBuilder.AddParameterData("companyID", typeof(long), CompanyID);
            queryParameterBuilder.FillParameters(query);
            query.AddScalar("effecttiveDateFrom", NHibernateUtil.DateTime);
            query.AddScalar("effecttiveDateTo", NHibernateUtil.DateTime);
            IList<FixedAdvanceOutstanding> result = query.SetResultTransformer(Transformers.AliasToBean(typeof(FixedAdvanceOutstanding))).List<FixedAdvanceOutstanding>();

            bool flag = false;
            foreach (FixedAdvanceOutstanding row in result)
            {
                if (row.effecttiveDateFrom <= DateTime.Now && row.effecttiveDateTo > DateTime.Now)
                    flag = true;
            }
            return flag;
        }
        

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

        public FixedAdvanceDocument GetFixedAdvanceByDocumentID(long documentID)
        {
            ICriteria criteria = GetCurrentSession().CreateCriteria(typeof(FixedAdvanceDocument), "fav");
            criteria.Add(Expression.Eq("fav.DocumentID.DocumentID", documentID));
            return criteria.UniqueResult<FixedAdvanceDocument>();
        }

        public FixedAdvanceDocument GetFixedAdvanceByFixedAdvanceID(long fixedAdvanceID)
        {
            ICriteria criteria = GetCurrentSession().CreateCriteria(typeof(FixedAdvanceDocument), "fav");
            criteria.Add(Expression.Eq("fav.FixedAdvanceID", fixedAdvanceID));
            return criteria.UniqueResult<FixedAdvanceDocument>();
        }

        public IList<FixedAdvanceRefObjectValues> FindRefFixedAdvance(long comId, long userId, long requesterId, long docId, string seachType)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(" Select a.FixedAdvanceID,b.DocumentNo AS DocNo,a.NetAmount From [dbo].[FixedAdvanceDocument] a ");
            sql.AppendLine(" INNER JOIN [dbo].[Document] b ON a.DocumentID = b.DocumentID ");
            sql.AppendLine(" WHERE b.CompanyID = :comId AND b.RequesterID = :requesterId AND b.Active = 1 ");
            sql.AppendLine(" AND a.FixedAdvanceID <> :docId");
           
            if (seachType == "outstandingOnly")
            {
                sql.AppendLine(" AND b.CacheCurrentStateName = 'Outstanding' "); 
                if (requesterId != userId)
                {
                    sql.AppendLine(" AND (b.CreatorID = :userId ) ");
                }
            
            } 

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            query.SetParameter("comId", comId);
            query.SetParameter("requesterId", requesterId);
            query.SetParameter("docId", docId);
            if (seachType == "outstandingOnly")
            {
                if (requesterId != userId)
                {
                    query.SetParameter("userId", userId);

                }
            }
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.FillParameters(query);
            query.AddScalar("FixedAdvanceID", NHibernateUtil.Int64);
            query.AddScalar("DocNo", NHibernateUtil.String);
            query.AddScalar("NetAmount", NHibernateUtil.Double);
            return query.SetResultTransformer(Transformers.AliasToBean(typeof(FixedAdvanceRefObjectValues))).List<FixedAdvanceRefObjectValues>();
        }

        public IList<FixedAdvanceCanRefObjectValues> FindFixedAdvanceCanRef(long comId, long userId, long requesterId, long docId, long currentfixedid)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(" SELECT FixedAdvanceID,DocNo,NetAmount,RefCurrentStateName ");
            sql.AppendLine(" FROM ( SELECT a.FixedAdvanceID,b.DocumentNo AS DocNo,a.NetAmount,a.RefFixedAdvanceID,ref.FixedAdvanceIDRef ");
            sql.AppendLine(" ,ISNULL(ref.RefCurrentStateName,'') AS RefCurrentStateName FROM [dbo].[FixedAdvanceDocument] a ");
            sql.AppendLine(" INNER JOIN [dbo].[Document] b ON a.DocumentID = b.DocumentID ");
            sql.AppendLine(" LEFT JOIN ( SELECT f.FixedAdvanceID AS FixedAdvanceIDRef,d.CacheCurrentStateName AS RefCurrentStateName,f.RefFixedAdvanceID ");
            sql.AppendLine(" 	FROM [dbo].[FixedAdvanceDocument] f JOIN [dbo].[Document] d ON f.DocumentID = d.DocumentID ");
            sql.AppendLine(" 	WHERE d.CompanyID = :comId AND d.RequesterID = :requesterId AND d.CacheCurrentStateName <> 'Cancel' ) AS ref ON ref.RefFixedAdvanceID = a.FixedAdvanceID ");
            sql.AppendLine(" WHERE b.CompanyID = :comId AND (b.RequesterID = :requesterId ) ");
            if (requesterId != userId)
            {
                sql.AppendLine(" AND (b.CreatorID = :userId ) ");
                
            }
            sql.AppendLine(" AND b.Active = 1 AND b.CacheCurrentStateName = 'Outstanding' ) tb");
            sql.AppendLine("  WHERE FixedAdvanceID <> :docId ");
            sql.AppendLine(" AND ( FixedAdvanceIDRef = :docId OR RefCurrentStateName = '' )  ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            query.SetParameter("comId", comId);
            query.SetParameter("requesterId", requesterId);
            query.SetParameter("docId", docId);
            if (requesterId != userId)
            {
                query.SetParameter("userId", userId);

            }
            //query.SetParameter("currentfixedid", currentfixedid);
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.FillParameters(query);
            query.AddScalar("FixedAdvanceID", NHibernateUtil.Int64);
            query.AddScalar("DocNo", NHibernateUtil.String);
            query.AddScalar("NetAmount", NHibernateUtil.Double);
            query.AddScalar("RefCurrentStateName", NHibernateUtil.String);
            return query.SetResultTransformer(Transformers.AliasToBean(typeof(FixedAdvanceCanRefObjectValues))).List<FixedAdvanceCanRefObjectValues>();
        }

        public FixedAdvanceDocument FindNetAmountQuery(long docId)
        {
            ICriteria criteria = GetCurrentSession().CreateCriteria(typeof(FixedAdvanceDocument), "fav");
            criteria.Add(Expression.Eq("fav.FixedAdvanceID", docId));
            return criteria.UniqueResult<FixedAdvanceDocument>();
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

       
        public IList<FixedAdvanceOutstanding> FindFixedAdvanceOutstandingRequest(long requesterID,long companyID, long currentWorkFlowID, short languageID, int firstResult, int maxResults, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<FixedAdvanceOutstanding>(ScgeAccountingQueryProvider.FixedAdvanceDocumentQuery, "FindFixedAdvanceOutstandingQuery", new object[] { requesterID, companyID, currentWorkFlowID, languageID, false, sortExpression }, firstResult, maxResults, sortExpression);
        }

        public int CountFindFixedAdvanceOutstandingRequest(long requesterID, long companyID, long currentWorkFlowID, short languageID)
        {
            return NHibernateQueryHelper.CountByCriteria(ScgeAccountingQueryProvider.FixedAdvanceDocumentQuery, "FindFixedAdvanceOutstandingQuery", new object[] { requesterID, companyID, currentWorkFlowID, languageID, true, string.Empty });
        }


        #region ISQLQuery FindFixedAdvanceOutstandingQuery(long requesterID, long currentWorkFlowID, short languareID, bool isCount, string sortExpression)
        public ISQLQuery FindFixedAdvanceOutstandingQuery(long requesterID, long companyID, long currentWorkFlowID, short languageID, bool isCount, string sortExpression)
        {
            ISQLQuery query;
            if (!isCount)
            {
                StringBuilder sqlBuilder = new StringBuilder();

                sqlBuilder.AppendLine("select b.DocumentID,b.DocumentNo as fixedAdvanceNo ,b.Subject ,b.CacheCurrentStateName as fixedAdvanceStatus , a.EffectiveFromDate as effecttiveDateFrom,a.EffectiveToDate as effecttiveDateTo , a.Amount as amount,a.Objective , a.FixedAdvanceID as fixedAdvanceDocumentID ");
                sqlBuilder.AppendLine(" from FixedAdvanceDocument a inner join Document b on a.DocumentID = b.DocumentID ");
                sqlBuilder.AppendLine(" inner join [dbo].[WorkFlow] c on b.DocumentID = c.DocumentID ");
                sqlBuilder.AppendLine(" where b.requesterID = :requesterID and a.active = 1 AND c.WorkFlowID <> :currentWorkFlowID");
                //sqlBuilder.AppendLine(" and b.CacheCurrentStateName = 'outstanding' and b.CompanyID = :companyID ");
                sqlBuilder.AppendLine(" and b.CacheCurrentStateName = 'outstanding' ");
               

                if (string.IsNullOrEmpty(sortExpression))
                    sqlBuilder.AppendLine(" ORDER BY b.DocumentNo ");
                else
                    sqlBuilder.AppendLine(string.Format(" ORDER BY {0}", sortExpression));
                query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
                //query.SetParameterList("documentStatus", documentStatus);
                QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
                queryParameterBuilder.AddParameterData("requesterID", typeof(long), requesterID);
                //queryParameterBuilder.AddParameterData("companyID", typeof(long), companyID);
                queryParameterBuilder.AddParameterData("currentWorkFlowID", typeof(long), currentWorkFlowID);


                queryParameterBuilder.FillParameters(query);
                query.AddScalar("DocumentID", NHibernateUtil.Int32);
                query.AddScalar("fixedAdvanceNo", NHibernateUtil.String);
                query.AddScalar("subject", NHibernateUtil.String);
                query.AddScalar("fixedAdvanceStatus", NHibernateUtil.String);
                query.AddScalar("objective", NHibernateUtil.String);
                query.AddScalar("effecttiveDateFrom", NHibernateUtil.DateTime);
                query.AddScalar("effecttiveDateTo", NHibernateUtil.DateTime);
                query.AddScalar("amount", NHibernateUtil.Double);
                query.AddScalar("fixedAdvanceDocumentID", NHibernateUtil.Int32);
                query.SetResultTransformer(Transformers.AliasToBean(typeof(FixedAdvanceOutstanding)));
            }
            else
            {
                StringBuilder sqlBuilder = new StringBuilder();
                sqlBuilder.AppendLine("select count(b.DocumentID) as [count] ");
                sqlBuilder.AppendLine(" from FixedAdvanceDocument a inner join Document b on a.DocumentID = b.DocumentID ");
                sqlBuilder.AppendLine(" inner join [dbo].[WorkFlow] c on b.DocumentID = c.DocumentID ");
                sqlBuilder.AppendLine(" where b.requesterID = :requesterID and a.active = 1 AND c.WorkFlowID <> :currentWorkFlowID");
                sqlBuilder.AppendLine(" and b.CacheCurrentStateName = 'outstanding' and b.CompanyID = :companyID ");
                query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());

                QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
                queryParameterBuilder.AddParameterData("requesterID", typeof(long), requesterID);
                queryParameterBuilder.AddParameterData("companyID", typeof(long), companyID);
                queryParameterBuilder.AddParameterData("currentWorkFlowID", typeof(long), currentWorkFlowID);
                queryParameterBuilder.FillParameters(query);
                query.AddScalar("count", NHibernateUtil.Int32);
            }
            return query;
        }
        #endregion



        #region ForExpense
        public IList<FixedAdvanceOutstanding> FindFixedAdvanceOutstandingRequestForExpense(long requesterID, long companyID, long currentExpenseID, short languageID, int firstResult, int maxResults, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<FixedAdvanceOutstanding>(ScgeAccountingQueryProvider.FixedAdvanceDocumentQuery, "FindFixedAdvanceOutstandingQueryForExpense", new object[] { requesterID, companyID, currentExpenseID, languageID, false, sortExpression }, firstResult, maxResults, sortExpression);
        }

        public int CountFindFixedAdvanceOutstandingRequestForExpense(long requesterID, long companyID, long currentWorkFlowID, short languageID)
        {
            return NHibernateQueryHelper.CountByCriteria(ScgeAccountingQueryProvider.FixedAdvanceDocumentQuery, "FindFixedAdvanceOutstandingQueryForExpense", new object[] { requesterID, companyID, currentWorkFlowID, languageID, true, string.Empty });
        }

        public ISQLQuery FindFixedAdvanceOutstandingQueryForExpense(long requesterID, long companyID, long currentExpenseID, short languageID, bool isCount, string sortExpression)
        {
            ISQLQuery query;
            if (!isCount)
            {
                StringBuilder sqlBuilder = new StringBuilder();
                sqlBuilder.AppendLine(" select DocumentID,fixedAdvanceNo ,Subject ,fixedAdvanceStatus ");
                sqlBuilder.AppendLine(" , effecttiveDateFrom,effecttiveDateTo , amount,Objective ,fixedAdvanceDocumentID From ( ");

                sqlBuilder.AppendLine(" select b.DocumentID,b.DocumentNo as fixedAdvanceNo ,b.Subject ,e.displayName as fixedAdvanceStatus ");
                sqlBuilder.AppendLine(" , a.EffectiveFromDate as effecttiveDateFrom,a.EffectiveToDate as effecttiveDateTo , a.Amount as amount,a.Objective , a.FixedAdvanceID as fixedAdvanceDocumentID  ");
                sqlBuilder.AppendLine(" from FixedAdvanceDocument a inner join Document b on a.DocumentID = b.DocumentID  ");
                sqlBuilder.AppendLine(" inner join [dbo].[WorkFlow] c on b.DocumentID = c.DocumentID  ");
                sqlBuilder.AppendLine(" inner join workflowstate d on c.currentstate = d.workflowstateID   ");
                sqlBuilder.AppendLine(" inner join workflowstatelang e on d.workflowstateID = e.workflowstateID and e.languageID = :languageID  ");
                sqlBuilder.AppendLine(" where b.requesterID = :requesterID  and a.active = 1 AND GETDATE() <= DATEADD(MONTH,1,a.EffectiveToDate) ");
                sqlBuilder.AppendLine(" and b.CacheCurrentStateName = 'outstanding' and b.CompanyID = :companyID ");
                sqlBuilder.AppendLine(" union  ");
                sqlBuilder.AppendLine(" select b.DocumentID,b.DocumentNo as fixedAdvanceNo ,b.Subject ,e.displayName as fixedAdvanceStatus ");
                sqlBuilder.AppendLine("  , a.EffectiveFromDate as effecttiveDateFrom,a.EffectiveToDate as effecttiveDateTo , a.Amount as amount,a.Objective , a.FixedAdvanceID as fixedAdvanceDocumentID  ");
                sqlBuilder.AppendLine(" from FixedAdvanceDocument a inner join Document b on a.DocumentID = b.DocumentID  ");
                sqlBuilder.AppendLine("  inner join [dbo].[WorkFlow] c on b.DocumentID = c.DocumentID  ");
                sqlBuilder.AppendLine("  inner join workflowstate d on c.currentstate = d.workflowstateID   ");
                sqlBuilder.AppendLine("  inner join workflowstatelang e on d.workflowstateID = e.workflowstateID and e.languageID = :languageID  ");
                sqlBuilder.AppendLine("  inner join FnExpenseDocument fe on fe.FixedAdvanceID = a.FixedAdvanceID and fe.ExpenseID = :currentexpenseid ) tb ");
                
                if (string.IsNullOrEmpty(sortExpression))
                    sqlBuilder.AppendLine(" ORDER BY fixedAdvanceNo ");
                else
                    sqlBuilder.AppendLine(string.Format(" ORDER BY {0}", sortExpression));
                query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
                //query.SetParameterList("documentStatus", documentStatus);
                QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
                queryParameterBuilder.AddParameterData("requesterID", typeof(long), requesterID);
                queryParameterBuilder.AddParameterData("companyID", typeof(long), companyID);
                queryParameterBuilder.AddParameterData("languageID", typeof(short), languageID);
                queryParameterBuilder.AddParameterData("currentexpenseid", typeof(long), currentExpenseID);


                queryParameterBuilder.FillParameters(query);
                query.AddScalar("DocumentID", NHibernateUtil.Int32);
                query.AddScalar("fixedAdvanceNo", NHibernateUtil.String);
                query.AddScalar("subject", NHibernateUtil.String);
                query.AddScalar("fixedAdvanceStatus", NHibernateUtil.String);
                query.AddScalar("objective", NHibernateUtil.String);
                query.AddScalar("effecttiveDateFrom", NHibernateUtil.DateTime);
                query.AddScalar("effecttiveDateTo", NHibernateUtil.DateTime);
                query.AddScalar("amount", NHibernateUtil.Double);
                query.AddScalar("fixedAdvanceDocumentID", NHibernateUtil.Int32);
                query.SetResultTransformer(Transformers.AliasToBean(typeof(FixedAdvanceOutstanding)));
            }
            else
            {
                StringBuilder sqlBuilder = new StringBuilder();
                sqlBuilder.AppendLine("select count(*) as [count] ");
                sqlBuilder.AppendLine(" from ( select b.DocumentID,b.DocumentNo as fixedAdvanceNo ,b.Subject ,e.displayName as fixedAdvanceStatus ");
                sqlBuilder.AppendLine(" , a.EffectiveFromDate as effecttiveDateFrom,a.EffectiveToDate as effecttiveDateTo , a.Amount as amount,a.Objective , a.FixedAdvanceID as fixedAdvanceDocumentID  ");
                sqlBuilder.AppendLine(" from FixedAdvanceDocument a inner join Document b on a.DocumentID = b.DocumentID  ");
                sqlBuilder.AppendLine(" inner join [dbo].[WorkFlow] c on b.DocumentID = c.DocumentID  ");
                sqlBuilder.AppendLine(" inner join workflowstate d on c.currentstate = d.workflowstateID   ");
                sqlBuilder.AppendLine(" inner join workflowstatelang e on d.workflowstateID = e.workflowstateID and e.languageID = :languageID  ");
                sqlBuilder.AppendLine(" where b.requesterID = :requesterID  and a.active = 1 AND GETDATE() <= DATEADD(MONTH,1,a.EffectiveToDate)  ");
                sqlBuilder.AppendLine(" and b.CacheCurrentStateName = 'outstanding' and b.CompanyID = :companyID ");
                sqlBuilder.AppendLine(" union  ");
                sqlBuilder.AppendLine(" select b.DocumentID,b.DocumentNo as fixedAdvanceNo ,b.Subject ,e.displayName as fixedAdvanceStatus ");
                sqlBuilder.AppendLine("  , a.EffectiveFromDate as effecttiveDateFrom,a.EffectiveToDate as effecttiveDateTo , a.Amount as amount,a.Objective , a.FixedAdvanceID as fixedAdvanceDocumentID  ");
                sqlBuilder.AppendLine(" from FixedAdvanceDocument a inner join Document b on a.DocumentID = b.DocumentID  ");
                sqlBuilder.AppendLine("  inner join [dbo].[WorkFlow] c on b.DocumentID = c.DocumentID  ");
                sqlBuilder.AppendLine("  inner join workflowstate d on c.currentstate = d.workflowstateID   ");
                sqlBuilder.AppendLine("  inner join workflowstatelang e on d.workflowstateID = e.workflowstateID and e.languageID = :languageID  ");
                sqlBuilder.AppendLine("  inner join FnExpenseDocument fe on fe.FixedAdvanceID = a.FixedAdvanceID and fe.ExpenseID = :currentexpenseid  ) tb ");

                query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
                QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
                queryParameterBuilder.AddParameterData("requesterID", typeof(long), requesterID);
                queryParameterBuilder.AddParameterData("companyID", typeof(long), companyID);
                queryParameterBuilder.AddParameterData("languageID", typeof(short), languageID);
                queryParameterBuilder.AddParameterData("currentexpenseid", typeof(long), currentExpenseID);
                queryParameterBuilder.FillParameters(query);
                query.AddScalar("count", NHibernateUtil.Int32);
            }
            return query;
        }
        #endregion
    }
}
