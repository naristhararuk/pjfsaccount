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
    public class DbParameterQuery : NHibernateQueryBase<DbParameter, short>, IDbParameterQuery
    {
        private static string strGetParameterByGroupNo_SeqNo = "SELECT	*  FROM  DbParameter  WHERE GroupNo = :GroupNo  AND SeqNo = :SeqNo AND Active=1  ";
        
        #region IDbParameterQuery Members

        #region public string getParameterByGroupNo_SeqNo(string groupNo, string seqNo)
        public string getParameterByGroupNo_SeqNo(string groupNo, string seqNo)
        {
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(strGetParameterByGroupNo_SeqNo);
            query.SetInt16("GroupNo", Utilities.ParseShort(groupNo));
            query.SetInt16("SeqNo", Utilities.ParseShort(seqNo));
            query.AddEntity(typeof(DTO.DbParameter));
            IList<DbParameter> dbParameter = query.List<DbParameter>();
            if (dbParameter.Count > 0)
            {
                return dbParameter[0].ParameterValue;
            }
            else
            {
                return "";
            }
        }
        #endregion public string getParameterByGroupNo_SeqNo(string groupNo, string seqNo)        

        public IList<DbParameter> GetParameterByGroupNo(string groupNo)
        {
            string strSQL = " SELECT * FROM  DbParameter WHERE GroupNo = :GroupNo ";

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(strSQL);
            query.SetInt16("GroupNo", Utilities.ParseShort(groupNo));
            query.AddEntity(typeof(DTO.DbParameter));

            IList<DbParameter> dbParameter = query.List<DbParameter>();

            return dbParameter;
        }

        #region public ISQLQuery FindByParameterCriteria(short groupNo, bool isCount, string sortExpression)
        //use parameter in value object class.
        public ISQLQuery FindByParameterCriteria(short groupNo, bool isCount, string sortExpression)
        {
            ISQLQuery query; 
            StringBuilder sqlBuilder = new StringBuilder();            

            if (!isCount)
            {
                sqlBuilder.Append(" SELECT ");
                sqlBuilder.Append("     DbParameter.Id              AS Id,");
                sqlBuilder.Append("     DbParameter.GroupNo         AS GroupNo,");
                sqlBuilder.Append("     DbParameter.SeqNo           AS SeqNo,");
                sqlBuilder.Append("     DbParameter.ConfigurationName  AS ConfigurationName,");
                sqlBuilder.Append("     DbParameter.ParameterValue  AS ParameterValue,");
                sqlBuilder.Append("     DbParameter.Comment         AS Comment,");
                sqlBuilder.Append("     DbParameter.ParameterType   AS ParameterType,");
                sqlBuilder.Append("     DbParameter.Active          AS Active ");
                sqlBuilder.Append(" FROM DbParameterGroup ");
                sqlBuilder.Append("     INNER JOIN DbParameter ON ");
                sqlBuilder.Append("     DbParameterGroup.GroupNo = DbParameter.GroupNo ");
                sqlBuilder.Append(" WHERE DbParameter.GroupNo = :GroupNo ");

                if (string.IsNullOrEmpty(sortExpression))
                    sqlBuilder.AppendLine("ORDER BY DbParameter.GroupNo,DbParameter.SeqNo,DbParameter.ConfigurationName,DbParameter.ParameterValue,DbParameter.Comment,DbParameter.ParameterType");
                else
                    sqlBuilder.AppendLine(string.Format(" ORDER BY {0}", sortExpression));

                query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString()); 

                QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
                queryParameterBuilder.AddParameterData("GroupNo", typeof(Int16), groupNo);
                queryParameterBuilder.FillParameters(query);
                query.AddScalar("Id", NHibernateUtil.Int16);
                query.AddScalar("ConfigurationName", NHibernateUtil.String);
                query.AddScalar("ParameterValue", NHibernateUtil.String);
                query.AddScalar("Comment", NHibernateUtil.String);
                query.AddScalar("ParameterType", NHibernateUtil.String);

                query.SetResultTransformer(Transformers.AliasToBean(typeof(Parameter)));
            }
            else
            {
                sqlBuilder.Append(" SELECT COUNT(*) AS Count FROM DbParameter WHERE GroupNo = :GroupNo ");

                query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString()); 

                QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
                queryParameterBuilder.AddParameterData("GroupNo", typeof(Int16), groupNo);
                queryParameterBuilder.FillParameters(query);
                query.AddScalar("Count", NHibernateUtil.Int32);
                query.UniqueResult();
            }

            return query;
        }
        #endregion public ISQLQuery FindByParameterCriteria(short groupNo, bool isCount, string sortExpression)

        #region public IList<Parameter> GetParameterList(short groupNo, int firstResult, int maxResult, string sortExpression)
        //use parameter in value object class.
        public IList<Parameter> GetParameterList(short groupNo, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<Parameter>(SsDbQueryProvider.DbParameterQuery, "FindByParameterCriteria", new object[] { groupNo, false, sortExpression }, firstResult, maxResult, sortExpression);
        }
        #endregion public IList<Parameter> GetParameterList(short groupNo, int firstResult, int maxResult, string sortExpression)

        #region public int CountByParameterCriteria(short groupNo)
        public int CountByParameterCriteria(short groupNo)
        {
            return NHibernateQueryHelper.CountByCriteria(SsDbQueryProvider.DbParameterQuery, "FindByParameterCriteria", new object[] { groupNo, true, string.Empty });
        }
        #endregion public int CountByParameterCriteria(short groupNo)       

        public string GetParameterByName(string parameterName)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("select parametervalue from dbparameter where configurationname = :configurationname");
            QueryParameterBuilder param = new QueryParameterBuilder();
            param.AddParameterData("configurationname", typeof(string), parameterName);      
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            param.FillParameters(query);
            query.AddScalar("parametervalue", NHibernateUtil.String);
            if (query.List().Count > 0)
            {
                return query.UniqueResult<string>();

            }
            else
            {
                return "";
            }
       
        }

        public IList<HashBox> GetAllNameAndValueParameter()
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("select ConfigurationName AS 'Key',ParameterValue AS 'Value' from dbparameter");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.AddScalar("Key", NHibernateUtil.String);
            query.AddScalar("Value", NHibernateUtil.String);
            query.SetResultTransformer(Transformers.AliasToBean(typeof(HashBox)));
            return query.List<HashBox>();


        }
        #endregion IDbParameterQuery Members
    }
}
