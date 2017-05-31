using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SCG.eAccounting.Web.Helper;
using SS.SU.DTO;
using SS.SU.Query;

namespace SCG.eAccounting.Web.UserControls.LOV.SCG.DB
{
    public partial class UserAutoCompleteLookup : BaseUserControl, IEditorUserControl
    {
        private SuUser userInfo;
        public SuUser UserInfo
        {
            get { return userInfo; }
            set { userInfo = value; }
        }

        /// <summary>
        /// UserID is user login
        /// </summary>
        public string UserID
        {
            get { return ctlUserTextBoxAutoComplete.UserID; }
            set { ctlUserTextBoxAutoComplete.UserID = value; }
        }

        /// <summary>
        /// EmployeeID is user id 
        /// </summary>
        public string EmployeeID
        {
            get { return ctlExpID.Value; }
            set { this.ctlExpID.Value = value; }
        }
        public string UserNameID
        {
            set { this.SetControl(UIHelper.ParseLong(value)); }
        }
        public string EmployeeName
        {
            get { return ctlUserTextBoxAutoComplete.EmployeeName; }
            set { ctlUserTextBoxAutoComplete.EmployeeName = value; }
        }
        public bool DisplayCaption
        {
            set
            {
                if (value)
                    ctlUserName.Style.Add("display", "inline-block");
                else
                    ctlUserName.Style.Add("display", "none");
            }
        }
        public bool IsApprovalFlag
        {
            get
            {
                if (ViewState["IsApprovalFlag"] != null)
                    return (bool)ViewState["IsApprovalFlag"];
                return false;
            }
            set
            {
                ViewState["IsApprovalFlag"] = value;
                ctlUserTextBoxAutoComplete.IsApprovalFlag = value;
            }
        }
        public bool Display
        {
            set
            {
                if (value)
                {
                    ctlContainer.Style["display"] = "inline-block";
                }
                else
                {
                    ctlContainer.Style["display"] = "none";
                }
            }
        }
        public string Text
        {
            get { return ctlUserTextBoxAutoComplete.UserID; }
        }

        public bool? IsActive
        {
            get
            {
                if (ViewState["IsActive"] != null)
                    return (bool)ViewState["IsActive"];
                return null;
            }
            set
            {
                ViewState["IsActive"] = value;
                ctlUserTextBoxAutoComplete.IsActive = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BaseUserControl ctl = GetCurrentPopUpControl();

            if (ctl is UserProfileLookup)
            {
                ctl.OnPopUpReturn += new PopUpReturnEventHandler(ctlUserProfileLookup_OnNotifyPopup);
            }
            CallOnObjectLookUpCalling();
        }
        #region Lookup Event
        
        protected void ctlUserProfileLookup_OnNotifyPopup(object sender, PopUpReturnArgs args)
        {
            if (args.Type.Equals(PopUpReturnType.OK))
            {
                userInfo = (SuUser)args.Data;
                if (userInfo != null)
                {
                    ctlUserTextBoxAutoComplete.UserID = userInfo.UserName;
                    EmployeeID = userInfo.Userid.ToString();
                    ctlUserName.Text = userInfo.EmployeeName;
                    CallOnObjectLookUpReturn(userInfo);
                }
                ctlUpdatePanelUser.Update();
            }
        }
        #endregion
        protected void ctlUserTextBoxAutoComplete_NotifyPopupResult(object sender, string action, string result)
        {
            if (action.Equals("select"))
            {
                userInfo = QueryProvider.SuUserQuery.FindByIdentity(UIHelper.ParseLong(result));
                EmployeeID = userInfo.Userid.ToString();
                ctlUserName.Text = userInfo.EmployeeName;
            }
            else if (action.Equals("textchanged"))
            {
                if (string.IsNullOrEmpty(result))
                {
                    ResetValue();
                }
                else
                {
                    userInfo = QueryProvider.SuUserQuery.FindUserByUserName(result,IsApprovalFlag);
                    if (userInfo != null)
                    {
                        EmployeeID = userInfo.Userid.ToString();
                        ctlUserName.Text = userInfo.EmployeeName;
                    }
                }
            }
            CallOnObjectLookUpReturn(userInfo);
            ctlUpdatePanelUser.Update();
        }
        protected void ctlSearch_Click(object sender, ImageClickEventArgs e)
        {
            UserProfileLookup ctlUserProfileLookup = LoadPopup<UserProfileLookup>("~/UserControls/LOV/SCG.DB/UserProfileLookup.ascx", ctlPopUpUpdatePanel);
            ctlUserProfileLookup.SearchApprovalFlag = IsApprovalFlag;
            ctlUserProfileLookup.Show();
        }
        public void ResetValue()
        {
            ctlUserTextBoxAutoComplete.UserID = string.Empty;
            ctlUserTextBoxAutoComplete.EmployeeID = string.Empty;
            ctlUserTextBoxAutoComplete.IsApprovalFlag = false;
            ctlExpID.Value = string.Empty;
            ctlUpdatePanelUser.Update();
        }
        public void SetControl(long userID)
        {
            SuUser userInfo = QueryProvider.SuUserQuery.FindByIdentity(userID);
            if (userInfo != null)
            {
                ctlUserTextBoxAutoComplete.UserID = userInfo.UserName;
            }
        }

        public long? RequesterID
        {
            get
            {
                if (ViewState["RequesterID"] != null)
                    return UIHelper.ParseLong(ViewState["RequesterID"].ToString());
                return null;
            }
            set { ViewState["RequesterID"] = value; }
        }

        public void ShowDefaultApprover(long requesterId)
        {
            SuUser requester = QueryProvider.SuUserQuery.FindByIdentity(requesterId);
            if (requester != null && requester.Supervisor > 0)
            {
                RequesterID = requester.Userid;
                this.SetControl(requester.Supervisor);
            }
            else
            {
                this.ResetValue();
            }
        }
    }
}