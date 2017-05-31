using System;
using SS.Standard.Data.NHibernate.Service;
using SS.Standard.Data.NHibernate.Dao;
using SCG.DB.DTO;


namespace SCG.DB.BLL
{
    public interface IDbProfileListService : IService<DbProfileList, Guid>
    {
         void AddProfileList(DbProfileList ProfileList, long UserAccount);
         void UpdateProfileList(DbProfileList ProfileList, long UserAccount);
         void RemoveProfileList(DbProfileList ProfileList);
    }
}
