using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using SS.Standard.Data.NHibernate.Service;

using SS.DB.DTO;

namespace SS.DB.BLL
{
    public interface IDbLanguageService : IService<DbLanguage, short>
    {
        new IList<DbLanguage> FindAll();
        new void Delete(DbLanguage domain);
        new DbLanguage FindByIdentity(short id);
        new short Save(DbLanguage domain);
        new void SaveOrUpdate(DbLanguage domain);
        new void Update(DbLanguage domain);
        void AddLanguage(DbLanguage language);
        void AddLanguage(DbLanguage language,HttpPostedFile imageFileStream);
        void UpdateLanguage(DbLanguage language);
        void UpdateLanguage(DbLanguage language, HttpPostedFile imageFileStream);
    }
}
