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
    public partial class MoneyBuyingRequestLetterGenerator : BasePage
    {
        public IDbBuyingRunningService DbBuyingRunningService { get; set; }
        public IDbBuyingLetterDetailService DbBuyingLetterDetailService { get; set; }
        public IDbBuyingLetterService DbBuyingLetterService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {

                if (Request.Cookies["BuyingRequestCookie_" + UserAccount.UserID] != null)
                {
                    txtCompanyList.Text = Server.HtmlEncode(Request.Cookies["BuyingRequestCookie_" + UserAccount.UserID]["CompanyCode"]);
                    ctlCalendar.DateValue = Server.HtmlEncode(Request.Cookies["BuyingRequestCookie_" + UserAccount.UserID]["RequestDateOfAdvance"]);
                    if (Server.HtmlEncode(Request.Cookies["BuyingRequestCookie_" + UserAccount.UserID]["IsIncludeGeneratedLetter"]) == "true")
                    {
                        ctlChkGeneratedLetter.Checked = true;
                    }
                    else
                    {
                        ctlChkGeneratedLetter.Checked = false;
                    }
                    ctlLetterId.Text = Server.HtmlEncode(Request.Cookies["BuyingRequestCookie_" + UserAccount.UserID]["LetterNo"]);
                }
                if (!string.IsNullOrEmpty(Request.QueryString["LetterNo"]))
                {
                    GenerateReport(Request.QueryString["LetterNo"]);
                }
            }

        }

        #region Button Click Event
        protected void btnAddCompany_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ctlCompanyTextboxAutoComplete.CompanyCode))
            {
                if (string.IsNullOrEmpty(txtCompanyList.Text))
                {
                    txtCompanyList.Text += ctlCompanyTextboxAutoComplete.CompanyCode;
                }
                else
                {
                    txtCompanyList.Text += "," + ctlCompanyTextboxAutoComplete.CompanyCode;
                }
            }
            ctlCompanyTextboxAutoComplete.ResetValue();
            ctlUpdatePanelRequestCriteria.Update();
        }

        protected void ctlSearchBtn_Click(object sender, EventArgs e)
        {
            Request.Cookies.Remove("BuyingRequestCookie_" + UserAccount.UserID);
            if (Request.Cookies["BuyingRequestCookie_" + UserAccount.UserID] == null)
            {
                HttpCookie moneyRequestCookie = new HttpCookie("BuyingRequestCookie_" + UserAccount.UserID);
                moneyRequestCookie.Values["RequestDateOfAdvance"] = ctlCalendar.DateValue;
                moneyRequestCookie.Values["CompanyCode"] = txtCompanyList.Text;
                moneyRequestCookie.Values["IsIncludeGeneratedLetter"] = ctlChkGeneratedLetter.Checked.ToString();
                moneyRequestCookie.Values["LetterNo"] = ctlLetterId.Text;
                moneyRequestCookie.Expires = DateTime.Now.AddDays(62);
                Response.Cookies.Add(moneyRequestCookie);
            }


            if (ctlCalendar.Value.HasValue == true && !string.IsNullOrEmpty(txtCompanyList.Text))
            {
                ctlReqGrid.DataCountAndBind();
                ShowGenerateButton();
                ctlUpdatePanelRequestCriteria.Update();
            }
            else
            {
                if (string.IsNullOrEmpty(txtCompanyList.Text))
                {
                    ValidationErrors.AddError("Export.Error", new ErrorMessage("Company is required."));
                }
                if (ctlCalendar.Value.HasValue == false)
                {
                    ValidationErrors.AddError("Export.Error", new ErrorMessage("Request date is required."));
                }
            }
        }

        protected void ctlGenerateBtn_Click(object sender, EventArgs e)
        {
            List<MoneyRequestSearchResult> allList = selectToGenerateList();
            allList.Sort((x, y) => string.Compare(x.DocumentNo, y.DocumentNo));
            //List<MoneyRequestSearchResult> toGenerateList = new List<MoneyRequestSearchResult>();
            string letterNoList = string.Empty;

            if (allList.Count() > 0)
            {
                try
                {
                    lock (letterNoList)
                    {
                        letterNoList = DbBuyingLetterService.GenerateBuyingLetter(allList);
                        ctlReqGrid.DataCountAndBind();
                        ctlUpdatePanelRequestCriteria.Update();
                        //letterNoList = letterNoList.Substring(0, letterNoList.Length - 1);
                        string url = "MoneyBuyingRequestLetterGenerator.aspx?LetterNo=" + letterNoList;
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "Testscript", "document.location = '" + url + "'", true);
                    }
                }
                catch (ServiceValidationException ex)
                {
                    ValidationErrors.MergeErrors(ex.ValidationErrors);
                    //ctlReqGrid.DataCountAndBind();
                    ctlUpdatePanelRequestCriteria.Update();
                }
                catch (Exception ex)
                {
                    ValidationErrors.AddError("Generate.Error", new ErrorMessage(ex.Message));
                }
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
            if (ctlReqGrid.Rows.Count > 0)
            {
                RegisterScriptForGridView();
            }

        }

        protected void ctlReqGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ShowLetter")
            {
                int rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                string letterNo = ctlReqGrid.DataKeys[rowIndex].Value.ToString();                                               // This is error case
                string url = "MoneyBuyingRequestLetterGenerator.aspx?LetterNo=" + letterNo;
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "Testscript", "document.location = '" + url + "'", true);

            }
        }

        protected void ctlReqGrid_RowCreated(object sender, GridViewRowEventArgs e)
        {
        }
        protected void ctlReqGrid_PageIndexChanged(object sender, EventArgs e)
        {

        }
        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            MoneyRequestSearchResult searchResultCriteria = GetSearchResultCriteria();

            return ScgDbQueryProvider.DbMoneyRequestQuery.GetMoneyRequestList(searchResultCriteria, startRow, pageSize, sortExpression);
        }

        protected void ctlReqGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            LinkButton lb = (LinkButton)e.Row.FindControl("ctlLetterID");

            if (lb != null)
            {
                if (!lb.Text.Equals(string.Empty))
                {

                    CheckBox chk = (CheckBox)e.Row.FindControl("ctlSelect");
                    chk.Enabled = false;
                }
            }

        }
        protected void ctlReqGrid_RowCommand(object sender, GridViewRowEventArgs e)
        {


        }

        public int RequestCount()
        {
            MoneyRequestSearchResult searchResultCriteria = GetSearchResultCriteria();
            int count = ScgDbQueryProvider.DbMoneyRequestQuery.CountMoneyRequestByCriteria(searchResultCriteria);

            return count;
        }
        public MoneyRequestSearchResult GetSearchResultCriteria()
        {
            MoneyRequestSearchResult searchResultCriteria = new MoneyRequestSearchResult();
            searchResultCriteria.RequestDateOfAdvance = (DateTime)ctlCalendar.Value;
            string companyList = GetCompanyCodeList(txtCompanyList.Text);
            searchResultCriteria.CompanyCode = companyList;
            searchResultCriteria.LetterNo = ctlLetterId.Text;
            searchResultCriteria.IsIncludeGeneratedLetter = ctlChkGeneratedLetter.Checked;
            return searchResultCriteria;
        }
        #endregion ctlReqGrid Event4

        #region General Method
        private void RegisterScriptForGridView()
        {
            StringBuilder script = new StringBuilder();
            script.Append("function validateCheckBox(objChk, objFlag) ");
            script.Append("{ ");
            script.Append("CheckboxesCheckUncheck(objChk, objFlag, ");
            script.Append("'" + ctlReqGrid.ClientID + "', '" + ctlReqGrid.HeaderRow.FindControl("ctlHeader").ClientID + "'); ");
            script.Append("} ");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "validateChkBox", script.ToString(), true);
        }

        #region GenerateReport()


        public void GenerateReport(string letterNoList)
        {
            List<ReportParameter> rptParam = new List<ReportParameter>();
            ReportParameter paramLetterNo = new ReportParameter();
            paramLetterNo.ParamterName = "LetterNo";
            paramLetterNo.ParamterValue = letterNoList;
            rptParam.Add(paramLetterNo);

            byte[] results = new FilesGenerator().GetByte(ParameterServices.ReportingURL, ParameterServices.ReportUserName, ParameterServices.ReportPassword, ParameterServices.ReportDomainName, ParameterServices.ReportFolderPath, "MoneyBuyingRequestLetter", rptParam, FilesGenerator.ExportType.PDF);
            ReportHelper.FlushReportPDF(this.Page, results, "MoneyBuyingRequestLetter");
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

        private List<MoneyRequestSearchResult> selectToGenerateList()
        {
            List<MoneyRequestSearchResult> toGenerateList = new List<MoneyRequestSearchResult>();

            for (int i = 0; i < ctlReqGrid.Rows.Count; i++)
            {
                CheckBox chkSelect = (CheckBox)ctlReqGrid.Rows[i].Cells[0].FindControl("ctlSelect");
                if (chkSelect != null)
                {
                    if (chkSelect.Checked)
                    {
                        MoneyRequestSearchResult moneyRequest = new MoneyRequestSearchResult();
                        Label DocumentNo = (Label)ctlReqGrid.Rows[i].Cells[1].FindControl("ctlRequestNo");
                        moneyRequest.DocumentNo = DocumentNo.Text;
                        moneyRequest.CompanyCode = DocumentNo.Text.Substring(4, 4);
                        DateTime tmpYear = DateTime.Today;
                        moneyRequest.Year = tmpYear.Year;
                        Label DocumentID = (Label)ctlReqGrid.Rows[i].Cells[5].FindControl("ctlDocumentID");
                        moneyRequest.DocumentID = Convert.ToInt64(DocumentID.Text);
                        moneyRequest.RequestDateOfAdvance = ctlCalendar.Value;
                        toGenerateList.Add(moneyRequest);
                    }
                }
            }

            return toGenerateList;
        }

        private void ShowGenerateButton()
        {
            divGenerateButton.Style["display"] = "block";
        }
        #endregion General Method
    }
}