using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.QueryDao;

using SS.SU.DTO;
using SS.SU.Query;

using NHibernate;
using NHibernate.Transform;

namespace SS.SU.Query.Hibernate
{
    public class SuDivisionQuery : NHibernateQueryBase<SuDivision, short>, ISuDivisionQuery
    {
        #region ISuDivisionQuery Members
        public IList<TranslatedListItem> GetTranslatedList(short languageID)
        {
            StringBuilder strQuery = new StringBuilder();
            strQuery.AppendLine(" SELECT div.DivisionID as Id, divLang.DivisionName as Text FROM SuDivision div ");
            strQuery.AppendLine(" INNER JOIN SuDivisionLang divLang ");
            strQuery.AppendLine(" ON div.DivisionID = divLang.DivisionID ");
            strQuery.AppendLine(" WHERE divLang.LanguageID = :LanguageID ");
            strQuery.AppendLine(" ORDER BY divLang.DivisionName ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());
            query.SetInt16("LanguageID", languageID == short.Parse("0") ? short.Parse("1") : languageID);
            query.AddScalar("Id", NHibernateUtil.Int16);
            query.AddScalar("Text", NHibernateUtil.String);

            IList<TranslatedListItem> list = query.SetResultTransformer(Transformers.AliasToBean(typeof(TranslatedListItem))).List<TranslatedListItem>();
            return list;
        }
        public IList<SuDivisionSearchResult> GetDivisionList(short languageID, short organizationId, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<SuDivisionSearchResult>(QueryProvider.SuDivisionQuery, "FindSuDivisionSearchResult", new object[] { languageID, organizationId, sortExpression, false }, firstResult, maxResult, sortExpression);
        }
        public ISQLQuery FindSuDivisionSearchResult(short languageID, short organizationId, string sortExpression, bool isCount)
        {
            StringBuilder strQuery = new StringBuilder();
            ISQLQuery query;
            if (isCount)
            {
                strQuery.AppendLine(" SELECT Count(*) as Count ");
                strQuery.AppendLine(" FROM SuDivision div ");
                strQuery.AppendLine(" INNER JOIN DbLanguage lang ");
                strQuery.AppendLine(" ON lang.LanguageID = :languageID ");
                strQuery.AppendLine(" INNER JOIN SuDivisionLang divLang ");
                strQuery.AppendLine(" ON divLang.DivisionID = div.DivisionID AND divLang.LanguageID = lang.LanguageID ");
                strQuery.AppendLine(" WHERE div.OrganizationID = :organizationID ");
                query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());
                query.SetInt16("languageID", languageID);
                query.SetInt16("organizationID", organizationId);
                query.AddScalar("Count", NHibernateUtil.Int32);
            }
            else
            {
                strQuery.AppendLine(" SELECT div.DivisionID as DivisionId, div.Comment as Comment, div.Active as Active ");
                strQuery.AppendLine(" , divLang.DivisionName as DivisionName, lang.LanguageID as LanguageId ");
                strQuery.AppendLine(" FROM SuDivision div ");
                strQuery.AppendLine(" INNER JOIN DbLanguage lang ");
                strQuery.AppendLine(" ON lang.LanguageID = :languageID ");
                strQuery.AppendLine(" INNER JOIN SuDivisionLang divLang ");
                strQuery.AppendLine(" ON divLang.DivisionID = div.DivisionID AND divLang.LanguageID = lang.LanguageID ");
                strQuery.AppendLine(" WHERE div.OrganizationID = :organizationID ");
                if (string.IsNullOrEmpty(sortExpression))
                {
                    strQuery.AppendLine(" order by DivisionId, DivisionName, Comment, div.Active");
                }
                else
                {
                    strQuery.AppendLine(string.Format(" order by {0}", sortExpression));
                }
                query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());
                query.SetInt16("languageID", languageID);
                query.SetInt16("organizationID", organizationId);
                query.AddScalar("DivisionId", NHibernateUtil.Int16);
                query.AddScalar("DivisionName", NHibernateUtil.String);
                query.AddScalar("LanguageId", NHibernateUtil.Int16);
                query.AddScalar("Comment", NHibernateUtil.String);
                query.AddScalar("Active", NHibernateUtil.Boolean);
                query.SetResultTransformer(Transformers.AliasToBean(typeof(SuDivisionSearchResult)));
            }
            return query;
        }
        public int GetCountDivisionList(short languageID, short organizationId)
        {
            return NHibernateQueryHelper.CountByCriteria(
                QueryProvider.SuDivisionQuery,
                "FindSuDivisionSearchResult",
                new object[] { languageID, organizationId, string.Empty, true });
        }
        #endregion

        #region TestMasterGrid
        public Object GetList()
        {
            return base.FindAll();
        }
        public int GetCount()
        {
            return base.FindAll().Count;
        }
        #endregion
    }
}
