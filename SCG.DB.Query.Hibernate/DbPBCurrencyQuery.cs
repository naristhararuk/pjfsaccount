using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.QueryDao;
using SCG.DB.DTO;
using SS.Standard.Data.NHibernate.QueryCreator;
using NHibernate;
using NHibernate.Transform;

namespace SCG.DB.Query.Hibernate
{
    public class DbPBCurrencyQuery : NHibernateQueryBase<DbPBCurrency, long>, IDbPBCurrencyQuery
    {
        public IList<DbPBCurrency> FindPBCurrencyByPBID(long pbID)
        {
            ISQLQuery query = GetCurrentSession().CreateSQLQuery("select ID,PBID,CurrencyID,UpdBy,UpdDate,CreBy,CreDate,UpdPgm from DbPBCurrency where PBID = :pbID");
            query.SetInt64("pbID", pbID);
            query.AddScalar("ID", NHibernateUtil.Int64);
            query.AddScalar("PBID", NHibernateUtil.Int64);
            query.AddScalar("CurrencyID", NHibernateUtil.Int16);
            query.AddScalar("UpdBy",NHibernateUtil.Int64);
            query.AddScalar("UpdDate",NHibernateUtil.DateTime);
            query.AddScalar("CreBy",NHibernateUtil.Int64);
            query.AddScalar("CreDate",NHibernateUtil.DateTime);
            query.AddScalar("UpdPgm",NHibernateUtil.String);
            query.SetResultTransformer(Transformers.AliasToBean(typeof(DbPBCurrency)));

            return  query.List<DbPBCurrency>();;
        }

        public IList<DbPBCurrency> FindPBLocalCurrencyByPBID(long pbID, string currencyType)
        {
            string sql = @"select PBCurrency.ID,PBCurrency.PBID,Currency.CurrencyID,Currency.Symbol,PBCurrency.UpdBy,PBCurrency.UpdDate,PBCurrency.CreBy,PBCurrency.CreDate,PBCurrency.UpdPgm from DbPBCurrency as PBCurrency inner join DbCurrency as Currency on PBCurrency.CurrencyID = Currency.CurrencyID 
                where PBCurrency.PBID = :pbID  order by PBCurrency.ID";
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql);
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

    }
}

