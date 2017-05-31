using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SS.DB.DTO.ValueObject;
using SS.DB.Query;
using SCG.eAccounting.Web.Helper;
using SS.DB.DTO;
using SS.DB.BLL;
using SCG.eAccounting.Web.UserControls;

namespace SCG.eAccounting.Web.Forms.SS.DB.Programs
{
    public partial class CurrencySetup : BasePage
    {

        public IDbCurrencyService DbCurrencyService { get; set; }
        public IDbCurrencyLangService DbCurrencyLangService { get; set; }
        public short CurrencyId
        {
            get { return UIHelper.ParseShort(currency.Value); }
            set { currency.Value = value.ToString(); }
        }
       
        private void RefreshGridData(object sender, EventArgs e)
        {
           
            ctlCurrencyEditor.HidePopUp();
            ctlCurrencySetupGrid.DataCountAndBind();
            ctlCurrencySetupUpdatePanel.Update();

        }
        
        protected override void OnPreLoad(EventArgs e)
        {
            base.OnPreLoad(e);
            //ProgramCode = "CurrencySetup";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
            ctlCurrencyEditor.Notify_Ok += new EventHandler(RefreshGridData);
            ctlCurrencyEditor.Notify_Cancle += new EventHandler(RefreshGridData);

            if (!Page.IsPostBack)
            {
                ctlCurrencySetupGrid.DataCountAndBind();
            }
           
        }
        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
        }

        protected void ctlCurrency_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex;
            if (e.CommandName.Equals("CurrencyEdit"))
            {
                rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                CurrencyId = UIHelper.ParseShort(ctlCurrencySetupGrid.DataKeys[rowIndex].Values["CurrencyID"].ToString());
               
              
                ctlCurrencyEditor.Initialize(FlagEnum.EditFlag, CurrencyId);
                ctlCurrencyEditor.ShowPopUp();
                
            }
          
            if (e.CommandName.Equals("CurrencyDelete"))
            {
                try
                {
                    rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                    CurrencyId = UIHelper.ParseShort(ctlCurrencySetupGrid.DataKeys[rowIndex].Value.ToString());
                    // delete by cascade on delete
                    //IList<DbCurrencyLang> clang = SsDbQueryProvider.DbCurrencyLangQuery.FindCurrencyLangByCID(CurrencyId);
                    //DbCurrencyLangService.DeleteCurrencyLang(clang);
                 
                    DbCurrency currency = SsDbQueryProvider.DbCurrencyQuery.FindCurrencyById(CurrencyId);
                    DbCurrencyService.DeleteCurrency(currency);

                }
                catch (Exception ex)
                {
                    if (((System.Data.SqlClient.SqlException)(ex.GetBaseException())).Number == 547)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertInUseData",
                            "alert('This data is now in use.');", true);
                        ctlCurrencySetupGrid.DataCountAndBind();
                    }
                }

                ctlCurrencySetupGrid.DataCountAndBind();
                ctlCurrencySetupUpdatePanel.Update();

            }


        }

        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            VOUCurrencySetup criteria = GetSuUserCriteria();
            return SsDbQueryProvider.DbCurrencyQuery.GetCurrencyListByCriteria(criteria, startRow, pageSize, sortExpression);
        
        }
        public int RequestCount()
        {
            VOUCurrencySetup criteria = GetSuUserCriteria();
            int count = SsDbQueryProvider.DbCurrencyQuery.CountCurrencyByCriteria(criteria);
            return count;
           
        }
        protected void ctlAdd_Click(object sender, ImageClickEventArgs e)
        {
         
            ctlCurrencyEditor.Initialize(FlagEnum.NewFlag,0);
            ctlCurrencyEditor.ShowPopUp();
         

        }
        protected void ctlCurrencySearch_Click(object sender, ImageClickEventArgs e)
        {
            ctlCurrencySetupGrid.DataCountAndBind();

        }
        public VOUCurrencySetup GetSuUserCriteria()
        {
            VOUCurrencySetup currency = new VOUCurrencySetup();
            currency.Symbol= ctlSymbol.Text;

            currency.Description= ctrDescription.Text;
            return currency;
        }

    }
}
