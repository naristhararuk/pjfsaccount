using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SCG.eAccounting.Web.Helper;
using SCG.eAccounting.DTO.ValueObject;

namespace SCG.eAccounting.Web.UserControls.InboxSearchResult
{
    public partial class SearchResultCriteria : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Search_OnClick(object sender, EventArgs e)
        {
            SearchResult1.Criteria = GetCriteria();
            SearchResult1.BindGridView();
        }
        public SearchCriteria GetCriteria()
        { 
            SearchCriteria criteria = new SearchCriteria();

            criteria.BudgetYear = ctlInputYear.Text;
            criteria.DocTypeFrom = ctlInputDocTypeFrom.Text;
            criteria.DocTypeTo = ctlInputDocTypeTo.Text;
            criteria.CostCenterFrom = ctlInputCostCenterFrom.Text;
            criteria.CostCenterTo = ctlInputCostCenterTo.Text;
            criteria.CreDateFrom = ctlCalCreateDocFrom.DateValue;
            criteria.CreDateTo = ctlCalCreateDocTo.DateValue;
            criteria.DocNoFrom = ctlInputDocNoFrom.Text;
            criteria.DocNoTo = ctlInputDocNoTo.Text;
            if(!string.IsNullOrEmpty(ctlInputAmountFrom.Text))
            {
                criteria.AmountFrom = UIHelper.ParseDouble(ctlInputAmountFrom.Text);
            }
            if (!string.IsNullOrEmpty(ctlInputAmountTo.Text))
            {
                criteria.AmountFrom = UIHelper.ParseDouble(ctlInputAmountTo.Text);
            }
            criteria.DocStatus = ctlSelectedDocStatus.SelectedValue;

            return criteria; 
        }

        protected void ctlSearchCostCenter_Click(object sender, ImageClickEventArgs e)
        {
            CostCenterLookupPrototype1.Show();
        }

        protected void ctlSearchDocType_Click(object sender, ImageClickEventArgs e)
        {

        }

        
    }
}