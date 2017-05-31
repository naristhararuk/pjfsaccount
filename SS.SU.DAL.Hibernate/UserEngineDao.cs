using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using NHibernate.Transform;

using SS.Standard.Security;

using SS.Standard.Data.NHibernate.Dao;

using SS.SU.DTO;

namespace SS.SU.DAL.Hibernate
{
    public partial class UserEngineDao : NHibernateDaoBase<SuUser, long>,IUserEngineDao
    {
        //private StringBuilder strQuery;
        //private static string strSetFailTimeQuery = "SELECT	*  FROM  SuUser  WHERE UserName = :UserName ";
        //private static string strGetUserTranslateList = "SELECT SuUser.UserID, SuUser.UserName,SuUserProfileLang.FirstName,SuUserProfileLang.Lastname,SuUser.PersonalWebUrl,  SuUser.OrganizationID   , SuOrganizationLang.OrganizationName , SuUser.DivisionID  , SuDivisionLang.DivisionName  ,SuUser.LanguageID as UserLanguageID,SuLanguage.LanguageName as UserLanguageName,SuLanguage.LanguageCode as UserLanguageCode,SuUser.LanguageID as CurrentLanguageID,SuLanguage.LanguageName as CurrentLanguageName,SuLanguage.LanguageCode as CurrentLanguageCode ,SuUser.FailTime, SuUser.EffDate, SuUser.EndDate , SuUser.ChangePassword  , SuUser.LanguageID  , SuLanguage.LanguageName  , SuLanguage.LanguageCode ,SuUser.Active, SuUser.EffDate,SuUser.EndDate      FROM         SuUser LEFT OUTER JOIN SuLanguage ON SuUser.LanguageID = SuLanguage.LanguageID    LEFT OUTER JOIN SuDivision ON SuUser.DivisionID = SuDivision.DivisionID   LEFT OUTER JOIN SuOrganization ON SuUser.OrganizationID = SuOrganization.OrganizationID   AND SuDivision.OrganizationID = SuOrganization.OrganizationID    LEFT OUTER JOIN SuOrganizationLang ON SuLanguage.LanguageID = SuOrganizationLang.LanguageID    AND SuOrganization.OrganizationID = SuOrganizationLang.OrganizationID   LEFT OUTER JOIN SuDivisionLang ON SuLanguage.LanguageID = SuDivisionLang.LanguageID  AND SuDivision.DivisionID = SuDivisionLang.DivisionID  LEFT OUTER JOIN SuUserProfile ON SuUser.UserID = SuUserProfile.UserID  LEFT OUTER JOIN  SuUserProfileLang ON SuUserProfile.UserID=SuUserProfileLang.UserID    WHERE     SuUser.UserID = :UserID AND SuUserProfileLang.LanguageID=SuUser.LanguageID";
        //private static string strGetSuUser = "SELECT * FROM SuUser WHERE UserID = :UserID";
        //#region IUserDao Members

        ////public long SignIn(string username, string password)
        ////{
           
        ////}

        //#endregion

        //#region IUserDao Members

        //public void setLanguage(int languageID)
        //{

        //}

        //public UserSession getUserTranslateList(long userid)
        //{
          
        //  //  ISQLQuery query = GetCurrentSession().CreateSQLQuery(strGetSuUser);
        //  //  query.SetInt64("UserID", userid);
        //  //  query.AddEntity(typeof(UserSession));
        //  //  IList<UserSession> user = query.List<UserSession>();
        //  //  if (user.Count > 0)
        //  //  {
        //  //      foreach (UserSession item in user)
        //  //      {
        //  //          UserSession a = new UserSession(
        //  //              item.UserID
        //  //              ,item.UserName
        //  //              ,item.
        //  //              );
        //  //          ///a.UserID = item.UserID;
        //  //      }
        //  //  }
        //  //  else
        //  //  {
        //  //      //SetFailTime(username);
        //  //      //return 0;
        //  //  }


        //  //  query = GetCurrentSession().CreateSQLQuery(strGetUserTranslateList);



        //  query.SetInt64("UserID", userid);
        //  query.AddScalar("UserID", NHibernateUtil.Int64);
        //  query.AddScalar("UserName", NHibernateUtil.String);
        //  query.AddScalar("FisrtName", NHibernateUtil.String);
        //  query.AddScalar("LastName", NHibernateUtil.String);

        //  query.AddScalar("PersonalWebUrl", NHibernateUtil.String);
        //  //query.AddScalar("sessionID", NHibernateUtil.String);
        //  query.AddScalar("OrganizationID", NHibernateUtil.Int16);
        //  query.AddScalar("OrganizationName", NHibernateUtil.String);

        //  query.AddScalar("DivisionID", NHibernateUtil.Int16);
        //  query.AddScalar("DivisionName", NHibernateUtil.String);

        //  query.AddScalar("UserLanguageID", NHibernateUtil.Int16);
        //  query.AddScalar("UserLanguageName", NHibernateUtil.String);
        //  query.AddScalar("UserLanguageCode", NHibernateUtil.String);


        //  query.AddScalar("CurrentLanguageID", NHibernateUtil.Int16);
        //  query.AddScalar("CurrentLanguageName", NHibernateUtil.String);
        //  query.AddScalar("CurrentLanguageCode", NHibernateUtil.String);


        //  query.AddScalar("Active", NHibernateUtil.Boolean);


        ////  query.AddScalar("UserRole", NHibernateUtil.Object);
        ////  query.AddScalar("UserMenu", NHibernateUtil.Object);

        //  query.AddScalar("EffDate", NHibernateUtil.DateTime);
        //  query.AddScalar("EndDate", NHibernateUtil.DateTime);

        //  //  //query.AddScalar("userTransactionPermisstion", NHibernateUtil.Object);
        //  //  //query.AddEntity(typeof(UserSession));
        //  //  //IList<UserSession> user = query.List<UserSession>();

        //    //IList<UserSession> user = query.SetResultTransformer(Transformers.AliasToBean(typeof(UserSession))).List<UserSession>();
        //    return null;
        //}
        //#endregion

        //#region IUserEngineDao Members


        ////public bool ResetFailTime(long userid)
        ////{
        ////    //SuUser user = this.FindByIdentity(userid);
            
        ////    //user.FailTime = 0;

        ////    //this.Update(user);


        ////  //  ISession sess = GetCurrentSession();
        ////  //  sess.FlushMode = FlushMode.Auto;
        ////  //  SuUser u = sess.Load(typeof(SuUser), userid,LockMode.Upgrade) as SuUser;
        ////  //  u.FailTime =0;
        ////  //  SaveOrUpdate(u);

        ////  // // SuUser user = (SuUser)GetCurrentSession().Load(typeof(SuUser), userid);
        ////  ////  GetCurrentSession().FlushMode = FlushMode.Auto;
        ////  //  //sess.
        ////  // // user.FailTime = 5;
        ////  //  //Update(user);

        ////    return true;
        ////}

        //private void SetFailTime(string username)
        //{
        //    ISQLQuery query = GetCurrentSession().CreateSQLQuery(strSetFailTimeQuery);
        //    query.SetString("UserName", username.Trim());
        //    query.AddEntity(typeof(DTO.SuUser));
        //    IList<SuUser> user = query.List<SuUser>();
        //    SuUser objUser = (SuUser)GetCurrentSession().Load(typeof(SuUser),user[0].Userid);
        //    objUser.FailTime=1;
        //    GetCurrentSession().Update(objUser);

        //}

        //#endregion

        //#region IUserEngineDao Members


        //public long SignIn(string username, string password)
        //{
        //    throw new NotImplementedException();
        //}

        //public bool ResetFailTime(long userID)
        //{
        //    throw new NotImplementedException();
        //}

        //#endregion
    }
}
