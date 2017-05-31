using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using NHibernate.Transform;

using SS.Standard.Data.NHibernate.Query;
using SS.Standard.Data.NHibernate.QueryDao;
using SS.Standard.Data.NHibernate.QueryCreator;

using SCG.DB.DTO;
using SCG.DB.Query;
using SCG.DB.DTO.ValueObject;
using NHibernate.Expression;
using SS.Standard.Security;

namespace SCG.DB.Query.Hibernate
{
    public class DbPBQuery : NHibernateQueryBase<Dbpb, long>, IDbPBQuery
    {
        #region IFnPaymentTypeLangQuery Members
        /// <summary>
        /// query for bind ddl (PaymentType)
        /// </summary>
        /// <param name="groupStatus"></param>
        /// <param name="languageID"></param>
        /// <returns></returns>


        public IList<PaymentTypeListItem> GetPbListItem(long companyID, short languageID)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" SELECT PB.PBID as ID, PB.PBCode as Code , PBL.Description as Text");
            sqlBuilder.Append(" FROM DbPB as PB");
            sqlBuilder.Append(" LEFT JOIN DBPBLang as PBL on PB.PBID = PBL.PBID");
            sqlBuilder.Append(" WHERE PB.CompanyID = :companyID AND PBL.LanguageID =:languageID AND PB.Active = 1 AND PBL.Active = 1 ");

            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            parameterBuilder.AddParameterData("companyID", typeof(long), companyID);
            parameterBuilder.AddParameterData("languageID", typeof(string), languageID);

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            parameterBuilder.FillParameters(query);
            query.AddScalar("Code", NHibernateUtil.String)
                .AddScalar("Text", NHibernateUtil.String)
                .AddScalar("ID", NHibernateUtil.Int64);

            return query.SetResultTransformer(Transformers.AliasToBean(typeof(PaymentTypeListItem))).List<PaymentTypeListItem>();
        }

        #endregion
        public IUserAccount UserAccount { get; set; }
        public VOPB GetDescription(long pbID, int languageID)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" select DbPB.PBCode as PBCode, DbPBLang.Description as Description,DbPB.CompanyID as CompanyID ");
            sqlBuilder.Append(" from DbPBLang INNER JOIN ");
            sqlBuilder.Append(" DbPB on DbPBLang.PBID = DbPB.PBID ");
            sqlBuilder.Append(" WHERE DbPB.PBID = :PBID and ");
            sqlBuilder.Append(" DbPBLang.LanguageID = :LanguageID ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("PBID", typeof(long), pbID);
            queryParameterBuilder.AddParameterData("LanguageID", typeof(int), languageID);
            queryParameterBuilder.FillParameters(query);

            query.AddScalar("PBCode", NHibernateUtil.String);
            query.AddScalar("Description", NHibernateUtil.String);
            query.AddScalar("CompanyID", NHibernateUtil.Int64);
            query.SetResultTransformer(Transformers.AliasToBean(typeof(VOPB)));

            return query.UniqueResult<VOPB>();
        }

        public ISQLQuery FindPbByCriteria(VOPB pb, string sortExpression, bool isCount)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();

            if (!isCount)
            {
                sqlBuilder.Append(" select DbPB.PBID as Pbid, DbPB.PBCode as PBCode, DbPB.CompanyCode as CompanyCode, ");
                sqlBuilder.Append(" DbPB.Active as Active,  DbPB.PettyCashLimit as PettyCashLimit,DbPB.BlockPost as BlockPost , ");
                sqlBuilder.Append(" DbPBLang.Description as Description ");
            }
            else
            {
                sqlBuilder.Append(" select count(DbPB.PBID) as Count ");
            }
            sqlBuilder.Append(" from DbPB  ");
            sqlBuilder.Append(" left join DbPBLang  on DbPB.PBID=DbPBLang.PBID and DbPBLang.LanguageID=:langId ");
            queryParameterBuilder.AddParameterData("langId", typeof(short), UserAccount.CurrentLanguageID);


            #region WhereClause
            StringBuilder whereClauseBuilder = new StringBuilder();

            if (!string.IsNullOrEmpty(pb.Description))
            {
                whereClauseBuilder.Append(" and DbPBLang.Description Like :Description ");
                queryParameterBuilder.AddParameterData("Description", typeof(string), String.Format("%{0}%", pb.Description));
            }

            if (!string.IsNullOrEmpty(pb.PBCode))
            {
                whereClauseBuilder.Append(" and DbPB.PBCode Like :CompanyCode ");
                queryParameterBuilder.AddParameterData("CompanyCode", typeof(string), String.Format("%{0}%", pb.PBCode));
            }

            if (!string.IsNullOrEmpty(whereClauseBuilder.ToString()))
            {
                sqlBuilder.Append(String.Format(" where 1=1 {0} ", whereClauseBuilder.ToString()));
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
                    sqlBuilder.Append(" order by DbPB.PBCode,DbPB.CompanyCode,DbPBLang.Description,DbPB.PettyCashLimit,DbPB.BlockPost ");
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
                query.AddScalar("Pbid", NHibernateUtil.Int64)
                     .AddScalar("PBCode", NHibernateUtil.String)
                     .AddScalar("Description", NHibernateUtil.String)
                     .AddScalar("PettyCashLimit", NHibernateUtil.Double)
                     .AddScalar("BlockPost", NHibernateUtil.Boolean)
                     .AddScalar("CompanyCode", NHibernateUtil.String)
                     .AddScalar("Active", NHibernateUtil.Boolean);
                query.SetResultTransformer(Transformers.AliasToBean(typeof(VOPB)));
            }

            return query;
        }
        public IList<VOPB> GetPbList(VOPB pb, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<VOPB>(ScgDbQueryProvider.DbPBQuery, "FindPbByCriteria", new object[] { pb, sortExpression, false }, firstResult, maxResult, sortExpression);

        }
        public int CountPbByCriteria(VOPB pb)
        {
            return NHibernateQueryHelper.CountByCriteria(ScgDbQueryProvider.DbPBQuery, "FindPbByCriteria", new object[] { pb, string.Empty, true });
        }

        public bool IsRepOffice (long UserID)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" select isnull(DbPb.RepOffice,0) as RepOffice  from SuUser as suUser");
            sqlBuilder.Append(" LEFT JOIN DBLocation as location  on suUser.LocationID = location.LocationID");
            sqlBuilder.Append(" LEFT JOIN DbPB as DbPb on location.DefaultPbID = DbPb.PBID");
            sqlBuilder.Append(" WHERE suUser.UserID = :UserID ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("UserID", typeof(long), UserID);
            queryParameterBuilder.FillParameters(query);
            query.AddScalar("RepOffice", NHibernateUtil.Boolean);

            return query.UniqueResult<bool?>() ?? false;
        }


    }
}
