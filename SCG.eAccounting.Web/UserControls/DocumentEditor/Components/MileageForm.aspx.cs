using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SCG.eAccounting.BLL;
using SCG.eAccounting.Web.Helper;

namespace SCG.eAccounting.Web.UserControls.DocumentEditor.Components
{
    public partial class MileageForm : BasePage, IDocumentEditor
    {
        public IFnExpenseDocumentService FnExpenseDocumentService { get; set; }

		public Guid TxId
        {
            get
            {
                if (ViewState[ViewStateName.TransactionID] == null)
                    return Guid.Empty;
                return (Guid)ViewState[ViewStateName.TransactionID];
            }
            set { ViewState[ViewStateName.TransactionID] = value; }
        }
        public long ExpDocumentID
        {
            get
            {
                if (ViewState[ViewStateName.DocumentID] == null)
                    return 0;
                return (long)ViewState[ViewStateName.DocumentID];
            }
            set { ViewState[ViewStateName.DocumentID] = value; }
        }
        public long MileageID
        {
            get { return (long)ViewState["MileageID"]; }
			set { ViewState["MileageID"] = value; }
        }
        public string InitialFlag
        {
            get
            {
                if (ViewState[ViewStateName.InitialFlag] != null)
                    return ViewState[ViewStateName.InitialFlag].ToString();
                return string.Empty;
            }
            set { ViewState[ViewStateName.InitialFlag] = value; }
        }
        public IList<object> VisibleFields
        {
            get { return (IList<object>)ViewState[ViewStateName.VisibleFields]; }
            set { ViewState[ViewStateName.VisibleFields] = value; }
        }
        public IList<object> EditableFields
        {
            get { return (IList<object>)ViewState[ViewStateName.EditableFields]; }
            set { ViewState[ViewStateName.EditableFields] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.TxId = new Guid(Request.QueryString["txID"]);
                this.ExpDocumentID = Convert.ToInt64(string.IsNullOrEmpty(Request.QueryString["expId"]) ? "-1" : Request.QueryString["expId"]);
                this.InitialFlag = Request.QueryString["mode"];

                long? documentID = string.IsNullOrEmpty(Request.QueryString["docId"]) ? (long?)null : ((Convert.ToInt64(Request.QueryString["docId"]) < 0) ? (long?)null : Convert.ToInt64(Request.QueryString["docId"]));
				string documentType = string.IsNullOrEmpty(Request.QueryString["docType"]) ? ZoneType.Domestic : Request.QueryString["docType"];

                this.VisibleFields = FnExpenseDocumentService.GetVisibleFields(documentID);
                this.EditableFields = FnExpenseDocumentService.GetEditableFields(documentID);

                long? mileageId = null;
                if (!string.IsNullOrEmpty(Request.QueryString["mileageId"]))
					mileageId = UIHelper.ParseLong(Request.QueryString["mileageId"]);

				ctlMileage.DocumentType = documentType;
				ctlMileage.DocumentID = documentID;
                ctlMileage.ExpDocumentID = this.ExpDocumentID;
                ctlMileage.Initialize(this.InitialFlag, mileageId, this.TxId);
            }
        }
        #region IDocumentEditor Members
		#region Not use in popup
		public void Initialize(string initFlag, long? documentID)
		{
			throw new NotImplementedException();
		}
		public long Save()
		{
			throw new NotImplementedException();
		}
		public void RollBackTransaction()
		{
			throw new NotImplementedException();
		}
		public void Copy(long wfid)
		{
			throw new NotImplementedException();
		}
        public void EnabledViewPostButton(bool IsLock)
        {
           
        }

        public void EnabledPostRemittanceButton(bool IsLock)
        {
            
        }
		#endregion

        public bool IsContainEditableFields(object editableFieldEnum)
        {
            return this.EditableFields.Contains(editableFieldEnum);
        }
        public bool IsContainVisibleFields(object visibleFieldEnum)
        {
            return this.VisibleFields.Contains(visibleFieldEnum);
        }

        private DsNullHandler dsNullHandler;
        event DsNullHandler IDocumentEditor.DsNull
        {
            add
            {
                dsNullHandler += value;
            }
            remove
            {
                dsNullHandler -= value;
            }
        }

        public bool RequireDocumentAttachment()
        {
            return false;
        }
        #endregion
    }
}
