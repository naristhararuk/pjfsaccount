using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

//using SCG.GL.DTO.ValueObject;
using System.Collections.Generic;
using SCG.eAccounting.Web.Helper;
using SCG.eAccounting.BLL;
using SS.Standard.UI;
using SCG.eAccounting.Query;
using SCG.eAccounting.DTO.DataSet;

namespace SCG.eAccounting.Web.UserControls.DocumentEditor.Components
{
    public partial class ExpensesViewByAccount : BaseUserControl, IEditorComponent
    {
        public IFnExpenseDocumentService FnExpenseDocumentService { get; set; }
        public ITransactionService TransactionService { get; set; }
        private bool refreshHeaderGrid;

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (refreshHeaderGrid)
            {
                ctlExpenseItemGrid.DataBind();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
           
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
        public string DocumentType
        {
            get { return ctlType.Value; }
            set { ctlType.Value = value; }
        }
        public string InitialFlag
        {
            get { return ViewState[ViewStateName.InitialFlag].ToString(); }
            set { ViewState[ViewStateName.InitialFlag] = value; }
        }
        public bool IsEmptyData
        {
            get { return (bool)ViewState["IsEmptyData"]; }
            set { ViewState["IsEmptyData"] = value; }
        }

        public bool IsRepOffice
        {
            get
            {
                if (ViewState["IsRepOffice"] != null)
                    return (bool)ViewState["IsRepOffice"];
                return false;
            }
            set { ViewState["IsRepOffice"] = value; }
        }

        public string SelectedCurrency
        {
            get
            {
                if (ViewState["SelectedCurrency"] != null)
                    return string.IsNullOrEmpty(ViewState["SelectedCurrency"].ToString()) ? CurrencySymbol.THB.ToString() : ViewState["SelectedCurrency"].ToString();
                return CurrencySymbol.THB.ToString();
            }
            set { ViewState["SelectedCurrency"] = value; }
        }

        #region Public Method
        public void BindControl(bool refreshHeader)
        {
            this.refreshHeaderGrid = refreshHeader;
            if (!InitialFlag.Equals(FlagEnum.NewFlag))
            {
                ctlExpenseItemGrid.DataCountAndBind();
                ctlDivTotal.Visible = true;
                ctlDivSummary.Visible = true;
                BindSummary();
            }
            else
            {
                this.refreshHeaderGrid = false;
                ctlDivTotal.Visible = false;
                ctlDivSummary.Visible = false;
            }


            if (RequestCount() > 0)
                IsEmptyData = false;
            else
                IsEmptyData = true;

            ctlUpdatePanelViewByAccount.Update();
        }
        public void Initialize(Guid txID, long documentID, string initFlag)
        {
            this.TransactionId = txID;
            this.ExpDocumentID = documentID;
            this.InitialFlag = initFlag;

            this.BindControl(true);
        }

        #endregion

        protected int RequestCount()
        {
            return ScgeAccountingQueryProvider.FnExpenseInvoiceItemQuery.CountInvoiceItemList(this.ExpDocumentID, UserAccount.CurrentLanguageID);
        }

        protected Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            return ScgeAccountingQueryProvider.FnExpenseInvoiceItemQuery.GetInvoiceItemListByExpenseID(this.ExpDocumentID, UserAccount.CurrentLanguageID, startRow, pageSize, sortExpression);
        }

        public void BindSummary()
        {
            if (ctlExpenseItemGrid.Rows.Count > 0)
            {
                double totalAmount = 0;
                foreach (GridViewRow row in ctlExpenseItemGrid.Rows)
                {
                    if (!IsRepOffice)
                    {
                        Literal ctlLblAmount = ctlExpenseItemGrid.Rows[row.RowIndex].FindControl("ctlLblAmount") as Literal;
                        totalAmount += UIHelper.ParseDouble(ctlLblAmount.Text);
                    }
                    else
                    {
                        Literal ctlLblLocalCurrencyAmount = ctlExpenseItemGrid.Rows[row.RowIndex].FindControl("ctlLblAmountLocalCurrency") as Literal;
                        totalAmount += UIHelper.ParseDouble(ctlLblLocalCurrencyAmount.Text);
                    }
                }
                ctlTotalAmountTHB.Text = UIHelper.BindDecimal(totalAmount.ToString());
            }
            ctlAmount.Text = UIHelper.BindDecimal(FnExpenseDocumentService.GetExpenseTotalBaseAmount(this.TransactionId, ExpDocumentID).ToString());
            ctlNetAmount.Text = UIHelper.BindDecimal(FnExpenseDocumentService.GetExpenseTotalNetAmount(this.TransactionId, ExpDocumentID).ToString());
            ctlVatAmount.Text = UIHelper.BindDecimal(FnExpenseDocumentService.GetExpenseTotalVatAmount(this.TransactionId, ExpDocumentID).ToString());
            ctlWHTAmount.Text = UIHelper.BindDecimal(FnExpenseDocumentService.GetExpenseTotalWHTAmount(this.TransactionId, ExpDocumentID).ToString());
            ctlUpdatePanelViewByAccount.Update();
        }

        protected void ctlExpenseItemGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal ctlLblSeq = e.Row.FindControl("ctlLblSeq") as Literal;
                ctlLblSeq.Text = ((ctlExpenseItemGrid.PageSize * ctlExpenseItemGrid.CustomPageIndex) + (e.Row.RowIndex + 1)).ToString();
            }
        }

        protected void ctlExpenseItemGrid_DataBound(object sender, EventArgs e)
        {
            ctlExpenseItemGrid.Columns[8].HeaderText = string.Format(GetMessage("AmountCurrencyHeaderColumn"), "(" + SelectedCurrency + ")");

            //set show column for domestic/foreign
            if (DocumentType.Equals(ZoneType.Foreign))
            {
                ctlExpenseItemGrid.Columns[5].Visible = true;
                ctlExpenseItemGrid.Columns[6].Visible = true;
                ctlExpenseItemGrid.Columns[7].Visible = true;
                if (IsRepOffice)
                {
                    ctlExpenseItemGrid.Columns[8].Visible = true;
                    ctlExpenseItemGrid.Columns[9].Visible = false;
                }
                else
                {
                    ctlExpenseItemGrid.Columns[8].Visible = false;
                    ctlExpenseItemGrid.Columns[9].Visible = true;
                }
            }
            else
            {
                ctlExpenseItemGrid.Columns[4].Visible = false;
                ctlExpenseItemGrid.Columns[5].Visible = false;
                ctlExpenseItemGrid.Columns[6].Visible = false;
                ctlExpenseItemGrid.Columns[7].Visible = false;
                if (IsRepOffice)
                {
                    ctlExpenseItemGrid.Columns[8].Visible = true;
                    ctlExpenseItemGrid.Columns[9].Visible = false;
                }
                else
                {
                    ctlExpenseItemGrid.Columns[8].Visible = false;
                    ctlExpenseItemGrid.Columns[9].Visible = true;
                }
            }
        }
    }
}