using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SCG.eAccounting.Web.Helper;
using SCG.DB.DTO;
using SCG.DB.Query;
using SCG.eAccounting.Web.UserControls;
using SS.DB.BLL.Implement;
using SCG.DB.BLL;
using SCG.eAccounting.DTO;
using SS.DB.DTO;
using SS.DB.Query;
using SCG.eAccounting.BLL.Implement;
using SCG.eAccounting.BLL;

namespace SCG.eAccounting.Web.Forms.SCG.DB.Programs
{
    public partial class ManageIO : BasePage
    {
        public IDbIOService DbIOService { get; set; }
        //public IFnExpenseInvoiceItemService FnExpenseInvoiceItemService { get; set; }

        public long InternalOrderId
        {
            get { return UIHelper.ParseLong(internalOrderId.Value); }
            set { internalOrderId.Value = value.ToString(); }
        }
       
        private void RefreshGridData(object sender, EventArgs e)
        {
            
            ctlIOEditor.HidePopUp();
            ctlIOGrid.DataCountAndBind();
            ctlIOUpdatePanel.Update();

        }

        protected override void OnPreLoad(EventArgs e)
        {
            base.OnPreLoad(e);
            //ProgramCode = "ManageIO";
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            ctlIOEditor.Notify_Ok += new EventHandler(RefreshGridData);
            ctlIOEditor.Notify_Cancle += new EventHandler(RefreshGridData);

            if (!Page.IsPostBack)
            {

            }

        }
        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
        }

        protected void ctlCurrency_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex;
            if (e.CommandName.Equals("IOEdit"))
            {
                rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                InternalOrderId = UIHelper.ParseLong(ctlIOGrid.DataKeys[rowIndex].Values["IOID"].ToString());


                ctlIOEditor.Initialize(FlagEnum.EditFlag, InternalOrderId);
                ctlIOEditor.ShowPopUp();

            }
            if (e.CommandName.Equals("IODelete"))
            {
                try
                {
                    rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                    InternalOrderId = UIHelper.ParseLong(ctlIOGrid.DataKeys[rowIndex].Value.ToString());
                    DbInternalOrder io = ScgDbQueryProvider.DbIOQuery.FindByIdentity(InternalOrderId);
                    DbIOService.DeleteIO(io);
                   
                }
                catch (Exception ex)
                {
                    if (((System.Data.SqlClient.SqlException)(ex.GetBaseException())).Number == 547)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertInUseData",
                            "alert('This data is now in use.');", true);
                        ctlIOGrid.DataCountAndBind();
                    }
                }

                ctlIOGrid.DataCountAndBind();
                ctlIOUpdatePanel.Update();

            }


        }

        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            DbInternalOrder criteria = GetSuUserCriteria();
            return ScgDbQueryProvider.DbIOQuery.GetInternalOrderList(criteria, startRow, pageSize, sortExpression);
         

        }
        public int RequestCount()
        {
            DbInternalOrder criteria = GetSuUserCriteria();
            return ScgDbQueryProvider.DbIOQuery.CountDataByIOCriteria(criteria);
         

        }
        protected void ctlAdd_Click(object sender, ImageClickEventArgs e)
        {

            ctlIOEditor.Initialize(FlagEnum.NewFlag, 0);
            ctlIOEditor.ShowPopUp();


        }
        protected void ctlIOSearch_Click(object sender, ImageClickEventArgs e)
        {
            ctlIOGrid.DataCountAndBind();

            // ==========================
            // Case 1
            //DbInternalOrder criteria = new DbInternalOrder();
            //criteria.IONumber = "002001000555";
            //criteria.IOType = "F201";
            //criteria.IOText = "Vernier Calipers(digital display 12)";
            //criteria.CostCenterID = new DbCostCenter(13).CostCenterID;
            //criteria.CostCenterCode = "0020";
            //criteria.CompanyID = new DbCompany(8).CompanyID;
            //criteria.CompanyCode = "0130";
            //criteria.EffectiveDate = DateTime.Now;
            //criteria.ExpireDate = DateTime.Now;
            //criteria.Active = true;
            //criteria.CreBy = 1;
            //criteria.CreDate = DateTime.Now;
            //criteria.UpdBy = 1;
            //criteria.UpdDate = DateTime.Now;
            //criteria.UpdPgm = "KLA";
            //DbIOService.SaveOrUpdate(criteria);

            // ==========================
            // Case 2
            //FnExpenseInvoiceItem invoiceItem = new FnExpenseInvoiceItem();
            //invoiceItem.Invoice = new FnExpenseInvoice(10);
            //invoiceItem.CostCenter = new DbCostCenter(10);
            //invoiceItem.Account = new DbAccount(10);
            //invoiceItem.IO = new DbInternalOrder(10);
            //invoiceItem.SaleItem = "";
            //invoiceItem.SaleOrder = "";
            //invoiceItem.Description = "";
            //invoiceItem.ReferenceNo = "";
            //DbCurrency currency = SsDbQueryProvider.DbCurrencyQuery.FindByCurrencySymbol("THB", true, false);
            //invoiceItem.CurrencyID = (long)(currency == null ? ParameterServices.USDCurrencyID : currency.CurrencyID);
            //invoiceItem.CurrencyAmount = 2000;
            //invoiceItem.NonDeductAmount = (double)0;
            //invoiceItem.ExchangeRate = (double)1;
            //invoiceItem.Amount = 2000;
            //invoiceItem.UpdPgm = "KLA";
            //invoiceItem.UpdBy = 1;
            //invoiceItem.UpdDate = DateTime.Now;
            //invoiceItem.CreBy = 1;
            //invoiceItem.CreDate = DateTime.Now;
            //try
            //{
            //    FnExpenseInvoiceItemService.SaveOrUpdate(invoiceItem);
            //}
            //catch (Exception ex)
            //{
            //    int i = 0;
            //}

            // ==========================
            // Case 3
            //DbInternalOrder dbInter = ScgDbQueryProvider.DbIOQuery.FindByIdentity(10);
            //FnExpenseInvoiceItem invoiceItem = new FnExpenseInvoiceItem(10);
            //invoiceItem.IO = new DbInternalOrder(10);
            //try
            //{
            //    FnExpenseInvoiceItemService.SaveOrUpdate(invoiceItem);
            //}
            //catch (Exception ex)
            //{
            //    int i = 0;
            //}
        }
        public DbInternalOrder GetSuUserCriteria()
        {
            DbInternalOrder io = new DbInternalOrder();
            io.IONumber = ctlIONumber.Text;

            io.IOText = ctlIOText.Text;
            return io;
        }
    }
}
