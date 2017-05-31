using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;

using SS.SU.DTO;
using SS.SU.DTO.ValueObject;

namespace SS.SU.DAL
{
    public interface ITmpSuUserDao : IDao<TmpSuUser, long>
    {
        void DeleteAll();
        long FindCostCenter(string costCenter);
        //long FindSuperVisor(string userName);
        long FindLocation(string location);
        IList<NewUserEmail> FindNewUser();
        void BeforeCommit();
        void CommitImport(int byId, int RoleId);
        void AfterCommit(int byId);
        void CommitManualImport(int byId, int RoleId);
    }
}
