using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SCG.eAccounting.Web.Helper;
using SCG.eAccounting.BLL;
using SS.Standard.UI;
using SS.Standard.WorkFlow.EventUserControl;
using SS.Standard.WorkFlow.DTO;
using SS.Standard.WorkFlow.Query;
using SS.Standard.WorkFlow.DTO.ValueObject;

namespace SCG.eAccounting.Web.UserControls.WorkFlow
{
    public partial class ReceiveDocument : BaseUserControl, IEventControl
    {
        public int WorkFlowStateEventID { get; set; }
        public IFnExpenseDocumentService FnExpenseDocumentService { get; set; }

        public long DocumentID
        {
            get { return UIHelper.ParseLong(ViewState[ViewStateName.DocumentID].ToString()); }
            set { ViewState[ViewStateName.DocumentID] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        public void Initialize(long workFlowID)
        {
            Document document = WorkFlowQueryProvider.WorkFlowQuery.GetDocumentByWorkFlowID(workFlowID);
            if (document != null)
                ctlBoxID.Text = FnExpenseDocumentService.GenerateDefaultBoxID(document.DocumentID);
        }

        #region IEventControl Members

        public object GetEventData(int workFlowStateEventID)
        {
            ReceiveResponse returnObj = new ReceiveResponse();
            returnObj.WorkFlowStateEventID = workFlowStateEventID;
            returnObj.BoxID = ctlBoxID.Text;

            return returnObj;
        }

        #endregion
    }
}