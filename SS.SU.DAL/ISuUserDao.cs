using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;

using SS.SU.DTO;
using SS.SU.DTO.ValueObject;

namespace SS.SU.DAL
{
    public interface ISuUserDao : IDao<SuUser, long>
    {
		IList<SuUser> FindByUserName(string userName);
        IList<VOUserProfile>  FindUserProfileByUserName(string userName);
        void DeleteAllSuUserAndTADocumentInfo(short userID);
        bool FindUserName(long userId, string userName);
        bool IsPrivilege(SuUser user);
        void SyncNewUser();
        void SyncUpdateUser(string userName);
        void SyncDeleteUser(string userName);
    }
}
