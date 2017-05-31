using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.eAccounting.DTO;
using SS.Standard.Data.NHibernate.QueryDao;
using NHibernate;
using System.Collections;
using NHibernate.Expression;
using SS.Standard.Data.NHibernate.QueryCreator;
using SCG.eAccounting.DTO.ValueObject;
using NHibernate.Transform;
using SS.SU.DTO;

namespace SCG.eAccounting.Query.Hibernate
{
    public class TADocumentAdvanceQuery : NHibernateQueryBase<TADocumentAdvance, int>, ITADocumentAdvanceQuery
    {
        #region public IList<TADocumentAdvance> FindTADocumentAdvanceByTADocumentID(long taDocumentID)
        public IList<TADocumentAdvance> FindTADocumentAdvanceByTADocumentID(long taDocumentID)
        {
            ICriteria criteria = GetCurrentSession().CreateCriteria(typeof(TADocumentAdvance), "t");
            criteria.Add(Expression.Eq("t.TADocument.TADocumentID", taDocumentID));

            return criteria.List<TADocumentAdvance>();
        }
        #endregion public IList<TADocumentAdvance> FindTADocumentAdvanceByTADocumentID(long taDocumentID)

        #region public IList<TADocumentObj> FindAdvanceByTADocumentID(short languageID,long taDocumentID)
        public IList<TADocumentObj> FindAdvanceByTADocumentID(short languageID,long taDocumentID)
        {
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.AppendLine(" SELECT ");
            sqlBuilder.AppendLine("     a.DocumentID as DocumentID, ");
            sqlBuilder.AppendLine("     a.DocumentNo as AdvanceNo, ");
            sqlBuilder.AppendLine("     d.DisplayName as AdvanceStatus, ");
            sqlBuilder.AppendLine("     a.Subject as Description, ");
            sqlBuilder.AppendLine("     e.UserName as RequesterName, ");
            sqlBuilder.AppendLine("     e.UserID as RequesterID, ");
            sqlBuilder.AppendLine("     f.UserName as ReceiverName, ");
            sqlBuilder.AppendLine("     av.RequestDateOfRemittance as DueDate, ");
            sqlBuilder.AppendLine("     av.Amount as AmountTHB, ");
            sqlBuilder.AppendLine("     av.TADocumentID as TADocumentID, ");
            sqlBuilder.AppendLine("     av.AdvanceID as AdvanceID, av.MainCurrencyAmount as AdvanceMainCurrencyAmount ");
            sqlBuilder.AppendLine(" FROM Document a ");
            sqlBuilder.AppendLine("     INNER JOIN AvAdvanceDocument av ");
            sqlBuilder.AppendLine("     ON a.DocumentID = av.DocumentID ");
            sqlBuilder.AppendLine("     INNER JOIN WorkFlow b ");
            sqlBuilder.AppendLine("     ON b.DocumentID = av.DocumentID ");
            sqlBuilder.AppendLine("     INNER JOIN WorkFlowState c ");
            sqlBuilder.AppendLine("     ON c.WorkFlowTypeID = b.WorkFlowTypeID ");
            sqlBuilder.AppendLine("     AND b.CurrentState = c.WorkFlowStateID ");
            sqlBuilder.AppendLine("     INNER JOIN WorkFlowStateLang d ");
            sqlBuilder.AppendLine("     ON d.WorkFlowStateID = c.WorkFlowStateID ");
            sqlBuilder.AppendLine("     INNER JOIN SuUser e ");
            sqlBuilder.AppendLine("     ON a.RequesterID = e.UserID ");
            sqlBuilder.AppendLine("     INNER JOIN SuUser f ");
            sqlBuilder.AppendLine("     ON a.ReceiverID  = f.UserID ");
            sqlBuilder.AppendLine("     AND a.Active  = 1 ");
            sqlBuilder.AppendLine("     AND d.LanguageID  = :LanguageID ");
            sqlBuilder.AppendLine(" WHERE av.TADocumentID = :TADocumentID ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());

            parameterBuilder.AddParameterData("LanguageID", typeof(short), languageID);
            parameterBuilder.AddParameterData("TADocumentID", typeof(Int64), taDocumentID);
            parameterBuilder.FillParameters(query);

            query.AddScalar("DocumentID", NHibernateUtil.Int64);
            query.AddScalar("TADocumentID", NHibernateUtil.Int64);
            query.AddScalar("AdvanceID", NHibernateUtil.Int64);
            query.AddScalar("AdvanceNo", NHibernateUtil.String);
            query.AddScalar("AdvanceStatus", NHibernateUtil.String);
            query.AddScalar("Description", NHibernateUtil.String);
            query.AddScalar("RequesterID", NHibernateUtil.Int64);
            query.AddScalar("RequesterName", NHibernateUtil.String);
            query.AddScalar("ReceiverName", NHibernateUtil.String);
            query.AddScalar("DueDate", NHibernateUtil.DateTime);
            query.AddScalar("AmountTHB", NHibernateUtil.Decimal);
            query.AddScalar("AdvanceMainCurrencyAmount", NHibernateUtil.Double);

            IList<TADocumentObj> list =
               query.SetResultTransformer(Transformers.AliasToBean(typeof(TADocumentObj))).List<TADocumentObj>();
            return list;
        }
        #endregion public IList<TADocumentObj> FindAdvanceByTADocumentID(short languageID, long taDocumentID)

        #region public IList<SuUser> FindUserIDAdvanceByTADocumentID(long taDocumentID)
        public IList<SuUser> FindUserIDAdvanceByTADocumentID(long taDocumentID)
        {
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.Append(" SELECT ");
            sqlBuilder.Append("     b.UserID as Userid , ");
            sqlBuilder.Append("     b.UserName as UserName ");
            sqlBuilder.Append(" FROM TaDocumentTraveller a ");
            sqlBuilder.Append("     INNER JOIN SuUser b ");
            sqlBuilder.Append("     ON a.UserID = b.UserID "); 
            sqlBuilder.Append(" WHERE a.UserID NOT IN ( "); 
            sqlBuilder.Append("     SELECT d.RequesterID  ");
            sqlBuilder.Append("     FROM AvAdvanceDocument c ");
            sqlBuilder.Append("     INNER JOIN Document d ");  
            sqlBuilder.Append("     ON d.DocumentID = c.DocumentID ");
            sqlBuilder.Append("     WHERE c.TADocumentID = :TADocumentID ) ");
            sqlBuilder.Append(" AND a.TADocumentID = :TADocumentID");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());

            parameterBuilder.AddParameterData("TADocumentID", typeof(Int64), taDocumentID);
            parameterBuilder.FillParameters(query);

            query.AddScalar("Userid", NHibernateUtil.Int64);
            query.AddScalar("UserName", NHibernateUtil.String);

            IList<SuUser> list =
               query.SetResultTransformer(Transformers.AliasToBean(typeof(SuUser))).List<SuUser>();
            return list;
        }
        #endregion public IList<SuUser> FindUserIDAdvanceByTADocumentID(long taDocumentID)
    }
}
