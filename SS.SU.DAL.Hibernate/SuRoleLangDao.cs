using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;

using SS.SU.DTO;
using SS.SU.DAL;
using NHibernate;
using NHibernate.Expression;
using NHibernate.Transform;
using SS.Standard.Data.NHibernate.QueryCreator;
using SS.SU.DTO.ValueObject;

namespace SS.SU.DAL.Hibernate
{
    public partial class SuRoleLangDao : NHibernateDaoBase<SuRoleLang, long>, ISuRoleLangDao
    {
        #region ISuRoleLangDao Members


        public ICriteria FindBySuRoleLangCriteria(SuRoleLang rolelang)
        {
            ICriteria criteria = GetCurrentSession().CreateCriteria(typeof(SuRoleLang), "t");
            criteria.Add(Expression.Eq("t.Language.Languageid", rolelang.Language.Languageid));
            return criteria;
        }
        public IList<RoleLang> FindByRoleId(short roleId)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Select  rl.ID as RoleLangId, rl.RoleId as RoleId, rl.RoleName as RoleName, rl.Comment as Comment, rl.Active as Active,l.LanguageId as LanguageId, l.LanguageName as LanguageName ");
            sql.Append("from DbLanguage as l ");
            sql.Append("left join SuRoleLang as rl on l.LanguageId = rl.LanguageId and rl.RoleId = :RoleId ");
            sql.Append("left join SuRole as r on rl.RoleId = r.RoleId");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("RoleId", typeof(Int16), roleId);
            queryParameterBuilder.FillParameters(query);
            query.AddScalar("RoleLangId", NHibernateUtil.Int16);
            query.AddScalar("RoleId", NHibernateUtil.Int16);
            query.AddScalar("LanguageId", NHibernateUtil.Int16);
            query.AddScalar("RoleName", NHibernateUtil.String);
            query.AddScalar("Comment", NHibernateUtil.String);
            query.AddScalar("Active", NHibernateUtil.Boolean);
            query.AddScalar("LanguageName", NHibernateUtil.String);

            IList<SS.SU.DTO.ValueObject.RoleLang> list = query.SetResultTransformer(Transformers.AliasToBean(typeof(SS.SU.DTO.ValueObject.RoleLang)))
                .List<SS.SU.DTO.ValueObject.RoleLang>();
            return list; 
        }

        public void DeleteAllRoleLang(short roleId)
        {
            GetCurrentSession()
            .Delete("from SuRoleLang rl where rl.Role.Roleid = :RoleId ", new object[] { roleId }, new NHibernate.Type.IType[] { NHibernateUtil.Int16 });
        }
        

        #endregion
    }
}
