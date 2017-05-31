using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service;
using System.Web.UI.WebControls;
using SS.SU.DTO;
using SS.SU.DTO.ValueObject;

namespace SS.SU.BLL
{
    public interface ITmpSuUserService : IService<TmpSuUser, long>
    {
        void AddUser(TmpSuUser tmpSuUser);
        void AddUserList(List<TmpSuUser> tmpUserList);
        long FindCostCenter(string costCenter);
        //long FindSuperVisor(string userName);
        long FindLocation(string location);
        IList<NewUserEmail> FindNewUser();
        void BeforeCommit();
        void CommitImport(int byId, int RoleId);
        void DeleteAll();
        void AfterCommit(int byId);
        void CommitManualImport(int byId, int RoleId);
    }
}
