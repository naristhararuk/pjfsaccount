using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SCG.eAccounting.DTO.ValueObject;
using SCG.eAccounting.Query;
using SCG.eAccounting.BLL;

namespace SCG.eAccounting.Web.Forms.SCG.eAccounting.Programs
{
    public partial class Draft : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ctlInboxEmployeeSearchResult.VisibleColumnSelect = false;
                ctlInboxEmployeeSearchResult.VisibleColumnAttachFile = false;
                ctlInboxEmployeeSearchResult.VisibleColumnReferenceNo = false;
                ctlInboxEmployeeSearchResult.VisibleColumnRequesterDate = false;
                ctlInboxEmployeeSearchResult.VisibleColumnStatus = false;
                ctlInboxEmployeeSearchResult.DataBind();
                this.SearchResult();
            }
        }

        #region public SearchCriteria BuildCriteria()
        public SearchCriteria BuildCriteria()
        {
            SearchCriteria criteria = new SearchCriteria();
            criteria.UserID = UserAccount.UserID;
            criteria.FlagQuery = "Draft";

            criteria.FlagSearch = "Employee";
            criteria.FlagJoin = string.Empty;
            criteria.Role = string.Empty;
            criteria.LanguageID = UserAccount.CurrentLanguageID;

            return criteria;
        }
        #endregion public SearchCriteria BuildCriteria()

        #region public void SearchResult()
        public void SearchResult()
        {
            SearchCriteria criteria = this.BuildCriteria();

            ctlInboxEmployeeSearchResult.BindInboxGridView(criteria);

            ctlInboxEmployeeSearchResult.Legend = string.Concat(GetMessage(string.Concat("WS_", WorkFlowStateFlag.Draft)), " (" + ctlInboxEmployeeSearchResult.RowCount + ")");
            ctlUpdatePanel.Update();
        }
        #endregion public void SearchResult()
    }
}
