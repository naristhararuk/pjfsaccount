using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SCG.eAccounting.Web.Helper;
using SCG.eAccounting.BLL.Implement;
using SCG.eAccounting.BLL;
using SCG.eAccounting.DTO.DataSet;
using SCG.eAccounting.Query;

namespace SCG.eAccounting.Web.UserControls.DocumentEditor.Components
{
    public partial class ExpensesMPA : BaseUserControl
    {
        public ExpenseDocumentEditor ExpenseDocumentEditor { get; set; }
        public IFnExpenseDocumentService FnExpenseDocumentService { get; set; }
        public ITransactionService TransactionService { get; set; }
        private bool refreshHeaderGrid;
        public string InitialFlag
        {
            get { return ViewState[ViewStateName.InitialFlag].ToString(); }
            set { ViewState[ViewStateName.InitialFlag] = value; }
        }

        public long CompanyID
        {
            get { return UIHelper.ParseLong(ViewState["CompanyID"].ToString()); }
            set { ViewState["CompanyID"] = value; }
        }

        public long RequesterID
        {
            get { return UIHelper.ParseLong(ViewState["RequesterID"].ToString()); }
            set { ViewState["RequesterID"] = value; }
        }

        public long? CurrentUserID
        {
            set { ViewState["CurrentUserID"] = value; }
            get { return ViewState["CurrentUserID"] == null ? 0 : UIHelper.ParseLong(ViewState["CurrentUserID"].ToString()); }
        }

        public Guid TransactionId
        {
            get { return (Guid)ViewState[ViewStateName.TransactionID]; }
            set { ViewState[ViewStateName.TransactionID] = value; }
        }
        public long ExpDocumentID
        {
            get { return (long)ViewState[ViewStateName.DocumentID]; }
            set { ViewState[ViewStateName.DocumentID] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ctlMPADocumentLookup.OnObjectLookUpCalling += new BaseUserControl.ObjectLookUpCallingEventHandler(ExpensesMPA_OnObjectLookUpCalling);
            ctlMPADocumentLookup.OnObjectLookUpReturn += new BaseUserControl.ObjectLookUpReturnEventHandler(ExpensesMPA_OnObjectLookUpReturn);
        }

        public void Initialize(Guid txID, long documentID, string initFlag)
        {
            this.TransactionId = txID;
            this.ExpDocumentID = documentID;
            this.InitialFlag = initFlag;
            BindControl(true);
        }


        protected void ctlExpeseMPAGridView_DataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal ctlNoLabel = e.Row.FindControl("ctlNoLabel") as Literal;

                ctlNoLabel.Text = ((ctlExpeseMPAGridView.PageSize * ctlExpeseMPAGridView.CustomPageIndex) + (e.Row.RowIndex + 1)).ToString();
                if (InitialFlag.Equals(FlagEnum.ViewFlag))
                {
                    ((ImageButton)e.Row.FindControl("ctlDelete")).Visible = false;
                }
            }
        }

        protected void ExpensesMPA_OnObjectLookUpCalling(object sender, ObjectLookUpCallingEventArgs e)
        {
            UserControls.LOV.SCG.eAccounting.MPALookup expensesMPALookUp = sender as UserControls.LOV.SCG.eAccounting.MPALookup;
            expensesMPALookUp.CompanyID = CompanyID;
            expensesMPALookUp.RequesterID = RequesterID;
            expensesMPALookUp.CurrentUserID = CurrentUserID;
        }

        protected void ExpensesMPA_OnObjectLookUpReturn(object sender, ObjectLookUpReturnArgs e)
        {
            if (e.ObjectReturn != null)
            {
                IList<SCG.eAccounting.DTO.ValueObject.ExpensesMPA> list = (IList<SCG.eAccounting.DTO.ValueObject.ExpensesMPA>)e.ObjectReturn;
                FnExpenseDocumentService.AddExpenseMPAToTransaction(this.TransactionId, this.ExpDocumentID, list);
                this.BindExpenseMPAGridView();

                if (!ExpenseDocumentEditor.NotifyUpdateExpense()) return;

                ExpenseDocumentEditor.NotifyPaymentDetailChange();
            }
            ctlUpdatePanelExpenseGeneral.Update();
        }

        public void BindControl(bool refreshHeader)
        {
            this.refreshHeaderGrid = refreshHeader;
            if (InitialFlag.Equals(FlagEnum.ViewFlag))
            {
                ctlAddExpenseMPA.Visible = false;
                ctlExpeseMPAGridView.Columns[4].Visible = false;
            }
            else
            {
                ctlAddExpenseMPA.Visible = true;
                ctlExpeseMPAGridView.Columns[4].Visible = true;
            }
            BindExpenseMPAGridView();
            ctlUpdatePanelExpenseGeneral.Update();
        }

        public void BindExpenseMPAGridView()
        {
            ExpenseDataSet expenseDS = (ExpenseDataSet)TransactionService.GetDS(this.TransactionId);
            ExpenseDataSet.FnExpenseMPARow[] rows = (ExpenseDataSet.FnExpenseMPARow[])expenseDS.FnExpenseMPA.Select();

            List<long> expenseMPAIdList = new List<long>();
            foreach (ExpenseDataSet.FnExpenseMPARow row in rows)
            {
                // Prepare list of advance id for query
                expenseMPAIdList.Add(row.MPADocumentID);
            }

            if (expenseMPAIdList.Count > 0)
            {
                ctlExpeseMPAGridView.DataSource = ScgeAccountingQueryProvider.MPADocumentQuery.FindByExpenseMPAID(expenseMPAIdList);
                ctlExpeseMPAGridView.DataBind();
            }
            else
            {
                ctlExpeseMPAGridView.DataSource = null;
                ctlExpeseMPAGridView.DataBind();
            }

            ctlUpdatePanelExpenseGeneral.Update();
        }

        protected void ctlctlAddExpenseMPA_Click(object sender, EventArgs e)
        {
            if (!ExpenseDocumentEditor.NotifyUpdateExpense()) return;
            ctlMPADocumentLookup.Show();

        }

        protected void ctlctlAddExpenseMPA_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("DeleteExpensesMPA"))
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                long mpaDocumentID = UIHelper.ParseLong(ctlExpeseMPAGridView.DataKeys[rowIndex]["MPADocumentID"].ToString());
                FnExpenseDocumentService.DeleteExpenseMPAFromTransaction(this.TransactionId, mpaDocumentID);
                this.BindExpenseMPAGridView();
                this.ExpenseDocumentEditor.NotifyPaymentDetailChange();
            }
            else if (e.CommandName.Equals("PopupDocument"))
            {
                int rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                long workflowID = UIHelper.ParseLong(ctlExpeseMPAGridView.DataKeys[rowIndex]["WorkflowID"].ToString());
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), string.Empty, "window.open('../Programs/DocumentView.aspx?wfid=" + workflowID.ToString() + "')", true);
            }
        }
    }
}