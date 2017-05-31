using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using System.Text;
using SCG.DB.DTO;
using SCG.eAccounting.Web.UserControls.LOV.SCG.DB;
using SCG.eAccounting.Web.Helper;
using SCG.DB.BLL;
using SS.Standard.Utilities;
using SCG.DB.Query;
using SCG.DB.DTO.ValueObject;

namespace SCG.eAccounting.Web.UserControls
{
    public partial class CompanyTaxInfo : BaseUserControl
    {
        public IDbCompanyTaxService DbCompanyTaxService { get; set; }
        public long TaxID
        {
            get { return this.ViewState["TaxID"] == null ? (long)0 : (long)this.ViewState["TaxID"]; }
            set { this.ViewState["TaxID"] = value; }
        }

        public event EventHandler Notify_Ok;
        public event EventHandler Notify_Cancel;

        private void RefreshGridData(object sender, EventArgs e)
        {
            ctlCompanyTaxEditor.HidePopUp();
            ctlGridRole.DataCountAndBind();
            ctlUpdPanelGridView.Update();

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ctlCompanyTaxEditor.Notify_Ok += new EventHandler(RefreshGridData);
            ctlCompanyTaxEditor.Notify_Cancle += new EventHandler(RefreshGridData);
            if (!Page.IsPostBack)
            {
                
            }
        }

        #region This pane event
        public void Initialize(long taxID)
        {
            TaxID = taxID;
            if (TaxID != 0)
            {
                lblTaxText.Text = ScgDbQueryProvider.DbTaxQuery.FindByIdentity(TaxID).TaxCode.ToString();
            }
        }

        public void Hide()
        {
            ctlPanelCompanyTaxInfo.Style["display"] = "none";
        }

        public void Show()
        {
            ctlPanelCompanyTaxInfo.Style["display"] = "block";
            RefreshGridView();
        }

        private void RefreshGridView()
        {
            ctlCompanyTaxEditor.HidePopUp();
            ctlGridRole.DataCountAndBind();
            ctlUpdPanelGridView.Update();
        }

        #endregion

        #region GridView
        protected void ctlTaxGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "RoleEdit")
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                long taxId = Convert.ToInt64(ctlGridRole.DataKeys[rowIndex].Value);
                ctlCompanyTaxEditor.Initialize(FlagEnum.EditFlag, taxId,TaxID);
                ctlCompanyTaxEditor.ShowPopUp();
            }
        }
        protected void Grid_DataBound(object sender, EventArgs e)
        {
            if (ctlGridRole.Rows.Count > 0)
            {
                RegisterScriptForGridView();
            }
        }

        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            DbCompanyTax dbCompanyTax = new DbCompanyTax();
            dbCompanyTax.TaxID = TaxID;
            return ScgDbQueryProvider.DbCompanyTaxQuery.GetCompanyTaxList(dbCompanyTax, startRow, pageSize, sortExpression);
        }

        private void RegisterScriptForGridView()
        {
            StringBuilder script = new StringBuilder();
            script.Append("function validateCheckBoxControl(objChk, objFlag) ");
            script.Append("{ ");

            script.Append("CheckboxesCheckUncheck(objChk, objFlag, ");
            script.Append("'" + ctlGridRole.ClientID + "', '" + ctlGridRole.HeaderRow.FindControl("ctlHeader").ClientID + "'); ");
            script.Append("} ");

            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "validateChkBox", script.ToString(), true);
        }
        #endregion

        #region ButtonEvent
        protected void Add_Click(object sender, ImageClickEventArgs e)
        {
            ctlCompanyTaxEditor.Initialize(FlagEnum.NewFlag,0, TaxID);
            ctlCompanyTaxEditor.ShowPopUp();
        }

        protected void Delete_Click(object sender, EventArgs e)
        {

            foreach (GridViewRow row in ctlGridRole.Rows)
            {
                if (((CheckBox)(row.Cells[0].Controls[1])).Checked)
                {
                    int rowIndex = row.RowIndex;
                    string ID = ctlGridRole.DataKeys[rowIndex].Value.ToString();
                    DbCompanyTax dbCompanyTax = new DbCompanyTax();
                    dbCompanyTax.ID = UIHelper.ParseLong(ID);
                    DbCompanyTaxService.DeleteCompanyTax(dbCompanyTax);
                }
            }

            RefreshGridView();
        }

        protected void ctlButtonCancel_Click(object sender, EventArgs e)
        {
            if (Notify_Cancel != null)
            {
                Notify_Cancel(sender, e);
            }
            Hide();
        }
        #endregion

    }
}