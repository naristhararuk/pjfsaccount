using System;
using System.Web;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using SS.Standard.Data.NHibernate.Service;

using SS.SU.DTO;
using SS.SU.DTO.ValueObject;

namespace SS.SU.BLL
{
    public interface ISuUserService : IService<SuUser, long>
    {
        new IList<SuUser> FindAll();
        new void Delete(SuUser domain);
        new SuUser FindByIdentity(long id);
        new long Save(SuUser domain);
        new void SaveOrUpdate(SuUser domain);
        new void Update(SuUser domain);




        void DeleteUser(SuUser user);
        long AddNewUser(SuUser user, short languageId, HttpPostedFile mapFile);
        long AddNewUser(SuUser user, short languageId);
        void UpdateUser(SuUser user, short languageId, HttpPostedFile mapFile);
        void UpdateUser(SuUser user, short languageId);
        long SaveUserProfile(SuUser user);
        void UpdateUserProfile(SuUser user);
        IList<SuUser> FindByUserName(string userName);
        string Forgetpassword(string UserName);
        bool IsPrivilege(SuUser user);
    }
}
