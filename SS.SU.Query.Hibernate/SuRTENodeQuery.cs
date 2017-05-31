using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.QueryDao;
using SS.Standard.Data.NHibernate.QueryCreator;

using SS.SU.DTO;
using SS.SU.DTO.ValueObject;
using SS.SU.Query;

using NHibernate;
using NHibernate.Transform;
using NHibernate.Expression;

using SS.Standard.Security;

namespace SS.SU.Query.Hibernate
{
    public class SuRTENodeQuery : NHibernateQueryBase<SuRTENode, short>, ISuRTENodeQuery
    {
        public ISQLQuery FindSuRTENodeByLanguageId(short languageId, string sortExpression, bool isCount)
        {
            StringBuilder strQuery = new StringBuilder();
            if (isCount)
            {
                strQuery.AppendLine(" SELECT Count(n.NodeId) as Count ");
                strQuery.AppendLine(" From SuRTENode n ");
                strQuery.AppendLine(" INNER JOIN DbLanguage lang ON ");
                strQuery.AppendLine(" lang.LanguageID = :LanguageID ");
            }
            else
            {
                strQuery.AppendLine(" SELECT n.NodeId as Nodeid, n.NodeHeaderId as NodeHeaderid, n.NodeOrderNo as NodeOrderNo, ");
                strQuery.AppendLine(" n.NodeType as NodeType, n.ImagePath as ImagePath, n.Comment as Comment, n.Active as Active");

                strQuery.AppendLine(" From SuRTENode n ");
                strQuery.AppendLine(" INNER JOIN DbLanguage lang ON ");
                strQuery.AppendLine(" lang.LanguageID = :LanguageID ");

                if (!string.IsNullOrEmpty(sortExpression))
                {
                    strQuery.AppendLine(String.Format(" ORDER BY {0} ", sortExpression));
                }
                else
                {
                    strQuery.AppendLine(" ORDER BY n.NodeId, n.NodeHeaderId, n.NodeOrderNo, n.NodeType, n.Comment, n.Active");
                }

            }

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());
            query.SetInt16("LanguageID", languageId);

            if (isCount)
            {
                query.AddScalar("Count", NHibernateUtil.Int32);
            }
            else
            {
                query.AddScalar("Nodeid", NHibernateUtil.Int16);
                query.AddScalar("NodeHeaderid", NHibernateUtil.Int16);
                query.AddScalar("NodeOrderNo", NHibernateUtil.Int16);
                query.AddScalar("NodeType", NHibernateUtil.String);
                query.AddScalar("Comment", NHibernateUtil.String);
                query.AddScalar("Active", NHibernateUtil.Boolean);
                query.AddScalar("ImagePath", NHibernateUtil.String);

                query.SetResultTransformer(Transformers.AliasToBean(typeof(SuRTENode)));
            }
            return query;
        }

        public int GetCountList(short languageId)
        {
            return NHibernateQueryHelper.CountByCriteria(
                QueryProvider.SuRTENodeQuery,
                "FindSuRTENodeByLanguageId",
                new object[] { languageId, string.Empty, true });
        }
        public IList<SuRTENode> GetSuRTENodeList(short languageId, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<SuRTENode>(
                QueryProvider.SuRTENodeQuery,
                "FindSuRTENodeByLanguageId",
                new object[] { languageId, sortExpression, false },
                firstResult, maxResult, sortExpression);
        }
    }
}
