using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

using SS.SU.DTO;

namespace SS.SU.BLL
{
    public interface ISuUserFavoriteActorService : IService<SuUserFavoriteActor, long>
    {
        long AddFavoriteApprover(SuUserFavoriteActor user);
        long AddFavoriteInitiator(SuUserFavoriteActor user);
        void DeleteFavorite(SuUserFavoriteActor favorite);
        
       
    }
}
