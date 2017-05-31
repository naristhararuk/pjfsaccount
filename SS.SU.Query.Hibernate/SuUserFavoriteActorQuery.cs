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
using NHibernate.Expression;

using SS.Standard.Security;

namespace SS.SU.Query.Hibernate 
{
    public class SuUserFavoriteActorQuery : NHibernateQueryBase<SuUserFavoriteActor, long>, ISuUserFavoriteActorQuery
    {
        public IList<SuUserFavoriteActor> FindApproverByUserId(long userID)
        {
            //StringBuilder sqlBuilder = new StringBuilder();
            //sqlBuilder.AppendLine("FROM SuUserFavoriteActor as uf ");
            //sqlBuilder.AppendLine(" WHERE uf.User.Userid = :UserId and uf.ActorType = 1 ");   //ActorType 1 = Approver 

            //IQuery query = GetCurrentSession().CreateQuery(sqlBuilder.ToString());
            //query.SetInt64("UserId", userID);

            //return query.List<SuUserFavoriteActor>();

            IList<SuUserFavoriteActor> list = GetCurrentSession()
                .CreateQuery("FROM SuUserFavoriteActor u where u.User.Userid = :UserId and u.ActorType = 1")
                .SetInt64("UserId", userID).List<SuUserFavoriteActor>();
            return list;

        }
        public IList<SuUserFavoriteActor> FindInitiatorByUserId(long userID)
        {
            //StringBuilder sqlBuilder = new StringBuilder();
            //sqlBuilder.AppendLine("FROM SuUserFavoriteActor as uf ");
            //sqlBuilder.AppendLine(" WHERE uf.User.Userid = :UserId and uf.ActorType = 1 ");   //ActorType 1 = Approver 

            //IQuery query = GetCurrentSession().CreateQuery(sqlBuilder.ToString());
            //query.SetInt64("UserId", userID);

            //return query.List<SuUserFavoriteActor>();

            IList<SuUserFavoriteActor> list = GetCurrentSession()
                .CreateQuery("FROM SuUserFavoriteActor u where u.User.Userid = :UserId and u.ActorType = 2")
                .SetInt64("UserId", userID).List<SuUserFavoriteActor>();
            return list;

        }
        public SuUserFavoriteActor FindUserFavoriteByFavoriteId(long fid)
        {

            ISQLQuery query = GetCurrentSession().CreateSQLQuery("SELECT  *  FROM  SuUserFavoriteActor  WHERE [UserFavoriteActorID] = :fid ");
            query.SetInt64("fid", fid);
            query.AddEntity(typeof(SuUserFavoriteActor));
            IList<SuUserFavoriteActor> user = query.List<SuUserFavoriteActor>();
            if (user.Count > 0)
            {
                return user[0];
            }
            else
            {
                return null;
            }

            
        }

        public IList<SuUserFavoriteActor> FindUserFavoriteByRequesterId(long requesterId, string actorType)
        {
            IList<SuUserFavoriteActor> list = GetCurrentSession()
                .CreateQuery("FROM SuUserFavoriteActor u where u.User.Userid = :UserId and u.ActorType = :ActorType and Active= 1")
                .SetInt64("UserId", requesterId)
                .SetString("ActorType", actorType)
                .List<SuUserFavoriteActor>();
            return list;
        }

        

        //#region IQuery<SuUserFavoriteActor,short> Members


        //public SuUserFavoriteActor FindByIdentity(short id)
        //{
        //    throw new NotImplementedException();
        //}

        //public SuUserFavoriteActor FindProxyByIdentity(short id)
        //{
        //    throw new NotImplementedException();
        //}

        //#endregion

        
    }
}
