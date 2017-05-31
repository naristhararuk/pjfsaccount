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

using SS.SU.BLL;
using SCG.eAccounting.Web.Helper;

using SS.DB.Query;
using SS.DB.DTO;
using SS.DB.BLL;
using SS.Standard.Utilities;

namespace SCG.eAccounting.Web.Forms.SCG.DB.Programs
{
    public partial class WithHoldingTax : BasePage
    {
        public IDbWithHoldingTaxService DbWithHoldingTaxService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ctlTxtWHTCode.Text = "***EMPTY***";
                ctlTxtWHTName.Text = "***EMPTY***";
                ctlTxtWHTCode.Text = "";
                ctlTxtWHTName.Text = "";
            }
        }



        #region <== Main Program ==>

        #region protected void ctlSearch_Click(object sender, ImageClickEventArgs e)
        protected void ctlSearch_Click(object sender, ImageClickEventArgs e)
        {
            ctlGridWithholdingTax.DataCountAndBind();
            updPanelGridView.Update();
        }
        #endregion protected void ctlSearch_Click(object sender, ImageClickEventArgs e)

        #region public Object RequestData(int startRow, int pageSize, string sortExpression)
        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            return ScgDbQueryProvider.DbWithHoldingTaxQuery.GetWithHoldingTaxList(new DbWithHoldingTax(), UserAccount.CurrentLanguageID, startRow, pageSize, sortExpression, ctlTxtWHTCode.Text, ctlTxtWHTName.Text);
        }
        #endregion public Object RequestData(int startRow, int pageSize, string sortExpression)

        #region public int RequestCount()
        public int RequestCount()
        {
            int count = ScgDbQueryProvider.DbWithHoldingTaxQuery.CountWithHoldingTaxByCriteria(new DbWithHoldingTax(), ctlTxtWHTCode.Text, ctlTxtWHTName.Text);
            return count;
        }
        #endregion public int RequestCount()

        #region protected void ctlGridWithholdingTax_RowCommand(object sender, GridViewCommandEventArgs e)
        protected void ctlGridWithholdingTax_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "WHTEdit")
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                short WHTTaxCode = UIHelper.ParseShort(ctlGridWithholdingTax.DataKeys[rowIndex].Value.ToString());

                ctlGridWithholdingTax.EditIndex = rowIndex;
                IList<DbWithHoldingTax> WHTTaxList = new List<DbWithHoldingTax>();
                DbWithHoldingTax WHTTax = DbWithHoldingTaxService.FindByIdentity(WHTTaxCode);

                WHTTaxList.Add(WHTTax);

                ctlFormViewWHT.DataSource = WHTTaxList;
                ctlFormViewWHT.PageIndex = 0;

                ctlFormViewWHT.ChangeMode(FormViewMode.Edit);
                ctlFormViewWHT.DataBind();

                updPanelFormView.Update();
                ctlModalPopupWHT.Show();
            }
            else if (e.CommandName == "WHTDelete")
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                short WHTTaxCode = UIHelper.ParseShort(ctlGridWithholdingTax.DataKeys[rowIndex].Value.ToString());

                DbWithHoldingTax WHTTax = DbWithHoldingTaxService.FindByIdentity(WHTTaxCode);

                DbWithHoldingTaxService.Delete(WHTTax);
                ctlGridWithholdingTax.DataCountAndBind();
                updPanelGridView.Update();

                //ctlMessage.Message = GetMessage("DeleteSuccessFully");
            }
        }
        #endregion protected void ctlGridWithholdingTax_RowCommand(object sender, GridViewCommandEventArgs e)

        #region protected void ctlBtnAddWHT_Click(object sender, ImageClickEventArgs e)
        protected void ctlBtnAddWHT_Click(object sender, ImageClickEventArgs e)
        {
            ctlFormViewWHT.DataSource = null;
            ctlFormViewWHT.ChangeMode(FormViewMode.Insert);
            updPanelFormView.Update();
            ctlModalPopupWHT.Show();
        }
        #endregion protected void ctlBtnAddWHT_Click(object sender, ImageClickEventArgs e)

        #endregion <== Main Program ==>

        #region <== Form View ==>

        #region protected void ctlFormViewWHT_ModeChanging(object sender, FormViewModeEventArgs e)
        protected void ctlFormViewWHT_ModeChanging(object sender, FormViewModeEventArgs e)
        {

        }
        #endregion protected void ctlFormViewWHT_ModeChanging(object sender, FormViewModeEventArgs e)

        #region protected void ctlFormViewWHT_ItemCommand(object sender, FormViewCommandEventArgs e)
        protected void ctlFormViewWHT_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            if (e.CommandName == "Cancel")
            {
                ctlModalPopupWHT.Hide();
                updPanelFormView.Update();
                ctlGridWithholdingTax.DataCountAndBind();
            }
        }
        #endregion protected void ctlFormViewWHT_ItemCommand(object sender, FormViewCommandEventArgs e)

        #region protected void ctlFormViewWHT_DataBound(object sender, EventArgs e)
        protected void ctlFormViewWHT_DataBound(object sender, EventArgs e)
        {
            if (ctlFormViewWHT.CurrentMode.Equals(FormViewMode.Edit))
            {
                TextBox ctlWHTTypyCode = ctlFormViewWHT.FindControl("ctlTxtWHTCode") as TextBox;
                ctlWHTTypyCode.Focus();
            }
        }
        #endregion protected void ctlFormViewWHT_DataBound(object sender, EventArgs e)


        #region protected void ctlFormViewWHT_ItemInserting(object sender, FormViewInsertEventArgs e)
        protected void ctlFormViewWHT_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            DbWithHoldingTax WHTTax = new DbWithHoldingTax();

            TextBox txtWHTCode  = ctlFormViewWHT.FindControl("ctlTxtWHTCode") as TextBox;
            TextBox txtWHTName  = ctlFormViewWHT.FindControl("ctlTxtWHTName") as TextBox;
            TextBox txtRate     = ctlFormViewWHT.FindControl("ctlTxtRate") as TextBox;
            CheckBox chkActive  = ctlFormViewWHT.FindControl("chkActive") as CheckBox;
            TextBox txtSeq      = ctlFormViewWHT.FindControl("ctlSeq") as TextBox;

            double douRate = 0;
            try
            { douRate = UIHelper.ParseDouble(txtRate.Text); }
            catch
            { douRate = 0; }

            WHTTax.WhtCode = txtWHTCode.Text;
            WHTTax.WhtName = txtWHTName.Text;
            WHTTax.Rate = douRate;
            WHTTax.Active = chkActive.Checked;
            if (!string.IsNullOrEmpty(txtSeq.Text))
            {
                WHTTax.Seq = Helper.UIHelper.ParseInt(txtSeq.Text);
            }
            else
            {
                WHTTax.Seq = null;
            }
            WHTTax.UpdPgm = ProgramCode;
            WHTTax.CreDate = DateTime.Now.Date;
            WHTTax.UpdDate = DateTime.Now.Date;
            WHTTax.CreBy = UserAccount.UserID;
            WHTTax.UpdBy = UserAccount.UserID;

            try
            {
                CheckDataValueInsert(WHTTax);
                DbWithHoldingTaxService.Save(WHTTax);

                e.Cancel = true;
                ctlGridWithholdingTax.DataCountAndBind();
                ctlModalPopupWHT.Hide();
                updPanelGridView.Update();
                //ctlMessage.Message = GetMessage("SaveSuccessFully");
            }
            catch (ServiceValidationException ex)
            {
                ValidationErrors.MergeErrors(ex.ValidationErrors);
            }
        }
        #endregion protected void ctlFormViewWHT_ItemInserting(object sender, FormViewInsertEventArgs e)

        #region protected void ctlFormViewWHT_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        protected void ctlFormViewWHT_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            short WHTId = UIHelper.ParseShort(ctlFormViewWHT.DataKey.Value.ToString());
            DbWithHoldingTax WHTTax = DbWithHoldingTaxService.FindByIdentity(WHTId);

            TextBox txtWHTCode  = ctlFormViewWHT.FindControl("ctlTxtWHTCode") as TextBox;
            TextBox txtWHTName  = ctlFormViewWHT.FindControl("ctlTxtWHTName") as TextBox;
            TextBox txtRate     = ctlFormViewWHT.FindControl("ctlTxtRate") as TextBox;
            CheckBox chkActive  = ctlFormViewWHT.FindControl("chkActive") as CheckBox;
            TextBox txtSeq      = ctlFormViewWHT.FindControl("ctlSeq") as TextBox;

            double douRate = 0;
            try
            { douRate = UIHelper.ParseDouble(txtRate.Text); }
            catch
            { douRate = 0; }

            WHTTax.WhtCode  = txtWHTCode.Text;
            WHTTax.WhtName  = txtWHTName.Text;
            WHTTax.Rate     = douRate;
            WHTTax.Active   = chkActive.Checked;
            if (!string.IsNullOrEmpty(txtSeq.Text))
            {
                WHTTax.Seq = Helper.UIHelper.ParseInt(txtSeq.Text);
            }
            else
            {
                WHTTax.Seq = null;
            }
            

            WHTTax.UpdPgm   = ProgramCode;
            WHTTax.UpdDate  = DateTime.Now.Date;
            WHTTax.UpdBy    = UserAccount.UserID;

            try
            {
                CheckDataValueUpdate(WHTTax);
                DbWithHoldingTaxService.SaveOrUpdate(WHTTax);

                e.Cancel = true;
                ctlGridWithholdingTax.DataCountAndBind();
                ctlModalPopupWHT.Hide();
                updPanelGridView.Update();

                //ctlMessage.Message = GetMessage("SaveSuccessFully");
            }
            catch (ServiceValidationException ex)
            {
                ValidationErrors.MergeErrors(ex.ValidationErrors);
            }
        }
        #endregion protected void ctlFormViewWHT_ItemUpdating(object sender, FormViewUpdateEventArgs e)

        #endregion <== Form View ==>

        private void CheckDataValueInsert(DbWithHoldingTax WHTTax)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (string.IsNullOrEmpty(WHTTax.WhtCode))
                errors.AddError("WHT.Error", new Spring.Validation.ErrorMessage("WhtCode"));
            if (string.IsNullOrEmpty(WHTTax.WhtName))
                errors.AddError("WHT.Error", new Spring.Validation.ErrorMessage("WhtName"));
            if (WHTTax.Rate == 0)
                errors.AddError("WHT.Error", new Spring.Validation.ErrorMessage("Rate>0"));
            if (ScgDbQueryProvider.DbWithHoldingTaxQuery.isDuplicationWHTCode(WHTTax.WhtCode))
                errors.AddError("WHT.Error", new Spring.Validation.ErrorMessage("DuplicationWhtCode"));
            
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
        }

        private void CheckDataValueUpdate(DbWithHoldingTax WHTTax)
        {
            Literal ctlLblWhtCodeInGrid = ctlGridWithholdingTax.Rows[ctlGridWithholdingTax.EditIndex].FindControl("ctlLblWhtCode") as Literal;
            TextBox txtWHTCode = ctlFormViewWHT.FindControl("ctlTxtWHTCode") as TextBox;

            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (string.IsNullOrEmpty(WHTTax.WhtCode))
                errors.AddError("WHT.Error", new Spring.Validation.ErrorMessage("WhtCode"));
            if (string.IsNullOrEmpty(WHTTax.WhtName))
                errors.AddError("WHT.Error", new Spring.Validation.ErrorMessage("WhtName"));
            if (WHTTax.Rate == 0)
                errors.AddError("WHT.Error", new Spring.Validation.ErrorMessage("Rate>0"));

            if (
                (ctlLblWhtCodeInGrid.Text != txtWHTCode.Text) &&
                (ScgDbQueryProvider.DbWithHoldingTaxQuery.isDuplicationWHTCode(WHTTax.WhtCode)))
                errors.AddError("WHT.Error", new Spring.Validation.ErrorMessage("DuplicationWhtCode"));
            
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
        }

        
    }
}
