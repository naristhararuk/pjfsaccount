using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.QueryDao;
using SCG.DB.DTO;
using NHibernate;
using SCG.DB.DTO.ValueObject;
using SS.Standard.Data.NHibernate.QueryCreator;
using NHibernate.Transform;
using SS.Standard.Security;

namespace SCG.DB.Query.Hibernate
{
    public class DbPbRateQuery : NHibernateQueryBase<DbPbRate, long>, IDbPbRateQuery
    {
        //public IUserAccount UserAccount { get; set; }
        public ISQLQuery FindPbByCriteria(VOPB pb, string sortExpression, bool isCount)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();

            if (!isCount)
            {
                sqlBuilder.Append(" select distinct DbPB.PBID as Pbid, DbPB.PBCode as PBCode, DbPBLang.Description as Description ");
            }
            else
            {
                sqlBuilder.Append(" select count(DbPB.PBID) as Count ");
            }
            sqlBuilder.Append(" from DbPB  ");
            sqlBuilder.Append(" left join DbPBLang  on DbPB.PBID=DbPBLang.PBID and DbPBLang.LanguageID=:langId ");
            sqlBuilder.Append(" inner join SuRolePB on DbPb.PBID=SuRolePB.PBID");
            sqlBuilder.Append(" inner join SuUserRole on SuRolePB.RoleID = SuUserRole.RoleID ");
            if (pb.UserID.HasValue)
            {
                sqlBuilder.Append(" and SuUserRole.UserID =:userId ");
                queryParameterBuilder.AddParameterData("userId", typeof(long), pb.UserID);
            }
            queryParameterBuilder.AddParameterData("langId", typeof(short), pb.LanguageID);
            


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
                sqlBuilder.Append(String.Format(" where 1=1 and DbPB.Active=1 and DbPB.RepOffice=1 {0} ", whereClauseBuilder.ToString()));
            }
            else
            {
                sqlBuilder.Append(" where DbPB.Active=1 and DbPB.RepOffice=1 ");
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
                    sqlBuilder.Append("order by DbPB.PBCode,DbPB.PBID,DbPBLang.Description ");
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
                     .AddScalar("Description", NHibernateUtil.String);

                query.SetResultTransformer(Transformers.AliasToBean(typeof(VOPB)));
            }

            return query;
        }
        public IList<VOPB> GetPbList(VOPB pb, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<VOPB>(ScgDbQueryProvider.DbPbRateQuery, "FindPbByCriteria", new object[] { pb, sortExpression, false }, firstResult, maxResult, sortExpression);

        }
        public IList<VOPB> GetPbList(VOPB pb, string sortExpression)
        {
            return NHibernateQueryHelper.FindByCriteria<VOPB>(ScgDbQueryProvider.DbPbRateQuery, "FindPbByCriteria", new object[] { pb, sortExpression, false }, sortExpression);

        }
        public int CountPbByCriteria(VOPB pb)
        {
            return NHibernateQueryHelper.CountByCriteria(ScgDbQueryProvider.DbPbRateQuery, "FindPbByCriteria", new object[] { pb, string.Empty, true });
        }


        public ISQLQuery FindPbInfoByCriteria(long pbId, string sortExpression, bool isCount)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();

            if (!isCount)
            {
                sqlBuilder.Append(" select DbPbRate.ID as ID, DbPbRate.PBID as PBID,DbPbRate.EffectiveDate as EffectiveDate,mainCurrency.CurrencyID as MainCurrencyID, mainCurrency.Symbol as FromCurrencySymbol, ");
                sqlBuilder.Append(" DbPbRate.FromAmount as FromAmount,currency.CurrencyID as CurrencyID,  currency.Symbol as ToCurrencySymbol,DbPbRate.ToAmount as ToAmount , ");
                sqlBuilder.Append(" DbPbRate.ExchangeRate as ExchangeRate,DbPbRate.UpdateBy as UpdateBy,DbPbRate.UpdDate as UpdDate ");
            }
            else
            {
                sqlBuilder.Append(" select count(DbPbRate.ID) as Count ");
            }
            sqlBuilder.Append(" from DbPbRate  ");
            sqlBuilder.Append(" inner join DbCurrency mainCurrency on DbPbRate.MainCurrencyID=mainCurrency.CurrencyID");
            sqlBuilder.Append(" inner join DbCurrency currency on DbPbRate.CurrencyID=currency.CurrencyID");
            sqlBuilder.Append(" where DbPbRate.PBID =:pbId");
            queryParameterBuilder.AddParameterData("pbId", typeof(long), pbId);


            #region Order By
            if (!isCount)
            {
                if (!string.IsNullOrEmpty(sortExpression))
                {
                    sqlBuilder.Append(String.Format(" order by {0} ", sortExpression));
                }
                else
                {
                    sqlBuilder.Append(" order by DbPbRate.EffectiveDate DESC,DbPbRate.UpdDate DESC ");
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


                query.AddScalar("ID", NHibernateUtil.Int64)
                     .AddScalar("PBID", NHibernateUtil.Int64)
                     .AddScalar("EffectiveDate", NHibernateUtil.DateTime)
                     .AddScalar("MainCurrencyID", NHibernateUtil.Int16)
                     .AddScalar("FromCurrencySymbol", NHibernateUtil.String)
                     .AddScalar("FromAmount", NHibernateUtil.Double)
                     .AddScalar("CurrencyID", NHibernateUtil.Int16)
                     .AddScalar("ToCurrencySymbol", NHibernateUtil.String)
                     .AddScalar("ToAmount", NHibernateUtil.Double)
                     .AddScalar("ExchangeRate", NHibernateUtil.Double)
                     .AddScalar("UpdateBy", NHibernateUtil.String)
                     .AddScalar("UpdDate", NHibernateUtil.DateTime);


                query.SetResultTransformer(Transformers.AliasToBean(typeof(VOPbRate)));
            }

            return query;
        }
        public IList<VOPbRate> GetPbInfoList(long pbId, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<VOPbRate>(ScgDbQueryProvider.DbPbRateQuery, "FindPbInfoByCriteria", new object[] { pbId, sortExpression, false }, firstResult, maxResult, sortExpression);

        }
        public int CountPbInfoByCriteria(long pbId)
        {
            return NHibernateQueryHelper.CountByCriteria(ScgDbQueryProvider.DbPbRateQuery, "FindPbInfoByCriteria", new object[] { pbId, string.Empty, true });
        }

        public IList<DbPBCurrency> FindPBLocalCurrencyByPBID(long pbID, string currencyType)
        {
            ISQLQuery query = GetCurrentSession().CreateSQLQuery("select PBCurrency.ID,PBCurrency.PBID,Currency.CurrencyID,Currency.Symbol,PBCurrency.UpdBy,PBCurrency.UpdDate,PBCurrency.CreBy,PBCurrency.CreDate,PBCurrency.UpdPgm from DbPBCurrency as PBCurrency inner join DbCurrency as Currency on PBCurrency.CurrencyID = Currency.CurrencyID where PBCurrency.PBID = :pbID");
            query.SetInt64("pbID", pbID);
            query.AddScalar("ID", NHibernateUtil.Int64);
            query.AddScalar("PBID", NHibernateUtil.Int64);
            query.AddScalar("CurrencyID", NHibernateUtil.Int16);
            query.AddScalar("Symbol", NHibernateUtil.String);            
            query.AddScalar("UpdBy", NHibernateUtil.Int64);
            query.AddScalar("UpdDate", NHibernateUtil.DateTime);
            query.AddScalar("CreBy", NHibernateUtil.Int64);
            query.AddScalar("CreDate", NHibernateUtil.DateTime);
            query.AddScalar("UpdPgm", NHibernateUtil.String);
            query.SetResultTransformer(Transformers.AliasToBean(typeof(DbPBCurrency)));

            return query.List<DbPBCurrency>(); ;
        }
        
        public double GetExchangeRate(long pbID, short mainCurrencyId, short toCurrencyID)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" select ExchangeRate from DbPBRate where ID = (select MAX(ID) from DbPBRate ");
            sqlBuilder.Append(" where EffectiveDate = (Select MAX(EffectiveDate) from DbPBRate where PBID = :pbID and MainCurrencyID = :mainCurrencyID and CurrencyID = :toCurrencyID) ");
            sqlBuilder.Append(" and PBID = :pbID and MainCurrencyID = :mainCurrencyID and CurrencyID = :toCurrencyID) ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("pbID", typeof(long), pbID);
            queryParameterBuilder.AddParameterData("mainCurrencyID", typeof(short), mainCurrencyId);
            queryParameterBuilder.AddParameterData("toCurrencyID", typeof(short), toCurrencyID);
            queryParameterBuilder.FillParameters(query);

            query.AddScalar("ExchangeRate", NHibernateUtil.Double);

            object exRate = query.UniqueResult();
            if (exRate != null)
            {
                return (double)exRate;
            }
            return 0;
           
        }

    }
}
