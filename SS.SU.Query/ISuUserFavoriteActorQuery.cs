using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;
using SS.SU.DTO;
using SS.Standard.Data.Query.NHibernate;
using NHibernate;

namespace SS.SU.Query
{
    public interface ISuUserFavoriteActorQuery : IQuery<SuUserFavoriteActor, long>
    {
        IList<SuUserFavoriteActor> FindApproverByUserId(long userID);
        IList<SuUserFavoriteActor> FindInitiatorByUserId(long userID);
        SuUserFavoriteActor FindUserFavoriteByFavoriteId(long fid);       
        IList<SuUserFavoriteActor> FindUserFavoriteByRequesterId(long requesterId, string actorType);
    }
}
