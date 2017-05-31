using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SCG.eAccounting.Web.Helper;
using SCG.eAccounting.DTO.ValueObject;
using SCG.eAccounting.Query;
using SCG.eAccounting.Query.Hibernate;

namespace SCG.eAccounting.Web.Forms.SCG.eAccounting.Programs
{
    public partial class TASearch : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //this.SetRequestType();
                ctlFieldSearch.Visible = false;
                ctlUpdatePanelSearchResult.Update();
            }
        }

        protected void ctlSearch_Click(object sender, EventArgs e)
        {
            ctlFieldSearch.Visible = true;
            ctlTASearchResultGrid.DataCountAndBind();
            ctlUpdatePanelSearchResult.Update();
        }

        protected int RequestCount()
        {
            return ScgeAccountingQueryProvider.SCGDocumentQuery.CountTADocumentCriteria(CreateCriteria());
        }

        protected object RequestData(int startRow, int pageSize, string sortExpression)
        {
            return ScgeAccountingQueryProvider.SCGDocumentQuery.GetTADocumentCriteriaList(CreateCriteria(), startRow, pageSize, sortExpression);
        }

        private TASearchCriteria CreateCriteria()
        {
            TASearchCriteria criteria = new TASearchCriteria();
            criteria.CompanyID = UIHelper.ParseLong(ctlCompanyTextboxAutoComplete.CompanyID);
            //criteria.DocumentTypeID = UIHelper.ParseLong(ctlRequestType.SelectedValue);
            criteria.RequestNo = ctlRequestNo.Text;
            criteria.Subject = TextBox10.Text;
            criteria.RequestDateFrom = UIHelper.ParseDate(ctlRequestDateFrom.DateValue);
            criteria.RequestDateTo = UIHelper.ParseDate(ctlRequestDateTo.DateValue);
            criteria.ApproveDateFrom = UIHelper.ParseDate(ctlApproveDateFrom.DateValue);
            criteria.ApproveDateTo = UIHelper.ParseDate(ctlApproveDateTo.DateValue);
            criteria.CreatorID = UIHelper.ParseLong(ctlUserAutoCompleteLookupCreator.EmployeeID);
            criteria.TravellerNameTH = ctlTravellerNameTH.Text;
            criteria.TravellerNameEN = ctlTravellerNameEN.Text;
            criteria.Country = TextBox11.Text;
            criteria.Province = TextBox12.Text;

            return criteria;
        }

        protected void ctlTASearchResultGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("PopupDocument"))
            {
                int rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                long workflowID = UIHelper.ParseLong(ctlTASearchResultGrid.DataKeys[rowIndex]["WorkflowID"].ToString());
                Response.Redirect("../Programs/DocumentView.aspx?wfid=" + workflowID.ToString() + "&fromTASearch=1");
            }
        }

        //private void SetRequestType()
        //{
        //    IList<TranslatedListItem> documentTypeList = ScgeAccountingQueryProvider.SCGDocumentQuery.FindDocumentTypeCriteria();

        //    var items = from documentTypes in documentTypeList
        //                select new TranslatedListItem() { ID = documentTypes.ID, Symbol = GetMessage(string.Concat("DT_", documentTypes.Symbol)) };

        //    ctlRequestType.DataSource = items.ToList<TranslatedListItem>();
        //    ctlRequestType.DataTextField = "Symbol";
        //    ctlRequestType.DataValueField = "ID";
        //    ctlRequestType.DataBind();

        //    ctlRequestType.Items.Insert(0, new ListItem(GetMessage("All_Item"), string.Empty));
        //}
    }
}