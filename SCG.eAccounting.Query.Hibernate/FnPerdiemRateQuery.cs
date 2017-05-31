using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using NHibernate.Expression;
using NHibernate.Transform;

using SCG.eAccounting.DTO;
using SCG.eAccounting.DTO.ValueObject;
using SCG.eAccounting.Query;
using SS.Standard.Data.NHibernate.Query;
using SS.Standard.Data.NHibernate.QueryDao;
using SS.Standard.Data.NHibernate.QueryCreator;

namespace SCG.eAccounting.Query.Hibernate
{
    public class FnPerdiemRateQuery : NHibernateQueryBase<FnPerdiemRate, long>, IFnPerdiemRateQuery
    {
        public IList<PerdiemRateValObj> FindByPerdiemProfileID(long perdiemProfileID, short languageID)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("  select f.PerdiemRateID as PerdiemRateID ,f.PerdiemProfileID as PerdiemProfileID , f.ZoneID as ZoneID , zl.ZoneName as ZoneName , f.PersonalLevel as PersonalLevel , f.ExtraPerdiemRate as ExtraPerdiemRate , f.OfficialPerdiemRate as OfficialPerdiemRate , f.InternationalStaffPerdiemRate as InternationalStaffPerdiemRate ,f.SCGStaffPerdiemRate as SCGStaffPerdiemRate , f.Active as Active ");
            sqlBuilder.Append(" from FnPerdiemRate f");
            sqlBuilder.Append(" inner join DbZone as z on f.ZoneID = z.ZoneID ");
            sqlBuilder.Append("left join DbZoneLang as zl on z.ZoneID = zl.ZoneID and zl.LanguageID =:LanguageID ");
            sqlBuilder.Append(" where f.PerdiemProfileID = :perdiemProfileID ");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("perdiemProfileID", typeof(long), perdiemProfileID);
            queryParameterBuilder.AddParameterData("LanguageID", typeof(Int16), languageID);
            queryParameterBuilder.FillParameters(query);
            query.AddScalar("PerdiemRateID", NHibernateUtil.Int64);
            query.AddScalar("PerdiemProfileID", NHibernateUtil.Int64);
            query.AddScalar("ZoneID", NHibernateUtil.Int16);
            query.AddScalar("ZoneName", NHibernateUtil.String);
            query.AddScalar("PersonalLevel", NHibernateUtil.String);
            query.AddScalar("ExtraPerdiemRate", NHibernateUtil.Double);
            query.AddScalar("OfficialPerdiemRate", NHibernateUtil.Double);
            query.AddScalar("InternationalStaffPerdiemRate", NHibernateUtil.Double);
            query.AddScalar("SCGStaffPerdiemRate", NHibernateUtil.Double);
            query.AddScalar("Active", NHibernateUtil.Boolean);
            return query.SetResultTransformer(Transformers.AliasToBean(typeof(PerdiemRateValObj))).List<PerdiemRateValObj>();
        }
        public IList<PerdiemRateValObj> GetPerdiemRateByRequesterID(long requesterId)
        {         
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" select  Pr.PerdiemRateID as PerdiemRateID,  Pr.PerdiemProfileID as PerdiemProfileID,  Pf.PerdiemProfileName as PerdiemProfileName,  Pr.ZoneID as ZoneID,  Pc.CompanyID as CompanyID, Pr.PersonalLevel as PersonalLevel, Pr.ExtraPerdiemRate as ExtraPerdiemRate, Pr.OfficialPerdiemRate  as OfficialPerdiemRate ");
            sqlBuilder.Append(" from    FnPerdiemRate Pr left join FnPerdiemProfile Pf on Pr.PerdiemProfileID = Pf.PerdiemProfileID ");
            sqlBuilder.Append("         left join FnPerdiemProfileCompany Pc on Pf.PerdiemProfileID = Pc.PerdiemProfileID ");
            sqlBuilder.Append("         left join SuUser Su on Pc.CompanyID = Su.CompanyID and Pr.PersonalLevel = Su.PersonalLevel ");
            sqlBuilder.Append("         where Su.UserID = :RequesterID ");
            sqlBuilder.Append("         order by Pr.ZoneID ");   

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("RequesterID", typeof(long), requesterId);
            queryParameterBuilder.FillParameters(query);
            query.AddScalar("PerdiemRateID", NHibernateUtil.Int64);
            query.AddScalar("PerdiemProfileID", NHibernateUtil.Int64);
            query.AddScalar("PerdiemProfileName", NHibernateUtil.String);
            query.AddScalar("ZoneID", NHibernateUtil.Int16);           
            query.AddScalar("CompanyID", NHibernateUtil.Int64);
            query.AddScalar("PersonalLevel", NHibernateUtil.String);
            query.AddScalar("ExtraPerdiemRate", NHibernateUtil.Double);
            query.AddScalar("OfficialPerdiemRate", NHibernateUtil.Double);
            
            query.SetResultTransformer(Transformers.AliasToBean(typeof(PerdiemRateValObj)));
            IList<PerdiemRateValObj> ilistquery = query.List<PerdiemRateValObj>();

            return ilistquery;
        }
        public IList<PerdiemRateValObj> GetPerdiemRateByRequesterIDForRepOffice(long requesterId,string personalGroup)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" select  Pr.PerdiemRateID as PerdiemRateID,  Pr.PerdiemProfileID as PerdiemProfileID,  Pf.PerdiemProfileName as PerdiemProfileName,  Pr.ZoneID as ZoneID,  Pc.CompanyID as CompanyID, Pr.PersonalLevel as PersonalLevel, Pr.ExtraPerdiemRate as ExtraPerdiemRate, Pr.OfficialPerdiemRate  as OfficialPerdiemRate ");
            if (personalGroup.StartsWith("A"))
            {
                sqlBuilder.Append(", Pr.InternationalStaffPerdiemRate as StuffPerdiemRate ");
            }
            else if (personalGroup.StartsWith("I"))
            {
                sqlBuilder.Append(", Pr.SCGStaffPerdiemRate as StuffPerdiemRate ");
            }
            sqlBuilder.Append(" from    FnPerdiemRate Pr left join FnPerdiemProfile Pf on Pr.PerdiemProfileID = Pf.PerdiemProfileID ");
            sqlBuilder.Append("         left join FnPerdiemProfileCompany Pc on Pf.PerdiemProfileID = Pc.PerdiemProfileID ");
            sqlBuilder.Append("         left join SuUser Su on Pc.CompanyID = Su.CompanyID and Pr.PersonalLevel = Su.PersonalLevel ");
            sqlBuilder.Append("         where Su.UserID = :RequesterID ");
            sqlBuilder.Append("         order by Pr.ZoneID ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("RequesterID", typeof(long), requesterId);
            queryParameterBuilder.FillParameters(query);
            query.AddScalar("PerdiemRateID", NHibernateUtil.Int64);
            query.AddScalar("PerdiemProfileID", NHibernateUtil.Int64);
            query.AddScalar("PerdiemProfileName", NHibernateUtil.String);
            query.AddScalar("ZoneID", NHibernateUtil.Int16);
            query.AddScalar("CompanyID", NHibernateUtil.Int64);
            query.AddScalar("PersonalLevel", NHibernateUtil.String);
            query.AddScalar("ExtraPerdiemRate", NHibernateUtil.Double);
            query.AddScalar("OfficialPerdiemRate", NHibernateUtil.Double);

            if (personalGroup.StartsWith("A") || personalGroup.StartsWith("I"))
            {
                query.AddScalar("StuffPerdiemRate", NHibernateUtil.Double);
            }
            

            query.SetResultTransformer(Transformers.AliasToBean(typeof(PerdiemRateValObj)));
            IList<PerdiemRateValObj> ilistquery = query.List<PerdiemRateValObj>();

            return ilistquery;
        }
    }
}
