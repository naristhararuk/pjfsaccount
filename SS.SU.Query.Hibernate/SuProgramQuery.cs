using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.QueryDao;
using SS.Standard.Data.NHibernate.QueryCreator;

using SS.SU.DTO;
using SS.SU.DTO.ValueObject;
using SS.SU.Query;

using NHibernate;
using NHibernate.Transform;

namespace SS.SU.Query.Hibernate
{
    public class SuProgramQuery : NHibernateQueryBase<SuProgram, short>, ISuProgramQuery
    {
        #region ISuProgramQuery Members
        public IList<SuProgramSearchResult> GetTranslatedList(SuProgramSearchResult criteria, short languageID, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<SuProgramSearchResult>(QueryProvider.SuProgramQuery, "FindSuProgramSearchResult", new object[] { criteria, languageID }, firstResult, maxResult, sortExpression);
        }
        public ISQLQuery FindSuProgramSearchResult(SuProgramSearchResult programSearchResult, short languageID)
        {
            StringBuilder strQuery = new StringBuilder();
            strQuery.AppendLine(" SELECT p.ProgramID as ProgramID, p.ProgramCode as ProgramCode, p.ProgramPath as ProgramPath ");
            strQuery.AppendLine(" , p.Comment as Comment, p.Active as Active ");
            strQuery.AppendLine(" , pl.ProgramName as ProgramName, l.LanguageID as LanguageId ");
            strQuery.AppendLine(" FROM SuProgram p ");
            strQuery.AppendLine(" INNER JOIN DbLanguage l ");
            strQuery.AppendLine(" ON l.LanguageID = :languageID ");
            strQuery.AppendLine(" INNER JOIN SuProgramLang pl ");
            strQuery.AppendLine(" ON pl.ProgramID = p.ProgramID AND pl.LanguageID = l.LanguageID ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());
            query.SetInt16("languageID", languageID);
            query.AddScalar("ProgramID", NHibernateUtil.Int16);
            query.AddScalar("ProgramCode", NHibernateUtil.String);
            query.AddScalar("ProgramPath", NHibernateUtil.String);
            query.AddScalar("ProgramName", NHibernateUtil.String);
            query.AddScalar("LanguageId", NHibernateUtil.Int16);
            query.AddScalar("Comment", NHibernateUtil.String);
            query.AddScalar("Active", NHibernateUtil.Boolean);
            query.SetResultTransformer(Transformers.AliasToBean(typeof(SuProgramSearchResult)));

            return query;
        }
        public int FindCountSuProgramSearchResult(SuProgramSearchResult programSearchResult, short languageID)
        {
            StringBuilder strQuery = new StringBuilder();
            strQuery.AppendLine(" SELECT Count(*) as Count ");
            strQuery.AppendLine(" FROM SuProgram p ");
            strQuery.AppendLine(" INNER JOIN DbLanguage l ");
            strQuery.AppendLine(" ON l.LanguageID = :languageID ");
            strQuery.AppendLine(" INNER JOIN SuProgramLang pl ");
            strQuery.AppendLine(" ON pl.ProgramID = p.ProgramID AND pl.LanguageID = l.LanguageID ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());
            query.SetInt16("languageID", languageID);
            query.AddScalar("Count", NHibernateUtil.Int32);

            return Convert.ToInt32(query.UniqueResult());
        }
        #region Use in tranlation
        public IList<SuProgramSearchResult> FindSuProgramByLanguageId(short languageID)
        {
            StringBuilder strQuery = new StringBuilder();
            strQuery.AppendLine(" SELECT p.ProgramID as ProgramID, p.ProgramCode as ProgramCode, p.ProgramPath as ProgramPath ");
            strQuery.AppendLine(" , p.Comment as Comment, p.Active as Active ");
            strQuery.AppendLine(" , pl.ProgramsName as ProgramName, l.LanguageID as LanguageId ");
            strQuery.AppendLine(" FROM SuProgram p ");
            strQuery.AppendLine(" INNER JOIN SuProgramLang pl ");
            strQuery.AppendLine(" ON pl.ProgramID = p.ProgramID");
            strQuery.AppendLine(" INNER JOIN DbLanguage l ");
            strQuery.AppendLine(" ON l.LanguageID = pl.languageID and l.LanguageID = :LanguageID");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());
            query.SetInt16("LanguageID", languageID);
            query.AddScalar("ProgramID", NHibernateUtil.Int16);
            query.AddScalar("ProgramCode", NHibernateUtil.String);
            query.AddScalar("ProgramPath", NHibernateUtil.String);
            query.AddScalar("ProgramName", NHibernateUtil.String);
            query.AddScalar("LanguageId", NHibernateUtil.Int16);
            query.AddScalar("Comment", NHibernateUtil.String);
            query.AddScalar("Active", NHibernateUtil.Boolean);
            query.SetResultTransformer(Transformers.AliasToBean(typeof(SuProgramSearchResult)));

            return query.List<SuProgramSearchResult>();
        }
        #endregion

        public ISQLQuery FindByProgramCriteria(SuProgram program,string sortExpression, bool isCount)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            if (!isCount)
            {
                sqlBuilder.Append("select p.Programid as Programid, p.ProgramCode as ProgramCode,p.Comment as Comment,p.Active as Active from SuProgram p ");
                if (string.IsNullOrEmpty(sortExpression))
                {
                    sqlBuilder.AppendLine("ORDER BY Programid,ProgramCode,Comment,Active");
                }
                else
                {
                    sqlBuilder.AppendLine(string.Format(" ORDER BY {0}", sortExpression));
                }
            }
            else
            {
                sqlBuilder.Append("select count(p.Programid) as ProgramCount from SuProgram p ");
            }

            ISQLQuery query ;

            if (!isCount)
            {
                query =GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
                query.AddScalar("Programid", NHibernateUtil.Int16)
                    .AddScalar("ProgramCode", NHibernateUtil.String)
                .AddScalar("Comment", NHibernateUtil.String)
                .AddScalar("Active", NHibernateUtil.Boolean);
                query.SetResultTransformer(Transformers.AliasToBean(typeof(SS.SU.DTO.SuProgram)));
            }
            else
            {
                query =GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
                query.AddScalar("ProgramCount", NHibernateUtil.Int32);
                query.UniqueResult();
            }
            return query;
        }
        public IList<SuProgram> GetProgramList(SuProgram program, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<SuProgram>(QueryProvider.SuProgramQuery, "FindByProgramCriteria", new object[] { program,sortExpression, false }, firstResult, maxResult, sortExpression);
        }
        public int CountBySuProgramCriteria(SuProgram programCriteria)
        {
            return NHibernateQueryHelper.CountByCriteria(QueryProvider.SuProgramQuery, "FindByProgramCriteria", new object[] { programCriteria,string.Empty, true });
        }
        public short FindProgramIDByProgramCode(string programCode)
        {
            StringBuilder sql = new StringBuilder();
            QueryParameterBuilder parameter = new QueryParameterBuilder();
            sql.Append("SELECT Programid ");
            sql.Append("FROM SuProgram ");
            sql.Append("WHERE ProgramCode = :programCode ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            parameter.AddParameterData("programCode",typeof(string), programCode);
            parameter.FillParameters(query);
            query.AddScalar("Programid", NHibernateUtil.Int16);

            query.SetResultTransformer(Transformers.AliasToBean(typeof(SuProgram)));
            IList<SuProgram> list = query.List<SuProgram>();
            if (list.Count > 0)
            {
                return list[0].Programid;
            }
            else
                return 0;
        }
       
        #endregion
    }
}
