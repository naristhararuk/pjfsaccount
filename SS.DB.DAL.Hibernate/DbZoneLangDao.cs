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
    public partial class DbZoneLangDao : NHibernateDaoBase<DbZoneLang, long>, IDbZoneLangDao
    {

        public IList<DbZoneResult> FindZoneLangByZoneId(short zoneId)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Select  zl.ID as Id,zl.ID as ZoneLangID,zl.ZoneName as ZoneName, zl.Comment as Comment, zl.Active as Active,l.LanguageID as LanguageID, l.LanguageName as LanguageName ");
            sql.Append("from DbLanguage as l ");
            sql.Append("left join DbZoneLang as zl on l.LanguageId = zl.LanguageId and zl.ZoneId = :ZoneId ");
            //sql.Append("left join Suprogram as p on pl.ProgramId = p.ProgramId");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("ZoneId", typeof(Int16), zoneId);
            queryParameterBuilder.FillParameters(query);
            query.AddScalar("ZoneLangID", NHibernateUtil.Int16);
            query.AddScalar("LanguageID", NHibernateUtil.Int16);
            query.AddScalar("ZoneName", NHibernateUtil.String);
            query.AddScalar("Comment", NHibernateUtil.String);
            query.AddScalar("Active", NHibernateUtil.Boolean);
            query.AddScalar("LanguageName", NHibernateUtil.String);

            IList<DbZoneResult> list = query.SetResultTransformer(Transformers.AliasToBean(typeof(DbZoneResult)))
                .List<DbZoneResult>();
            return list;
        }

        public void DeleteZoneLangByID(short zoneId)
        {
            GetCurrentSession()
            .Delete("from DbZoneLang zl where zl.Zone.Zoneid = :zoneId "
            , new object[] { zoneId }
            , new NHibernate.Type.IType[] { NHibernateUtil.Int16 });
        }     
        
    }
}
