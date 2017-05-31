using System;
using System.Collections;
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
using NHibernate.Expression;

using SS.Standard.Security;
using SS.DB.Query;

namespace SS.SU.Query.Hibernate
{
    public class SuUserQuery : NHibernateQueryBase<SuUser, long>, ISuUserQuery
    {
        #region ISuUserQuery Members
        private static string strGetSuUserByUserName_Password = "SELECT  *  FROM  SuUser  WHERE [UserName] = :UserName  AND [Password] = :Password  AND [Active] = 1  AND [SetFailTime] > [FailTime]";
        private static string strGetSuUserByUserName = "SELECT *  FROM  SuUser  WHERE UserName = :UserName AND Active = 1 ";
        //private static string strGetUserSessionList = "SELECT SuUser.UserID, SuUser.UserName,SuUserLang.FirstName,SuUserLang.Lastname,SuUser.PersonalWebUrl,  SuUser.OrganizationID   , SuOrganizationLang.OrganizationName , SuUser.DivisionID  , SuDivisionLang.DivisionName  ,SuUser.LanguageID as UserLanguageID,DbLanguage.LanguageName as UserLanguageName,DbLanguage.LanguageCode as UserLanguageCode,SuUser.LanguageID as CurrentUserLanguageID,DbLanguage.LanguageName as CurrentUserLanguageName,DbLanguage.LanguageCode as CurrentUserLanguageCode , SuUser.EffDate,SuUser.EndDate,'SessionID' as SessionID    FROM   SuUser LEFT OUTER JOIN DbLanguage ON SuUser.LanguageID = DbLanguage.LanguageID    LEFT OUTER JOIN SuDivision ON SuUser.DivisionID = SuDivision.DivisionID   LEFT OUTER JOIN SuOrganization ON SuUser.OrganizationID = SuOrganization.OrganizationID   AND SuDivision.OrganizationID = SuOrganization.OrganizationID    LEFT OUTER JOIN SuOrganizationLang ON DbLanguage.LanguageID = SuOrganizationLang.LanguageID    AND SuOrganization.OrganizationID = SuOrganizationLang.OrganizationID   LEFT OUTER JOIN SuDivisionLang ON DbLanguage.LanguageID = SuDivisionLang.LanguageID  AND SuDivision.DivisionID = SuDivisionLang.DivisionID  LEFT OUTER JOIN SuUserLang ON SuUser.UserID = SuUserLang.UserID     WHERE    SuUserLang.LanguageID=SuUser.LanguageID AND  SuUser.UserID = :UserID";
        //private static string strGetUserRoleList = "SELECT UR.UserID,R.RoleID,RL.RoleName,R.VerifyDocument,R.ApproveVerifyDocument,R.VerifyPayment,R.ApproveVerifyPayment,R.CounterCashier,R.ReceiveDocument  FROM SuUserRole as UR LEFT OUTER JOIN  SuRole as R on UR.RoleID = R.RoleID LEFT OUTER JOIN SuRoleLang as RL ON R.RoleID = RL.RoleID AND RL.LanguageID = :LanguageID  WHERE UR.UserID = :UserID AND UR.Active = 1 AND R.Active=1 AND RL.Active=1";
        private static string strGetUserRoleList = "SELECT UR.UserID,'RoleName' as RoleName,R.RoleID,R.VerifyDocument,R.ApproveVerifyDocument,R.VerifyPayment,R.ApproveVerifyPayment,R.CounterCashier,R.ReceiveDocument,R.AllowMultipleApprovePayment,R.AllowMultipleApproveAccountant  FROM SuUserRole as UR LEFT OUTER JOIN  SuRole as R on UR.RoleID = R.RoleID WHERE UR.UserID = :UserID AND UR.Active = 1 AND R.Active=1";
        //private static string strGetUserTransactionPermissionList = "SELECT *  FROM [SuUserTransactionPermission]  WHERE [UserID] =:UserID  AND [Active] = 1";
        public IList<SuUserSearchResult> GetTranslatedList(short languageID)
        {
            StringBuilder strQuery = new StringBuilder();
            strQuery.AppendLine(" SELECT users.UserID, users.UserName, users.Password, users.SetFailTime, users.FailTime ");
            strQuery.AppendLine(" , users.EffDate, users.EndDate, users.ChangePassword, users.Comment, users.Active ");
            strQuery.AppendLine(" , div.DivisionID, divLang.DivisionName, org.OrganizationID, orgLang.OrganizationName ");
            strQuery.AppendLine(" , lang.LanguageID, lang.LanguageName ");
            strQuery.AppendLine(" FROM SuUser users ");
            strQuery.AppendLine(" INNER JOIN SuOrganization org ");
            strQuery.AppendLine(" ON org.OrganizationID = users.OrganizationID ");
            strQuery.AppendLine(" INNER JOIN SuOrganizationLang orgLang ");
            strQuery.AppendLine(" ON orgLang.OrganizationID = org.OrganizationID AND orgLang.LanguageID = :languageID ");
            strQuery.AppendLine(" INNER JOIN SuDivision div ");
            strQuery.AppendLine(" ON div.DivisionID = users.DivisionID ");
            strQuery.AppendLine(" INNER JOIN SuDivisionLang divLang ");
            strQuery.AppendLine(" ON divLang.DivisionID = div.DivisionID AND divLang.LanguageID = :languageID ");
            strQuery.AppendLine(" INNER JOIN DbLanguage lang ");
            strQuery.AppendLine(" ON lang.LanguageID = users.LanguageID ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());

            query.SetInt16("languageID", languageID);
            query.AddScalar("UserID", NHibernateUtil.Int64);
            query.AddScalar("UserName", NHibernateUtil.String);
            query.AddScalar("Password", NHibernateUtil.String);
            query.AddScalar("SetFailTime", NHibernateUtil.Int16);
            query.AddScalar("FailTime", NHibernateUtil.Int16);
            query.AddScalar("EffDate", NHibernateUtil.DateTime);
            query.AddScalar("EndDate", NHibernateUtil.DateTime);
            query.AddScalar("ChangePassword", NHibernateUtil.Boolean);
            query.AddScalar("Comment", NHibernateUtil.String);
            query.AddScalar("Active", NHibernateUtil.Boolean);
            query.AddScalar("DivisionID", NHibernateUtil.Int16);
            query.AddScalar("DivisionName", NHibernateUtil.String);
            query.AddScalar("OrganizationID", NHibernateUtil.Int16);
            query.AddScalar("OrganizationName", NHibernateUtil.String);
            query.AddScalar("LanguageID", NHibernateUtil.Int16);
            query.AddScalar("LanguageName", NHibernateUtil.String);

            IList<SuUserSearchResult> list = query.SetResultTransformer(Transformers.AliasToBean(typeof(SuUserSearchResult))).List<SuUserSearchResult>();

            return list;
        }
        public IList GetUserList(short languageID)
        {
            StringBuilder strQuery = new StringBuilder();
            strQuery.AppendLine(" SELECT * ");
            //strQuery.AppendLine(" SELECT users.UserID, users.UserName, users.Password, users.SetFailTime, users.FailTime ");
            //strQuery.AppendLine(" , users.EffDate, users.EndDate, users.ChangePassword, users.Comment, users.Active ");
            //strQuery.AppendLine(" , div.DivisionID, divLang.DivisionName, org.OrganizationID, orgLang.OrganizationName ");
            //strQuery.AppendLine(" , lang.LanguageID, lang.LanguageName ");
            //strQuery.AppendLine(" FROM SuUser users ");
            //strQuery.AppendLine(" INNER JOIN SuOrganization org ");
            //strQuery.AppendLine(" ON org.OrganizationID = users.OrganizationID ");
            //strQuery.AppendLine(" INNER JOIN SuOrganizationLang orgLang ");
            //strQuery.AppendLine(" ON orgLang.OrganizationID = org.OrganizationID AND orgLang.LanguageID = :languageID ");
            //strQuery.AppendLine(" INNER JOIN SuDivision div ");
            //strQuery.AppendLine(" ON div.DivisionID = users.DivisionID ");
            //strQuery.AppendLine(" INNER JOIN SuDivisionLang divLang ");
            //strQuery.AppendLine(" ON divLang.DivisionID = div.DivisionID AND divLang.LanguageID = :languageID ");
            //strQuery.AppendLine(" INNER JOIN DbLanguage lang ");
            //strQuery.AppendLine(" ON lang.LanguageID = users.LanguageID ");

            //ISQLQuery query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());
            //query.SetInt16("languageID", languageID);
            //query.AddEntity(typeof(SuUser));

            ICriteria criteria = GetCurrentSession().CreateCriteria(typeof(SuUser), "users");
            criteria.CreateCriteria("Organization", "org", NHibernate.SqlCommand.JoinType.InnerJoin);
            criteria.CreateCriteria("org.OrganizationLang", "orgLang", NHibernate.SqlCommand.JoinType.InnerJoin);
            criteria.Add(Expression.Eq("orgLang.Language.Languageid", languageID));
            criteria.CreateCriteria("Division", "div", NHibernate.SqlCommand.JoinType.InnerJoin);
            criteria.CreateCriteria("div.DivisionLang", "divLang", NHibernate.SqlCommand.JoinType.InnerJoin);
            criteria.Add(Expression.Eq("divLang.Language.Languageid", languageID));
            criteria.CreateCriteria("Language", "lang", NHibernate.SqlCommand.JoinType.InnerJoin);
            criteria.SetProjection(Projections.ProjectionList()
                .Add(Projections.Property("orgLang.OrganizationName"), "orgName")
                .Add(Projections.Property("divLang.DivisionName"), "divName")
                .Add(Projections.Property("lang.LanguageName"), "langName")
                .Add(Projections.Property("users.Userid"), "uID")
                .Add(Projections.Property("users.UserName"), "uName")
                .Add(Projections.Property("users.Password"), "uPassword")
                .Add(Projections.Property("users.EffDate"), "uEffDate")
                .Add(Projections.Property("users.EndDate"), "uEndDate")
                .Add(Projections.Property("users.FailTime"), "uFailTime")
                .Add(Projections.Property("users.ChangePassword"), "uChangePassword")
                .Add(Projections.Property("users.Active"), "uActive")
                .Add(Projections.Property("users.Comment"), "uComment"));

            //criteria.CreateCriteria("DivisionLang", "divLang", NHibernate.SqlCommand.JoinType.InnerJoin);
            //criteria.Add(Expression.Eq("divLang.LanguageId", languageID));
            //criteria.SetProjection(Projections.ProjectionList()
            //    .Add(Projections.Property("org.Comment"))
            //    .Add(Projections.Property("div.Comment")));


            //.CreateAlias("Organization", "org")
            //.CreateAlias("Division", "div")
            //.CreateAlias("OrganizationLang", "orgLang")
            //.CreateAlias("DivisionLang", "divLang")
            ////.CreateAlias("DivisionLang", "divLang")
            ////.SetProjection(Projections.Property("OrganizationName"))
            //.SetProjection( Projections.ProjectionList()
            //    .Add(Projections.Property("orgLang.OrganizationName"))
            //    .Add(Projections.Property("divLang.DivisionName")));



            //query.SetInt16("languageID", languageID);
            //query.AddScalar("UserID", NHibernateUtil.Int64);
            //query.AddScalar("UserName", NHibernateUtil.String);
            //query.AddScalar("Password", NHibernateUtil.String);
            //query.AddScalar("SetFailTime", NHibernateUtil.Int16);
            //query.AddScalar("FailTime", NHibernateUtil.Int16);
            //query.AddScalar("EffDate", NHibernateUtil.DateTime);
            //query.AddScalar("EndDate", NHibernateUtil.DateTime);
            //query.AddScalar("ChangePassword", NHibernateUtil.Boolean);
            //query.AddScalar("Comment", NHibernateUtil.String);
            //query.AddScalar("Active", NHibernateUtil.Boolean);
            //query.AddScalar("DivisionID", NHibernateUtil.Int16);
            //query.AddScalar("DivisionName", NHibernateUtil.String);
            //query.AddScalar("OrganizationID", NHibernateUtil.Int16);
            //query.AddScalar("OrganizationName", NHibernateUtil.String);
            //query.AddScalar("LanguageID", NHibernateUtil.Int16);
            //query.AddScalar("LanguageName", NHibernateUtil.String);

            //IList<SuUser> list = query.SetResultTransformer(Transformers.AliasToBean(typeof(SuUser))).List<SuUser>();
            IList list = criteria.List();

            return list;
        }
        public IList<SuUserGridView> FindUserAll()
        {
            throw new NotImplementedException();
        }
        public SuUser GetSuUserByUserName_Password(string UserName, string Password)
        {
            //ICriteria criteria = GetCurrentSession().CreateCriteria(typeof(SuUser), "u");
            //criteria.Add(Expression.And(Expression.Eq("u.UserName", UserName.Trim())
            //    , Expression.Eq("u.Password", SS.Standard.Utilities.Encryption.Md5Hash(Password))));
            //IList<SuUser> user = criteria.List<SuUser>();

            IList<SuUser> user = GetCurrentSession()
                .CreateQuery("FROM SuUser u WHERE u.UserName = :UserName AND u.Password = :Password AND u.Active = 1 ")
                .SetString("UserName", UserName.Trim())
                .SetString("Password", SS.Standard.Utilities.Encryption.Md5Hash(Password))
                .List<SuUser>();
//                .SetInt16("Active", 1)
                //.List<SuUser>();

            
            if (user.Count > 0)
            {
                return user[0];
            }
            else
            {
                return null;
            }


            //ISQLQuery query = GetCurrentSession().CreateSQLQuery(strGetSuUserByUserName_Password);

            //query.SetString("UserName", UserName.Trim());
            //query.SetString("Password",SS.Standard.Utilities.Encryption.Md5Hash(Password));
            //query.AddScalar("Userid", NHibernateUtil.Int64);
            //query.AddScalar("FailTime", NHibernateUtil.Int16);
            //query.AddScalar("PasswordExpiryDate",NHibernateUtil.DateTime);
            //query.AddScalar("ChangePassword", NHibernateUtil.Boolean);
            //query.AddScalar("LanguageID", NHibernateUtil.Int16);

            //IList<SuUser> user = query.SetResultTransformer(Transformers.AliasToBean(typeof(SuUser))).List<SuUser>();
            //if (user.Count > 0)
            //{
            //    return user[0];
            //}
            //else
            //{
            //    return null;
            //}
        }
        public SuUser GetSuUserByUserName(string UserName)
        {
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(strGetSuUserByUserName);
            query.SetString("UserName", UserName.Trim());
            query.AddEntity(typeof(SuUser));
            IList<SuUser> user = query.List<SuUser>();
            if (user.Count > 0)
            {
                return user[0];
            }
            else
            {
                return null;
            }
        }
        public bool IsLocked(string UserName)
        {
            bool flag = false;
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(strGetSuUserByUserName);
            query.SetString("UserName", UserName.Trim());
            query.AddEntity(typeof(SuUser));
            IList<SuUser> user = query.List<SuUser>();
            if (user.Count > 0)
            {
                if (user[0].SetFailTime <= user[0].FailTime)
                    flag = true;
            }
            return flag;
        }
        public IList<UserSession> GetUserSessionList(long userID, short languageID)
        {

            #region Roles
            ISQLQuery queryRole = GetCurrentSession().CreateSQLQuery(strGetUserRoleList);
            queryRole.SetInt64("UserID", userID);
            // queryRole.SetInt16("LanguageID", languageID);
            queryRole.AddScalar("UserID", NHibernateUtil.Int64);
            queryRole.AddScalar("RoleID", NHibernateUtil.Int16);
            queryRole.AddScalar("RoleName", NHibernateUtil.String);
            queryRole.AddScalar("VerifyDocument", NHibernateUtil.Boolean);
            queryRole.AddScalar("ApproveVerifyDocument", NHibernateUtil.Boolean);
            queryRole.AddScalar("VerifyPayment", NHibernateUtil.Boolean);
            queryRole.AddScalar("ApproveVerifyPayment", NHibernateUtil.Boolean);
            queryRole.AddScalar("CounterCashier", NHibernateUtil.Boolean);
            queryRole.AddScalar("ReceiveDocument", NHibernateUtil.Boolean);
            queryRole.AddScalar("AllowMultipleApprovePayment", NHibernateUtil.Boolean);
            queryRole.AddScalar("AllowMultipleApproveAccountant", NHibernateUtil.Boolean);
            IList<UserRoles> RoleList = queryRole.SetResultTransformer(Transformers.AliasToBean(typeof(UserRoles))).List<UserRoles>();
            #endregion




            #region Transaction Permission
            //ISQLQuery queryTransactionPermission = GetCurrentSession().CreateSQLQuery(strGetUserTransactionPermissionList);
            //queryTransactionPermission.SetInt64("UserID", userID);
            //queryTransactionPermission.AddEntity(typeof(SuUserTransactionPermission));
            //IList<SuUserTransactionPermission> UserTransactionPermissionList = queryTransactionPermission.List<SuUserTransactionPermission>();
            #endregion


            StringBuilder strGetUserSessionList = new StringBuilder();
            strGetUserSessionList.Append("SELECT SuUser.UserID ");
            strGetUserSessionList.Append(",SuUser.UserName ");
            strGetUserSessionList.Append(",SuUser.EmployeeName ");
            strGetUserSessionList.Append(",SuUser.LanguageID as UserLanguageID ");
            strGetUserSessionList.Append(",DbLanguage.LanguageName as UserLanguageName ");
            strGetUserSessionList.Append(",DbLanguage.LanguageCode as UserLanguageCode ");
            strGetUserSessionList.Append(",SuUser.LanguageID as CurrentUserLanguageID ");
            strGetUserSessionList.Append(",DbLanguage.LanguageName as CurrentUserLanguageName ");
            strGetUserSessionList.Append(",DbLanguage.LanguageCode as CurrentUserLanguageCode  ");
            strGetUserSessionList.Append(",'SessionID' as SessionID     ");
            strGetUserSessionList.Append("FROM   SuUser LEFT OUTER JOIN DbLanguage ON SuUser.LanguageID = DbLanguage.LanguageID     ");
            strGetUserSessionList.Append(" WHERE    SuUser.LanguageID= :LanguageID ");
            strGetUserSessionList.Append("AND  SuUser.UserID = :UserID");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(strGetUserSessionList.ToString());
            query.SetInt64("LanguageID", languageID);
            query.SetInt64("UserID", userID);


            query.AddScalar("UserID", NHibernateUtil.Int64);
            query.AddScalar("UserName", NHibernateUtil.String);
            query.AddScalar("EmployeeName", NHibernateUtil.String);
            // query.AddScalar("LastName", NHibernateUtil.String);

            //query.AddScalar("PersonalWebUrl", NHibernateUtil.String);
            //query.AddScalar("OrganizationID", NHibernateUtil.Int16);
            //query.AddScalar("OrganizationName", NHibernateUtil.String);

            //query.AddScalar("DivisionID", NHibernateUtil.Int16);
            //query.AddScalar("DivisionName", NHibernateUtil.String);

            query.AddScalar("UserLanguageID", NHibernateUtil.Int16);
            query.AddScalar("UserLanguageName", NHibernateUtil.String);
            query.AddScalar("UserLanguageCode", NHibernateUtil.String);


            query.AddScalar("CurrentUserLanguageID", NHibernateUtil.Int16);
            query.AddScalar("CurrentUserLanguageName", NHibernateUtil.String);
            query.AddScalar("CurrentUserLanguageCode", NHibernateUtil.String);




            //query.AddScalar("EffDate", NHibernateUtil.DateTime);
            //query.AddScalar("EndDate", NHibernateUtil.DateTime);





            IList<UserSession> list = query.SetResultTransformer(Transformers.AliasToBean(typeof(UserSession))).List<UserSession>();
            list[0].UserRole = RoleList as List<UserRoles>;
            //list[0].UserTransactionPermission = UserTransactionPermissionList as List<SuUserTransactionPermission>;
            return list;
        }
        #endregion


        // Create For Program : UserProfile.
        // Create By : Pichai C.
        // Create Date : 30-Dec-2008.
        // Update By : Pichai C.
        // Update Date : 21-Dec-2008.
        #region ISuUserQuery Members (FOR PROGRAM USERPROFILE)
        public IList<SuUserSearchResult> GetSuUserSearchResultList(short languageID, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<SuUserSearchResult>(QueryProvider.SuUserQuery, "FindSuUserSearchResult", new object[] { languageID, sortExpression, false }, firstResult, maxResult, sortExpression);
        }
        public int GetCountSuUserSearchResult(short languageID)
        {
            return NHibernateQueryHelper.CountByCriteria(QueryProvider.SuUserQuery, "FindSuUserSearchResult", new object[] { languageID, string.Empty, true });
        }
        public ISQLQuery FindSuUserSearchResult(short languageID, string sortExpression, bool isCount)
        {
            StringBuilder strQuery = new StringBuilder();
            ISQLQuery query;

            if (isCount)
            {
                strQuery.AppendLine(" SELECT Count(*) as Count ");
                strQuery.AppendLine(" FROM SuUser users ");
                strQuery.AppendLine(" INNER JOIN SuOrganization org ");
                strQuery.AppendLine(" ON org.OrganizationID = users.OrganizationID ");
                strQuery.AppendLine(" INNER JOIN SuOrganizationLang orgLang ");
                strQuery.AppendLine(" ON orgLang.OrganizationID = org.OrganizationID AND orgLang.LanguageID = :languageID ");
                strQuery.AppendLine(" INNER JOIN SuDivision div ");
                strQuery.AppendLine(" ON div.DivisionID = users.DivisionID ");
                strQuery.AppendLine(" INNER JOIN SuDivisionLang divLang ");
                strQuery.AppendLine(" ON divLang.DivisionID = div.DivisionID AND divLang.LanguageID = :languageID ");
                strQuery.AppendLine(" INNER JOIN DbLanguage lang ");
                strQuery.AppendLine(" ON lang.LanguageID = users.LanguageID ");

                query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());
                query.SetInt16("languageID", languageID);
                query.AddScalar("Count", NHibernateUtil.Int32);
            }
            else
            {
                strQuery.AppendLine(" SELECT users.UserID, users.UserName, users.Password, users.SetFailTime, users.FailTime ");
                strQuery.AppendLine(" , users.EffDate, users.EndDate, users.ChangePassword, users.Comment, users.Active ");
                strQuery.AppendLine(" , div.DivisionID, divLang.DivisionName, org.OrganizationID, orgLang.OrganizationName ");
                strQuery.AppendLine(" , lang.LanguageID, lang.LanguageName ");
                strQuery.AppendLine(" FROM SuUser users ");
                strQuery.AppendLine(" INNER JOIN SuOrganization org ");
                strQuery.AppendLine(" ON org.OrganizationID = users.OrganizationID ");
                strQuery.AppendLine(" INNER JOIN SuOrganizationLang orgLang ");
                strQuery.AppendLine(" ON orgLang.OrganizationID = org.OrganizationID AND orgLang.LanguageID = :languageID ");
                strQuery.AppendLine(" INNER JOIN SuDivision div ");
                strQuery.AppendLine(" ON div.DivisionID = users.DivisionID ");
                strQuery.AppendLine(" INNER JOIN SuDivisionLang divLang ");
                strQuery.AppendLine(" ON divLang.DivisionID = div.DivisionID AND divLang.LanguageID = :languageID ");
                strQuery.AppendLine(" INNER JOIN DbLanguage lang ");
                strQuery.AppendLine(" ON lang.LanguageID = users.LanguageID ");

                if (string.IsNullOrEmpty(sortExpression))
                {
                    strQuery.AppendLine(" ORDER BY users.UserName, orgLang.OrganizationName, divLang.DivisionName, users.EffDate, users.EndDate, users.SetFailTime, users.Comment, users.Active ");
                }
                else
                {
                    strQuery.AppendLine(" ORDER BY " + sortExpression);
                }

                query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());
                query.SetInt16("languageID", languageID);
                query.AddScalar("UserID", NHibernateUtil.Int64);
                query.AddScalar("UserName", NHibernateUtil.String);
                query.AddScalar("Password", NHibernateUtil.String);
                query.AddScalar("SetFailTime", NHibernateUtil.Int16);
                query.AddScalar("FailTime", NHibernateUtil.Int16);
                query.AddScalar("EffDate", NHibernateUtil.DateTime);
                query.AddScalar("EndDate", NHibernateUtil.DateTime);
                query.AddScalar("ChangePassword", NHibernateUtil.Boolean);
                query.AddScalar("Comment", NHibernateUtil.String);
                query.AddScalar("Active", NHibernateUtil.Boolean);
                query.AddScalar("DivisionID", NHibernateUtil.Int16);
                query.AddScalar("DivisionName", NHibernateUtil.String);
                query.AddScalar("OrganizationID", NHibernateUtil.Int16);
                query.AddScalar("OrganizationName", NHibernateUtil.String);
                query.AddScalar("LanguageID", NHibernateUtil.Int16);
                query.AddScalar("LanguageName", NHibernateUtil.String);
                query.SetResultTransformer(Transformers.AliasToBean(typeof(SuUserSearchResult)));
            }

            return query;
        }
        #endregion

        #region For Initialtor Lookup
        public int GetCountInitialtorLookupResultByCriteria(UserCriteria criteria, short languageId)
        {
            return NHibernateQueryHelper.CountByCriteria(QueryProvider.SuUserQuery, "FindInitialtorLookupResultByCriteria", new object[] { criteria, languageId, string.Empty, true });

        }
        public ISQLQuery FindInitialtorLookupResultByCriteria(UserCriteria criteria, short languageId, string sortExpression, bool isCount)
        {
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            StringBuilder strQuery = new StringBuilder();
            ISQLQuery query;

            if (isCount)
            {
                strQuery.AppendLine(" SELECT Count(users.UserID) as Count  From SuUser users where 1=1 and users.Active=1 ");
            }
            else
            {
                strQuery.AppendLine(" SELECT  users.userid,users.userName,users.employeeName,users.email,users.sms  From SuUser users where 1=1 and users.Active=1 ");
            }

            StringBuilder whereClauseBuilder = new StringBuilder();
            if (criteria.UserId != null)
            {
                whereClauseBuilder.Append(" and users.UserId = :UserId ");
                parameterBuilder.AddParameterData("UserId", typeof(long), criteria.UserId);
            }
            if (!string.IsNullOrEmpty(criteria.UserName))
            {
                whereClauseBuilder.Append(" and users.UserName like :UserName ");
                parameterBuilder.AddParameterData("UserName", typeof(string), "%" + criteria.UserName + "%");
            }
            if (!string.IsNullOrEmpty(criteria.Email))
            {
                whereClauseBuilder.Append(" and users.Email like :Email ");
                parameterBuilder.AddParameterData("Email", typeof(string), "%" + criteria.Email + "%");
            }
            if (!string.IsNullOrEmpty(criteria.UserIdNOTIN))
            {
                whereClauseBuilder.Append(" and users.UserID NOT IN(" + criteria.UserIdNOTIN.ToString() + ") ");
            }
            if (!string.IsNullOrEmpty(criteria.UserIdIN))
            {
                whereClauseBuilder.Append(" and users.UserID IN(" + criteria.UserIdIN.ToString() + ") ");
            }

            strQuery.Append(whereClauseBuilder.ToString());

            query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());
            parameterBuilder.FillParameters(query);
            if (isCount)
            {
                query.AddScalar("Count", NHibernateUtil.Int32);
            }
            else
            {
                query.AddScalar("userid", NHibernateUtil.Int64);
                query.AddScalar("userName", NHibernateUtil.String);
                query.AddScalar("employeeName", NHibernateUtil.String);
                query.AddScalar("email", NHibernateUtil.String);
                query.AddScalar("sms", NHibernateUtil.Boolean);
                query.SetResultTransformer(Transformers.AliasToBean(typeof(SuUserSearchResult)));
            }

            return query;
        }
        #endregion

        public IList<SuUserSearchResult> GetInitialtorLookupSearchResultByCriteria(UserCriteria criteria, short languageId, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<SuUserSearchResult>(QueryProvider.SuUserQuery, "FindInitialtorLookupResultByCriteria", new object[] { criteria, languageId, string.Empty, false }, firstResult, maxResult, sortExpression);
        }
        #region ISuUserQuery Members (FOR PROGRAM USERLOOKUP)
        public ISQLQuery FindSuUserSearchResultByCriteria(UserCriteria criteria, short languageId, string sortExpression, bool isCount)
        {
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            StringBuilder strQuery = new StringBuilder();
            ISQLQuery query;

            if (isCount)
            {
                strQuery.AppendLine(" SELECT Count(users.UserID) as Count ");
            }
            else
            {
                strQuery.AppendLine(" SELECT distinct(users.UserID) as UserID, users.UserName as UserName, users.EmployeeName as EmployeeName,users.Email as Email,users.SMS");
                strQuery.AppendLine(" , div.DivisionID as DivisionID, divLang.DivisionName as DivisionName, org.OrganizationID as OrganizationID, orgLang.OrganizationName as OrganizationName ");
            }
            strQuery.AppendLine(" FROM SuUser users ");
            // strQuery.AppendLine(" INNER JOIN SuUserLang userLang ");
            //  strQuery.AppendLine(" ON users.UserID = userLang.UserID and userLang.LanguageID = :LanguageID ");
            strQuery.AppendLine(" INNER JOIN SuOrganization org ");
            strQuery.AppendLine(" ON org.OrganizationID = users.OrganizationID ");
            strQuery.AppendLine(" INNER JOIN SuOrganizationLang orgLang ");
            strQuery.AppendLine(" ON orgLang.OrganizationID = org.OrganizationID and orgLang.LanguageID = :LanguageID ");
            strQuery.AppendLine(" INNER JOIN SuDivision div ");
            strQuery.AppendLine(" ON div.DivisionID = users.DivisionID ");
            strQuery.AppendLine(" INNER JOIN SuDivisionLang divLang ");
            strQuery.AppendLine(" ON divLang.DivisionID = div.DivisionID and divLang.LanguageID = :LanguageID");

            parameterBuilder.AddParameterData("LanguageID", typeof(short), languageId);
            //string whereClause = " where 1=1 ";
            StringBuilder whereClauseBuilder = new StringBuilder();
            if (criteria.UserId != null)
            {
                whereClauseBuilder.Append(" and users.UserId = :UserId ");
                parameterBuilder.AddParameterData("UserId", typeof(long), criteria.UserId);
            }
            if (criteria.UserName != null)
            {
                whereClauseBuilder.Append(" and users.UserName like :UserName ");
                parameterBuilder.AddParameterData("UserName", typeof(string), "%" + criteria.UserName + "%");
            }
            //if (!string.IsNullOrEmpty(criteria.Name))
            //{
            //    whereClauseBuilder.Append(" and userLang.FirstName like :Name or userLang.LastName like :Name");
            //    parameterBuilder.AddParameterData("Name", typeof(string), "%" + criteria.Name + "%");
            //}
            if (!string.IsNullOrEmpty(criteria.CompanyName))
            {
                whereClauseBuilder.Append(" and orgLang.OrganizationName like :OrganizationName ");
                parameterBuilder.AddParameterData("OrganizationName", typeof(string), "%" + criteria.CompanyName + "%");
            }
            if (!string.IsNullOrEmpty(criteria.DivisionName))
            {
                whereClauseBuilder.Append(" and divLang.DivisionName like :DivisionName ");
                parameterBuilder.AddParameterData("DivisionName", typeof(string), "%" + criteria.DivisionName + "%");
            }
            if (!string.IsNullOrEmpty(criteria.UserIdNOTIN))
            {
                whereClauseBuilder.Append(" and users.UserID NOT IN(" + criteria.UserIdNOTIN.ToString() + ") ");
                // add แบบ parameterBuilder ไม่ได้อะ ใช้แบบนี้ไปก่อน อิอิ
                // parameterBuilder.AddParameterData("UserIdFilter", typeof(string), criteria.UserIdFilter);
            }
            if (!string.IsNullOrEmpty(criteria.UserIdIN))
            {
                whereClauseBuilder.Append(" and users.UserID IN(" + criteria.UserIdIN.ToString() + ") ");
                // add แบบ parameterBuilder ไม่ได้อะ ใช้แบบนี้ไปก่อน อิอิ
                // parameterBuilder.AddParameterData("UserIdFilter", typeof(string), criteria.UserIdFilter);
            }
            if (!string.IsNullOrEmpty(whereClauseBuilder.ToString()))
            {
                strQuery.AppendLine(" where 1=1 ");
                strQuery.AppendLine(whereClauseBuilder.ToString());
            }


            query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());
            parameterBuilder.FillParameters(query);
            if (isCount)
            {
                query.AddScalar("Count", NHibernateUtil.Int32);
            }
            else
            {
                //query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());
                //query.SetInt16("languageID", languageID);
                query.AddScalar("UserID", NHibernateUtil.Int64);
                query.AddScalar("UserName", NHibernateUtil.String);
                query.AddScalar("EmployeeName", NHibernateUtil.String);
                // query.AddScalar("LastName", NHibernateUtil.String);
                query.AddScalar("Email", NHibernateUtil.String);
                query.AddScalar("DivisionID", NHibernateUtil.Int16);
                query.AddScalar("DivisionName", NHibernateUtil.String);
                query.AddScalar("OrganizationID", NHibernateUtil.Int16);
                query.AddScalar("OrganizationName", NHibernateUtil.String);
                query.SetResultTransformer(Transformers.AliasToBean(typeof(SuUserSearchResult)));
            }

            return query;
        }
        public IList<SuUserSearchResult> GetUserSearchResultByCriteria(UserCriteria criteria, short languageId, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<SuUserSearchResult>(QueryProvider.SuUserQuery, "FindSuUserSearchResultByCriteria", new object[] { criteria, languageId, string.Empty, false }, firstResult, maxResult, sortExpression);
        }
        public int GetCountUserSearchResultByCriteria(UserCriteria criteria, short languageId)
        {
            return NHibernateQueryHelper.CountByCriteria(QueryProvider.SuUserQuery, "FindSuUserSearchResultByCriteria", new object[] { criteria, languageId, string.Empty, true });
        }

        public SuUserSearchResult FindUserByUserIdLanguageId(long userId, short divId, short orgId, short languageId)
        {
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            StringBuilder strQuery = new StringBuilder();

            strQuery.AppendLine(" SELECT distinct(users.UserID) as UserID, users.UserName as UserName, users.EmployeeName as EmployeeName,  users.Email as Email,users.SMS ");
            strQuery.AppendLine(" , div.DivisionID as DivisionID, divLang.DivisionName as DivisionName, org.OrganizationID as OrganizationID, orgLang.OrganizationName as OrganizationName ");
            strQuery.AppendLine(" FROM SuUser users ");
            //strQuery.AppendLine(" INNER JOIN SuUserLang userLang ");
            //strQuery.AppendLine(" ON users.UserID = userLang.UserID and userLang.LanguageID = :LanguageID ");
            strQuery.AppendLine(" INNER JOIN SuOrganization org ");
            strQuery.AppendLine(" ON org.OrganizationID = users.OrganizationID ");
            strQuery.AppendLine(" INNER JOIN SuOrganizationLang orgLang ");
            strQuery.AppendLine(" ON orgLang.OrganizationID = org.OrganizationID and orgLang.LanguageID = :LanguageID ");
            strQuery.AppendLine(" INNER JOIN SuDivision div ");
            strQuery.AppendLine(" ON div.DivisionID = users.DivisionID ");
            strQuery.AppendLine(" INNER JOIN SuDivisionLang divLang ");
            strQuery.AppendLine(" ON divLang.DivisionID = div.DivisionID and divLang.LanguageID = :LanguageID");
            strQuery.AppendLine(" where users.UserID = :UserID and div.DivisionID = :DivisionID and org.OrganizationID = :OrganizationID");

            parameterBuilder.AddParameterData("UserID", typeof(long), userId);
            parameterBuilder.AddParameterData("DivisionID", typeof(short), divId);
            parameterBuilder.AddParameterData("OrganizationID", typeof(short), orgId);
            parameterBuilder.AddParameterData("LanguageID", typeof(short), languageId);

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());
            parameterBuilder.FillParameters(query);

            query.AddScalar("UserID", NHibernateUtil.Int64);
            query.AddScalar("UserName", NHibernateUtil.String);
            query.AddScalar("EmployeeName", NHibernateUtil.String);
            //query.AddScalar("LastName", NHibernateUtil.String);
            query.AddScalar("Email", NHibernateUtil.String);
            query.AddScalar("DivisionID", NHibernateUtil.Int16);
            query.AddScalar("DivisionName", NHibernateUtil.String);
            query.AddScalar("OrganizationID", NHibernateUtil.Int16);
            query.AddScalar("OrganizationName", NHibernateUtil.String);

            query.SetResultTransformer(Transformers.AliasToBean(typeof(SuUserSearchResult)));

            return query.UniqueResult<SuUserSearchResult>();
        }


        public SuUserSearchResult FindUserByUserIdLanguageId(long userId)
        {
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            StringBuilder strQuery = new StringBuilder();

            strQuery.AppendLine(" SELECT distinct(users.UserID) as UserID, users.UserName as UserName, users.EmployeeName as EmployeeName,  users.Email as Email,users.SMS,users.CompanyCode as CompanyCode ");
            strQuery.AppendLine(" FROM SuUser users ");
            strQuery.AppendLine(" where users.UserID = :UserID ");

            parameterBuilder.AddParameterData("UserID", typeof(long), userId);
            //   parameterBuilder.AddParameterData("LanguageID", typeof(short), languageId);

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());
            parameterBuilder.FillParameters(query);

            query.AddScalar("UserID", NHibernateUtil.Int64);
            query.AddScalar("UserName", NHibernateUtil.String);
            query.AddScalar("EmployeeName", NHibernateUtil.String);
            query.AddScalar("CompanyCode", NHibernateUtil.String);
            query.AddScalar("Email", NHibernateUtil.String);
            query.AddScalar("SMS", NHibernateUtil.Boolean);


            query.SetResultTransformer(Transformers.AliasToBean(typeof(SuUserSearchResult)));

            return query.UniqueResult<SuUserSearchResult>();
        }
        #endregion

        #region public ISQLQuery FindByCriteria
        public ISQLQuery FindByUserCriteria(UserCriteria criteria, bool isCount, string sortExpression)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            if (!isCount)
            {
                sqlBuilder.Append(" select u.Userid as Userid, u.UserName as UserName, u.EmployeeName as EmployeeName, u.EmployeeCode as EmployeeCode , u.CompanyCode AS CompanyCode ");
                sqlBuilder.Append(" , u.Email as Email, u.SMSApproveOrReject as SMSApproveOrReject, u.SMSReadyToReceive as SMSReadyToReceive,u.Active AS Active ");
            }
            else
            {
                sqlBuilder.Append(" select count(u.Userid) as Count ");
            }
            sqlBuilder.Append(" from SuUser u ");

            #region WhereClause
            StringBuilder whereClauseBuilder = new StringBuilder();
            // Edit By Kookkla 12/06/2009
            // LOG NUMBER : 387
            whereClauseBuilder.Append("  and u.Active = 1 ");  
            if (!string.IsNullOrEmpty(criteria.UserName))
            {
                whereClauseBuilder.Append(" and u.UserName Like :UserName ");
                queryParameterBuilder.AddParameterData("UserName", typeof(string), String.Format("%{0}%", criteria.UserName));
            }
            if (!string.IsNullOrEmpty(criteria.EmployeeName))
            {
                whereClauseBuilder.Append(" and u.EmployeeName Like :EmployeeName ");
                queryParameterBuilder.AddParameterData("EmployeeName", typeof(string), String.Format("%{0}%", criteria.EmployeeName));
            }
            
            if (!string.IsNullOrEmpty(criteria.EmployeeCode))
            {
                whereClauseBuilder.Append(" and u.EmployeeCode Like :EmployeeCode ");
                queryParameterBuilder.AddParameterData("EmployeeCode", typeof(string), String.Format("%{0}%", criteria.EmployeeCode));
            }

            if (criteria.IsFavoriteApprover)
            {
                sqlBuilder.Append(" inner join SuUserFavoriteActor approver on approver.ActorUserID = u.UserID and approver.ActorType = '1' ");
                if (criteria.RequesterID.HasValue && criteria.RequesterID.Value > 0)
                {
                    whereClauseBuilder.Append(" and approver.Userid = :RequesterIDApprover ");
                    queryParameterBuilder.AddParameterData("RequesterIDApprover", typeof(long), criteria.RequesterID.Value);
                }
            }
            //if (criteria.IsFavoriteInitiator)
            //{
            //    sqlBuilder.Append(" inner join SuUserFavoriteActor initiator on initiator.ActorUserID = u.UserID and initiator.ActorType = '2' ");
            //    if (criteria.RequesterID.HasValue && criteria.RequesterID.Value > 0)
            //    {
            //        whereClauseBuilder.Append(" and initiator.Userid = :RequesterIDInitiator ");
            //        queryParameterBuilder.AddParameterData("RequesterIDInitiator", typeof(long), criteria.RequesterID.Value);
            //    }
            //}
            if (criteria.IsApprovalFlag)
            {
                whereClauseBuilder.Append(" and u.ApprovalFlag = :ApprovalFlag ");
                queryParameterBuilder.AddParameterData("ApprovalFlag", typeof(bool), criteria.IsApprovalFlag);
            }
            if (!string.IsNullOrEmpty(criteria.UserIdNOTIN))
            {
                whereClauseBuilder.Append(" and u.UserID NOT IN(" + criteria.UserIdNOTIN.ToString() + ") ");
            }
            if (!string.IsNullOrEmpty(whereClauseBuilder.ToString()))
            {
                sqlBuilder.Append(String.Format(" where 1=1 {0} ", whereClauseBuilder.ToString()));
            }

            #endregion
            #region Order By
            if (!isCount)
            {
                sqlBuilder.Append(" group by u.Userid, u.UserName, u.EmployeeName, u.EmployeeCode, u.CompanyCode , u.Email, u.SMSApproveOrReject, u.SMSReadyToReceive,u.Active ");
                if (!string.IsNullOrEmpty(sortExpression))
                {
                    sqlBuilder.Append(String.Format(" order by {0} ", sortExpression));
                }
                else
                {
                    sqlBuilder.Append(" order by u.Userid, u.UserName, u.EmployeeName, u.EmployeeCode , u.CompanyCode  ");
                }
            }
            #endregion

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            queryParameterBuilder.FillParameters(query);

            if (isCount)
            {
                query.AddScalar("Count", NHibernateUtil.Int32);
            }
            else
            {
                query.AddScalar("Userid", NHibernateUtil.Int64)
                    .AddScalar("UserName", NHibernateUtil.String)
                    .AddScalar("EmployeeName", NHibernateUtil.String)
                    .AddScalar("EmployeeCode", NHibernateUtil.String)
                    .AddScalar("CompanyCode", NHibernateUtil.String)
                    .AddScalar("Email", NHibernateUtil.String)
                    .AddScalar("SMSApproveOrReject", NHibernateUtil.Boolean)
                    .AddScalar("SMSReadyToReceive", NHibernateUtil.Boolean)
                    .AddScalar("Active", NHibernateUtil.Boolean);

                query.SetResultTransformer(Transformers.AliasToBean(typeof(SuUser)));
            }

            return query;
        }
        #endregion
        #region public ISQLQuery FindUserProfileByCriteria
        public ISQLQuery FindUserProfileByCriteria(VOUserProfile criteria, bool isCount, string sortExpression)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            if (!isCount)
            {
                sqlBuilder.Append("select u.userid as UserId, u.username as UserName, com.CompanyName as CompanyName,u.EmployeeName as EmployeeName ,u.Active as Active,u.CompanyCode as CompanyCode ,u.IsNotifyRemindWaitApprove As EmailActive ");
            }
            else
            {
                sqlBuilder.Append(" select count(u.Userid) as Count ");
            }
            sqlBuilder.Append(" from SuUser u ");
            sqlBuilder.Append(" left join DbCompany com on u.companyid=com.companyid ");

            #region WhereClause
            StringBuilder whereClauseBuilder = new StringBuilder();

            if (!string.IsNullOrEmpty(criteria.UserName))
            {
                whereClauseBuilder.Append(" and u.username like :UserName ");
                queryParameterBuilder.AddParameterData("UserName", typeof(string), String.Format("%{0}%", criteria.UserName));
            }
            if (criteria.UserGroupId.HasValue)
            {
                sqlBuilder.Append(" inner join SuUserRole ur on u.userid=ur.userid  ");
                whereClauseBuilder.Append(" and ur.roleid = :UserGroupId ");
                queryParameterBuilder.AddParameterData("UserGroupId", typeof(long), criteria.UserGroupId.Value);
            }
            if (!string.IsNullOrEmpty(criteria.EmployeeName))
            {
                whereClauseBuilder.Append(" and u.EmployeeName Like :EmployeeName ");
                queryParameterBuilder.AddParameterData("EmployeeName", typeof(string), String.Format("%{0}%", criteria.EmployeeName));
            }

            if (!string.IsNullOrEmpty(whereClauseBuilder.ToString()))
            {
                sqlBuilder.Append(String.Format(" where 1=1 {0} ", whereClauseBuilder.ToString()));
            }
            #endregion
            #region Order By
            if (!isCount)
            {
                if (!string.IsNullOrEmpty(sortExpression))
                {
                    sqlBuilder.Append(String.Format(" order by {0} ", sortExpression));
                }
                else
                {
                    sqlBuilder.Append(" order by u.UserId, u.UserName, u.EmployeeName,com.CompanyName ,u.Active");
                }
            }
            #endregion

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            queryParameterBuilder.FillParameters(query);

            if (isCount)
            {
                query.AddScalar("Count", NHibernateUtil.Int32);
            }
            else
            {
                query.AddScalar("UserId", NHibernateUtil.Int64)
                    .AddScalar("UserName", NHibernateUtil.String)
                    .AddScalar("EmployeeName", NHibernateUtil.String)
                    .AddScalar("CompanyCode", NHibernateUtil.String)
                    .AddScalar("Active", NHibernateUtil.Boolean)
                    .AddScalar("EmailActive", NHibernateUtil.Boolean);
                // .AddScalar("CompanyName", NHibernateUtil.String)

                query.SetResultTransformer(Transformers.AliasToBean(typeof(VOUserProfile)));
            }

            return query;
        }
        #endregion

        public IList<SuUser> GetUserProfileList(UserCriteria criteria, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<SuUser>(QueryProvider.SuUserQuery, "FindByUserCriteria", new object[] { criteria, false, sortExpression }, firstResult, maxResult, sortExpression);
        }

        public IList<VOUserProfile> GetUserProfileListByCriteria(VOUserProfile criteria, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<VOUserProfile>(QueryProvider.SuUserQuery, "FindUserProfileByCriteria", new object[] { criteria, false, sortExpression }, firstResult, maxResult, sortExpression);
        }

        public int CountByUserCriteria(UserCriteria criteria)
        {
            return NHibernateQueryHelper.CountByCriteria(QueryProvider.SuUserQuery, "FindByUserCriteria", new object[] { criteria, true, string.Empty });
        }

        # region CountUserProfileByCriteria

        public int CountUserProfileByCriteria(VOUserProfile criteria)
        {
            return NHibernateQueryHelper.CountByCriteria(QueryProvider.SuUserQuery, "FindUserProfileByCriteria", new object[] { criteria, true, string.Empty });
        }
        #endregion
        public IList<SuUser> FindAutoComplete(string prefixText, UserAutoCompleteParameter param)
        {
            ICriteria criteria = GetCurrentSession().CreateCriteria(typeof(SuUser), "u");
            // Edit By Kookkla 12/06/2009
            // LOG NUMBER : 387
            //criteria.Add(Expression.Eq("u.Active", true));

            if (!string.IsNullOrEmpty(prefixText))
                criteria.Add(Expression.Like("u.UserName", string.Format("{0}%", prefixText)));

            if (param.IsApprovalFlag)
                criteria.Add(Expression.Eq("u.ApprovalFlag", param.IsApprovalFlag));

            if (param.IsActive != null)
            {
                if (param.IsActive == true)
                {
                    criteria.Add(Expression.Eq("u.Active", param.IsActive));
                }
            }

            return criteria.List<SuUser>();
        }
        public SuUser FindUserByID(long userID)
        {
            ICriteria criteria = GetCurrentSession().CreateCriteria(typeof(SuUser), "u");
            criteria.Add(Expression.Eq("u.Userid", userID));
            SuUser user = criteria.UniqueResult<SuUser>();
            //Console.WriteLine(user.Company);
            //Console.WriteLine(user.CostCenter);

            return user;
        }

        public IList<SuUser> FindByUser(long userID, string UserName)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" SELECT UserID as Userid , EmployeeName as EmployeeName ,UserName as UserName,CompanyCode as CompanyCode");
            sqlBuilder.Append(" FROM SuUser ");
            sqlBuilder.Append(" WHERE  (Userid = :userID) ");

            if (!string.IsNullOrEmpty(UserName))
            {
                sqlBuilder.Append(" And UserName like :UserName ");
            }
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());

            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("userID", typeof(long), userID);
            if (!string.IsNullOrEmpty(UserName))
                queryParameterBuilder.AddParameterData("UserName", typeof(string), "%" + UserName + "%");
            queryParameterBuilder.FillParameters(query);
            query.AddScalar("Userid", NHibernateUtil.Int64);
            query.AddScalar("UserName", NHibernateUtil.String);
            query.AddScalar("EmployeeName", NHibernateUtil.String);
            query.AddScalar("CompanyCode", NHibernateUtil.String);

            return query.SetResultTransformer(Transformers.AliasToBean(typeof(SuUser))).List<SuUser>();
        }

        #region ISuUserQuery for Expire Members

        public IList<SuUser> FindReminderExpireAccount(DateTime checkPointDate)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            int remiderDuration = Convert.ToInt32(ParameterServices.RemainDateToSendEMail);
            sqlBuilder.Append("SELECT Userid ");
            sqlBuilder.Append("FROM SuUser  ");
            sqlBuilder.Append("WHERE Active = 1 AND IsNull(IsAdUser, 0) <> 1 AND ");
            sqlBuilder.Append("(	SuUser.PasswordExpiryDate <= :chkdate2) ");
            sqlBuilder.Append(" AND ");
            sqlBuilder.Append("(	SuUser.PasswordExpiryDate >= :chkdate) ");

            QueryParameterBuilder paramBuilder = new QueryParameterBuilder();
            paramBuilder.AddParameterData("chkdate", typeof(DateTime), checkPointDate);
            paramBuilder.AddParameterData("chkdate2", typeof(DateTime), checkPointDate.AddDays(remiderDuration));
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            paramBuilder.FillParameters(query);
            query.AddScalar("Userid", NHibernateUtil.Int64);
            query.SetResultTransformer(Transformers.AliasToBean(typeof(SuUser))).List<SuUser>();
            return query.List<SuUser>();
        }

        #endregion

        public SuUser FindUserByUserName(string username, bool isApprovalFlag)
        {
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" from SuUser where UserName = :UserName and Active = 1 ");
            parameterBuilder.AddParameterData("UserName", typeof(string), username);

            if (isApprovalFlag)
            {
                sqlBuilder.Append(" and ApprovalFlag = :IsApprovalFlag");
                parameterBuilder.AddParameterData("IsApprovalFlag", typeof(bool), isApprovalFlag);
            }
            IQuery query = GetCurrentSession().CreateQuery(sqlBuilder.ToString());
            parameterBuilder.FillParameters(query);
            return query.UniqueResult<SuUser>();
        }

        public string FindUserNameByMobilePhoneNo(string MobilePhoneNo)
        {
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" from SuUser where MobilePhoneNo = :MobilePhoneNo and Active = 1 ");
            parameterBuilder.AddParameterData("MobilePhoneNo", typeof(string), MobilePhoneNo);
           
            IQuery query = GetCurrentSession().CreateQuery(sqlBuilder.ToString());
            parameterBuilder.FillParameters(query);
            SuUser user = query.UniqueResult<SuUser>();
            return user.UserName;
        }

        public SuUser FindSuUserByUserName(string UserName)
        {
            ISQLQuery query = GetCurrentSession().CreateSQLQuery("SELECT *  FROM  SuUser  WHERE UserName = :UserName ");
            query.SetString("UserName", UserName.Trim());
            query.AddEntity(typeof(SuUser));
            IList<SuUser> user = query.List<SuUser>();
            if (user.Count > 0)
            {
                return user[0];
    }
            else
            {
                return null;
}
        }
    }
}
