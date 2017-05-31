using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

using SS.Standard.UI;

using SCG.DB.DTO;
using SCG.DB.DTO.ValueObject;
using SCG.eAccounting.Web.Helper;


namespace SCG.eAccounting.Web.UserControls.LOV.SCG.DB
{
    public partial class CostCenterTextBoxAutoComplete : BaseUserControl//, IEditorUserControl
    {
        #region Property

        public string CostCenterID
        {
            get { return ctlCostCenterID.Text; }
            set { ctlCostCenterID.Text = value; }
        }
        public string CostCenterCode
        {
            get { return ctlCostCenter.Text; }
            set { ctlCostCenter.Text = value; }
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

                BuildAutoCompleteParameter();
            }
        }
        #endregion

        public int setTextBox
        {
            set { ctlCostCenter.Width = value; }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ctlCostCenterTexboxAutoCompleteUpdatePanel.Update();
            CallOnObjectLookUpCalling();
            ctlCostCenterAutoComplete.BehaviorID = String.Format("CostCenterAutoCompleteEx{0}", ctlCostCenter.ClientID);
            SetAutoCompleteEvent();


            // companyId will be set by Objectcalling
            long costCenterID = UIHelper.ParseLong(CostCenterCode.ToString());
            SCGAutoCompleteParameter parameter = new SCGAutoCompleteParameter();
            parameter.CostCenterID = costCenterID;
            parameter.CompanyID = CompanyId.Value;

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            ctlCostCenterAutoComplete.ContextKey = serializer.Serialize(parameter);
            ctlCostCenterAutoComplete.UseContextKey = true;

            //if (!Page.IsPostBack)
            //  BuildAutoCompleteParameter();

        }
        public delegate void NotifyPopupResultHandler(object sender, string returnAction, string returnValue);
        public event NotifyPopupResultHandler NotifyPopupResult;

        //protected void ctlReturnAction_ValueChanged(object sender, EventArgs e)
        //{
        //    if (NotifyPopupResult != null)
        //    {
        //        NotifyPopupResult(this, ctlReturnAction.Value, ctlReturnValue.Value);
        //        ctlReturnAction.Value = "";
        //    }

        //}
        private void SetAutoCompleteEvent()
        {
            string ClientId = ctlCostCenter.ClientID;
            ctlCostCenterAutoComplete.Animations = ctlCostCenterAutoComplete.Animations.Replace("CostCenterAutoCompleteEx", ctlCostCenterAutoComplete.BehaviorID);
            ctlCostCenterAutoComplete.OnClientItemSelected = String.Format("{0}_OnSelected", ClientId);
            //ctlCostCenterAutoComplete.OnClientPopulating = String.Format("{0}_OnPopulating", ClientId);
            //ctlCostCenterAutoComplete.OnClientPopulated = String.Format("{0}_OnPopulated", ClientId);
            ctlCostCenterAutoComplete.OnClientItemOut = String.Format("{0}_OnItemOut", ClientId);
            ctlCostCenterAutoComplete.OnClientShowing = String.Format("{0}_OnShowing", ClientId);
            ctlCostCenterAutoComplete.OnClientShown = String.Format("{0}_OnShown", ClientId);
            ctlCostCenterAutoComplete.OnClientHidden = String.Format("{0}_OnHidden", ClientId);
            ctlCostCenterAutoComplete.OnClientHiding = String.Format("{0}_OnHiding", ClientId);
        }
        public void ResetValue()
        {

        }

        public void BuildAutoCompleteParameter()
        {
            long costCenterID = UIHelper.ParseLong(CostCenterID.ToString());

            SCGAutoCompleteParameter parameter = new SCGAutoCompleteParameter();
            parameter.CostCenterID = costCenterID;
            parameter.CompanyID = CompanyId.Value;

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            ctlCostCenterAutoComplete.ContextKey = serializer.Serialize(parameter);
            ctlCostCenterAutoComplete.UseContextKey = true;
        }
        protected void ctlCostCenter_TextChanged(object sender, EventArgs e)
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
                    NotifyPopupResult(this, "textchanged", ctlCostCenter.Text);
                }
            }
        }

        #region Member IEditorUserControl
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
            get { return ctlCostCenter.Text; }
        }
        public void SetAutoCompleteReadOnly()
        {
            ctlCostCenter.Enabled = false;
            ctlButtonAutoCompletePanel.Visible = false;
        }
        public void SetAutoCompleteNotReadOnly()
        {
            ctlCostCenter.Enabled = true;
            ctlButtonAutoCompletePanel.Visible = true;

        }
        #endregion

    }
}