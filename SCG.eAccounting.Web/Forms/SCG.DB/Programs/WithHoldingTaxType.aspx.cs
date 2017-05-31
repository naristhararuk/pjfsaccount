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
    public partial class WithHoldingTaxType : BasePage
    {
        public IDbWithHoldingTaxTypeService DbWithHoldingTaxTypeService { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ctlGridWithholdingTax.DataCountAndBind();
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
            return ScgDbQueryProvider.DbWithHoldingTaxTypeQuery.GetWithHoldingTaxTypeList(new DbWithHoldingTaxType(), UserAccount.CurrentLanguageID, startRow, pageSize, sortExpression,ctlTxtWHTTypeCode.Text,ctlTxtWHTTypeName.Text);
        }
        #endregion public Object RequestData(int startRow, int pageSize, string sortExpression)

        #region public int RequestCount()
        public int RequestCount()
        {
            int count = ScgDbQueryProvider.DbWithHoldingTaxTypeQuery.CountWithHoldingTaxTypeByCriteria(new DbWithHoldingTaxType(), ctlTxtWHTTypeCode.Text, ctlTxtWHTTypeName.Text);
            return count;
        }
        #endregion public int RequestCount()

        #region protected void ctlGridWithholdingTax_RowCommand(object sender, GridViewCommandEventArgs e)
        protected void ctlGridWithholdingTax_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "WHTTypeEdit")
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                short WHTTaxTypeCode = UIHelper.ParseShort(ctlGridWithholdingTax.DataKeys[rowIndex].Value.ToString());

                ctlGridWithholdingTax.EditIndex = rowIndex;
                IList<DbWithHoldingTaxType> WHTTaxTypeList = new List<DbWithHoldingTaxType>();
                DbWithHoldingTaxType WHTTaxType = DbWithHoldingTaxTypeService.FindByIdentity(WHTTaxTypeCode);

                WHTTaxTypeList.Add(WHTTaxType);

                ctlFormViewWHT.DataSource = WHTTaxTypeList;
                ctlFormViewWHT.PageIndex = 0;

                ctlFormViewWHT.ChangeMode(FormViewMode.Edit);
                ctlFormViewWHT.DataBind();

                updPanelFormView.Update();
                ctlModalPopupWHT.Show();
                //BankLangGridViewFinish();
            }
            else if (e.CommandName == "WHTTypeDelete")
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                short WHTTaxTypeCode = UIHelper.ParseShort(ctlGridWithholdingTax.DataKeys[rowIndex].Value.ToString());

                DbWithHoldingTaxType WHTTaxType = DbWithHoldingTaxTypeService.FindByIdentity(WHTTaxTypeCode);

                DbWithHoldingTaxTypeService.Delete(WHTTaxType);
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
                ctlFormViewWHT.ChangeMode(FormViewMode.ReadOnly);
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
            DbWithHoldingTaxType WHTTaxType = new DbWithHoldingTaxType();

            TextBox txtWHTTypeCode  = ctlFormViewWHT.FindControl("ctlTxtWHTCode") as TextBox;
            TextBox txtWHTTypeName  = ctlFormViewWHT.FindControl("ctlTxtWHTName") as TextBox;
            CheckBox chkPeople      = ctlFormViewWHT.FindControl("chkPeople") as CheckBox;

            CheckBox chkActive      = ctlFormViewWHT.FindControl("chkActive") as CheckBox;

            WHTTaxType.WhtTypeCode  = txtWHTTypeCode.Text;
            WHTTaxType.WhtTypeName  = txtWHTTypeName.Text;
            WHTTaxType.IsPeople     = chkPeople.Checked;
            WHTTaxType.Active       = chkActive.Checked;

            WHTTaxType.UpdPgm = ProgramCode;
            WHTTaxType.CreDate = DateTime.Now.Date;
            WHTTaxType.UpdDate = DateTime.Now.Date;
            WHTTaxType.CreBy = UserAccount.UserID;
            WHTTaxType.UpdBy = UserAccount.UserID;
            
            try
            {
                CheckDataValueInsert(WHTTaxType);
                DbWithHoldingTaxTypeService.Save(WHTTaxType);
                
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
            short WHTTypeId = UIHelper.ParseShort(ctlFormViewWHT.DataKey.Value.ToString());
            DbWithHoldingTaxType WHTTaxType = DbWithHoldingTaxTypeService.FindByIdentity(WHTTypeId);

            TextBox txtWHTTypeCode  = ctlFormViewWHT.FindControl("ctlTxtWHTCode") as TextBox;
            TextBox txtWHTTypeName  = ctlFormViewWHT.FindControl("ctlTxtWHTName") as TextBox;
            CheckBox chkPeople      = ctlFormViewWHT.FindControl("chkPeople") as CheckBox;
            CheckBox chkActive      = ctlFormViewWHT.FindControl("chkActive") as CheckBox;

            WHTTaxType.WhtTypeCode  = txtWHTTypeCode.Text;
            WHTTaxType.WhtTypeName  = txtWHTTypeName.Text;
            WHTTaxType.IsPeople     = chkPeople.Checked;
            WHTTaxType.Active       = chkActive.Checked;

            WHTTaxType.UpdPgm   = ProgramCode;
            WHTTaxType.UpdDate  = DateTime.Now.Date;
            WHTTaxType.UpdBy    = UserAccount.UserID;
            
            try
            {
                CheckDataValueUpdate(WHTTaxType);
                DbWithHoldingTaxTypeService.SaveOrUpdate(WHTTaxType);

                e.Cancel = true;
                ctlGridWithholdingTax.DataCountAndBind();
                ctlModalPopupWHT.Hide();
                updPanelGridView.Update();

                //ctlMessage.Message = GetMessage("SaveSuccessFully");
            }
            catch ( ServiceValidationException ex)
            {
                ValidationErrors.MergeErrors(ex.ValidationErrors);
            }
        }
        #endregion protected void ctlFormViewWHT_ItemUpdating(object sender, FormViewUpdateEventArgs e)

        #endregion <== Form View ==>

        private void CheckDataValueInsert(DbWithHoldingTaxType WHTTaxType)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (string.IsNullOrEmpty(WHTTaxType.WhtTypeCode))
            {
                errors.AddError("WHTType.Error", new Spring.Validation.ErrorMessage("RequiredWHTTaxTypeCode"));
            }
            else if(ScgDbQueryProvider.DbWithHoldingTaxTypeQuery.isDuplicationWHTTypeCode(WHTTaxType.WhtTypeCode))
            {
                errors.AddError("WHTType.Error", new Spring.Validation.ErrorMessage("DuplicationWhtTypeCode"));
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
        }

        private void CheckDataValueUpdate(DbWithHoldingTaxType WHTTaxType)
        {
            Literal ctlLblWhtTypeCodeInGrid = ctlGridWithholdingTax.Rows[ctlGridWithholdingTax.EditIndex].FindControl("ctlLblWhtTypeCode") as Literal;
            TextBox txtWHTTypeCode = ctlFormViewWHT.FindControl("ctlTxtWHTCode") as TextBox;

            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (string.IsNullOrEmpty(WHTTaxType.WhtTypeCode))
            {
                errors.AddError("WHTType.Error", new Spring.Validation.ErrorMessage("RequiredWHTTaxTypeCode"));
            }
            else if ( 
                (ctlLblWhtTypeCodeInGrid.Text!=txtWHTTypeCode.Text) && 
                (ScgDbQueryProvider.DbWithHoldingTaxTypeQuery.isDuplicationWHTTypeCode(WHTTaxType.WhtTypeCode))     )
            {
                errors.AddError("WHTType.Error", new Spring.Validation.ErrorMessage("DuplicationWhtTypeCode"));
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
        }
    }
}
