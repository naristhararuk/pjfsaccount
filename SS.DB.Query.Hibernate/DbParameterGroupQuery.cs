using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.QueryDao;
using SS.Standard.Data.NHibernate.QueryCreator;
using SS.Standard.Utilities;
using SS.Standard.Security;
using SS.DB.DTO;
using SS.DB.Query;
using SS.DB.DTO.ValueObject;
using NHibernate;
using NHibernate.Transform;

namespace SS.DB.Query.Hibernate
{
    public class DbParameterGroupQuery : NHibernateQueryBase<DbParameterGroup, short>, IDbParameterGroupQuery
    {
        public IList<DbParameterGroup> GetParameterGroupByGroupNo(string groupNo)
        {
            string strSQL = " SELECT * FROM  DbParameterGroup WHERE GroupNo = :GroupNo ";

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(strSQL);
            query.SetInt16("GroupNo", Utilities.ParseShort(groupNo));
            query.AddEntity(typeof(DTO.DbParameter));

            IList<DbParameterGroup> dbParameterGroup = query.List<DbParameterGroup>();

            return dbParameterGroup;
        }

        # region public ISQLQuery FindByParameterGroupCriteria(DbParameterGroup parameterGroup, bool isCount, string sortExpression)
        public ISQLQuery FindByParameterGroupCriteria(DbParameterGroup parameterGroup, bool isCount, string sortExpression)
        {
            ISQLQuery query;
            StringBuilder sqlBuilder = new StringBuilder();

            if (!isCount)
            {
                sqlBuilder.Append(" SELECT ");
                sqlBuilder.Append("     DbParameterGroup.GroupNo    AS GroupNo,");
                sqlBuilder.Append("     DbParameterGroup.GroupName  AS GroupName,");
                sqlBuilder.Append("     DbParameterGroup.Comment    AS Comment,");
                sqlBuilder.Append("     DbParameterGroup.Active     AS Active ");
                sqlBuilder.Append(" FROM DbParameterGroup ");

                if (string.IsNullOrEmpty(sortExpression))
                    sqlBuilder.AppendLine("ORDER BY DbParameterGroup.GroupNo,DbParameterGroup.Comment,DbParameterGroup.Active");
                else
                    sqlBuilder.AppendLine(string.Format(" ORDER BY {0}", sortExpression));

                query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());

                QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
                queryParameterBuilder.FillParameters(query);
                query.AddScalar("GroupNo", NHibernateUtil.Int16);
                query.AddScalar("GroupName", NHibernateUtil.String);
                query.AddScalar("Comment", NHibernateUtil.String);
                query.AddScalar("Active", NHibernateUtil.Boolean);

                query.SetResultTransformer(Transformers.AliasToBean(typeof(DbParameterGroup)));
            }
            else
            {
                sqlBuilder.Append(" SELECT COUNT(*) AS Count FROM DbParameterGroup ");

                query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());

                query.AddScalar("Count", NHibernateUtil.Int32);
                query.UniqueResult();
            }
            
            return query;
        }
        #endregion public ISQLQuery FindByParameterGroupCriteria(DbParameterGroup parameterGroup, bool isCount, string sortExpression)

        #region public IList<DbParameterGroup> GetParameterGroupList(DbParameterGroup parameterGroup, int firstResult, int maxResult, string sortExpression)
        public IList<DbParameterGroup> GetParameterGroupList(DbParameterGroup parameterGroup, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<DbParameterGroup>(SsDbQueryProvider.DbParameterGroupQuery, "FindByParameterGroupCriteria", new object[] { parameterGroup, false, sortExpression }, firstResult, maxResult, sortExpression);
        }
        #endregion public IList<DbParameterGroup> GetParameterGroupList(DbParameterGroup parameterGroup, int firstResult, int maxResult, string sortExpression)

        #region public int CountByParameterGroupCriteria(DbParameterGroup parameterGroup)
        public int CountByParameterGroupCriteria(DbParameterGroup parameterGroup)
        {
            return NHibernateQueryHelper.CountByCriteria(SsDbQueryProvider.DbParameterGroupQuery, "FindByParameterGroupCriteria", new object[] { parameterGroup, true, string.Empty });
        }
        #endregion public int CountByParameterGroupCriteria(DbParameterGroup parameterGroup)
    }
}
