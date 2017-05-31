using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SCG.eAccounting.Web.Helper;
using SCG.DB.BLL;
using SCG.DB.DTO.ValueObject;
using SCG.DB.Query;
using SCG.DB.DTO;
using SS.Standard.Utilities;

namespace SCG.eAccounting.Web.Forms.SCG.DB.Programs
{
    public partial class MaintainRate : BasePage
    {
        public IDbPbRateService DbPbRateService { get; set; }
        public long PBID
        {
            get
            {
                if (ViewState["PbId"] != null)
                    return (long)ViewState["PbId"];
                return 0;
            }
            set { ViewState["PbId"] = value; }
        }
        public Dbpb DbPb
        {
            get
            {
                if (ViewState["DbPb"] != null)
                    return (Dbpb)ViewState["DbPb"];
                return null;
            }
            set { ViewState["DbPb"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ctlMaintain.DataCountAndBind();
                string script = string.Format("sumExchangeRate('{0}','{1}','{2}')", ctlFromAmountTextBox.ClientID, ctlToAmountTextBox.ClientID, ctlExchangeRateTextbox.ClientID);
                ctlFromAmountTextBox.Attributes.Add("onblur", script);
                ctlToAmountTextBox.Attributes.Add("onblur", script);
                ctlUpdatePanelMaintainRateGrid.Update();
            }
        }

        

        protected void PB_DataBound(object sender, EventArgs e)
        {
            foreach (GridViewRow row in ctlMaintain.Rows)
            {
            }
        }

        protected int RequestCount()
        {
            VOPB criteria = GetPBCriteria();
            int count = ScgDbQueryProvider.DbPbRateQuery.CountPbByCriteria(criteria);
            return count;
        }

        protected object RequestData(int startRow, int pageSize, string sortExpression)
        {
            VOPB criteria = GetPBCriteria();
            return ScgDbQueryProvider.DbPbRateQuery.GetPbList(criteria, startRow, pageSize, sortExpression);
        }

        protected void ctlMaintain_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex;
            if (e.CommandName.Equals("MaintainRate"))
            {
                ctlDatePicker.Value = DateTime.Now;
                rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                ctlMaintain.SelectedIndex = rowIndex;
                PBID = UIHelper.ParseShort(ctlMaintain.DataKeys[rowIndex].Values["Pbid"].ToString());
                DbPb = ScgDbQueryProvider.DbPBQuery.FindByIdentity(PBID);
                if (DbPb != null && DbPb.MainCurrencyID != null)
                {
                    CurrencyDropdown1.BindCurrency(DbPb.MainCurrencyID.Value);
                    CurrencyDropdown1.Enable = false;
                }
                else
                {
                    CurrencyDropdown1.ResetControl();
                    CurrencyDropdown1.Enable = true;
                }
                ctlDivMaintainInfo.Style["display"] = "block";
                ctlMaintainInfo.DataCountAndBind();
            }
            ctlUpdatePanelMaintainRateGrid.Update();
        }

        protected void ctlAdd_Click(object sender, ImageClickEventArgs e)
        {
            DbPbRate pbRate = new DbPbRate();
            pbRate.PBID = PBID;
            pbRate.EffectiveDate = ctlDatePicker.Value;
            pbRate.MainCurrencyID = UIHelper.ParseShort(CurrencyDropdown1.SelectedValue);
            pbRate.FromAmount = UIHelper.ParseDouble(ctlFromAmountTextBox.Text);
            pbRate.CurrencyID = UIHelper.ParseShort(CurrencyDropdown2.SelectedValue);
            pbRate.ToAmount = UIHelper.ParseDouble(ctlToAmountTextBox.Text);
            pbRate.ExchangeRate = UIHelper.ParseDouble(ctlExchangeRateTextbox.Text);
            pbRate.UpdateBy = UserAccount.EmployeeName;
            pbRate.UpdPgm = this.ProgramCode;
            pbRate.CreBy = UserAccount.UserID;
            pbRate.CreDate = DateTime.Now;
            pbRate.UpdBy = UserAccount.UserID;
            pbRate.UpdDate = DateTime.Now;
            try
            {
                DbPbRateService.AddPbRate(pbRate);
                ctlMaintainInfo.DataCountAndBind();
                ctlUpdatePanelMaintainRateGrid.Update();
                ClearField();
            }
            catch (ServiceValidationException ex)
            {
                ValidationErrors.MergeErrors(ex.ValidationErrors);
            }
        }
        public void ClearField()
        {
            ctlDatePicker.Value = DateTime.Now;
            if (DbPb != null && DbPb.MainCurrencyID == null)
            {
                CurrencyDropdown1.ResetControl();
            }
            CurrencyDropdown2.ResetControl();
            ctlFromAmountTextBox.Text = "";
            ctlExchangeRateTextbox.Text = "";
            ctlToAmountTextBox.Text = "";
        }

        protected int RequestCountInfo()
        {
            VOPB criteria = GetPBCriteria();
            int count = ScgDbQueryProvider.DbPbRateQuery.CountPbInfoByCriteria(PBID);
            return count;
        }

        protected object RequestDataInfo(int startRow, int pageSize, string sortExpression)
        {
            VOPB criteria = GetPBCriteria();
            return ScgDbQueryProvider.DbPbRateQuery.GetPbInfoList(PBID, startRow, pageSize, sortExpression);
        }

        public VOPB GetPBCriteria()
        {
            VOPB pb = new VOPB();
            pb.UserID = UserAccount.UserID;
            pb.LanguageID = UserAccount.CurrentLanguageID;
            return pb;
        }
    }
}