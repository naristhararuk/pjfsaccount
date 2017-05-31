using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SCG.eAccounting.Web.Helper;
using SCG.DB.DTO.ValueObject;
using System.Web.Script.Serialization;
using SCG.DB.DTO;
using SCG.eAccounting.Web.UserControls.DocumentEditor;
using SCG.DB.Query;

namespace SCG.eAccounting.Web.UserControls.LOV.SCG.DB
{
    public partial class CompanyTextboxAutoComplete : BaseUserControl, IEditorUserControl
    {
        public bool CompanyCodeReadOnly
        {
            set { ctlCompanyCode.Enabled = !value; }
            get { return ctlCompanyCode.ReadOnly; }
        }
        public string CompanyID
        {
            get { return ctlReturnValue.Value; }
            set { ctlReturnValue.Value = value; }
        }
        public string CompanyCode
        {
            get { return ctlCompanyCode.Text; }
            set { ctlCompanyCode.Text = value; }
        }

        public string CompanyName
        {
            get { return ctlCompanyName.Text; }
            set { ctlCompanyName.Text = value; }
        }
        public void SetAutoCompleteReadOnly()
        {
            ctlCompanyCode.Enabled = false;
            ctlButtonAutoCompletePanel.Visible = false;
        }
        public void SetAutoCompleteNotReadOnly()
        {
            ctlCompanyCode.Enabled = true;
            ctlButtonAutoCompletePanel.Visible = true;
        }
        public string CompanyCodeByCompanyID
        {
            set { this.SetControl(UIHelper.ParseLong(value)); }
        }
        public bool? FlagActive
        {
            get { return string.IsNullOrEmpty(ctlFlagActive.Value) ? (bool?)null : bool.Parse(ctlFlagActive.Value); }
            set { ctlFlagActive.Value = value.HasValue ? value.ToString() : string.Empty; }
        }
        public bool UseEccOnly // keep value in hidden field
        {
            get { return string.IsNullOrEmpty(ctlFlagUseEccOnly.Value) ? false : bool.Parse(ctlFlagUseEccOnly.Value); }
            set { ctlFlagUseEccOnly.Value = value.ToString(); }
        }
        public bool UseExpOnly // keep value in hidden field
        {
            get { return string.IsNullOrEmpty(ctlFlagUseExpOnly.Value) ? false : bool.Parse(ctlFlagUseExpOnly.Value); }
            set { ctlFlagUseExpOnly.Value = value.ToString(); }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            ctlCompanyAutoComplete.BehaviorID = String.Format("LocationAutoCompleteEx{0}", ctlCompanyCode.ClientID);
            SetAutoCompleteEvent();

            DbCompany parameter = new DbCompany();
            //parameter.LanguageId = langId;
            parameter.CompanyName = CompanyName;
            parameter.CompanyCode = CompanyCode;
            if (string.IsNullOrEmpty(CompanyID))
            {
                parameter.CompanyID = Convert.ToInt64(0);
            }
            else
            {
                parameter.CompanyID = Convert.ToInt64(CompanyID);
            }
            parameter.Active = FlagActive;
            if (UseEccOnly || UseExpOnly)
            {
                parameter.UseEcc = UseExpOnly ? false : true;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            ctlCompanyAutoComplete.ContextKey = serializer.Serialize(parameter);
            ctlCompanyAutoComplete.UseContextKey = true;
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
        protected void ctlCompanyCode_TextChanged(object sender, EventArgs e)
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
                    NotifyPopupResult(this, "textchanged", ctlCompanyCode.Text);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(ctlCompanyCode.Text))
                {
                    this.ResetValue();
                }
                else
                {
                    bool? useECC = null;
                    if (UseEccOnly || UseExpOnly)
                    {
                        useECC = UseExpOnly ? false : true;
                    }


                    DbCompany dbCom = ScgDbQueryProvider.DbCompanyQuery.GetDbCompanyByCriteria(ctlCompanyCode.Text, useECC, FlagActive);
                    if (dbCom != null)
                        this.CompanyID = dbCom.CompanyID.ToString();
                    else
                        this.ResetValue();
                }
            }
        }
        private void SetAutoCompleteEvent()
        {
            string ClientId = ctlCompanyCode.ClientID;
            ctlCompanyAutoComplete.Animations = ctlCompanyAutoComplete.Animations.Replace("CompanyAutoCompleteEx", ctlCompanyAutoComplete.BehaviorID);
            ctlCompanyAutoComplete.OnClientItemSelected = String.Format("{0}_OnSelected", ClientId);
            //ctlCompanyAutoComplete.OnClientPopulating = String.Format("{0}_OnPopulating", ClientId);
            //ctlCompanyAutoComplete.OnClientPopulated = String.Format("{0}_OnPopulated", ClientId);
            ctlCompanyAutoComplete.OnClientItemOut = String.Format("{0}_OnItemOut", ClientId);
            ctlCompanyAutoComplete.OnClientShowing = String.Format("{0}_OnShowing", ClientId);
            ctlCompanyAutoComplete.OnClientShown = String.Format("{0}_OnShown", ClientId);
            ctlCompanyAutoComplete.OnClientHidden = String.Format("{0}_OnHidden", ClientId);
            ctlCompanyAutoComplete.OnClientHiding = String.Format("{0}_OnHiding", ClientId);
        }

        public void ResetValue()
        {
            CompanyID = string.Empty;
            CompanyCode = string.Empty;
        }
        public bool Display
        {
            set
            {
                if (value)
                    ctlContainerTable.Style.Add("display", "inline-block");
                else
                    ctlContainerTable.Style.Add("display", "none");
            }
        }
        public string Text
        {
            get { return ctlCompanyCode.Text; }
        }
        public int setTextBox
        {
            set { ctlCompanyCode.Width = value; }

        }

        public void SetControl(long companyID)
        {
            DbCompany company = ScgDbQueryProvider.DbCompanyQuery.FindByIdentity(companyID);
            if (company != null)
            {
                CompanyID = companyID.ToString();
                CompanyCode = company.CompanyCode;
            }
        }
    }
}