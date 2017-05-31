using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SCG.eAccounting.Web.Helper;
using SS.Standard.UI;

using SS.SU.Query;
using SS.SU.DTO;
using SCG.eAccounting.Web.UserControls.LOV.SCG.DB;

namespace SCG.eAccounting.Web.UserControls.LOV.SS.DB
{
    public partial class SupervisorUserField : BaseUserControl, IEditorUserControl
    {
        #region Property

        public bool Display
        {
            set
            {
                if (value)
                    ctlContainer.Style.Add("display", "inline-block");
                else
                    ctlContainer.Style.Add("display", "none");
            }
        }
        public string Text
        {
            get { return ctlUserName.Value + '-' + ctlDescription.Text; }
        }
        public long? UserID
        {
            get
            {
                if (string.IsNullOrEmpty(ctlUserID.Value))
                    return null;
                return UIHelper.ParseLong(ctlUserID.Value);
            }
            set
            {
                if (value.HasValue)
                    ctlUserID.Value = value.Value.ToString();
                else
                    ctlUserID.Value = string.Empty;
            }
        }

        public string UserName
        {
            get { return ctlUserName.Value; }
            set { ctlUserName.Value = value; }
        }

        public void CheckForReadOnly()
        {
            if (!ReadOnly)
            {
                ctlSearch.Visible = true;
                ctlUserTextBoxAutoComplete.SetAutoCompleteNotReadOnly();
            }
            else
            {
                ctlUserTextBoxAutoComplete.SetAutoCompleteReadOnly();
                ctlSearch.Visible = false;
            }
        }
        public bool ReadOnly
        {
            get
            {
                if (ViewState["SupervisorReadOnly"] != null)
                    return (bool)ViewState["SupervisorReadOnly"];
                return false;
            }
            set { ViewState["SupervisorReadOnly"] = value; }
        }
        public bool IsApprovalFlag
        {
            get
            {
                if (ViewState["IsApprovalFlag"] != null)
                    return (bool)ViewState["IsApprovalFlag"];
                return true;
            }
            set
            {
                ViewState["IsApprovalFlag"] = value;
                ctlUserTextBoxAutoComplete.IsApprovalFlag = value;
            }
        }
        #endregion

        #region Page_Load Event
        protected void Page_Load(object sender, EventArgs e)
        {
            BaseUserControl ctl = GetCurrentPopUpControl();
            if (ctl is UserProfileLookup)
            {
                ctl.OnPopUpReturn += new PopUpReturnEventHandler(ctlUserProfileLookup_OnNotifyPopup);
            }
            ctlUserTextBoxAutoComplete.IsApprovalFlag = true;
            ctlUserTextBoxAutoComplete.DataBind();
        }

        #endregion

        protected void ctlUserProfileLookup_OnNotifyPopup(object sender, PopUpReturnArgs args)
        {
            if (args.Type.Equals(PopUpReturnType.OK))
            {
                SuUser user = (SuUser)args.Data;
                UserName = user.UserName.ToString();
                ctlUserTextBoxAutoComplete.UserID = user.Userid;
                UserID = user.Userid;
                ctlUserTextBoxAutoComplete.UserName = user.UserName;
                ctlUserTextBoxAutoComplete.EmployeeName = user.EmployeeName;
                ctlDescription.Text = user.EmployeeName;
                CallOnObjectLookUpReturn(user);
            }
            ctlUpdatePanelUser.Update();
        }

        protected void ctlSearch_Click(object sender, ImageClickEventArgs e)
        {
            UserProfileLookup ctlUserProfileLookup = LoadPopup<UserProfileLookup>("~/UserControls/LOV/SCG.DB/UserProfileLookup.ascx", ctlPopUpUpdatePanel);
            ctlUserProfileLookup.SearchApprovalFlag = true;
            ctlUserProfileLookup.Show();
        }
        protected void ctlAutoUser_NotifyPopupResult(object sender, string action, string result)
        {
            SuUser userInfo = null;
            if (action.Equals("select"))
            {
                userInfo = QueryProvider.SuUserQuery.FindByIdentity(UIHelper.ParseLong(result));
                UserID = userInfo.Userid;
                ctlUserTextBoxAutoComplete.UserName = userInfo.UserName;
                ctlDescription.Text = userInfo.EmployeeName;
            }
            else if (action.Equals("textchanged"))
            {
                if (string.IsNullOrEmpty(result))
                {
                    ResetValue();
                }
                else
                {
                    userInfo = QueryProvider.SuUserQuery.FindUserByUserName(result, IsApprovalFlag);
                    if (userInfo != null)
                    {
                        UserID = userInfo.Userid;
                        ctlUserTextBoxAutoComplete.UserName = userInfo.UserName;
                        ctlDescription.Text = userInfo.EmployeeName;
                    }
                    else
                    {
                        ResetValue();
                    }
                }
            }
            CallOnObjectLookUpReturn(userInfo);
            ctlUpdatePanelUser.Update();
        }

        #region Public Method

        public void ShowDefault()
        {

        }
        public void BindCostCenterControl(long userId)
        {
            SuUser user = QueryProvider.SuUserQuery.FindProxyByIdentity(userId);
            if (user != null)
            {
                ctlUserTextBoxAutoComplete.UserID = user.Userid;
                ctlUserTextBoxAutoComplete.UserName = user.UserName;
                ctlUserTextBoxAutoComplete.EmployeeName = user.EmployeeName;
                ctlDescription.Text = user.EmployeeName;

            }

        }
        public void SetValue(long userId)
        {
            SuUser user = QueryProvider.SuUserQuery.FindByIdentity(userId);
            if (user != null)
            {
                ctlUserName.Value = user.UserName;
                ctlUserID.Value = user.Userid.ToString();
                ctlDescription.Text = user.EmployeeName;
                ctlUserTextBoxAutoComplete.UserID = user.Userid;
                ctlUserTextBoxAutoComplete.UserName = user.UserName;
                ctlUserTextBoxAutoComplete.EmployeeName = user.EmployeeName;
            }

        }
        public void ResetValue()
        {
            ctlUserTextBoxAutoComplete.ResetControlValue();
            ctlDescription.Text = string.Empty;
            UserID = null;
            UserName = string.Empty;
            ctlUpdatePanelUser.Update();
        }
        public void SetWidthTextBox(int width)
        {
            ctlUserTextBoxAutoComplete.setTextBox = width;
        }
        #endregion
    }
}