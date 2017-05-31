using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;

namespace SCG.eAccounting.Web.UserControls.DocumentEditor.Components
{
    public partial class ExpVat : BaseUserControl
    {

        public struct Expense
        {
            public string CostCenter { get; set; }
            public string AccountCode { get; set; }
            public string InternalOrder { get; set; }
            public string Description { get; set; }
            public string Amount { get; set; }
            public string ReferanceNo { get; set; }
            public string FromdDate { get; set; }
            public string ToDate { get; set; }
            public string TotalAmount { get; set; }
        }

        public IList<Expense> ExpenseList()
        {
            IList<Expense> expenseList = new List<Expense>();
            Expense expense = new Expense();
            expense.CostCenter = "0110-9430";
            expense.AccountCode = "0311873800-ค่าสัมมนาในประเทศ";
            expense.InternalOrder = "640310";
            expense.Description = "ค่าสัมนา (ปีแห่งการลงทุน) ";
            expense.FromdDate = DateTime.Now.ToShortDateString();
            expense.ToDate = DateTime.Now.ToShortDateString();
            expense.Amount = "934.50";
            expense.ReferanceNo = "";
            expenseList.Add(expense);

            expense = new Expense();

            expense.CostCenter = "0110-9430";
            expense.AccountCode = "0311873800-ค่รสัมมนาในประเทศ";
            expense.InternalOrder = "640310";
            expense.Description = "ค่าสัมนา (ปีแห่งการลงทุน) ";
            expense.FromdDate = DateTime.Now.ToShortDateString();
            expense.ToDate = DateTime.Now.ToShortDateString();
            expense.Amount = "15,000.00";
            expense.ReferanceNo = "";
            expenseList.Add(expense);

            return expenseList;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
                ctlExpenseGridview.DataSource = ExpenseList();
                ctlExpenseGridview.DataBind();
                ctlChkDiv.Visible = true;
            
        }

        protected void ctlVatChk_CheckedChanged(object sender, EventArgs e)
        {
            if (ctlVatChk.Checked)
            {
                ctlInvoicefds.Visible = true;
                ctlVAT.Visible = true;
            }
            if (!ctlVatChk.Checked)
            {
                ctlVAT.Visible = false;
            }
            if (!ctlVatChk.Checked && !ctlWhtChk.Checked)
            {
                ctlInvoicefds.Visible = false;
                ctlwht.Visible = false;
                ctlVAT.Visible = false;
            }
        }

        protected void ctlWhtChk_CheckedChanged(object sender, EventArgs e)
        {
            if (ctlWhtChk.Checked)
            {
                ctlInvoicefds.Visible = true;
                ctlwht.Visible = true;
            }
            if(!ctlWhtChk.Checked)
            {
                ctlwht.Visible = false;
            }
            if (!ctlVatChk.Checked && !ctlWhtChk.Checked)
            {
                ctlInvoicefds.Visible = false;
                ctlwht.Visible = false;
                ctlVAT.Visible = false;
            }
        }
        protected void ctlAdd_Click(object sender, EventArgs e)
        {
            ctlExpenseGridview.DataSource = ExpenseList();
            ctlExpenseGridview.DataBind();
            ctlChkDiv.Visible = true;


        }
        public void Show()
        {
            CallOnObjectLookUpCalling();
            ModalPopupExtender1.Show();
            this.UpdatePanelGridView.Update();
        }

        protected void ctlSubmit_Click(object sender, ImageClickEventArgs e)
        {
            ModalPopupExtender1.Hide();
            UpdatePanelGridView.Update();
        }

        protected void ctlCancel_Click(object sender, ImageClickEventArgs e)
        {
            ModalPopupExtender1.Hide();
            UpdatePanelGridView.Update();
        }
    }
}