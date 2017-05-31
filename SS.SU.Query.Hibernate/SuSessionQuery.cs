using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.QueryDao;

using SS.SU.DTO;
using SS.SU.Query;

using NHibernate;
using NHibernate.Transform;
using NHibernate.Expression;

using SS.Standard.Security;

namespace SS.SU.Query.Hibernate
{
    public class SuSessionQuery : NHibernateQueryBase<SuSession, long>, ISuSessionQuery
    {
        #region ISuUserQuery Members
        private static string strDeleteSessionTimeOut = "SELECT * FROM SuSession WHERE [TimeStamp] < :TimeStamp ";
        private static string strGetSuSession = "SELECT * FROM SuSession WHERE UserID = :UserID";

        public IList<SuSession> GetUserSessionTimeOutList(DateTime TimeStamp)
        {
            //  string strStatement = string.Format(strDeleteSessionTimeOut, TimeStamp);
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(strDeleteSessionTimeOut);
            query.SetTimestamp("TimeStamp", TimeStamp);
            query.AddEntity(typeof(SuSession));
            IList<SuSession> userSession = query.List<SuSession>();
            if (userSession.Count > 0)
            {
                return userSession;
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region ISuUserQuery Members
        public SuSession GetUserSession(long userID)
        {
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(strGetSuSession);
            query.SetInt64("UserID", userID);
            query.AddEntity(typeof(SuSession));
            IList<SuSession> suSession = query.List<SuSession>();
            return suSession.Count > 0 ? suSession[0] : null;
        }
        #endregion

       
    }
}
