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
    public interface IDbParameterGroupService : IService<DbParameterGroup, short>
    {
        new IList<DbParameterGroup> FindAll();
        new void Delete(DbParameterGroup domain);
        new DbParameterGroup FindByIdentity(short id);
        new short Save(DbParameterGroup domain);
        new void SaveOrUpdate(DbParameterGroup domain);
        new void Update(DbParameterGroup domain);
        void AddParameterGroup(DbParameterGroup parameterGroup);
        void UpdateParameterGroup(DbParameterGroup parameterGroup);
    }
}