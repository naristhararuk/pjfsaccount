using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;

using SS.SU.DTO;
using SS.SU.BLL;
using SS.SU.DAL;
using SS.SU.DTO.ValueObject; 

namespace SS.SU.BLL.Implement
{
    public class SuUserFavoriteActorService : ServiceBase<SuUserFavoriteActor, long>, ISuUserFavoriteActorService
    {
        public override IDao<SuUserFavoriteActor, long> GetBaseDao()
        {
            return DaoProvider.SuUserFavoriteActorDao;
           
        }
        public long AddFavoriteApprover(SuUserFavoriteActor user)
        {
            long id = 0;
            id = DaoProvider.SuUserFavoriteActorDao.Save(user);
            return id;
        }
        public long AddFavoriteInitiator(SuUserFavoriteActor user)
        {
            long id = 0;
            id = DaoProvider.SuUserFavoriteActorDao.Save(user);
            return id;
        }
        public void DeleteFavorite(SuUserFavoriteActor favorite)
        {
            DaoProvider.SuUserFavoriteActorDao.Delete(favorite);
        }
    }
}
