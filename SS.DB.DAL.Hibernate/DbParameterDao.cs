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
    public partial class DbParameterDao : NHibernateDaoBase<DbParameter, short>, IDbParameterDao
    {
        public DbParameterDao()
        {
        }
        public IList<Parameter> FindByDbParameterCriteria(DbParameter parameter)
        {
            //StringBuilder sql = new StringBuilder();
            //sql.Append("Select  d.ParameterValue ");
            //sql.Append("from DbParameter as d ");
            //sql.Append("where d.ParameterValue = :ParameterValue ");

            //ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            //QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            //queryParameterBuilder.AddParameterData("ParameterValue", typeof(String), parameter.ParameterValue);
            //queryParameterBuilder.FillParameters(query);
            //query.AddScalar("ParameterValue", NHibernateUtil.String); ;

            StringBuilder sql = new StringBuilder();
            sql.Append("Select d.GroupNo, d.SeqNo ");
            sql.Append("from DbParameter as d ");
            sql.Append("where d.GroupNo = :GroupNo ");
            sql.Append("and d.SeqNo = :SeqNo ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("GroupNo", typeof(Int16), parameter.GroupNo.GroupNo);
            queryParameterBuilder.AddParameterData("SeqNo", typeof(Int16), parameter.SeqNo);
            queryParameterBuilder.FillParameters(query);
            query.AddScalar("GroupNo", NHibernateUtil.Int16);
            query.AddScalar("SeqNo", NHibernateUtil.Int16);

            IList<SS.DB.DTO.ValueObject.Parameter> list = query.SetResultTransformer(Transformers.AliasToBean(typeof(SS.DB.DTO.ValueObject.Parameter)))
                .List<SS.DB.DTO.ValueObject.Parameter>();

            return list;
        }
        #region public void DeleteAll(short Id)
        public void DeleteAll(short Id)
        {
            GetCurrentSession()
            .Delete(" FROM DbParameter as d WHERE d.ID = :Id ",
            new object[] { Id },
            new NHibernate.Type.IType[] { NHibernateUtil.Int16 });
        }
        #endregion public void DeleteAll(short Id)
    }
}