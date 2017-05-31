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
    public class DbProvinceLangQuery : NHibernateQueryBase<DbProvinceLang, long>, IDbProvinceLangQuery
    {
        #region public IList<SS.DB.DTO.ValueObject.ProvinceLang> FindByProvinceId(short provinceId)
        public IList<SS.DB.DTO.ValueObject.ProvinceLang> FindByProvinceId(short provinceId)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" SELECT ");
            sql.Append("    DbProvinceLang.ID , ");
            sql.Append("    DbProvinceLang.ProvinceId , ");
            sql.Append("    DbProvinceLang.ProvinceName , ");
            sql.Append("    DbProvinceLang.Comment , ");
            sql.Append("    DbProvinceLang.Active , ");
            sql.Append("    DbLanguage.LanguageId , ");
            sql.Append("    DbLanguage.LanguageName ");
            sql.Append(" FROM DbLanguage");
            sql.Append("    LEFT JOIN DbProvinceLang ON DbLanguage.LanguageId = DbProvinceLang.LanguageId AND DbProvinceLang.ProvinceID = :ProvinceId ");
            sql.Append("    LEFT JOIN DbProvince     ON DbProvinceLang.ProvinceID = DbProvince.ProvinceID");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());

            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("ProvinceId", typeof(Int16), provinceId);
            queryParameterBuilder.FillParameters(query);

            query.AddScalar("ProvinceId", NHibernateUtil.Int16);
            query.AddScalar("LanguageId", NHibernateUtil.Int16);
            query.AddScalar("ProvinceName", NHibernateUtil.String);
            query.AddScalar("Comment", NHibernateUtil.String);
            query.AddScalar("Active", NHibernateUtil.Boolean);
            query.AddScalar("LanguageName", NHibernateUtil.String);

            IList<ProvinceLang> list =
                query.SetResultTransformer(Transformers.AliasToBean(typeof(ProvinceLang))).List<ProvinceLang>();
            return list;
        }
        #endregion public IList<SS.DB.DTO.ValueObject.ProvinceLang> FindByProvinceId(short provinceId)
    }
}
