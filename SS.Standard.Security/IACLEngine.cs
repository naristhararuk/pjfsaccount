using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SS.Standard.Security
{
    public interface IACLEngine
    {
        void UpdateDataACL();
        void UpdateDataUserRoles();
        void UpdateDataUserTransactionPermission();
        bool Permission(string ProgramCode, bool Redirect);
    }

    public enum ACL
    {
        View,
        Add,
        Edit,
        Delete
    }
}
