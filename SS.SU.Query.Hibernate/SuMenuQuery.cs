using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using SS.Standard.Data.NHibernate.QueryDao;

using SS.SU.DTO;
using SS.SU.DTO.ValueObject;
using SS.SU.Query;
using NHibernate;
using NHibernate.Transform;
using SS.Standard.Security;
using SS.Standard.Data.NHibernate.QueryCreator;

namespace SS.SU.Query.Hibernate
{
    public class SuMenuQuery : NHibernateQueryBase<SuMenu, short>, ISuMenuQuery
    {
        #region ISuMenuQuery Members
        public List<UserMenu> FindAllMenu(long userID, short languageID)
        {


            ISQLQuery query = GetCurrentSession().CreateSQLQuery(
                @"
SELECT  M.MenuID,ISNULL(M.MenuMainID,M.MenuID)AS MenuMainID 
                ,ML.MenuName 
				,P.ProgramPath 
				,M.MenuLevel 
				,M.MenuSeq 
				,ML.LanguageID as MenuLanguageID
				,SUR.RoleID 

            FROM SuMenu as M  
			LEFT OUTER JOIN SuMenuLang as ML ON M.MenuID = ML.MenuID AND ML.LanguageID = :LanguageID
			LEFT OUTER JOIN SuProgram as P ON M.ProgramID = P.ProgramID  
			LEFT OUTER JOIN SuProgramRole SPR ON P.ProgramID = SPR.ProgramID 
			LEFT OUTER JOIN SuUserRole SUR ON SPR.RoleID = SUR.RoleID AND SUR.UserID = :UserID  
            WHERE M.Active = 1 AND ML.Active=1 AND  (m.programid is null OR sur.roleid is not null)
Order by M.MenuLevel ASC, M.MenuSeq ASC "
                );
            query.SetInt64("UserID", userID);
            query.SetInt16("LanguageID", languageID);
            query.AddScalar("MenuID", NHibernateUtil.Int16);
            query.AddScalar("MenuMainID", NHibernateUtil.Int16);
            query.AddScalar("MenuName", NHibernateUtil.String);
            query.AddScalar("ProgramPath", NHibernateUtil.String);
            query.AddScalar("MenuLevel", NHibernateUtil.Int16);
            query.AddScalar("MenuSeq", NHibernateUtil.Int16);
            query.AddScalar("MenuLanguageID", NHibernateUtil.Int16);
            query.AddScalar("RoleID", NHibernateUtil.Int16);
            IList<UserMenu> menuList = query.SetResultTransformer(Transformers.AliasToBean(typeof(UserMenu))).List<UserMenu>();
            return menuList as List<UserMenu>;

        }

        public IList<MenuLang> FindMenuLangByTranslateId(short menuId)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            //sqlBuilder.Append("select l.LanguageId as LanguageId, l.LanguageName as LanguageName, tl.TranslateId as TranslateId,");
            //sqlBuilder.Append(" tl.Id as TranslateLangId, tl.TranslateWord as TranslateWord, tl.Comment as Comment, tl.Active as Active");
            //sqlBuilder.Append(" from DbLanguage l left join SuGlobalTranslateLang tl on tl.LanguageId = l.LanguageId and tl.translateId = :TranslateId");
            ISQLQuery query;

            sqlBuilder.AppendLine(" select l.LanguageId as LanguageId ");
            sqlBuilder.AppendLine(" ,l.LanguageName as LanguageName ");
            sqlBuilder.AppendLine(" ,m.menuId as MenuId ");
            sqlBuilder.AppendLine(" ,m.Id as MenuLangId ");
            sqlBuilder.AppendLine(" ,m.menuName as MenuName ");
            sqlBuilder.AppendLine(" ,m.Comment as Comment ");
            sqlBuilder.AppendLine(" ,m.Active as Active");
            sqlBuilder.AppendLine(" from DbLanguage l ");
            sqlBuilder.AppendLine(" left outer join SuMenuLang m ");
            sqlBuilder.AppendLine(" on l.LanguageId = m.LanguageId ");
            sqlBuilder.AppendLine(" and m.menuId = :MenuID ");
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            parameterBuilder.AddParameterData("MenuID", typeof(long), menuId);

            query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            parameterBuilder.FillParameters(query);
            query.AddScalar("LanguageId", NHibernateUtil.Int16)
                .AddScalar("LanguageName", NHibernateUtil.String)
                .AddScalar("MenuId", NHibernateUtil.Int16)
                .AddScalar("MenuLangId", NHibernateUtil.Int16)
                .AddScalar("MenuName", NHibernateUtil.String)
                .AddScalar("Comment", NHibernateUtil.String)
                .AddScalar("Active", NHibernateUtil.Boolean);

            return query.SetResultTransformer(Transformers.AliasToBean(typeof(SU.DTO.ValueObject.MenuLang))).List<MenuLang>();

        }

        public ISQLQuery FindSuMenuSearchResult(short languageID, string sortExpression, bool isCount)
        {
            StringBuilder strQuery = new StringBuilder();
            ISQLQuery query;

            if (isCount)
            {
                strQuery.AppendLine(" select count(*) as Count ");
                strQuery.AppendLine(" from SuMenu ");
                strQuery.AppendLine(" left outer join SuMenuLang ");
                strQuery.AppendLine(" on SuMenu.MenuId = SuMenuLang.MenuId ");
                strQuery.AppendLine(" and SuMenuLang.LanguageID = :languageID ");

                query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());
                query.SetInt16("languageID", languageID);
                query.AddScalar("Count", NHibernateUtil.Int32);

            }
            else
            {

                strQuery.AppendLine(" select Main.menuCode as menuMainCode,SuMenuLang.languageId,SuMenuLang.menuName,SuMenu.menuId ");
                strQuery.AppendLine(" ,SuMenu.menuCode,SuMenu.programid,SuMenu.menuMainid,SuMenu.menuLevel,SuMenu.menuSeq,SuMenu.comment,SuMenu.active ");
                strQuery.AppendLine(" from SuMenu ");
                strQuery.AppendLine(" left outer join SuMenu Main ");
                strQuery.AppendLine(" on Main.menuId = SuMenu.menuMainId ");
                strQuery.AppendLine(" left outer join SuMenuLang ");
                strQuery.AppendLine(" on SuMenu.MenuId = SuMenuLang.MenuId ");
                strQuery.AppendLine(" and SuMenuLang.LanguageID = :languageID ");
                if (string.IsNullOrEmpty(sortExpression))
                {
                    strQuery.Append(" order by SuMenu.menuCode,Main.menuCode,SuMenuLang.menuName, SuMenu.Comment ");
                }
                else
                {
                    strQuery.Append(string.Format(" order by {0} ", sortExpression));
                }

                query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());
                query.SetInt16("languageID", languageID);
                query.AddScalar("menuMainCode", NHibernateUtil.String);
                query.AddScalar("languageId", NHibernateUtil.Int16);
                query.AddScalar("menuName", NHibernateUtil.String);
                query.AddScalar("menuId", NHibernateUtil.Int16);
                query.AddScalar("menuCode", NHibernateUtil.String);
                query.AddScalar("programid", NHibernateUtil.Int16);
                query.AddScalar("menuMainid", NHibernateUtil.Int16);
                query.AddScalar("menuLevel", NHibernateUtil.Int16);
                query.AddScalar("menuSeq", NHibernateUtil.Int16);
                query.AddScalar("comment", NHibernateUtil.String);
                query.AddScalar("active", NHibernateUtil.Boolean);
                query.SetResultTransformer(Transformers.AliasToBean(typeof(SuMenuSearchResult)));
            }

            return query;
        }
        public IList<SuMenuSearchResult> GetTranslatedList(short languageID, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<SuMenuSearchResult>(
                QueryProvider.SuMenuQuery
                , "FindSuMenuSearchResult"
                , new object[] { languageID, sortExpression, false }
                , firstResult
                , maxResult
                , sortExpression);
        }
        public int GetCountMenuList(short languageID)
        {
            return NHibernateQueryHelper.CountByCriteria(
                QueryProvider.SuMenuQuery,
                "FindSuMenuSearchResult",
                new object[] { languageID, string.Empty, true });

        }

        public short GetMenuLevel(short? mainMenuId, short menuId)
        {
            short menuLevel = 0;

            if (mainMenuId.HasValue)
            {
                SuMenu menuMain = FindByIdentity(mainMenuId.Value);
                menuLevel = menuMain.MenuLevel;
                menuLevel++;
            }


            return menuLevel;
        }
        #endregion

        #region TestMasterGrid
        public Object GetList(int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<SuMenuSearchResult>(
                QueryProvider.SuMenuQuery
                , "FindSuMenuSearchResult"
                , new object[] { Convert.ToInt16("1"), sortExpression, false }
                , firstResult
                , maxResult
                , sortExpression);
            //return base.FindAll();
        }
        public int GetCount()
        {
            return NHibernateQueryHelper.CountByCriteria(
                QueryProvider.SuMenuQuery,
                "FindSuMenuSearchResult",
                new object[] { Convert.ToInt16("1"), string.Empty, true });
            //return base.FindAll().Count;
        }
        #endregion
        public short FindMenuMainIDByProgramID(short programID)
        {
            StringBuilder sql = new StringBuilder();
            QueryParameterBuilder parameter = new QueryParameterBuilder();
            sql.Append("SELECT MenuMainID ");
            sql.Append("FROM SuMenu ");
            sql.Append("WHERE ProgramID = :programID ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            parameter.AddParameterData("programID",typeof(short), programID);
            parameter.FillParameters(query);
            query.AddScalar("MenuMainID", NHibernateUtil.Int16);

            query.SetResultTransformer(Transformers.AliasToBean(typeof(MenuPath)));
            IList<MenuPath> list = query.List<MenuPath>();
            if (list.Count > 0)
            {
                return list[0].MenuMainID.Value;
            }
            else
                return 0;
        }

        public IList<MenuPath> FindAllByLanguage(short languageID)
        {
            StringBuilder sql = new StringBuilder();
            QueryParameterBuilder parameter = new QueryParameterBuilder();
            sql.Append("SELECT SuMenu.MenuID,SuMenu.MenuMainID,SuMenuLang.MenuName,SuMenu.ProgramID,SuMenu.MenuLevel ");
            sql.Append("FROM SuMenu  ");
            sql.Append("LEFT OUTER JOIN SuMenuLang ");
            sql.Append("ON  SuMenu.MenuID = SuMenuLang.MenuID ");
            sql.Append("AND SuMenuLang.LanguageID = :languareID ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            parameter.AddParameterData("languareID",typeof(short), languageID);
            parameter.FillParameters(query);
            query.AddScalar("MenuID", NHibernateUtil.Int16);
            query.AddScalar("MenuMainID", NHibernateUtil.Int16);
            query.AddScalar("MenuName", NHibernateUtil.String);
            query.AddScalar("ProgramID", NHibernateUtil.Int16);
            query.AddScalar("MenuLevel", NHibernateUtil.Int16);

            return query.SetResultTransformer(Transformers.AliasToBean(typeof(MenuPath))).List<MenuPath>();
           

        }
    }
    
}
