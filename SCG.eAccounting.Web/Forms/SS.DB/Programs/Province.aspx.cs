using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;


using SS.Standard.UI;
using SS.Standard.Security;

using SS.SU.BLL;
using SCG.eAccounting.Web.Helper;
using SS.DB.DTO.ValueObject;

using SS.DB.DAL;
using SS.DB.Query;
using SS.DB.DTO;
using SS.DB.BLL;
using SS.Standard.Utilities;
using SS.SU.DTO;

namespace SCG.eAccounting.Web.Forms.SS.DB.Programs
{
    public partial class Province : BasePage
    {
        #region Define Variable
        IList<ProvinceLang> ProvinceList;

        public IDbProvinceService       DbProvinceService { get; set; }
        public IDbProvinceLangService   DbProvinceLangService { get; set; }
        public IDbLanguageService       DbLanguageService { get; set; }
        public IDbRegionService         DbRegionService { get; set; }
        #endregion Define Variable

        #region protected void Page_Load(object sender, EventArgs e)
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
        #endregion protected void Page_Load(object sender, EventArgs e)

 

        #region <== Main Program ==>

        #region public Object RequestData(int startRow, int pageSize, string sortExpression)
        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            return SsDbQueryProvider.DbProvinceQuery.GetProvinceList(new ProvinceLang(), UserAccount.CurrentLanguageID, startRow, pageSize, sortExpression);
        }
        #endregion public Object RequestData(int startRow, int pageSize, string sortExpression)

        #region public int RequestCount()
        public int RequestCount()
        {
            int count = SsDbQueryProvider.DbProvinceQuery.CountByProvinceCriteria(new ProvinceLang());
            return count;
        }
        #endregion public int RequestCount()

        #region protected void ctlGridProvince_RowCommand(object sender, GridViewCommandEventArgs e)
        protected void ctlGridProvince_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ProvinceEdit")
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                short ProvinceId = UIHelper.ParseShort(ctlGridProvince.DataKeys[rowIndex].Value.ToString());

                ctlGridProvince.EditIndex = rowIndex;

                IList<DbProvince> provinceList = new List<DbProvince>();
                DbProvince province = DbProvinceService.FindByIdentity(ProvinceId);

                provinceList.Add(province);

                ctlProvinceFormView.DataSource = provinceList;
                ctlProvinceFormView.PageIndex = 0;

                ctlProvinceFormView.ChangeMode(FormViewMode.Edit);
                ctlProvinceFormView.DataBind();

                UpdatePanelProvinceForm.Update();
                ctlProvinceModalPopupExtender.Show();
                ProvinceLangGridViewFinish();
            }
            else if (e.CommandName == "Select")
            {
                int rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                short provinceId = UIHelper.ParseShort(ctlGridProvince.DataKeys[rowIndex].Value.ToString());

                ctlProvinceLangGrid.DataSource = SsDbQueryProvider.DbProvinceLangQuery.FindByProvinceId(provinceId);
                ctlProvinceLangGrid.DataBind();

                if (ctlProvinceLangGrid.Rows.Count > 0)
                {
                    ctlSubmit.Visible = true;
                    ctlCancel.Visible = true;
                    ctlProvinceLangLangFds.Visible = true;
                }
                else
                {
                    ctlSubmit.Visible = false;
                    ctlCancel.Visible = false;
                    ctlProvinceLangLangFds.Visible = false;
                }
                ctlProvinceLangUpdatePanel.Update();
            }
        }
        #endregion protected void ctlGridProvince_RowCommand(object sender, GridViewCommandEventArgs e)

        #region protected void ctlGridProvince_DataBound(object sender, EventArgs e)
        protected void ctlGridProvince_DataBound(object sender, EventArgs e)
        {
            if (ctlGridProvince.Rows.Count > 0)
            {
                RegisterScriptForGridView();

                ctlBtnDeleteProvince.OnClientClick = String.Format("return ConfirmDelete('{0}')", ctlGridProvince.ClientID);
                ProvinceLangGridViewFinish();
            }
        }
        #endregion protected void ctlGridProvince_DataBound(object sender, EventArgs e)

        #region protected void ctlGridProvince_PageIndexChanged(object sender, EventArgs e)
        protected void ctlGridProvince_PageIndexChanged(object sender, EventArgs e)
        {
            if (ctlGridProvince.Rows.Count > 0)
            {
                divButton.Visible = true;
                RegisterScriptForGridView();

                ctlBtnDeleteProvince.OnClientClick = String.Format("return ConfirmDelete('{0}')", ctlGridProvince.ClientID);
            }
            else
            {
                divButton.Visible = false;
            }
        }
        #endregion protected void ctlGridProvince_PageIndexChanged(object sender, EventArgs e)

        #region protected void ctlBtnAddProvince_Click(object sender, ImageClickEventArgs e)
        protected void ctlBtnAddProvince_Click(object sender, ImageClickEventArgs e)
        {
            ctlProvinceFormView.DataSource = null;
            ctlProvinceFormView.ChangeMode(FormViewMode.Insert);

            UpdatePanelProvinceForm.Update();
            ctlProvinceModalPopupExtender.Show();
            ProvinceLangGridViewFinish();
        }
        #endregion protected void ctlBtnAddProvince_Click(object sender, ImageClickEventArgs e)

        #region protected void ctlBtnDeleteProvince_Click(object sender, EventArgs e)
        protected void ctlBtnDeleteProvince_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in ctlGridProvince.Rows)
            {
                if ((row.RowType == DataControlRowType.DataRow) && ((CheckBox)row.FindControl("ctlSelect")).Checked)
                {
                    short provinceId = UIHelper.ParseShort(ctlGridProvince.DataKeys[row.RowIndex].Value.ToString());

                    if (!((CheckBox)row.FindControl("ctlChkActive")).Checked)//ไม่ Active
                    {
                        try
                        {                            
                            DbProvince province = DbProvinceService.FindProxyByIdentity(provinceId);

                            SsDbDaoProvider.DbProvinceLangDao.DeleteAllProvinceLang(provinceId);

                            DbProvinceService.Delete(province);
                            UpdatePanelGridView.Update();
                            ProvinceLangGridViewFinish();
                        }
                        catch (Exception ex)
                        {
                            if (((System.Data.SqlClient.SqlException)(ex.GetBaseException())).Number == 547)
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertInUseData",
                                    "alert('Account ID : " + provinceId.ToString() + " is Active. Can't Delete It');", true);
                            }
                        }
                    }
                    else//Active
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertActiveData",
                                    "alert('Account ID :" + provinceId.ToString() + " is Active. Can't Delete It');", true);
                    }
                }
            }

            ctlGridProvince.DataCountAndBind();
        }
        #endregion protected void ctlBtnDeleteProvince_Click(object sender, EventArgs e)

        #endregion <== Main Program ==>

        #region <== Form View ==>

        #region protected void ctlProvinceFormView_ModeChanging(object sender, FormViewModeEventArgs e)
        protected void ctlProvinceFormView_ModeChanging(object sender, FormViewModeEventArgs e)
        {

        }
        #endregion protected void ctlProvinceFormView_ModeChanging(object sender, FormViewModeEventArgs e)

        #region protected void ctlProvinceFormView_ItemCommand(object sender, FormViewCommandEventArgs e)
        protected void ctlProvinceFormView_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            if (e.CommandName == "Cancel")
            {
                ctlProvinceModalPopupExtender.Hide();
                UpdatePanelProvinceForm.Update();
                ctlGridProvince.DataCountAndBind();
            }
        }
        #endregion protected void ctlProvinceFormView_ItemCommand(object sender, FormViewCommandEventArgs e)

        #region protected void ctlProvinceFormView_DataBound(object sender, EventArgs e)
        protected void ctlProvinceFormView_DataBound(object sender, EventArgs e)
        {
            SetCombo();

            if (ctlProvinceFormView.CurrentMode.Equals(FormViewMode.Edit))
            {
                Label ctlLblProvinceIDShow = ctlProvinceFormView.FindControl("ctlLblProvinceIDShow") as Label;
                Label ctlLblProvinceNameShow = ctlProvinceFormView.FindControl("ctlLblProvinceNameShow") as Label;

                LinkButton ctlProvinceIDInGrid  = ctlGridProvince.Rows[ctlGridProvince.EditIndex].FindControl("ctlLinkProvinceId") as LinkButton;
                Label ctlProvinceNameInGrid     = ctlGridProvince.Rows[ctlGridProvince.EditIndex].FindControl("ctlLblProvinceName") as Label;
                
                ctlLblProvinceIDShow.Text       = ctlProvinceIDInGrid.Text;
                ctlLblProvinceNameShow.Text     = ctlProvinceNameInGrid.Text;

                DropDownList ctlCmbRegionId = ctlProvinceFormView.FindControl("ctlCmbRegionId") as DropDownList;
                DbProvince dbProvince = DbProvinceService.FindByIdentity(UIHelper.ParseShort(ctlProvinceIDInGrid.Text));
                ctlCmbRegionId.SelectedValue = dbProvince.Region.Regionid.ToString();
            }
        }
        #endregion protected void ctlProvinceFormView_DataBound(object sender, EventArgs e)


        #region protected void ctlProvinceFormView_ItemInserting(object sender, FormViewInsertEventArgs e)
        protected void ctlProvinceFormView_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            DbProvince dbProvince = new DbProvince();
            DbProvinceLang dbProvinceLang = new DbProvinceLang();

            TextBox ctlTxtProvinceName  = ctlProvinceFormView.FindControl("ctlTxtProvinceName") as TextBox;
            DropDownList ctlCmbRegionId = ctlProvinceFormView.FindControl("ctlCmbRegionId") as DropDownList;
            TextBox ctlTxtComment       = ctlProvinceFormView.FindControl("ctlTxtComment") as TextBox;
            CheckBox chkActive          = ctlProvinceFormView.FindControl("chkActive") as CheckBox;

            //Master
            dbProvince.Region = DbRegionService.FindByIdentity(UIHelper.ParseShort(ctlCmbRegionId.SelectedValue));
            dbProvince.Comment = ctlTxtComment.Text;
            dbProvince.Active = chkActive.Checked;

            dbProvince.UpdPgm = ProgramCode;
            dbProvince.CreDate = DateTime.Now.Date;
            dbProvince.UpdDate = DateTime.Now.Date;
            dbProvince.CreBy = UserAccount.UserID;
            dbProvince.UpdBy = UserAccount.UserID;

            //Datial
            dbProvinceLang.Province = dbProvince;
            dbProvinceLang.Language = DbLanguageService.FindByIdentity(UserAccount.CurrentLanguageID);
            dbProvinceLang.ProvinceName = ctlTxtProvinceName.Text;
            dbProvinceLang.Comment = "";
            dbProvinceLang.Active = true;

            dbProvinceLang.CreBy = UserAccount.UserID;
            dbProvinceLang.CreDate = DateTime.Now.Date;
            dbProvinceLang.UpdPgm = ProgramCode;
            dbProvinceLang.UpdBy = UserAccount.UserID;
            dbProvinceLang.UpdDate = DateTime.Now.Date;
            
            try
            {
                DbProvinceService.Save(dbProvince);
                DbProvinceLangService.Save(dbProvinceLang);

                e.Cancel = true;
                ctlGridProvince.DataCountAndBind();
                ctlProvinceModalPopupExtender.Hide();
                UpdatePanelGridView.Update();
            }
            catch (ServiceValidationException ex)
            {
                ValidationErrors.MergeErrors(ex.ValidationErrors);
            }
        }
        #endregion protected void ctlProvinceFormView_ItemInserting(object sender, FormViewInsertEventArgs e)

        #region protected void ctlProvinceFormView_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        protected void ctlProvinceFormView_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            short provinceId = UIHelper.ParseShort(ctlProvinceFormView.DataKey.Value.ToString());
            DbProvince dbProvince = DbProvinceService.FindByIdentity(provinceId);

            TextBox ctlTxtProvinceName = ctlProvinceFormView.FindControl("ctlTxtProvinceName") as TextBox;
            DropDownList ctlCmbRegionId = ctlProvinceFormView.FindControl("ctlCmbRegionId") as DropDownList;
            TextBox ctlTxtComment = ctlProvinceFormView.FindControl("ctlTxtComment") as TextBox;
            CheckBox chkActive = ctlProvinceFormView.FindControl("chkActive") as CheckBox;


            //Master
            dbProvince.Region = DbRegionService.FindByIdentity(UIHelper.ParseShort(ctlCmbRegionId.SelectedValue));
            dbProvince.Comment = ctlTxtComment.Text;
            dbProvince.Active = chkActive.Checked;

            dbProvince.UpdPgm = ProgramCode;
            dbProvince.UpdDate = DateTime.Now.Date;
            dbProvince.UpdBy = UserAccount.UserID;

            try
            {
                DbProvinceService.Save(dbProvince);
                e.Cancel = true;
                ctlGridProvince.DataCountAndBind();
                ctlProvinceModalPopupExtender.Hide();
                UpdatePanelGridView.Update();
            }
            catch (ServiceValidationException ex)
            {
                ValidationErrors.MergeErrors(ex.ValidationErrors);
            }
        }
        #endregion protected void ctlProvinceFormView_ItemUpdating(object sender, FormViewUpdateEventArgs e)

        #endregion <== Form View ==>

        #region <== Grid Language ==>

        #region protected void ctlProvinceLangGrid_DataBound(object sender, EventArgs e)
        protected void ctlProvinceLangGrid_DataBound(object sender, EventArgs e)
        {
            foreach (GridViewRow row in ctlProvinceLangGrid.Rows)
            {
                TextBox ctlProvinceName = (TextBox)ctlProvinceLangGrid.Rows[row.RowIndex].FindControl("ctlProvinceName");
                TextBox ctlComment = (TextBox)ctlProvinceLangGrid.Rows[row.RowIndex].FindControl("ctlComment");
                CheckBox ctlActive = (CheckBox)ctlProvinceLangGrid.Rows[row.RowIndex].FindControl("ctlActive");

                if (string.IsNullOrEmpty(ctlProvinceName.Text) && string.IsNullOrEmpty(ctlComment.Text))
                    ctlActive.Checked = true;
            }
        }
        #endregion protected void ctlProvinceLangGrid_DataBound(object sender, EventArgs e)

        #region protected void ctlSubmit_Click(object sender, EventArgs e)
        protected void ctlSubmit_Click(object sender, EventArgs e)
        {
            IList<DbProvinceLang> provinceLangList = new List<DbProvinceLang>();
            DbProvince province = new DbProvince(UIHelper.ParseShort(ctlGridProvince.SelectedValue.ToString()));
     
            foreach (GridViewRow row in ctlProvinceLangGrid.Rows)
            {
                TextBox ctlProvinceName = (TextBox)ctlProvinceLangGrid.Rows[row.RowIndex].FindControl("ctlProvinceName");
                TextBox ctlComment = (TextBox)ctlProvinceLangGrid.Rows[row.RowIndex].FindControl("ctlComment");
                CheckBox ctlActive = (CheckBox)ctlProvinceLangGrid.Rows[row.RowIndex].FindControl("ctlActive");

                if (!string.IsNullOrEmpty(ctlProvinceName.Text) || !string.IsNullOrEmpty(ctlComment.Text))
                {
                    DbLanguage lang = new DbLanguage(UIHelper.ParseShort(ctlProvinceLangGrid.DataKeys[row.RowIndex].Value.ToString()));

                    DbProvinceLang provinceLang = new DbProvinceLang();
                    provinceLang.Language = lang;
                    provinceLang.Province = province;
                    provinceLang.ProvinceName = ctlProvinceName.Text;
                    provinceLang.Comment = ctlComment.Text;
                    provinceLang.Active = ctlActive.Checked;

                    provinceLang.CreBy = UserAccount.UserID;
                    provinceLang.CreDate = DateTime.Now;
                    provinceLang.UpdBy = UserAccount.UserID; ;
                    provinceLang.UpdDate = DateTime.Now;
                    provinceLang.UpdPgm = ProgramCode;
                    provinceLangList.Add(provinceLang);
                }
            }

            DbProvinceLangService.UpdateProvinceLang(provinceLangList);

            ctlGridProvince.DataCountAndBind();
            UpdatePanelGridView.Update();
            ProvinceLangGridViewFinish();
        }
        #endregion protected void ctlSubmit_Click(object sender, EventArgs e)

        #region protected void ctlCancel_Click(object sender, EventArgs e)
        protected void ctlCancel_Click(object sender, EventArgs e)
        {
            ProvinceLangGridViewFinish();
        }
        #endregion protected void ctlCancel_Click(object sender, EventArgs e)

        #endregion <== Grid Language ==>

        #region <== Function ==>

        #region private void RegisterScriptForGridView()
        private void RegisterScriptForGridView()
        {
            StringBuilder script = new StringBuilder();
            script.Append("function validateCheckBox(objChk, objFlag) ");
            script.Append("{ ");
            script.Append("CheckboxesCheckUncheck(objChk, objFlag, ");
            script.Append("'" + ctlGridProvince.ClientID + "', '" + ctlGridProvince.HeaderRow.FindControl("ctlSelectHeader").ClientID + "'); ");
            script.Append("} ");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "validateChkBox", script.ToString(), true);
        }
        #endregion private void RegisterScriptForGridView()

        #region public void ProvinceLangGridViewFinish()
        public void ProvinceLangGridViewFinish()
        {
            ctlProvinceLangGrid.DataSource = null;
            ctlProvinceLangGrid.DataBind();
            ctlProvinceLangUpdatePanel.Update();
            ctlSubmit.Visible = false;
            ctlCancel.Visible = false;
            ctlProvinceLangLangFds.Visible = false;
            ctlGridProvince.SelectedIndex = -1;
        }
        #endregion public void ProvinceLangGridViewFinish()

        #region public void SetCombo()
        public void SetCombo()
        {
            DropDownList region = ctlProvinceFormView.FindControl("ctlCmbRegionId") as DropDownList;

            region.DataSource = SsDbQueryProvider.DbRegionLangQuery.FindRegionByLangCriteria(UserAccount.CurrentLanguageID);
            region.DataTextField = "Text";
            region.DataValueField = "Id";
            region.DataBind();
        }
        #endregion public void SetCombo()

        #endregion <== Function ==>
    }
}
