using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SCG.DB.DTO.ValueObject;
using System.Web.Script.Serialization;
using SS.Standard.UI;
using SCG.eAccounting.Web.Helper;
using SCG.DB.DTO;
using SCG.DB.Query;

namespace SCG.eAccounting.Web.UserControls.LOV.SCG.DB
{
    public partial class IOTextBoxAutoComplete : BaseUserControl
    {
        #region Property
        public string InternalOrder
        {
            get { return ctlIO.Text; }
            set { ctlIO.Text = value; }
        }
        public string IODescription
        {
            get { return ctlIOText.Text; }
            set { ctlIOText.Text = value; }
        }
        public long? CompanyId
        {
            get
            {
                if (string.IsNullOrEmpty(ctlCompanyId.Text))
                    return null;
                return UIHelper.ParseLong(ctlCompanyId.Text);
            }
            set
            {
                if (value.HasValue)
                    ctlCompanyId.Text = value.Value.ToString();
                else
                    ctlCompanyId.Text = string.Empty;

                BuildAutoCompleteParameter();
            }
        }
        public long? CostCenterId
        {
            get
            {
                if (string.IsNullOrEmpty(ctlCostCenterId.Text))
                    return null;
                return UIHelper.ParseLong(ctlCostCenterId.Text);
            }
            set
            {
                if (value.HasValue)
                    ctlCostCenterId.Text = value.Value.ToString();
                else
                    ctlCostCenterId.Text = string.Empty;

                BuildAutoCompleteParameter();
            }
        }
        public string CompanyCode
        {
            get { return ctlCompanyCode.Text; }
            set { ctlCompanyCode.Text = value; }
        }
        public string CostCenterCode
        {
            get { return ctlCostCenterCode.Text; }
            set { ctlCostCenterCode.Text = value; }
        }
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
            get
            {
                if (string.IsNullOrEmpty(ctlIO.Text) || string.IsNullOrEmpty(ctlIOText.Text))
                {
                    return string.Empty;
                }
                else
                {
                    return ctlIO.Text + '-' + ctlIOText.Text;
                }
            }
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            ctlIOTextAutoComplete.BehaviorID = String.Format("IOAutoCompleteEx{0}", ctlIO.ClientID);
            SetAutoCompleteEvent();

            if(!Page.IsPostBack)
                BuildAutoCompleteParameter();
        }


        public delegate void NotifyPopupResultHandler(object sender, string returnAction, string returnValue);
        public event NotifyPopupResultHandler NotifyPopupResult;

        //protected void ctlReturnAction_ValueChanged(object sender, EventArgs e)
        //{
        //    if (NotifyPopupResult != null)
        //    {
        //        NotifyPopupResult(this, ctlReturnAction.Value, ctlReturnValue.Value);
        //        ctlReturnAction.Value = string.Empty;
        //    }
        //}
        private void SetAutoCompleteEvent()
        {
            string ClientId = ctlIO.ClientID;
            ctlIOTextAutoComplete.Animations = ctlIOTextAutoComplete.Animations.Replace("IOAutoCompleteEx", ctlIOTextAutoComplete.BehaviorID);
            ctlIOTextAutoComplete.OnClientItemSelected = String.Format("{0}_OnSelected", ClientId);
            //ctlIOTextAutoComplete.OnClientPopulating = String.Format("{0}_OnPopulating", ClientId);
            //ctlIOTextAutoComplete.OnClientPopulated = String.Format("{0}_OnPopulated", ClientId);
            ctlIOTextAutoComplete.OnClientItemOut = String.Format("{0}_OnItemOut", ClientId);
            ctlIOTextAutoComplete.OnClientShowing = String.Format("{0}_OnShowing", ClientId);
            ctlIOTextAutoComplete.OnClientShown = String.Format("{0}_OnShown", ClientId);
            ctlIOTextAutoComplete.OnClientHidden = String.Format("{0}_OnHidden", ClientId);
            ctlIOTextAutoComplete.OnClientHiding = String.Format("{0}_OnHiding", ClientId);
        }

        public void BuildAutoCompleteParameter()
        {
            
            IOAutoCompleteParameter parameter = new IOAutoCompleteParameter();
            parameter.CompanyId = CompanyId;
            parameter.CompanyCode = CompanyCode;
            parameter.CostCenterId = CostCenterId;
            parameter.CostCenterCode = CostCenterCode;
            parameter.IOID = UIHelper.ParseLong(InternalOrder);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            ctlIOTextAutoComplete.ContextKey = serializer.Serialize(parameter);
            ctlIOTextAutoComplete.UseContextKey = true;
        }

        protected void ctlIO_TextChanged(object sender, EventArgs e)
        {
            if (NotifyPopupResult != null)
            {
                if (!string.IsNullOrEmpty(ctlReturnAction.Value))
                {
                    long ioId = UIHelper.ParseLong(ctlReturnValue.Value);
                    DbInternalOrder selectedIO = ScgDbQueryProvider.DbIOQuery.FindByIdentity(ioId);

                    if (DateTime.Now > selectedIO.ExpireDate)
                        ModalPopupExtender1ShowMessage.Show();  
                    else
                        NotifyPopupResult(this, ctlReturnAction.Value, ctlReturnValue.Value);
                    
                    ctlReturnAction.Value = string.Empty;
                }
                else
                {
                    NotifyPopupResult(this, "textchanged", ctlIO.Text);
                }
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            NotifyPopupResult(this, ctlReturnAction.Value, ctlReturnValue.Value);  
            this.ModalPopupExtender1ShowMessage.Hide();
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            this.ModalPopupExtender1ShowMessage.Hide();
            ctlReturnAction.Value = string.Empty;
            ctlIO.Text = string.Empty;
            UpdatePanelIO.Update();
        }

        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            this.ModalPopupExtender1ShowMessage.Hide();
            ctlReturnAction.Value = string.Empty;
            ctlIO.Text = string.Empty;
            UpdatePanelIO.Update();
        }
    }
}