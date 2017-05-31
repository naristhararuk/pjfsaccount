using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using NHibernate.Transform;
using NHibernate.Expression;

using SS.Standard.Data.NHibernate.QueryDao;
using SS.Standard.Data.NHibernate.QueryCreator;

using SS.SU.DTO;
using SS.SU.DTO.ValueObject;

using SS.SU.Query;

namespace SS.SU.Query.Hibernate
{
    public class SuUserRoleQuery : NHibernateQueryBase<SuUserRole, long>, ISuUserRoleQuery
    {
		#region ISuUserRoleQuery Members


        public SuUserRole FindUserRoleByUserRoleId(long id) 
        {

            ISQLQuery query = GetCurrentSession().CreateSQLQuery("SELECT  *  FROM  SuUserRole  WHERE [ID] = :id ");
            query.SetInt64("id", id);
            query.AddEntity(typeof(SuUserRole));
            IList<SuUserRole> user = query.List<SuUserRole>();
            if (user.Count > 0)
            {
                return user[0];
            }
            else
            {
                return null;
            }


        }



        public IList<SuUserRole> FindGroupByUserId(long userID)
        {
            //StringBuilder sqlBuilder = new StringBuilder();
            //sqlBuilder.AppendLine("FROM SuUserFavoriteActor as uf ");
            //sqlBuilder.AppendLine(" WHERE uf.User.Userid = :UserId and uf.ActorType = 1 ");   //ActorType 1 = Approver 

            //IQuery query = GetCurrentSession().CreateQuery(sqlBuilder.ToString());
            //query.SetInt64("UserId", userID);

            //return query.List<SuUserFavoriteActor>();

            IList<SuUserRole> list = GetCurrentSession()
                .CreateQuery("FROM SuUserRole u where u.User.Userid = :UserId ")
                .SetInt64("UserId", userID).List<SuUserRole>();
            return list;

        }



        public IList<SuUserRole> FindUserRoleByUserId(long userId)
        {
            return GetCurrentSession().CreateQuery("from SuUserRole where active = '1' and UserID = :UserID")
                .SetInt64("UserID", userId)
                .List<SuUserRole>();
        }


		public IList<UserRole> FindUserRoleByUserId(long userId, short languageId)
		{
			StringBuilder sqlBuilder = new StringBuilder();
			sqlBuilder.AppendLine("	SELECT ur.ID as UserRoleId, ur.UserID as UserId, ur.RoleId as RoleId, rl.RoleName as RoleName, ur.Comment as Comment, ur.Active as Active ");
			sqlBuilder.AppendLine(" FROM SuUserRole ur INNER JOIN SuRole r on r.RoleID = ur.RoleID ");
			sqlBuilder.AppendLine(" INNER JOIN SuRoleLang rl on rl.RoleID = r.RoleID and rl.LanguageID = :languageID ");
			sqlBuilder.AppendLine(" WHERE ur.UserID = :userID ");

			QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
			parameterBuilder.AddParameterData("languageID", typeof(short), languageId);
			parameterBuilder.AddParameterData("userID", typeof(long), userId);

			ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
			parameterBuilder.FillParameters(query);
			query.AddScalar("UserRoleId", NHibernateUtil.Int64)
				.AddScalar("UserId", NHibernateUtil.Int64)
				.AddScalar("RoleId", NHibernateUtil.Int16)
				.AddScalar("RoleName", NHibernateUtil.String)
				.AddScalar("Comment", NHibernateUtil.String)
				.AddScalar("Active", NHibernateUtil.Boolean);

			return query.SetResultTransformer(Transformers.AliasToBean(typeof(UserRole))).List<UserRole>();
		}
		
		//public IList<SuUserRoleSearchResult> GetTranslatedList(SuUserRoleSearchResult criteria, long userID, short languageID, int firstResult, int maxResult, string sortExpression)
		//{
		//    return NHibernateQueryHelper.FindPagingByCriteria<SuUserRoleSearchResult>(QueryProvider.SuUserRoleQuery, "FindSuUserRoleSearchResult", new object[] { criteria, userID, languageID }, firstResult, maxResult, sortExpression);
		//}
		//public ISQLQuery FindSuUserRoleSearchResult(SuUserRoleSearchResult userRoleSearchResult, long userID, short languageID)
		//{
		//    StringBuilder sqlBuilder = new StringBuilder();
		//    sqlBuilder.AppendLine("	SELECT ur.ID as UserRoleId, ur.UserID as UserId, ur.RoleId as RoleId, rl.RoleName as RoleName, ur.Comment as Comment, ur.Active as Active ");
		//    sqlBuilder.AppendLine(" FROM SuUserRole ur INNER JOIN SuRole r on r.RoleID = ur.RoleID ");
		//    sqlBuilder.AppendLine(" INNER JOIN SuRoleLang rl on rl.RoleID = r.RoleID and rl.LanguageID = :languageID ");
		//    sqlBuilder.AppendLine(" WHERE ur.UserID = :userID ");

		//    QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
		//    parameterBuilder.AddParameterData("languageID", typeof(short), languageID);
		//    parameterBuilder.AddParameterData("userID", typeof(long), userID);

		//    ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
		//    parameterBuilder.FillParameters(query);
		//    query.AddScalar("UserRoleId", NHibernateUtil.Int64)
		//        .AddScalar("UserId", NHibernateUtil.Int64)
		//        .AddScalar("RoleId", NHibernateUtil.Int16)
		//        .AddScalar("RoleName", NHibernateUtil.String)
		//        .AddScalar("Comment", NHibernateUtil.String)
		//        .AddScalar("Active", NHibernateUtil.Boolean);
		//    query.SetResultTransformer(Transformers.AliasToBean(typeof(SuUserRoleSearchResult)));

		//    return query;
		//}
		//public int FindCountSuUserRoleSearchResult(SuUserRoleSearchResult userRoleSearchResult, long userID, short languageID)
		//{
		//    StringBuilder sqlBuilder = new StringBuilder();
		//    sqlBuilder.AppendLine("	SELECT Count(*) as Count ");
		//    sqlBuilder.AppendLine(" FROM SuUserRole ur INNER JOIN SuRole r on r.RoleID = ur.RoleID ");
		//    sqlBuilder.AppendLine(" INNER JOIN SuRoleLang rl on rl.RoleID = r.RoleID and rl.LanguageID = :languageID ");
		//    sqlBuilder.AppendLine(" WHERE ur.UserID = :userID ");

		//    ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
		//    query.SetInt64("userID", userID);
		//    query.SetInt16("languageID", languageID);
		//    query.AddScalar("Count", NHibernateUtil.Int32);
			
		//    return Convert.ToInt32(query.UniqueResult());
		//}
		#endregion

		#region ISuUserRoleQuery Members
		public IList<SuUserRoleSearchResult> GetSuUserRoleSearchResultList(long userID, short languageID, int firstResult, int maxResult, string sortExpression)
		{
			return NHibernateQueryHelper.FindPagingByCriteria<SuUserRoleSearchResult>(QueryProvider.SuUserRoleQuery, "FindSuUserRoleSearchResult", new object[] { userID, languageID, sortExpression, false }, firstResult, maxResult, sortExpression);
		}
		public ISQLQuery FindSuUserRoleSearchResult(long userID, short languageID, string sortExpression, bool isCount)
		{
			StringBuilder sqlBuilder = new StringBuilder();
			ISQLQuery query;

			if (isCount)
			{
				sqlBuilder.AppendLine("	SELECT Count(*) as Count ");
				sqlBuilder.AppendLine(" FROM SuUserRole ur INNER JOIN SuRole r on r.RoleID = ur.RoleID ");
				sqlBuilder.AppendLine(" INNER JOIN SuRoleLang rl on rl.RoleID = r.RoleID and rl.LanguageID = :languageID ");
				sqlBuilder.AppendLine(" WHERE ur.UserID = :userID ");

				query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
				query.SetInt64("userID", userID);
				query.SetInt16("languageID", languageID);
				query.AddScalar("Count", NHibernateUtil.Int32);
			}
			else
			{
				sqlBuilder.AppendLine("	SELECT ur.ID as UserRoleId, ur.UserID as UserId, ur.RoleId as RoleId, rl.RoleName as RoleName, ur.Comment as Comment, ur.Active as Active ");
				sqlBuilder.AppendLine(" FROM SuUserRole ur INNER JOIN SuRole r on r.RoleID = ur.RoleID ");
				sqlBuilder.AppendLine(" INNER JOIN SuRoleLang rl on rl.RoleID = r.RoleID and rl.LanguageID = :languageID ");
				sqlBuilder.AppendLine(" WHERE ur.UserID = :userID ");

				if (string.IsNullOrEmpty(sortExpression))
				{
					sqlBuilder.AppendLine(" ORDER BY ur.ID, ur.UserID, ur.RoleId, rl.RoleName, ur.Active ");
				}
				else
				{
					sqlBuilder.AppendLine(" ORDER BY " + sortExpression);
				}

				query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
				query.SetInt64("userID", userID);
				query.SetInt16("languageID", languageID);
				query.AddScalar("UserRoleId", NHibernateUtil.Int64)
					.AddScalar("UserId", NHibernateUtil.Int64)
					.AddScalar("RoleId", NHibernateUtil.Int16)
					.AddScalar("RoleName", NHibernateUtil.String)
					.AddScalar("Comment", NHibernateUtil.String)
					.AddScalar("Active", NHibernateUtil.Boolean);
				query.SetResultTransformer(Transformers.AliasToBean(typeof(SuUserRoleSearchResult)));
			}
			
			return query;
		}
		public int GetCountSuUserRoleSearchResult(long userID, short languageID)
		{
			return NHibernateQueryHelper.CountByCriteria(QueryProvider.SuUserRoleQuery, "FindSuUserRoleSearchResult", new object[] { userID, languageID, string.Empty, true });
		}
		#endregion



        #region Get Role


        public IList<SuRole> GetRoleReceiveDocument()
        {
            return GetCurrentSession().CreateQuery("from SuRole where Active = '1' and ReceiveDocument = '1'")
               .List<SuRole>();
        }

        public IList<SuRole> GetRoleVerifyDocument()
        {
            return GetCurrentSession().CreateQuery("from SuRole where Active = '1' and VerifyDocument = '1'")
               .List<SuRole>();
        }

        public IList<SuRole> GetRoleApproveVerifyDocument()
        {
            return GetCurrentSession().CreateQuery("from SuRole where Active = '1' and ApproveVerifyDocument = '1'")
               .List<SuRole>();
        }

        public IList<SuRole> GetRoleVerifyAndApproveVerifyDocument()
        {
            return GetCurrentSession().CreateQuery("from SuRole where Active = '1' and VerifyDocument = '1' and ApproveVerifyDocument = '1'")
               .List<SuRole>();
        }

        public IList<SuRole> GetRoleVerifyPayment()
        {
            return GetCurrentSession().CreateQuery("from SuRole where Active = '1' and VerifyPayment = '1'")
               .List<SuRole>();
        }

        public IList<SuRole> GetRoleApproveVerifyPayment()
        {
            return GetCurrentSession().CreateQuery("from SuRole where Active = '1' and ApproveVerifyPayment = '1'")
               .List<SuRole>();
        }

        public IList<SuRole> GetRoleVerifyAndApproveVerifyPayment()
        {
            return GetCurrentSession().CreateQuery("from SuRole where Active = '1' and VerifyPayment = '1' and ApproveVerifyPayment = '1'")
               .List<SuRole>();
        }

        public IList<SuRole> GetRoleCounterCashier()
        {
            return GetCurrentSession().CreateQuery("from SuRole where Active = '1' and CounterCashier = '1'")
               .List<SuRole>();
        }

        #endregion

        public IList<short> GetUserRoleServiceTeamByUserID(long userID)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();

            sqlBuilder.AppendLine(" SELECT sr.RoleID ");
            sqlBuilder.AppendLine(" FROM SuRole sr ");
            sqlBuilder.AppendLine(" INNER JOIN SuUserRole sur ");
            sqlBuilder.AppendLine(" ON sr.RoleID = sur.RoleID ");
            sqlBuilder.AppendLine(" AND sur.UserID = :UserID ");
            sqlBuilder.AppendLine(" WHERE sr.VerifyDocument = 1 or sr.ReceiveDocument = 1 or sr.ApproveVerifyDocument = 1 ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());

            parameterBuilder.FillParameters(query);
            query.SetInt64("UserID", userID);
            query.AddScalar("RoleID", NHibernateUtil.Int16);

            return query.List<short>();
        }

        public IList<short> GetUserRolePBByUserID(long userID)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();

            sqlBuilder.AppendLine(" SELECT sr.RoleID ");
            sqlBuilder.AppendLine(" FROM SuRole sr ");
            sqlBuilder.AppendLine(" INNER JOIN SuUserRole sur ");
            sqlBuilder.AppendLine(" ON sr.RoleID = sur.RoleID ");
            sqlBuilder.AppendLine(" AND sur.UserID = :UserID ");
            sqlBuilder.AppendLine(" WHERE sr.ApproveVerifyPayment = 1 or sr.VerifyPayment = 1 or sr.CounterCashier = 1 ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());

            parameterBuilder.FillParameters(query);
            query.SetInt64("UserID", userID);
            query.AddScalar("RoleID", NHibernateUtil.Int16);

            return query.List<short>();
        }
    }
}
