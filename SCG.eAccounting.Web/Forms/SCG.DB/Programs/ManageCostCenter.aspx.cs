using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SCG.DB.Query;
using SS.Standard.UI;
using SCG.DB.DTO;
using SCG.eAccounting.Web.Helper;
using SCG.DB.BLL;
using SS.Standard.Utilities;
using SCG.eAccounting.Web.UserControls;

namespace SCG.eAccounting.Web.Forms.SCG.DB.Programs
{
    public partial class ManageCostCenter : BasePage
    {
        public IDbCostCenterService DbCostCenterService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            ctlCostCenterEditor.Notify_Ok += new EventHandler(RefreshUpdatePanel);
            
            if (!Page.IsPostBack)
            {
                // 01-06-2009 แก้ไขตาม Spec 
                //ctlGridView .DataCountAndBind();
                //ctlUpdPanelGridView.Update();
            }
        }

        protected void ctlSearch_Click(object sender, EventArgs e)
        {
            ctlGridView.DataCountAndBind();
            ctlUpdPanelGridView.Update();
        }

        private void RefreshUpdatePanel(object sender,EventArgs e)
        {
            ctlGridView.DataCountAndBind();
            ctlUpdPanelGridView.Update();
        }
        
        protected Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            DbCostCenter costCenter = new DbCostCenter();
            costCenter.CostCenterCode = ctlTxtCostCenterCode.Text;
            costCenter.Description = ctlTxtCostCenterDescription.Text;
            return ScgDbQueryProvider.DbCostCenterQuery.FindCostCenterCompany(costCenter,  startRow, pageSize, sortExpression);
        }

        protected int RequestCount()
        {
            DbCostCenter costCenter = new DbCostCenter();
            costCenter.CostCenterCode = ctlTxtCostCenterCode.Text;
            costCenter.Description = ctlTxtCostCenterDescription.Text;
            return ScgDbQueryProvider.DbCostCenterQuery.CountCostCenterCompany(costCenter);
        }

        protected void Add_Click(object sender, ImageClickEventArgs e)
        {
            ctlCostCenterEditor.Initial("New", 0);
            ctlCostCenterEditor.ShowPopUp();
        }

        protected void ctlGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            #region e.CommandName == "Delete"
            if (e.CommandName == "DeleteCostCenter")
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                long costCenterID = UIHelper.ParseLong(ctlGridView.DataKeys[rowIndex].Value.ToString());

                DbCostCenter costCenter = new DbCostCenter();
                costCenter.CostCenterID = costCenterID;
                try
                {
                    DbCostCenterService.DeleteCostCenter(costCenter);
                }
                catch (ServiceValidationException ex)
                {
                    ValidationErrors.MergeErrors(ex.ValidationErrors);
                }
            }
            #endregion
            #region e.CommandName == "Edit"
            else if (e.CommandName == "EditCostCenter")
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                long costCenterID = UIHelper.ParseLong(ctlGridView.DataKeys[rowIndex].Value.ToString());
                ctlCostCenterEditor.Initial(FlagEnum.EditFlag, costCenterID);
                ctlCostCenterEditor.ShowPopUp();


            }
            #endregion
            RefreshUpdatePanel(sender, e);
        }
    }
}
