using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.QueryDao;
using SCG.DB.DTO;
using NHibernate;
using NHibernate.Transform;
using SCG.DB.DTO.ValueObject;
using SS.Standard.Data.NHibernate.QueryCreator;

namespace SCG.DB.Query.Hibernate
{
    public class DbSapInstanceQuery : NHibernateQueryBase<DbSapInstance, string>, IDbSapInstanceQuery
    {
        public IList<DbSapInstance> FindALL()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT * From DbSapInstance ");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());

            query.AddScalar("Code", NHibernateUtil.String);
            query.AddScalar("AliasName", NHibernateUtil.String);
            query.AddScalar("SystemID", NHibernateUtil.String);
            query.AddScalar("Client", NHibernateUtil.String);
            query.AddScalar("UserName", NHibernateUtil.String);
            query.AddScalar("Password", NHibernateUtil.String);
            query.AddScalar("Language", NHibernateUtil.String);
            query.AddScalar("SystemNumber", NHibernateUtil.String);
            query.AddScalar("MsgServerHost", NHibernateUtil.String);
            query.AddScalar("LogonGroup", NHibernateUtil.String);
            query.AddScalar("UserCPIC", NHibernateUtil.String);
            query.AddScalar("DocTypeExpPostingDM", NHibernateUtil.String);
            query.AddScalar("DocTypeExpRmtPostingDM", NHibernateUtil.String);
            query.AddScalar("DocTypeExpPostingFR", NHibernateUtil.String);
            query.AddScalar("DocTypeExpRmtPostingFR", NHibernateUtil.String);
            query.AddScalar("DocTypeExpICPostingFR", NHibernateUtil.String);
            query.AddScalar("DocTypeAdvancePostingDM", NHibernateUtil.String);
            query.AddScalar("DocTypeAdvancePostingFR", NHibernateUtil.String);
            query.AddScalar("DocTypeRmtPosting", NHibernateUtil.String);
            query.AddScalar("DocTypeFixedAdvancePosting", NHibernateUtil.String);
            query.AddScalar("DocTypeFixedAdvanceReturnPosting", NHibernateUtil.String);
            query.AddScalar("UpdBy", NHibernateUtil.String);
            query.AddScalar("UpdDate", NHibernateUtil.String);
            query.AddScalar("CreBy", NHibernateUtil.String);
            query.AddScalar("CreDate", NHibernateUtil.String);
            query.AddScalar("UpdPgm", NHibernateUtil.String);
            query.AddScalar("RowVersion", NHibernateUtil.String);

            return query.SetResultTransformer(Transformers.AliasToBean(typeof(DbSapInstance))).List<DbSapInstance>();
        }

        public IList<SapInstanceData> GetSapInstanceList()
        {
            StringBuilder sqlBuider = new StringBuilder();
            sqlBuider.Append("select sap.Code, sap.AliasName from DbSapInstance sap");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuider.ToString());
            query.AddScalar("Code", NHibernateUtil.String)
                .AddScalar("AliasName", NHibernateUtil.String);
            query.SetResultTransformer(Transformers.AliasToBean(typeof(SapInstanceData)));
            return query.List<SapInstanceData>();
        }
        public ISQLQuery FindSapInstanceByCriteria(SapInstanceCriteria criteria, string sortExpression, bool isCount)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            QueryParameterBuilder paramBuilder = new QueryParameterBuilder();
            if (!isCount)
            {
                sqlBuilder.Append(" select Code ,AliasName,SystemID,Client,UserName,Password,Language,SystemNumber,MsgServerHost,LogonGroup,UserCPIC,DocTypeExpPostingDM,DocTypeExpRmtPostingDM,DocTypeExpPostingFR,DocTypeExpRmtPostingFR,DocTypeExpICPostingFR,DocTypeAdvancePostingDM,DocTypeAdvancePostingFR,DocTypeRmtPosting,UpdBy, UpdDate, CreBy,CreDate,UpdPgm,RowVersion, DocTypeFixedAdvance as DocTypeFixedAdvancePosting,DocTypeFixedAdvanceReturn as DocTypeFixedAdvanceReturnPosting  ");
            }
            else
            {
                sqlBuilder.Append(" select count(*) as Count ");
            }

            sqlBuilder.Append("  from DbSapInstance where 1=1  ");

            if (!string.IsNullOrEmpty(criteria.AliasName))
            {
                sqlBuilder.Append(" and AliasName like :aliasname ");
                paramBuilder.AddParameterData("aliasname", typeof(string), criteria.AliasName);
            }
            if (!string.IsNullOrEmpty(criteria.MsgServerHost))
            {
                sqlBuilder.Append(" and MsgServerHost like :msgserverhost ");
                paramBuilder.AddParameterData("msgserverhost", typeof(string), criteria.MsgServerHost);
            }
            if (!string.IsNullOrEmpty(sortExpression))
            {

                sqlBuilder.Append(String.Format(" order by {0} ", sortExpression));
            }

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            paramBuilder.FillParameters(query);
            if (isCount)
            {
                query.AddScalar("Count", NHibernateUtil.Int32);
            }
            else
            {
                query.AddScalar("Code", NHibernateUtil.String);
                query.AddScalar("AliasName", NHibernateUtil.String);
                query.AddScalar("SystemID", NHibernateUtil.String);
                query.AddScalar("Client", NHibernateUtil.String);
                query.AddScalar("UserName", NHibernateUtil.String);
                query.AddScalar("Password", NHibernateUtil.String);
                query.AddScalar("Language", NHibernateUtil.String);
                query.AddScalar("SystemNumber", NHibernateUtil.String);
                query.AddScalar("MsgServerHost", NHibernateUtil.String);
                query.AddScalar("LogonGroup", NHibernateUtil.String);
                query.AddScalar("UserCPIC", NHibernateUtil.String);
                query.AddScalar("DocTypeExpPostingDM", NHibernateUtil.String);
                query.AddScalar("DocTypeExpRmtPostingDM", NHibernateUtil.String);
                query.AddScalar("DocTypeExpPostingFR", NHibernateUtil.String);
                query.AddScalar("DocTypeExpRmtPostingFR", NHibernateUtil.String);
                query.AddScalar("DocTypeExpICPostingFR", NHibernateUtil.String);
                query.AddScalar("DocTypeAdvancePostingDM", NHibernateUtil.String);
                query.AddScalar("DocTypeAdvancePostingFR", NHibernateUtil.String);
                query.AddScalar("DocTypeRmtPosting", NHibernateUtil.String);
                query.AddScalar("DocTypeFixedAdvancePosting", NHibernateUtil.String);
                query.AddScalar("DocTypeFixedAdvanceReturnPosting", NHibernateUtil.String);
                query.SetResultTransformer(Transformers.AliasToBean(typeof(DbSapInstance)));
            }
            return query;
        }
        public int CountSapInstance(SapInstanceCriteria criteria)
        {
            return NHibernateQueryHelper.CountByCriteria(ScgDbQueryProvider.DbSapInstanceQuery, "FindSapInstanceByCriteria", new object[] { criteria, string.Empty, true });
        }
        public IList<DbSapInstance> GetSapInstanceListByCriteria(SapInstanceCriteria criteria, int startRow, int pageSize, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<DbSapInstance>(ScgDbQueryProvider.DbSapInstanceQuery, "FindSapInstanceByCriteria", new object[] { criteria, sortExpression, false }, startRow, pageSize, sortExpression);
        }
    }
}

