using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using SS.Standard.UI;
using SS.SU.DTO.ValueObject;

namespace SCG.eAccounting.Web.UserControls.LOV.SCG.DB
{
    public partial class UserProfileTextBoxAutoComplete : BaseUserControl
    {
        #region Property
        public string UserID
        {
            get { return ctlUserID.Text; }
            set { ctlUserID.Text = value; }
        }
        public string EmployeeID
        {
            get { return ctlEmployeeID.Text; }
            set { ctlEmployeeID.Text = value; }
        }
        public string EmployeeName
        {
            get { return ctlEmployeeName.Text; }
            set { ctlEmployeeName.Text = value; }
        }
        public bool IsApprovalFlag
        {
            get
            {
                if (ViewState["IsApprovalFlag"] != null)
                    return (bool)ViewState["IsApprovalFlag"];
                return false;
            }
            set { ViewState["IsApprovalFlag"] = value; }
        }

        public bool? IsActive
        {
            get
            {
                if (ViewState["IsActive"] != null)
                    return (bool)ViewState["IsActive"];
                return null;
            }
            set { ViewState["IsActive"] = value; }
        }

        //public string CostCenterId
        //{
        //    get { return ctlCostCenterId.Text; }
        //    set { ctlCostCenterId.Text = value; }
        //}
        //public string CompanyCode
        //{
        //    get { return ctlCompanyCode.Text; }
        //    set { ctlCompanyCode.Text = value; }
        //}
        //public string CostCenterCode
        //{
        //    get { return ctlCostCenterCode.Text; }
        //    set { ctlCostCenterCode.Text = value; }
        //}
       
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            ctlUpdatePanelUser.Update();
            ctlUserTextAutoComplete.BehaviorID = String.Format("UserAutoCompleteEx{0}", ctlUserID.ClientID);
            SetAutoCompleteEvent();
       
            UserAutoCompleteParameter parameter = new UserAutoCompleteParameter();
            parameter.IsApprovalFlag = IsApprovalFlag;
            parameter.IsActive = IsActive;
            //parameter.CostCenterCode = CostCenterCode;

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            ctlUserTextAutoComplete.ContextKey = serializer.Serialize(parameter);
            ctlUserTextAutoComplete.UseContextKey = true;
        }

        public delegate void NotifyPopupResultHandler(object sender, string returnAction, string returnValue);
        public event NotifyPopupResultHandler NotifyPopupResult;

        //protected void ctlReturnAction_ValueChanged(object sender, EventArgs e)
        //{
        //    NotifyPopupResult(this, ctlReturnAction.Value, ctlReturnValue.Value);
        //    ctlReturnAction.Value = string.Empty;
        //}

        protected void ctlUserID_TextChanged(object sender, EventArgs e)
        {
            if (NotifyPopupResult != null)
            {
                if (!string.IsNullOrEmpty(ctlReturnAction.Value))
                {
                    NotifyPopupResult(this, ctlReturnAction.Value, ctlReturnValue.Value);
                    ctlReturnAction.Value = string.Empty;
                }
                else
                {
                    NotifyPopupResult(this, "textchanged", ctlUserID.Text);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(ctlUserID.Text))
                {
                    this.ResetControl();
                }
            }
        }

        private void SetAutoCompleteEvent()
        {
            string ClientId = ctlUserID.ClientID;
            ctlUserTextAutoComplete.Animations = ctlUserTextAutoComplete.Animations.Replace("UserAutoCompleteEx", ctlUserTextAutoComplete.BehaviorID);
            ctlUserTextAutoComplete.OnClientItemSelected = String.Format("{0}_OnSelected", ClientId);
            //ctlUserTextAutoComplete.OnClientPopulating = String.Format("{0}_OnPopulating", ClientId);
            //ctlUserTextAutoComplete.OnClientPopulated = String.Format("{0}_OnPopulated", ClientId);
            ctlUserTextAutoComplete.OnClientItemOut = String.Format("{0}_OnItemOut", ClientId);
            ctlUserTextAutoComplete.OnClientShowing = String.Format("{0}_OnShowing", ClientId);
            ctlUserTextAutoComplete.OnClientShown = String.Format("{0}_OnShown", ClientId);
            ctlUserTextAutoComplete.OnClientHidden = String.Format("{0}_OnHidden", ClientId);
            ctlUserTextAutoComplete.OnClientHiding = String.Format("{0}_OnHiding", ClientId);
        }
        public void ResetControl()
        {
            ctlUserID.Text = string.Empty;
            ctlEmployeeID.Text = string.Empty;
        }

    }
}