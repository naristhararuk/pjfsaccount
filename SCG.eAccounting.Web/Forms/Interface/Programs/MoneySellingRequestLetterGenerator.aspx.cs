using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SS.Standard.UI;
using System.Text;
using SCG.DB.DTO.ValueObject;
using SCG.DB.Query;
using SCG.eAccounting.Report;
using SCG.eAccounting.Web.Helper;
using SS.DB.Query;
using Spring.Validation;
using SCG.DB.BLL;
using SS.Standard.Utilities;

namespace SCG.eAccounting.Web.Forms.Interface.Programs
{
    public partial class MoneySellingRequestLetterGenerator : BasePage
    {
        public IDbSellingRunningService DbSellingRunningService { get; set; }
        public IDbSellingLetterDetailService DbSellingLetterDetailService { get; set; }
        public IDbSellingLetterService DbSellingLetterService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                if (Request.Cookies["SellingRequestCookie_" + UserAccount.UserID] != null)
                {
                    if (!string.IsNullOrEmpty(Request.Cookies["SellingRequestCookie_" + UserAccount.UserID]["RequestDate"]))
                    {
                        ctlDate.DateValue = Server.HtmlEncode(Request.Cookies["SellingRequestCookie_" + UserAccount.UserID]["RequestDate"]);
                    }
                    txtComListRm.Text = Server.HtmlEncode(Request.Cookies["SellingRequestCookie_" + UserAccount.UserID]["CompanyCode"]);
                    if (Server.HtmlEncode(Request.Cookies["SellingRequestCookie_" + UserAccount.UserID]["IsIncludeGeneratedLetter"]) == "true")
                    {
                        ctlChkGeneratedLetterRm.Checked = true;
                    }
                    else
                    {
                        ctlChkGeneratedLetterRm.Checked = false;
                    }
                    ctlLetterIdRm.Text = Server.HtmlEncode(Request.Cookies["SellingRequestCookie_" + UserAccount.UserID]["LetterNo"]);
                }
                if (!string.IsNullOrEmpty(Request["LetterNo"]))
                {
                    this.GenerateReport(Request["LetterNo"]);
                }
            }
        }

        #region Button Click Event
        protected void btnAddCompany_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ctlCompanyTextboxAutoCompleteRm.CompanyCode))
            {
                if (string.IsNullOrEmpty(txtComListRm.Text))
                {
                    txtComListRm.Text += ctlCompanyTextboxAutoCompleteRm.CompanyCode;
                }
                else
                {
                    txtComListRm.Text += "," + ctlCompanyTextboxAutoCompleteRm.CompanyCode;
                }
            }
            ctlCompanyTextboxAutoCompleteRm.ResetValue();
            ctlUpdatePanelRequestCriteriaRm.Update();
        }

        protected void ctlSearchBtn_Click(object sender, EventArgs e)
        {
            Request.Cookies.Remove("SellingRequestCookie_" + UserAccount.UserID);
            if (Request.Cookies["SellingRequestCookie_" + UserAccount.UserID] == null)
            {
                HttpCookie moneyRequestCookie = new HttpCookie("SellingRequestCookie_" + UserAccount.UserID);
                moneyRequestCookie.Values["CompanyCode"] = txtComListRm.Text;
                moneyRequestCookie.Values["LetterID"] = ctlLetterIdRm.Text;
                moneyRequestCookie.Values["RequestDate"] = ctlDate.Text;
                moneyRequestCookie.Values["IsIncludeGeneratedLetter"] = ctlChkGeneratedLetterRm.Checked.ToString();
                moneyRequestCookie.Expires = DateTime.Now.AddDays(62);
                Response.Cookies.Add(moneyRequestCookie);
            }
            if (string.IsNullOrEmpty(txtComListRm.Text))
            {
                ValidationErrors.AddError("Export.Error", new ErrorMessage("Company is required."));
            }
            else
            {
                ctlGridRm.DataCountAndBind();
                ShowGenerateButton();
                ctlUpdatePanelRequestCriteriaRm.Update();
            }
        }

        private List<SellingRequestLetterParameter> selectToGenerateList()
        {
            List<SellingRequestLetterParameter> toGenerateList = new List<SellingRequestLetterParameter>();
            if (ctlDate.Value.HasValue)
            {
                for (int i = 0; i < ctlGridRm.Rows.Count; i++)
                {
                    CheckBox chkSelect = (CheckBox)ctlGridRm.Rows[i].Cells[0].FindControl("ctlSelectRm");
                    if (chkSelect != null)
                    {
                        if (chkSelect.Checked)
                        {
                            SellingRequestLetterParameter moneyRequest = new SellingRequestLetterParameter();
                            Label DocumentNo = (Label)ctlGridRm.Rows[i].Cells[1].FindControl("ctlRequestNoRm");
                            moneyRequest.DocumentNo = DocumentNo.Text;
                            moneyRequest.CompanyCode = DocumentNo.Text.Substring(4, 4);
                            DateTime tmpYear = DateTime.Today;
                            moneyRequest.Year = tmpYear.Year;
                            Label DocumentID = (Label)ctlGridRm.Rows[i].Cells[5].FindControl("ctlDocumentIDRm");
                            moneyRequest.DocumentID = Convert.ToInt64(DocumentID.Text);
                            moneyRequest.RequestDate = (DateTime)ctlDate.Value;
                            toGenerateList.Add(moneyRequest);
                        }
                    }
                }
            }
            else
            {
                ValidationErrors.AddError("Generate.Error", new ErrorMessage("Date of Remitance is required."));
            }

            return toGenerateList;
        }

        protected void ctlGenerateBtn_Click(object sender, EventArgs e)
        {
            List<SellingRequestLetterParameter> allList = selectToGenerateList();
            allList.Sort((x, y) => string.Compare(x.DocumentNo, y.DocumentNo));
            //List<SellingRequestLetterParameter> toGenerateList = new List<SellingRequestLetterParameter>();
            string letterNoList = string.Empty;

            if (allList.Count() > 0)
            {
                try
                {
                    lock (letterNoList)
                    {
                        letterNoList = DbSellingLetterService.GenerateSellingLetter(allList);
                        ctlGridRm.DataCountAndBind();
                        ctlUpdatePanelRequestCriteriaRm.Update();
                        //letterNoList = letterNoList.Substring(0, letterNoList.Length - 1);
                        string url = "MoneySellingRequestLetterGenerator.aspx?LetterNo=" + letterNoList;
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "Testscript", "document.location = '" + url + "'", true);
                    }
                }
                catch (ServiceValidationException ex)
                {
                    ValidationErrors.MergeErrors(ex.ValidationErrors);
                    ctlGridRm.DataCountAndBind();
                    ctlUpdatePanelRequestCriteriaRm.Update();
                }
                catch (Exception ex)
                {
                    ValidationErrors.AddError("Generate.Error", new ErrorMessage(ex.Message));
                }

                //for (int i = 0; i < allList.Count; i++)
                //{

                //    if (allList[i].CompanyCode != prevCompanyCode)
                //    {
                //        prevCompanyCode = allList[i].CompanyCode;
                //        letterNo = DbSellingRunningService.RetrieveNextSellingRunningNo(allList[i].CompanyCode, allList[i].Year);
                //        letterID = DbSellingLetterDetailService.AddLetterDetail(allList[i], letterNo);

                //        letterNoList += letterNo + ",";
                //    }
                //    DbSellingLetterService.AddLetterAndDocument(allList[i].DocumentID, letterID);
                //}
                //ctlGridRm.DataCountAndBind();
                //ctlUpdatePanelRequestCriteriaRm.Update();
                //letterNoList = letterNoList.Substring(0, letterNoList.Length - 1);
                //string url = "MoneySellingRequestLetterGenerator.aspx?LetterNo=" + letterNoList;
                //ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "Testscript", "document.location = '" + url + "'", true);

            }
            else
            {
                ValidationErrors.AddError("Generate.Error", new ErrorMessage("Please select document."));
            }
        }

        #endregion Button Click Event

        #region ctlReqGrid Event

        protected void ctlReqGrid_DataBound(object sender, EventArgs e)
        {
            if (ctlGridRm.Rows.Count > 0)
            {
                RegisterScriptForGridView();
            }

        }

        protected void ctlReqGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            LinkButton lb = (LinkButton)e.Row.FindControl("ctlLetterIDRm");

            if (lb != null)
            {
                if (!lb.Text.Equals(string.Empty))
                {

                    CheckBox chk = (CheckBox)e.Row.FindControl("ctlSelectRm");
                    chk.Enabled = false;
                }
            }

        }

        protected void ctlReqGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                int rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                string letterNo = ctlGridRm.DataKeys[rowIndex].Value.ToString();
                //this.GenerateReport("0020-11-00001");                                                 // This is error case
                string url = "MoneySellingRequestLetterGenerator.aspx?LetterNo=" + letterNo;
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "Testscript", "document.location = '" + url + "'", true);
            }
        }

        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            SellingRequestLetterParameter searchResultCriteria = GetSearchResultCriteria();

            return ScgDbQueryProvider.DbMoneyRequestQuery.GetMoneySellingRequestList(searchResultCriteria, startRow, pageSize, sortExpression);
        }

        public int RequestCount()
        {
            SellingRequestLetterParameter searchResultCriteria = GetSearchResultCriteria();

            int count = ScgDbQueryProvider.DbMoneyRequestQuery.CountMoneySellingRequestByCriteria(searchResultCriteria);

            return count;
        }

        public SellingRequestLetterParameter GetSearchResultCriteria()
        {
            SellingRequestLetterParameter searchResultCriteria = new SellingRequestLetterParameter();
            string companyList = GetCompanyCodeList(txtComListRm.Text);
            searchResultCriteria.CompanyCode = companyList;
            searchResultCriteria.IsIncludeGeneratedLetter = ctlChkGeneratedLetterRm.Checked;
            searchResultCriteria.LetterNo = ctlLetterIdRm.Text;
            return searchResultCriteria;
        }

        #endregion ctlReqGrid Event

        #region General Method

        private void RegisterScriptForGridView()
        {
            StringBuilder script = new StringBuilder();
            script.Append("function validateCheckBox(objChk, objFlag) ");
            script.Append("{ ");
            script.Append("CheckboxesCheckUncheck(objChk, objFlag, ");
            script.Append("'" + ctlGridRm.ClientID + "', '" + ctlGridRm.HeaderRow.FindControl("ctlHeaderRm").ClientID + "'); ");
            script.Append("} ");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "validateChkBox", script.ToString(), true);
        }

        #region GenerateReport()
        private void GenerateReport(string letterNo)
        {
            List<ReportParameter> rptParam = new List<ReportParameter>();

            if (!string.IsNullOrEmpty(letterNo))
            {
                ReportParameter paramLetterNo = new ReportParameter();
                paramLetterNo.ParamterName = "LetterNo";
                paramLetterNo.ParamterValue = letterNo;
                rptParam.Add(paramLetterNo);
                byte[] results = new FilesGenerator().GetByte(ParameterServices.ReportingURL,
                    ParameterServices.ReportUserName,
                    ParameterServices.ReportPassword,
                    ParameterServices.ReportDomainName,
                    ParameterServices.ReportFolderPath,
                    "MoneySellingRequestLetter",
                    rptParam,
                    FilesGenerator.ExportType.PDF);
                ReportHelper.FlushReportPDF(this.Page, results, "MoneySellingRequestLetter");
            }
        }

        #endregion GenerateReport()

        private string GetCompanyCodeList(string companyList)
        {
            string comToGenReport = string.Empty;
            if (companyList.Length > 4)
            {
                string[] companyCode = companyList.Split(',');

                foreach (string s in companyCode)
                {
                    comToGenReport = comToGenReport + "'" + s + "',";
                }
                comToGenReport = comToGenReport.Substring(0, comToGenReport.Length - 1);
            }
            else
            {
                comToGenReport = "'" + companyList + "'";
            }
            return comToGenReport;
        }

        private void ShowGenerateButton()
        {
            ctlDate.Value = DateTime.Now;
            divGenerateButton.Style["display"] = "block";
        }

        #endregion General Method
    }
}
