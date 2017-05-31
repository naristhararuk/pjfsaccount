using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate.Transform;
using SS.Standard.Data.NHibernate.Dao;

using SS.SU.DTO;
using SS.SU.DTO.ValueObject;
using SS.SU.DAL;
using NHibernate;
using SS.Standard.Data.NHibernate.QueryCreator;
using NHibernate.Expression;

namespace SS.SU.DAL.Hibernate
{
    public partial class SuProgramLangDao : NHibernateDaoBase<SuProgramLang, long>, ISuProgramLangDao
    {
        public IList<ProgramLang> FindByProgramId(short programId)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Select  pl.ID as ProgramLangId, pl.ProgramId as ProgramId, pl.ProgramsName as ProgramName, pl.Comment as Comment, pl.Active as Active,l.LanguageId as LanguageId, l.LanguageName as LanguageName ");
            sql.Append("from DbLanguage as l ");
            sql.Append("left join SuProgramLang as pl on l.LanguageId = pl.LanguageId and pl.programId = :ProgramId ");
            sql.Append("left join Suprogram as p on pl.ProgramId = p.ProgramId");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("ProgramId", typeof(Int16), programId);
            queryParameterBuilder.FillParameters(query);
            query.AddScalar("ProgramLangId", NHibernateUtil.Int64);
            query.AddScalar("ProgramId", NHibernateUtil.Int16);
            query.AddScalar("LanguageId", NHibernateUtil.Int16);
            query.AddScalar("ProgramName", NHibernateUtil.String);
            query.AddScalar("Comment", NHibernateUtil.String);
            query.AddScalar("Active", NHibernateUtil.Boolean);
            query.AddScalar("LanguageName", NHibernateUtil.String);

            IList<SS.SU.DTO.ValueObject.ProgramLang> list = query.SetResultTransformer(Transformers.AliasToBean(typeof(SS.SU.DTO.ValueObject.ProgramLang)))
                .List<SS.SU.DTO.ValueObject.ProgramLang>();
            return list;
        }
        public ICriteria FindBySuProgramLangCriteria(SuProgramLang programlang)
        {
            ICriteria criteria = GetCurrentSession().CreateCriteria(typeof(SuProgramLang), "pl");
            criteria.Add(Expression.Eq("pl.Language.Languageid", programlang.Language.Languageid));
            criteria.Add(Expression.Eq("pl.ProgramsName", programlang.ProgramsName));
            return criteria;
        }
        public ISQLQuery FindBySuProgramLangQuery(SuProgramLang programLang, short roleId, short languageId, bool isCount)
        {
            StringBuilder sql = new StringBuilder();
            if (!isCount)
            {
                sql.Append(" select pl.ID as ProgramLangId, pl.ProgramId as ProgramId, pl.ProgramsName as ProgramName ,pl.Comment as Comment from SuProgramLang as pl");
            }
            else
            {
                sql.Append(" select count(*) as Count from SuProgramLang as pl");
            }
            sql.Append(" where pl.Languageid = :LanguageId");
            sql.Append(" and pl.ProgramsName Like :ProgramsName");
            sql.Append(" and pl.Programid not in");
            sql.Append(" (select pr.Programid from SuProgramRole as pr where pr.RoleID = :RoleId)");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            query.SetInt16("RoleId", roleId)
                .SetInt16("LanguageId", languageId)
                .SetString("ProgramsName", "%" + programLang.ProgramsName + "%");

            if (!isCount)
            {
                query.AddScalar("ProgramLangId", NHibernateUtil.Int64);
                query.AddScalar("ProgramId", NHibernateUtil.Int16);
                query.AddScalar("ProgramName", NHibernateUtil.String);
                query.AddScalar("Comment", NHibernateUtil.String);
                query.SetResultTransformer(Transformers.AliasToBean(typeof(ProgramLang)));
            }
            else
            {
                query.AddScalar("Count", NHibernateUtil.Int32);
            }

            return query;
        }
        public void DeleteAllProgramLang(short programId)
        {
            GetCurrentSession()
            .Delete("from SuProgramLang pl where pl.Program.Programid = :ProgramId ", new object[] { programId }, new NHibernate.Type.IType[] { NHibernateUtil.Int16 });
        }
    }
}
