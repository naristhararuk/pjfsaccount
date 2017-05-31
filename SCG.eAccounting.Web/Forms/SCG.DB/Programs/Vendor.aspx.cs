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
    public partial class Vendor : BasePage
    {
        public IDbVendorService DbVendorService { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ctlTxtVendorCode.Text = "***EMPTY***";
                ctlTxtVendorName.Text = "***EMPTY***";
                ctlGridVendor.DataCountAndBind();
                ctlTxtVendorCode.Text = "";
                ctlTxtVendorName.Text = "";
            }
        }



        #region <== Main Program ==>

        #region protected void ctlSearch_Click(object sender, ImageClickEventArgs e)
        protected void ctlSearch_Click(object sender, ImageClickEventArgs e)
        {
            ctlGridVendor.DataCountAndBind();
            updPanelGridView.Update();
        }
        #endregion protected void ctlSearch_Click(object sender, ImageClickEventArgs e)

        #region public Object RequestData(int startRow, int pageSize, string sortExpression)
        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            return ScgDbQueryProvider.DbVendorQuery.GetVendorList(new DbVendor(), UserAccount.CurrentLanguageID, startRow, pageSize, sortExpression, ctlTxtVendorCode.Text, ctlTxtVendorName.Text);
        }
        #endregion public Object RequestData(int startRow, int pageSize, string sortExpression)

        #region public int RequestCount()
        public int RequestCount()
        {
            int count = ScgDbQueryProvider.DbVendorQuery.CountVendorByCriteria(new DbVendor(), ctlTxtVendorCode.Text, ctlTxtVendorName.Text);
            return count;
        }
        #endregion public int RequestCount()

        #region protected void ctlGridVendor_RowCommand(object sender, GridViewCommandEventArgs e)
        protected void ctlGridVendor_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "VendorEdit")
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                long VendorCode = UIHelper.ParseLong(ctlGridVendor.DataKeys[rowIndex].Value.ToString());

                ctlGridVendor.EditIndex = rowIndex;
                IList<DbVendor> VendorList = new List<DbVendor>();
                DbVendor Vendor = DbVendorService.FindByIdentity(VendorCode);

                VendorList.Add(Vendor);

                ctlFormViewVendor.DataSource = VendorList;
                ctlFormViewVendor.PageIndex = 0;

                ctlFormViewVendor.ChangeMode(FormViewMode.Edit);
                ctlFormViewVendor.DataBind();

                updPanelFormView.Update();
                ctlModalPopupVendor.Show();
            }
            else if (e.CommandName == "VendorDelete")
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                long VendorCode = UIHelper.ParseLong(ctlGridVendor.DataKeys[rowIndex].Value.ToString());

                DbVendor Vendor = DbVendorService.FindByIdentity(VendorCode);

                DbVendorService.Delete(Vendor);
                ctlGridVendor.DataCountAndBind();
                updPanelGridView.Update();

                //ctlMessage.Message = GetMessage("DeleteSuccessFully");
            }
        }
        #endregion protected void ctlGridVendor_RowCommand(object sender, GridViewCommandEventArgs e)

        #region protected void ctlBtnAddVendor_Click(object sender, ImageClickEventArgs e)
        protected void ctlBtnAddVendor_Click(object sender, ImageClickEventArgs e)
        {
            ctlFormViewVendor.DataSource = null;
            ctlFormViewVendor.ChangeMode(FormViewMode.Insert);
            updPanelFormView.Update();
            ctlModalPopupVendor.Show();
        }
        #endregion protected void ctlBtnAddVendor_Click(object sender, ImageClickEventArgs e)

        #endregion <== Main Program ==>

        #region <== Form View ==>

        #region protected void ctlFormViewVendor_ModeChanging(object sender, FormViewModeEventArgs e)
        protected void ctlFormViewVendor_ModeChanging(object sender, FormViewModeEventArgs e)
        {

        }
        #endregion protected void ctlFormViewVendor_ModeChanging(object sender, FormViewModeEventArgs e)

        #region protected void ctlFormViewVendor_ItemCommand(object sender, FormViewCommandEventArgs e)
        protected void ctlFormViewVendor_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            if (e.CommandName == "Cancel")
            {
                ctlModalPopupVendor.Hide();
                updPanelFormView.Update();
                ctlGridVendor.DataCountAndBind();
            }
        }
        #endregion protected void ctlFormViewVendor_ItemCommand(object sender, FormViewCommandEventArgs e)

        #region protected void ctlFormViewVendor_DataBound(object sender, EventArgs e)
        protected void ctlFormViewVendor_DataBound(object sender, EventArgs e)
        {
            if (ctlFormViewVendor.CurrentMode.Equals(FormViewMode.Edit))
            {
                TextBox ctlVendorCode = ctlFormViewVendor.FindControl("ctlTxtVendorCode") as TextBox;
                ctlVendorCode.Focus();
            }
        }
        #endregion protected void ctlFormViewVendor_DataBound(object sender, EventArgs e)


        #region protected void ctlFormViewVendor_ItemInserting(object sender, FormViewInsertEventArgs e)
        protected void ctlFormViewVendor_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            DbVendor Vendor = new DbVendor();

            TextBox txtVendorCode = ctlFormViewVendor.FindControl("ctlTxtVendorCode") as TextBox;
            TextBox txtVendorTitle = ctlFormViewVendor.FindControl("ctlTxtVendorTitle") as TextBox;
            TextBox txtVendorName1 = ctlFormViewVendor.FindControl("ctlTxtVendorName1") as TextBox;
            TextBox txtVendorName2 = ctlFormViewVendor.FindControl("ctlTxtVendorName2") as TextBox;
            TextBox txtStreet = ctlFormViewVendor.FindControl("ctlTxtStreet") as TextBox;
            TextBox txtCity = ctlFormViewVendor.FindControl("ctlTxtCity") as TextBox;
            TextBox txtDistrict = ctlFormViewVendor.FindControl("ctlTxtDistrict") as TextBox;
            TextBox txtCountry = ctlFormViewVendor.FindControl("ctlTxtCountry") as TextBox;
            TextBox txtPostalCode = ctlFormViewVendor.FindControl("ctlTxtPostalCode") as TextBox;
            TextBox txtTaxNo1 = ctlFormViewVendor.FindControl("ctlTxtTaxNo1") as TextBox;
            TextBox txtTaxNo2 = ctlFormViewVendor.FindControl("ctlTxtTaxNo2") as TextBox;
            TextBox txtTaxNo3 = ctlFormViewVendor.FindControl("ctlTxtTaxNo3") as TextBox;
            TextBox txtTaxNo4 = ctlFormViewVendor.FindControl("ctlTxtTaxNo4") as TextBox;
            CheckBox chkBlockDelete = ctlFormViewVendor.FindControl("chkBlockDelete") as CheckBox;
            CheckBox chkBlockPost = ctlFormViewVendor.FindControl("chkBlockPost") as CheckBox;
            CheckBox chkActive = ctlFormViewVendor.FindControl("chkActive") as CheckBox;

            Vendor.VendorCode = txtVendorCode.Text;
            Vendor.VendorTitle = txtVendorTitle.Text;
            Vendor.VendorName1 = txtVendorName1.Text;
            Vendor.VendorName2 = txtVendorName2.Text;
            Vendor.Street = txtStreet.Text;
            Vendor.City = txtCity.Text;
            Vendor.District = txtDistrict.Text;
            Vendor.Country = txtCountry.Text;
            Vendor.PostalCode = txtPostalCode.Text;
            Vendor.TaxNo1 = txtTaxNo1.Text;
            Vendor.TaxNo2 = txtTaxNo2.Text;
            Vendor.TaxNo3 = txtTaxNo3.Text;
            Vendor.TaxNo4 = txtTaxNo4.Text;
            Vendor.BlockDelete = chkBlockDelete.Checked;
            Vendor.BlockPost = chkBlockPost.Checked;
            Vendor.Active = chkActive.Checked;

            Vendor.UpdPgm = ProgramCode;
            Vendor.CreDate = DateTime.Now.Date;
            Vendor.UpdDate = DateTime.Now.Date;
            Vendor.CreBy = UserAccount.UserID;
            Vendor.UpdBy = UserAccount.UserID;

            try
            {
                CheckDataValueInsert(Vendor);
                DbVendorService.Save(Vendor);

                e.Cancel = true;
                ctlGridVendor.DataCountAndBind();
                ctlModalPopupVendor.Hide();
                updPanelGridView.Update();

                //ctlMessage.Message = GetMessage("SaveSuccessFully");
            }
            catch (ServiceValidationException ex)
            {
                ValidationErrors.MergeErrors(ex.ValidationErrors);
            }
        }
        #endregion protected void ctlFormViewVendor_ItemInserting(object sender, FormViewInsertEventArgs e)

        #region protected void ctlFormViewVendor_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        protected void ctlFormViewVendor_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            long VendorID = UIHelper.ParseLong(ctlFormViewVendor.DataKey.Value.ToString());
            DbVendor Vendor = DbVendorService.FindByIdentity(VendorID);

            TextBox txtVendorCode = ctlFormViewVendor.FindControl("ctlTxtVendorCode") as TextBox;
            TextBox txtVendorTitle = ctlFormViewVendor.FindControl("ctlTxtVendorTitle") as TextBox;
            TextBox txtVendorName1 = ctlFormViewVendor.FindControl("ctlTxtVendorName1") as TextBox;
            TextBox txtVendorName2 = ctlFormViewVendor.FindControl("ctlTxtVendorName2") as TextBox;
            TextBox txtStreet = ctlFormViewVendor.FindControl("ctlTxtStreet") as TextBox;
            TextBox txtCity = ctlFormViewVendor.FindControl("ctlTxtCity") as TextBox;
            TextBox txtDistrict = ctlFormViewVendor.FindControl("ctlTxtDistrict") as TextBox;
            TextBox txtCountry = ctlFormViewVendor.FindControl("ctlTxtCountry") as TextBox;
            TextBox txtPostalCode = ctlFormViewVendor.FindControl("ctlTxtPostalCode") as TextBox;
            TextBox txtTaxNo1 = ctlFormViewVendor.FindControl("ctlTxtTaxNo1") as TextBox;
            TextBox txtTaxNo2 = ctlFormViewVendor.FindControl("ctlTxtTaxNo2") as TextBox;
            TextBox txtTaxNo3 = ctlFormViewVendor.FindControl("ctlTxtTaxNo3") as TextBox;
            TextBox txtTaxNo4 = ctlFormViewVendor.FindControl("ctlTxtTaxNo4") as TextBox;
            CheckBox chkBlockDelete = ctlFormViewVendor.FindControl("chkBlockDelete") as CheckBox;
            CheckBox chkBlockPost = ctlFormViewVendor.FindControl("chkBlockPost") as CheckBox;
            CheckBox chkActive = ctlFormViewVendor.FindControl("chkActive") as CheckBox;

            Vendor.VendorCode = txtVendorCode.Text;
            Vendor.VendorTitle = txtVendorTitle.Text;
            Vendor.VendorName1 = txtVendorName1.Text;
            Vendor.VendorName2 = txtVendorName2.Text;
            Vendor.Street = txtStreet.Text;
            Vendor.City = txtCity.Text;
            Vendor.District = txtDistrict.Text;
            Vendor.Country = txtCountry.Text;
            Vendor.PostalCode = txtPostalCode.Text;
            Vendor.TaxNo1 = txtTaxNo1.Text;
            Vendor.TaxNo2 = txtTaxNo2.Text;
            Vendor.TaxNo3 = txtTaxNo3.Text;
            Vendor.TaxNo4 = txtTaxNo4.Text;
            Vendor.BlockDelete = chkBlockDelete.Checked;
            Vendor.BlockPost = chkBlockPost.Checked;
            Vendor.Active = chkActive.Checked;

            Vendor.UpdPgm = ProgramCode;
            Vendor.UpdDate = DateTime.Now.Date;
            Vendor.UpdBy = UserAccount.UserID;

            try
            {
                CheckDataValueUpdate(Vendor);
                DbVendorService.SaveOrUpdate(Vendor);

                e.Cancel = true;
                ctlGridVendor.DataCountAndBind();
                ctlModalPopupVendor.Hide();
                updPanelGridView.Update();

                //ctlMessage.Message = GetMessage("SaveSuccessFully");
            }
            catch (ServiceValidationException ex)
            {
                ValidationErrors.MergeErrors(ex.ValidationErrors);
            }
        }
        #endregion protected void ctlFormViewVendor_ItemUpdating(object sender, FormViewUpdateEventArgs e)

        #endregion <== Form View ==>

        private void CheckDataValueInsert(DbVendor Vendor)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (string.IsNullOrEmpty(Vendor.VendorCode))
                errors.AddError("Vendor.Error", new Spring.Validation.ErrorMessage("VendorCode"));
            if (string.IsNullOrEmpty(Vendor.VendorName1))
                errors.AddError("Vendor.Error", new Spring.Validation.ErrorMessage("VendorName1"));
            if (string.IsNullOrEmpty(Vendor.Street))
                errors.AddError("Vendor.Error", new Spring.Validation.ErrorMessage("Street"));
            if (string.IsNullOrEmpty(Vendor.City))
                errors.AddError("Vendor.Error", new Spring.Validation.ErrorMessage("City"));
            if (string.IsNullOrEmpty(Vendor.Country))
                errors.AddError("Vendor.Error", new Spring.Validation.ErrorMessage("Country"));
            if (string.IsNullOrEmpty(Vendor.PostalCode))
                errors.AddError("Vendor.Error", new Spring.Validation.ErrorMessage("PostalCode"));
            if (ScgDbQueryProvider.DbVendorQuery.isDuplicationVendorCode(Vendor.VendorCode))
                errors.AddError("Vendor.Error", new Spring.Validation.ErrorMessage("DuplicationVendorCode"));

            //if (!string.IsNullOrEmpty(Vendor.TaxNo1) && !string.IsNullOrEmpty(Vendor.TaxNo2))
            //    errors.AddError("Vendor.Error", new Spring.Validation.ErrorMessage("TaxNoHaveOneValueOnly"));
            //else if (string.IsNullOrEmpty(Vendor.TaxNo1) && string.IsNullOrEmpty(Vendor.TaxNo2))
            //    errors.AddError("Vendor.Error", new Spring.Validation.ErrorMessage("TaxNoIsEmpty"));

            if (string.IsNullOrEmpty(Vendor.TaxNo3))
                errors.AddError("Vendor.Error", new Spring.Validation.ErrorMessage("TaxNo3IsEmpty"));

            if (string.IsNullOrEmpty(Vendor.TaxNo4))
                errors.AddError("Vendor.Error", new Spring.Validation.ErrorMessage("TaxNo4IsEmpty"));

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
        }

        private void CheckDataValueUpdate(DbVendor Vendor)
        {
            Literal ctlLblVenderCodeInGrid = ctlGridVendor.Rows[ctlGridVendor.EditIndex].FindControl("ctlLblVendorCode") as Literal;
            TextBox txtVendorCode = ctlFormViewVendor.FindControl("ctlTxtVendorCode") as TextBox;

            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (string.IsNullOrEmpty(Vendor.VendorCode))
                errors.AddError("Vendor.Error", new Spring.Validation.ErrorMessage("VendorCode"));
            if (string.IsNullOrEmpty(Vendor.VendorName1))
                errors.AddError("Vendor.Error", new Spring.Validation.ErrorMessage("VendorName1"));
            if (string.IsNullOrEmpty(Vendor.Street))
                errors.AddError("Vendor.Error", new Spring.Validation.ErrorMessage("Street"));
            if (string.IsNullOrEmpty(Vendor.City))
                errors.AddError("Vendor.Error", new Spring.Validation.ErrorMessage("City"));
            if (string.IsNullOrEmpty(Vendor.Country))
                errors.AddError("Vendor.Error", new Spring.Validation.ErrorMessage("Country"));
            if (string.IsNullOrEmpty(Vendor.PostalCode))
                errors.AddError("Vendor.Error", new Spring.Validation.ErrorMessage("PostalCode"));
            if (
                    (ctlLblVenderCodeInGrid.Text != txtVendorCode.Text) &&
                    (ScgDbQueryProvider.DbVendorQuery.isDuplicationVendorCode(Vendor.VendorCode))
               )
                errors.AddError("Vendor.Error", new Spring.Validation.ErrorMessage("DuplicationVendorCode"));

            //if (!string.IsNullOrEmpty(Vendor.TaxNo1) && !string.IsNullOrEmpty(Vendor.TaxNo2))
            //    errors.AddError("Vendor.Error", new Spring.Validation.ErrorMessage("TaxNoHaveOneValueOnly"));
            //else if (string.IsNullOrEmpty(Vendor.TaxNo1) && string.IsNullOrEmpty(Vendor.TaxNo2))
            //    errors.AddError("Vendor.Error", new Spring.Validation.ErrorMessage("TaxNoIsEmpty"));

            if (string.IsNullOrEmpty(Vendor.TaxNo3))
                errors.AddError("Vendor.Error", new Spring.Validation.ErrorMessage("TaxNo3IsEmpty"));

            if (string.IsNullOrEmpty(Vendor.TaxNo4))
                errors.AddError("Vendor.Error", new Spring.Validation.ErrorMessage("TaxNo4IsEmpty"));

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
        }
    }
}
