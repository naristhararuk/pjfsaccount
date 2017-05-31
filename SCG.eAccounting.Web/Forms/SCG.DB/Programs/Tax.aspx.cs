using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;


using SS.Standard.UI;
using SS.Standard.Security;

using SCG.DB.DAL;
using SCG.DB.DTO;
using SCG.DB.BLL;
using SCG.DB.Query;
using SCG.DB.DTO.ValueObject;

using SCG.eAccounting.Web.Helper;
using SS.DB.DTO;
using SS.Standard.Utilities;


namespace SCG.eAccounting.Web.Forms.SCG.DB.Programs
{
    public partial class Tax : BasePage
    {
        #region Properties
        public IDbTaxService DbTaxService { get; set; }
        public DbTax Criteria
        {
            get { return (DbTax)ViewState["SearchCriteria"]; }
            set { ViewState["SearchCriteria"] = value; }
        }
        #endregion

        #region EventLoad
        protected override void OnPreLoad(EventArgs e)
        {
            base.OnPreLoad(e);
            //ProgramCode = "Tax";
        }

        private void RefreshGridData(object sender, EventArgs e)
        {
            ctlUpdatePanelGridview.Update();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ctlCompanyTaxInfo1.Notify_Cancel += new EventHandler(RefreshGridData);
            if (!Page.IsPostBack)
            {
                this.Criteria = BuildCriteria();
                ctlTaxGrid.DataCountAndBind();
                ctlUpdatePanelGridview.Update();
        }
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
        }
        #endregion EventLoad

        #region TaxGridviewEvent
        protected void ctlTaxGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "UserEdit")
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                long taxId = Convert.ToInt64(ctlTaxGrid.DataKeys[rowIndex].Value);
                ctlTaxForm.PageIndex = (ctlTaxGrid.PageIndex * ctlTaxGrid.PageSize) + rowIndex;
                ctlTaxForm.ChangeMode(FormViewMode.Edit);
                IList<DbTax> taxList = new List<DbTax>();
                DbTax tax = DbTaxService.FindByIdentity(taxId);
                taxList.Add(tax);

                ctlTaxForm.DataSource = taxList;
                ctlTaxForm.DataBind();
                ctlTaxGrid.DataCountAndBind();

                UpdatePanelTaxForm.Update();
                ctlTaxModalPopupExtender.Show();
            }
            else if (e.CommandName == "UserDelete")
            {
                try
                {
                    int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                    long taxId = Convert.ToInt64(ctlTaxGrid.DataKeys[rowIndex].Value);
                    DbTax tax = DbTaxService.FindByIdentity(taxId);
                    DbTaxService.Delete(tax);
                }
                catch (Exception ex)
                {
                    if (((System.Data.SqlClient.SqlException)(ex.GetBaseException())).Number == 547)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertinusedata",
                            "alert('this data is now in use.');", true);
                        ctlTaxGrid.DataCountAndBind();
                        ctlUpdatePanelGridview.Update();
                    }
                }
                ctlTaxGrid.DataCountAndBind();
                ctlUpdatePanelGridview.Update();
            }
            else if (e.CommandName == "Company")
            {
                int rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                long taxId = Convert.ToInt64(ctlTaxGrid.DataKeys[rowIndex].Value);

                ctlCompanyTaxInfo1.Initialize(taxId);
                ctlCompanyTaxInfo1.Show();
        }
        }


        protected void ctlTaxGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
        protected void ctlTaxForm_DataBound(object sender, EventArgs e)
        {
            if (ctlTaxForm.CurrentMode != FormViewMode.ReadOnly)
            {
                TextBox ctlTaxCode = ctlTaxForm.FindControl("ctlEditTaxCode") as TextBox;
                TextBox ctlEditRate = ctlTaxForm.FindControl("ctlEditRate") as TextBox;
                if (ctlEditRate != null)
                {
                    double rate = UIHelper.ParseDouble(ctlEditRate.Text);
                    ctlEditRate.Text = rate.ToString("#,##0.0000");
                }

                TextBox ctlEditRateNonDeduct = ctlTaxForm.FindControl("ctlEditRateNonDeduct") as TextBox;
                if (ctlEditRateNonDeduct != null)
                {
                    double rateNonDeduct = UIHelper.ParseDouble(ctlEditRateNonDeduct.Text);
                    ctlEditRateNonDeduct.Text = rateNonDeduct.ToString("#,##0.0000");
                }

                ctlTaxForm.Focus();
            }
        }

        protected void ctlTaxForm_ModeChanging(object sender, FormViewModeEventArgs e)
        {
            e.Cancel = true;
        }

        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            return ScgDbQueryProvider.DbTaxQuery.GetTaxList(this.Criteria, startRow, pageSize, sortExpression);
        }
        public int RequestCount()
        {
            int count = ScgDbQueryProvider.DbTaxQuery.CountByTaxCriteria(this.Criteria);
            return count;
        }
        #region protected void ctlTaxForm_ItemCommand(object sender, FormViewCommandEventArgs e)
        protected void ctlTaxForm_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            if (e.CommandName == "Cancel")
            {
                ctlTaxGrid.DataCountAndBind();
                ClosePopUp();
            }
        }
        #endregion protected void ctlTaxForm_ItemCommand(object sender, FormViewCommandE
        #endregion

        #region protected void ctlTaxForm_ItemInserting(object sender, FormViewInsertEventArgs e)
        protected void ctlTaxForm_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            DbTax dbTax = new DbTax();
            TextBox txtTaxCode = ctlTaxForm.FindControl("ctlTaxCode") as TextBox;
            TextBox txtTaxName = ctlTaxForm.FindControl("ctlTaxName") as TextBox;
            TextBox txtGL = ctlTaxForm.FindControl("ctlGL") as TextBox;
            TextBox txtRate = ctlTaxForm.FindControl("ctlRate") as TextBox;
            TextBox txtRateNonDeduct = ctlTaxForm.FindControl("ctlRateNonDeduct") as TextBox;
            CheckBox ctlActive = ctlTaxForm.FindControl("ctlActive") as CheckBox;
            CheckBox ctlApplyAllCompany = ctlTaxForm.FindControl("ctlApplyAllCompany") as CheckBox;

            dbTax.TaxCode = txtTaxCode.Text;
            dbTax.TaxName = txtTaxName.Text;
            dbTax.GL = txtGL.Text;
            if (!string.IsNullOrEmpty(txtRate.Text))
                dbTax.Rate = UIHelper.ParseDouble(txtRate.Text);
            else
                dbTax.Rate = -1;
            dbTax.RateNonDeduct = UIHelper.ParseDouble(txtRateNonDeduct.Text);
            dbTax.Active = ctlActive.Checked;
            dbTax.ApplyAllCompany = ctlApplyAllCompany.Checked;

            dbTax.UpdPgm = ProgramCode;
            dbTax.CreDate = DateTime.Now.Date;
            dbTax.UpdDate = DateTime.Now.Date;
            dbTax.CreBy = UserAccount.UserID;
            dbTax.UpdBy = UserAccount.UserID;
            
            try
            {
                DbTaxService.AddTax(dbTax);
                e.Cancel = true;
                ctlTaxGrid.DataCountAndBind();
                ctlTaxForm.ChangeMode(FormViewMode.ReadOnly);
                ClosePopUp();

            }
            catch (ServiceValidationException ex)
            {
                ValidationErrors.MergeErrors(ex.ValidationErrors);
            }
        }
        #endregion protected void ctlTaxForm_ItemInserting(object sender, FormViewInsertEventArgs e)

        #region ctlTaxForm_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        protected void ctlTaxForm_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            long taxId = UIHelper.ParseLong(ctlTaxForm.DataKey.Value.ToString());
            DbTax dbTax = DbTaxService.FindByIdentity(taxId);

            TextBox txtTaxCode = ctlTaxForm.FindControl("ctlEditTaxCode") as TextBox;
            TextBox txtTaxName = ctlTaxForm.FindControl("ctlEditTaxName") as TextBox;
            TextBox txtGL = ctlTaxForm.FindControl("ctlEditGL") as TextBox;
            TextBox txtRate = ctlTaxForm.FindControl("ctlEditRate") as TextBox;
            TextBox txtRateNonDeduct = ctlTaxForm.FindControl("ctlEditRateNonDeduct") as TextBox;
            CheckBox ctlActive = ctlTaxForm.FindControl("ctlActive") as CheckBox;
            CheckBox ctlApplyAllCompany = ctlTaxForm.FindControl("ctlApplyAllCompany") as CheckBox;

            dbTax.TaxCode = txtTaxCode.Text;
            dbTax.TaxName = txtTaxName.Text;
            dbTax.GL = txtGL.Text;
            dbTax.Rate = UIHelper.ParseDouble(txtRate.Text);
            dbTax.RateNonDeduct = UIHelper.ParseDouble(txtRateNonDeduct.Text);
            dbTax.Active = ctlActive.Checked;
            dbTax.ApplyAllCompany = ctlApplyAllCompany.Checked;

            dbTax.UpdPgm = ProgramCode;
            dbTax.UpdDate = DateTime.Now.Date;
            dbTax.UpdBy = UserAccount.UserID;

            try
            {
                DbTaxService.UpdateTax(dbTax);
                ctlTaxGrid.DataCountAndBind();
                ctlTaxForm.ChangeMode(FormViewMode.ReadOnly);
                ClosePopUp();
            }
            catch (ServiceValidationException ex)
            {
                ValidationErrors.MergeErrors(ex.ValidationErrors);
            }
        }
        #endregion
        
        #region ButtonEvent
        protected void ctlAddNew_Click(object sender, ImageClickEventArgs e)
        {
            ctlTaxModalPopupExtender.Show();
            ctlTaxGrid.DataCountAndBind();
            ctlTaxForm.ChangeMode(FormViewMode.Insert);
            UpdatePanelTaxForm.Update();
        }
        #endregion

        #region Private Function
        public void ClosePopUp()
        {
            ctlTaxForm.ChangeMode(FormViewMode.ReadOnly);
            ctlTaxModalPopupExtender.Hide();
            ctlUpdatePanelGridview.Update();
        }
        protected void ctlSearch_Click(object sender, ImageClickEventArgs e)
        {
            this.Criteria = BuildCriteria();
            ctlTaxGrid.DataCountAndBind();
            ctlUpdatePanelGridview.Update();
        }
        public DbTax BuildCriteria()
        {
            DbTax dbTax = new DbTax();
            dbTax.TaxCode = ctlTaxCode.Text;
            dbTax.TaxName = ctlDescription.Text;
            return dbTax;
        }
        #endregion



    }
}
