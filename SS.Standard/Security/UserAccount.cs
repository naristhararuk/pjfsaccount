using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SS.Standard.Data;

/// <summary>
/// Summary description for User
/// </summary>
namespace SS.Standard.Security
{
    
    public class UserAccount
    {

        private UserAccount()
        {
            
        }

        public static int UserID
        {
            get
            {
                if (HttpContext.Current.Session["User"] != null)
                {
                    UserSession user = (UserSession)HttpContext.Current.Session["User"];
                    return user.UserID;
                }
                else return 0;
            }

        }

        public static string UserName
        {
            get
            {
                if (HttpContext.Current.Session["User"] != null)
                {
                    UserSession user = (UserSession)HttpContext.Current.Session["User"];
                    return user.UserName;
                }
                else return null;
            }
        }

        public static string EmployeeName
        {
            get
            {
                if (HttpContext.Current.Session["User"] != null)
                {
                    UserSession user = (UserSession)HttpContext.Current.Session["User"];
                    return user.EmployeeName;
                }
                else return null;
            }
            set {
                if (HttpContext.Current.Session["User"] != null)
                {
                    ((UserSession)(HttpContext.Current.Session["User"])).EmployeeName = value;
                    
                }
            }
        }

        //public static string LastName
        //{
        //    get
        //    {
        //        if (HttpContext.Current.Session["User"] != null)
        //        {
        //            UserSession user = (UserSession)HttpContext.Current.Session["User"];
        //            return user.LastName;
        //        }
        //        else return null;
        //    }
        //    set {

        //        if (HttpContext.Current.Session["User"] != null)
        //        {
        //            ((UserSession)(HttpContext.Current.Session["User"])).LastName = value;

        //        }
        //    }
        //}

        public static int CURRENT_OrganizationID
        {
            get
            {
                if (HttpContext.Current.Session["User"] != null)
                {
                    UserSession user = (UserSession)HttpContext.Current.Session["User"];
                    return user.CURRENT_OrganizationID;
                }
                else return 0;
            }
            set
            {
                if (HttpContext.Current.Session["User"] != null)
                {
                    UserSession user = (UserSession)HttpContext.Current.Session["User"];
                    user.CURRENT_OrganizationID = value;
                }
            }
        }

        public static string CURRENT_OrganizationName
        {
            get
            {
                if (HttpContext.Current.Session["User"] != null)
                {
                    UserSession user = (UserSession)HttpContext.Current.Session["User"];
                    return user.CURRENT_OrganizationName;
                }
                else return string.Empty;
            }
            set
            {
                if (HttpContext.Current.Session["User"] != null)
                {
                    UserSession user = (UserSession)HttpContext.Current.Session["User"];
                    user.CURRENT_OrganizationName = value;
                }
            }
        }

        public static List<UserMenu> UserMenu
        {
            get
            {
                if (HttpContext.Current.Session["User"] != null)
                {
                    UserSession user = (UserSession)HttpContext.Current.Session["User"];
                    return user.UserMenu;
                }
                else return null;
            }
            set
            {
                if (HttpContext.Current.Session["User"] != null)
                {
                    UserSession user = (UserSession)HttpContext.Current.Session["User"];
                    user.UserMenu = value;
                }
            }
        }

        public static int? CURRENT_LanguageID
        {
            get
            {
                if (HttpContext.Current.Session["User"] != null)
                {
                    UserSession user = (UserSession)HttpContext.Current.Session["User"];
                    return user.CURRENT_LanguageID;
                }
                else return null;
            }
            set
            {
                if (HttpContext.Current.Session["User"] != null)
                {
                    UserSession user = (UserSession)HttpContext.Current.Session["User"];
                    user.CURRENT_LanguageID = value;
                }
            }
        }


        public static int LanguageID
        {
            get
            {
                if (HttpContext.Current.Session["User"] != null)
                {
                    UserSession user = (UserSession)HttpContext.Current.Session["User"];
                    return user.LanguageID;
                }
                else return 0;
            }
            set
            {
                if (HttpContext.Current.Session["User"] != null)
                {
                    UserSession user = (UserSession)HttpContext.Current.Session["User"];
                    user.CURRENT_LanguageID = value;
                }
            }
        }

        public static string CURRENT_LANG_NAME
        {
            get
            {
                if (HttpContext.Current.Session["User"] != null)
                {
                    UserSession user = (UserSession)HttpContext.Current.Session["User"];
                    return user.CURRENT_LANG_NAME;
                }
                else return string.Empty;
            }
            set
            {
                if (HttpContext.Current.Session["User"] != null)
                {
                    UserSession user = (UserSession)HttpContext.Current.Session["User"];
                    user.CURRENT_LANG_NAME = value;
                }
            }
        }

        public static string SessionID
        {
            get
            {
                if (HttpContext.Current.Session["User"] != null)
                {
                    UserSession user = (UserSession)HttpContext.Current.Session["User"];
                    return user.SessionID;
                }
                else return null;
            }
        }

        public static bool Authentication
        {
            get
            {
                if (HttpContext.Current.Session["User"] != null)
                {
                    UserSession user = (UserSession)HttpContext.Current.Session["User"];
                    if (user.UserID > 0) return true;
                    else return false;
                }
                else return false;
            }
        }

        public static string[] UserRole
        {
            get
            {
                if (HttpContext.Current.Session["User"] != null)
                {
                    UserSession user = (UserSession)HttpContext.Current.Session["User"];
                    return user.UserRole;
                }
                else return null;
            }
        }
    }
}
