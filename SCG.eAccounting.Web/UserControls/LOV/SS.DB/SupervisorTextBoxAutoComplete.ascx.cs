using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SCG.eAccounting.Web.Helper;
using SCG.DB.DTO.ValueObject;
using SS.SU.DTO.ValueObject;
using System.Web.Script.Serialization;
using SS.SU.DTO;


namespace SCG.eAccounting.Web.UserControls.LOV.SS.DB
{
    public partial class SupervisorTextBoxAutoComplete : BaseUserControl
    {
        
        public long? UserID
        {
            get
            {
                if (string.IsNullOrEmpty(ctlUserID.Text))
                    return null;
                return UIHelper.ParseLong(ctlUserID.Text);
            }
            set
            {
                if (value.HasValue)
                    ctlUserID.Text = value.Value.ToString();
                else
                    ctlUserID.Text = string.Empty;
            }
        }
        public string UserName
        {
            get { return ctlSupervisor.Text; }
            set { ctlSupervisor.Text = value; }
        }
        public string EmployeeName
        {
            get { return ctlEmployeeName.Text; }
            set { ctlEmployeeName.Text = value; }
        }
        public void SetAutoCompleteReadOnly()
        {
            ctlSupervisor.Enabled = false;
            ctlButtonAutoCompletePanel.Visible = false;
        }
        public void SetAutoCompleteNotReadOnly()
        {
            ctlSupervisor.Enabled = true;
            ctlButtonAutoCompletePanel.Visible = true;
        }
        public bool IsApprovalFlag
        {
            get
            {
                if (ViewState["IsApprovalFlag"] != null)
                    return (bool)ViewState["IsApprovalFlag"];
                return true;
            }
            set { ViewState["IsApprovalFlag"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            ctlSupervisorTextAutoComplete.BehaviorID = String.Format("SupervisorAutoCompleteEx{0}", ctlSupervisor.ClientID);
            SetAutoCompleteEvent();

            UserAutoCompleteParameter parameter = new UserAutoCompleteParameter();
            parameter.IsApprovalFlag = IsApprovalFlag;

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            ctlSupervisorTextAutoComplete.ContextKey = serializer.Serialize(parameter);
            ctlSupervisorTextAutoComplete.UseContextKey = true;
        }


        public delegate void NotifyPopupResultHandler(object sender, string returnAction, string returnValue);
        public event NotifyPopupResultHandler NotifyPopupResult;

        //protected void ctlReturnAction_ValueChanged(object sender, EventArgs e)
        //{
        //    NotifyPopupResult(this, ctlReturnAction.Value, ctlReturnValue.Value);
        //    ctlReturnAction.Value = string.Empty;
        //}
        private void SetAutoCompleteEvent()
        {
            string ClientId = ctlSupervisor.ClientID;
            ctlSupervisorTextAutoComplete.Animations = ctlSupervisorTextAutoComplete.Animations.Replace("SupervisorAutoCompleteEx", ctlSupervisorTextAutoComplete.BehaviorID);
            ctlSupervisorTextAutoComplete.OnClientItemSelected = String.Format("{0}_OnSelected", ClientId);
            //ctlSupervisorTextAutoComplete.OnClientPopulating = String.Format("{0}_OnPopulating", ClientId);
            //ctlSupervisorTextAutoComplete.OnClientPopulated = String.Format("{0}_OnPopulated", ClientId);
            ctlSupervisorTextAutoComplete.OnClientItemOut = String.Format("{0}_OnItemOut", ClientId);
            ctlSupervisorTextAutoComplete.OnClientShowing = String.Format("{0}_OnShowing", ClientId);
            ctlSupervisorTextAutoComplete.OnClientShown = String.Format("{0}_OnShown", ClientId);
            ctlSupervisorTextAutoComplete.OnClientHidden = String.Format("{0}_OnHidden", ClientId);
            ctlSupervisorTextAutoComplete.OnClientHiding = String.Format("{0}_OnHiding", ClientId);
        }
        public int setTextBox
        {
            set { ctlSupervisor.Width = value; }

        }
        public void ResetControlValue()
        {
            UserID = null;
            ctlSupervisor.Text = string.Empty;
            EmployeeName = string.Empty;
        }
        protected void ctlSupervisor_TextChanged(object sender, EventArgs e)
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
                    NotifyPopupResult(this, "textchanged", ctlSupervisor.Text);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(ctlUserID.Text))
                {
                    this.ResetControlValue();
                }
            }
        }
    }
}
