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

using System.Threading;

using SS.Standard.Security;

/// <summary>
/// Summary description for User
/// </summary>
namespace SS.Standard.Security.Implement
{
    [Serializable]
    public partial class UserAccount : IUserAccount
    {
        public IUserEngineService UserEngineService { get; set; }

        UserSession GetUserSession()
        {
            LocalDataStoreSlot slot = Thread.GetNamedDataSlot(SessionEnum.WebSession.UserProfiles.ToString());
            if (slot == null) slot = Thread.AllocateNamedDataSlot(SessionEnum.WebSession.UserProfiles.ToString());
            if (Thread.GetData(slot) == null)
            {
                UserSession session = new UserSession();
                session.CurrentUserLanguageID = 1;
                Thread.SetData(slot, new UserSession());
            }
            return (UserSession)Thread.GetData(slot);
        }

        public void SetLanguage(short l)
        {
            DbLanguage language = UserEngineService.GetLanguage(l);

            this.CurrentLanguageID = language.Languageid;
            this.CurrentLanguageName = language.LanguageName;
            this.CurrentLanguageCode = language.LanguageCode;
        }

        public virtual bool Authentication
        {
            get { return GetUserSession().IsAuthenticated; }
            set { GetUserSession().IsAuthenticated = value; }
        }

        public virtual long UserID
        {
            get { return GetUserSession().UserID; }
            set { GetUserSession().UserID = value; }
        }

        public virtual string UserName
        {
            get { return GetUserSession().UserName; }
            set { GetUserSession().UserName = value; }
        }

        public virtual string EmployeeName
        {
            get { return GetUserSession().EmployeeName; }
            set { GetUserSession().EmployeeName = value; }
        }

        public virtual string CurrentProgramCode
        {
            get { return GetUserSession().CurrentProgramCode; }
            set { GetUserSession().CurrentProgramCode = value; }
        }

        public virtual short CurrentLanguageID
        {
            get { return GetUserSession().CurrentUserLanguageID; }
            set { GetUserSession().CurrentUserLanguageID = value; }
        }

        public virtual string CurrentLanguageName
        {
            get { return GetUserSession().CurrentUserLanguageName; }
            set { GetUserSession().CurrentUserLanguageName = value; }
        }

        public virtual string CurrentLanguageCode
        {
            get { return GetUserSession().CurrentUserLanguageCode; }
            set { GetUserSession().CurrentUserLanguageCode = value; }
        }

        public virtual short UserLanguageID
        {
            get { return GetUserSession().UserLanguageID; }
            set { GetUserSession().UserLanguageID = value; }
        }

        public virtual string UserLanguageName
        {
            get { return GetUserSession().UserLanguageName; }
            set { GetUserSession().UserLanguageName = value; }
        }

        public virtual string UserLanguageCode
        {
            get { return GetUserSession().UserLanguageCode; }
            set { GetUserSession().UserLanguageCode = value; }
        }

        public virtual string SessionID
        {
            get { return GetUserSession().SessionID; }
            set { GetUserSession().SessionID = value; }
        }

        public virtual List<UserRoles> UserRole
        {
            get { return GetUserSession().UserRole; }
            set { GetUserSession().UserRole = value; }
        }

        //public virtual List<SuUserTransactionPermission> UserTransactionPermission
        //{
        //    get { return GetUserSession().UserTransactionPermission; }
        //    set { GetUserSession().UserTransactionPermission = value; }
        //}

        public virtual bool IsReceiveDocument
        {
            get { return GetUserSession().IsReceiveDocument; }
            set { GetUserSession().IsReceiveDocument = value; }
        }

        public virtual bool IsVerifyDocument
        {
            get { return GetUserSession().IsVerifyDocument; }
            set { GetUserSession().IsVerifyDocument = value; }
        }

        public virtual bool IsVerifyPayment
        {
            get { return GetUserSession().IsVerifyPayment; }
            set { GetUserSession().IsVerifyPayment = value; }
        }

        public virtual bool IsCounterCashier
        {
            get { return GetUserSession().IsCounterCashier; }
            set { GetUserSession().IsCounterCashier = value; }
        }

        public virtual bool IsApproveVerifyPayment
        {
            get { return GetUserSession().IsApproveVerifyPayment; }
            set { GetUserSession().IsApproveVerifyPayment = value; }
        }

        public virtual bool IsApproveVerifyDocument
        {
            get { return GetUserSession().IsApproveVerifyDocument; }
            set { GetUserSession().IsApproveVerifyDocument = value; }
        }
        public virtual bool IsAccountant
        {
            get { return GetUserSession().IsAccountant; }
            set { GetUserSession().IsAccountant = value; }
        }

        public virtual bool IsPayment
        {
            get { return GetUserSession().IsPayment; }
            set { GetUserSession().IsPayment = value; }
        }

        public virtual bool IsAdmin
        {
            get { return GetUserSession().IsAdmin; }
            set { GetUserSession().IsAdmin = value; }
        }

        public virtual bool IsAllowMultipleApprovePayment
        {
            get { return GetUserSession().IsAllowMultipleApprovePayment; }
            set { GetUserSession().IsAllowMultipleApprovePayment = value; }
        }

        public virtual bool IsAllowMultipleApproveAccountant
        {
            get { return GetUserSession().IsAllowMultipleApproveAccountant; }
            set { GetUserSession().IsAllowMultipleApproveAccountant = value; }
        }

    }
}
