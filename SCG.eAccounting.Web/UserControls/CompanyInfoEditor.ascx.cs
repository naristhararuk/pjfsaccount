using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SCG.eAccounting.Web.Helper;
using System.Text;
using SCG.DB.Query;
using SCG.DB.DTO;
using SCG.DB.BLL;
using System.Collections;

namespace SCG.eAccounting.Web.UserControls
{
    public partial class CompanyInfoEditor : BaseUserControl
    {
        //public ISuUserFavoriteActorService SuUserFavoriteActorService { get; set; }
        //public ISuUserService SuUserService { get; set; }
        public IDbAccountCompanyService DbAccountCompanyService { get; set; }
        //public event EventHandler Notify_Ok;
        //public event EventHandler Notify_Cancle;
        public long AccountID
        {
            get { return UIHelper.ParseLong(acount.Value); }
            set { acount.Value = value.ToString(); }
        }
        public void Initialize(long id)
        {
            AccountID = id;
            ctlCompanyInfoGrid.DataCountAndBind();
            ctlCompanyInfoUpdatePanel.Update();
            
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            ctlCompanyLookUp.OnObjectLookUpReturn += new BaseUserControl.ObjectLookUpReturnEventHandler(EditorLookup_OnObjectLookUpReturn);
        }


        protected void SaveCompanyInfo_Click(object sender, ImageClickEventArgs e)
        {
            ctlCompanyLookUp.Show();
         
        }
        protected void CloseCompanyInfo_Click(object sender, ImageClickEventArgs e)
        {
            ctlCompanyInfoUpdatePanel.Update();
            HideDetail();
        }
        protected void EditorLookup_OnObjectLookUpReturn(object sender, ObjectLookUpReturnArgs e)
        {
            ShowAdd.Text= null;
            ShowAdd.Visible = false;
            IList<DbCompany> account = (List<DbCompany>)e.ObjectReturn;

            IList<DbAccountCompany> list = new List<DbAccountCompany>();
            ArrayList groupComList = new ArrayList();

            foreach (GridViewRow row in ctlCompanyInfoGrid.Rows)
            {
                Label ctlCom = (Label)ctlCompanyInfoGrid.Rows[row.RowIndex].FindControl("ctlCompanyCode");

                groupComList.Add(ctlCom.Text);
            }
            foreach (DbCompany ac in account)
            {
                if (!groupComList.Contains(ac.CompanyCode))
                {
                    DbAccountCompany accountCompany = new DbAccountCompany();
                    accountCompany.AccountID = ScgDbQueryProvider.DbAccountQuery.FindByIdentity(AccountID);
                    accountCompany.UseParent = true;
                    accountCompany.Active = true;
                    accountCompany.CompanyID = ac;
                    accountCompany.CreBy = UserAccount.UserID;
                    accountCompany.CreDate = DateTime.Now;
                    accountCompany.UpdBy = UserAccount.UserID;
                    accountCompany.UpdDate = DateTime.Now;
                    accountCompany.UpdPgm = UserAccount.CurrentProgramCode;
             
                    list.Add(accountCompany);
                }

                else
                {
                    
                    ShowAdd.Text += "COMPANY CODE " + ac.CompanyCode + " ALREADY ADDED <br>";
                    ShowAdd.Visible = true;
                    
                }

            }

            DbAccountCompanyService.AddAccountCompanyList(list);      
            ctlCompanyInfoGrid.DataCountAndBind();
            ctlCompanyInfoUpdatePanel.Update();

        }
        protected void DeleteCompanyInfo_Click(object sender, ImageClickEventArgs e)
        {
            foreach (GridViewRow row in ctlCompanyInfoGrid.Rows)
            {
                if ((row.RowType == DataControlRowType.DataRow) && (((CheckBox)row.FindControl("ctlSelect")).Checked))
                {
                    try
                    {
                        long id = UIHelper.ParseLong(ctlCompanyInfoGrid.DataKeys[row.RowIndex]["ID"].ToString());
                        DbAccountCompany ac = ScgDbQueryProvider.DbAccountCompanyQuery.FindByIdentity(id);
                        DbAccountCompanyService.Delete(ac);

                    }
                    catch (Exception ex)
                    {
                        string exMessage = ex.Message;
                        Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
                        errors.AddError("DbAccountCompany.Error", new Spring.Validation.ErrorMessage("CannotDelete"));
                        ValidationErrors.MergeErrors(errors);
                    }
                }
            }
            ctlCompanyInfoGrid.DataCountAndBind();
            ctlCompanyInfoUpdatePanel.Update();


        }
    
        protected void CompanyInfoGrid_Databound(object sender, EventArgs e)
        {

            if (ctlCompanyInfoGrid.Rows.Count > 0)
            {
                RegisterScriptForGridView();
            }

        }
     
        protected Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            return ScgDbQueryProvider.DbAccountCompanyQuery.FindAccountCompanyByAccountID(AccountID);

        }
    
        
        private void RegisterScriptForGridView()
        {
            StringBuilder script = new StringBuilder();
            script.Append("function validateCheckBoxControl(objChk, objFlag) ");
            script.Append("{ ");
            script.Append("CheckboxesCheckUncheck(objChk, objFlag, ");
            script.Append("'" + ctlCompanyInfoGrid.ClientID + "', '" + ctlCompanyInfoGrid.HeaderRow.FindControl("ctlHeader").ClientID + "'); ");
            script.Append("} ");

            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "validateChkBox", script.ToString(), true);
        }
        
        public void ShowDetail()
        {
            ctlCompanyInfoFieldSet.Visible = true;
        }
        public void HideDetail()
        {
            ctlCompanyInfoFieldSet.Visible = false;

        }

        protected void CompanyInfoGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "CompanyEdit")
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                long accountCompanyId = Convert.ToInt64(ctlCompanyInfoGrid.DataKeys[rowIndex].Values[0]);
                long accountId = Convert.ToInt64(ctlCompanyInfoGrid.DataKeys[rowIndex].Values[1]);
                ctlExpenseCompanyEditor.Initialize(FlagEnum.EditFlag, accountId, accountCompanyId);
                ctlExpenseCompanyEditor.ShowPopUp();
            }
        }
       
    }
}