using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.SU.Query;
using SS.Standard.UI;
using SCG.DB.DTO;
using SS.SU.DTO;
using SCG.DB.Query;
using SCG.eAccounting.Web.Helper;

namespace SCG.eAccounting.Web.UserControls.DocumentEditor.Components
{
    public partial class ActorData : BaseUserControl, IEditorUserControl
    {
        #region Property
        public string UserID
        {
            get { return ctlUserID.Text; }
            set { ctlUserID.Text = value; }
        }
        public string CompanyID
        {
            get { return ctlCompanyIDLabel.Text; }
            set { ctlCompanyIDLabel.Text = value; }
        }
        public string CompanyCode
        {
            get { return ctlCompanyCodeLabel.Text; }
            set { ctlCompanyCodeLabel.Text = value; }
        }
        public string Legend
        {
            set { ctlActorDataLegend.Text = value; }
        }
        public bool ShowSearchUser
        {
            set { ctlSearchUser.Visible = value; }
            get { return ctlSearchUser.Visible; }
        }
        public bool ShowFavoriteButton
        {
            set { ctlFavorite.Visible = value; }
            get { return ctlFavorite.Visible; }
        }
        public bool ShowSMS
        {
            set { ctlSMSCheckBox.Visible = value; }
        }
        public bool IsCheckSMS
        {
            get { return ctlSMSCheckBox.Checked; }
            set { ctlSMSCheckBox.Checked = value; }
        }
        public string Width
        {
            set { ctlTableFieldSet.Width = value; }
        }
        public string Height
        {
            set { ctlTableFieldSet.Height = value; }
        }
        public string Mode
        {
            get { return ctlMode.Text; }
            set { ctlMode.Text = value; }
        }
        public bool ShowUserID
        {
            set { ctlUserDiv.Visible = value; }
        }
        public bool ShowVendorCode
        {
            set { ctlVendorCodeDiv.Visible = value; }
            get { return ctlVendorCodeDiv.Visible; }
        }
        public string InitialFlag
        {
            get
            {
                if (ViewState[ViewStateName.InitialFlag] != null)
                    return ViewState[ViewStateName.InitialFlag].ToString();
                return string.Empty;
            }
            set { ViewState[ViewStateName.InitialFlag] = value; }
        }
        public object ControlGroupID
        {
            get { return (object)ViewState["ControlGroupID"]; }
            set { ViewState["ControlGroupID"] = value; }
        }
        public Guid TxId
        {
            get
            {
                if (ViewState[ViewStateName.TransactionID] == null)
                    return Guid.Empty;
                return (Guid)ViewState[ViewStateName.TransactionID];
            }
            set { ViewState[ViewStateName.TransactionID] = value; }
        }
        public long ExpDocumentID
        {
            get
            {
                if (ViewState[ViewStateName.DocumentID] == null)
                    return 0;
                return (long)ViewState[ViewStateName.DocumentID];
            }
            set { ViewState[ViewStateName.DocumentID] = value; }
        }
        public bool IsApprover
        {
            get
            {
                if (ViewState["IsApprover"] != null)
                    return (bool)ViewState["IsApprover"];
                return false;
            }
            set { ViewState["IsApprover"] = value; }
        }

        public bool IsReceiver
        {
            get
            {
                if (ViewState["IsReceiver"] != null)
                    return (bool)ViewState["IsReceiver"];
                return false;
            }
            set { ViewState["IsReceiver"] = value; }
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
        #region IUserControl Members
        public bool Display
        {
            set
            {
                if (value)
                {
                    ctlTableFieldSet.Style["display"] = "inline-block";
                }
                else
                {
                    ctlTableFieldSet.Style["display"] = "none";
                }
            }
        }
        public string Text
        {
            get { return string.Format("{0}<br/>{1} / {2}<br/>{3} / {4} /{5}", ctlName.Text, ctlOrganization.Text, ctlDivision.Text, ctlEMailAddress.Text, ctlCostCenterCode.Text, ctlPhoneNumber.Text); }
        }
        #endregion

        public IDocumentEditor DocumentEditor { get; set; }

        #endregion

        #region Page_Load Event
        protected void Page_Load(object sender, EventArgs e)
        {
            ctlUserAutoCompleteLookup.OnObjectLookUpCalling += new BaseUserControl.ObjectLookUpCallingEventHandler(ctlUserAutoCompleteLookup_OnObjectLookUpCalling);
            ctlUserAutoCompleteLookup.OnObjectLookUpReturn += new BaseUserControl.ObjectLookUpReturnEventHandler(ctlUserAutoCompleteLookup_OnObjectLookUpReturn);
            ctlUserFavoriteLookup.OnObjectLookUpCalling += new BaseUserControl.ObjectLookUpCallingEventHandler(ctlUserFavoriteLookup_OnObjectLookUpCalling);
            ctlUserFavoriteLookup.OnPopUpReturn += new BaseUserControl.PopUpReturnEventHandler(ctlUserFavoriteLookup_OnNotifyPopup);
        }
        #endregion

        #region AutoComplete & Lookup Event
        protected void ctlUserAutoCompleteLookup_OnObjectLookUpCalling(object sender, ObjectLookUpCallingEventArgs e)
        {
            UserControls.LOV.SCG.DB.UserAutoCompleteLookup control = sender as UserControls.LOV.SCG.DB.UserAutoCompleteLookup;
            if (IsApprover)
                control.IsApprovalFlag = true;
                control.IsActive = true;
        }
        protected void ctlUserAutoCompleteLookup_OnObjectLookUpReturn(object sender, ObjectLookUpReturnArgs e)
        {
            if (e.ObjectReturn != null)
            {
                SuUser userInfo = (SuUser)e.ObjectReturn;

                this.SetControl(userInfo.Userid);

                CallOnObjectLookUpReturn(userInfo);
            }
            else
            {
                ResetControl();
            }
            ctlUpdatePanelActorData.Update();
        }
        #endregion

        public void ShowDefault()
        {
            this.SetControl(UserAccount.UserID);
        }
        public void SetValue(long userID)
        {
            this.SetControl(userID);
            ctlUpdatePanelActorData.Update();
        }
        public void SetControl(long userID)
        {
            SuUser userInfo = QueryProvider.SuUserQuery.FindByIdentity(userID);
            if (userInfo != null)
            {
                UserID = userInfo.Userid.ToString();

                ctlName.Text = userInfo.EmployeeName;
                ctlEmployeeCode.Text = userInfo.EmployeeCode;
                ctlVendorCode.Text = string.IsNullOrEmpty(userInfo.VendorCode) ? "-" : userInfo.VendorCode;
                ctlPositionName.Text = userInfo.PositionName;
                if (ShowSearchUser)
                {
                    ctlUserAutoCompleteLookup.UserID = userInfo.UserName;
                    ctlUserLoginName.Text = userInfo.UserName;
                }
                else
                {
                    ctlUserDiv.Visible = true;
                    ctlUserLoginName.Text = userInfo.UserName;
                }

                if (userInfo.Company != null)
                {
                    CompanyID = userInfo.Company.CompanyID.ToString();
                    ctlOrganization.Text = userInfo.Company.CompanyName;
                }
                else
                {
                    ctlOrganization.Text = "-";
                }

                ctlDivision.Text = string.IsNullOrEmpty(userInfo.SectionName) ? "-" : userInfo.SectionName;

                ctlEMailAddress.Text = string.IsNullOrEmpty(userInfo.Email) ? "-" : userInfo.Email;

                if (userInfo.CostCenter != null)
                {
                    ctlCostCenterCode.Text = userInfo.CostCenter.CostCenterCode;
                }
                else
                {
                    ctlCostCenterCode.Text = "-";
                }

                ctlPhoneNumber.Text = string.IsNullOrEmpty(userInfo.PhoneNo) ? "-" : userInfo.PhoneNo;


                if (IsApprover)
                    ctlSMSCheckBox.Checked = userInfo.SMSApproveOrReject;


                if (IsReceiver)
                    ctlSMSCheckBox.Checked = userInfo.SMSReadyToReceive;
            }
        }
        #region Button Event

        protected void ctlFavorite_Click(object sender, ImageClickEventArgs e)
        {
            ctlUserFavoriteLookup.Show();
        }
        #endregion

        #region IEditorComponents Members (Initialize)
        /// <summary>
        ///	First initialize when parent control are initialize.
        ///	Use for Label Extender only.
        /// </summary>
        /// <param name="txID"></param>
        /// <param name="documentID"></param>
        /// <param name="initFlag"></param>
        public void Initialize(Guid txID, long documentID, string initFlag)
        {
            this.TxId = txID;
            this.ExpDocumentID = documentID;
            this.InitialFlag = initFlag;
            BindControl();
        }
        #endregion

        public void BindControl()
        {
            if (IsApprover)
            {
                if (this.InitialFlag.Equals(FlagEnum.ViewFlag))
                {
                    ctlFavorite.Visible = false;
                }
                else
                {
                    ctlFavorite.Visible = DocumentEditor == null ? true : DocumentEditor.IsContainEditableFields(ControlGroupID);
                }
            }
            else
            {
                ctlFavorite.Visible = false;
            }

            ctlUpdatePanelActorData.Update();
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
                this.ResetControl();
            }
        }
        protected void ctlUserFavoriteLookup_OnObjectLookUpCalling(object sender, ObjectLookUpCallingEventArgs e)
        {
            UserControls.LOV.SCG.DB.UserProfileLookup lookup = sender as UserControls.LOV.SCG.DB.UserProfileLookup;
            //lookup.UserId = ctlUserAutoCompleteLookup.UserID;
            lookup.SearchFavoriteApprover = IsApprover;
            lookup.RequesterID = RequesterID;
        }
        protected void ctlUserFavoriteLookup_OnNotifyPopup(object sender, PopUpReturnArgs args)
        {
            if (args.Type.Equals(PopUpReturnType.OK) && !ctlUserFavoriteLookup.isMultiple)
            {
                SuUser userInfo = (SuUser)args.Data;
                if (userInfo != null)
                {
                    this.SetControl(userInfo.Userid);
                }
                ctlUpdatePanelActorData.Update();
            }
        }

        public void ResetControl()
        {
            UserID = string.Empty;

            ctlName.Text = string.Empty;
            ctlEmployeeCode.Text = string.Empty;
            ctlVendorCode.Text = string.Empty;
            ctlUserAutoCompleteLookup.UserID = string.Empty;
            ctlUserLoginName.Text = string.Empty;

            CompanyID = string.Empty;
            ctlPositionName.Text = "-";
            ctlOrganization.Text = "-";
            ctlDivision.Text = "-";
            ctlEMailAddress.Text = "-";
            ctlCostCenterCode.Text = "-";
            ctlPhoneNumber.Text = "-";
            ctlSMSCheckBox.Checked = false;
            ctlSMSCheckBox.Checked = false;
        }
    }
}