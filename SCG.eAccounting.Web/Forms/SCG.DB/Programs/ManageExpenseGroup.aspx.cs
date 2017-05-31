using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using System.Web.UI.HtmlControls;
using SCG.DB.Query;

using SCG.eAccounting.Web.Helper;
using SCG.DB.DTO;
using SCG.eAccounting.Web.UserControls;
using SCG.DB.BLL;
using SCG.DB.DTO.ValueObject;

namespace SCG.eAccounting.Web.Forms.SCG.DB.Programs
{
    public partial class ManageExpenseGroup : BasePage
        
    {
        #region property
        public IDbExpenseGroupService DbExpenseGroupService { set; get; }
        public long ExpenseGropID
        {
            get { return UIHelper.ParseLong(expense.Value); }
            set { expense.Value = value.ToString(); }
        }     
        #endregion

        #region on load
        private void RefreshGridData(object sender, EventArgs e)
        {
            ctlExpenseGroupEditor.HidePopUp();
            ctlExpenseGroupGrid.DataCountAndBind();
            ctlExpenseGroupUpdatePanel.Update();
        }
        protected override void OnPreLoad(EventArgs e)
        {
            base.OnPreLoad(e);
            //ProgramCode = "ManageExpenseGroup";
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            ctlExpenseGroupEditor.Notify_Ok += new EventHandler(RefreshGridData);
            ctlExpenseGroupEditor.Notify_Cancle += new EventHandler(RefreshGridData);
          

            if (!Page.IsPostBack)
            {
                ctlExpenseGroupGrid.DataCountAndBind();
                ctlExpenseGroupGrid.SelectedIndex = -1;
                
            }

        }
        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
        }
        #endregion

        #region ExpenseGroup Gridview Event
        protected void ExpenseGroupGrid_PageIndexChanged(object sender, EventArgs e)
        {
            ExpenseGroupGridViewFinish();
        }
        public void ExpenseGroupGridViewFinish()
        {
            ctlExpenseGroupGrid.SelectedIndex = -1;

            ctlExpenseGroupGrid.DataBind();
            ctlExpenseGroupUpdatePanel.Update();

        }

        protected void ExpenseGroup_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = 0;


            if (e.CommandName.Equals("ExpenseGroup"))
            {
                rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                ExpenseGropID = UIHelper.ParseLong(ctlExpenseGroupGrid.DataKeys[rowIndex].Value.ToString());
                ctlExpenseInfoEditor.Initialize(ExpenseGropID);
                ctlExpenseInfoEditor.ShowDetail();
         
            }
            if (e.CommandName.Equals("ExpenseGroupEdit"))
            {
                rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                ExpenseGropID = UIHelper.ParseLong(ctlExpenseGroupGrid.DataKeys[rowIndex].Value.ToString());
                // call for edit/add popup
                ctlExpenseGroupEditor.Initialize(FlagEnum.EditFlag, ExpenseGropID);
                ctlExpenseGroupEditor.ShowPopUp();
                ctlExpenseInfoEditor.HideDetail();
                
            }
            


            if (e.CommandName.Equals("ExpenseGroupDelete"))
            {
                try
                {
                    rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                    ExpenseGropID = UIHelper.ParseLong(ctlExpenseGroupGrid.DataKeys[rowIndex].Value.ToString());
                    // get DbExpenseGroup to delete
                    DbExpenseGroup expense = ScgDbQueryProvider.DbExpenseGroupQuery.FindByIdentity(ExpenseGropID);
                    DbExpenseGroupService.DeleteExpenseGroup(expense);
             

                }
                catch (Exception ex)
                {
                    if (((System.Data.SqlClient.SqlException)(ex.GetBaseException())).Number == 547)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertInUseData",
                            "alert('This data is now in use.');", true);
                        ctlExpenseGroupGrid.DataCountAndBind();
                    }
                }
                
            }

                ctlExpenseGroupGrid.SelectedIndex = rowIndex;
                ctlExpenseGroupGrid.DataCountAndBind();
                ctlExpenseGroupUpdatePanel.Update();

        }

        protected void ExpenseGroup_DataBound(object sender, EventArgs e)
        {
            
            
        }
        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            VOExpenseGroup criteria = GetExpenseGroupCriteria();
            return ScgDbQueryProvider.DbExpenseGroupQuery.GetExpenseGroupByCriteria(criteria, startRow, pageSize, sortExpression);
            
       
        }
        public int RequestCount()
        {
            VOExpenseGroup criteria = GetExpenseGroupCriteria();
            int count = ScgDbQueryProvider.DbExpenseGroupQuery.CountExpenseGroupByCriteria(criteria);
            return count;
        }
        #endregion

        #region set value method
        public VOExpenseGroup GetExpenseGroupCriteria()
        {
            VOExpenseGroup expGroup = new VOExpenseGroup();
            expGroup.ExpenseGroupCode = ctlExpenseGroupSearch.Text;
            expGroup.Description = ctlDescriptionSearch.Text;
          
            return expGroup;
        }
        #endregion
       
        #region button event
        protected void ExpenseGroupSearch_Click(object sender, ImageClickEventArgs e)
        {
            ctlExpenseGroupGrid.SelectedIndex = -1;
            ctlExpenseInfoEditor.HideDetail();
            ctlExpenseGroupGrid.DataCountAndBind();

        }
        protected void ctlAdd_Click(object sender, ImageClickEventArgs e)
        {
            // call for add/edit popup
  
            ctlExpenseGroupEditor.Initialize(FlagEnum.NewFlag, 0);
            ctlExpenseGroupEditor.ShowPopUp();
            ctlExpenseInfoEditor.HideDetail();
        }
        #endregion
        

    }
}
