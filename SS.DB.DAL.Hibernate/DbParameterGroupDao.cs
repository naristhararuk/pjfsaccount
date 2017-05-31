using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Transform;
using SS.Standard.Data.NHibernate.Dao;
using SS.DB.DTO;
using SS.DB.DAL;
using NHibernate;
using NHibernate.Expression;
using SS.Standard.Data.NHibernate.QueryCreator;
using SS.DB.DTO.ValueObject;

namespace SS.DB.DAL.Hibernate
{
    public partial class DbParameterGroupDao : NHibernateDaoBase<DbParameterGroup, short>, IDbParameterGroupDao
    {
        public DbParameterGroupDao()
        {
        }
        public IList<ParameterGroup> FindByDbParameterGroupCriteria(DbParameterGroup parameterGroup)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Select  d.ParameterValue ");
            sql.Append("from DbParameterGroup as d ");
            sql.Append("where d.GroupName = :GroupName ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("GroupName", typeof(String), parameterGroup.GroupName);
            queryParameterBuilder.FillParameters(query);
            query.AddScalar("GroupName", NHibernateUtil.String); ;

            IList<SS.DB.DTO.ValueObject.ParameterGroup> list = query.SetResultTransformer(Transformers.AliasToBean(typeof(SS.DB.DTO.ValueObject.ParameterGroup)))
                .List<SS.DB.DTO.ValueObject.ParameterGroup>();

            return list;
        }
        #region public void DeleteAll(short GroupNo)
        public void DeleteAll(short GroupNo)
        {
            GetCurrentSession()
            .Delete(" FROM DbParameterGroup as d WHERE d.GroupNo = :GroupNo ",
            new object[] { GroupNo },
            new NHibernate.Type.IType[] { NHibernateUtil.Int16 });
        }
        #endregion public void DeleteAll(short GroupNo)
    }
}
