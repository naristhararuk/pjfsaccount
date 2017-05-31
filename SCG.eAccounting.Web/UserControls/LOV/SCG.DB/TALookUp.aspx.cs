using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SCG.eAccounting.Query.Hibernate;
using SCG.eAccounting.Query;
using SCG.eAccounting.Web.Helper;
using SCG.eAccounting.DTO.ValueObject;
using System.Text;
using SS.Standard;
using SS.Standard.WorkFlow.Query;
using workflow = SS.Standard.WorkFlow.DTO;
using SCG.eAccounting.BLL;
using SCG.eAccounting.DTO;


namespace SCG.eAccounting.Web.UserControls.LOV.SCG.DB
{
    public partial class TALookUp : BasePage
    {
        #region Property
        #region public bool isMultiple
        public bool isMultiple
        {
            get
            {
                if (ViewState["isMultiple"] == null)
                    return false;
                else
                    return bool.Parse(ViewState["isMultiple"].ToString());
            }
            set
            {
                ViewState["isMultiple"] = value;
            }
        }
        #endregion public bool isMultiple
        public string CompanyID
        {
            get
            {
                //if (string.IsNullOrEmpty(CompanyID))
                //    return string.Empty;
                //else
                    return ctlCompanyID.Value;
            }
            set
            {
                ctlCompanyID.Value = value;
            }
        }
        public string RequesterID
        {
            get
            {
                //if (string.IsNullOrEmpty(RequesterID))
                //    return string.Empty;
                //else
                    return ctlRequesterID.Value;
            }
            set
            {
                ctlRequesterID.Value = value;
            }
        }
        public string TravelBy
        {
            get
            {
                //if (string.IsNullOrEmpty(TravelBy))
                //    return string.Empty;
                //else
                    return ctlTravelBy.Value;
            }
            set
            {
                ctlTravelBy.Value = value;
            }
        }
        public string Text
        {
            get
            {
                if (!string.IsNullOrEmpty(ctlDocumentNo.Text) || !string.IsNullOrEmpty(ctlDescription.Text))
                {
                    return ctlDocumentNo.Text + '-' + ctlDescription.Text;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public bool Display
        {
            set
            {
                if (value)
                    ctlDescription.Style.Add("display", "inline-block");
                else
                    ctlDescription.Style.Add("display", "none");
            }
        }
        public bool isQueryForAdvance 
        {        
            get
            {
                if (ViewState["isQueryForAdvance"] != null)
                    return (bool)(ViewState["isQueryForAdvance"]);
                else
                    return false;
            }
            set { ViewState["isQueryForAdvance"] = value; }
        }
        public bool isQueryForRemittance
        {
            get
            {
                if (ViewState["isQueryForRemittance"] != null)
                    return (bool)(ViewState["isQueryForRemittance"]);
                else
                    return false;
            }
            set { ViewState["isQueryForRemittance"] = value; }
        }
        public bool isQueryForExpense
        {
            get
            {
                if (ViewState["isQueryForExpense"] != null)
                    return (bool)(ViewState["isQueryForExpense"]);
                else
                    return false;
            }
            set { ViewState["isQueryForExpense"] = value; }
        }
        #endregion Property

        #region protected void Page_Load(object sender, EventArgs e)
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                isMultiple = bool.Parse(Request["isMultiple"].ToString());
                if (Request["CompanyID"].ToString().Length > 0)
                    CompanyID = Request["CompanyID"].ToString();                
                if (Request["RequesterID"].ToString().Length > 0)
                    RequesterID = Request["RequesterID"].ToString();
                TravelBy = Request["TravelBy"].ToString();
                isQueryForAdvance = bool.Parse(Request["isQueryForAdvance"].ToString());
                isQueryForRemittance = bool.Parse(Request["isQueryForRemittance"].ToString());
                isQueryForExpense = bool.Parse(Request["isQueryForExpense"].ToString());

                this.Show();
            }
        }
        #endregion protected void Page_Load(object sender, EventArgs e)

        private void SetEnumForQueryTA(out System.Enum queryFor, out System.Enum withoutDocument)
        {
            queryFor = SCGDocumentQuery.GetDocumentListFor.General;
            withoutDocument = SCGDocumentQuery.SearchTAby.General;
            if (isQueryForAdvance)
            {
                queryFor = SCGDocumentQuery.GetDocumentListFor.Advance;
                if (ctlRadioWithoutAdvance.Checked) withoutDocument = SCGDocumentQuery.SearchTAby.WithoutAdvance;
            }
            else if (isQueryForRemittance)
            {
                queryFor = SCGDocumentQuery.GetDocumentListFor.Remittance;
                if (ctlRadioWithoutRemittance.Checked) withoutDocument = SCGDocumentQuery.SearchTAby.WithoutRemittance;
            }
            else if (isQueryForExpense)
            {
                queryFor = SCGDocumentQuery.GetDocumentListFor.Expense;
                if (ctlRadioWithoutExpense.Checked) withoutDocument = SCGDocumentQuery.SearchTAby.WithoutExpense;
            }
        }

        #region public Object RequestData(int startRow, int pageSize, string sortExpression)
        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            System.Enum queryFor;
            System.Enum withoutDocument;
            SetEnumForQueryTA(out queryFor, out withoutDocument);

            //return ScgeAccountingQueryProvider.SCGDocumentQuery.GetDocumentList(startRow, pageSize, srtExpression, ctlDocumentNo.Text, ((ctlAdvanceReference.SelectedIndex == 0) ? "" : ctlAdvanceReference.SelectedValue), ctlDescription.Text, UIHelper.ParseLong(ctlCompanyID.Value), UIHelper.ParseLong(ctlRequesterID.Value), ctlTravelBy.Value, queryFor, withoutDocument);
            return ScgeAccountingQueryProvider.SCGDocumentQuery.GetDocumentList(startRow, pageSize, sortExpression, ctlDocumentNo.Text, ctlDescription.Text, UIHelper.ParseLong(ctlCompanyID.Value), UIHelper.ParseLong(ctlRequesterID.Value), ctlTravelBy.Value, queryFor, withoutDocument,null );
        }
        #endregion public Object RequestData(int startRow, int pageSize, string sortExpression)

        #region public int RequestCount()
        public int RequestCount()
        {
            System.Enum queryFor;
            System.Enum withoutDocument;
            SetEnumForQueryTA(out queryFor, out withoutDocument);
            //int count = ScgeAccountingQueryProvider.SCGDocumentQuery.CountByDocumentCriteria(ctlDocumentNo.Text, ((ctlAdvanceReference.SelectedIndex == 0) ? "" : ctlAdvanceReference.SelectedValue), ctlDescription.Text, UIHelper.ParseLong(ctlCompanyID.Value), UIHelper.ParseLong(ctlRequesterID.Value), ctlTravelBy.Value, queryFor, withoutDocument);
            int count = ScgeAccountingQueryProvider.SCGDocumentQuery.CountByDocumentCriteria(ctlDocumentNo.Text, ctlDescription.Text, UIHelper.ParseLong(ctlCompanyID.Value), UIHelper.ParseLong(ctlRequesterID.Value), ctlTravelBy.Value, queryFor, withoutDocument,null);
            return count;
        }
        #endregion public int RequestCount()

        #region protected void ctlSearch_Click(object sender, ImageClickEventArgs e)
        protected void ctlSearch_Click(object sender, ImageClickEventArgs e)
        {
            ctlTADocumentGridView.DataCountAndBind();
            UpdatePanelGridView.Update();
        }
        #endregion protected void ctlSearch_Click(object sender, ImageClickEventArgs e)

        #region  protected void ctlTADocumentGrid_DataBound(object sender, EventArgs e)
        protected void ctlTADocumentGrid_DataBound(object sender, EventArgs e)
        {
            if (isMultiple && ctlTADocumentGridView.Rows.Count > 0)
                RegisterScriptForGridView();
        }
        #endregion  protected void ctlTADocumentGrid_DataBound(object sender, EventArgs e)

        #region protected void ctlCancel_Click(object sender, ImageClickEventArgs e)
        protected void ctlCancel_Click(object sender, ImageClickEventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), this.ClientID + "Cancel", "notifyPopupResult('cancel')", true);
            Hide();
        }
        #endregion protected void ctlCancel_Click(object sender, ImageClickEventArgs e)

        #region private void CallOnObjectLookUpReturn(string id)
        private void CallOnObjectLookUpReturn(string id)
        {
            Hide();
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), this.ClientID + "notifyPopupResult()", "notifyPopupResult('ok' , '" + id + "')", true);
        }
        #endregion private void CallOnObjectLookUpReturn(string id)

        #region protected void ctlTADocumentGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        protected void ctlTADocumentGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            bool isValid = true;
            if (!isMultiple)
            {
                if (e.CommandName.Equals("Select"))
                {
                    int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                    long documentId = UIHelper.ParseLong(ctlTADocumentGridView.DataKeys[rowIndex].Value.ToString());
                    IList<TADocumentObj> taDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.FindByDocumentIdentity(Convert.ToInt64(documentId));
                    long taDocID = Convert.ToInt64(taDocument[0].TADocumentID);
                    Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
                    IList<Advance> advanceList = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.FindAdvanceDocumentForRequesterByTADocumentID(UIHelper.ParseLong(ctlRequesterID.Value), taDocID);
                    if (advanceList.Count > 0)
                    {
                        foreach (Advance ad in advanceList)
                        {
                            workflow.WorkFlow wf = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(ad.DocumentID);
                            if (wf != null && wf.CurrentState.Name == "Outstanding")
                            {
                                IList<FnExpenseDocument> expenseList = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.FindExpenseReferenceTAForRequesterByTADocumentID(UIHelper.ParseLong(ctlRequesterID.Value), taDocID);
                                IList<FnRemittance> remittanceList = ScgeAccountingQueryProvider.FnRemittanceQuery.FindRemittanceReferenceTAForRequesterByTADocumentID(UIHelper.ParseLong(ctlRequesterID.Value), taDocID);

                                //check expense
                                if (expenseList.Count > 0)
                                {
                                    foreach (FnExpenseDocument exp in expenseList)
                                    {
                                        workflow.WorkFlow expenseWF = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(exp.Document.DocumentID);

                                        if (!(expenseWF.CurrentState.Name == "Cancel" || expenseWF.CurrentState.Name == "Complete"))
                                        {
                                            isValid = false;
                                            break;
                                        }
                                    }
                                }

                                //check remittance
                                if (isValid && remittanceList.Count > 0)
                                {
                                    foreach (FnRemittance rmt in remittanceList)
                                    {
                                        workflow.WorkFlow remittanceWF = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(rmt.Document.DocumentID);
                                        if (!(remittanceWF.CurrentState.Name == "Cancel" || remittanceWF.CurrentState.Name == "Complete"))
                                        {
                                            isValid = false;
                                            break;
                                        }
                                    }
                                }

                            }

                            if (!isValid) break;
                        }
                    }
                    

                    if (!isValid)
                    {
                        errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("CannotReferenceTADocument"));
                        this.ValidationErrors.MergeErrors(errors);
                        ctlUpdatePanelValidate.Update();
                    }
                    else
                    {
                    CallOnObjectLookUpReturn(documentId.ToString());
                    Hide();
                }
            }
        }

        }
        #endregion protected void ctlTADocumentGrid_RowCommand(object sender, GridViewCommandEventArgs e)

        #region protected void ctlSelect_Click(object sender, ImageClickEventArgs e)
        protected void ctlSelect_Click(object sender, ImageClickEventArgs e)
        {
            if (isMultiple)
            {
                //IList<TADocumentObj> taDocumentList = new List<TADocumentObj>();
                string taDocumentList = string.Empty;

                foreach (GridViewRow row in ctlTADocumentGridView.Rows)
                {
                    if ((row.RowType == DataControlRowType.DataRow) && ((CheckBox)row.FindControl("ctlSelect")).Checked)
                    {
                        long documentId = UIHelper.ParseLong(ctlTADocumentGridView.DataKeys[row.RowIndex].Value.ToString());
                        taDocumentList += documentId.ToString() + "|";
                        taDocumentList.Remove(taDocumentList.Length - 1, 1);
                    }
                }

            }
        }
        #endregion protected void ctlSelect_Click(object sender, ImageClickEventArgs e)

        #region private void RegisterScriptForGridView()
        private void RegisterScriptForGridView()
        {
            StringBuilder script = new StringBuilder();
            script.Append("function validateCheckBox(objChk, objFlag) ");
            script.Append("{ ");
            script.Append("CheckboxesCheckUncheck(objChk, objFlag, ");
            script.Append("'" + ctlTADocumentGridView.ClientID + "', '" + ctlTADocumentGridView.HeaderRow.FindControl("ctlSelectHeader").ClientID + "'); ");
            script.Append("} ");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "validateChkBox", script.ToString(), true);
        }
        #endregion private void RegisterScriptForGridView()

        #region public void Show()
        public void Show()
        {
            if (isMultiple)
            {
                ctlTADocumentGridView.Columns[0].Visible = true;
                ctlTADocumentGridView.Columns[1].Visible = false;
                ctlSelect.Visible = true;
                ctlLine.Visible = true;
            }
            else
            {
                ctlTADocumentGridView.Columns[0].Visible = false;
                ctlTADocumentGridView.Columns[1].Visible = true;
                ctlSelect.Visible = false;
                ctlLine.Visible = false;
            }

            divWithoutAdvance.Visible = isQueryForAdvance;
            divWithoutRemittance.Visible = isQueryForRemittance;
            divWithoutExpense.Visible = isQueryForExpense;
            ctlRadioAllTA.Checked = true;

            ctlTADocumentGridView.DataCountAndBind();
            this.UpdatePanelSearchAccount.Update();
            this.UpdatePanelGridView.Update();
        }
        #endregion public void Show()

        #region public void Hide()
        public void Hide()
        {
            ctlDocumentNo.Text = string.Empty;
            ctlDescription.Text = string.Empty;
        }
        #endregion public void Hide()
    }
}
