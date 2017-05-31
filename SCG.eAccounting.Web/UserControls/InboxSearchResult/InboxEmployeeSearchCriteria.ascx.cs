using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SCG.eAccounting.Query;
using SS.Standard.UI;
using SCG.eAccounting.DTO.ValueObject;
using SCG.eAccounting.Web.Helper;

namespace SCG.eAccounting.Web.UserControls.InboxSearchResult
{
    public partial class InboxEmployeeSearchCriteria : BaseUserControl
    {
        #region Property
        public string RequestNo
		{
			get { return ctlRequestNo.Text; }
            set { ctlRequestNo.Text = value; }
		}
        public string RequestDate
        {
            get { return ctlRequestDateFromCal.DateValue; }
            set { ctlRequestDateFromCal.DateValue = value; }
        }
        public string RequestDateFrom
        {
            get { return ctlRequestDateFromCal.DateValue; }
            set { ctlRequestDateFromCal.DateValue = value; }
        }
        public string RequestDateTo
        {
            get { return ctlRequestDateToCal.DateValue; }
            set { ctlRequestDateToCal.DateValue = value; }
        }
        public string CreatorID
        {
            get { return ctlUserAutoCompleteLookupCreator.EmployeeID; }
            set { ctlUserAutoCompleteLookupCreator.EmployeeID = value; }
        }
        public string RequesterID
        {
            get { return ctlUserAutoCompleteLookupRequester.EmployeeID; }
            set { ctlUserAutoCompleteLookupRequester.EmployeeID = value; }
        }
        public string CreatorNameID
        {
            set { ctlUserAutoCompleteLookupCreator.SetControl(UIHelper.ParseLong(value)); }
        }
        public string RequesterNameID
        {
            set { ctlUserAutoCompleteLookupRequester.SetControl(UIHelper.ParseLong(value)); }
        }
        public string RequesterType
        {
            get { return ctlRequestType.SelectedValue; }
            set { ctlRequestType.SelectedValue = value; }
        }
        #endregion Property

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.SetCombo();
            }
        }

        public void SetCombo()
        {
            IList<TranslatedListItem> documentTypeList = ScgeAccountingQueryProvider.SCGDocumentQuery.FindDocumentTypeCriteria();

            var items = from documentTypes in documentTypeList
                        select new TranslatedListItem() { ID = documentTypes.ID, Symbol = GetMessage(string.Concat("DT_", documentTypes.Symbol)) };

            ctlRequestType.DataSource = items.ToList<TranslatedListItem>();
            ctlRequestType.DataTextField = "Symbol";
            ctlRequestType.DataValueField = "ID";
            ctlRequestType.DataBind();

            ctlRequestType.Items.Insert(0, new ListItem(GetMessage("All_Item"), ""));
        }
    }
}