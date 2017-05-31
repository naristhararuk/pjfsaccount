﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SCG.eAccounting.BLL;
using SS.Standard.UI;
using SCG.eAccounting.Web.Helper;
using SCG.DB.Query;

namespace SCG.eAccounting.Web.UserControls.DocumentEditor.Components
{
    public partial class PerdiemPopup : BasePage, IDocumentEditor
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
        public long MileageId
        {
            get { return (long)ViewState["MileageId"]; }
            set { ViewState["MileageId"] = value; }
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
                this.TxId = new Guid(Request.QueryString["txId"]);
                this.ExpDocumentID = Convert.ToInt64(string.IsNullOrEmpty(Request.QueryString["expId"]) ? "-1" : Request.QueryString["expId"]);
                this.InitialFlag = Request.QueryString["mode"];

                long? documentID = string.IsNullOrEmpty(Request.QueryString["docId"]) ? (long?)null : ((Convert.ToInt64(Request.QueryString["docId"]) < 0) ? (long?)null : Convert.ToInt64(Request.QueryString["docId"]));

                this.VisibleFields = FnExpenseDocumentService.GetVisibleFields(documentID);
                this.EditableFields = FnExpenseDocumentService.GetEditableFields(documentID);

                long? perdiemId = null;
                if (!string.IsNullOrEmpty(Request.QueryString["perdiemId"]))
                    perdiemId = UIHelper.ParseLong(Request.QueryString["perdiemId"]);

                ctlPerdiem.Initialize(this.InitialFlag, perdiemId, this.ExpDocumentID, this.TxId);
            }
        }

        #region IDocumentEditor Members

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

        public bool IsContainEditableFields(object editableFieldEnum)
        {
            return this.EditableFields.Contains(editableFieldEnum);
        }

        public bool IsContainVisibleFields(object visibleFieldEnum)
        {
            return this.VisibleFields.Contains(visibleFieldEnum);
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

        public void CheckUserRepOffice(long requesterId)
        {
            IsRepOffice = ScgDbQueryProvider.DbPBQuery.IsRepOffice(requesterId);
        }

        public bool RequireDocumentAttachment()
        {
            return false;
        }
        #endregion
    }
}
