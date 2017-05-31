using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.QueryDao;
using SCG.eAccounting.DTO;
using SCG.eAccounting.DTO.ValueObject;
using NHibernate;
using NHibernate.Transform;
using SS.Standard.Data.NHibernate.QueryCreator;
using NHibernate.Expression;


namespace SCG.eAccounting.Query.Hibernate
{
    public class DocumentInitiatorQuery : NHibernateQueryBase<DocumentInitiator, long>, IDocumentInitiatorQuery
    {
        #region IDocumentInitiatorDao Members

        public IList<InitiatorData> GetDocumentInitiatorList(long documentID)
        {
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            string sqlCommand = @"SELECT  DocumentInitiator.Seq as InitiatorSeq, SuUser.EmployeeName as InitiatorName, SuUser.Email as InitiatorEmail, 
            (case DocumentInitiator.InitiatorType when '1' then 'Accept' when '2' then 'CC' end) as InitialType, ISNULL(SuUser.SMSApproveOrReject,0) as IsSMS 
            FROM   DocumentInitiator 
            INNER JOIN SuUser ON DocumentInitiator.UserID = SuUser.UserID 
            WHERE DocumentInitiator.DocumentID = :DocumentID
            Order By DocumentInitiator.Seq";
            parameterBuilder.AddParameterData("DocumentID", typeof(long), documentID);
            
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlCommand);
            parameterBuilder.FillParameters(query);
            query.AddScalar("InitiatorSeq", NHibernateUtil.Int16)
                .AddScalar("InitiatorName", NHibernateUtil.String)
                .AddScalar("InitiatorEmail", NHibernateUtil.String)
                .AddScalar("InitialType", NHibernateUtil.String)
                .AddScalar("IsSMS", NHibernateUtil.Boolean);

            return query.SetResultTransformer(Transformers.AliasToBean(typeof(InitiatorData))).List<InitiatorData>();


            // query.AddEntity(typeof(DocumentInitiatorLang));
            //return query.List<DocumentInitiatorLang>();
        }


        public IList<DocumentInitiator> GetDocumentInitiatorByDocumentID(long documentID)
        {
            return GetCurrentSession().CreateQuery("from DocumentInitiator where DocumentID = :DocumentID and Active='1'")
                .SetInt64("DocumentID", documentID)
                .List<DocumentInitiator>();
        }

        public IList<DocumentInitiator> GetResponsedInitiatorByDocumentID(long documentID)
        {
            return GetCurrentSession().CreateQuery("from DocumentInitiator where DocumentID = :DocumentID and Active = '1' and InitiatorType = '1' and DoApprove='1' order by Seq")
                .SetInt64("DocumentID", documentID)
                .List<DocumentInitiator>();
        }

        public IList<DocumentInitiator> GetNotResponseInitiatorByDocumentID(long documentID)
        {
            return GetCurrentSession().CreateQuery("from DocumentInitiator where DocumentID = :DocumentID and Active = '1' and InitiatorType = '1' and DoApprove='0' and IsSkip = '0' order by Seq")
                .SetInt64("DocumentID", documentID)
                .List<DocumentInitiator>();
        }

        public DocumentInitiator GetNextResponseInitiatorByDocumentID(long documentID)
        {
            IList<DocumentInitiator> documentInitiators = GetNotResponseInitiatorByDocumentID(documentID);
            if (documentInitiators.Count > 0)
            {
             return documentInitiators[0];
            }
            else
            {
             return null;
            }
        }

        public IList<DocumentInitiator> GetOverRoleInitiatorByDocumentID(long documentID)
        {
            return GetCurrentSession().CreateQuery("from DocumentInitiator where DocumentID = :DocumentID and Active = '1' and IsSkip = '1' order by Seq")
                .SetInt64("DocumentID", documentID)
                .List<DocumentInitiator>();
        }

        public IList<DocumentInitiator> GetCCInitiatorByDocumentID(long documentID)
        {
            return GetCurrentSession().CreateQuery("from DocumentInitiator where DocumentID = :DocumentID and Active = '1' and InitiatorType = '2' order by Seq")
                .SetInt64("DocumentID", documentID)
                .List<DocumentInitiator>();
        }

        public IList<DocumentInitiator> FindDocumentInitiatorByDocumentID_UserID(long documentID , long userID)
        {
            string sqlQuery = @" SELECT * FROM  DocumentInitiator with (nolock) where DocumentID = :DocumentID and UserID = :UserID ";

            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            parameterBuilder.AddParameterData("DocumentID", typeof(long), documentID);
            parameterBuilder.AddParameterData("UserID", typeof(long), userID);
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlQuery);
            parameterBuilder.FillParameters(query);
            query.AddScalar("InitiatorID", NHibernateUtil.Int64);
            query.AddScalar("Seq", NHibernateUtil.Int16);
            query.AddScalar("InitiatorType", NHibernateUtil.String);
            //query.AddEntity("DocumentID", typeof(SCGDocument));
            return query.SetResultTransformer(Transformers.AliasToBean(typeof(DocumentInitiator))).List<DocumentInitiator>();
        }

        public IList<UserFavoriteInitiator> FindUserFavoriteInitiatorByUserID(InitiatorCriteria criteria, long UserID)
        {
            //ActoType = 2 : Initialtor 
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT Row_Number() OVER(ORDER BY SuUserFavoriteActor.UserID DESC) as InitiatorSeq,  SuUserFavoriteActor.UserFavoriteActorID");
            sqlBuilder.Append(", SuUserFavoriteActor.UserID");
            sqlBuilder.Append(", SuUserFavoriteActor.ActorType");
            sqlBuilder.Append(", SuUserFavoriteActor.ActorUserID");
            sqlBuilder.Append(", SuUser.Email");
            sqlBuilder.Append(", SuUser.EmployeeName");
            sqlBuilder.Append(", SuUser.SMS");
            sqlBuilder.Append(" FROM  SuUserFavoriteActor LEFT OUTER JOIN");
            sqlBuilder.Append("    SuUser ON SuUserFavoriteActor.ActorUserID = SuUser.UserID ");
            sqlBuilder.Append(" WHERE SuUserFavoriteActor.UserID = :UserID");
            sqlBuilder.AppendFormat(" AND SuUserFavoriteActor.ActorType = {0}", "2");

            if (criteria.UserIDFilter != null && criteria.UserIDFilter.Count > 0)
            {
                string filter = "";
                foreach (DocumentInitiatorLang item in criteria.UserIDFilter)
                {
                    filter += "'" + item.UserID.ToString() + "',";
                }
                sqlBuilder.AppendFormat(" AND SuUserFavoriteActor.ActorUserID NOT IN ({0})", filter.TrimEnd(','));
            }
            sqlBuilder.Append(" ORDER BY SuUserFavoriteActor.ActorUserID ASC");

            parameterBuilder.AddParameterData("UserID", typeof(long), UserID);


            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            parameterBuilder.FillParameters(query);
            query.AddScalar("UserFavoriteActorID", NHibernateUtil.Int64)
                .AddScalar("UserID", NHibernateUtil.Int64)
                .AddScalar("ActorUserID", NHibernateUtil.Int64)
                .AddScalar("ActorType", NHibernateUtil.String)
                .AddScalar("EmployeeName", NHibernateUtil.String)
                .AddScalar("Email", NHibernateUtil.String)
                .AddScalar("SMS", NHibernateUtil.Boolean)
                .AddScalar("InitiatorSeq", NHibernateUtil.Int16);

            return query.SetResultTransformer(Transformers.AliasToBean(typeof(UserFavoriteInitiator))).List<UserFavoriteInitiator>();
        }
      
        #endregion

        public IList<DocumentInitiatorLang> GetDocumentInitiatorByDocumentIDAndInitiatorType(long documentID)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT di.InitiatorID AS InitiatorID , u.UserID as UserID , u.EmployeeName AS EmployeeName , u.Email AS Email , u.SMS AS SMS , di.isSkip AS isSkip , di.SkipReason AS SkipReason ");
            sql.Append("FROM DocumentInitiator AS di ");
            sql.Append("INNER JOIN SuUser AS u ON u.UserID = di.UserID ");
            sql.Append("WHERE di.DocumentID =:documentID AND InitiatorType = '1' ");
            sql.Append("ORDER BY di.Seq ASC");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            parameterBuilder.AddParameterData("documentID", typeof(long), documentID);
            parameterBuilder.FillParameters(query);
            query.AddScalar("InitiatorID", NHibernateUtil.Int64);
            query.AddScalar("UserID", NHibernateUtil.Int64);
            query.AddScalar("EmployeeName", NHibernateUtil.String);
            query.AddScalar("Email", NHibernateUtil.String);
            query.AddScalar("SMS", NHibernateUtil.Boolean);
            query.AddScalar("isSkip", NHibernateUtil.Boolean);
            query.AddScalar("SkipReason", NHibernateUtil.String);

            return query.SetResultTransformer(Transformers.AliasToBean(typeof(DocumentInitiatorLang))).List<DocumentInitiatorLang>();
        }

    }
}
