using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using NHibernate.Transform;

using SS.Standard.Data.NHibernate.QueryCreator;
using SS.Standard.Data.NHibernate.QueryDao;

using SCG.eAccounting.DTO;
using SCG.eAccounting.DTO.ValueObject;
using NHibernate.Expression;
using SS.DB.Query;


namespace SCG.eAccounting.Query.Hibernate
{
    public class SCGDocumentQuery : NHibernateQueryBase<SCGDocument, long>, ISCGDocumentQuery
    {
        #region ISCGDocumentQuery Members
        private const string TravellByFR = "F";
        private const string ZoneTypeDM = "DM";
        private const string ZoneTypeFR = "FR";

        public SCGDocument GetSCGDocumentByDocumentID(long documentID)
        {
            return GetCurrentSession().CreateQuery("from SCGDocument where DocumentID = :DocumentID")
                .SetInt64("DocumentID", documentID)
                .UniqueResult<SCGDocument>();
        }

        public string GetDocumentCurrentStateName(short languageID, long documentID)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" select  wsl.DisplayName");
            sqlBuilder.Append(" from    Document d  inner join WorkFlow w on d.DocumentID = w.DocumentID");
            sqlBuilder.Append("                     inner join WorkFlowStateLang wsl on w.CurrentState = wsl.WorkFlowStateID and wsl.LanguageID = :LanguageID");
            sqlBuilder.Append(" where   d.DocumentID = :DocumentID");

            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            parameterBuilder.AddParameterData("LanguageID", typeof(short), languageID);
            parameterBuilder.AddParameterData("DocumentID", typeof(long), documentID);

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            parameterBuilder.FillParameters(query);
            query.AddScalar("DisplayName", NHibernateUtil.String);

            return query.UniqueResult<string>();
        }

        public ISQLQuery FindSCGDocumentByCriteria(SearchCriteria criteria, short languageId)
        {
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            parameterBuilder.AddParameterData("LanguageID", typeof(short), languageId);

            StringBuilder strQuery = new StringBuilder();
            strQuery.Append(" select (row_number() over (order by d.DocumentID,d.DocumentNo,dt.DocumentTypeName, cl.FirstName, cl.LastName)) as Seq, ");
            strQuery.Append(" d.DocumentID as DocumentID, d.DocumentNo as DocumentNo, dt.DocumentTypeName as DocumentTypeName, ");
            strQuery.Append(" cl.FirstName as FirstName, cl.LastName as LastName ");
            strQuery.Append(" from Document d ");
            strQuery.Append(" left join DocumentType dt on d.DocumentTypeID = dt.DocumentTypeID ");
            strQuery.Append(" left join SuUser c on c.UserID = d.CreatorID ");
            strQuery.Append(" left join SuUserLang cl on cl.UserID = c.UserID and cl.LanguageID = :LanguageID ");

            #region whereClause by criteria
            StringBuilder whereClauseBuilder = new StringBuilder();
            if (!string.IsNullOrEmpty(criteria.BudgetYear))
            {

            }
            if ((!string.IsNullOrEmpty(criteria.DocTypeFrom)) || (!string.IsNullOrEmpty(criteria.DocTypeTo)))
            {
                strQuery.Append(" and (dt.DocumentTypeName between :DocTypeFrom and :DocTypeTo) ");
                parameterBuilder.AddParameterData("DocTypeFrom", typeof(string), criteria.DocTypeFrom);
                parameterBuilder.AddParameterData("DocTypeTo", typeof(string), criteria.DocTypeTo);
            }
            if ((!string.IsNullOrEmpty(criteria.CostCenterFrom)) || (!string.IsNullOrEmpty(criteria.CostCenterTo)))
            {

            }
            if ((!string.IsNullOrEmpty(criteria.CreDateFrom)) || (!string.IsNullOrEmpty(criteria.CreDateTo)))
            {
                strQuery.Append(" and (d.CreDate between :CreDateFrom and :CreDateTo) ");
                parameterBuilder.AddParameterData("CreDateFrom", typeof(string), criteria.CreDateFrom);
                parameterBuilder.AddParameterData("CreDateTo", typeof(string), criteria.CreDateTo);
            }
            if ((!string.IsNullOrEmpty(criteria.DocNoFrom)) || (!string.IsNullOrEmpty(criteria.DocNoTo)))
            {
                strQuery.Append(" and (d.DocumentNo between :DocNoFrom and :DocNoTo) ");
                parameterBuilder.AddParameterData("DocNoFrom", typeof(string), criteria.DocNoFrom);
                parameterBuilder.AddParameterData("DocNoTo", typeof(string), criteria.DocNoTo);
            }
            if (criteria.AmountFrom != null)
            {

            }
            if (criteria.AmountTo != null)
            {

            }

            if (!string.IsNullOrEmpty(whereClauseBuilder.ToString()))
            {
                strQuery.Append(String.Format(" where 1=1 {0} ", whereClauseBuilder.ToString()));
            }

            #endregion

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());
            parameterBuilder.FillParameters(query);
            query.AddScalar("Seq", NHibernateUtil.Int32)
                .AddScalar("DocumentID", NHibernateUtil.Int64)
                .AddScalar("DocumentNo", NHibernateUtil.String)
                .AddScalar("DocumentTypeName", NHibernateUtil.String)
                .AddScalar("FirstName", NHibernateUtil.String)
                .AddScalar("LastName", NHibernateUtil.String);

            query.SetResultTransformer(Transformers.AliasToBean(typeof(SCG.eAccounting.DTO.ValueObject.SearchResultData)));

            return query;
        }


        #region public ISQLQuery FindByDocumentCriteria(bool isCount, string sortExpression, string documentNo, string description, long companyID, long requesterID, string travelBy, Enum queryFor, Enum withoutDocument, long? taDocumentID)
        public ISQLQuery FindByDocumentCriteria(bool isCount, string sortExpression, string documentNo, string description, long companyID, long requesterID, string travelBy, Enum queryFor, Enum withoutDocument, long? taDocumentID)
        {
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            StringBuilder sqlBuilder = new StringBuilder();
            if (!isCount)
            {
                sqlBuilder.Append(" SELECT TOP 100 PERCENT ");
                sqlBuilder.Append("     WorkFlow.WorkflowID        AS WorkflowID, ");
                sqlBuilder.Append("     Document.DocumentID        AS DocumentID, ");
                sqlBuilder.Append("     Document.DocumentNo        AS DocumentNo, ");
                sqlBuilder.Append("     Document.Subject           AS Subject, ");
                sqlBuilder.Append("     TADocument.TADocumentID    AS TADocumentID, ");
                sqlBuilder.Append("     TADocument.FromDate        AS FromDate, ");
                sqlBuilder.Append("     TADocument.ToDate          AS ToDate, ");
                // ========= Add missing column by Anuwat S on 19/04/2009 =========
                sqlBuilder.Append("     TADocument.IsBusinessPurpose        AS IsBusinessPurpose, ");
                sqlBuilder.Append("     TADocument.IsTrainningPurpose       AS IsTrainningPurpose, ");
                sqlBuilder.Append("     TADocument.IsOtherPurpose           AS IsOtherPurpose, ");
                sqlBuilder.Append("     TADocument.OtherPurposeDescription  AS OtherPurposeDescription, ");
                sqlBuilder.Append("     TADocument.Ticketing                AS Ticketing, ");
                // ======= End add missing column by Anuwat S on 19/04/2009 =======
                sqlBuilder.Append("     TADocument.Province        AS Province, ");
                sqlBuilder.Append("     TADocument.Country         AS Country, ");
                sqlBuilder.Append("     DocumentType.DocumentTypeName AS DocumentTypeName ");
            }
            else
            {
                sqlBuilder.Append(" SELECT COUNT(*) AS TACount ");
            }

            sqlBuilder.Append(" FROM Document ");
            sqlBuilder.Append("     INNER JOIN TADocument ON ");
            sqlBuilder.Append("			Document.DocumentID =  TADocument.DocumentID AND ");
            sqlBuilder.Append("         Document.Active     = 1 ");
            sqlBuilder.Append("     INNER JOIN DocumentType ON ");
            sqlBuilder.Append("         Document.DocumentTypeID =  DocumentType.DocumentTypeID ");
            sqlBuilder.Append("     INNER JOIN WorkFlow ON ");
            sqlBuilder.Append("         WorkFlow.DocumentID =  Document.DocumentID ");
            sqlBuilder.Append(" WHERE ");
            sqlBuilder.Append("     (Document.DocumentTypeID = 2 or Document.DocumentTypeID = 8)");
            sqlBuilder.Append("     AND WorkFlow.CurrentState = 17 ");

            if (!string.IsNullOrEmpty(documentNo))
            {
                sqlBuilder.Append(" AND Document.DocumentNo LIKE :DocumentNo ");
                parameterBuilder.AddParameterData("DocumentNo", typeof(string), "%" + documentNo + "%");
            }
            if (!string.IsNullOrEmpty(description))
            {
                sqlBuilder.Append(" AND Document.Subject LIKE :Description ");
                parameterBuilder.AddParameterData("Description", typeof(string), "%" + description + "%");
            }
            if (companyID != 0)
            {
                sqlBuilder.Append(" AND Document.CompanyID = :CompanyID ");
                parameterBuilder.AddParameterData("CompanyID", typeof(long), companyID);
            }
            if (requesterID != 0)
            {
                // Find Requester that was member of TA
                //sqlBuilder.Append(" AND Document.RequesterID = :RequesterID ");
                sqlBuilder.Append(" AND :RequesterID IN (SELECT UserID FROM TADocumentTraveller WHERE TADocumentTraveller.TADocumentID = TADocument.TADocumentID) ");
                parameterBuilder.AddParameterData("RequesterID", typeof(long), requesterID);
            }
            if (travelBy != null && travelBy != "")
            {
                sqlBuilder.Append(" AND TADocument.TravelBy = :TravelBy ");
                parameterBuilder.AddParameterData("TravelBy", typeof(string), travelBy);
            }
            if (taDocumentID != null && taDocumentID > 0)
            {
                sqlBuilder.Append(" AND TADocument.TADocumentID = :TADocumentID ");
                parameterBuilder.AddParameterData("TADocumentID", typeof(long), taDocumentID);
            }

            //parameterBuilder.AddParameterData("CompanyID", typeof(long), companyID);
            //parameterBuilder.AddParameterData("RequesterID", typeof(long), requesterID);
            if (!GetDocumentListFor.General.Equals(queryFor) && !GetDocumentListFor.Advance.Equals(queryFor) && !SearchTAby.General.Equals(withoutDocument))
            {
                // If TA has Advance document(s), the Advance document(s) must be on Outstanding state
                sqlBuilder.AppendLine(" AND (");
                sqlBuilder.AppendLine("     TADocument.TADocumentID IN ( ");
                sqlBuilder.AppendLine("         SELECT	AvAdvanceDocument.TADocumentID ");
                sqlBuilder.AppendLine("         FROM	AvAdvanceDocument INNER JOIN ");
                sqlBuilder.AppendLine(" 	    	[Document] ON AvAdvanceDocument.DocumentID = [Document].DocumentID INNER JOIN ");
                sqlBuilder.AppendLine("		    	WorkFlow ON [Document].DocumentID = WorkFlow.DocumentID ");
                sqlBuilder.AppendLine("         WHERE WorkFlow.CurrentState = 25 ");
                sqlBuilder.AppendLine("	        AND [Document].CompanyID = :CompanyID ");
                sqlBuilder.AppendLine("	        AND [Document].RequesterID = :RequesterID ");
                //sqlBuilder.AppendLine("	        AND AdvanceType = 'FR' ");

                // Remittance document will query
                // - TA has not been reference on Remittance document
                // - TA has not been reference on Expense document
                if (GetDocumentListFor.Remittance.Equals(queryFor))
                {
                    sqlBuilder.AppendLine("         AND NOT EXISTS (SELECT AdvanceID FROM FnRemittanceAdvance WHERE AdvanceID = AvAdvanceDocument.AdvanceID)");
                    sqlBuilder.AppendLine("         AND NOT EXISTS (SELECT AdvanceID FROM FnExpenseAdvance WHERE AdvanceID = AvAdvanceDocument.AdvanceID)");
                }
                // Expense document will query
                // - TA has not been reference on Expense document
                if (GetDocumentListFor.Expense.Equals(queryFor))
                {
                    sqlBuilder.AppendLine("         AND NOT EXISTS (SELECT AdvanceID FROM FnExpenseAdvance WHERE AdvanceID = AvAdvanceDocument.AdvanceID)");
                    if (travelBy.Equals(TravellByFR))
                    {
                        sqlBuilder.Append(@" 
                            AND( 
                                    EXISTS (
	                                    SELECT AdvanceID FROM FnRemittanceAdvance remittedAdv
		                                    INNER JOIN FnRemittance remitted ON remitted.RemittanceID = remittedAdv.RemittanceID
		                                    INNER JOIN Document doc ON remitted.DocumentID = doc.DocumentID
		                                    INNER JOIN Workflow wf ON wf.DocumentID = remitted.DocumentID
		                                    INNER JOIN WorkFlowState wfs 
		                                    ON wfs.WorkFlowStateID = wf.CurrentState
		                                    WHERE wfs.Name in ('Complete','Cancel') AND AdvanceID = AvAdvanceDocument.AdvanceID
                                    ) OR
                                    NOT EXISTS (
	                                    SELECT AdvanceID FROM FnRemittanceAdvance remittedAdv
		                                    INNER JOIN FnRemittance remitted ON remitted.RemittanceID = remittedAdv.RemittanceID
		                                    INNER JOIN Document doc ON remitted.DocumentID = doc.DocumentID
		                                    INNER JOIN Workflow wf ON wf.DocumentID = remitted.DocumentID
		                                    INNER JOIN WorkFlowState wfs 
		                                    ON wfs.WorkFlowStateID = wf.CurrentState
		                                    WHERE wfs.Name not in ('Complete','Cancel') AND AdvanceID = AvAdvanceDocument.AdvanceID
                                    )
                                ) "
                        );
                    }
                }
                sqlBuilder.AppendLine("         ) ");

                // In case query TA for Expense
                // - TA has not been reference on Expense document (exclude Cancelled Expense document)
                // - TA has not been reference on Advance document
                if (GetDocumentListFor.Expense.Equals(queryFor))
                {
                    sqlBuilder.AppendLine(" OR ( ");
                    sqlBuilder.AppendLine("    NOT EXISTS ( SELECT     FnExpenseDocument.TADocumentID ");
                    sqlBuilder.AppendLine("                 FROM         WorkFlow INNER JOIN ");
                    sqlBuilder.AppendLine("                     [Document] ON WorkFlow.DocumentID = [Document].DocumentID INNER JOIN ");
                    sqlBuilder.AppendLine("                     FnExpenseDocument ON [Document].DocumentID = FnExpenseDocument.DocumentID ");
                    sqlBuilder.AppendLine("                 WHERE TADocumentID = TADocument.TADocumentID AND WorkFlow.CurrentState <> 35");
                    sqlBuilder.AppendLine("                 )");
                    sqlBuilder.AppendLine("     AND NOT EXISTS (SELECT TADocumentID FROM AvAdvanceDocument WHERE TADocumentID = TADocument.TADocumentID) ");
                    sqlBuilder.AppendLine("     ) ");
                }
                sqlBuilder.AppendLine("	) ");
            }

            if (SearchTAby.WithoutAdvance.Equals(withoutDocument))
            {
                sqlBuilder.AppendLine(" AND NOT EXISTS (SELECT TADocumentID FROM AvAdvanceDocument WHERE TADocumentID = TADocument.TADocumentID)");
            }
            else if (SearchTAby.WithoutRemittance.Equals(withoutDocument))
            {
                sqlBuilder.AppendLine(" AND NOT EXISTS (SELECT TADocumentID FROM FnRemittance WHERE TADocumentID = TADocument.TADocumentID)");
            }
            else if (SearchTAby.WithoutExpense.Equals(withoutDocument))
            {
                sqlBuilder.AppendLine(" AND NOT EXISTS (SELECT TADocumentID FROM FnExpenseDocument WHERE TADocumentID = TADocument.TADocumentID)");
            }

            if (!isCount)
            {
                if (string.IsNullOrEmpty(sortExpression))
                    sqlBuilder.AppendLine("ORDER BY Document.DocumentNo,Document.Subject,TADocument.FromDate,TADocument.ToDate ");
                else
                    sqlBuilder.AppendLine(string.Format(" ORDER BY {0}", sortExpression));
            }
            #region Old source code
            /*
            }
            else
            {
                sqlBuilder.Append(" SELECT COUNT(*) AS TACount ");
                sqlBuilder.Append(" FROM Document ");
                sqlBuilder.Append("     INNER JOIN TADocument ON ");
                sqlBuilder.Append("         Document.DocumentID =  TADocument.DocumentID AND ");
                sqlBuilder.Append("         Document.Active     = 1 ");
                sqlBuilder.Append("     INNER JOIN DocumentType ON ");
                sqlBuilder.Append("         Document.DocumentTypeID =  DocumentType.DocumentTypeID ");
                sqlBuilder.Append("     INNER JOIN WorkFlow ON ");
                sqlBuilder.Append("         WorkFlow.DocumentID = Document.DocumentID ");
				sqlBuilder.Append(" WHERE ");
                sqlBuilder.Append("     Document.DocumentTypeID = 2 ");
				sqlBuilder.Append("     AND WorkFlow.CurrentState = 17 ");

                if (!string.IsNullOrEmpty(documentNo))
                {
                    sqlBuilder.Append(" AND Document.DocumentNo LIKE :DocumentNo ");
                    parameterBuilder.AddParameterData("DocumentNo", typeof(string), "%" + documentNo + "%");
                }
                if (!string.IsNullOrEmpty(documentTypeID))
                {
                    sqlBuilder.Append(" AND Document.DocumentTypeID = :DocumentTypeID ");
                    parameterBuilder.AddParameterData("DocumentTypeID", typeof(short), short.Parse(documentTypeID));
                }
                if (!string.IsNullOrEmpty(description))
                {
                    sqlBuilder.Append(" AND Document.Subject LIKE :Description ");
                    parameterBuilder.AddParameterData("Description", typeof(string), "%" + description + "%");
                }
                if (companyID != 0)
                {
                    sqlBuilder.Append(" AND Document.CompanyID = :CompanyID ");
                    parameterBuilder.AddParameterData("CompanyID", typeof(long), companyID);
                }
                if (requesterID != 0)
                {
                    sqlBuilder.Append(" AND Document.RequesterID = :RequesterID ");
                    parameterBuilder.AddParameterData("RequesterID", typeof(long), requesterID);
                }
                if (travelBy != null && travelBy != "")
                {
                    sqlBuilder.Append(" AND TADocument.TravelBy = :TravelBy ");
                    parameterBuilder.AddParameterData("TravelBy", typeof(string), travelBy);
                }
            }*/
            #endregion Old source code

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());

            parameterBuilder.FillParameters(query);

            if (!isCount)
            {
                query.AddScalar("DocumentID", NHibernateUtil.Int64);
                query.AddScalar("TADocumentID", NHibernateUtil.Int64);
                query.AddScalar("DocumentNo", NHibernateUtil.String);
                query.AddScalar("Subject", NHibernateUtil.String);
                query.AddScalar("FromDate", NHibernateUtil.DateTime);
                query.AddScalar("ToDate", NHibernateUtil.DateTime);
                // ========= Add missing column by Anuwat S on 19/04/2009 =========
                query.AddScalar("WorkflowID", NHibernateUtil.Int64);
                query.AddScalar("IsBusinessPurpose", NHibernateUtil.Boolean);
                query.AddScalar("IsTrainningPurpose", NHibernateUtil.Boolean);
                query.AddScalar("IsOtherPurpose", NHibernateUtil.Boolean);
                query.AddScalar("OtherPurposeDescription", NHibernateUtil.String);
                query.AddScalar("Ticketing", NHibernateUtil.String);
                // ======= End add missing column by Anuwat S on 19/04/2009 =======
                query.AddScalar("Province", NHibernateUtil.String);
                query.AddScalar("Country", NHibernateUtil.String);
                query.AddScalar("DocumentTypeName", NHibernateUtil.String);


                query.SetResultTransformer(Transformers.AliasToBean(typeof(TADocumentObj)));
            }
            else
            {
                query.AddScalar("TACount", NHibernateUtil.Int32);
                query.UniqueResult();
            }
            return query;
        }
        #endregion public ISQLQuery FindByDocumentCriteria(bool isCount, string sortExpression, string documentNo, string description, long companyID, long requesterID, string travelBy, Enum queryFor, Enum withoutDocument, long? taDocumentID)

        public enum GetDocumentListFor
        {
            General,
            Advance,
            Remittance,
            Expense
        }

        public enum SearchTAby
        {
            General,
            WithoutAdvance,
            WithoutRemittance,
            WithoutExpense
        }

        #region public IList<TADocumentObj> GetDocumentList(int firstResult, int maxResult, string sortExpression, string documentNo, string description, long companyID, long requesterID, string travelBy, Enum queryFor, Enum withoutDocument, long? taDocumentID)
        public IList<TADocumentObj> GetDocumentList(int firstResult, int maxResult, string sortExpression, string documentNo, string description, long companyID, long requesterID, string travelBy, Enum queryFor, Enum withoutDocument, long? taDocumentID)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<TADocumentObj>(ScgeAccountingQueryProvider.SCGDocumentQuery, "FindByDocumentCriteria", new object[] { false, sortExpression, documentNo, description, companyID, requesterID, travelBy, queryFor, withoutDocument, taDocumentID }, firstResult, maxResult, sortExpression);
        }
        #endregion public IList<TADocumentObj> GetDocumentList(int firstResult, int maxResult, string sortExpression, string documentNo, string description, long companyID, long requesterID, string travelBy, Enum queryFor, Enum withoutDocument, long? taDocumentID)

        #region public int CountByDocumentCriteria(string documentNo, string documentTypeID, string description, long companyID, long requesterID, string travelBy, Enum queryFor, Enum withoutDocument, long? taDocumentID)
        public int CountByDocumentCriteria(string documentNo, string description, long companyID, long requesterID, string travelBy, Enum queryFor, Enum withoutDocument, long? taDocumentID)
        {
            return NHibernateQueryHelper.CountByCriteria(ScgeAccountingQueryProvider.SCGDocumentQuery, "FindByDocumentCriteria", new object[] { true, string.Empty, documentNo, description, companyID, requesterID, travelBy, queryFor, withoutDocument, taDocumentID });
        }
        #endregion public int CountByDocumentCriteria(string documentNo, string description, long companyID, long requesterID, string travelBy, Enum queryFor, Enum withoutDocument, long? taDocumentID)

        #region public IList<TranslatedListItem>FindDocumentTypeCriteria()
        public IList<TranslatedListItem> FindDocumentTypeCriteria()
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine(" SELECT ");
            sqlBuilder.AppendLine("     DocumentTypeID   AS ID, ");
            sqlBuilder.AppendLine("     DocumentTypeName AS Symbol ");
            sqlBuilder.AppendLine(" FROM ");
            sqlBuilder.AppendLine("     DocumentType ");
            sqlBuilder.AppendLine(" WHERE Active = 1 ");
            sqlBuilder.AppendLine(" AND DocumentTypeID <> 9 "); //EHRExpenseDocument = ExpenseDomesticDocument
            //sqlBuilder.AppendLine(" ORDER BY DocumentTypeName ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());

            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.FillParameters(query);

            query.AddScalar("ID", NHibernateUtil.Int16);
            query.AddScalar("Symbol", NHibernateUtil.String);
            query.SetResultTransformer(Transformers.AliasToBean(typeof(TranslatedListItem)));

            return query.List<TranslatedListItem>();
        }
        #endregion public IList<TranslatedListItem>FindDocumentTypeCriteria()

        #region public IList<TranslatedListItem>FindDocumentTypeCriteriaNoTA()
        public IList<TranslatedListItem> FindDocumentTypeCriteriaNoTA()
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine(" SELECT ");
            sqlBuilder.AppendLine("     DocumentTypeID   AS ID, ");
            sqlBuilder.AppendLine("     DocumentTypeName AS Symbol ");
            sqlBuilder.AppendLine(" FROM ");
            sqlBuilder.AppendLine("     DocumentType ");
            sqlBuilder.AppendLine(" WHERE Active = 1 ");
            sqlBuilder.AppendLine(" AND DocumentTypeID not in (2,8) "); //EHRExpenseDocument = ExpenseDomesticDocument
            //sqlBuilder.AppendLine(" ORDER BY DocumentTypeName ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());

            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.FillParameters(query);

            query.AddScalar("ID", NHibernateUtil.Int16);
            query.AddScalar("Symbol", NHibernateUtil.String);
            query.SetResultTransformer(Transformers.AliasToBean(typeof(TranslatedListItem)));

            return query.List<TranslatedListItem>();
        }
        #endregion public IList<TranslatedListItem>FindDocumentTypeCriteriaNoTA()

        #region public IList<TranslatedListItem>FindStatusCriteria(short languageID, int documentTypeID)
        public IList<TranslatedListItem> FindStatusCriteria(short languageID, int documentTypeID)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();

            //get all display name.
            sqlBuilder.AppendLine(" SELECT Distinct ");
            //sqlBuilder.AppendLine(" c.WorkFlowStateID   AS ID,"); 
            sqlBuilder.AppendLine(" d.DisplayName AS Symbol ");
            sqlBuilder.AppendLine(" from WorkFlowTypeDocumentType a inner join DocumentType b ");
            sqlBuilder.AppendLine(" on a.DocumentTypeID = b.DocumentTypeID ");
            sqlBuilder.AppendLine(" inner join WorkFlowState c ");
            sqlBuilder.AppendLine(" on a.WorkFlowTypeID = c.WorkFlowTypeID ");
            sqlBuilder.AppendLine(" inner join WorkFlowStateLang d ");
            sqlBuilder.AppendLine(" on c.WorkFlowStateID = d.WorkFlowStateID ");
            sqlBuilder.AppendLine(" and a.Active = 1 ");
            sqlBuilder.AppendLine(" and d.LanguageID = :LanguageID ");
            //sqlBuilder.AppendLine(" WHERE ");
            //sqlBuilder.AppendLine(" c.Name in ('Draft','Cancel','WaitInitial','WaitApprove','WaitVerify','WaitRemittance') "); 
            //sqlBuilder.AppendLine(" and a.DocumentTypeID = :DocumentTypeID ");            
            sqlBuilder.Append(" Order by d.DisplayName ");

            queryParameterBuilder.AddParameterData("LanguageID", typeof(long), languageID);
            //queryParameterBuilder.AddParameterData("DocumentTypeID", typeof(int), documentTypeID);

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());

            queryParameterBuilder.FillParameters(query);

            //query.AddScalar("ID", NHibernateUtil.Int16);
            query.AddScalar("Symbol", NHibernateUtil.String);
            query.SetResultTransformer(Transformers.AliasToBean(typeof(TranslatedListItem)));

            return query.List<TranslatedListItem>();
        }
        #endregion public IList<TranslatedListItem>FindStatusCriteria(short languageID, int documentTypeID)

        #region  public IList<TADocumentObj> FindByDocumentIdentity(long documentId)
        public IList<TADocumentObj> FindByDocumentIdentity(long documentId)
        {
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.Append(" SELECT ");
            sqlBuilder.Append("     WorkFlow.WorkflowID        AS WorkflowID, ");
            sqlBuilder.Append("     Document.DocumentID        AS DocumentID ,");
            sqlBuilder.Append("     Document.DocumentNo        AS DocumentNo ,");
            sqlBuilder.Append("     Document.Subject           AS Subject ,");
            sqlBuilder.Append("     TADocument.TADocumentID    AS TADocumentID ,");
            sqlBuilder.Append("     TADocument.FromDate        AS FromDate ,");
            sqlBuilder.Append("     TADocument.ToDate          AS ToDate ,");
            // ========= Add missing column by Anuwat S on 19/04/2009 =========
            sqlBuilder.Append("     TADocument.IsBusinessPurpose        AS IsBusinessPurpose, ");
            sqlBuilder.Append("     TADocument.IsTrainningPurpose       AS IsTrainningPurpose, ");
            sqlBuilder.Append("     TADocument.IsOtherPurpose           AS IsOtherPurpose, ");
            sqlBuilder.Append("     TADocument.OtherPurposeDescription  AS OtherPurposeDescription, ");
            sqlBuilder.Append("     TADocument.Ticketing                AS Ticketing, ");
            // ======= End add missing column by Anuwat S on 19/04/2009 =======
            sqlBuilder.Append("     TADocument.Province        AS Province ,");
            sqlBuilder.Append("     TADocument.Country         AS Country ,");
            //=========================Add query travelby by oum 27/04/2009 ======
            sqlBuilder.Append("     TADocument.TravelBy         AS TravelBy, ");
            sqlBuilder.Append("     DocumentType.DocumentTypeName AS DocumentTypeName ");
            sqlBuilder.Append(" FROM TADocument ");
            sqlBuilder.Append("     INNER JOIN Document ON ");
            sqlBuilder.Append("         TADocument.DocumentID =  Document.DocumentID AND ");
            sqlBuilder.Append("         TADocument.Active     = 1 ");
            sqlBuilder.Append("     INNER JOIN DocumentType ON ");
            sqlBuilder.Append("         Document.DocumentTypeID =  DocumentType.DocumentTypeID ");
            sqlBuilder.Append("     INNER JOIN WorkFlow ON ");
            sqlBuilder.Append("         Document.DocumentID =  WorkFlow.DocumentID AND ");
            sqlBuilder.Append("         WorkFlow.WorkFlowTypeID = 4 AND ");
            sqlBuilder.Append("         WorkFlow.CurrentState = 17 ");
            sqlBuilder.Append(" WHERE TADocument.DocumentID = :DocumentID ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            parameterBuilder.AddParameterData("DocumentID", typeof(Int64), documentId);
            parameterBuilder.FillParameters(query);
            query.AddScalar("DocumentID", NHibernateUtil.Int64);
            query.AddScalar("TADocumentID", NHibernateUtil.Int64);
            query.AddScalar("DocumentNo", NHibernateUtil.String);
            query.AddScalar("Subject", NHibernateUtil.String);
            query.AddScalar("FromDate", NHibernateUtil.DateTime);
            query.AddScalar("ToDate", NHibernateUtil.DateTime);
            // ========= Add missing column by Anuwat S on 19/04/2009 =========
            query.AddScalar("WorkflowID", NHibernateUtil.Int64);
            query.AddScalar("IsBusinessPurpose", NHibernateUtil.Boolean);
            query.AddScalar("IsTrainningPurpose", NHibernateUtil.Boolean);
            query.AddScalar("IsOtherPurpose", NHibernateUtil.Boolean);
            query.AddScalar("OtherPurposeDescription", NHibernateUtil.String);
            query.AddScalar("Ticketing", NHibernateUtil.String);
            // ======= End add missing column by Anuwat S on 19/04/2009 =======
            query.AddScalar("Province", NHibernateUtil.String);
            query.AddScalar("Country", NHibernateUtil.String);
            query.AddScalar("DocumentTypeName", NHibernateUtil.String);
            query.AddScalar("TravelBy", NHibernateUtil.String);

            IList<TADocumentObj> list =
               query.SetResultTransformer(Transformers.AliasToBean(typeof(TADocumentObj))).List<TADocumentObj>();
            return list;
        }
        #endregion  public IList<TADocumentObj> FindByDocumentIdentity(long documentId)

        #endregion

        #region ISCGDocumentQuery for Export Members

        public IList<ExportDocumentImage> FindDocumentImage(string status, string sapCode)
        {
            StringBuilder stringBuilder = new StringBuilder();
            QueryParameterBuilder paramBuilder = new QueryParameterBuilder();
            stringBuilder.AppendLine("SELECT doc.DocumentID, ");
            stringBuilder.AppendLine("doc.PostingDate as DocumentDate, ");
            stringBuilder.AppendLine("doc.DocumentNo as DocNumber, ");
            stringBuilder.AppendLine("dip.STATUS, DIP.FiDocNumber as FI_DOC, ");
            stringBuilder.AppendLine("comp.CompanyCode, ");
            stringBuilder.AppendLine("bap.Doc_Kind as ImageDocumentType ");
            stringBuilder.AppendLine("FROM [Document] doc ");
            stringBuilder.AppendLine("INNER JOIN BAPIACHE09 bap ");
            stringBuilder.AppendLine("ON bap.DOC_ID = doc.DocumentID ");
            stringBuilder.AppendLine("LEFT  JOIN DbDocumentImagePosting DIP ");
            stringBuilder.AppendLine("ON DOC.DocumentID = DIP.DocumentID ");
            stringBuilder.AppendLine("LEFT JOIN dbCompany comp ");
            stringBuilder.AppendLine("ON  comp.CompanyID = doc.companyID ");
            stringBuilder.AppendLine("LEFT JOIN WorkFlow wf ");
            stringBuilder.AppendLine("ON doc.DocumentID = wf.DocumentID ");
            stringBuilder.AppendLine("LEFT JOIN WorkFlowState wfs ");
            stringBuilder.AppendLine("ON wf.CurrentState = wfs.WorkflowStateID ");
            if (status == "GF")
                stringBuilder.AppendLine("WHERE dip.status <> 'S' ");
            else
            {
                stringBuilder.AppendLine("WHERE ISNULL(DIP.STATUS,'N') = :status  ");
                paramBuilder.AddParameterData("status", typeof(string), status);
            }
            stringBuilder.AppendLine("AND wfs.NAME in ('Complete','OutStanding') AND bap.Doc_Seq='M' ");
            stringBuilder.AppendLine("AND comp.SapCode = :sapCode ");




            ISQLQuery query = GetCurrentSession().CreateSQLQuery(stringBuilder.ToString());
            paramBuilder.AddParameterData("sapCode", typeof(string), sapCode);
            paramBuilder.FillParameters(query);
            query.AddScalar("DocumentID", NHibernateUtil.Int64)
                 .AddScalar("DocumentDate", NHibernateUtil.Date)
                .AddScalar("FI_DOC", NHibernateUtil.String)
                .AddScalar("DocNumber", NHibernateUtil.String)
                              .AddScalar("STATUS", NHibernateUtil.String)
           .AddScalar("CompanyCode", NHibernateUtil.String)
                     .AddScalar("ImageDocumentType", NHibernateUtil.String);

            query.SetResultTransformer(Transformers.AliasToBean(typeof(ExportDocumentImage)));
            return query.List<ExportDocumentImage>();
        }

        #endregion

        #region ISCGDocumentQuery Inbox

        #region public ISQLQuery FindInboxEmployeeCriteria(SearchCriteria criteria, bool isCount, string sortExpression)
        public ISQLQuery FindInboxEmployeeCriteria(SearchCriteria criteria, bool isCount, string sortExpression)
        {
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            StringBuilder sqlBuilder = new StringBuilder();

            #region Select
            if (!isCount)
            {
                sqlBuilder.AppendLine(" select ");
                sqlBuilder.AppendLine("     DocumentID, ");
                sqlBuilder.AppendLine("     WorkflowID, ");
                if (criteria.FlagQuery.Equals("Draft"))
                {
                    sqlBuilder.AppendLine("     CreateDate, ");
                    sqlBuilder.AppendLine("     DocumentTypeName, ");
                }
                sqlBuilder.AppendLine("     RequestNo, ");
                sqlBuilder.AppendLine("     DocumentTypeID, ");
                sqlBuilder.AppendLine("     Subject, ");
                sqlBuilder.AppendLine("     CreatorID, ");
                sqlBuilder.AppendLine("     CreatorName, ");
                sqlBuilder.AppendLine("     RequesterID, ");
                sqlBuilder.AppendLine("     RequesterName, ");
                if (criteria.FlagQuery.Equals("Employee"))
                {
                    sqlBuilder.AppendLine("     DocumentStatus, ");
                    sqlBuilder.AppendLine("     ReferenceNo, ");
                    sqlBuilder.AppendLine("     RequestDate, ");
                    sqlBuilder.AppendLine("     WorkFlowStateID, ");
                    sqlBuilder.AppendLine("     WorkFlowTypeID, ");
                }
                sqlBuilder.AppendLine("     Amount ");
            }
            else
            {
                sqlBuilder.AppendLine(" select count(*) as inboxCount ");
            }
            #endregion Select
            #region Inbox Employee
            if (criteria.FlagQuery.Equals("Employee"))
            {
                sqlBuilder.AppendLine(" from ");
                sqlBuilder.AppendLine(" ( ");
                sqlBuilder.AppendLine(" select ");
                sqlBuilder.AppendLine("     a.DocumentID as DocumentID, ");
                sqlBuilder.AppendLine("     b.WorkflowID as WorkflowID, ");
                sqlBuilder.AppendLine("     a.DocumentNo as RequestNo, ");
                sqlBuilder.AppendLine("     isnull(a.ReferenceNo,'') as ReferenceNo, ");
                sqlBuilder.AppendLine("     a.DocumentTypeID as DocumentTypeID, ");
                sqlBuilder.AppendLine("     a.DocumentDate as RequestDate, ");
                sqlBuilder.AppendLine("     a.Subject as Subject, ");
                sqlBuilder.AppendLine("     a.CreatorID as CreatorID, ");
                sqlBuilder.AppendLine("     g.EmployeeName as CreatorName, ");
                sqlBuilder.AppendLine("     a.RequesterID as RequesterID, ");
                sqlBuilder.AppendLine("     h.EmployeeName as RequesterName, ");
                sqlBuilder.AppendLine("     e.WorkFlowStateID as WorkFlowStateID, ");
                sqlBuilder.AppendLine("     e.WorkFlowTypeID as WorkFlowTypeID, ");
                sqlBuilder.AppendLine("     case ");
                sqlBuilder.AppendLine("         when a.DocumentTypeID = '1' or  a.DocumentTypeID = '5' then i.Amount "); //AdvanceDocument
                sqlBuilder.AppendLine("         when a.DocumentTypeID = '3' or  a.DocumentTypeID = '7' then j.DifferenceAmount "); //ExpenseDocument
                sqlBuilder.AppendLine("         when a.DocumentTypeID = '4' then k.TotalAmount "); //RemiitanceDocument
                sqlBuilder.AppendLine("         else 0 ");
                sqlBuilder.AppendLine("     end as Amount, ");
                sqlBuilder.AppendLine("     e.Name as DocumentStatus ");
                sqlBuilder.AppendLine(" from ");
                sqlBuilder.AppendLine("     Document a  with (nolock)");
                sqlBuilder.AppendLine("     inner join WorkFlow b  with (nolock)");
                sqlBuilder.AppendLine("     on a.DocumentID = b.DocumentID ");
                sqlBuilder.AppendLine("     inner join WorkFlowStateEventPermission c  with (nolock)");
                sqlBuilder.AppendLine("     on b.WorkFlowID = c.WorkFlowID ");
                sqlBuilder.AppendLine("         and c.UserID = :UserID ");
                sqlBuilder.AppendLine("     inner join WorkFlowStateEvent d  with (nolock)");
                sqlBuilder.AppendLine("     on c.WorkFlowStateEventID = d.WorkFlowStateEventID ");
                sqlBuilder.AppendLine("     inner join WorkFlowState e  with (nolock)");
                sqlBuilder.AppendLine("     on d.WorkFlowStateID = e.WorkFlowStateID ");
                sqlBuilder.AppendLine("     inner join WorkFlowType f  with (nolock)");
                sqlBuilder.AppendLine("     on b.WorkFlowTypeID = f.WorkFlowTypeID ");
                sqlBuilder.AppendLine("     left outer join SuUser g  with (nolock)");
                sqlBuilder.AppendLine("     on a.CreatorID = g.UserID ");
                sqlBuilder.AppendLine("     left outer join SuUser h  with (nolock)");
                sqlBuilder.AppendLine("     on a.RequesterID = h.UserID ");
                sqlBuilder.AppendLine("     left outer join AvAdvanceDocument i  with (nolock)");
                sqlBuilder.AppendLine("     on a.DocumentID = i.DocumentID ");
                sqlBuilder.AppendLine("     left outer join FnExpenseDocument j  with (nolock)");
                sqlBuilder.AppendLine("     on a.DocumentID = j.DocumentID ");
                sqlBuilder.AppendLine("     left outer join FnRemittance k  with (nolock)");
                sqlBuilder.AppendLine("     on a.DocumentID = k.DocumentID ");
                sqlBuilder.AppendLine("     and a.Active = 1 ");
                sqlBuilder.AppendLine(" where a.IsContainDocumentNo = 1 ");
                sqlBuilder.AppendLine("     and d.Name in ('Send','Approve') ");

                parameterBuilder.AddParameterData("UserID", typeof(long), criteria.UserID);
                if (criteria.UserID > 0)
                {
                    sqlBuilder.AppendLine("     and (a.CreatorID = :UserID ");
                    sqlBuilder.AppendLine("         or a.RequesterID = :UserID ");
                    sqlBuilder.AppendLine("         or c.UserID = :UserID) ");
                }
                if (!string.IsNullOrEmpty(criteria.RequestNo))
                {
                    sqlBuilder.AppendLine(" and a.DocumentNo Like :RequestNo ");
                    parameterBuilder.AddParameterData("RequestNo", typeof(string), "%" + criteria.RequestNo + "%");
                }
                if (!string.IsNullOrEmpty(criteria.RequestDateFrom))
                {
                    sqlBuilder.AppendLine(" and convert(varchar,a.DocumentDate,103) >= convert(varchar,:RequestDate,103) ");
                    parameterBuilder.AddParameterData("RequestDate", typeof(string), criteria.RequestDateFrom);
                }
                if (!string.IsNullOrEmpty(criteria.RequestDateTo))
                {
                    sqlBuilder.AppendLine(" and convert(varchar,a.DocumentDate,103) <= convert(varchar,:RequestDate,103) ");
                    parameterBuilder.AddParameterData("RequestDate", typeof(string), criteria.RequestDateTo);
                }
                if (criteria.CreatorID > 0)
                {
                    sqlBuilder.AppendLine(" and a.CreatorID = :CreatorID ");
                    parameterBuilder.AddParameterData("CreatorID", typeof(long), criteria.CreatorID);
                }
                if (criteria.RequesterID > 0)
                {
                    sqlBuilder.AppendLine(" and a.RequesterID = :RequesterID ");
                    parameterBuilder.AddParameterData("RequesterID", typeof(long), criteria.RequesterID);
                }
                if (criteria.DocumentTypeID > 0)
                {
                    sqlBuilder.AppendLine(" and a.DocumentTypeID = :DocumentTypeID ");
                    parameterBuilder.AddParameterData("DocumentTypeID", typeof(Int32), criteria.DocumentTypeID);
                }
                if (!string.IsNullOrEmpty(criteria.DocumentStatus))
                {
                    sqlBuilder.AppendLine(" and e.Name = :Name ");
                    parameterBuilder.AddParameterData("Name", typeof(string), criteria.DocumentStatus);
                }
            }
            #endregion Inbox Employee
            #region Inbox Draft
            else
            {
                sqlBuilder.AppendLine(" from ");
                sqlBuilder.AppendLine(" ( ");
                sqlBuilder.AppendLine(" select ");
                sqlBuilder.AppendLine("     a.DocumentID as DocumentID, ");
                sqlBuilder.AppendLine("     b.WorkflowID as WorkflowID, ");
                sqlBuilder.AppendLine("     isnull(nullif(a.DocumentNo,''),'N/A') as RequestNo, ");
                //sqlBuilder.AppendLine("     '' as ReferenceNo, ");
                sqlBuilder.AppendLine("     a.DocumentTypeID as DocumentTypeID, ");
                //sqlBuilder.AppendLine("     a.DocumentDate as RequestDate, ");
                sqlBuilder.AppendLine("     a.Subject as Subject, ");
                sqlBuilder.AppendLine("     a.CreatorID as CreatorID, ");
                sqlBuilder.AppendLine("     d.EmployeeName as CreatorName, ");
                sqlBuilder.AppendLine("     a.RequesterID as RequesterID, ");
                sqlBuilder.AppendLine("     e.EmployeeName as RequesterName, ");
                sqlBuilder.AppendLine("     case ");
                sqlBuilder.AppendLine("         when a.DocumentTypeID = '1' or  a.DocumentTypeID = '5' then f.Amount "); //AdvanceDocument
                sqlBuilder.AppendLine("         when a.DocumentTypeID = '3' or  a.DocumentTypeID = '7' then g.DifferenceAmount "); //ExpenseDocument
                sqlBuilder.AppendLine("         when a.DocumentTypeID = '4' then h.TotalAmount "); //RemiitanceDocument
                sqlBuilder.AppendLine("         else 0 ");
                sqlBuilder.AppendLine("     end as Amount, ");
                //sqlBuilder.AppendLine("     c.Name as DocumentStatus ");
                sqlBuilder.AppendLine("     a.CreDate as CreateDate, ");
                sqlBuilder.AppendLine("     i.DocumentTypeName as DocumentTypeName ");
                sqlBuilder.AppendLine(" from ");
                sqlBuilder.AppendLine("     Document a with (nolock) inner join WorkFlow b with (nolock) ");
                sqlBuilder.AppendLine("     on a.DocumentID = b.DocumentID ");
                sqlBuilder.AppendLine("     inner join DocumentType i with (nolock) ");
                sqlBuilder.AppendLine("     on a.DocumentTypeID = i.DocumentTypeID ");
                sqlBuilder.AppendLine("     inner join WorkFlowState c with (nolock) ");
                sqlBuilder.AppendLine("     on c.WorkFlowStateID = b.CurrentState ");
                sqlBuilder.AppendLine("     left outer join SuUser d with (nolock) ");
                sqlBuilder.AppendLine("     on a.CreatorID = d.UserID ");
                sqlBuilder.AppendLine("     left outer join SuUser e with (nolock) ");
                sqlBuilder.AppendLine("     on a.RequesterID = e.UserID ");
                sqlBuilder.AppendLine("     left outer join AvAdvanceDocument f with (nolock) ");
                sqlBuilder.AppendLine("     on a.DocumentID = f.DocumentID ");
                sqlBuilder.AppendLine("     left outer join FnExpenseDocument g with (nolock) ");
                sqlBuilder.AppendLine("     on a.DocumentID = g.DocumentID ");
                sqlBuilder.AppendLine("     left outer join FnRemittance h with (nolock) ");
                sqlBuilder.AppendLine("     on a.DocumentID = h.DocumentID ");
                sqlBuilder.AppendLine("     and a.Active = 1 ");
                sqlBuilder.AppendLine(" where isnull(a.DocumentNo,'') = '' ");
                sqlBuilder.AppendLine("     and c.Name = 'Draft' ");

                if (criteria.UserID > 0)
                {
                    sqlBuilder.AppendLine("     and a.CreatorID = :UserID ");
                    parameterBuilder.AddParameterData("UserID", typeof(long), criteria.UserID);
                }

            }
            sqlBuilder.Append("     ) temp ");
            #endregion Inbox Draft
            #region Order by
            if (!isCount)
            {
                if (string.IsNullOrEmpty(sortExpression))
                {
                    if (criteria.FlagQuery.Equals("Employee"))
                    {
                        sqlBuilder.AppendLine("ORDER BY RequestDate Desc, DocumentID,RequestNo,ReferenceNo,DocumentTypeID,Subject,CreatorID,CreatorName,RequesterID,RequesterName,WorkFlowStateID,WorkFlowTypeID,Amount ");
                    }
                    else //Draft
                    {
                        sqlBuilder.AppendLine("ORDER BY CreateDate Desc ,DocumentID,RequestNo,DocumentTypeID,Subject,CreatorID,CreatorName,RequesterID,RequesterName,Amount ");
                    }
                }
                else
                {
                    sqlBuilder.AppendLine(string.Format(" ORDER BY {0}", sortExpression));
                }
            }
            #endregion Order by

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            parameterBuilder.FillParameters(query);

            if (!isCount)
            {
                query.AddScalar("DocumentID", NHibernateUtil.Int64);
                query.AddScalar("WorkflowID", NHibernateUtil.Int64);
                query.AddScalar("RequestNo", NHibernateUtil.String);
                if (criteria.FlagQuery.Equals("Draft"))
                {
                    query.AddScalar("CreateDate", NHibernateUtil.DateTime);
                    query.AddScalar("DocumentTypeName", NHibernateUtil.String);
                }
                if (criteria.FlagQuery.Equals("Employee"))
                {
                    query.AddScalar("DocumentStatus", NHibernateUtil.String);
                    query.AddScalar("WorkFlowStateID", NHibernateUtil.Int32);
                    query.AddScalar("WorkFlowTypeID", NHibernateUtil.Int32);
                    query.AddScalar("ReferenceNo", NHibernateUtil.String);
                    query.AddScalar("RequestDate", NHibernateUtil.DateTime);
                }
                query.AddScalar("DocumentTypeID", NHibernateUtil.Int64);
                query.AddScalar("Subject", NHibernateUtil.String);
                query.AddScalar("CreatorID", NHibernateUtil.Int64);
                query.AddScalar("CreatorName", NHibernateUtil.String);
                query.AddScalar("RequesterID", NHibernateUtil.Int64);
                query.AddScalar("RequesterName", NHibernateUtil.String);
                query.AddScalar("Amount", NHibernateUtil.Double);

                query.SetResultTransformer(Transformers.AliasToBean(typeof(SearchResultData)));
            }
            else
            {
                query.AddScalar("inboxCount", NHibernateUtil.Int32);
                //query.UniqueResult();
            }
            return query;
        }
        #endregion public ISQLQuery FindInboxEmployeeCriteria(SearchCriteria criteria, bool isCount, string sortExpression)

        #region public IList<SearchResultData> GetCriteriaList(SearchCriteria criteria, int firstResult, int maxResult, string sortExpression)
        public IList<SearchResultData> GetCriteriaList(SearchCriteria criteria, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<SearchResultData>(ScgeAccountingQueryProvider.SCGDocumentQuery, "FindInboxEmployeeCriteria", new object[] { criteria, false, sortExpression }, firstResult, maxResult, sortExpression);
        }
        #endregion public IList<SearchResultData> GetCriteriaList(SearchCriteria criteria, int firstResult, int maxResult, string sortExpression)

        #region public int CountCriteria(SearchCriteria criteria)
        public int CountCriteria(SearchCriteria criteria)
        {
            return NHibernateQueryHelper.CountByCriteria(ScgeAccountingQueryProvider.SCGDocumentQuery, "FindInboxEmployeeCriteria", new object[] { criteria, true, string.Empty });
        }
        #endregion public int CountCriteria(SearchCriteria criteria)

        #region public int GetWorkStateEvent(int workFlowStateID, string name)
        public int GetWorkStateEvent(int workFlowStateID, string name)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine(" select  WorkFlowStateEventID ");
            sqlBuilder.AppendLine(" from    WorkFlowStateEvent ");
            sqlBuilder.AppendLine(" where   WorkFlowStateID = :WorkFlowStateID");
            sqlBuilder.AppendLine(" and     Name = :Name");

            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            parameterBuilder.AddParameterData("WorkFlowStateID", typeof(Int32), workFlowStateID);
            parameterBuilder.AddParameterData("Name", typeof(string), name);
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            parameterBuilder.FillParameters(query);
            query.AddScalar("WorkFlowStateEventID", NHibernateUtil.Int32);

            return int.Parse(query.UniqueResult().ToString());
        }
        #endregion public int GetWorkStateEvent(int workFlowStateID, string name)

        #region public IList<SearchResultData> FindDraftCriteria(long userID)
        public IList<SearchResultData> FindDraftCriteria(long userID)
        {
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.AppendLine(" select ");
            sqlBuilder.AppendLine("     a.DocumentID as DocumentID, ");
            sqlBuilder.AppendLine("     'N/A' as RequestNo, ");
            sqlBuilder.AppendLine("     isnull(a.ReferenceNo,'') as ReferenceNo, ");
            sqlBuilder.AppendLine("     a.DocumentTypeID as DocumentTypeID, ");
            sqlBuilder.AppendLine("     a.DocumentDate as RequestDate, ");
            sqlBuilder.AppendLine("     a.Subject as Subject, ");
            sqlBuilder.AppendLine("     a.CreatorID as CreatorID, ");
            sqlBuilder.AppendLine("     e.UserName as CreatorName, ");
            sqlBuilder.AppendLine("     a.RequesterID as RequesterID, ");
            sqlBuilder.AppendLine("     e.UserName as RequesterName, ");
            sqlBuilder.AppendLine("     case ");
            sqlBuilder.AppendLine("         when a.DocumentTypeID = '1' or  a.DocumentTypeID = '5' then f.Amount "); //AdvanceDocument
            sqlBuilder.AppendLine("         when a.DocumentTypeID = '3' or  a.DocumentTypeID = '7' then g.TotalExpense "); //ExpenseDocument
            sqlBuilder.AppendLine("         when a.DocumentTypeID = '4' then h.TotalAmount "); //RemiitanceDocument
            sqlBuilder.AppendLine("         else 0 ");
            sqlBuilder.AppendLine("     end as Amount, ");
            sqlBuilder.AppendLine("     c.Name as DocumentStatus ");
            sqlBuilder.AppendLine(" from ");
            sqlBuilder.AppendLine("     Document a inner join WorkFlow b ");
            sqlBuilder.AppendLine("     on a.DocumentID = b.DocumentID ");
            sqlBuilder.AppendLine("     inner join WorkFlowState c ");
            sqlBuilder.AppendLine("     on c.WorkFlowStateID = b.CurrentState ");
            sqlBuilder.AppendLine("     left outer join SuUser d ");
            sqlBuilder.AppendLine("     on a.CreatorID = d.UserID ");
            sqlBuilder.AppendLine("     left outer join SuUser e ");
            sqlBuilder.AppendLine("     on a.RequesterID = e.UserID ");
            sqlBuilder.AppendLine("     left outer join AvAdvanceDocument f ");
            sqlBuilder.AppendLine("     on a.DocumentID = f.DocumentID ");
            sqlBuilder.AppendLine("     left outer join FnExpenseDocument g ");
            sqlBuilder.AppendLine("     on a.DocumentID = g.DocumentID ");
            sqlBuilder.AppendLine("     left outer join FnRemittance h ");
            sqlBuilder.AppendLine("     on a.DocumentID = h.DocumentID ");
            sqlBuilder.AppendLine("     and a.Active = 1 ");
            sqlBuilder.AppendLine(" where a.IsContainDocumentNo = 1 ");
            sqlBuilder.AppendLine("     and c.Name = 'Draft' ");

            if (userID > 0)
            {
                sqlBuilder.AppendLine("     and (a.CreatorID = :UserID ");
                sqlBuilder.AppendLine("         or a.RequesterID = :UserID ");
                sqlBuilder.AppendLine("         or a.ReceiverID = :UserID) ");
                parameterBuilder.AddParameterData("UserID", typeof(long), userID);
            }

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            parameterBuilder.FillParameters(query);

            query.AddScalar("DocumentID", NHibernateUtil.Int64);
            query.AddScalar("RequestNo", NHibernateUtil.String);
            query.AddScalar("ReferenceNo", NHibernateUtil.String);
            query.AddScalar("DocumentTypeID", NHibernateUtil.Int64);
            query.AddScalar("RequestDate", NHibernateUtil.DateTime);
            query.AddScalar("Subject", NHibernateUtil.String);
            query.AddScalar("CreatorID", NHibernateUtil.Int64);
            query.AddScalar("CreatorName", NHibernateUtil.String);
            query.AddScalar("RequesterID", NHibernateUtil.Int64);
            query.AddScalar("RequesterName", NHibernateUtil.String);
            query.AddScalar("Amount", NHibernateUtil.Double);
            query.AddScalar("DocumentStatus", NHibernateUtil.String);
            query.SetResultTransformer(Transformers.AliasToBean(typeof(SearchResultData)));

            return query.List<SearchResultData>();
        }
        #endregion public IList<SearchResultData> FindDraftCriteria(long userID)

        #region public ISQLQuery FindInboxAccountantPaymentCriteria(SearchCriteria criteria, bool isCount, string sortExpression)
        public ISQLQuery FindInboxAccountantPaymentCriteria(SearchCriteria criteria, bool isCount, string sortExpression)
        {
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();

            string select = string.Empty;
            string from = string.Empty;
            string where = string.Empty;
            string sqlQuery = string.Empty;

            #region Search
            #region Select
            select = @" select Distinct isnull(a.CacheAttachment,'0') as CountAtm,
						isnull(a.CacheBoxID,'') as BoxID,
						(case when ISNULL(a.CacheAttachment, 0) = 0 and ISNULL(a.CacheBoxID,'') ='' THEN 0 
						when ISNULL(a.CacheBoxID,'') <> '' and ISNULL(a.CacheAttachment, 0) = 1  THEN 1
						when ISNULL(a.CacheAttachment, 0) = 1 THEN 3 ELSE 2	END) AS Attachment,
                        a.DocumentID as DocumentID, 
                        a.CacheWorkflowID as WorkflowID, 
                        isnull(nullif(a.DocumentNo,''),'N/A') as RequestNo, 
                        isnull(a.ReferenceNo,'') as ReferenceNo, 
                        a.DocumentTypeID as DocumentTypeID, 
                        a.DocumentDate as RequestDate, 
                        a.Subject as Subject, 
                        a.CreatorID as CreatorID, 
                        a.CacheCreatorName as CreatorName, 
                        a.RequesterID as RequesterID, 
                        a.CacheRequesterName as RequesterName, 
                        a.ApproverID as ApproverID, 
                        a.CacheCurrentState as WorkFlowStateID, 
                        a.CacheCurrentWorkFlowTypeID as WorkFlowTypeID, 
                        isnull(a.CacheAmountTHB,0) as amount,
                        isnull(a.CacheAmountMainCurrency,0) as AmountMainCurrency,
                        isnull(a.CacheAmountLocalCurrency,0) as AmountLocalCurrency,
                        l.DisplayName as DocumentStatus, 
                        a.CreDate as CreateDate,
                        a.CacheDocumentTypeName as DocumentTypeName,
                        a.CacheCurrentStateName as StateName,
                        a.ApproveDate as ApproveDate, 
                        a.ReceiveDocumentDate as ReceiveDocumentDate ";
                        

            #endregion Select
            #region From
            #region Inbox
            if (criteria.FlagJoin.Equals("Inbox"))
            {
                #region From
                from = @" from 
                        WorkFlowStateEventPermission c with (nolock) ";

                from += " left join SuUserRole with (nolock) on SuUserRole.RoleID = c.RoleID and (SuUserRole.UserID = :UserID or c.UserID = :UserID) ";
                if (criteria.MutipleApprove)
                {
                    from += " inner join SuRole with (nolock) on SuUserRole.RoleID = SuRole.RoleID ";
                }
                from += @" inner loop join Document a with (nolock) on a.CacheWorkflowID = c.WorkFlowID AND c.CacheIsdirect = 1 ";
                 if (criteria.FlagSearch.Equals("Employee"))
                {
                    from += " and c.CacheTaskgroup = 1 ";
                }
                if (criteria.FlagSearch.Equals("Accountant"))
                {
                    from += " and c.CacheTaskgroup = 2 ";
                }
                if (criteria.FlagSearch.Equals("Payment"))
                {
                    from += " and c.CacheTaskgroup = 3 ";
                }
                from += @" left join WorkFlowStateLang l with (nolock) 
						on l.WorkFlowStateID = a.CacheCurrentState and l.LanguageID = :languageID ";
                #endregion From
                where = @"  where a.Active=1 ";
                #region Where
                if ((criteria.Role.Equals(string.Empty) && criteria.UserID > 0) || criteria.UserID > 0)
                {
                    if (criteria.SelcetseHrFrom == true)
                    {
                        where += " and a.DocumentNo like 'ehr%'";
                    }
                    string imageOptionCriteria = string.Empty;

                    for (int i = 0; i < criteria.ImageOption.Count; i++)
                    {
                        if (criteria.ImageOption[i] == ImageOptionCriteria.Image)
                        {
                            imageOptionCriteria += " (ISNULL(a.CacheAttachment,0) = 1) and ISNULL(a.CacheBoxID,'') = ''";
                        }
                        else if (criteria.ImageOption[i] == ImageOptionCriteria.HardCopy)
                        {
                            imageOptionCriteria += " (ISNULL(a.CacheBoxID,'') <> '') and ISNULL(a.CacheAttachment, 0) = 0 ";
                        }
                        else if (criteria.ImageOption[i] == ImageOptionCriteria.ImageHardCopy)
                        {
                            imageOptionCriteria += " (ISNULL(a.CacheBoxID,'') <> '') ";
                        }

                        if (criteria.ImageOption.Count > 1 && i < criteria.ImageOption.Count - 1)
                        {
                            imageOptionCriteria += " or ";
                        }
                    }

                    if (criteria.ImageOption.Count > 0)
                    {
                        where += " and (" + imageOptionCriteria + ") ";
                    }

                    //where += " and (EXISTS (SELECT SuUserRole.RoleID FROM SuUserRole with (nolock) ";
                    //if (criteria.MutipleApprove)
                    //{
                    //    where += " inner join SuRole with (nolock) on SuUserRole.RoleID = SuRole.RoleID ";
                    //}
                    // where += " WHERE SuUserRole.UserID = :UserID AND SuUserRole.RoleID = c.RoleID ";
                    if (criteria.MutipleApprove)
                    {
                        if (criteria.FlagSearch.Equals("Accountant"))
                        {
                            where += " and SuRole.AllowMultipleApproveAccountant = 1 ";
                        }
                        if (criteria.FlagSearch.Equals("Payment"))
                        {
                            where += " and SuRole.AllowMultipleApprovePayment = 1 ";
                        }
                    }
                    //where += ") or c.UserID = :UserID) and e.Name <> 'Cancel' ";
                    where += " and a.CacheCurrentStateName <> 'Cancel' and (SuUserRole.RoleID is not null or c.UserID = :UserID) ";
                    parameterBuilder.AddParameterData("UserID", typeof(long), criteria.UserID);
                }
                #endregion Where
            }
            else
            {
                from = @" from
                    Document a with (nolock)
					left join WorkFlowStateLang l with (nolock) 
						on l.WorkFlowStateID = a.CacheCurrentState and l.LanguageID = :languageID ";
                where = @"  where a.Active=1 ";
            }
            #endregion Inbox
            #endregion From


            #region Where

            if (criteria.FlagQuery != null && criteria.FlagQuery.Equals("Draft"))
            {
                where += " and a.IsContainDocumentNo = 0 ";
                where += " and a.CreatorID = :UserID ";

                parameterBuilder.AddParameterData("UserID", typeof(long), criteria.UserID);
            }
            else if (criteria.SearchType == null || (criteria.SearchType != null && !criteria.SearchType.Equals("1")))
            {
                where += " and a.IsContainDocumentNo = 1 ";
            }

            parameterBuilder.AddParameterData("languageID", typeof(short), criteria.LanguageID);

            if (criteria.DocumentTypeID > 0)
            {
                where += "and a.DocumentTypeID = :DocumentTypeID ";
                parameterBuilder.AddParameterData("DocumentTypeID", typeof(Int32), criteria.DocumentTypeID);
            }
            if (!string.IsNullOrEmpty(criteria.Status))
            {
                where += "and l.DisplayName = :DocumentStatus ";
                parameterBuilder.AddParameterData("DocumentStatus", typeof(string), criteria.Status);
            }
            if (!string.IsNullOrEmpty(criteria.Role) && criteria.UserID > 0)
            {
                if (criteria.Role.Equals("Creator"))
                {
                    where += " and a.CreatorID = :UserID ";
                }
                if (criteria.Role.Equals("Requester"))
                {
                    where += " and a.RequesterID = :UserID ";
                }
                if (criteria.Role.Equals("Approver"))
                {
                    where += " and a.ApproverID = :UserID ";
                }

                parameterBuilder.AddParameterData("UserID", typeof(long), criteria.UserID);
            }
          
            if (criteria.CompanyID > 0)
            {
                where += " and a.CompanyID = :CompanyID ";
                parameterBuilder.AddParameterData("CompanyID", typeof(long), criteria.CompanyID);
            }
            if (!string.IsNullOrEmpty(criteria.RequestNo))
            {
                where += " and a.DocumentNo = :RequestNo ";
                parameterBuilder.AddParameterData("RequestNo", typeof(string), criteria.RequestNo);
            }
            if (!string.IsNullOrEmpty(criteria.RequestDateFrom))
            {
                where += " AND a.DocumentDate >= CONVERT(DATETIME, :RequestDateFrom, 103) ";
                parameterBuilder.AddParameterData("RequestDateFrom", typeof(string), criteria.RequestDateFrom);
            }
            if (!string.IsNullOrEmpty(criteria.RequestDateTo))
            {
                where += " AND a.DocumentDate < CONVERT(DATETIME, :RequestDateTo, 103) +1 ";
                parameterBuilder.AddParameterData("RequestDateTo", typeof(string), criteria.RequestDateTo);
            }
            if (criteria.CreatorID > 0)
            {
                where += " and a.CreatorID = :CreatorID ";
                parameterBuilder.AddParameterData("CreatorID", typeof(long), criteria.CreatorID);
            }
            if (criteria.RequesterID > 0)
            {
                where += " and a.RequesterID = :RequesterID ";
                parameterBuilder.AddParameterData("RequesterID", typeof(long), criteria.RequesterID);
            }
            if (criteria.Receiver > 0)
            {
                where += " and a.ReceiverID = :ReceiverID ";
                parameterBuilder.AddParameterData("ReceiverID", typeof(long), criteria.Receiver);
            }
            if (!string.IsNullOrEmpty(criteria.Subject))
            {
                where += " and a.Subject Like :Subject ";
                parameterBuilder.AddParameterData("Subject", typeof(string), "%" + criteria.Subject + "%");
            }
            if (criteria.AmountFrom > 0)
            {
                where += " and (a.CacheAmountTHB >= :AmountFrom) ";
                parameterBuilder.AddParameterData("AmountFrom", typeof(decimal), criteria.AmountFrom);
            }
            if (criteria.AmountTo > 0)
            {
                where += " and (a.CacheAmountTHB <= :AmountTo ) ";
                parameterBuilder.AddParameterData("AmountTo", typeof(decimal), criteria.AmountTo);
            }
            if (!string.IsNullOrEmpty(criteria.ReferneceNo))
            {
                where += " and a.ReferenceNo = :ReferenceNo ";
                parameterBuilder.AddParameterData("ReferenceNo", typeof(string), criteria.ReferneceNo);
            }
            if (criteria.ServiceTeamID > 0)
            {
                where += " and (a.CacheServiceTeamID = :ServiceTeamID) ";
                parameterBuilder.AddParameterData("ServiceTeamID", typeof(long), criteria.ServiceTeamID);
            }
            if (criteria.PBID > 0)
            {
                where += " and (a.CachePBID = :PBID) ";
                parameterBuilder.AddParameterData("PBID", typeof(long), criteria.PBID);
            }
            if (criteria.FlagJoin.Equals("Search") && (criteria.FlagSearch.Equals("Payment") || criteria.FlagSearch.Equals("Accountant")))
            {
                where += " and a.DocumentTypeID not in (2,8) ";
            }
            if (!string.IsNullOrEmpty(criteria.DocumentStatus))
            {
                where += "  and a.CacheCurrentStateName = :Name";
                parameterBuilder.AddParameterData("Name", typeof(string), criteria.DocumentStatus);
            }

            #endregion Where
            #endregion Search

        

            #region History
            if (criteria.FlagJoin.Equals("History"))
            {
                #region From
                from += @"  left join ( select o.WorkFlowID,d.Taskgroup,d.DocumentTypeID From WorkFlowResponse o with (nolock)
                        inner join WorkFlowEventClassification d with (nolock)
                        on d.WorkFlowStateEventID = o.WorkFlowStateEventID  ";
                if (criteria.FlagSearch.Equals("Employee"))
                {
                    from += " and d.TaskGroup = 1 ";
                }
                if (criteria.FlagSearch.Equals("Accountant"))
                {
                    from += " and d.TaskGroup = 2 ";
                }
                if (criteria.FlagSearch.Equals("Payment"))
                {
                    from += " and d.TaskGroup = 3 ";
                }
                from += @" and o.ResponseBy = :UserID ) dd on a.CacheWorkflowID = dd.WorkFlowID  and dd.DocumentTypeID = a.DocumentTypeID 
                            left join ( select tt.UserID,ta.DocumentID From TADocument ta with (nolock)
                           inner join TADocumentTraveller tt with (nolock) on ta.TADocumentID = tt.TADocumentID and tt.UserID = :UserID) ttt on a.DocumentID = ttt.DocumentID ";
               
                #endregion From
                #region Where
                if ((criteria.Role.Equals(string.Empty) && criteria.UserID > 0) || criteria.UserID > 0)
                {
                    where += @" and a.IsContainDocumentNo = 1
                            and (a.CreatorID = :UserID
                            or a.RequesterID = :UserID
                            or a.ReceiverID = :UserID
                            or ttt.UserID is not null
                            or dd.TaskGroup is not null )";
                    parameterBuilder.AddParameterData("UserID", typeof(long), criteria.UserID);
                }
                #endregion Where
            }
            #endregion History
            if (criteria.Role.Equals("Initiator"))
            {
                from += @"
                    inner join DocumentInitiator m with (nolock)
                        on a.DocumentID = m.DocumentID
                        and m.UserID = :UserID ";
            }
            if (criteria.FlagJoin.Equals("Search"))
            {
                if (criteria.FlagSearch.Equals("Accountant"))
                {
                    from += @"  inner join SuRoleService srs with (nolock)
                                on srs.ServiceTeamID = a.CacheServiceTeamID
                                inner join SuUserRole sur with (nolock)
                                on sur.RoleID = srs.RoleID
                                and sur.UserID = :UserID
                                inner join SuRole sr with (nolock)
                                on sr.RoleID = sur.RoleID
                                and (sr.VerifyDocument = 1 or sr.ReceiveDocument = 1 or sr.ApproveVerifyDocument = 1) ";

                    parameterBuilder.AddParameterData("UserID", typeof(long), criteria.UserID);
                }
                if (criteria.FlagSearch.Equals("Payment"))
                {
                    from += @"  inner join SuRolePB srp with (nolock)
                                on srp.PBID = a.CachePBID
                                inner join SuUserRole sur with (nolock) 
                                on sur.RoleID = srp.RoleID
                                and sur.UserID = :UserID
                                inner join SuRole sr with (nolock)
                                on sr.RoleID = sur.RoleID
                                and (sr.ApproveVerifyPayment = 1 or sr.VerifyPayment = 1 or sr.CounterCashier = 1) ";

                    parameterBuilder.AddParameterData("UserID", typeof(long), criteria.UserID);
                }
            }

            if (isCount)
            {
                sqlQuery = "select count(*) as inboxCount from ( ";
                sqlQuery += select + from + where + ")temp";
            }
            else
            {
                sqlQuery = @" select DocumentID, WorkflowID, RequestNo, ReferenceNo, DocumentTypeID,
                                     RequestDate, Subject, CreatorID, CreatorName, RequesterID, RequesterName,
                                     ApproverID, WorkFlowStateID, WorkFlowTypeID, Amount, DocumentStatus , CreateDate, DocumentTypeName, StateName  ,AmountMainCurrency   ,AmountLocalCurrency , ApproveDate  ,ReceiveDocumentDate                 
                              from ( ";
                sqlQuery += select + from + where + ")temp";
            }

            if (!isCount)
            {
                if (string.IsNullOrEmpty(sortExpression))
                {
                    if (criteria.FlagJoin.Equals("Search"))
                    {
                        sqlQuery += " ORDER BY RequestDate Desc,RequestNo,ReferenceNo,DocumentTypeID,Subject,CreatorID,CreatorName,RequesterID,RequesterName,Amount ";
                    }
                    else if (criteria.FlagJoin.Equals("Inbox") && criteria.FlagSearch.Equals("Accountant"))
                    {
                        sqlQuery += " ORDER BY ApproveDate ASC,RequestNo,ReferenceNo,DocumentTypeID,Subject,CreatorID,CreatorName,RequesterID,RequesterName,WorkFlowStateID,WorkFlowTypeID,Amount ";
                    }
                    else
                    {
                        sqlQuery += " ORDER BY CreateDate DESC,RequestDate Desc,RequestNo,ReferenceNo,DocumentTypeID,Subject,CreatorID,CreatorName,RequesterID,RequesterName,WorkFlowStateID,WorkFlowTypeID,Amount ";
                    }
                }
                else
                {
                    sqlQuery += string.Format(" ORDER BY {0}", sortExpression);
                }
            }

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlQuery);
            parameterBuilder.FillParameters(query);

            if (!isCount)
            {
                query.AddScalar("DocumentID", NHibernateUtil.Int64);
                query.AddScalar("WorkflowID", NHibernateUtil.Int64);
                query.AddScalar("RequestNo", NHibernateUtil.String);
                query.AddScalar("ReferenceNo", NHibernateUtil.String);
                query.AddScalar("WorkFlowStateID", NHibernateUtil.Int32);
                query.AddScalar("WorkFlowTypeID", NHibernateUtil.Int32);
                query.AddScalar("DocumentTypeID", NHibernateUtil.Int64);
                query.AddScalar("RequestDate", NHibernateUtil.DateTime);
                query.AddScalar("Subject", NHibernateUtil.String);
                query.AddScalar("CreatorID", NHibernateUtil.Int64);
                query.AddScalar("CreatorName", NHibernateUtil.String);
                query.AddScalar("RequesterID", NHibernateUtil.Int64);
                query.AddScalar("RequesterName", NHibernateUtil.String);
                query.AddScalar("ApproverID", NHibernateUtil.Int64);
                query.AddScalar("Amount", NHibernateUtil.Double);
                query.AddScalar("DocumentStatus", NHibernateUtil.String);
                query.AddScalar("CreateDate", NHibernateUtil.DateTime);
                query.AddScalar("DocumentTypeName", NHibernateUtil.String);
                query.AddScalar("StateName", NHibernateUtil.String);
                query.AddScalar("AmountMainCurrency", NHibernateUtil.Double);
                query.AddScalar("AmountLocalCurrency", NHibernateUtil.Double);
                query.AddScalar("ApproveDate", NHibernateUtil.DateTime);
                query.AddScalar("ReceiveDocumentDate", NHibernateUtil.DateTime);

                query.SetResultTransformer(Transformers.AliasToBean(typeof(SearchResultData)));
            }
            else
            {
                query.AddScalar("inboxCount", NHibernateUtil.Int32);
            }
            return query;
        }
        #endregion public ISQLQuery FindInboxAccountantPaymentCriteria(SearchCriteria criteria, bool isCount, string sortExpression)

        #region public IList<SearchResultData> GetAccountantPaymentCriteriaList(SearchCriteria criteria, int firstResult, int maxResult, string sortExpression)
        public IList<SearchResultData> GetAccountantPaymentCriteriaList(SearchCriteria criteria, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<SearchResultData>(ScgeAccountingQueryProvider.SCGDocumentQuery, "FindInboxAccountantPaymentCriteria", new object[] { criteria, false, sortExpression }, firstResult, maxResult, sortExpression);
        }
        #endregion public IList<SearchResultData> GetAccountantPaymentCriteriaList(SearchCriteria criteria, int firstResult, int maxResult, string sortExpression)

        #region public int CountAccountantPaymentCriteria(SearchCriteria criteria)
        public int CountAccountantPaymentCriteria(SearchCriteria criteria)
        {
            return NHibernateQueryHelper.CountByCriteria(ScgeAccountingQueryProvider.SCGDocumentQuery, "FindInboxAccountantPaymentCriteria", new object[] { criteria, true, string.Empty });
        }
        #endregion public int CountAccountantPaymentCriteria(SearchCriteria criteria)

        #region public IList<SearchResultData> GetDocumentAttachmentByDocumentID(long documentID)
        public IList<SearchResultData> GetDocumentAttachmentByDocumentID(long documentID)
        {
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.AppendLine(" select ");
            sqlBuilder.AppendLine("     DocumentID ");
            sqlBuilder.AppendLine(" from ");
            sqlBuilder.AppendLine("     DocumentAttachment ");
            sqlBuilder.AppendLine(" where ");
            sqlBuilder.AppendLine("     DocumentID = :DocumentID ");
            sqlBuilder.AppendLine("     and Active = 1 ");

            parameterBuilder.AddParameterData("DocumentID", typeof(long), documentID);

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            parameterBuilder.FillParameters(query);

            query.AddScalar("DocumentID", NHibernateUtil.Int64);
            query.SetResultTransformer(Transformers.AliasToBean(typeof(SearchResultData)));

            return query.List<SearchResultData>();
        }
        #endregion public IList<SearchResultData> GetDocumentAttachmentByDocumentID(long documentID)

        #endregion ISCGDocumentQuery Inbox

        #region query for DocumentFollowUpReport
        #region public ISQLQuery FindByDocumentFollowReportCriteria(short languageId,long companyID,long fromLocationId,long toLocationId,string fromDate,string toDate,long creatorId,long requestId,long costCenterId,long serviceTeamId,int valueStatus,bool isCount, string sortExpression)
        public ISQLQuery FindByDocumentFollowReportCriteria(bool isPosting, short languageId, long companyID, string fromLocationCode, string toLocationCode, string fromDate, string toDate, long creatorId, long requestId, long costCenterId, long serviceTeamId, int valueStatus, bool isCount, string sortExpression)
        {
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            //DateTime dtDate = DateTime.Now;
            //string strDate = "";
            StringBuilder sqlBuilder = new StringBuilder();
            if (!isCount)
            {
                sqlBuilder.AppendLine("select DocumentID,DocumentDate,Status,RequestNo,CreatorName,RequesterName,TelNo,Email,Description,RequesterID,CreatorID,Amount,SendTime,CountDays ");
            }
            else
            {
                sqlBuilder.Append(" SELECT COUNT(*) as ReportCount ");
            }
            #region Query
            sqlBuilder.AppendLine(" from (select distinct a.documentID as DocumentID,a.documentDate as DocumentDate,e.displayname as Status ");
            sqlBuilder.AppendLine(" ,a.DocumentNo as RequestNo,f.UserName as CreatorName,c.UserName as RequesterName ");
            sqlBuilder.AppendLine(" ,c.phoneno as TelNo, c.Email as Email,a.subject as Description ,a.requesterid as RequesterID,a.CreatorID as CreatorID");
            sqlBuilder.AppendLine(" ,b.totalExpense as Amount,(case when tableA.countSendDate is null then 0 else tableA.countSendDate end) as SendTime ");
            sqlBuilder.AppendLine(",ISNULL(DATEDIFF(day,(case ");
            sqlBuilder.AppendLine(" when (select count(1)  from DocumentAttachment where DocumentID = a.DocumentID) = 0 then (select max(wfr.ResponseDate) from workflowresponse wfr  ");
            sqlBuilder.AppendLine(" inner join WorkflowStateEvent wfse on wfse.WorkflowStateEventID = wfr.WorkflowStateEventID and wfse.Name = 'Approve' ");
            sqlBuilder.AppendLine(" inner join WorkflowState ws on ws.WorkflowStateID = wfse.WorkflowStateID and ws.WorkflowStateID = 29 ");
            sqlBuilder.AppendLine(" where wfr.workflowid = d.workflowid and wfr.active = 1)");
            sqlBuilder.AppendLine(" else ");
            sqlBuilder.AppendLine(" (select max(wfr.ResponseDate) from workflowresponse wfr  ");
            sqlBuilder.AppendLine(" inner join WorkflowStateEvent wfse on wfse.WorkflowStateEventID = wfr.WorkflowStateEventID and wfse.Name = 'Approve'  ");
            sqlBuilder.AppendLine(" inner join WorkflowState ws on ws.WorkflowStateID = wfse.WorkflowStateID and ws.WorkflowStateID = 31  ");
            sqlBuilder.AppendLine(" where wfr.workflowid = d.workflowid and wfr.active = 1)");
            sqlBuilder.AppendLine(" end),GETDATE()),0) as CountDays");
            sqlBuilder.AppendLine(" from document a   ");
            sqlBuilder.AppendLine(" inner join fnexpensedocument b on a.documentid = b.documentid ");
            sqlBuilder.AppendLine(" left outer join suuser c on a.requesterid =  c.userid ");
            sqlBuilder.AppendLine(" left outer join workFlow d on d.documentid = a.documentid ");
            sqlBuilder.AppendLine(" left outer join workflowstatelang e on e.workflowstateid = d.currentstate and e.languageid = :languageId ");
            sqlBuilder.AppendLine(" left outer join suuser f on  f.userid = a.creatorid ");
            sqlBuilder.AppendLine(" left outer join dblocation g on g.locationid = c.locationid ");
            sqlBuilder.AppendLine(" left outer join FnExpenseInvoice h on h.ExpenseID = b.ExpenseID ");
            sqlBuilder.AppendLine(" left outer join FnExpenseInvoiceItem i on i.InvoiceID = h.InvoiceID ");
            sqlBuilder.AppendLine(" left outer join workflowstate j on j.workflowtypeid = d.workflowtypeid and j.workflowstateid = d.currentstate ");
            sqlBuilder.AppendLine(" left outer join  ");
            sqlBuilder.AppendLine(" (select count(senddate) as countSendDate ,b.requestNo ");
            sqlBuilder.AppendLine(" from document a ");
            sqlBuilder.AppendLine(" left outer join suemaillog b on a.documentNo = b.requestNo and b.emailtype = 'EM09' ");
            sqlBuilder.AppendLine(" group by b.requestNo) tableA on tableA.requestNo = a.DocumentNO ");
            sqlBuilder.AppendLine(" where a.documenttypeid in (3,7) and a.active =1 ");
            sqlBuilder.AppendLine(" and j.name not in ('WaitApprove','Cancel') and j.ordinal >= 4 ");
            if (companyID != 0)
            {
                sqlBuilder.AppendLine(" and a.companyid = :companyID ");
                parameterBuilder.AddParameterData("companyID", typeof(long), companyID);
            }
            if (!string.IsNullOrEmpty(fromLocationCode))
            {
                sqlBuilder.AppendLine(" and g.locationCode <= :fromLocationCode");
                parameterBuilder.AddParameterData("fromLocationCode", typeof(string), fromLocationCode);
            }
            if (!string.IsNullOrEmpty(toLocationCode))
            {
                sqlBuilder.AppendLine(" and g.locationCode >= :toLocationCode");
                parameterBuilder.AddParameterData("toLocationCode", typeof(string), toLocationCode);
            }
            if (!fromDate.Equals(string.Empty))
            {
                sqlBuilder.AppendLine(" and a.documentdate >= convert(datetime,:fromDate,103) ");
                parameterBuilder.AddParameterData("fromDate", typeof(string), fromDate);
            }
            if (!toDate.Equals(string.Empty))
            {
                sqlBuilder.AppendLine(" and a.documentdate < convert(datetime,:toDate,103) + 1 ");
                parameterBuilder.AddParameterData("toDate", typeof(string), toDate);
            }
            if (requestId != 0)
            {
                sqlBuilder.AppendLine(" and c.userid = :requestId ");
                parameterBuilder.AddParameterData("requestId", typeof(long), requestId);
            }
            if (creatorId != 0)
            {
                sqlBuilder.AppendLine(" and f.userid = :creatorId ");
                parameterBuilder.AddParameterData("creatorId", typeof(long), creatorId);
            }
            if (costCenterId != 0)
            {
                sqlBuilder.AppendLine(" and i.costcenterid = :costCenterId");
                parameterBuilder.AddParameterData("costCenterId", typeof(long), costCenterId);
            }
            if (serviceTeamId != 0)
            {
                sqlBuilder.AppendLine(" and b.ServiceTeamID = :serviceTeamId ");
                parameterBuilder.AddParameterData("serviceTeamId", typeof(long), serviceTeamId);
            }
            if (isPosting)
            {
                sqlBuilder.AppendLine(" and a.PostingStatus = 'C' ");
            }
            if (valueStatus == 1)
                sqlBuilder.AppendLine(" and ( b.BoxID is null or b.BoxID = '' )");
            else if (valueStatus == 2)
                sqlBuilder.AppendLine(" and b.BoxID is not null and b.BoxID <> '' ");
            sqlBuilder.AppendLine(" ) tb where CountDays > 0 ");
            #endregion

            if (!isCount)
            {
                if (string.IsNullOrEmpty(sortExpression))
                    sqlBuilder.AppendLine(" ORDER BY CountDays DESC ");
                else
                    sqlBuilder.AppendLine(string.Format(" ORDER BY {0}", sortExpression));
            }
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());

            parameterBuilder.AddParameterData("languageId", typeof(short), languageId);
            parameterBuilder.FillParameters(query);

            if (!isCount)
            {
                query.AddScalar("DocumentID", NHibernateUtil.Int64);
                query.AddScalar("DocumentDate", NHibernateUtil.DateTime);
                query.AddScalar("Status", NHibernateUtil.String);
                query.AddScalar("RequestNo", NHibernateUtil.String);
                query.AddScalar("CreatorName", NHibernateUtil.String);
                query.AddScalar("RequesterName", NHibernateUtil.String);
                query.AddScalar("TelNo", NHibernateUtil.String);
                query.AddScalar("Email", NHibernateUtil.String);
                query.AddScalar("Description", NHibernateUtil.String);
                query.AddScalar("Amount", NHibernateUtil.Double);
                //query.AddScalar("ResponseDate", NHibernateUtil.DateTime);
                query.AddScalar("SendTime", NHibernateUtil.Int32);
                query.AddScalar("RequesterID", NHibernateUtil.Int64);
                query.AddScalar("CreatorID", NHibernateUtil.Int64);
                query.AddScalar("CountDays", NHibernateUtil.Int64);

                query.SetResultTransformer(Transformers.AliasToBean(typeof(VODocumentFollowUpReport)));
            }
            else
            {
                query.AddScalar("ReportCount", NHibernateUtil.Int32);
                query.UniqueResult();
            }
            return query;
        }
        #endregion public ISQLQuery FindByDocumentCriteria(bool isCount, string sortExpression, string documentNo, short documentTypeID, string description, long companyID, long requesterID, string travelBy)

        #region public IList<VODocumentFollowUpReport> GetDocumentReportList(int firstResult, int maxResult, string sortExpression, short languageId, long companyID, long fromLocationId, long toLocationId, string fromDate, string toDate, long creatorId, long requestId, long costCenterId, long serviceTeamId, int valueStatus)
        public IList<VODocumentFollowUpReport> GetDocumentReportList(int firstResult, int maxResult, string sortExpression, bool isPosting, short languageId, long companyID, string fromLocationCode, string toLocationCode, string fromDate, string toDate, long creatorId, long requestId, long costCenterId, long serviceTeamId, int valueStatus)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<VODocumentFollowUpReport>(ScgeAccountingQueryProvider.SCGDocumentQuery, "FindByDocumentFollowReportCriteria", new object[] { isPosting, languageId, companyID, fromLocationCode, toLocationCode, fromDate, toDate, creatorId, requestId, costCenterId, serviceTeamId, valueStatus, false, sortExpression }, firstResult, maxResult, sortExpression);
        }
        #endregion public IList<VODocumentFollowUpReport> GetDocumentReportList(int firstResult, int maxResult, string sortExpression, short languageId, long companyID, long fromLocationId, long toLocationId, string fromDate, string toDate, long creatorId, long requestId, long costCenterId, long serviceTeamId, int valueStatus)

        #region public int CountByDocumentReportCriteria(short languageId,long companyID,long fromLocationId,long toLocationId,string fromDate,string toDate,long creatorId,long requestId,long costCenterId,long serviceTeamId,int valueStatus)
        public int CountByDocumentReportCriteria(bool isPosting, short languageId, long companyID, string fromLocationCode, string toLocationCode, string fromDate, string toDate, long creatorId, long requestId, long costCenterId, long serviceTeamId, int valueStatus)
        {
            return NHibernateQueryHelper.CountByCriteria(ScgeAccountingQueryProvider.SCGDocumentQuery, "FindByDocumentFollowReportCriteria", new object[] { isPosting, languageId, companyID, fromLocationCode, toLocationCode, fromDate, toDate, creatorId, requestId, costCenterId, serviceTeamId, valueStatus, true, string.Empty });
        }
        #endregion public int CountByDocumentReportCriteria(short languageId,long companyID,long fromLocationId,long toLocationId,string fromDate,string toDate,long creatorId,long requestId,long costCenterId,long serviceTeamId,int valueStatus)

        #endregion

        #region query for Advance Over Due Report
        #region public FindByAdvanceOverDueReportCriteria(VOAdvanceOverDueReport vo, bool isCount, string sortExpression)
        public ISQLQuery FindByAdvanceOverDueReportCriteria(VOAdvanceOverDueReport vo, bool isCount, string sortExpression)
        {
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            StringBuilder sqlBuilder = new StringBuilder();
            if (!isCount)
            {
                #region Query
                sqlBuilder.AppendLine(" select DocumentID, RequesterID, AdvanceID,DocumentNo, DocumentDate, RequesterName ");
                sqlBuilder.AppendLine(" ,Description, AdvanceAmt, ExpenseAmt, RemittanceAmt, OutstandingAmt ");
                sqlBuilder.AppendLine(" ,ExpenseNo, ExpenseID, ExpenseStatus, DueDate, RequestDateofRemittance, OverdueDays, Sendtime, ExpenseBoxID ");
                #endregion
            }
            else
            {
                #region Count query
                sqlBuilder.AppendLine(" select count(*) as ReportCount ");
                #endregion
            }
            sqlBuilder.AppendLine(" from (select distinct a.documentID as DocumentID,a.requesterid as RequesterID,b.AdvanceID as AdvanceID ,a.documentNo as DocumentNo ");
            sqlBuilder.AppendLine(" ,b.RequestDateOfAdvance as DocumentDate ,c.EmployeeName as RequesterName ,a.subject as Description ,b.Amount as AdvanceAmt ");
            sqlBuilder.AppendLine(" ,isnull(fnexp.TotalExpense,0.00) as ExpenseAmt ,isnull(fnrem.totalamount,0.00) as RemittanceAmt ,ISNULL(fnexp.DifferenceAmount,0.00) as OutstandingAmt ");
            sqlBuilder.AppendLine(" ,expense1.documentNo as ExpenseNo ,expense1.documentID as ExpenseID , expense1.displayname as ExpenseStatus ");
            sqlBuilder.AppendLine(" ,(case when b.RequestDateOfRemittance >= b.DueDateOfRemittance then b.RequestDateOfRemittance else b.DueDateOfRemittance end) as DueDate ");
            sqlBuilder.AppendLine(" ,b.requestDateofRemittance as RequestDateofRemittance ");
            sqlBuilder.AppendLine(" ,ISNULL(DATEDIFF(day,(case when b.RequestDateOfRemittance >= b.DueDateOfRemittance then b.RequestDateOfRemittance else b.DueDateOfRemittance end),GETDATE()),0) as OverdueDays ");
            sqlBuilder.AppendLine(" , (case when tableA.countSendDate is null then 0 else tableA.countSendDate end) as Sendtime ");
            sqlBuilder.AppendLine(" , a.companyid as CompanyID, d.locationcode as LocationCode, b.AdvanceType as AdvanceType, expense1.BoxID as ExpenseBoxID ");
            sqlBuilder.AppendLine(" from document a ");
            sqlBuilder.AppendLine(" left outer join avadvancedocument b  on a.documentid = b.documentid ");
            //sqlBuilder.AppendLine(" left outer join fnexpenseadvance fnexpadv on b.advanceid = fnexpadv.advanceid ");
            sqlBuilder.AppendLine(" left outer join fnremittanceadvance fnremadv on b.advanceid = fnremadv.advanceid ");
            sqlBuilder.AppendLine(" left outer join fnremittance fnrem on fnremadv.remittanceid = fnrem.remittanceid ");
            sqlBuilder.AppendLine(" left outer join suuser c  on c.userid = a.requesterid ");
            sqlBuilder.AppendLine(" left outer join dblocation d  on d.locationid = c.locationid ");
            sqlBuilder.AppendLine(" left outer join workflow wf  on wf.documentid = a.documentid ");
            sqlBuilder.AppendLine(" left outer join workflowstate ws  on ws.WorkFlowStateID = wf.CurrentState ");
            sqlBuilder.AppendLine(" left outer join (select count(senddate) as countSendDate ,b.requestNo from document a ");
            sqlBuilder.AppendLine(" left outer join suemaillog b on a.documentNo = b.requestNo and b.emailtype = 'EM10' ");
            sqlBuilder.AppendLine(" where b.status = 1 group by b.requestNo) tableA on tableA.requestNo = a.DocumentNO ");
            sqlBuilder.AppendLine(" left join  ( select x2.expenseid, x1.AdvanceID,x1.CreDate as DateExpenseReferAdvance, x3.DocumentNo, x3.DocumentID, x6.DisplayName, x2.BoxID  from fnexpenseadvance x1  ");
            sqlBuilder.AppendLine(" left join fnexpensedocument x2 on x1.expenseid = x2.expenseid  ");
            sqlBuilder.AppendLine(" left join [document] x3 on x2.documentid = x3.documentid  ");
            sqlBuilder.AppendLine(" left join workflow x4 on x3.documentid = x4.documentid  ");
            sqlBuilder.AppendLine(" inner join workflowstate x5 on x4.CurrentState = x5.WorkflowStateID and x5.Name <> 'Cancel'");
            sqlBuilder.AppendLine(" left outer join workflowstatelang x6 on x6.WorkFlowStateID = x5.WorkFlowStateID and x6.languageid = :languageId ");
            sqlBuilder.AppendLine(" ) expense1 on  expense1.AdvanceID = b.advanceID and expense1.DateExpenseReferAdvance = (select max(CreDate) as CreateDate from fnexpenseadvance where AdvanceID = expense1.AdvanceID)");
            sqlBuilder.AppendLine(" left outer join  fnexpensedocument fnexp on expense1.ExpenseID = fnexp.expenseid ");
            sqlBuilder.AppendLine(" where a.active =1 and b.active = 1 ");
            sqlBuilder.AppendLine(" and ws.name = 'Outstanding')t1 where OverdueDays > 0 ");


            if (vo.CompanyID != 0)
            {
                sqlBuilder.AppendLine(" and CompanyID = :companyID ");
                parameterBuilder.AddParameterData("companyID", typeof(long), vo.CompanyID);
            }
            if (!string.IsNullOrEmpty(vo.FromLocationCode))
            {
                sqlBuilder.AppendLine(" and LocationCode >= :locationForm");
                parameterBuilder.AddParameterData("locationForm", typeof(string), vo.FromLocationCode);
            }
            if (!string.IsNullOrEmpty(vo.ToLocationCode))
            {
                sqlBuilder.AppendLine(" and LocationCode <= :locationTo");
                parameterBuilder.AddParameterData("locationTo", typeof(string), vo.ToLocationCode);
            }
            if (!string.IsNullOrEmpty(vo.FromDueDate))
            {
                sqlBuilder.AppendLine(" and convert(datetime,DueDate,103) >= convert(datetime,:dueDateForm,103) ");
                parameterBuilder.AddParameterData("dueDateForm", typeof(string), vo.FromDueDate);
            }
            if (!string.IsNullOrEmpty(vo.ToDueDate))
            {
                sqlBuilder.AppendLine(" and convert(datetime,DueDate,103) <= convert(datetime,:dueDateTo,103) ");
                parameterBuilder.AddParameterData("dueDateTo", typeof(string), vo.ToDueDate);
            }
            if (vo.FromAdvanceAmount != 0)
            {
                sqlBuilder.AppendLine(" and AdvanceAmt >= :fromAmount ");
                parameterBuilder.AddParameterData("fromAmount", typeof(double), vo.FromAdvanceAmount);
            }
            if (vo.ToAdvanceAmount != 0)
            {
                sqlBuilder.AppendLine(" and AdvanceAmt <= :toAmount ");
                parameterBuilder.AddParameterData("toAmount", typeof(double), vo.ToAdvanceAmount);
            }
            if (!string.IsNullOrEmpty(vo.AdvanceType))
            {
                if (!vo.AdvanceType.Equals("ALL"))
                {
                    sqlBuilder.AppendLine(" and AdvanceType = :advanceType");
                    parameterBuilder.AddParameterData("advanceType", typeof(string), vo.AdvanceType);
                }
            }
            if (vo.RequesterID != 0)
            {
                sqlBuilder.AppendLine(" and RequesterID = :requesterID ");
                parameterBuilder.AddParameterData("requesterID", typeof(long), vo.RequesterID);
            }
            if (vo.FromOverDue != 0)
            {
                sqlBuilder.AppendLine(" and OverdueDays >= :fromOverDue ");
                parameterBuilder.AddParameterData("fromOverDue", typeof(int), vo.FromOverDue);
            }
            if (vo.ToOverDue != 0)
            {
                sqlBuilder.AppendLine(" and OverdueDays <= :toOverDue ");
                parameterBuilder.AddParameterData("toOverDue", typeof(int), vo.ToOverDue);
            }

            if (vo.ToOverDue != 0)
            {
                sqlBuilder.AppendLine(" and OverdueDays <= :toOverDue ");
                parameterBuilder.AddParameterData("toOverDue", typeof(int), vo.ToOverDue);
            }

            if (!isCount)
            {
                if (!string.IsNullOrEmpty(sortExpression))
                    sqlBuilder.AppendLine(string.Format(" ORDER BY {0}", sortExpression));
                else
                    sqlBuilder.AppendLine(" ORDER BY OverdueDays DESC , DocumentNo ASC ");
            }

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());

            parameterBuilder.AddParameterData("languageId", typeof(short), vo.LanguageID);
            parameterBuilder.FillParameters(query);

            if (!isCount)
            {
                query.AddScalar("DocumentID", NHibernateUtil.Int64);
                query.AddScalar("DocumentNo", NHibernateUtil.String);
                query.AddScalar("DocumentDate", NHibernateUtil.DateTime);
                query.AddScalar("RequesterName", NHibernateUtil.String);
                query.AddScalar("Description", NHibernateUtil.String);
                query.AddScalar("AdvanceAmt", NHibernateUtil.Double);
                query.AddScalar("ExpenseAmt", NHibernateUtil.Double);
                query.AddScalar("OutstandingAmt", NHibernateUtil.Double);
                query.AddScalar("ExpenseNo", NHibernateUtil.String);
                query.AddScalar("ExpenseStatus", NHibernateUtil.String);
                query.AddScalar("DueDate", NHibernateUtil.DateTime);
                query.AddScalar("RequestDateofRemittance", NHibernateUtil.DateTime);
                query.AddScalar("Sendtime", NHibernateUtil.Int32);
                query.AddScalar("RequesterID", NHibernateUtil.Int64);
                query.AddScalar("AdvanceID", NHibernateUtil.Int64);
                query.AddScalar("ExpenseID", NHibernateUtil.Int64);
                query.AddScalar("RemittanceAmt", NHibernateUtil.Double);
                query.AddScalar("OverdueDays", NHibernateUtil.Int64);
                query.SetResultTransformer(Transformers.AliasToBean(typeof(VOAdvanceOverDueReport)));
            }
            else
            {
                query.AddScalar("ReportCount", NHibernateUtil.Int32);
                //query.UniqueResult();
            }
            return query;
        }
        #endregion public FindByAdvanceOverDueReportCriteria(VOAdvanceOverDueReport vo, bool isCount, string sortExpression)

        #region public IList<VOAdvanceOverDueReport> GetAdvanceDocumentReportList(int firstResult, int maxResult, string sortExpression, long companyID, long locationForm, long locationTo, string dueDateForm, string dueDateTo, double amountFrom, double amountTo, int overDayFrom, int overDayTo, string advanceType, long requesterID)
        public IList<VOAdvanceOverDueReport> GetAdvanceDocumentReportList(int firstResult, int maxResult, string sortExpression, VOAdvanceOverDueReport vo)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<VOAdvanceOverDueReport>(ScgeAccountingQueryProvider.SCGDocumentQuery, "FindByAdvanceOverDueReportCriteria", new object[] { vo, false, sortExpression }, firstResult, maxResult, sortExpression);
        }
        #endregion public IList<VOAdvanceOverDueReport> GetAdvanceDocumentReportList(int firstResult, int maxResult, string sortExpression, VOAdvanceOverDueReport vo)

        #region public int CountByDocumentReportCriteria(VOAdvanceOverDueReport vo)
        public int CountByAdvanceReportCriteria(VOAdvanceOverDueReport vo)
        {
            return NHibernateQueryHelper.CountByCriteria(ScgeAccountingQueryProvider.SCGDocumentQuery, "FindByAdvanceOverDueReportCriteria", new object[] { vo, true, string.Empty });
        }
        #endregion public int CountByAdvanceReportCriteria(VOAdvanceOverDueReport vo)

        #endregion

        #region public DocumentReportName GetReportNameByDocumentID(long DocumentID)
        public DocumentReportName GetReportNameByDocumentID(long DocumentID)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" SELECT ");
            sqlBuilder.Append(" (CASE docType.DocumentTypeID ");
            sqlBuilder.Append("	WHEN 1 THEN avd.AdvanceID ");
            sqlBuilder.Append("	WHEN 2 THEN ta.TADocumentID ");
            sqlBuilder.Append("	WHEN 3 THEN expD.ExpenseID ");
            sqlBuilder.Append("	WHEN 4 THEN rem.RemittanceID ");
            sqlBuilder.Append("	WHEN 5 THEN avf.AdvanceID ");
            sqlBuilder.Append("	WHEN 7 THEN expF.ExpenseID ");
            sqlBuilder.Append("	WHEN 8 THEN ta.TADocumentID ");
            sqlBuilder.Append("	WHEN 10 THEN mpa.MPADocumentID ");
            sqlBuilder.Append("	WHEN 11 THEN ca.CADocumentID ");
            sqlBuilder.Append("	WHEN 12 THEN fav.FixedAdvanceID ");
            sqlBuilder.Append(" END) AS ReportParam, ");
            sqlBuilder.Append(" (CASE docType.DocumentTypeID ");
            sqlBuilder.Append("	WHEN 1 THEN 'AVReportDomestic'");
            sqlBuilder.Append("	WHEN 2 THEN 'TAReport' ");
            sqlBuilder.Append("	WHEN 3 THEN (CASE ");
            sqlBuilder.Append("     (SELECT COUNT(1) as EXPCOUNT ");
            sqlBuilder.Append("     FROM FNExpenseDocument expD INNER JOIN FnExpenseAdvance expDAdv ");
            sqlBuilder.Append("     ON expD.ExpenseID = expDAdv.ExpenseID WHERE expD.DocumentID = doc.DocumentID) ");
            sqlBuilder.Append("     WHEN 0 THEN 'PettyCashReimbursementDM' ");
            sqlBuilder.Append("     ELSE 'AdvReimbursementDomestic'");
            sqlBuilder.Append("     END) ");
            sqlBuilder.Append("	WHEN 4 THEN 'RMTADVReport'");
            sqlBuilder.Append("	WHEN 5 THEN 'AVReportForeign' ");
            sqlBuilder.Append("	WHEN 7 THEN (CASE ");
            sqlBuilder.Append("     (SELECT COUNT(1) as EXPCOUNT ");
            sqlBuilder.Append("     FROM FNExpenseDocument expD INNER JOIN FnExpenseAdvance expDAdv ");
            sqlBuilder.Append("     ON expD.ExpenseID = expDAdv.ExpenseID WHERE expD.DocumentID = doc.DocumentID ");
            sqlBuilder.Append("     ) ");
            sqlBuilder.Append("     WHEN 0 THEN 'PettyCashReimbursementFR' ");
            sqlBuilder.Append("     ELSE 'AdvReimbursementForeign' ");
            sqlBuilder.Append("	    END) ");
            sqlBuilder.Append("	WHEN 8 THEN 'TAReport' ");
            sqlBuilder.Append("	WHEN 10 THEN 'MPADocumentPrintForm' ");
            sqlBuilder.Append("	WHEN 11 THEN 'CADocumentPrintForm' ");
            sqlBuilder.Append("	WHEN 12 THEN 'FixedAdvanceDocumentPrintForm' ");
            sqlBuilder.Append(" END) AS ReportName");
            sqlBuilder.Append(" FROM [Document] doc");
            sqlBuilder.Append(" LEFT JOIN DocumentType docType ON doc.DocumentTypeID = docType.DocumentTypeID ");
            sqlBuilder.Append(" LEFT JOIN AvAdvanceDocument avd ON avd.DocumentID = doc.DocumentID ");
            sqlBuilder.Append(" LEFT JOIN AvAdvanceDocument avf ON avf.DocumentID = doc.DocumentID ");
            sqlBuilder.Append(" LEFT JOIN TADocument ta ON ta.DocumentID = doc.DocumentID ");
            sqlBuilder.Append(" LEFT JOIN FnExpenseDocument expD ON expD.DocumentID = doc.DocumentID ");
            sqlBuilder.Append(" LEFT JOIN FnExpenseDocument expF ON expF.DocumentID = doc.DocumentID ");
            sqlBuilder.Append(" LEFT JOIN FnRemittance rem ON rem.DocumentID = doc.DocumentID ");
            sqlBuilder.Append(" LEFT JOIN MPADocument mpa ON mpa.DocumentID = doc.DocumentID ");
            sqlBuilder.Append(" LEFT JOIN CADocument ca ON ca.DocumentID = doc.DocumentID ");
            sqlBuilder.Append(" LEFT JOIN FixedAdvanceDocument fav ON fav.DocumentID = doc.DocumentID ");
            sqlBuilder.Append(" WHERE doc.DocumentID = :DocumentID");
            QueryParameterBuilder paramBuilder = new QueryParameterBuilder();
            paramBuilder.AddParameterData("DocumentID", typeof(long), DocumentID);
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            paramBuilder.FillParameters(query);
            query.AddScalar("ReportParam", NHibernateUtil.String);
            query.AddScalar("ReportName", NHibernateUtil.String);
            query.SetResultTransformer(Transformers.AliasToBean(typeof(DocumentReportName)));
            return query.UniqueResult<DocumentReportName>();
        }
        #endregion public DocumentReportName GetReportNameByDocumentID(long DocumentID)

        #region public IList<SearchResultData> FindInboxEmployeeSummaryCriteria(SearchCriteria criteria)
        public IList<SearchResultData> FindInboxEmployeeSummaryCriteria(SearchCriteria criteria)
        {
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            StringBuilder sqlBuilder = new StringBuilder();

            if (criteria.FlagQuery.Equals("Employee"))
            {
                sqlBuilder.AppendLine(" select ");
                sqlBuilder.AppendLine("     e.Name as DocumentStatus ");
                sqlBuilder.AppendLine(" from ");
                sqlBuilder.AppendLine("     Document a with (nolock) ");
                sqlBuilder.AppendLine("     inner join WorkFlow b with (nolock) ");
                sqlBuilder.AppendLine("     on a.DocumentID = b.DocumentID ");
                sqlBuilder.AppendLine("     inner join WorkFlowStateEventPermission c with (nolock) ");
                sqlBuilder.AppendLine("     on b.WorkFlowID = c.WorkFlowID ");
                sqlBuilder.AppendLine("     and c.UserID = :UserID ");
                sqlBuilder.AppendLine("     inner join WorkFlowStateEvent d with (nolock) ");
                sqlBuilder.AppendLine("     on c.WorkFlowStateEventID = d.WorkFlowStateEventID ");
                sqlBuilder.AppendLine("     inner join WorkFlowState e with (nolock) ");
                sqlBuilder.AppendLine("     on d.WorkFlowStateID = e.WorkFlowStateID ");
                sqlBuilder.AppendLine("     inner join WorkFlowType f with (nolock) ");
                sqlBuilder.AppendLine("     on b.WorkFlowTypeID = f.WorkFlowTypeID ");
                sqlBuilder.AppendLine("     left outer join SuUser g with (nolock) ");
                sqlBuilder.AppendLine("     on a.CreatorID = g.UserID ");
                sqlBuilder.AppendLine("     left outer join SuUser h with (nolock) ");
                sqlBuilder.AppendLine("     on a.RequesterID = h.UserID ");
                sqlBuilder.AppendLine("     left outer join AvAdvanceDocument i with (nolock) ");
                sqlBuilder.AppendLine("     on a.DocumentID = i.DocumentID ");
                sqlBuilder.AppendLine("     left outer join FnExpenseDocument j with (nolock) ");
                sqlBuilder.AppendLine("     on a.DocumentID = j.DocumentID ");
                sqlBuilder.AppendLine("     left outer join FnRemittance k with (nolock) ");
                sqlBuilder.AppendLine("     on a.DocumentID = k.DocumentID ");
                sqlBuilder.AppendLine("     and a.Active = 1 ");
                sqlBuilder.AppendLine(" where a.IsContainDocumentNo = 1 ");
                sqlBuilder.AppendLine("     and d.Name in ('Send','Approve') ");

                parameterBuilder.AddParameterData("UserID", typeof(long), criteria.UserID);
                if (criteria.UserID > 0)
                {
                    sqlBuilder.AppendLine("     and (a.CreatorID = :UserID ");
                    sqlBuilder.AppendLine("         or a.RequesterID = :UserID ");
                    sqlBuilder.AppendLine("         or c.UserID = :UserID) ");
                }
                if (!string.IsNullOrEmpty(criteria.DocumentStatus))
                {
                    sqlBuilder.AppendLine("     and e.Name = :Name ");
                    parameterBuilder.AddParameterData("Name", typeof(string), criteria.DocumentStatus);
                }
                if (!string.IsNullOrEmpty(criteria.FlagOutstanding))
                {
                    if (criteria.FlagOutstanding.Equals("Outstanding"))
                    {
                        sqlBuilder.AppendLine("     and i.RequestDateOfRemittance >= :currentDate ");
                    }
                    else
                    {
                        sqlBuilder.AppendLine("     and i.RequestDateOfRemittance < :currentDate ");
                    }
                    parameterBuilder.AddParameterData("currentDate", typeof(DateTime), DateTime.Now);
                }
            }
            else
            {
                sqlBuilder.AppendLine(" select ");
                sqlBuilder.AppendLine("     c.Name as DocumentStatus ");
                sqlBuilder.AppendLine(" from ");
                sqlBuilder.AppendLine("     Document a with (nolock) inner join WorkFlow b with (nolock) ");
                sqlBuilder.AppendLine("     on a.DocumentID = b.DocumentID ");
                sqlBuilder.AppendLine("     inner join DocumentType i with (nolock) ");
                sqlBuilder.AppendLine("     on a.DocumentTypeID = i.DocumentTypeID ");
                sqlBuilder.AppendLine("     inner join WorkFlowState c with (nolock) ");
                sqlBuilder.AppendLine("     on c.WorkFlowStateID = b.CurrentState ");
                sqlBuilder.AppendLine("     left outer join SuUser d with (nolock) ");
                sqlBuilder.AppendLine("     on a.CreatorID = d.UserID ");
                sqlBuilder.AppendLine("     left outer join SuUser e with (nolock) ");
                sqlBuilder.AppendLine("     on a.RequesterID = e.UserID ");
                sqlBuilder.AppendLine("     left outer join AvAdvanceDocument f with (nolock) ");
                sqlBuilder.AppendLine("     on a.DocumentID = f.DocumentID ");
                sqlBuilder.AppendLine("     left outer join FnExpenseDocument g with (nolock) ");
                sqlBuilder.AppendLine("     on a.DocumentID = g.DocumentID ");
                sqlBuilder.AppendLine("     left outer join FnRemittance h with (nolock) ");
                sqlBuilder.AppendLine("     on a.DocumentID = h.DocumentID ");
                sqlBuilder.AppendLine("     and a.Active = 1 ");
                sqlBuilder.AppendLine(" where isnull(a.DocumentNo,'') = '' ");
                sqlBuilder.AppendLine("     and c.Name = 'Draft' ");

                if (criteria.UserID > 0)
                {
                    sqlBuilder.AppendLine("     and a.CreatorID = :UserID ");
                    parameterBuilder.AddParameterData("UserID", typeof(long), criteria.UserID);
                }
            }
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            parameterBuilder.FillParameters(query);

            query.AddScalar("DocumentStatus", NHibernateUtil.String);

            return query.SetResultTransformer(Transformers.AliasToBean(typeof(SearchResultData))).List<SearchResultData>();
        }
        #endregion public IList<SearchResultData> FindInboxEmployeeSummaryCriteria(SearchCriteria criteria)

        #region public IList<SearchResultData> FindInboxAccountantPaymentSummaryCriteria(SearchCriteria criteria)
        public IList<SearchResultData> FindInboxAccountantPaymentSummaryCriteria(SearchCriteria criteria)
        {
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();

            string select = string.Empty;
            string from = string.Empty;
            string where = string.Empty;
            string sqlQuery = string.Empty;

            #region Select
            select = @" select Distinct
                        a.DocumentID as DocumentID, 
                        b.WorkflowID as WorkflowID, 
                        a.DocumentNo as RequestNo, 
                        a.DocumentTypeID as DocumentTypeID, 
                        a.DocumentDate as RequestDate, 
                        a.Subject as Subject, 
                        a.CreatorID as CreatorID, 
                        a.RequesterID as RequesterID, 
                        a.ApproverID as ApproverID, 
                        e.WorkFlowStateID as WorkFlowStateID, 
                        e.WorkFlowTypeID as WorkFlowTypeID, 
                        e.Name as DocumentStatus,
                        (case when isnull(a.documentno, '') = '' then null else TaskGroup end) as TaskGroup ";
            #endregion Select
            #region From
            from = @"   from
                        Document a with (nolock)
                        inner join WorkFlow b with (nolock)
                        on a.DocumentID = b.DocumentID                             
                        inner join WorkFlowState e with (nolock)
                        on b.CurrentState = e.WorkFlowStateID
                        inner join WorkFlowStateEventPermission c with (nolock)
                        on b.WorkFlowID = c.WorkFlowID
                        inner join WorkFlowEventClassification d with (nolock)
                        on d.DocumentTypeID = a.DocumentTypeID
                        and d.WorkFlowStateEventID = c.WorkFlowStateEventID and d.IsDirect = 1 ";
            #endregion From
            #region Where
            where = @" where (EXISTS (SELECT 'X' RoleID FROM SuUserRole WHERE SuUserRole.UserID = :UserID AND SuUserRole.RoleID = c.RoleID) or c.UserID = :UserID 
                        )and a.Active = 1 ";
            #endregion Where

            parameterBuilder.AddParameterData("UserID", typeof(long), criteria.UserID);

            sqlQuery = @" select TaskGroup, DocumentStatus, count(*) as ItemCount                           
                          from ( ";
            sqlQuery += select + from + where + ")temp group by TaskGroup, DocumentStatus ";

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlQuery);
            parameterBuilder.FillParameters(query);

            query.AddScalar("TaskGroup", NHibernateUtil.Int32);
            query.AddScalar("DocumentStatus", NHibernateUtil.String);
            query.AddScalar("ItemCount", NHibernateUtil.Int32);
            return query.SetResultTransformer(Transformers.AliasToBean(typeof(SearchResultData))).List<SearchResultData>();

        }
        #endregion public IList<SearchResultData> FindInboxAccountantPaymentSummaryCriteria(SearchCriteria criteria)

        #region public IList<SearchResultData> GetAccountantPaymentCriteriaList(SearchCriteria criteria)
        public IList<SearchResultData> GetAccountantPaymentCriteriaList(SearchCriteria criteria)
        {
            return FindInboxAccountantPaymentCriteria(criteria, false, string.Empty).List<SearchResultData>();
        }
        #endregion public IList<SearchResultData> GetAccountantPaymentCriteriaList(SearchCriteria criteria)

        #region public int CountDraftNoDocumentEmployeeCriteria(SearchCriteria criteria)
        public int CountDraftNoDocumentEmployeeCriteria(SearchCriteria criteria)
        {
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.AppendLine(" select count(*) as ItemCount ");
            sqlBuilder.AppendLine(" from Document a with (nolock) ");
            sqlBuilder.AppendLine(" inner join WorkFlow w with (nolock) on a.DocumentID = w.DocumentID ");
            sqlBuilder.AppendLine(" inner join WorkFlowState ws with (nolock) on w.CurrentState = ws.WorkFlowStateID ");
            sqlBuilder.AppendLine(" where a.Active=1 and isnull(a.DocumentNo,'') = '' and ws.Name <> 'Cancel' ");

            if (criteria.UserID > 0)
            {
                sqlBuilder.AppendLine(" and a.CreatorID = :UserID ");
                parameterBuilder.AddParameterData("UserID", typeof(long), criteria.UserID);
            }

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            parameterBuilder.FillParameters(query);

            query.AddScalar("ItemCount", NHibernateUtil.Int32);

            return query.UniqueResult<Int32>();
        }
        #endregion #region public int CountDraftNoDocumentEmployeeCriteria(SearchCriteria criteria)
        #region ISCGDocumentQuery Members


        public IList<long> GetDocumentIDFollowUpList()
        {
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            int WaitForDocumentFollowupDayValue = Convert.ToInt32(ParameterServices.WaitForDocumentFollowupDay);
            int WaitForDocumentRepeatFollowupDayValue = Convert.ToInt32(ParameterServices.WaitForDocumentRepeatFollowupDay);
            int sum = WaitForDocumentFollowupDayValue + WaitForDocumentRepeatFollowupDayValue;
            string sql = @"(select doc.documentid from document doc
                                inner join fnexpensedocument expdoc on doc.documentid = expdoc.documentid 
                                inner join workflow wf on wf.documentid = doc.documentid
                                inner join WorkFlowState wfs on wfs.WorkFlowStateID = wf.currentstate and wf.currentstate > 31 and wfs.Ordinal > 6 
                                inner join (select wf.workflowid, max(wfr.responsedate) as responsedate from workflow wf 
	                                inner join WorkFlowResponse wfr on wf.workflowid = wfr.workflowid
	                                inner join WorkflowStateEvent wfse on wfse.WorkflowStateEventID = wfr.WorkflowStateEventID and wfse.Name = 'Approve' 
	                                inner join WorkflowState ws on ws.WorkflowStateID = wfse.WorkflowStateID and ws.WorkflowStateID = 31
	                                where wfr.active = 1 group by wf.workflowid) x on wf.workflowid = x.workflowid
                                where doc.documenttypeid in (3,7) and wfs.Name <> 'Cancel'
                                and (expdoc.boxid is null or expdoc.BoxID = '')
                                and datediff(day,x.responsedate,getdate()) >= (select parametervalue from dbparameter where configurationname = 'WaitForReceivedDocFollowupDay')
                                and (datediff(day,x.responsedate,getdate())-(select parametervalue from dbparameter where configurationname = 'WaitForReceivedDocFollowupDay')) % (select parametervalue from dbparameter where configurationname = 'WaitForReceivedDocRepeatFollowupDay') = 0
                                )
                                union
                                (select wf.DocumentID from workflow wf 
                                inner join fnexpensedocument expdoc on wf.DocumentID = expdoc.DocumentID 
                                inner join WorkFlowState wfs on wfs.WorkFlowStateID = wf.currentstate and wf.currentstate = 37 --and wfs.Ordinal = 4 
                                inner join (select wf.workflowid, max(wfr.responsedate) as responsedate from workflow wf 
	                                inner join WorkFlowResponse wfr on wf.workflowid = wfr.workflowid
	                                inner join WorkflowStateEvent wfse on wfse.WorkflowStateEventID = wfr.WorkflowStateEventID and wfse.Name = 'Approve' 
	                                where wfr.active = 1 
	                                group by wf.workflowid) x on wf.workflowid = x.workflowid 
                                where   wf.WorkflowTypeID = 7
                                and (expdoc.boxid is null or expdoc.BoxID = '') and wfs.Name <> 'Cancel'
                                and datediff(day,x.responsedate,getdate()) in (:value1 , :value2) 
                                )";
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql);

            parameterBuilder.AddParameterData("value1", typeof(int), WaitForDocumentFollowupDayValue);
            parameterBuilder.AddParameterData("value2", typeof(int), sum);
            parameterBuilder.FillParameters(query);

            query.AddScalar("documentid", NHibernateUtil.Int64);
            return query.List<Int64>();
        }

        #endregion

        public IList<VOAdvanceOverDueReport> GetAdvanceOverdueList()
        {
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            string sql = @"select a.documentID as DocumentID, b.AdvanceID as AdvanceID, a.DocumentNo as DocumentNo
                from document a 
                left outer join avadvancedocument b  on a.documentid = b.documentid 
                left outer join workflow wf  on wf.documentid = a.documentid 
                left outer join workflowstate ws  on ws.WorkFlowStateID = wf.CurrentState 
                left outer join (select count(senddate) as countSendDate ,b.requestNo from document a 
                left outer join suemaillog b on a.documentNo = b.requestNo and b.emailtype = 'EM10' 
                where b.status = 1 group by b.requestNo) tableA on tableA.requestNo = a.DocumentNo
                left join (select x1.advanceid, ex1.boxid, x3.ordinal from fnexpensedocument ex1
                inner join fnexpenseadvance x1 on x1.expenseid = ex1.expenseid
                inner join workflow x2 on ex1.documentid = x2.documentid  
                inner join workflowstate x3 on x2.CurrentState = x3.WorkflowStateID where x3.Name <> 'Cancel') t1 on t1.advanceid = b.advanceID 
                where ws.Name = 'OutStanding' and isnull(CountSendDate,0) = 0 and isnull(t1.BoxID,'')='' and isnull(t1.ordinal,0) < 5
                and  convert(datetime,(case when b.RequestDateOfRemittance >= b.DueDateOfRemittance then b.RequestDateOfRemittance else b.DueDateOfRemittance end),103) <= convert(datetime,:CurrentDate,103)";

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql);

            parameterBuilder.AddParameterData("CurrentDate", typeof(string), DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy"));
            parameterBuilder.FillParameters(query);

            query.AddScalar("DocumentID", NHibernateUtil.Int64);
            query.AddScalar("DocumentNo", NHibernateUtil.String);
            query.AddScalar("AdvanceID", NHibernateUtil.Int64);
            query.SetResultTransformer(Transformers.AliasToBean(typeof(VOAdvanceOverDueReport)));

            return query.List<VOAdvanceOverDueReport>();
        }

        /*เพิ่มแจ้งเตือนก่อนครบกำหนด fixedadvance*/
        public IList<FixedAdvanceBeforeDue> GetFixedAdvanceBeforedueList()
        {
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            string sql = @"SELECT a.DocumentId
                FROM FixedAdvanceDocument a
                INNER JOIN document b ON a.DocumentId = b.DocumentId AND b.CacheCurrentStateName = 'Outstanding'
                WHERE DATEDIFF(DAY, GETDATE(), a.EffectiveToDate) > 0 AND (DATEDIFF(DAY, GETDATE(), a.EffectiveToDate) = :Before7Day OR DATEDIFF(DAY, GETDATE(), a.EffectiveToDate) = :Before30Day) ";
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql);

            parameterBuilder.AddParameterData("Before7Day", typeof(int), ParameterServices.EMailAlertBeforeDay1);
            parameterBuilder.AddParameterData("Before30Day", typeof(int), ParameterServices.EMailAlertBeforeDay2);
            parameterBuilder.FillParameters(query);

            query.AddScalar("DocumentId", NHibernateUtil.Int64);
            query.SetResultTransformer(Transformers.AliasToBean(typeof(FixedAdvanceBeforeDue)));
            return query.List<FixedAdvanceBeforeDue>();
        }
        public IList<FixedAdvanceOverDue> GetFixedAdvanceOverdueList()
        {
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            string sql = @"	SELECT b.RequesterID , b.CacheWorkflowID FROM [dbo].[FixedAdvanceDocument] a
                             INNER JOIN [dbo].[Document] b ON  a.DocumentID = b.DocumentID
                             WHERE   b.CacheCurrentStateName = 'Outstanding' AND ((DATEDIFF(DAY, a.EffectiveToDate, GETDATE()) > 0 AND DATEDIFF(DAY, a.EffectiveToDate, GETDATE()) = :EMailAlertOverdueDay1 ))";
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql);

            parameterBuilder.AddParameterData("EMailAlertOverdueDay1", typeof(int), ParameterServices.EMailAlertOverdueDay1);
            parameterBuilder.FillParameters(query);
            query.AddScalar("RequesterID", NHibernateUtil.Int64);
            query.AddScalar("CacheWorkflowID", NHibernateUtil.Int64);
            query.SetResultTransformer(Transformers.AliasToBean(typeof(FixedAdvanceOverDue)));
            return query.List<FixedAdvanceOverDue>();
        }

        #region query for FixedAdvance OverDue Report

        #region public FindByFixedAdvanceOverDueReportCriteria
        public ISQLQuery FindByFixedAdvanceOverDueReportCriteria(VOFixedAdvanceOverDueReport vo, bool isCount, string sortExpression)
        {
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            StringBuilder sqlBuilder = new StringBuilder();
            if (!isCount)
            {
                #region Query
                sqlBuilder.AppendLine(" SELECT DocumentID ,DocumentNo ,FixedAdvanceStatus,RefFixedAdvanceNo,RequesterID,FixedAdvanceID ");
                   sqlBuilder.AppendLine(" ,RequesterName,CreDate,EffectiveFromDate,EffectiveToDate,RequestDate,FixedAdvanceTypeName,RefFixedAdvanceID ");
                   sqlBuilder.AppendLine(" ,ReturnDate,Subject,Objective,FixedAdvanceType,Amount,NetAmount,Sendtime,DueDate,OverDueDay,CompanyID,LocationCode ");
                #endregion
            }
            else
            {
                #region Count query
                sqlBuilder.AppendLine(" SELECT COUNT(*) AS ReportCount ");
                #endregion
            }

            sqlBuilder.Append(" FROM ( SELECT doc.DocumentID ,doc.DocumentNo ,workflow.FixedAdvanceStatus,CASE WHEN ref.DocumentNo IS NULL THEN '' ELSE ref.DocumentNo END AS RefFixedAdvanceNo ");
            sqlBuilder.Append(" ,doc.RequesterID,u.EmployeeName AS RequesterName,doc.CreDate,fix.EffectiveFromDate,fix.EffectiveToDate,fix.RequestDate,fix.FixedAdvanceID,fix.RefFixedAdvanceID ");
            sqlBuilder.Append(" ,fix.ReturnRequestDate AS ReturnDate,doc.Subject,fix.Objective,CASE WHEN fix.FixedAdvanceType = 1 THEN 'New' ELSE 'Adjust' END AS FixedAdvanceTypeName ");
            sqlBuilder.Append(" ,fix.Amount,fix.NetAmount,(CASE WHEN tableA.countSendDate is null THEN 0 ELSE tableA.countSendDate END) AS Sendtime,fix.FixedAdvanceType,workflow.LanguageID ");
            sqlBuilder.Append(" ,CASE WHEN fix.RequestDate > fix.EffectiveToDate THEN fix.RequestDate ELSE fix.EffectiveToDate END AS DueDate,lo.CompanyID,lo.LocationCode ");
            sqlBuilder.Append(" ,DATEDIFF(DAY,(CASE WHEN fix.RequestDate >= fix.EffectiveToDate THEN fix.RequestDate ELSE fix.EffectiveToDate END),GETDATE()) AS OverDueDay ");
            sqlBuilder.Append(" FROM Document doc JOIN FixedAdvanceDocument AS fix ON fix.DocumentID = doc.DocumentID ");
            sqlBuilder.Append(" JOIN suuser AS u  ON u.userid = doc.RequesterID ");
            sqlBuilder.Append(" JOIN DbLocation AS lo ON lo.LocationID = u.LocationID ");
            sqlBuilder.Append(" JOIN ( SELECT wf.DocumentID ,wfl.DisplayName AS FixedAdvanceStatus,wfl.LanguageID FROM WorkFlow AS wf ");
            sqlBuilder.Append(" JOIN workflowstate AS ws  ON ws.WorkFlowStateID = wf.CurrentState AND ws.Name = 'Outstanding' ");/*AND ws.Name = 'Outstanding' */
            sqlBuilder.Append(" JOIN workflowstatelang wfl ON wfl.WorkFlowStateID = ws.WorkFlowStateID ");
            sqlBuilder.Append(" ) AS workflow ON workflow.DocumentID = doc.DocumentID ");
            sqlBuilder.Append(" LEFT OUTER JOIN ( SELECT F.FixedAdvanceID,D.DocumentNo FROM FixedAdvanceDocument AS F ");
            sqlBuilder.Append(" LEFT OUTER JOIN Document AS D ON D.DocumentID = F.DocumentID ");
            sqlBuilder.Append(" ) ref ON ref.FixedAdvanceID = fix.RefFixedAdvanceID ");
            sqlBuilder.Append(" LEFT OUTER JOIN (SELECT COUNT(senddate) AS countSendDate ,b.requestNo FROM document a ");
            sqlBuilder.Append(" LEFT OUTER JOIN suemaillog b ON a.documentNo = b.requestNo and b.emailtype = 'EM15' WHERE b.status = 1 group by b.requestNo ");
            sqlBuilder.Append(" ) tableA ON tableA.requestNo = doc.DocumentNO ) tb WHERE LanguageID = :languageid AND OverdueDay > 0 ");

            
            if (vo.LanguageID != 0)
            {
                parameterBuilder.AddParameterData("languageid", typeof(short), vo.LanguageID);
            }
            else
            {
                parameterBuilder.AddParameterData("languageid", typeof(short), 1);
            }
            if (vo.CompanyID != 0)
            {
                sqlBuilder.AppendLine(" AND CompanyID = :companyID ");
                parameterBuilder.AddParameterData("companyID", typeof(long), vo.CompanyID);
            }
            if (!string.IsNullOrEmpty(vo.FromLocationCode))
            {
                sqlBuilder.AppendLine(" AND LocationCode >= :locationForm ");
                parameterBuilder.AddParameterData("locationForm", typeof(string), vo.FromLocationCode);
            }
            if (!string.IsNullOrEmpty(vo.ToLocationCode))
            {
                sqlBuilder.AppendLine(" AND LocationCode <= :locationTo ");
                parameterBuilder.AddParameterData("locationTo", typeof(string), vo.ToLocationCode);
            }
            if (!string.IsNullOrEmpty(vo.FromDueDate))
            {
                sqlBuilder.AppendLine(" AND DueDate >= CONVERT(DATETIME,:dueDateForm,103) ");
                parameterBuilder.AddParameterData("dueDateForm", typeof(string), vo.FromDueDate);
            }
            if (!string.IsNullOrEmpty(vo.ToDueDate))
            {
                sqlBuilder.AppendLine(" AND DueDate <= CONVERT(DATETIME,:dueDateTo,103) ");
                parameterBuilder.AddParameterData("dueDateTo", typeof(string), vo.ToDueDate);
            }
            if (vo.FromFixedAdvanceAmount != 0)
            {
                sqlBuilder.AppendLine(" AND Amount >= :fromAmount ");
                parameterBuilder.AddParameterData("fromAmount", typeof(double), vo.FromFixedAdvanceAmount);
            }
            if (vo.ToFixedAdvanceAmount != 0)
            {
                sqlBuilder.AppendLine(" AND Amount <= :toAmount ");
                parameterBuilder.AddParameterData("toAmount", typeof(double), vo.ToFixedAdvanceAmount);
            }
            if (!string.IsNullOrEmpty(vo.FixedAdvanceType))
            {
                if (!vo.FixedAdvanceType.Equals("ALL"))
                {
                    sqlBuilder.AppendLine(" AND FixedAdvanceType = :fixedadvanceType ");
                    parameterBuilder.AddParameterData("fixedadvanceType", typeof(int), int.Parse(vo.FixedAdvanceType));
                }
            }
            if (vo.RequesterID != 0)
            {
                sqlBuilder.AppendLine(" AND RequesterID = :requesterID ");
                parameterBuilder.AddParameterData("requesterID", typeof(long), vo.RequesterID);
            }
            if (vo.FromOverDue != 0)
            {
                sqlBuilder.AppendLine(" AND OverDueDay >= :fromOverDue ");
                parameterBuilder.AddParameterData("fromOverDue", typeof(int), vo.FromOverDue);
            }
            if (vo.ToOverDue != 0)
            {
                sqlBuilder.AppendLine(" AND OverDueDay <= :toOverDue ");
                parameterBuilder.AddParameterData("toOverDue", typeof(int), vo.ToOverDue);
            }

            if (!isCount)
            {
                if (!string.IsNullOrEmpty(sortExpression))
                    sqlBuilder.AppendLine(string.Format(" ORDER BY {0}", sortExpression));
                else
                    sqlBuilder.AppendLine(" ORDER BY OverdueDay DESC , DocumentNo ASC ");
            }
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            parameterBuilder.FillParameters(query);

            if (!isCount)
            {
                query.AddScalar("DocumentID", NHibernateUtil.Int64);

                /*grid column*/
                query.AddScalar("DocumentNo", NHibernateUtil.String);
                query.AddScalar("CreDate", NHibernateUtil.DateTime);
                query.AddScalar("EffectiveFromDate", NHibernateUtil.DateTime);
                query.AddScalar("EffectiveToDate", NHibernateUtil.DateTime);
                query.AddScalar("RequesterName", NHibernateUtil.String);
                query.AddScalar("Subject", NHibernateUtil.String);
                query.AddScalar("Objective", NHibernateUtil.String);
                query.AddScalar("Amount", NHibernateUtil.Double);
                query.AddScalar("NetAmount", NHibernateUtil.Double);
                query.AddScalar("RefFixedAdvanceNo", NHibernateUtil.String);
                query.AddScalar("FixedAdvanceStatus", NHibernateUtil.String);
                query.AddScalar("DueDate", NHibernateUtil.DateTime);
                query.AddScalar("OverDueDay", NHibernateUtil.Int32);
                query.AddScalar("Sendtime", NHibernateUtil.Int32);

                query.AddScalar("FixedAdvanceID", NHibernateUtil.Int64);
                query.AddScalar("RefFixedAdvanceID", NHibernateUtil.Int64);
                query.AddScalar("RequesterID", NHibernateUtil.Int64);
                query.AddScalar("RequestDate", NHibernateUtil.DateTime);
                query.AddScalar("ReturnDate", NHibernateUtil.DateTime);
                query.AddScalar("FixedAdvanceTypeName", NHibernateUtil.String);


                query.SetResultTransformer(Transformers.AliasToBean(typeof(VOFixedAdvanceOverDueReport)));
            }
            else
            {
                query.AddScalar("ReportCount", NHibernateUtil.Int32);
                //query.UniqueResult();
            }
            return query;
        }
        #endregion public FindByFixedAdvanceOverDueReportCriteria

        #region public IList<VOFixedAdvanceOverDueReport> GetFixedAdvanceDocumentReportList(int firstResult, int maxResult, string sortExpression, long companyID, long locationForm, long locationTo, string dueDateForm, string dueDateTo, double amountFrom, double amountTo, int overDayFrom, int overDayTo, string advanceType, long requesterID)
        public IList<VOFixedAdvanceOverDueReport> GetFixedAdvanceDocumentReportList(int firstResult, int maxResult, string sortExpression, VOFixedAdvanceOverDueReport vo)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<VOFixedAdvanceOverDueReport>(ScgeAccountingQueryProvider.SCGDocumentQuery, "FindByFixedAdvanceOverDueReportCriteria", new object[] { vo, false, sortExpression }, firstResult, maxResult, sortExpression);
        }
        #endregion public IList<VOFixedAdvanceOverDueReport> GetFixedAdvanceDocumentReportList(int firstResult, int maxResult, string sortExpression, VOAdvanceOverDueReport vo)

        #region public int CountByFixedAdvanceReportCriteria(VOFixedAdvanceOverDueReport vo)
        public int CountByFixedAdvanceReportCriteria(VOFixedAdvanceOverDueReport vo)
        {
            return NHibernateQueryHelper.CountByCriteria(ScgeAccountingQueryProvider.SCGDocumentQuery, "FindByFixedAdvanceOverDueReportCriteria", new object[] { vo, true, string.Empty });
        }
        #endregion public int CountByFixedAdvanceReportCriteria(VOFixedAdvanceOverDueReport vo)

        #endregion


        public IList<long> GetDocumentIDByReferenceNo(string referenceNo)
        {
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            string sql = @"SELECT DocumentID FROM DOCUMENT WHERE ReferenceNo = :RefNo";

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql);

            parameterBuilder.AddParameterData("RefNo", typeof(string), referenceNo);
            parameterBuilder.FillParameters(query);

            query.AddScalar("DocumentID", NHibernateUtil.Int64);

            return query.List<long>();
        }

        public IList<long> GetDocumentIDByDocumentNo(string documentNo)
        {
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            string sql = @"SELECT DocumentID FROM DOCUMENT WHERE DocumentNo = :DocNo";

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql);

            parameterBuilder.AddParameterData("DocNo", typeof(string), documentNo);
            parameterBuilder.FillParameters(query);

            query.AddScalar("DocumentID", NHibernateUtil.Int64);

            return query.List<long>();
        }

        public ISQLQuery FindReportByCriteria(ReimbursementReportValueObj report, string sortExpression, bool isCount)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();

            if (!isCount)
            {
                sqlBuilder.Append(" select DocumentID, WorkflowID, RequestNo, RequestDate, Subject, Amount, ");
                sqlBuilder.Append(" AmountMainCurrency, AmountTHB, DocumentTypeName, StateName, PaidDate, PBID, Currency,Mark, FI_DOC  ");
            }
            else
            {
                sqlBuilder.Append(" select count(DocumentID) as Count ");
            }
            
            sqlBuilder.Append(" from ( ");
            sqlBuilder.Append(" select DocumentID, WorkflowID, RequestNo,  RequestDate,  Subject,  Amount,  AmountMainCurrency,   ");
            sqlBuilder.Append(" AmountTHB, DocumentTypeName, StateName, PaidDate, PBID,  Currency  ,Mark,FI_DOC ");
            sqlBuilder.Append(" from ( select Distinct  a.DocumentID as DocumentID, b.WorkflowID as WorkflowID, isnull(nullif(a.DocumentNo,''),'N/A') as RequestNo,   ");
            sqlBuilder.Append(" a.DocumentDate as RequestDate,  a.Subject as Subject,  isnull(ex2.DifferenceAmountLocalCurrency,0.00) as Amount,  isnull (ex2.DifferenceAmountMainCurrency , 0.00) as AmountMainCurrency,   ");
            sqlBuilder.Append(" isnull(ex2.DifferenceAmount,0.00) as AmountTHB,  i.DocumentTypeName as DocumentTypeName,  e.Name as StateName, ");
            sqlBuilder.Append(" (select ResponseDate from WorkFlowResponse wfRes ");
            sqlBuilder.Append("  inner join WorkFlowStateEvent wfEvent with (nolock) on wfRes.WorkFlowStateEventID = wfEvent.WorkFlowStateEventID ");
            sqlBuilder.Append("  where wfRes.WorkFlowID = b.WorkFlowID and wfRes.Active = 1 and wfEvent.Name = 'Pay' ");
            sqlBuilder.Append(" ) as PaidDate, ");
            sqlBuilder.Append(" isnull(ex2.PBID,null) as PBID,  dbC.Symbol as Currency ,a.Mark as Mark ,che09.FI_DOC as FI_DOC ");
            sqlBuilder.Append(" from Document a with (nolock)   ");
            sqlBuilder.Append(" inner join WorkFlow b with (nolock)on a.DocumentID = b.DocumentID   ");
            sqlBuilder.Append(" inner join DocumentType i with (nolock)on a.DocumentTypeID = i.DocumentTypeID   ");
            sqlBuilder.Append(" inner join WorkFlowState e with (nolock)on b.CurrentState = e.WorkFlowStateID and b.WorkflowTypeID = e.WorkflowTypeID and e.Name = 'Complete' ");
            sqlBuilder.Append(" left outer join FnExpenseDocument ex2 with (nolock) on ex2.DocumentID = a.DocumentID   ");
            sqlBuilder.Append(" left outer join DbCurrency dbC with (nolock) on dbC.CurrencyID = ex2.LocalCurrencyID ");
            sqlBuilder.Append(" left outer join BAPIACHE09 che09 with (nolock) on che09.DOC_ID = a.DocumentID  ");
            sqlBuilder.Append(" WHERE  a.DocumentTypeID in (3,7) and isnull(ex2.IsRepOffice,0) = 1 and che09.DOC_SEQ ='M' and che09.Doc_Kind=(case when isnull(ex2.DifferenceAmountLocalCurrency,0.00) < 0 then 'ExpenseRemittance' else 'Expense' end) ");
            sqlBuilder.Append(" ) as EXP ");

            sqlBuilder.Append(" UNION ");
            sqlBuilder.Append(" select DocumentID, WorkflowID, RequestNo,  RequestDate,  Subject,  Amount,  AmountMainCurrency,   ");
            sqlBuilder.Append(" AmountTHB, DocumentTypeName, StateName, PaidDate, PBID,  Currency  ,Mark,FI_DOC ");
            sqlBuilder.Append(" from (select Distinct  a.DocumentID as DocumentID, b.WorkflowID as WorkflowID, isnull(nullif(a.DocumentNo,''),'N/A') as RequestNo, ");
            sqlBuilder.Append(" a.DocumentDate as RequestDate,  a.Subject as Subject,  isnull(ex1.LocalCurrencyAmount,0.00) as Amount,  isnull(ex1.MainCurrencyAmount,0.00)  as AmountMainCurrency,  ");
            sqlBuilder.Append(" isnull(ex1.Amount,0.00) as AmountTHB,  i.DocumentTypeName as DocumentTypeName,  e.Name as StateName,  wfRes.ResponseDate as PaidDate,   ");
            sqlBuilder.Append(" isnull(ex1.PBID,null) as PBID,  dbC.Symbol as Currency,a.Mark as Mark,che09.FI_DOC as FI_DOC ");
            sqlBuilder.Append(" from Document a with (nolock)   ");
            sqlBuilder.Append(" inner join WorkFlow b with (nolock)on a.DocumentID = b.DocumentID   ");
            sqlBuilder.Append(" inner join DocumentType i with (nolock)on a.DocumentTypeID = i.DocumentTypeID   ");
            sqlBuilder.Append(" inner join WorkFlowState e with (nolock)on b.CurrentState = e.WorkFlowStateID and b.WorkflowTypeID = e.WorkflowTypeID and (e.Name = 'Complete' or e.Name = 'Outstanding') ");
            sqlBuilder.Append(" inner join WorkFlowResponse wfRes with (nolock) on b.WorkFlowID = wfRes.WorkFlowID   ");
            sqlBuilder.Append(" inner join WorkFlowStateEvent wfEvent with (nolock) on wfRes.WorkFlowStateEventID = wfEvent.WorkFlowStateEventID and wfEvent.Name = 'Pay' ");
            sqlBuilder.Append(" left outer join AvAdvanceDocument ex1 with (nolock) on ex1.DocumentID = a.DocumentID   ");
            sqlBuilder.Append(" left outer join AvAdvanceItem advItem with (nolock) on ex1.AdvanceID = advItem.AdvanceID   ");
            sqlBuilder.Append(" left outer join DbCurrency dbC with (nolock) on dbC.CurrencyID = advItem.CurrencyID ");
            sqlBuilder.Append(" left outer join BAPIACHE09 che09 with (nolock) on che09.DOC_ID = a.DocumentID  ");
            sqlBuilder.Append(" WHERE  a.DocumentTypeID = 1 and isnull(ex1.IsRepOffice,0) = 1 and che09.DOC_SEQ ='M' ");
            sqlBuilder.Append(" ) as ADV ");

            sqlBuilder.Append(" UNION ");
            sqlBuilder.Append(" select DocumentID, WorkflowID, RequestNo,  RequestDate,  Subject,  Amount,  AmountMainCurrency,   ");
            sqlBuilder.Append(" AmountTHB, DocumentTypeName, StateName, PaidDate, PBID,  Currency ,Mark ,FI_DOC ");
            sqlBuilder.Append(" from (select Distinct  a.DocumentID as DocumentID, b.WorkflowID as WorkflowID, isnull(nullif(a.DocumentNo,''),'N/A') as RequestNo,   ");
            sqlBuilder.Append(" a.DocumentDate as RequestDate,  a.Subject as Subject,  isnull(ex1.LocalCurrencyAmount,0.00) as Amount,  isnull(ex1.MainCurrencyAmount,0.00)  as AmountMainCurrency,   ");
            sqlBuilder.Append(" isnull(ex1.Amount,0.00) as AmountTHB,  i.DocumentTypeName as DocumentTypeName,  e.Name as StateName,  wfRes.ResponseDate as PaidDate,   ");
            sqlBuilder.Append(" isnull(ex1.PBID,null) as PBID,  null  as Currency,a.Mark as Mark,che09.FI_DOC as FI_DOC ");
            sqlBuilder.Append(" from Document a with (nolock)   ");
            sqlBuilder.Append("  inner join WorkFlow b with (nolock)on a.DocumentID = b.DocumentID   ");
            sqlBuilder.Append(" inner join DocumentType i with (nolock)on a.DocumentTypeID = i.DocumentTypeID   ");
            sqlBuilder.Append(" inner join WorkFlowState e with (nolock)on b.CurrentState = e.WorkFlowStateID and b.WorkflowTypeID = e.WorkflowTypeID and (e.Name = 'Complete' or e.Name = 'Outstanding') ");
            sqlBuilder.Append(" inner join WorkFlowResponse wfRes with (nolock) on b.WorkFlowID = wfRes.WorkFlowID   ");
            sqlBuilder.Append(" inner join WorkFlowStateEvent wfEvent with (nolock) on wfRes.WorkFlowStateEventID = wfEvent.WorkFlowStateEventID and wfEvent.Name = 'Pay'   ");
            sqlBuilder.Append(" left outer join AvAdvanceDocument ex1 with (nolock) on ex1.DocumentID = a.DocumentID   ");
            sqlBuilder.Append(" left outer join BAPIACHE09 che09 with (nolock) on che09.DOC_ID = a.DocumentID ");
            sqlBuilder.Append(" WHERE  a.DocumentTypeID = 5 and isnull(ex1.IsRepOffice,0) = 1  and che09.DOC_SEQ ='M'  ");
            sqlBuilder.Append(" ) as ADF  ");

            sqlBuilder.Append(" UNION  ");
            sqlBuilder.Append(" select DocumentID, WorkflowID, RequestNo,  RequestDate,  Subject,  Amount,  AmountMainCurrency, ");
            sqlBuilder.Append(" AmountTHB, DocumentTypeName, StateName, PaidDate, PBID,  Currency ,Mark ,FI_DOC ");
            sqlBuilder.Append(" from (select Distinct  a.DocumentID as DocumentID, b.WorkflowID as WorkflowID, isnull(nullif(a.DocumentNo,''),'N/A') as RequestNo, ");
            sqlBuilder.Append(" a.DocumentDate as RequestDate,  a.Subject as Subject,null as Amount,  (-1*isnull(ex3.MainCurrencyAmount,0.00)) as AmountMainCurrency, ");
            sqlBuilder.Append(" (-1*isnull(ex3.TotalAmount,0.00)) as AmountTHB,  i.DocumentTypeName as DocumentTypeName,  e.Name as StateName,  wfRes.ResponseDate as PaidDate, ");
            sqlBuilder.Append(" isnull(ex3.PBID,null) as PBID,  null as Currency ,a.Mark as Mark ,che09.FI_DOC as FI_DOC ");
            sqlBuilder.Append(" from Document a with (nolock)   ");
            sqlBuilder.Append("  inner join WorkFlow b with (nolock)on a.DocumentID = b.DocumentID  ");
            sqlBuilder.Append(" inner join DocumentType i with (nolock)on a.DocumentTypeID = i.DocumentTypeID  ");
            sqlBuilder.Append(" inner join WorkFlowState e with (nolock)on b.CurrentState = e.WorkFlowStateID and b.WorkflowTypeID = e.WorkflowTypeID and e.Name = 'Complete' ");
            sqlBuilder.Append(" inner join WorkFlowResponse wfRes with (nolock) on b.WorkFlowID = wfRes.WorkFlowID ");
            sqlBuilder.Append(" inner join WorkFlowStateEvent wfEvent with (nolock) on wfRes.WorkFlowStateEventID = wfEvent.WorkFlowStateEventID and wfEvent.Name = 'Approve' ");
            sqlBuilder.Append(" left outer join FnRemittance ex3 with (nolock) on ex3.DocumentID = a.DocumentID   ");
            sqlBuilder.Append(" left outer join BAPIACHE09 che09 with (nolock) on che09.DOC_ID = a.DocumentID  ");
            sqlBuilder.Append(" WHERE  a.DocumentTypeID = 4 and isnull(ex3.IsRepOffice,0) = 1 and che09.DOC_SEQ ='M' ");
            sqlBuilder.Append(" ) as RMT ");

            #region WhereClause
            StringBuilder whereClauseBuilder = new StringBuilder();
            if (report.PBID.HasValue)
            {
                whereClauseBuilder.Append(" and PBID = :PBID ");
                queryParameterBuilder.AddParameterData("PBID", typeof(string), String.Format("{0}", report.PBID.Value.ToString()));
            }


            if (report.RequestDateFrom.HasValue)
            {
                whereClauseBuilder.Append(" and RequestDate >= :RequestDateFrom ");
                queryParameterBuilder.AddParameterData("RequestDateFrom", typeof(DateTime), report.RequestDateFrom.Value);
            }

            if (report.RequestDateTo.HasValue)
            {
                whereClauseBuilder.Append(" and RequestDate < :RequestDateTo ");
                queryParameterBuilder.AddParameterData("RequestDateTo", typeof(DateTime), report.RequestDateTo.Value.AddDays(1));
            }

            if (report.PaidDateFrom.HasValue)
            {
                whereClauseBuilder.Append(" and PaidDate >= :PaidDateFrom ");
                queryParameterBuilder.AddParameterData("PaidDateFrom", typeof(DateTime), report.PaidDateFrom.Value);
            }

            if (report.PaidDateTo.HasValue)
            {
                whereClauseBuilder.Append(" and PaidDate < :PaidDateTo ");
                queryParameterBuilder.AddParameterData("PaidDateTo", typeof(DateTime), report.PaidDateTo.Value.AddDays(1));
            }

            if (!string.IsNullOrEmpty(report.RequestNoFrom))
            {
                whereClauseBuilder.Append(" and RequestNo >= :RequestNoFrom ");
                queryParameterBuilder.AddParameterData("RequestNoFrom", typeof(string), String.Format("{0}", report.RequestNoFrom));
            }
            if (!string.IsNullOrEmpty(report.RequestNoTo))
            {
                whereClauseBuilder.Append(" and RequestNo <= :RequestNoTo ");
                queryParameterBuilder.AddParameterData("RequestNoTo", typeof(string), String.Format("{0}", report.RequestNoTo));
            }

            if (report.MarkDocument.HasValue && report.MarkDocument.Value == true)
            {
                whereClauseBuilder.Append(" and Mark = :MarkDocument ");
                queryParameterBuilder.AddParameterData("MarkDocument", typeof(string), String.Format("{0}", report.MarkDocument));
            }
            else if (report.MarkDocument.HasValue && report.MarkDocument.Value == false)
            {
                whereClauseBuilder.Append(" and (Mark = :MarkDocument or Mark is null) ");
                queryParameterBuilder.AddParameterData("MarkDocument", typeof(string), String.Format("{0}", report.MarkDocument));
            }
            sqlBuilder.Append(" ) table1 ");
            if (!string.IsNullOrEmpty(whereClauseBuilder.ToString()))
            {
                sqlBuilder.Append(String.Format(" where 1=1  {0} ", whereClauseBuilder.ToString()));
            }

            #endregion
            #region Order By
            if (!isCount)
            {
                if (!string.IsNullOrEmpty(sortExpression))
                {
                    sqlBuilder.Append(String.Format(" order by {0} ", sortExpression));
                }
                else
                {
                    sqlBuilder.Append("order by PaidDate ,RequestNo");
                }
            }
            #endregion

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            queryParameterBuilder.FillParameters(query);

            if (isCount)
            {
                query.AddScalar("Count", NHibernateUtil.Int32);
            }
            else
            {


                query.AddScalar("DocumentID", NHibernateUtil.Int64)
                     .AddScalar("WorkflowID", NHibernateUtil.Int64)
                     .AddScalar("RequestNo", NHibernateUtil.String)
                     .AddScalar("RequestDate", NHibernateUtil.DateTime)
                    .AddScalar("Subject", NHibernateUtil.String)
                    .AddScalar("Amount", NHibernateUtil.Double)
                    .AddScalar("AmountMainCurrency", NHibernateUtil.Double)
                    .AddScalar("AmountTHB", NHibernateUtil.Double)
                    .AddScalar("DocumentTypeName", NHibernateUtil.String)
                    .AddScalar("StateName", NHibernateUtil.String)
                    .AddScalar("PaidDate", NHibernateUtil.DateTime)
                    .AddScalar("PBID", NHibernateUtil.Int64)
                    .AddScalar("Currency", NHibernateUtil.String)
                    .AddScalar("Mark", NHibernateUtil.Boolean)
                    .AddScalar("FI_DOC", NHibernateUtil.String);


                query.SetResultTransformer(Transformers.AliasToBean(typeof(ReimbursementReportValueObj)));
            }

            return query;
        }
        public IList<ReimbursementReportValueObj> GetReportList(ReimbursementReportValueObj report, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<ReimbursementReportValueObj>(ScgeAccountingQueryProvider.SCGDocumentQuery, "FindReportByCriteria", new object[] { report, sortExpression, false }, firstResult, maxResult, sortExpression);

        }
        public int CountReportByCriteria(ReimbursementReportValueObj report)
        {
            return NHibernateQueryHelper.CountByCriteria(ScgeAccountingQueryProvider.SCGDocumentQuery, "FindReportByCriteria", new object[] { report, string.Empty, true });
        }

        public ReimbursementReportValueObj GetPeriodPaidDate(string markList, string unmarkList)
        {
            ISQLQuery query = GetCurrentSession().CreateSQLQuery("EXECUTE  ReimbursementReport :DocList");
            string criteria = markList;
            if (!criteria.EndsWith(","))
            {
                criteria += ",";
            }
            criteria += unmarkList;

            query.SetString("DocList", criteria);
            query.AddScalar("DocumentID", NHibernateUtil.Int64)
                     .AddScalar("WorkflowID", NHibernateUtil.Int64)
                     .AddScalar("RequestNo", NHibernateUtil.String)
                     .AddScalar("RequestDate", NHibernateUtil.DateTime)
                    .AddScalar("Subject", NHibernateUtil.String)
                    .AddScalar("Amount", NHibernateUtil.Double)
                    .AddScalar("AmountMainCurrency", NHibernateUtil.Double)
                    .AddScalar("AmountTHB", NHibernateUtil.Double)
                    .AddScalar("DocumentTypeName", NHibernateUtil.String)
                    .AddScalar("StateName", NHibernateUtil.String)
                    .AddScalar("PaidDate", NHibernateUtil.DateTime)
                    .AddScalar("PBID", NHibernateUtil.Int64)
                    .AddScalar("Currency", NHibernateUtil.String);


            query.SetResultTransformer(Transformers.AliasToBean(typeof(ReimbursementReportValueObj)));
            ReimbursementReportValueObj result = new ReimbursementReportValueObj();

            IList<ReimbursementReportValueObj> list = query.List<ReimbursementReportValueObj>();
            if (list.Count > 0)
            {
                result.MinPaidDate = list.Min(x => x.PaidDate);
                result.MaxPaidDate = list.Max(x => x.PaidDate);
            }

            return result;
        }

        public ISQLQuery FindTADocumentByCriteria(TASearchCriteria criteria, bool isCount, string sortExpression)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            if (!isCount)
            {
                sqlBuilder.Append(@" select DocumentID, WorkflowID, RequestNo, Subject, RequesterName, CreatorName, CompanyCode, CompanyName, RequestDate, ApproveDate ");
            }
            else
            {
                sqlBuilder.Append(" select count(DocumentID) as Count ");
            }
            string sql = @" from ( select distinct doc.DocumentID, wf.WorkflowID, doc.DocumentNo as RequestNo, doc.Subject, requester.EmployeeName as RequesterName, creator.EmployeeName as CreatorName, com.CompanyCode, com.CompanyName,doc.DocumentDate as RequestDate,
                (select top 1 ResponseDate from WorkFlowResponse wr inner join WorkflowStateEvent we on we.WorkflowStateEventID = wr.WorkflowStateEventID 
                 where wr.Active = 1 and we.[Name]='Approve' and wr.WorkflowID = wf.WorkflowID order by ResponseDate asc) as ApproveDate 
                from [Document] doc
                inner join DbCompany com with (nolock) on com.CompanyID = doc.CompanyID
                inner join dbo.DbCompanyForTASearch comTA with (nolock) on com.CompanyCode = comTA.CompanyCode
                inner join Workflow wf with (nolock) on doc.DocumentID = wf.DocumentID
                inner join SuUser creator with (nolock) on doc.CreatorID = creator.UserID
                inner join SuUser requester with (nolock) on doc.RequesterID = requester.UserID
                inner join TADocument ta with (nolock) on doc.DocumentID = ta.DocumentID
                inner join TADocumentTraveller taTraveller with (nolock) on taTraveller.TADocumentID = ta.TADocumentID
                inner join SuUser traveller with (nolock) on taTraveller.UserID = traveller.UserID
                inner join WorkflowState ws with (nolock) on wf.CurrentState = ws.WorkflowStateID
                inner join WorkflowResponse wr with (nolock) on wr.WorkflowID = wf.WorkflowID
                inner join WorkflowStateEvent we with (nolock) on we.WorkflowStateEventID = wr.WorkflowStateEventID and we.[Name] = 'Approve'
                where doc.DocumentTypeID in (2, 8) and ws.[Name] = 'Complete' and wr.Active = 1 ";
            sqlBuilder.Append(sql);

            if (criteria.CompanyID > 0)
            {
                sqlBuilder.Append(" and doc.CompanyID = :CompanyID ");
                queryParameterBuilder.AddParameterData("CompanyID", typeof(Int32), criteria.CompanyID);
            }
            if (criteria.DocumentTypeID > 0)
            {
                sqlBuilder.Append(" and doc.DocumentTypeID = :DocumentTypeID ");
                queryParameterBuilder.AddParameterData("DocumentTypeID", typeof(Int32), criteria.DocumentTypeID);
            }
            if (!string.IsNullOrEmpty(criteria.RequestNo))
            {
                sqlBuilder.Append(" and doc.DocumentNo like :RequestNo");
                queryParameterBuilder.AddParameterData("RequestNo", typeof(string), "%" + criteria.RequestNo + "%");
            }
            if (!string.IsNullOrEmpty(criteria.Subject))
            {
                sqlBuilder.Append(" and doc.Subject Like :Subject");
                queryParameterBuilder.AddParameterData("Subject", typeof(string), "%" + criteria.Subject + "%");
            }
            if (criteria.CreatorID > 0)
            {
                sqlBuilder.Append(" and doc.CreatorID = :CreatorID ");
                queryParameterBuilder.AddParameterData("CreatorID", typeof(Int32), criteria.CreatorID);
            }
            if (!string.IsNullOrEmpty(criteria.TravellerNameTH))
            {
                sqlBuilder.Append(" and traveller.EmployeeName like :TravellerNameTH ");
                queryParameterBuilder.AddParameterData("TravellerNameTH", typeof(string), "%" + criteria.TravellerNameTH + "%");
            }
            if (!string.IsNullOrEmpty(criteria.TravellerNameEN))
            {
                //sqlBuilder.Append(" or exists( select 'X' from TADocumentTraveller where TADocumentID = ta.TADocumentID and EmployeeNameEng like :TravellerNameEN ) ");
                sqlBuilder.Append(" and taTraveller.EmployeeNameEng like :TravellerNameEN ");
                queryParameterBuilder.AddParameterData("TravellerNameEN", typeof(string), "%" + criteria.TravellerNameEN + "%");
            }
            if (!string.IsNullOrEmpty(criteria.Country))
            {
                sqlBuilder.Append(" and Country like :Country");
                queryParameterBuilder.AddParameterData("Country", typeof(string), "%" + criteria.Country + "%");
            }
            if (!string.IsNullOrEmpty(criteria.Province))
            {
                sqlBuilder.Append(" and Province like :Province");
                queryParameterBuilder.AddParameterData("Province", typeof(string), "%" + criteria.Province + "%");
            }
            if (criteria.RequestDateFrom.HasValue)
            {
                sqlBuilder.Append(" and doc.DocumentDate >= :RequestDateFrom");
                queryParameterBuilder.AddParameterData("RequestDateFrom", typeof(DateTime), criteria.RequestDateFrom);
            }
            if (criteria.RequestDateTo.HasValue)
            {
                sqlBuilder.Append(" and doc.DocumentDate <= :RequestDateTo");
                queryParameterBuilder.AddParameterData("RequestDateTo", typeof(DateTime), criteria.RequestDateTo);
            }
            if (criteria.ApproveDateFrom.HasValue)
            {
                sqlBuilder.Append(" and wr.ResponseDate >= :ApproveDateFrom");
                queryParameterBuilder.AddParameterData("ApproveDateFrom", typeof(DateTime), criteria.ApproveDateFrom);
            }
            if (criteria.ApproveDateTo.HasValue)
            {
                sqlBuilder.Append(" and wr.ResponseDate <= :ApproveDateTo");
                queryParameterBuilder.AddParameterData("ApproveDateTo", typeof(DateTime), criteria.ApproveDateTo);
            }


            sqlBuilder.Append(" )t1 ");

            #region Order By
            if (!isCount)
            {
                if (!string.IsNullOrEmpty(sortExpression))
                {
                    sqlBuilder.Append(String.Format(" order by {0} ", sortExpression));
                }
                else
                {
                    sqlBuilder.Append("order by RequestNo");
                }
            }
            #endregion

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            queryParameterBuilder.FillParameters(query);
            if (isCount)
            {
                query.AddScalar("Count", NHibernateUtil.Int32);
            }
            else
            {
                query.AddScalar("DocumentID", NHibernateUtil.Int64)
                     .AddScalar("WorkflowID", NHibernateUtil.Int64)
                     .AddScalar("RequestNo", NHibernateUtil.String)
                    .AddScalar("Subject", NHibernateUtil.String)
                    .AddScalar("RequesterName", NHibernateUtil.String)
                    .AddScalar("CreatorName", NHibernateUtil.String)
                    .AddScalar("CompanyCode", NHibernateUtil.String)
                    .AddScalar("CompanyName", NHibernateUtil.String)
                    .AddScalar("RequestDate", NHibernateUtil.DateTime)
                    .AddScalar("ApproveDate", NHibernateUtil.DateTime);

                query.SetResultTransformer(Transformers.AliasToBean(typeof(TASearchResultData)));
            }

            return query;
        }

        public IList<TASearchResultData> GetTADocumentCriteriaList(TASearchCriteria criteria, int firstResult, int maxResult, string sortExpression) // สำหรับ method RequestData เรียกใช้
        {
            return NHibernateQueryHelper.FindPagingByCriteria<TASearchResultData>(ScgeAccountingQueryProvider.SCGDocumentQuery, "FindTADocumentByCriteria", new object[] { criteria, false, sortExpression }, firstResult, maxResult, sortExpression);
        }

        public int CountTADocumentCriteria(TASearchCriteria criteria)
        {
            return NHibernateQueryHelper.CountByCriteria(ScgeAccountingQueryProvider.SCGDocumentQuery, "FindTADocumentByCriteria", new object[] { criteria, true, string.Empty });
        }

        public IList<SCGDocumentEmail> FindDocumentWaitApprove()
        {
            ISQLQuery query = GetCurrentSession().CreateSQLQuery("EXECUTE [FindDocumentWaitApprove]");
            query.AddScalar("DocumentID", NHibernateUtil.Int64)
                 .AddScalar("DocumentDate", NHibernateUtil.DateTime)
                 .AddScalar("EmployeeName", NHibernateUtil.String)
                 .AddScalar("UserName", NHibernateUtil.String)
                 .AddScalar("CacheWorkflowID", NHibernateUtil.Int64)
                 .AddScalar("UserID", NHibernateUtil.Int64)
                 .AddScalar("ApproverEmail", NHibernateUtil.String);
            query.SetResultTransformer(Transformers.AliasToBean(typeof(SCGDocumentEmail)));
            SCGDocumentEmail result = new SCGDocumentEmail();
            IList<SCGDocumentEmail> list = query.List<SCGDocumentEmail>();
            return list;
        }

    }
}
