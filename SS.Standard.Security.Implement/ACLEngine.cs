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

using SS.Standard.Security;

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

        public bool Permission(string ProgramCode, bool Redirect)
        {
            if (UserAccount.Authentication)
            {
                IList<SuProgramRole> programRoles =
                        SuProgramRoleQuery.FindByProgramCode_UserID(ProgramCode, UserAccount.UserID);

                if (programRoles.Count > 0) return true;
            }

            string url = HttpContext.Current.Request.Url.ToString();
            if (SS.DB.Query.ParameterServices.EnableSSLOnLoginPage)
                url = url.Replace(Uri.UriSchemeHttp, Uri.UriSchemeHttps);
            if (Redirect) HttpContext.Current.Response.Redirect(url.Replace(HttpContext.Current.Request.RawUrl, System.Web.VirtualPathUtility.ToAbsolute("~/Login.aspx")), true);
            return false;
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
    



}
