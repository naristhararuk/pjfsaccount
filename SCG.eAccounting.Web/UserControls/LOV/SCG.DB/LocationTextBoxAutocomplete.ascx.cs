using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using SCG.DB.DTO.ValueObject;
using SCG.eAccounting.Web.Helper;
using SS.Standard.UI;
using SCG.DB.DTO;

namespace SCG.eAccounting.Web.UserControls.LOV.SCG.DB
{
    public partial class LocationTextBoxAutocomplete : BaseUserControl
    {

        public long? LocationID
        {
            get
            {
                if (string.IsNullOrEmpty(ctlLocationID.Text))
                    return null;
                return UIHelper.ParseLong(ctlLocationID.Text);
            }
            set
            {
                if (value.HasValue)
                    ctlLocationID.Text = value.Value.ToString();
                else
                    ctlLocationID.Text = string.Empty;
            }
        }
        public long? CompanyId
        {
            get
            {
                if (ViewState["CompanyId"] != null)
                    return (long)(ViewState["CompanyId"]);
                else
                    return 0;
            }
            set
            {
                ViewState["CompanyId"] = value;
            }
        }
        public string LocationName
        {
            get { return ctlLocationName.Text; }
            set { ctlLocationName.Text = value; }
        }
        public string LocationCode
        {
            get { return ctlLocation.Text; }
            set { ctlLocation.Text = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ctlLocationTexboxAutoCompleteUpdatePanel.Update();
            CallOnObjectLookUpCalling();
            ctlLocationTextAutoComplete.BehaviorID = String.Format("LocationAutoCompleteEx{0}", ctlLocation.ClientID);
            SetAutoCompleteEvent();

            Location parameter = new Location();
            parameter.CompanyID= CompanyId.Value;

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            ctlLocationTextAutoComplete.ContextKey = serializer.Serialize(parameter);
            ctlLocationTextAutoComplete.UseContextKey = true;
        }

        protected void ctlLocation_TextChanged(object sender, EventArgs e)
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
                    NotifyPopupResult(this, "textchanged", ctlLocation.Text);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(ctlLocation.Text))
                {
                    this.ResetControl();
                }
            }
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
            string ClientId = ctlLocation.ClientID;
            ctlLocationTextAutoComplete.Animations = ctlLocationTextAutoComplete.Animations.Replace("LocationAutoCompleteEx", ctlLocationTextAutoComplete.BehaviorID);
            ctlLocationTextAutoComplete.OnClientItemSelected = String.Format("{0}_OnSelected", ClientId);
            //ctlLocationTextAutoComplete.OnClientPopulating = String.Format("{0}_OnPopulating", ClientId);
            //ctlLocationTextAutoComplete.OnClientPopulated = String.Format("{0}_OnPopulated", ClientId);
            ctlLocationTextAutoComplete.OnClientItemOut = String.Format("{0}_OnItemOut", ClientId);
            ctlLocationTextAutoComplete.OnClientShowing = String.Format("{0}_OnShowing", ClientId);
            ctlLocationTextAutoComplete.OnClientShown = String.Format("{0}_OnShown", ClientId);
            ctlLocationTextAutoComplete.OnClientHidden = String.Format("{0}_OnHidden", ClientId);
            ctlLocationTextAutoComplete.OnClientHiding = String.Format("{0}_OnHiding", ClientId);
        }
        public int setTextBox
        {
            set { ctlLocation.Width = value; }

        }
        public void SetAutoCompleteReadOnly()
        {

            ctlLocation.Enabled = false;
            ctlButtonLocationPanel.Visible = false;


        }
        public void SetAutoCompleteNotReadOnly()
        {

            ctlLocation.Enabled = true;
            ctlButtonLocationPanel.Visible = true;

        }

        public void ResetControl()
        {
            ctlLocation.Text = string.Empty;
        }
    }
}