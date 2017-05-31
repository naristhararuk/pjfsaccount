using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.DB.DTO;
using SS.Standard.Data.NHibernate.QueryDao;
using SS.Standard.Data.NHibernate.QueryCreator;
using SS.DB.DTO;
using SS.DB.DTO.ValueObject;

using NHibernate;
using NHibernate.Transform;
using SCG.DB.DTO.ValueObject;
using NHibernate.Expression;



namespace SCG.DB.Query.Hibernate
{
    public class DBPBLangQuery : NHibernateQueryBase<DbpbLang ,long>,IDbPbLangQuery
    {
        public IList<VOPB> FindPBLangByPBID(long pbID)
        {
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.Append(" SELECT    DbLanguage.LanguageID as LanguageID , DbLanguage.LanguageName as LanguageName ,");
            sqlBuilder.Append(" DbPBLang.PBLangID as PBLangID , DbPBLang.LanguageID AS PBLanguageID , ");
            sqlBuilder.Append(" DbPBLang.PBID as PBID , DbPBLang.Description as Description , DbPBLang.Comment as Comment , ");
            sqlBuilder.Append(" DbPBLang.Active as Active ");
            sqlBuilder.Append(" FROM  DbLanguage LEFT JOIN  ");
            sqlBuilder.Append(" DbPBLang ON DbLanguage.LanguageID = DbPBLang.LanguageID and DbPBLang.PBID= :CID ");
            queryParameterBuilder.AddParameterData("CID", typeof(long), pbID);




            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            queryParameterBuilder.FillParameters(query);

            query.AddScalar("LanguageID", NHibernateUtil.Int16)
                 .AddScalar("LanguageName", NHibernateUtil.String)
                 .AddScalar("PBLangID", NHibernateUtil.Int64)
                 .AddScalar("LanguageID", NHibernateUtil.Int16)
                 .AddScalar("PBLanguageID", NHibernateUtil.Int16)
                 .AddScalar("PBID", NHibernateUtil.Int64)
                 .AddScalar("Description", NHibernateUtil.String)
                 .AddScalar("Comment", NHibernateUtil.String)
                 .AddScalar("Active", NHibernateUtil.Boolean);

            query.SetResultTransformer(Transformers.AliasToBean(typeof(VOPB)));


            return query.SetResultTransformer(Transformers.AliasToBean(typeof(VOPB))).List<VOPB>(); ;

        }
        public DbpbLang GetPBLangByPBIDAndLanguageID(long pbID, short languageID)
        {
            ICriteria criteria = GetCurrentSession().CreateCriteria(typeof(DbpbLang), "pl");
            criteria.Add(Expression.And(Expression.Eq("pl.PBID.Pbid", pbID), Expression.Eq("pl.LanguageID.Languageid", languageID)));
            return criteria.UniqueResult<DbpbLang>();
        }

        #region IDbPbLangQuery Members

        public IList<TranslatedListItem> GetPBLangByCriteria(DbpbLang pbLang,short RoleID)
        {
            StringBuilder sqlbuilder = new StringBuilder();
            sqlbuilder.Append(" select pb.pbid as strID,pbl.description as strSymbol,pb.pbCode as strCode");
            sqlbuilder.Append(" FROM dbpb pb inner join dbpblang pbl");
            sqlbuilder.Append(" on pb.pbid = pbl.pbid");
            sqlbuilder.Append(" where 1=1 ");
            sqlbuilder.Append(" and pb.pbid = pbl.pbid ");
            
            sqlbuilder.Append(" and pbl.languageID = :languageID ");
            sqlbuilder.Append(" and pbl.active = 1 ");
            if (RoleID != 0)
            {
                sqlbuilder.Append("and pb.pbid not in (select pbid from surolepb where roleid = :RoleID)");
            }


            //sqlbuilder.AppendLine(" ORDER BY strID");

            sqlbuilder.AppendLine(" ORDER BY strCode");

            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            parameterBuilder.AddParameterData("languageID", typeof(short), pbLang.LanguageID.Languageid);
            if (RoleID != 0)
            {
                parameterBuilder.AddParameterData("RoleID", typeof(short), RoleID);
            }
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlbuilder.ToString());
            parameterBuilder.FillParameters(query);
            query.AddScalar("strID", NHibernateUtil.String)
                .AddScalar("strSymbol", NHibernateUtil.String)
                .AddScalar("strCode", NHibernateUtil.String);


            return query.SetResultTransformer(Transformers.AliasToBean(typeof(TranslatedListItem))).List<TranslatedListItem>();
       
           
        }


        public IList<TranslatedListItem> GetPBLangByCriteria(short languageID, long UserID, IList<short> userRole)
        {
            StringBuilder sqlbuilder = new StringBuilder();

            sqlbuilder.AppendLine(" select pb.pbid as strID,pbl.description as strSymbol,pb.pbCode as strCode");
            sqlbuilder.AppendLine(" from dbpb pb inner join dbpblang pbl");
            sqlbuilder.AppendLine(" on pb.pbid = pbl.pbid");
            sqlbuilder.AppendLine(" and pbl.languageID = :languageID ");
            sqlbuilder.AppendLine(" and pbl.active = 1 ");

            if (UserID != 0)
            {
                sqlbuilder.AppendLine(" inner join SuRolePB srp ");
                sqlbuilder.AppendLine(" on pb.pbid = srp.pbid ");
                sqlbuilder.AppendLine(" inner join SuUserRole sur "); 
                sqlbuilder.AppendLine(" on sur.RoleID = srp.RoleID "); 
                sqlbuilder.AppendLine(" inner join SuUser su "); 
                sqlbuilder.AppendLine(" on su.UserID = sur.UserID "); 
                sqlbuilder.AppendLine(" inner join SuRole sr ");
                sqlbuilder.AppendLine(" on sr.RoleID = sur.RoleID ");
                sqlbuilder.AppendLine(" where su.Userid = :UserID ");
                sqlbuilder.AppendLine(" AND sr.RoleID in (:UserRoleList) ");
                sqlbuilder.AppendLine(" Group By pb.pbid ,pbl.description,pb.pbCode ");
            }

            sqlbuilder.AppendLine(" ORDER BY strID");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlbuilder.ToString());

            if (UserID != 0)
            {
                query.SetInt64("UserID", UserID);
                
                query.SetParameterList("UserRoleList", userRole);
            }
            query.SetInt16("languageID", languageID);

            query.AddScalar("strID", NHibernateUtil.String);
            query.AddScalar("strSymbol", NHibernateUtil.String);
            query.AddScalar("strCode", NHibernateUtil.String);

            return query.SetResultTransformer(Transformers.AliasToBean(typeof(TranslatedListItem))).List<TranslatedListItem>();
        }
        #endregion

    }
}
