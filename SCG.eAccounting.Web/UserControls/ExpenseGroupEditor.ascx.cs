using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SCG.DB.DTO;
using SCG.DB.BLL;
using SCG.eAccounting.Web.Helper;
using SS.DB.DTO;
using SS.Standard.Utilities;
using SCG.DB.Query;

namespace SCG.eAccounting.Web.UserControls
{
    public partial class ExpenseGroupEditor : BaseUserControl
    {
      
        public IDbExpenseGroupLangService DbExpenseGroupLangService { get; set; }
        public IDbExpenseGroupService DbExpenseGroupService { get; set; }
        public event EventHandler Notify_Ok;
        public event EventHandler Notify_Cancle;

        public string Mode
        {
            get { return this.ViewState["Mode"] == null ? FlagEnum.NewFlag : (string)this.ViewState["Mode"]; }
            set { this.ViewState["Mode"] = value; }
        }
        public long EGroupID
        {
            get { return this.ViewState["EGroupID"] == null ? (long)0 : (long)this.ViewState["EGroupID"]; }
            set { this.ViewState["EGroupID"] = value; }
        }

        public void ResetValue()
        {
            //reset value and update grid&panel

            ctlExpenseGroup.Text = string.Empty;
            ctlActive.Checked = true;
            ctlExpenseEditorGrid.DataCountAndBind();
            ctlExpenseGroupUpdatePanel.Update();

        }
        public void Initialize(string mode, long expanseID)
        {
            Mode = mode.ToString();

            EGroupID = expanseID;
            if (mode.Equals(FlagEnum.EditFlag))
            {
                DbExpenseGroup ep = ScgDbQueryProvider.DbExpenseGroupQuery.FindProxyByIdentity(EGroupID);

                ctlExpenseGroup.Text = ep.ExpenseGroupCode;
                ctlActive.Checked = ep.Active;   
                ctlExpenseEditorGrid.DataCountAndBind();
                ctlExpenseGroupUpdatePanel.Update();

            }
            else if (mode.ToString() == FlagEnum.NewFlag)
            {
                ResetValue();

            }

        }
        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            return ScgDbQueryProvider.DbExpenseGroupLangQuery.FindExpenseGroupLang(EGroupID);
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void ctlAdd_Click(object sender, ImageClickEventArgs e)
        {

            DbExpenseGroup ep = new DbExpenseGroup();
         
            if (Mode.Equals(FlagEnum.EditFlag))
            {
                ep.ExpenseGroupID = EGroupID;
               

            }
            ep.ExpenseGroupCode=ctlExpenseGroup.Text;
            ep.Active = ctlActive.Checked;

            /// save in service
            ep.CreBy = UserAccount.UserID;
            ep.CreDate = DateTime.Now;
            ep.UpdBy = UserAccount.UserID;
            ep.UpdDate = DateTime.Now;
            ep.UpdPgm = UserAccount.CurrentProgramCode;
            

            try
            {
                // save or update PB
                if (Mode.Equals(FlagEnum.EditFlag))
                {
                    DbExpenseGroupService.UpdateExpenseGroup(ep);
                }
                else
                {
                    long id = DbExpenseGroupService.AddExpenseGroup(ep);
                    ep.ExpenseGroupID = id;

                }

                // save or update PBlang
                IList<DbExpenseGroupLang> list = new List<DbExpenseGroupLang>();

                foreach (GridViewRow row in ctlExpenseEditorGrid.Rows)
                {
                    short languageId = UIHelper.ParseShort(ctlExpenseEditorGrid.DataKeys[row.RowIndex]["LanguageID"].ToString());

                    TextBox description = row.FindControl("ctrDescription") as TextBox;
                    TextBox comment = (TextBox)row.FindControl("ctrComment") as TextBox;
                    CheckBox active = (CheckBox)row.FindControl("ctlActive") as CheckBox;

                    if ((!string.IsNullOrEmpty(description.Text)))
                    {
                        DbExpenseGroupLang exLang = new DbExpenseGroupLang();
                        exLang.Description = description.Text;
                        exLang.Comment = comment.Text;
                        exLang.Active = active.Checked;
                        // save in service 
                        exLang.CreBy = UserAccount.UserID;
                        exLang.CreDate = DateTime.Now;
                        exLang.ExpenseGroupID = ep;
                        exLang.LanguageID = new DbLanguage(languageId);
                        exLang.UpdBy = UserAccount.UserID;
                        exLang.UpdDate = DateTime.Now;
                        exLang.UpdPgm = UserAccount.CurrentProgramCode;
                        list.Add(exLang);
                    }

                }

                if (Mode.Equals(FlagEnum.EditFlag))
                {
                    DbExpenseGroupLangService.UpdateExpenseGroupLang(list);
                }
                if (Mode.Equals(FlagEnum.NewFlag))
                {
                    DbExpenseGroupLangService.AddExpenseGroupLang(list);
                }

                Notify_Ok(sender, e);

            }
            catch (ServiceValidationException ex)
            {
                this.ValidationErrors.MergeErrors(ex.ValidationErrors);
            }


        }
        protected void ExpenseEditor_DataBound(object sender, EventArgs e)
        {
            foreach (GridViewRow row in ctlExpenseEditorGrid.Rows)
            {
                TextBox ctrDescription = row.FindControl("ctrDescription") as TextBox;
                TextBox ctrComment = row.FindControl("ctrComment") as TextBox;
                CheckBox active = row.FindControl("ctlActive") as CheckBox;

                if ((string.IsNullOrEmpty(ctrDescription.Text)) && (string.IsNullOrEmpty(ctrComment.Text)))
                {

                    active.Checked = true;
                }
            }
        }
        protected void ctlCancel_Click(object sender, ImageClickEventArgs e)
        {
            if (Notify_Cancle != null)
            {
                Notify_Cancle(sender, e);
            }
            HidePopUp();

        }
        protected void Expense_RowCommand(object sender, GridViewCommandEventArgs e)
        {
        }
        public void HidePopUp()
        {
            ctlExpenseEditorModalPopupExtender.Hide();
        }
        public void ShowPopUp()
        {
            ctlExpenseEditorModalPopupExtender.Show();

        }
    }
}