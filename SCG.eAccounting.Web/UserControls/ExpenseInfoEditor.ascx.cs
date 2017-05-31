using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using System.Text;
using SCG.eAccounting.Web.Helper;
using SCG.DB.DTO;
using SCG.DB.Query;
using SCG.DB.BLL;

namespace SCG.eAccounting.Web.UserControls
{
    public partial class ExpenseInfoEditor : BaseUserControl
    {
        //public ISuUserFavoriteActorService SuUserFavoriteActorService { get; set; }
        //public ISuUserService SuUserService { get; set; }
        public IDbAccountService DbAccountService { get; set; }
        public long ExpenseGroupID
        {
            get { return UIHelper.ParseLong(epgID.Value); }
            set { epgID.Value = value.ToString(); }
        }
        public long AccountID
        {
            get { return UIHelper.ParseLong(account.Value); }
            set { account.Value = value.ToString(); }
        }
        public void Initialize(long id)
        {
            ExpenseGroupID = id;

            ctlExpenseInfoGrid.DataCountAndBind();
            ctlExpenseInfoUpdatePanel.Update();
            

        }
        private void RefreshGridData(object sender, EventArgs e)
        {
            ctlExpenseInfoGrid.DataCountAndBind();
            ctlExpenseInfoUpdatePanel.Update();
            ctlAccountLangEditor.HidePopUp();

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            ctlAccountLangEditor.Notify_Ok += new EventHandler(RefreshGridData);
            ctlAccountLangEditor.Notify_Cancle+= new EventHandler(RefreshGridData);
          
        }
      
        protected void addExpense_Click(object sender, ImageClickEventArgs e)
        {
            ctlCompanyInfoEditor.HideDetail();
            ctlAccountLangEditor.Initialize(FlagEnum.NewFlag, 0, ExpenseGroupID);
            ctlAccountLangEditor.ShowPopUp();
        }
        protected void closeExpense_Click(object sender, ImageClickEventArgs e)
        {
            ctlCompanyInfoEditor.HideDetail();
            ctlExpenseInfoUpdatePanel.Update();
            HideDetail();
        }


        protected void ExpenseInfoGrid_PageIndexChanged(object sender, EventArgs e)
        {
            ExpenseInfoGridViewFinish();
        }
        public void ExpenseInfoGridViewFinish()
        {
            ctlExpenseInfoGrid.SelectedIndex = -1;
            ctlExpenseInfoGrid.DataBind();
            ctlExpenseInfoUpdatePanel.Update();

        }
     
        protected void Expense_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex;
            if(e.CommandName.Equals("Company"))
            {
                rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;          
                AccountID = UIHelper.ParseShort(ctlExpenseInfoGrid.DataKeys[rowIndex].Values["AccountID"].ToString());
                ctlExpenseInfoGrid.SelectedIndex = rowIndex;
                ctlCompanyInfoEditor.Initialize(AccountID);
                ctlCompanyInfoEditor.ShowDetail();
                
            }
            if (e.CommandName.Equals("ExpenseInfoEdit"))
            {
                rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                ExpenseGroupID = UIHelper.ParseShort(ctlExpenseInfoGrid.DataKeys[rowIndex].Values["ExpenseGroupID"].ToString());
                AccountID = UIHelper.ParseShort(ctlExpenseInfoGrid.DataKeys[rowIndex].Values["AccountID"].ToString());
                ctlAccountLangEditor.Initialize(FlagEnum.EditFlag, AccountID, ExpenseGroupID);
                ctlAccountLangEditor.ShowPopUp();
            }
            if (e.CommandName.Equals("ExpenseInfoDelete"))
            {
                try
                {
                    rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                    ExpenseGroupID = UIHelper.ParseShort(ctlExpenseInfoGrid.DataKeys[rowIndex].Values["ExpenseGroupID"].ToString());
                    AccountID = UIHelper.ParseShort(ctlExpenseInfoGrid.DataKeys[rowIndex].Values["AccountID"].ToString());
                    DbAccount ac = ScgDbQueryProvider.DbAccountQuery.FindAccountByEGroupIDAID(ExpenseGroupID, AccountID);
                    DbAccountService.DeleteAccount(ac);
                    ctlCompanyInfoEditor.HideDetail();
                }
                catch (Exception ex)
                {
                    if (((System.Data.SqlClient.SqlException)(ex.GetBaseException())).Number == 547)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertInUseData",
                            "alert('This data is now in use.');", true);
                        ctlExpenseInfoGrid.DataCountAndBind();
                    }
                }

                ctlExpenseInfoGrid.DataCountAndBind();
                ctlExpenseInfoUpdatePanel.Update();

            }
        }
        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {

            return ScgDbQueryProvider.DbAccountQuery.GetAccountInfoByAccount(ExpenseGroupID, startRow, pageSize, sortExpression);
        }
        public int RequestCount()
        {
          
            return ScgDbQueryProvider.DbAccountQuery.CountAccountInfo(ExpenseGroupID);
       
        }
       
      
        public void ShowDetail()
        {
            ctlExpenseInfoFieldSet.Visible = true;
            ctlCompanyInfoEditor.HideDetail();
        }
        public void HideDetail()
        {
            ctlCompanyInfoEditor.HideDetail();
            ctlExpenseInfoFieldSet.Visible = false;

        }
    }
}