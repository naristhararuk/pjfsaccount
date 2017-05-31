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

namespace SCG.eAccounting.Web.Forms.SCG.DB.Programs
{
    public partial class Bank : BasePage
    {
        #region Define Variable
        IList<DbBank> bankList;

        public IDbLanguageService   DbLanguageService   { get; set; }
        public IDbBankService       DbBankService       { get; set; }
        public IDbBankLangService   DbBankLangService   { get; set; }
        #endregion Define Variable

        #region protected void Page_Load(object sender, EventArgs e)
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
        #endregion protected void Page_Load(object sender, EventArgs e)

        #region protected override void OnPreRender(EventArgs e)

        #endregion protected override void OnPreRender(EventArgs e)

        #region <== Main Program ==>

        #region public Object RequestData(int startRow, int pageSize, string sortExpression)
        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            return ScgDbQueryProvider.DbBankQuery.GetBankList(new BankLang(), UserAccount.CurrentLanguageID, startRow, pageSize, sortExpression);
        }
        #endregion public Object RequestData(int startRow, int pageSize, string sortExpression)

        #region public int RequestCount()
        public int RequestCount()
        {
            int count = ScgDbQueryProvider.DbBankQuery.CountByBankCriteria(new BankLang());
            return count;
        }
        #endregion public int RequestCount()

        #region protected void ctlGridBank_RowCommand(object sender, GridViewCommandEventArgs e)
        protected void ctlGridBank_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "BankEdit")
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                short bankId = UIHelper.ParseShort(ctlGridBank.DataKeys[rowIndex].Value.ToString());

                ctlGridBank.EditIndex = rowIndex;
                IList<DbBank> bankList = new List<DbBank>();
                DbBank bank = DbBankService.FindByIdentity(bankId);

                bankList.Add(bank);

                ctlBankFormView.DataSource = bankList;
                ctlBankFormView.PageIndex = 0;

                ctlBankFormView.ChangeMode(FormViewMode.Edit);
                ctlBankFormView.DataBind();

                UpdatePanelBankForm.Update();
                ctlBankModalPopupExtender.Show();
                BankLangGridViewFinish();
            }
            else if (e.CommandName == "Select")
            {
                int rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                short bankId = UIHelper.ParseShort(ctlGridBank.DataKeys[rowIndex].Value.ToString());

                ctlBankLangGrid.DataSource = DbBankLangService.FindByBankId(bankId);
                ctlBankLangGrid.DataBind();

                if (ctlBankLangGrid.Rows.Count > 0)
                {
                    ctlSubmit.Visible = true;
                    ctlCancel.Visible = true;
                    ctlBankLangLangFds.Visible = true;
                }
                else
                {
                    ctlSubmit.Visible = false;
                    ctlCancel.Visible = false;
                    ctlBankLangLangFds.Visible = false;
                }
                ctlBankLangUpdatePanel.Update();
            }
        }
        #endregion protected void ctlGridBank_RowCommand(object sender, GridViewCommandEventArgs e)

        #region protected void ctlGridBank_DataBound(object sender, EventArgs e)
        protected void ctlGridBank_DataBound(object sender, EventArgs e)
        {
            if (ctlGridBank.Rows.Count > 0)
            {
                RegisterScriptForGridView();

                ctlBtnDeleteBank.OnClientClick = String.Format("return ConfirmDelete('{0}')", ctlGridBank.ClientID);
                BankLangGridViewFinish();
            }
        }
        #endregion protected void ctlGridBank_DataBound(object sender, EventArgs e)

        #region protected void ctlGridBank_PageIndexChanged(object sender, EventArgs e)
        protected void ctlGridBank_PageIndexChanged(object sender, EventArgs e)
        {
            if (ctlGridBank.Rows.Count > 0)
            {
                divButton.Visible = true;
                RegisterScriptForGridView();

                ctlBtnDeleteBank.OnClientClick = String.Format("return ConfirmDelete('{0}')", ctlGridBank.ClientID);
            }
            else
            {
                divButton.Visible = false;
            }
        }
        #endregion protected void ctlGridBank_PageIndexChanged(object sender, EventArgs e)

        #region protected void ctlBtnAddBank_Click(object sender, ImageClickEventArgs e)
        protected void ctlBtnAddBank_Click(object sender, ImageClickEventArgs e)
        {
            BankLangGridViewFinish();
            ctlBankFormView.DataSource = null;
            ctlBankFormView.ChangeMode(FormViewMode.Insert);
            UpdatePanelBankForm.Update();
            ctlBankModalPopupExtender.Show();
        }
        #endregion protected void ctlBtnAddBank_Click(object sender, ImageClickEventArgs e)

        #region protected void ctlBtnDeleteBank_Click(object sender, EventArgs e)
        protected void ctlBtnDeleteBank_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in ctlGridBank.Rows)
            {
                if ((row.RowType == DataControlRowType.DataRow) && ((CheckBox)row.FindControl("ctlSelect")).Checked)
                {

                    short bankId = UIHelper.ParseShort(ctlGridBank.DataKeys[row.RowIndex].Value.ToString());
                    if (!((CheckBox)row.FindControl("ctlChkActive")).Checked)//ไม่ Active
                    {
                        try
                        {
                            BankLangGridViewFinish();
                            DbBank bank = DbBankService.FindProxyByIdentity(bankId);
                            ScgDbDaoProvider.DbBankLangDao.DeleteAllBankLang(bankId);
                            DbBankService.Delete(bank);
                            UpdatePanelGridView.Update();
                        }
                        catch (Exception ex)
                        {
                            if (((System.Data.SqlClient.SqlException)(ex.GetBaseException())).Number == 547)
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertInUseData",
                                    "alert('Bank ID : " + bankId.ToString() + " is Active. Can't Delete It');", true);
                            }
                        }
                    }
                    else//Active
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertActiveData",
                                    "alert('Bank ID :" + bankId.ToString() + " is Active. Can't Delete It');", true);
                    }
                }
            }

            ctlGridBank.DataCountAndBind();
        }
        #endregion protected void ctlBtnDeleteBank_Click(object sender, EventArgs e)

        #endregion <== Main Program ==>
        
        #region <== Form View ==>

        #region protected void ctlBankFormView_ModeChanging(object sender, FormViewModeEventArgs e)
        protected void ctlBankFormView_ModeChanging(object sender, FormViewModeEventArgs e)
        {

        }
        #endregion protected void ctlBankFormView_ModeChanging(object sender, FormViewModeEventArgs e)

        #region protected void ctlBankFormView_ItemCommand(object sender, FormViewCommandEventArgs e)
        protected void ctlBankFormView_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            if (e.CommandName == "Cancel")
            {
                ctlBankModalPopupExtender.Hide();
                UpdatePanelBankForm.Update();
                ctlGridBank.DataCountAndBind();
            }
        }
        #endregion protected void ctlBankFormView_ItemCommand(object sender, FormViewCommandEventArgs e)

        #region protected void ctlBankFormView_DataBound(object sender, EventArgs e)
        protected void ctlBankFormView_DataBound(object sender, EventArgs e)
        {
            //if (ctlBankFormView.CurrentMode.Equals(FormViewMode.Edit))
            //{
            //    Label ctlBankID = ctlBankFormView.FindControl("ctlLblBankIDShow") as Label;
            //    //Label ctlBankName = ctlBankFormView.FindControl("ctlLblBankNameShow") as Label;
            //    TextBox ctlBankType = ctlBankFormView.FindControl("ctlTxtAccType") as TextBox;
            //    ctlBankType.Focus();

            //    LinkButton ctlBankIDInGrid = ctlGridBank.Rows[ctlGridBank.EditIndex].FindControl("ctlLinkBankNo") as LinkButton;
            //    Label ctlBankNameInGrid = ctlGridBank.Rows[ctlGridBank.EditIndex].FindControl("ctlLinkBankName") as Label;

            //    ctlBankID.Text = ctlBankIDInGrid.Text;
            //    //ctlBankName.Text = ctlBankNameInGrid.Text;
            //}
        }
        #endregion protected void ctlBankFormView_DataBound(object sender, EventArgs e)

        #region protected void ctlBankFormView_ItemInserting(object sender, FormViewInsertEventArgs e)
        protected void ctlBankFormView_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            DbBank dbBank = new DbBank();

            TextBox txtBankNo = ctlBankFormView.FindControl("ctlTxtBankNo") as TextBox;
            TextBox txtComment = ctlBankFormView.FindControl("ctlTxtComment") as TextBox;
            CheckBox chkActive = ctlBankFormView.FindControl("chkActive") as CheckBox;

            dbBank.BankNo = txtBankNo.Text;
            dbBank.Comment = txtComment.Text;
            dbBank.Active = chkActive.Checked;

            dbBank.UpdPgm = ProgramCode;
            dbBank.CreDate = DateTime.Now.Date;
            dbBank.UpdDate = DateTime.Now.Date;
            dbBank.CreBy = UserAccount.UserID;
            dbBank.UpdBy = UserAccount.UserID;

            try
            {
                DbBankService.Save(dbBank);
                e.Cancel = true;
                ctlGridBank.DataCountAndBind();
                ctlBankModalPopupExtender.Hide();
                UpdatePanelGridView.Update();
            }
            catch
            { }
            //catch (ServiceValidationException ex)
            //{
            //    ValidationErrors.MergeErrors(ex.ValidationErrors);
            //}
        }
        #endregion protected void ctlAccountFormView_ItemInserting(object sender, FormViewInsertEventArgs e)

        #region protected void ctlBankFormView_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        protected void ctlBankFormView_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            short bankId = UIHelper.ParseShort(ctlBankFormView.DataKey.Value.ToString());
            DbBank dbBank = DbBankService.FindByIdentity(bankId);

            TextBox txtBankNo = ctlBankFormView.FindControl("ctlTxtBankNo") as TextBox;
            TextBox txtComment = ctlBankFormView.FindControl("ctlTxtComment") as TextBox;
            CheckBox chkActive = ctlBankFormView.FindControl("chkActive") as CheckBox;

            dbBank.BankNo = txtBankNo.Text;
            dbBank.Comment = txtComment.Text;
            dbBank.Active = chkActive.Checked;

            dbBank.UpdPgm = ProgramCode;
            dbBank.UpdDate = DateTime.Now.Date;
            dbBank.UpdBy = UserAccount.UserID;

            try
            {
                DbBankService.SaveOrUpdate(dbBank);

                // Cancel insert with DataSource.
                e.Cancel = true;
                ctlGridBank.DataCountAndBind();
                ctlBankModalPopupExtender.Hide();
                UpdatePanelGridView.Update();
            }
            catch
            { }
            //catch ( ServiceValidationException ex)
            //{
            //    ValidationErrors.MergeErrors(ex.ValidationErrors);
            //}
        }
        #endregion protected void ctlBankFormView_ItemUpdating(object sender, FormViewUpdateEventArgs e)

        #endregion <== Form View ==>

        #region <== Grid Language ==>

        #region protected void ctlBankLangGrid_DataBound(object sender, EventArgs e)
        protected void ctlBankLangGrid_DataBound(object sender, EventArgs e)
        {
            foreach (GridViewRow row in ctlBankLangGrid.Rows)
            {
                TextBox ctlBankName = (TextBox)ctlBankLangGrid.Rows[row.RowIndex].FindControl("ctlBankName");
                TextBox ctlABBRName = (TextBox)ctlBankLangGrid.Rows[row.RowIndex].FindControl("ctlABBRName");
                TextBox ctlComment = (TextBox)ctlBankLangGrid.Rows[row.RowIndex].FindControl("ctlComment");
                CheckBox ctlActive = (CheckBox)ctlBankLangGrid.Rows[row.RowIndex].FindControl("ctlActive");

                if (string.IsNullOrEmpty(ctlBankName.Text) && string.IsNullOrEmpty(ctlABBRName.Text) && string.IsNullOrEmpty(ctlComment.Text))
                {
                    ctlActive.Checked = true;
                }
            }
        }
        #endregion protected void ctlBankLangGrid_DataBound(object sender, EventArgs e)

        #region protected void ctlSubmit_Click(object sender, EventArgs e)
        protected void ctlSubmit_Click(object sender, EventArgs e)
        {
            IList<DbBankLang> bankLangList = new List<DbBankLang>();
            DbBank bank = new DbBank(UIHelper.ParseShort(ctlGridBank.SelectedValue.ToString()));

            foreach (GridViewRow row in ctlBankLangGrid.Rows)
            {
                TextBox ctlBankName = (TextBox)ctlBankLangGrid.Rows[row.RowIndex].FindControl("ctlBankName");
                TextBox ctlABBRName = (TextBox)ctlBankLangGrid.Rows[row.RowIndex].FindControl("ctlABBRName");
                TextBox ctlComment = (TextBox)ctlBankLangGrid.Rows[row.RowIndex].FindControl("ctlComment");
                CheckBox ctlActive = (CheckBox)ctlBankLangGrid.Rows[row.RowIndex].FindControl("ctlActive");

                if (!string.IsNullOrEmpty(ctlBankName.Text) || !string.IsNullOrEmpty(ctlABBRName.Text) || !string.IsNullOrEmpty(ctlComment.Text))
                {
                    //modify by tom 28/01/2009
                    //SS.DB.DTO.DbLanguage lang = new SS.DB.DTO.DbLanguage(UIHelper.ParseShort(ctlBankLangGrid.DataKeys[row.RowIndex].Value.ToString()));
                    DbLanguage lang = new DbLanguage(UIHelper.ParseShort(ctlBankLangGrid.DataKeys[row.RowIndex].Value.ToString()));
                     
                    DbBankLang bankLang = new DbBankLang();

                    bankLang.Language = lang;
                    bankLang.Bank = bank;
                    bankLang.BankName = ctlBankName.Text;
                    bankLang.AbbrName = ctlABBRName.Text;
                    bankLang.Comment = ctlComment.Text;
                    bankLang.Active = ctlActive.Checked;

                    bankLang.CreBy = UserAccount.UserID;
                    bankLang.CreDate = DateTime.Now;
                    bankLang.UpdBy = UserAccount.UserID; ;
                    bankLang.UpdDate = DateTime.Now;
                    bankLang.UpdPgm = ProgramCode;

                    bankLangList.Add(bankLang);
                }
            }
            DbBankLangService.UpdateBankLang(bankLangList);

            ctlGridBank.DataCountAndBind();
            UpdatePanelGridView.Update();

            BankLangGridViewFinish();
        }
        #endregion protected void ctlSubmit_Click(object sender, EventArgs e)

        #region protected void ctlCancel_Click(object sender, EventArgs e)
        protected void ctlCancel_Click(object sender, EventArgs e)
        {
            BankLangGridViewFinish();
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
            script.Append("'" + ctlGridBank.ClientID + "', '" + ctlGridBank.HeaderRow.FindControl("ctlSelectHeader").ClientID + "'); ");
            script.Append("} ");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "validateChkBox", script.ToString(), true);
        }
        #endregion private void RegisterScriptForGridView()

        #region public void BankLangGridViewFinish()
        public void BankLangGridViewFinish()
        {
            ctlBankLangGrid.DataSource = null;
            ctlBankLangGrid.DataBind();
            ctlBankLangUpdatePanel.Update();
            ctlSubmit.Visible = false;
            ctlCancel.Visible = false;
            ctlBankLangLangFds.Visible = false;
            ctlGridBank.SelectedIndex = -1;
        }
        #endregion public void BankLangGridViewFinish()

        #endregion <== Function ==>
    }
}
