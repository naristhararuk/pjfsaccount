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
    public interface IDbParameterService : IService<DbParameter, short>
    {
        new IList<DbParameter> FindAll();
        new void Delete(DbParameter domain);
        new DbParameter FindByIdentity(short id);
        new short Save(DbParameter domain);
        new void SaveOrUpdate(DbParameter domain);
        new void Update(DbParameter domain);
        void AddParameter(DbParameter parameter);
        void UpdateParameter(DbParameter parameter);
    }
}
