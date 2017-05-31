using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;

using SS.Standard.Data.NHibernate.Dao;

using SS.SU.DTO;
using SS.SU.DAL;
using SS.SU.DTO.ValueObject;
using SS.Standard.Data.NHibernate.QueryCreator;

namespace SS.SU.DAL.Hibernate
{
    public partial class SuUserDao : NHibernateDaoBase<SuUser, long>, ISuUserDao
    {



        #region ISuUserDao Members
        public bool SignIn(string username, string password)
        {


            return true;// GetCurrentSession().CreateQuery(" FROM SuUser WHERE UserName = :UserName AND  Password =:Password ").SetInt32("UserName", username).List<SuUser>();

        }
        #endregion



        #region ISuUserDao Members
        public IList<SuUser> FindByUserName(string userName)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine(" FROM SuUser su ");
            sqlBuilder.AppendLine(" WHERE su.UserName = :userName ");

            IQuery query = GetCurrentSession().CreateQuery(sqlBuilder.ToString());
            query.SetString("userName", userName);

            return query.List<SuUser>();
        }
        #endregion

        public bool FindUserName(long userId, string userName)
        {
            IList<SuUser> list = new List<SuUser>();
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine(" FROM SuUser user ");
            sqlBuilder.AppendLine(" WHERE user.Userid != :userId and user.UserName = :UserName ");

            IQuery query = GetCurrentSession().CreateQuery(sqlBuilder.ToString());
            query.SetString("UserName", userName);
            query.SetInt64("userId", userId);
            list = query.List<SuUser>();
            if (list.Count > 0)
                return true;
            else
                return false;

        }

        #region ISuUserDao Members
        public IList<VOUserProfile> FindUserProfileByUserName(string userName)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine(" FROM SuUser su ");
            sqlBuilder.AppendLine(" WHERE su.UserName = :userName ");

            IQuery query = GetCurrentSession().CreateQuery(sqlBuilder.ToString());
            query.SetString("userName", userName);

            return query.List<VOUserProfile>();
        }
        #endregion

        #region public void DeleteAllSuUserAndTADocumentInfo(short userID)
        public void DeleteAllSuUserAndTADocumentInfo(short userID)
        {
            GetCurrentSession()
            .Delete(" FROM SuUser INNER JOIN TADocumentTraveller ON	SuUser.UserID = TADocumentTraveller.UserID WHERE SuUser.UserID = :userID ",
            new object[] { userID },
            new NHibernate.Type.IType[] { NHibernateUtil.Int64 });
        }
        #endregion public void DeleteAllProvinceLang(short provinceId)

        public bool IsPrivilege(SuUser user)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine("select count(1) as cnt  ");
            sqlBuilder.AppendLine("from suuser t1 ");
            sqlBuilder.AppendLine("inner join suuserrole t2 ");
            sqlBuilder.AppendLine("on t1.userid = t2.userid  ");
            sqlBuilder.AppendLine("inner join surole t3 ");
            sqlBuilder.AppendLine("on t2.roleid = t3.roleid ");
            sqlBuilder.AppendLine("inner join suprivilegegroup t4 on t4.rolecode = t3.rolecode ");
            sqlBuilder.AppendLine("where t1.userid = :userid");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("userid", typeof(int), user.Userid);
            queryParameterBuilder.FillParameters(query);
            query.AddScalar("cnt", NHibernateUtil.Int32);

            int rowCount = query.UniqueResult<int>();
            if (rowCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SyncNewUser()
        {
            ISQLQuery query = GetCurrentSession().CreateSQLQuery("exec [dbo].[SyncNewUserData]");
            query.AddScalar("count", NHibernateUtil.Int32).UniqueResult();
        }

        public void SyncUpdateUser(string userName)
        {
            ISQLQuery query = GetCurrentSession().CreateSQLQuery("exec [dbo].[SyncUpdateUserData] :userName ");
            query.SetString("userName", userName);
            query.AddScalar("count", NHibernateUtil.Int32).UniqueResult();
        }

        public void SyncDeleteUser(string userName)
        {
            ISQLQuery query = GetCurrentSession().CreateSQLQuery("exec [dbo].[SyncDeleteUserData] :userName ");
            query.SetString("userName", userName);
            query.AddScalar("count", NHibernateUtil.Int32).UniqueResult();
        }
    }
}
