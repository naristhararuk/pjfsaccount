using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using NHibernate.Transform;

using SS.Standard.Data.NHibernate.QueryDao;

using SS.SU.DTO;
using SS.SU.DTO.ValueObject;
using SS.SU.Query;
using SS.Standard.Data.NHibernate.QueryCreator;

namespace SS.SU.Query.Hibernate
{
    public class SuGlobalTranslateQuery : NHibernateQueryBase<SuGlobalTranslate, long>, ISuGlobalTranslateQuery
    {
        #region ISuGlobalTranslateQuery Members

        public GlobalTranslate ResolveMessage(string translateSymbol, string languageCode)
        {
            StringBuilder strQuery = new StringBuilder();
            strQuery.AppendLine(" SELECT	TranslateWord  ");
            strQuery.AppendLine(" FROM	SuGlobalTranslate g ");
            strQuery.AppendLine(" INNER JOIN SuGlobalTranslateLang gl ");
            strQuery.AppendLine(" ON	g.TranslateID = gl.TranslateID ");
            strQuery.AppendLine(" INNER JOIN DbLanguage l ");
            strQuery.AppendLine(" ON	gl.LanguageID = l.LanguageID ");
            strQuery.AppendLine(" WHERE	l.LanguageCode = :LanguageCode ");
            strQuery.AppendLine(" AND	g.ProgramCode is null ");
            strQuery.AppendLine(" AND	g.TranslateControl is null ");
            strQuery.AppendLine(" AND	g.TranslateSymbol = :TranslateSymbol ");
            strQuery.AppendLine(" AND	g.Active = 1 ");
            strQuery.AppendLine(" AND	gl.Active = 1 ");
            strQuery.AppendLine(" AND	l.Active = 1 ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());
            query.SetString("LanguageCode", languageCode);
            query.SetString("TranslateSymbol", translateSymbol);
            query.AddScalar("TranslateWord", NHibernateUtil.String);
            GlobalTranslate globalTranslate = (GlobalTranslate)query.SetResultTransformer(Transformers.AliasToBean(typeof(GlobalTranslate))).UniqueResult();
            return globalTranslate;
        }

        public GlobalTranslate ResolveMessage(string programCode, string translateSymbol, string languageCode)
        {
            StringBuilder strQuery = new StringBuilder();
            strQuery.AppendLine(" SELECT	TranslateWord  ");
            strQuery.AppendLine(" FROM	SuGlobalTranslate g ");
            strQuery.AppendLine(" INNER JOIN SuGlobalTranslateLang gl ");
            strQuery.AppendLine(" ON	g.TranslateID = gl.TranslateID ");
            strQuery.AppendLine(" INNER JOIN DbLanguage l ");
            strQuery.AppendLine(" ON	gl.LanguageID = l.LanguageID ");
            strQuery.AppendLine(" WHERE	l.LanguageCode = :LanguageCode ");
            strQuery.AppendLine(" AND	g.TranslateSymbol = :TranslateSymbol ");
            strQuery.AppendLine(" AND	g.ProgramCode = :ProgramCode ");
            strQuery.AppendLine(" AND	g.TranslateControl is null ");
            strQuery.AppendLine(" AND	g.Active = 1 ");
            strQuery.AppendLine(" AND	gl.Active = 1 ");
            strQuery.AppendLine(" AND	l.Active = 1 ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());
            query.SetString("LanguageCode", languageCode);
            query.SetString("TranslateSymbol", translateSymbol);
            query.SetString("ProgramCode", programCode);
            query.AddScalar("TranslateWord", NHibernateUtil.String);
            GlobalTranslate globalTranslate = (GlobalTranslate)query.SetResultTransformer(Transformers.AliasToBean(typeof(GlobalTranslate))).UniqueResult();
            return globalTranslate;
        }

        public IList<GlobalTranslate> LoadProgramResources(string programCode, string languageCode)
        {
            StringBuilder strQuery = new StringBuilder();
            strQuery.AppendLine(" SELECT	TranslateControl , TranslateWord  ");
            strQuery.AppendLine(" FROM	SuGlobalTranslate g ");
            strQuery.AppendLine(" INNER JOIN SuGlobalTranslateLang gl ");
            strQuery.AppendLine(" ON	g.TranslateID = gl.TranslateID ");
            strQuery.AppendLine(" INNER JOIN DbLanguage l ");
            strQuery.AppendLine(" ON	gl.LanguageID = l.LanguageID ");
            strQuery.AppendLine(" WHERE	l.LanguageCode = :LanguageCode ");
            strQuery.AppendLine(" AND	g.ProgramCode = :ProgramCode ");
            strQuery.AppendLine(" AND	g.Active = 1 ");
            strQuery.AppendLine(" AND	gl.Active = 1 ");
            strQuery.AppendLine(" AND	l.Active = 1 ");
            strQuery.AppendLine(" AND	isnull(g.TranslateControl,'') <> '' ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());
            query.SetString("LanguageCode", languageCode);
            query.SetString("ProgramCode", programCode);
            query.AddScalar("TranslateControl", NHibernateUtil.String);
            query.AddScalar("TranslateWord", NHibernateUtil.String);
            IList<GlobalTranslate> globalTranslates = query.SetResultTransformer(Transformers.AliasToBean(typeof(GlobalTranslate))).List<GlobalTranslate>();
            return globalTranslates;
        }

        public IList<TranslateSearchResult> GetTranslatedList(SuGlobalTranslate criteria, short languageID, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<TranslateSearchResult>(
                QueryProvider.SuGlobalTranslateQuery,
                "FindGlobalTranslateSearchResult",
                new object[] { criteria, languageID, sortExpression, false },
                firstResult, maxResult, sortExpression);
        }
        public ISQLQuery FindGlobalTranslateSearchResult(SuGlobalTranslate criteria, short languageId, string sortExpression, bool isCount)
        {
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            StringBuilder sqlBuilder = new StringBuilder();
            ISQLQuery query;
            if (isCount)
            {
                sqlBuilder.Append("select count(t.TranslateId) as Count ");
                sqlBuilder.Append(" from SuGlobalTranslate t ");
                //sqlBuilder.Append(" left join SuProgram p on p.ProgramCode = t.ProgramCode ");
                //sqlBuilder.Append(" left join SuProgramLang pl on pl.ProgramId = p.ProgramId ");
                //sqlBuilder.Append(" and pl.LanguageID = :LanguageID");
                sqlBuilder.Append(" where 1=1 ");

                if (!string.IsNullOrEmpty(criteria.ProgramCode))
                {
                    sqlBuilder.Append(" and t.ProgramCode like :ProgramCode ");
                    parameterBuilder.AddParameterData("ProgramCode", typeof(string), String.Format("%{0}%", criteria.ProgramCode));
                }
                if (!string.IsNullOrEmpty(criteria.TranslateSymbol))
                {
                    sqlBuilder.Append(" and t.TranslateSymbol like :TranslateSymbol ");
                    parameterBuilder.AddParameterData("TranslateSymbol", typeof(string), String.Format("%{0}%", criteria.TranslateSymbol));
                }
                if (!string.IsNullOrEmpty(criteria.TranslateControl))
                {
                    sqlBuilder.Append(" and t.TranslateControl like :TranslateControl ");
                    parameterBuilder.AddParameterData("TranslateControl", typeof(string), String.Format("%{0}%", criteria.TranslateControl));
                }

                query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
                parameterBuilder.FillParameters(query);
                //query.SetInt16("LanguageID", languageId);
                query.AddScalar("Count", NHibernateUtil.Int32);
            }
            else
            {

                sqlBuilder.Append("select t.TranslateId as TranslateId, t.TranslateSymbol as TranslateSymbol, t.Comment as Comment, t.TranslateControl as TranslateControl, t.ProgramCode as ProgramCode ");

                sqlBuilder.Append(" from SuGlobalTranslate t ");
                //sqlBuilder.Append(" left join SuProgram p on p.ProgramCode = t.ProgramCode ");
                //sqlBuilder.Append(" left join SuProgramLang pl on pl.ProgramId = p.ProgramId ");
                //sqlBuilder.Append(" and pl.LanguageID = :LanguageID ");
                sqlBuilder.Append(" where 1=1 ");

                if (!string.IsNullOrEmpty(criteria.ProgramCode))
                {
                    sqlBuilder.Append(" and t.ProgramCode like :ProgramCode ");
                    parameterBuilder.AddParameterData("ProgramCode", typeof(string), String.Format("%{0}%", criteria.ProgramCode));
                }
                if (!string.IsNullOrEmpty(criteria.TranslateSymbol))
                {
                    sqlBuilder.Append(" and t.TranslateSymbol like :TranslateSymbol ");
                    parameterBuilder.AddParameterData("TranslateSymbol", typeof(string), String.Format("%{0}%", criteria.TranslateSymbol));
                }
                if (!string.IsNullOrEmpty(criteria.TranslateControl))
                {
                    sqlBuilder.Append(" and t.TranslateControl like :TranslateControl ");
                    parameterBuilder.AddParameterData("TranslateControl", typeof(string), String.Format("%{0}%", criteria.TranslateControl));
                }

                if (string.IsNullOrEmpty(sortExpression))
                {
                    sqlBuilder.Append(" order by TranslateId, ProgramCode, TranslateSymbol, TranslateControl, Comment ");
                }
                else
                {
                    sqlBuilder.Append(string.Format(" order by {0} ", sortExpression));
                }
                query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
                parameterBuilder.FillParameters(query);
                //query.SetInt16("LanguageID", languageId);
                query.AddScalar("TranslateId", NHibernateUtil.Int64);
                query.AddScalar("TranslateSymbol", NHibernateUtil.String);
                query.AddScalar("TranslateControl", NHibernateUtil.String);
                query.AddScalar("Comment", NHibernateUtil.String);
                query.AddScalar("ProgramCode", NHibernateUtil.String);

                query.SetResultTransformer(Transformers.AliasToBean(typeof(TranslateSearchResult)));
            }
            return query;
        }

        public int GetCountTranslatedList(SuGlobalTranslate criteria, short languageId)
        {
            return NHibernateQueryHelper.CountByCriteria(
                QueryProvider.SuGlobalTranslateQuery,
                "FindGlobalTranslateSearchResult",
                new object[] { criteria, languageId, string.Empty, true });
        }

        public string GetResolveControl(string programCode, string translateControl, string languageCode)
        {
            StringBuilder strQuery = new StringBuilder();
            strQuery.AppendLine(" SELECT	TranslateWord  ");
            strQuery.AppendLine(" FROM	SuGlobalTranslate g ");
            strQuery.AppendLine(" INNER JOIN SuGlobalTranslateLang gl ");
            strQuery.AppendLine(" ON	g.TranslateID = gl.TranslateID ");
            strQuery.AppendLine(" INNER JOIN DbLanguage l ");
            strQuery.AppendLine(" ON	gl.LanguageID = l.LanguageID ");
            strQuery.AppendLine(" WHERE	l.LanguageCode = :LanguageCode ");
            strQuery.AppendLine(" AND	g.TranslateControl = :TranslateControl ");
            strQuery.AppendLine(" AND	g.ProgramCode = :ProgramCode ");
            strQuery.AppendLine(" AND	g.Active = 1 ");
            strQuery.AppendLine(" AND	gl.Active = 1 ");
            strQuery.AppendLine(" AND	l.Active = 1 ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());
            query.SetString("LanguageCode", languageCode);
            query.SetString("TranslateControl", translateControl);
            query.SetString("ProgramCode", programCode);
            query.AddScalar("TranslateWord", NHibernateUtil.String);


            return query.UniqueResult<string>();
        }

        #endregion
    }
}
