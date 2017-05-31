using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using SS.Standard.UI;
using SCG.eAccounting.SAP.BAPI.Service;
using SCG.eAccounting.SAP;
using SCG.eAccounting.SAP.BAPI.Service.Posting;
using SCG.eAccounting.SAP.BAPI.Service.Const;
using System.Collections.Generic;

namespace SCG.eAccounting.Web.Forms.SCG.eAccounting.Programs
{
    public partial class TestPostingData : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnViewPost_Click(object sender, EventArgs e)
        {
            if (ddlType.SelectedValue.Equals("1"))
            {
                ViewPostTest1.Initialize(long.Parse(txtDocNo.Text), DocumentKind.Advance);
                ViewPostTest1.Show();
            }
            else if (ddlType.SelectedValue.Equals("2"))
            {
                ViewPostTest1.Initialize(long.Parse(txtDocNo.Text), DocumentKind.Remittance);
                ViewPostTest1.Show();
            }
            else if (ddlType.SelectedValue.Equals("3"))
            {
                ViewPostTest1.Initialize(long.Parse(txtDocNo.Text), DocumentKind.Expense);
                ViewPostTest1.Show();
            }
            else if (ddlType.SelectedValue.Equals("4"))
            {
                ViewPostTest1.Initialize(long.Parse(txtDocNo.Text), DocumentKind.ExpenseRemittance);
                ViewPostTest1.Show();
            }
        }

        #region protected void btnCreatePosting_Click(object sender, EventArgs e)
        protected void btnCreatePosting_Click(object sender, EventArgs e)
        {
            

            if (ddlType.SelectedValue.Equals("1"))//Advance
            {
                AdvancePostingService advanceService = new AdvancePostingService();
                advanceService.DeletePostingDataByDocId(long.Parse(txtDocNo.Text), DocumentKind.Advance.ToString());
                advanceService.CreatePostData(long.Parse(txtDocNo.Text), DocumentKind.Advance.ToString());
            }
            else if (ddlType.SelectedValue.Equals("2"))//Remittance
            {
                RemittancePostingService remittanceService = new RemittancePostingService();
                remittanceService.DeletePostingDataByDocId(long.Parse(txtDocNo.Text), DocumentKind.Remittance.ToString());
                remittanceService.CreatePostData(long.Parse(txtDocNo.Text), DocumentKind.Remittance.ToString());
            }
            else if (ddlType.SelectedValue.Equals("3"))//Expense
            {
                ExpensePostingService expenseService = new ExpensePostingService();
                expenseService.DeletePostingDataByDocId(long.Parse(txtDocNo.Text), DocumentKind.Expense.ToString());
                expenseService.CreatePostData(long.Parse(txtDocNo.Text), DocumentKind.Expense.ToString());
            }
            else if (ddlType.SelectedValue.Equals("4"))//Expense Remittance
            {
                ExpensePostingService expenseService = new ExpensePostingService();
                expenseService.DeletePostingDataByDocId(long.Parse(txtDocNo.Text), DocumentKind.ExpenseRemittance.ToString());
                expenseService.CreatePostData(long.Parse(txtDocNo.Text), DocumentKind.ExpenseRemittance.ToString());
            }
        }
        #endregion protected void btnCreatePosting_Click(object sender, EventArgs e)

        #region protected void btnSimulate_Click(object sender, EventArgs e)
        protected void btnSimulate_Click(object sender, EventArgs e)
        {
            
        }
        #endregion protected void btnSimulate_Click(object sender, EventArgs e)

        #region protected void btnPosting_Click(object sender, EventArgs e)
        protected void btnPosting_Click(object sender, EventArgs e)
        {
            
        }
        #endregion protected void btnPosting_Click(object sender, EventArgs e)

        protected void btnReverse_Click(object sender, EventArgs e)
        {
            
        }

        protected void btnApprove_Click(object sender, EventArgs e)
        {
            
        }

        
    }
}
