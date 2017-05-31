using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;

using SCG.DB.DTO;
using SCG.DB.DTO.ValueObject;
using SCG.DB.DAL;
using NHibernate;
using NHibernate.Expression;
using NHibernate.Transform;
using SS.Standard.Data.NHibernate.QueryCreator;

namespace SCG.DB.DAL.Hibernate
{
    public partial class DbCountryLangDao : NHibernateDaoBase<DbCountryLang, short>, IDbCountryLangDao
    {
       
        public DbCountryLangDao()
        {
        }

        #region public IList<SCG.DB.DTO.ValueObject.CountryLang> FindByCountryId(short CountryID)
        public IList<SCG.DB.DTO.ValueObject.CountryLang> FindByCountryId(short countryId)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" SELECT ");
            sql.Append("    DBCountryLang.ID , ");
            sql.Append("    DBCountryLang.CountryID , ");
            sql.Append("    DBCountryLang.CountryName , ");
            sql.Append("    DBCountryLang.Comment , ");
            sql.Append("    DBCountryLang.Active , ");
            sql.Append("    DbLanguage.LanguageId , ");
            sql.Append("    DbLanguage.LanguageName ");
            sql.Append(" FROM DbLanguage");
            sql.Append("    LEFT JOIN DBCountryLang    ON DbLanguage.LanguageId    = DbCountryLang.LanguageId AND DbCountryLang.CountryID = :CountryID ");
            sql.Append("    LEFT JOIN DbCountry        ON DbCountryLang.CountryID  = DbCountry.CountryID");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("CountryID", typeof(Int16), countryId);
            queryParameterBuilder.FillParameters(query);
            query.AddScalar("CountryID", NHibernateUtil.Int16);
            query.AddScalar("LanguageId", NHibernateUtil.Int16);
            query.AddScalar("CountryName", NHibernateUtil.String);
            query.AddScalar("Comment", NHibernateUtil.String);
            query.AddScalar("Active", NHibernateUtil.Boolean);
            query.AddScalar("LanguageName", NHibernateUtil.String);

            IList<SCG.DB.DTO.ValueObject.CountryLang> list =
                query.SetResultTransformer(Transformers.AliasToBean(typeof(SCG.DB.DTO.ValueObject.CountryLang))).List<SCG.DB.DTO.ValueObject.CountryLang>();
            return list;
        }
        #endregion  public IList<SCG.DB.DTO.ValueObject.CountryLang> FindByCountryId(short CountryID)

        #region public void DeleteAllCountryLang(short countryID)
        public void DeleteAllCountryLang(short countryId)
        {
            GetCurrentSession()
            .Delete(" FROM DbCountryLang country WHERE country.Country.CountryID = :CountryID ",
            new object[] { countryId },
            new NHibernate.Type.IType[] { NHibernateUtil.Int16 });
        }
        #endregion public void  DeleteAllCountryLang(short countryID)

        
      
    }
}
