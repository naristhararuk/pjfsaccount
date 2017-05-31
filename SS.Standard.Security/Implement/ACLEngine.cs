using System;

using System.Collections;

using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml;

using SS.SU.BLL;
using SS.SU.Query;
using SS.SU.DTO;
using SS.SU.Helper;

namespace SS.Standard.Security.Implement
{
    [Serializable]
    public partial class ACLEngine : IACLEngine
    {
        public IUserAccount UserAccount { get; set; }
        public IUserEngine UserEngine { get; set; }
        public IMenuEngine MenuEngine { get; set; }
        public ISuUserRoleService SuUserRoleService { get; set; }
        public ISuProgramRoleQuery SuProgramRoleQuery { get; set; }

        private IList<SuUserRole> AllDataRoles;
        private IList<SuProgramRole> AllDataProgramRoles;

        public void UpdateDataACL()
        {
            GetServiceDataProgramRole();

        }
        public void UpdateDataUserRoles()
        {
            GetServiceDataRoles();

        }
        public void UpdateDataUserTransactionPermission()
        { 
        
        }
        public bool Permission(ACL acl, string ProgramCode, bool Redirect)
        {
            List<SuProgramRole> ProgramRole = GetDataProgramRole().ToList<SuProgramRole>();
            List<UserRoles> PersonRoles     = UserAccount.UserRole;
           // List<UserMenu> Menus            = MenuEngine.GetDataMenu();

            short[] arrayUserRole = new short[PersonRoles.Count];
            ArrayList arrayRoleID = new ArrayList();
            for (int i = 0; i < PersonRoles.Count; i++)
            {
               arrayRoleID.Add(PersonRoles[i].RoleID);
            }


            //var ProgramPermission = from p in ProgramRole
            //                        where arrayUserRole.Contains(p.Role.Roleid) && p.Active == true && p.Role.Active==true
            //                        select p;


            IList<SuProgramRole> ProgramPermission = SuProgramRoleQuery.FindProgramPermission(arrayRoleID);

            bool flag=false;
            switch (acl)
            {
                case ACL.View:
                    foreach (SuProgramRole item in ProgramPermission)
                    {
                        if (item.Program.ProgramCode == ProgramCode)
                        {
                            flag = item.DisplayState;
                            break;
                        }
                    }
                    break;
                case ACL.Add:
                    foreach (SuProgramRole item in ProgramPermission)
                    {
                        if (item.Program.ProgramCode == ProgramCode)
                        {
                            flag = item.AddState;
                            break;
                        }
                    }
                    break;
                case ACL.Edit:
                    foreach (SuProgramRole item in ProgramPermission)
                    {
                        if (item.Program.ProgramCode == ProgramCode)
                        {
                            flag = item.EditState;
                            break;
                        }
                    }
                    break;
                case ACL.Delete:
                    foreach (SuProgramRole item in ProgramPermission)
                    {
                        if (item.Program.ProgramCode == ProgramCode)
                        {
                            flag = item.DeleteState;
                            break;
                        }
                    }
                    break;
                default:
                    break;
            }

            //SessionTimeOut()
            //SetSession()

            return flag;
        }
        private IList<SuUserRole> GetDataUserRole()
        {

            if (HttpContext.Current.Application[ApplicationEnum.WebApplication.DataRoles.ToString()] != null)
                {
                    AllDataRoles = (List<SuUserRole>)HttpContext.Current.Application[ApplicationEnum.WebApplication.DataRoles.ToString()];
                }
                else
                {
                    GetServiceDataRoles();

                }
            return AllDataRoles;
        }

        private void GetServiceDataRoles()
        {
            AllDataRoles = SuUserRoleService.FindAll();
            HttpContext.Current.Application[ApplicationEnum.WebApplication.DataRoles.ToString()] = AllDataRoles;
        }


        private IList<SuProgramRole> GetDataProgramRole()
        {
            if (HttpContext.Current.Application[ApplicationEnum.WebApplication.DataRoles.ToString()] != null)
            {
                AllDataProgramRoles = (List<SuProgramRole>)HttpContext.Current.Application[ApplicationEnum.WebApplication.DataRoles.ToString()];
            }
            else
            {
                GetServiceDataProgramRole();

            }
            return AllDataProgramRoles;

        }

        private void GetServiceDataProgramRole()
        {
            AllDataProgramRoles = SuProgramRoleQuery.FindAll();
            HttpContext.Current.Application[ApplicationEnum.WebApplication.DataRoles.ToString()] = AllDataRoles;
        }
    }
    


    public enum ACL
    {
        View,
        Add,
        Edit,
        Delete
    }
}
