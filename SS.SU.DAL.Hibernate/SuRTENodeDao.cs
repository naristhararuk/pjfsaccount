using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;

using SS.SU.DTO;
using SS.SU.DTO.ValueObject;
using SS.SU.DAL;

using NHibernate;
using NHibernate.Expression;
using NHibernate.Transform;

namespace SS.SU.DAL.Hibernate
{
    public class SuRTENodeDao : NHibernateDaoBase<SuRTENode, short>, ISuRTENodeDao
    {
        public IList<SuRTENodeSearchResult> GetRTENodeList(short languageId, string nodetype)
        {
            StringBuilder strQuery = new StringBuilder();
            strQuery.AppendLine(" SELECT n.NodeId,n.NodeHeaderId,n.NodeOrderNo,c.languageId,n.NodeType,c.Header,c.Content,n.ImagePath");
            strQuery.AppendLine(" From SuRTENode n LEFT OUTER JOIN SuRTEContent c on c.nodeid = n.nodeid ");
            strQuery.AppendLine(" WHERE c.LanguageID = :LanguageID and n.nodetype = :NodeType ");
            strQuery.AppendLine(" AND n.Active = 'TRUE' and n.NodeHeaderID= 0");
            strQuery.AppendLine(" ORDER BY n.NodeHeaderId,n.NodeOrderNo");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());
            query.SetInt16("LanguageID", languageId);
            query.SetString("NodeType", nodetype);
            query.AddScalar("NodeId", NHibernateUtil.Int16);
            query.AddScalar("NodeHeaderId", NHibernateUtil.Int16);
            query.AddScalar("NodeOrderNo", NHibernateUtil.Int16);
            query.AddScalar("languageId", NHibernateUtil.Int16);
            query.AddScalar("NodeType", NHibernateUtil.String);
            query.AddScalar("Header", NHibernateUtil.String);
            query.AddScalar("Content", NHibernateUtil.String);
            query.AddScalar("ImagePath", NHibernateUtil.String);

            IList<SuRTENodeSearchResult> list = query.SetResultTransformer(Transformers.AliasToBean(typeof(SuRTENodeSearchResult))).List<SuRTENodeSearchResult>();

            return list;
        }
        public IList<SuRTENodeSearchResult> GetRTEContentList(short languageId, string nodetype, short nodeId)
        {
            StringBuilder strQuery = new StringBuilder();
            strQuery.AppendLine(" SELECT n.NodeId,n.NodeHeaderId,n.NodeOrderNo,c.languageId,n.NodeType,c.Header,c.Content,n.ImagePath");
            strQuery.AppendLine(" From SuRTENode n LEFT OUTER JOIN SuRTEContent c on c.nodeid = n.nodeid ");
            strQuery.AppendLine(" WHERE c.LanguageID = :LanguageID and n.nodetype = :NodeType ");
            strQuery.AppendLine(" AND n.Active = 'TRUE' and n.NodeHeaderID = :NodeId");
            strQuery.AppendLine(" ORDER BY n.NodeHeaderId,n.NodeOrderNo");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());
            query.SetInt16("LanguageID", languageId);
            query.SetString("NodeType", nodetype);
            query.SetInt16("NodeId", nodeId);
            query.AddScalar("NodeId", NHibernateUtil.Int16);
            query.AddScalar("NodeHeaderId", NHibernateUtil.Int16);
            query.AddScalar("NodeOrderNo", NHibernateUtil.Int16);
            query.AddScalar("languageId", NHibernateUtil.Int16);
            query.AddScalar("NodeType", NHibernateUtil.String);
            query.AddScalar("Header", NHibernateUtil.String);
            query.AddScalar("Content", NHibernateUtil.String);
            query.AddScalar("ImagePath", NHibernateUtil.String);

            IList<SuRTENodeSearchResult> list = query.SetResultTransformer(Transformers.AliasToBean(typeof(SuRTENodeSearchResult))).List<SuRTENodeSearchResult>();

            return list;
        }
        public IList<SuRTENodeSearchResult> GetRTEContent(short languageId, string nodetype, short nodeId)
        {
            StringBuilder strQuery = new StringBuilder();
            strQuery.AppendLine(" SELECT n.NodeId,n.NodeHeaderId,n.NodeOrderNo,c.languageId,n.NodeType,c.Header,c.Content,n.ImagePath");
            strQuery.AppendLine(" From SuRTENode n LEFT OUTER JOIN SuRTEContent c on c.nodeid = n.nodeid ");
            strQuery.AppendLine(" WHERE c.LanguageID = :LanguageID and n.nodetype = :NodeType ");
            strQuery.AppendLine(" AND n.Active = 'TRUE' and n.NodeId = :NodeId");
            strQuery.AppendLine(" ORDER BY n.NodeHeaderId,n.NodeOrderNo");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());
            query.SetInt16("LanguageID", languageId);
            query.SetString("NodeType", nodetype);
            query.SetInt16("NodeId", nodeId);
            query.AddScalar("NodeId", NHibernateUtil.Int16);
            query.AddScalar("NodeHeaderId", NHibernateUtil.Int16);
            query.AddScalar("NodeOrderNo", NHibernateUtil.Int16);
            query.AddScalar("languageId", NHibernateUtil.Int16);
            query.AddScalar("NodeType", NHibernateUtil.String);
            query.AddScalar("Header", NHibernateUtil.String);
            query.AddScalar("Content", NHibernateUtil.String);
            query.AddScalar("ImagePath", NHibernateUtil.String);

            IList<SuRTENodeSearchResult> list = query.SetResultTransformer(Transformers.AliasToBean(typeof(SuRTENodeSearchResult))).List<SuRTENodeSearchResult>();

            return list;
        }

        public SuRTENodeSearchResult GetWelcome(short languageId, string nodeType)
        {
            StringBuilder strQuery = new StringBuilder();
            strQuery.AppendLine(" SELECT n.NodeId,n.NodeHeaderId,n.NodeOrderNo,c.languageId,n.NodeType,c.Header,c.Content,n.ImagePath");
            strQuery.AppendLine(" From SuRTENode n ");
            strQuery.AppendLine("LEFT JOIN SuRTEContent c on c.nodeid = n.nodeid ");
            strQuery.AppendLine(" WHERE c.LanguageID = :LanguageID and n.nodetype = :NodeType ");
            strQuery.AppendLine(" AND n.Active = 'TRUE' ");
            strQuery.AppendLine(" ORDER BY n.NodeHeaderId,n.NodeOrderNo");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());
            query.SetInt16("LanguageID", languageId);
            query.SetString("NodeType", nodeType);
            query.AddScalar("NodeId", NHibernateUtil.Int16);
            query.AddScalar("NodeHeaderId", NHibernateUtil.Int16);
            query.AddScalar("NodeOrderNo", NHibernateUtil.Int16);
            query.AddScalar("languageId", NHibernateUtil.Int16);
            query.AddScalar("NodeType", NHibernateUtil.String);
            query.AddScalar("Header", NHibernateUtil.String);
            query.AddScalar("Content", NHibernateUtil.String);
            query.AddScalar("ImagePath", NHibernateUtil.String);
            
            SuRTENodeSearchResult node = query.SetResultTransformer(Transformers.AliasToBean(typeof(SuRTENodeSearchResult))).UniqueResult<SuRTENodeSearchResult>();
            return  node;//query.SetResultTransformer(Transformers.AliasToBean(typeof(SuRTENodeSearchResult))).UniqueResult<SuRTENodeSearchResult>();
        }
    }
}
