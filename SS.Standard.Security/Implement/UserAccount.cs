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
using SS.SU.DTO;
using SS.SU.Helper;
using SS.SU.BLL;
using SS.DB.DTO;

/// <summary>
/// Summary description for User
/// </summary>
namespace SS.Standard.Security.Implement
{
    [Serializable]
    public partial class UserAccount : IUserAccount
    {
        public IUserEngineService UserEngineService { get; set; }
        
        public long UserID
        {
            get
            {
                if (HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()] != null)
                {
                    UserSession user = (UserSession)HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()];
                    return user.UserID;
                }
                else return -1;
            }

        }
        public void SetLanguage(short l)
        {
            DbLanguage language = UserEngineService.GetLanguage(l);

         this.CurrentLanguageID = language.Languageid;
         this.CurrentLanguageName = language.LanguageName;
         this.CurrentLanguageCode = language.LanguageCode;

        //IList<SuUserLang> userlang =  SuUserLangService.FindByUserIdAndLanguageId(UserID, language.Languageid);
        //if (userlang != null && userlang.Count > 0)
        //{
            
        //  // FirstName =  userlang[0].FirstName;
        //  // LastName  =  userlang[0].LastName;
        //    EmployeeName = userlang[0].Comment
        //}


        }
        public string UserName
        {
            get
            {
                if (HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()] != null)
                {
                    UserSession user = (UserSession)HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()];
                    return user.UserName;
                }
                else return null;
            }
        }
        public string EmployeeName
        {
            get
            {
                if (HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()] != null)
                {
                    UserSession user = (UserSession)HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()];
                    return user.EmployeeName;
                }
                else return null;
            }
            set
            {
                if (HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()] != null)
                {
                    ((UserSession)(HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()])).EmployeeName = value;

                }
            }
        }
        public List<UserRoles> UserRole
        {
            get
            {
                if (HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()] != null)
                {
                    UserSession user = (UserSession)HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()];
                    return user.UserRole;
                }
                else return null;
            }
            set
            {

                if (HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()] != null)
                {
                    ((UserSession)(HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()])).UserRole = value;

                }
            }
        }
        public List<SuUserTransactionPermission> UserTransactionPermission
        {
            get
            {
                if (HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()] != null)
                {
                    UserSession user = (UserSession)HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()];
                    return user.UserTransactionPermission;
                }
                else return null;
            }
            set
            {

                if (HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()] != null)
                {
                    ((UserSession)(HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()])).UserTransactionPermission = value;

                }
            }
        }
        public string SessionID
        {
            get
            {
                if (HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()] != null)
                {
                    UserSession user = (UserSession)HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()];
                    return user.SessionID;
                }
                else return null;
            }
        }
        public bool Authentication
        {
            get
            {
                if (HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()] != null)
                {
                    UserSession user = (UserSession)HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()];
                    if (user != null && user.UserID > 0) return true;
                    else return false;
                }
                else return false;
            }
        }
        public short UserLanguageID
        {
            get
            {
                if (HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()] != null)
                {
                    UserSession user = (UserSession)HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()];
                    return user.UserLanguageID;
                }
                else return -1;
            }
            set
            {
                if (HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()] != null)
                {
                    UserSession user = (UserSession)HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()];
                    user.UserLanguageID = value;
                }
            }
        }
        public string UserLanguageName
        {
            get
            {
                if (HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()] != null)
                {
                    UserSession user = (UserSession)HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()];
                    return user.UserLanguageName;
                }
                else return "";
            }
            set
            {
                if (HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()] != null)
                {
                    UserSession user = (UserSession)HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()];
                    user.UserLanguageName = value;
                }
            }
        }
        public string UserLanguageCode
        {
            get
            {
                if (HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()] != null)
                {
                    UserSession user = (UserSession)HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()];
                    return user.UserLanguageCode;
                }
                else return "";
            }
            set
            {
                if (HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()] != null)
                {
                    UserSession user = (UserSession)HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()];
                    user.UserLanguageCode = value;
                }
            }
        }
        public bool IsVerifier
        {
            get { return true; }
        }
        public bool IsApproveVerifier
        {
            get { return true; }
        }
        public bool IsCashier
        {
            get { return true; }
        }
        public string CurrentProgramCode
        {
            get
            {
                if (HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()] != null)
                {
                    UserSession user = (UserSession)HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()];
                    return user.CurrentProgramCode;
                }
                else return "";
            }
            set
            {
                if (HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()] != null)
                {
                    UserSession user = (UserSession)HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()];
                    user.CurrentProgramCode = value;
                }
            }
        }
        public short CurrentLanguageID
        {
            get
            {
                if (HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()] != null)
                {
                    UserSession user = (UserSession)HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()];
                    return user.CurrentUserLanguageID;
                }
                if (HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()+"_LanguageID"] == null)
                    HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()+"_LanguageID"] = SS.DB.Query.ParameterServices.DefaultLanguage;

                return (short)HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString() + "_LanguageID"];
            }
            set
            {
                if (HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()] != null)
                {
                    UserSession user = (UserSession)HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()];
                    user.CurrentUserLanguageID = value;
                }
                else
                    HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString() + "_LanguageID"] = value;
            }
        }
        public string CurrentLanguageCode
        {
            get
            {
                if (HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()] != null)
                {
                    UserSession user = (UserSession)HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()];
                    return user.CurrentUserLanguageCode;
                }
                else return null;
            }
            set
            {
                if (HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()] != null)
                {
                    UserSession user = (UserSession)HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()];
                    user.CurrentUserLanguageCode = value;
                }
            }
        }
        public string CurrentLanguageName
        {
            get
            {
                if (HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()] != null)
                {
                    UserSession user = (UserSession)HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()];
                    return user.CurrentUserLanguageName;
                }
                else return string.Empty;
            }
            set
            {
                if (HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()] != null)
                {
                    UserSession user = (UserSession)HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()];
                    user.CurrentUserLanguageName = value;
                }
            }
        }

        public bool IsReceiveDocument
        {
            get
            {
                if (HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()] != null)
                {
                    UserSession user = (UserSession)HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()];
                    return user.IsReceiveDocument;
                }
                else return false;
            }
        }

        public bool IsVerifyDocument
        {
            get
            {
                if (HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()] != null)
                {
                    UserSession user = (UserSession)HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()];
                    return user.IsVerifyDocument;
                }
                else return false;
            }
        }

        public bool IsVerifyPayment
        {
            get
            {
                if (HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()] != null)
                {
                    UserSession user = (UserSession)HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()];
                    return user.IsVerifyPayment;
                }
                else return false;
            }
        }

        public bool IsCounterCashier
        {
            get
            {
                if (HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()] != null)
                {
                    UserSession user = (UserSession)HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()];
                    return user.IsCounterCashier;
                }
                else return false;
            }
        }

        public bool IsApproveVerifyPayment
        {
            get
            {
                if (HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()] != null)
                {
                    UserSession user = (UserSession)HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()];
                    return user.IsApproveVerifyPayment;
                }
                else return false;
            }
        }

        public bool IsApproveVerifyDocument
        {
            get
            {
                if (HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()] != null)
                {
                    UserSession user = (UserSession)HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()];
                    return user.IsApproveVerifyDocument;
                }
                else return false;
            }
        }

        public bool IsAccountant
        {
            get
            {
                if (HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()] != null)
                {
                    UserSession user = (UserSession)HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()];
                    return user.IsReceiveDocument || user.IsVerifyDocument | user.IsApproveVerifyDocument;
                }
                else return false;
            }
        }

        public bool IsPayment
        {
            get
            {
                if (HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()] != null)
                {
                    UserSession user = (UserSession)HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()];
                    return user.IsVerifyPayment || user.IsApproveVerifyPayment || user.IsCounterCashier;
                }
                else return false;
            }
        }
    }
}
