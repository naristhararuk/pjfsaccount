using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.QueryDao;
using SCG.eAccounting.DTO;
using NHibernate;
using SS.Standard.Data.NHibernate.QueryCreator;
using NHibernate.Transform;
using SCG.eAccounting.DTO.ValueObject;
using NHibernate.Expression;

namespace SCG.eAccounting.Query.Hibernate
{
    public class FnPerdiemProfileCountryQuery : NHibernateQueryBase<FnPerdiemProfileCountry, long>, IFnPerdiemProfileCountryQuery
    {
        public IList<PerdiemProfileCountryValObj> FindByPerdiemProfileID(long perdiemProfileID, short languageID)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("  select ppc.ID as ID ,ppc.PerdiemProfileID as PerdiemProfileID , ppc.ZoneID as ZoneID , zl.ZoneName as ZoneName , ppc.CountryID as CountryID , cl.CountryName as CountryName ");
            sqlBuilder.Append(" from FnPerdiemProfileCountry ppc");
            sqlBuilder.Append(" inner join DbZone as z on ppc.ZoneID = z.ZoneID ");
            sqlBuilder.Append("left join DbZoneLang as zl on z.ZoneID = zl.ZoneID and zl.LanguageID =:LanguageID ");
            sqlBuilder.Append(" inner join DbCountry as c on ppc.CountryID = c.CountryID ");
            sqlBuilder.Append("left join DbCountryLang as cl on c.CountryID = cl.CountryID and cl.LanguageID =:LanguageID ");
            sqlBuilder.Append(" where ppc.PerdiemProfileID = :perdiemProfileID ");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("perdiemProfileID", typeof(long), perdiemProfileID);
            queryParameterBuilder.AddParameterData("LanguageID", typeof(Int16), languageID);
            queryParameterBuilder.FillParameters(query);
            query.AddScalar("ID", NHibernateUtil.Int64);
            query.AddScalar("PerdiemProfileID", NHibernateUtil.Int64);
            query.AddScalar("ZoneID", NHibernateUtil.Int16);
            query.AddScalar("ZoneName", NHibernateUtil.String);
            query.AddScalar("CountryID", NHibernateUtil.Int16);
            query.AddScalar("CountryName", NHibernateUtil.String);
            return query.SetResultTransformer(Transformers.AliasToBean(typeof(PerdiemProfileCountryValObj))).List<PerdiemProfileCountryValObj>();
        }
        public FnPerdiemProfileCountry FindByID(long ID)
        {
            ICriteria criteria = GetCurrentSession().CreateCriteria(typeof(FnPerdiemProfileCountry), "ppc");
            criteria.Add(Expression.Eq("ppc.ID", ID));
            return criteria.UniqueResult<FnPerdiemProfileCountry>();
        }
        public short FindCountryZoneID(short countryID, long requesterId)
        {
              
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("  select ppcou.ZoneID as ZoneID ");
            sqlBuilder.Append(" from FnPerdiemProfile pp ");
            sqlBuilder.Append(" inner join FnPerdiemProfileCompany ppc on pp.PerdiemProfileID = ppc.PerdiemProfileID ");
            sqlBuilder.Append(" inner join FnPerdiemProfileCountry ppcou on ppc.PerdiemProfileID = ppcou.PerdiemProfileID ");
            sqlBuilder.Append(" inner join SuUser u on u.CompanyID = ppc.CompanyID ");
            sqlBuilder.Append(" inner join DbCountry c on ppcou.CountryID = c.CountryID ");
            sqlBuilder.Append("  WHERE ppcou.CountryID = :CountryID and u.UserID = :UserID ");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("CountryID", typeof(Int16), countryID);
            queryParameterBuilder.AddParameterData("UserID", typeof(Int64), requesterId);
            queryParameterBuilder.FillParameters(query);
            query.AddScalar("ZoneID", NHibernateUtil.Int16);
            return query.UniqueResult<Int16>();
        }
    }
}
